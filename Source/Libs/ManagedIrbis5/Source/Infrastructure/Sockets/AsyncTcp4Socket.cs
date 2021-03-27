// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsyncTcp4Socket.cs -- клиентский сокет на основе TCP/IP 4
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    ///
    /// </summary>
    public sealed class AsyncTcp4Socket
        : IAsyncClientSocket
    {
        #region IAsyncClientSocket members

        public AsyncConnection? Connection { get; set; }

        /// <inheritdoc cref="IAsyncClientSocket.TransactAsync"/>
        public async Task<Response?> TransactAsync
            (
                AsyncQuery asyncQuery
            )
        {
            var connection = Connection.ThrowIfNull();
            connection.Cancellation.ThrowIfCancellationRequested();

            using var client = new TcpClient();
            try
            {
                await client.ConnectAsync(connection.Host, connection.Port);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                connection.LastError = -100_002;
                return null;
            }

            var socket = client.Client;
            var length = asyncQuery.GetLength();
            var prefix = Encoding.ASCII.GetBytes
                (
                    length.ToInvariantString() + "\n"
                );
            var body = asyncQuery.GetBody();

            try
            {
                await socket.SendAsync(prefix, SocketFlags.None);
                await socket.SendAsync(body, SocketFlags.None);
                socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                connection.LastError = -100_002;
                return null;
            }

            var result = new Response();
            try
            {
                while (true)
                {
                    var buffer = new byte[2048];
                    var chunk = new ArraySegment<byte>(buffer, 0, buffer.Length);
                    var read = await socket.ReceiveAsync(chunk, SocketFlags.None);
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
                Debug.WriteLine(exception.Message);
                connection.LastError = -100_002;
                return null;
            }

            return result;
        } // method TransactAsync

        #endregion
    }
}
