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

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;

using ManagedIrbis;

using Microsoft.Extensions.Configuration;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#endregion

namespace DirtyKesha;

internal class Program
{
    private static readonly string[] _goodTags = { "b", "/b", "i", "/i" };

    private static string _Evaluator
        (
            Match match
        )
    {
        var tag = match.Groups[1].Value.ToLowerInvariant();
        if (Array.IndexOf (_goodTags, tag) < 0)
        {
            return string.Empty;
        }

        return match.Value;
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

    private static async Task<string[]> GetBooks
        (
            string query
        )
    {
        await using var connection = ConnectionFactory.Shared.CreateAsyncConnection();
        var connectionString = Configuration["connection"].ThrowIfNullOrEmpty();
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
            return new[] { "Ничего не найдено" };
        }

        return found
            .Select (item => item.Text)
            .Where (line => !string.IsNullOrEmpty (line))
            .Select (line => line.ThrowIfNull())
            .Select (_CleanText)
            .Where (line => !string.IsNullOrEmpty (line))
            .OrderBy (line => line)
            .ToArray();
    }

    private static IConfiguration Configuration = null!;

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static async Task<int> Main
        (
            string[] args
        )
    {
        Utility.RegisterEncodingProviders();

        Configuration = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (args)
            .Build();

        var botToken = Configuration["token"].ThrowIfNullOrEmpty();
        var botClient = new TelegramBotClient (botToken);
        using var cancellationTokenSource = new CancellationTokenSource();

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
                cancellationTokenSource.Token
            );

        var me = await botClient.GetMeAsync (cancellationTokenSource.Token);

        Console.WriteLine ($"Start listening for @{me.Username}");
        Console.ReadLine();

        // Send cancellation request to stop bot
        cancellationTokenSource.Cancel();

        return 0;
    }

    private static Task HandlePollingErrorAsync
        (
            ITelegramBotClient client,
            Exception exception,
            CancellationToken cancellationToken
        )
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",

            _ => exception.ToString()
        };

        Console.WriteLine (ErrorMessage);

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

        Console.WriteLine ($"Received a '{messageText}' message in chat {chatId}.");

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

        var found = await GetBooks (messageText);
        Console.WriteLine ($"Found: {found.Length}");
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
}
