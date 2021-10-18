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

/* Tcp4ServerListener.cs -- серверный слушатель для простого TCP v4
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Серверный слушатель для простого TCP v4.
    /// </summary>
    public sealed class Tcp4ServerListener
        : IAsyncServerListener
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Tcp4ServerListener
            (
                IPEndPoint endPoint,
                CancellationToken cancellationToken
            )
        {
            _endPoint = endPoint;
            _listener = new TcpListener (endPoint);
            _cancellationToken = cancellationToken;
            _cancellationToken.Register (_StopListener);

        } // constructor

        #endregion

        #region Private members

        private readonly IPEndPoint _endPoint;
        private readonly TcpListener _listener;
        private readonly CancellationToken _cancellationToken;
        private bool _working;

        private void _StopListener()
        {
            _listener.Stop();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Create listener for the given port.
        /// </summary>
        public static Tcp4ServerListener ForPort
            (
                int portNumber,
                CancellationToken token
            )
        {
            Sure.InRange (portNumber, 1, 65535);

            var endPoint = new IPEndPoint (IPAddress.Any, portNumber);
            var result = new Tcp4ServerListener (endPoint, token);

            return result;

        } // method ForPort

        #endregion

        #region IAsyncServerListener members

        /// <inheritdoc cref="IAsyncServerListener.AcceptClientAsync"/>
        public async Task<IAsyncServerSocket?> AcceptClientAsync()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var client = await _listener.AcceptTcpClientAsync().ConfigureAwait (false);
            var result = new Tcp4ServerSocket (client, _cancellationToken);

            return result;

        } // method AcceptClientAsync

        /// <inheritdoc cref="IAsyncServerListener.GetLocalAddress"/>
        public string GetLocalAddress() => _endPoint.ToString();

        /// <inheritdoc cref="IAsyncServerListener.StartAsync"/>
        public Task StartAsync()
        {
            if (!_working)
            {
                _listener.Start();
                _working = true;
            }

            return Task.CompletedTask;

        } // method StartAsync

        /// <inheritdoc cref="IAsyncServerListener.StopAsync"/>
        public Task StopAsync()
        {
            if (_working)
            {
                _listener.Stop();
                _working = false;
            }

            return Task.CompletedTask;

        } // method StopAsync

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public async ValueTask DisposeAsync() => await StopAsync();

        #endregion

    } // class Tcp4ServerListener

} // namespace ManagedIrbis.Server.Sockets
