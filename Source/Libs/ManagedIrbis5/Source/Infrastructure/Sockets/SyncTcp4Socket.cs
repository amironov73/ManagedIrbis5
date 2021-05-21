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

/* SyncTcp4Socket.cs -- клиентский сокет на основе TCP/IP 4
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
    /// <summary>
    /// Сокет, реализующий синхронный режим для TCPv4-подключения.
    /// </summary>
    public sealed class SyncTcp4Socket
        : ISyncClientSocket
    {
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
            var connection = Connection.ThrowIfNull();
            connection.ThrowIfCancelled();

            using var client = new TcpClient();
            try
            {
                var host = connection.Host.ThrowIfNull("connection.Host");
                client.Connect(host, connection.Port);
            }
            catch (Exception exception)
            {
                Magna.TraceException(nameof(SyncTcp4Socket), exception);
                connection.SetLastError(-100_002);

                return default;
            }

            connection.ThrowIfCancelled();

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
                Magna.TraceException(nameof(SyncTcp4Socket), exception);
                connection.SetLastError(-100_002);

                return default;
            }

            var result = new Response();
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
                Magna.TraceException(nameof(SyncTcp4Socket), exception);
                connection.SetLastError(-100_002);

                return default;
            }

            return result;

        } // method TransactSync

        #endregion

    } // class SyncTcp4Socket

} // namespace ManagedIrbis.Infrastructure.Sockets
