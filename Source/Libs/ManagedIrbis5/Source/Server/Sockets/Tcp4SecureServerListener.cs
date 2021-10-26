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

/* Tcp4SecureServerListener.cs -- серверный слушатель для простого TCP v4
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net;
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
    public sealed class Tcp4SecureServerListener
        : Tcp4ServerListener
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Tcp4SecureServerListener
            (
                IPEndPoint endPoint,
                CancellationToken cancellationToken
            )
            : base (endPoint, cancellationToken)
        {
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Создание слушателя для указанного порта.
        /// </summary>
        public new static Tcp4SecureServerListener ForPort
            (
                int portNumber,
                CancellationToken token
            )
        {
            Sure.InRange (portNumber, 1, 65535);

            var endPoint = new IPEndPoint (IPAddress.Any, portNumber);
            var result = new Tcp4SecureServerListener (endPoint, token);

            return result;

        } // method ForPort

        #endregion

        #region IAsyncServerListener members

        /// <inheritdoc cref="IAsyncServerListener.AcceptClientAsync"/>
        public override async Task<IAsyncServerSocket?> AcceptClientAsync()
        {
            var client = await _listener.AcceptTcpClientAsync().ConfigureAwait (false);
            var result = new Tcp4SecureServerSocket (client, _cancellationToken);

            return result;

        } // method AcceptClientAsync

        #endregion

    } // class Tcp4SecureServerListener

} // namespace ManagedIrbis.Server.Sockets
