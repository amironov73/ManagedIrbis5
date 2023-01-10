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
using AM.Net;

using Istu.OldModel;

using ManagedIrbis;
using ManagedIrbis.Formatting;
using ManagedIrbis.Providers;
using ManagedIrbis.Searching;

using Microsoft.Extensions.Logging;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

        var serviceProvider = Magna.Host.Services;
        var teapot = new AsyncTeapotSearcher (serviceProvider);
        var found = await teapot.SearchFormatAsync (connection, query);
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<string>();
        }

        return found
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
            .Take (20)
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

    /// <summary>
    /// Подключение к серверу ИРБИС.
    /// </summary>
    private static async Task<IAsyncProvider?> GetIrbis()
    {
        var connection = ConnectionFactory.Shared.CreateAsyncConnection();
        var connectionString = Program.Configuration["connection"].ThrowIfNullOrEmpty();
        connectionString = IrbisUtility.Decrypt (connectionString);
        connection.ParseConnectionString (connectionString);
        await connection.ConnectAsync();

        return !connection.IsConnected ? null : connection;
    }

    /// <summary>
    /// Регистрация заказа на книгу.
    /// </summary>
    private static async Task RegisterBookOrder
        (
            ITelegramBotClient client,
            ChatId chatId,
            User? user,
            string encodedIndex,
            CancellationToken token
        )
    {
        if (user is null || user.IsBot)
        {
            return;
        }

        // раскодируем шифр книги
        var decodedIndex = KeshaUtility.DecodeTheOrder (encodedIndex);
        if (string.IsNullOrEmpty (decodedIndex))
        {
            return;
        }

        var storehouse = Storehouse.GetInstance
            (
                Program.ApplicationHost.Services,
                Program.Configuration
            );
        using var readerManager = storehouse.CreateReaderManager();

        // 1. Находим читателя по его Telegram-идентификатору
        var reader = readerManager.GetReaderByTelegramId (user.Id);
        if (reader is null)
        {
            await client.SendTextMessageAsync
                (
                    chatId,
                    "Читатель не зарегистрирован",
                    cancellationToken: token
                );
            return;
        }

        // 2. Находим книгу
        await using var irbis = await GetIrbis();
        if (irbis is null)
        {
            await client.SendTextMessageAsync
                (
                    chatId,
                    "Ошибка при регистрации заказа",
                    cancellationToken: token
                );
            return;
        }

        var book = await irbis.SearchReadOneRecordAsync ($"\"I={decodedIndex}\"");
        if (book is null)
        {
            await client.SendTextMessageAsync
                (
                    chatId,
                    "Не найдена книга",
                    cancellationToken: token
                );
            return;
        }

        // 3. Размещаем заказ
        using var orderManager = storehouse.CreateOrderManager();
        var format = new AsyncHardFormat
            (
                Program.ApplicationHost,
                irbis
            );
        var newOrder = new Order
        {
            Ticket = reader.Ticket,
            Name = reader.Name,
            Moment = DateTime.Now,
            Status = Order.NewOrder,
            Mfn = book.Mfn.ToInvariantString(),
            Description = format.Brief (book),
            Mailto = reader.Mail
        };
        orderManager.CreateOrder (newOrder);

        await client.SendTextMessageAsync
            (
                chatId,
                "Заказ успешно размещен",
                cancellationToken: token
            );
    }

    /// <summary>
    /// Связывание читателя с Telegram-идентификатором через email.
    /// </summary>
    private static async Task StoreTelegramIdForEmail
        (
            ITelegramBotClient client,
            long chatId,
            User? user,
            string email,
            CancellationToken token
        )
    {
        if (user is null || user.IsBot)
        {
            return;
        }

        var storehouse = Storehouse.GetInstance
            (
                Program.ApplicationHost.Services,
                Program.Configuration
            );
        using var readerManager = storehouse.CreateReaderManager();

        // 1. Находим читателя в базе по e-mail
        var reader = readerManager.GetReaderByEmail (email);
        if (reader is null)
        {
            await client.SendTextMessageAsync
                (
                    chatId,
                    "В базе нет читателя с таким e-mail",
                    cancellationToken: token
                );
            return;
        }

        // 2. Сохраняем TelegramID
        reader.TelegramId = user.Id;
        readerManager.UpdateReaderInfo (reader);
        await client.SendTextMessageAsync
            (
                chatId,
                "Ваш аккаунт авторизован, теперь Вы можете заказывать книги",
                cancellationToken: token
            );
    }

    private static async Task HandleCallbackQueryAsync
        (
            ITelegramBotClient client,
            Update update,
            CallbackQuery callbackQuery,
            CancellationToken token
        )
    {
        User? user = null;
        ChatId chatId = 0L;
        var bookId = "1234";

        await RegisterBookOrder (client, chatId, user, bookId, token);
    }

    /// <summary>
    /// Ответ на запрос пользователя.
    /// </summary>
    private static async Task HandleUpdateAsync
        (
            ITelegramBotClient client,
            Update update,
            CancellationToken token
        )
    {
        if (update.CallbackQuery is { } callbackQuery)
        {
            await HandleCallbackQueryAsync (client, update, callbackQuery, token);
            return;
        }

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

        if (string.IsNullOrWhiteSpace (messageText))
        {
            // пустые сообщения принципиально не обрабатываем
            return;
        }

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

        if (MailUtility.VerifyEmail (messageText))
        {
            // Читатель прислал свой e-mail, чтобы мы связали его с телеграммом
            await StoreTelegramIdForEmail (client, chatId, message.From, messageText, token);
            return;
        }

        // if (messageText.StartsWith ("//"))
        // {
        //     // заказ на книгу
        //     await RegisterBookOrder (client, chatId, message.From, messageText, token);
        //     return;
        // }

        await client.SendChatActionAsync
            (
                message.Chat.Id,
                ChatAction.Typing,
                null,
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
            var keyboard = new InlineKeyboardMarkup
                (
                    InlineKeyboardButton.WithCallbackData ("заказ", "MFN книги")
                );

            var answer = new SendMessageRequest (chatId, book)
            {
                ParseMode = ParseMode.Html,
                // Entities = entities,
                // DisableWebPagePreview = disableWebPagePreview,
                // DisableNotification = disableNotification,
                // ProtectContent = protectContent,
                // ReplyToMessageId = replyToMessageId,
                // AllowSendingWithoutReply = allowSendingWithoutReply,
                ReplyMarkup = keyboard,
                // MessageThreadId = messageThreadId,
            };

            await client.MakeRequestAsync (answer, token);

            // await client.SendTextMessageAsync
            //     (
            //         chatId,
            //         book,
            //         null,
            //         ParseMode.Html,
            //         cancellationToken: token
            //     );
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
