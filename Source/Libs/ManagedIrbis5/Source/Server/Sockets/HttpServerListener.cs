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

/* HttpServerListener.cs -- серверный слушатель для HTTP
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Серверный слушатель для HTTP.
    /// </summary>
    public sealed class HttpServerListener
        : Tcp4ServerListener
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public HttpServerListener
            (
                IPEndPoint endPoint,
                CancellationToken token
            )
            : base (endPoint, token)
        {
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Создание слушателя для указанного порта.
        /// </summary>
        public new static HttpServerListener ForPort
            (
                int portNumber,
                CancellationToken token
            )
        {
            var endPoint = new IPEndPoint (IPAddress.Any, portNumber);
            var result = new HttpServerListener (endPoint, token);

            return result;

        } // method ForPort

        #endregion

        #region IrbisServerListener methods

        /// <inheritdoc cref="IAsyncServerListener.AcceptClientAsync"/>
        public override async Task<IAsyncServerSocket?> AcceptClientAsync()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var client = await _listener.AcceptTcpClientAsync().ConfigureAwait (false);
            var result = new HttpServerSocket (client, _cancellationToken);

            return result;

        } // method AcceptClientAsync

        #endregion

    } // class HttpServerListener

} // namespace ManagedIrbis.Server.Sockets
