// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SyncHttpSocket.cs -- синхронный клиентский сокет поверх HTTP
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net.Sockets;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets
{
    //
    // Sample Over-HTTP request
    //
    // POST /cgi-bin/irbis64r_01/WebToIrbisServer.exe HTTP/1.1
    // User-Agent: GPNTB/Irbis64
    // Host: 127.0.0.1:6666
    // Accept: *.*
    // Content-length: 72
    //                           // empty line
    // A
    // C
    // A
    // 904625
    // 1
    // password
    // username
    //                           // empty line
    //                           // empty line
    //                           // empty line
    // username
    // password
    // IRBIS_END_REQUEST
    //

    //
    // Sample ordinary request
    //
    // 53
    // A
    // C
    // A
    // 322813
    // 1
    // password
    // username
    //                           // empty line
    //                           // empty line
    //                           // empty line
    // username
    // password
    //

    //
    // Иван Батрак
    //
    // Работа через http. У ирбисоводов есть программа webtoirbisserver,
    // которая являет собой такой прокси.
    // Раньше она включалась в дистрибутив, но в 2018 версии вроде бы ее нет.
    // Ее функциональность также включена в веб ирбис,
    // но Сбойчаков не был бы собой если бы это протестировал.
    // В веб ирбисе это есть но не работает, получается если веб ирбис
    // увидит эти параметры в ini он начнет пытаться проксировать запросы
    // и перестанет работать как веб ирбис и проксировать тоже будет криво.
    //
    // Настройки такие
    //
    // [IrbisServer]
    // Redirect=1
    // IP=127.0.0.1
    // PORT=6666
    //
    // Тут тоже все работает предельно просто.
    // Арму нужно добавить вот это в секции Main
    //
    // WebServer=1
    // WebCgi=/webtoirbisserver.php
    //
    // Я вот как раз набросал скрипт - прокси на php, чтобы ознакомиться
    // с темой.
    // Только не выкладывайте никуда, я не очень уверен, что это подойдет
    // как промышленное решение.
    // Так мельком посмотрел, что работает, глубоко не тестил.
    // Отдельно стоит упомянуть строчки в конце.
    // Там нужно помещать заголовок с длиной данных, иначе апач начинает
    // слать chunked ответ.
    // То есть кусками.
    // Длина, потом блок данных.
    // АРМы ирбис это не понимают, они там просто обрезают
    // http заголовки и дальше работают с пакетом как обычно.
    //

    /// <summary>
    /// Синхронный клиентский сокет поверх HTTP (web-cgi).
    /// </summary>
    public sealed class SyncHttpSocket
        : ISyncClientSocket
    {
        #region Constants

        /// <inheritdoc cref="ISyncClientSocket.RetryCount"/>
        public int RetryCount { get; set; }

        /// <inheritdoc cref="ISyncClientSocket.RetryDelay"/>
        public int RetryDelay { get; set; }

        /// <summary>
        /// Маркер начала запроса.
        /// </summary>
        public const string IrbisStartRequest = "IRBIS_START_REQUEST";

        /// <summary>
        /// Маркер конца запроса.
        /// </summary>
        public const string IrbisEndRequest = "IRBIS_END_REQUEST";

        #endregion

        #region Properties

        /// <summary>
        /// Подключение к ИРБИС-серверу, которое обслуживает данный сокет.
        /// </summary>
        public ISyncConnection? Connection { get; set; }

        #endregion

        #region ISyncClientSocket members

        /// <inheritdoc cref="ISyncClientSocket.TransactSync"/>
        public unsafe Response? TransactSync
            (
                SyncQuery query
            )
        {
            var connection = Connection.ThrowIfNull(nameof(Connection));
            connection.ThrowIfCancelled();

            using var client = new TcpClient(AddressFamily.InterNetwork);
            try
            {
                var host = connection.Host.ThrowIfNull(nameof(connection.Host));
                client.Connect(host, connection.Port);
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(SyncHttpSocket), exception);
                connection.SetLastError(-100_002);

                return default;
            }

            connection.ThrowIfCancelled();

            // TODO: implement

            var socket = client.Client;
            var length = query.GetLength();
            Span<byte> prefix = stackalloc byte[12];
            length = FastNumber.Int32ToBytes(length, prefix);
            prefix[length] = 10; // перевод строки
            prefix = prefix.Slice(0, length + 1);
            var body = query.GetBody();

            try
            {
                socket.Send(prefix, SocketFlags.None);
                socket.Send(body.Span, SocketFlags.None);
                socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(SyncHttpSocket), exception);
                connection.SetLastError(-100_002);

                return default;
            }

            var result = new Response(Connection.ThrowIfNull(nameof(Connection)));
            try
            {
                while (true)
                {
                    connection.ThrowIfCancelled();

                    var buffer = new byte[2048];
                    var chunk = new ArraySegment<byte>(buffer, 0, buffer.Length);
                    var read = socket.Receive(chunk, SocketFlags.None);
                    if (read <= 0)
                    {
                        break;
                    }

                    chunk = new ArraySegment<byte>(buffer, 0, read);
                    result.Add(chunk);
                }
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(SyncHttpSocket), exception);
                connection.SetLastError(-100_002);

                return default;
            }

            return result;

        } // method TransactSync

        #endregion

    } // class SyncHttpSocket

} // namespace ManagedIrbis.Infrastructure.Sockets
