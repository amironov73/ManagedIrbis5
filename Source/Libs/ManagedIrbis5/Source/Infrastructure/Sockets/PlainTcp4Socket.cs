// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PlainTcp4Socket.cs -- клиентский сокет на основе TCP/IP 4
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
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
    /// Клиентский сокет на основе TCP/IP 4.
    /// </summary>
    public sealed class PlainTcp4Socket
        : ClientSocket
    {
        #region ClientSocket methods

        public override async Task<Response?> TransactAsync
            (
                Query query
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
            //var stream = client.GetStream();

            var length = query.GetLength();
            var prefix = Encoding.ASCII.GetBytes
                (
                    length.ToInvariantString() + "\n"
                );
            var chunks = query.GetChunks();
            chunks[0] = prefix;
            var segments = new List<ArraySegment<byte>>(chunks.Length);
            foreach (var chunk in chunks)
            {
                var segment = new ArraySegment<byte>(chunk);
                segments.Add(segment);
            }

            try
            {
                foreach (var chunk in chunks)
                {
                    //connection.Cancellation.ThrowIfCancellationRequested();
                    //await stream.WriteAsync(chunk, connection.Cancellation);
                    await socket.SendAsync(segments, SocketFlags.None);
                    //await socket.SendAsync(chunk, SocketFlags.None);
                }

                // await stream.FlushAsync();
                // socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                connection.LastError = -100_002;
                return null;
            }

            //await stream.FlushAsync();
            socket.Shutdown(SocketShutdown.Send);

            var result = new Response();
            try
            {
                //connection.Cancellation.ThrowIfCancellationRequested();
                // await result.PullDataAsync
                //     (
                //         stream,
                //         2048,
                //         connection.Cancellation
                //     );
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
                    result._memory.Add(chunk);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                connection.LastError = -100_002;
                return null;
            }

            return result;
        }

        #endregion
    }
}
