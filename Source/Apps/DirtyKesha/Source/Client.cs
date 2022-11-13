// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Client.cs -- клиентская функциональность
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;

using ManagedIrbis;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#endregion

#nullable enable

namespace DirtyKesha;

/// <summary>
/// Клиентская функциональность.
/// </summary>
internal sealed class Client
{
    private static readonly string[] _goodTags = { "b", "/b", "i", "/i" };

    private static string _Evaluator
        (
            Match match
        )
    {
        var tag = match.Groups[1].Value.ToLowerInvariant();
        return Array.IndexOf (_goodTags, tag) < 0
            ? string.Empty
            : match.Value;
    }

    private static string _CleanText
        (
            string text
        )
    {
        var regex1 = new Regex ("<([A-Za-z/]+).*?>");
        var regex2 = new Regex ("<[Aa][^>]*?>[^<]*?</[Aa]>");

        var result = text.Replace ("<br>", "\n")
            .Replace ("<br/>", "\n")
            .Replace ("<p>", "\n")
            .Replace ("</div>", "\n");
        result = regex2.Replace (result, string.Empty);
        result = regex1.Replace (result, _Evaluator);
        result = result.Replace ("()", string.Empty);

        return result;
    }

    private static async Task<string[]> SearchForBooks
        (
            string query
        )
    {
        await using var connection = ConnectionFactory.Shared.CreateAsyncConnection();
        var connectionString = Program.Configuration["connection"].ThrowIfNullOrEmpty();
        connection.ParseConnectionString (connectionString);
        await connection.ConnectAsync();
        if (!connection.IsConnected)
        {
            return Array.Empty<string>();
        }

        var expression = $"\"K={query}$\" + \"T={query}$\" + \"A={query}$\"";
        var parameters = new SearchParameters
        {
            Database = connection.Database,
            Expression = expression,
            Format = "@",
            NumberOfRecords = 20
        };
        var found = await connection.SearchAsync (parameters);
        if (found is null || found.Length == 0)
        {
            return Array.Empty<string>();
        }

        return found
            .Select (item => item.Text)
            .Where (line => !string.IsNullOrEmpty (line))
            .Select (line => line.ThrowIfNull())
            .ToArray();
    }

    private static async Task<string[]> GetAndCleanDescriptions
        (
            string query
        )
    {
        var found = await SearchForBooks (query);

        return found
            .Select (_CleanText)
            .Where (line => !string.IsNullOrEmpty (line))
            .OrderBy (line => line)
            .ToArray();
    }

    private static Task HandlePollingErrorAsync
        (
            ITelegramBotClient client,
            Exception exception,
            CancellationToken cancellationToken
        )
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

            _ => exception.ToString()
        };

        Program.Logger.LogError (exception, "Polling error: {Message}", errorMessage);

        return Task.CompletedTask;
    }

    private static async Task HandleUpdateAsync
        (
            ITelegramBotClient client,
            Update update,
            CancellationToken token
        )
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
        {
            return;
        }

        // Only process text messages
        if (message.Text is not { } messageText)
        {
            return;
        }

        var chatId = message.Chat.Id;

        Program.Logger.LogInformation
            (
                "Received message {Message} in chat {Chat}",
                messageText,
                chatId
            );

        if (messageText == "/start")
        {
            await client.SendTextMessageAsync
                (
                    chatId,
                    "Введите ключевое слово, заглавие книги или фамилию автора",
                    cancellationToken: token
                );
            return;
        }

        await client.SendChatActionAsync
            (
                message.Chat.Id,
                ChatAction.Typing,
                token
            );

        var found = await GetAndCleanDescriptions (messageText);

        Program.Logger.LogInformation ("Found: {Length}", found.Length);

        if (found.IsNullOrEmpty())
        {
            await client.SendTextMessageAsync
                (
                    chatId,
                    "К сожалению, ничего не найдено",
                    cancellationToken: token
                );
            return;
        }

        foreach (var book in found)
        {
            await client.SendTextMessageAsync
                (
                    chatId,
                    book,
                    ParseMode.Html,
                    cancellationToken: token
                );
        }
    }

    /// <summary>
    /// Создание клиента согласно настройкам.
    /// </summary>
    private static TelegramBotClient CreateClient()
    {
        var proxyAddress = Program.Configuration["http-proxy"];
        var botToken = Program.Configuration["token"].ThrowIfNullOrEmpty();
        TelegramBotClient result;

        if (!string.IsNullOrEmpty (proxyAddress))
        {
            var proxyPort = Program.Configuration["proxy-port"].SafeToInt32 (8080);
            var webProxy = new WebProxy (proxyAddress, proxyPort);
            var httpClient = new HttpClient
                (
                    new HttpClientHandler { Proxy = webProxy, UseProxy = true }
                );
            result = new TelegramBotClient (botToken, httpClient);
        }
        else
        {
            result = new TelegramBotClient (botToken);
        }

        return result;
    }

    /// <summary>
    /// Запуск клиента.
    /// </summary>
    public static async void RunBot
        (
            CancellationToken cancellationToken
        )
    {
        var botClient = CreateClient();

        // StartReceiving does not block the caller thread.
        // Receiving is done on the ThreadPool.
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };
        botClient.StartReceiving
            (
                HandleUpdateAsync,
                HandlePollingErrorAsync,
                receiverOptions,
                cancellationToken
            );

        var me = await botClient.GetMeAsync (cancellationToken);

        Program.Logger.LogInformation ("Start listening for @{UserName}", me.Username);
    }
}
