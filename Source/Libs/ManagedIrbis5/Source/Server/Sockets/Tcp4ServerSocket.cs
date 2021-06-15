// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* Tcp4ServerSocket.cs -- простой серверный сокет для TCP v4
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Простой серверный (обслуживающий подключенного клиента)
    /// сокет для TCP v4.
    /// Ничего не сжимает, не шифрует, не переиспользуется.
    /// </summary>
    public sealed class Tcp4ServerSocket
        : IAsyncServerSocket
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Tcp4ServerSocket
            (
                TcpClient client,
                CancellationToken cancellationToken
            )
        {
            _client = client;
            _cancellationToken = cancellationToken;

        } // constructor

        #endregion

        #region Private members

        private readonly TcpClient _client;
        private readonly CancellationToken _cancellationToken;

        #endregion

        #region IAsyncServerSocket members

        /// <inheritdoc cref="IAsyncServerSocket.GetRemoteAddress"/>
        public string GetRemoteAddress() =>
            _client.Client.RemoteEndPoint?.ToString() ?? "(unknown)";

        /// <inheritdoc cref="IAsyncServerSocket.ReceiveAllAsync"/>
        public async Task<MemoryStream?> ReceiveAllAsync()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var result = new MemoryStream();
            NetworkStream stream = _client.GetStream();
            byte[] buffer = new byte[50 * 1024];

            while (true)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                var read = await stream.ReadAsync
                    (
                        buffer,
                        0,
                        buffer.Length,
                        _cancellationToken
                    )
                    .ConfigureAwait(false);
                if (read <= 0)
                {
                    break;
                }

                if (_cancellationToken.IsCancellationRequested)
                {
                    return null;
                }

                await result.WriteAsync
                    (
                        buffer,
                        0,
                        read,
                        _cancellationToken
                    ).ConfigureAwait(false);
            }

            result.Position = 0;

            return result;

        } // method ReadAllAsync

        /// <inheritdoc cref="IAsyncServerSocket.SendAsync"/>
        public async Task<bool> SendAsync
            (
                IEnumerable<ReadOnlyMemory<byte>> data
            )
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            var stream = _client.GetStream();

            foreach (var unit in data)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return false;
                }

                await stream.WriteAsync(unit, _cancellationToken);
            }

            return true;

        } // method SendAsync

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync()
        {
            _client.Dispose();

            return ValueTask.CompletedTask;

        } // method DisposeAsync

        #endregion

    } // class Tcp4ServerSocket

} // namespace ManagedIrbis.Server.Sockets
