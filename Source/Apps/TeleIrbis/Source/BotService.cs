// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* BotService.cs -- сервис, запускающий прослушивание телеграм.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using AM;

using ManagedIrbis;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

using Topshelf;
using Topshelf.Logging;

#endregion

#nullable enable

namespace TeleIrbis
{
    public sealed class BotService
        : ServiceControl, IUpdateHandler
    {
        #region Private members

        private LogWriter? _log;
        // private IWebProxy? _proxy;
        private string? _token;
        private TelegramBotClient? _client;
        private readonly CancellationTokenSource _cancellation = new ();

        #endregion

        #region ServiceControl members

        /// <summary>
        /// Запуск сервиса.
        /// </summary>
        public bool Start
            (
                HostControl hostControl
            )
        {
            _log = HostLogger.Get<BotService>();
            _log.Info(nameof(BotService) + "::" + nameof(Start));

            //var uri = new Uri("http://172.27.100.5:4444");
            //_proxy = new WebProxy(uri);
            _token = "no:such:token";
            //_client = new TelegramBotClient(_token, _proxy);
            _client = new TelegramBotClient(_token);

            var me = _client.GetMeAsync().Result;
            _log.Info($"BOT: {me.Username}");

            var cancellationToken = _cancellation.Token;
            _client.StartReceiving(this, cancellationToken);

            return true;
        }

        private static readonly string[] _goodTags = { "b", "/b", "i", "/i" };

        private static string _Evaluator(Match match)
        {
            string tag = match.Groups[1].Value.ToLowerInvariant();
            if (Array.IndexOf(_goodTags, tag) < 0)
            {
                return string.Empty;
            }

            return match.Value;
        }

        private static string _CleanText(string text)
        {
            Regex regex1 = new Regex("<([A-Za-z/]+).*?>");
            Regex regex2 = new Regex("<[Aa][^>]*?>[^<]*?</[Aa]>");

            string result = text.Replace("<br>", "\n")
                    .Replace("<br/>", "\n")
                    .Replace("<p>", "\n")
                    .Replace("</div>", "\n");
            result = regex2.Replace(result, string.Empty);
            result = regex1.Replace(result, _Evaluator);
            result = result.Replace("()", string.Empty);

            return result;
        }

        private async Task<string[]> GetBooks
            (
                string query
            )
        {
            await using var connection = ConnectionFactory.Shared.CreateAsyncConnection();
            var connectionString = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=ISTU;";
            connection.ParseConnectionString(connectionString);
            await connection.ConnectAsync();
            var expression = $"\"K={query}$\" + \"T={query}$\" + \"A={query}$\"";
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression,
                //Format = "@sbrief_istu",
                Format = "@",
                NumberOfRecords = 20
            };
            var found = await connection.SearchAsync(parameters);
            if (found is null || found.Length == 0)
            {
                return new[] {"Ничего не найдено"};
            }

            return found
                .Select(item => item.Text)
                .Where(line => !string.IsNullOrEmpty(line))
                .Select(line => line.ThrowIfNull())
                .Select(_CleanText)
                .Where(line => !string.IsNullOrEmpty(line))
                .OrderBy(line => line)
                .ToArray();
        }

        public UpdateType[] AllowedUpdates => new [] { UpdateType.Message };

        public Task HandleError
            (
                ITelegramBotClient botClient,
                Exception exception,
                CancellationToken cancellationToken
            )
        {
            _log?.Error(exception);

            return Task.CompletedTask;
        }

        public async Task HandleUpdate
            (
                ITelegramBotClient botClient,
                Update update,
                CancellationToken cancellationToken
            )
        {
            if (update.Message is not { Type: MessageType.Text } message)
            {
                // наш бот не умеет обрабатывать нетекстовые сообщения.
                return;
            }

            var text = message.Text.Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                // пришёл пустой текст, ничего с ним не делаем
                return;
            }

            _log?.InfoFormat("Got command: {0}", text);

            if (text == "/start")
            {
                if (_client is not null)
                {
                    await _client.SendTextMessageAsync
                        (
                            chatId: message.Chat.Id,
                            text: "Введите ключевое слово, заглавие книги или фамилию автора",
                            cancellationToken: cancellationToken
                        );
                }
                return;
            }

            if (_client is not null)
            {
                await _client.SendChatActionAsync
                    (
                        message.Chat.Id,
                        ChatAction.Typing, cancellationToken:
                        cancellationToken
                    );

                var found = await GetBooks(text);

                foreach (var book in found)
                {
                    await _client.SendTextMessageAsync
                        (
                            chatId: message.Chat.Id,
                            text: book,
                            ParseMode.Html,
                            cancellationToken: cancellationToken
                        );
                }
            }
        }

        /// <summary>
        /// Остановка сервиса.
        /// </summary>
        public bool Stop
            (
                HostControl hostControl
            )
        {
            _log?.Info(nameof(BotService) + "::" + nameof(Stop));

            _cancellation.Cancel();

            return true;
        }

        #endregion

    } // class BotService

} // namespace TeleIrbis
