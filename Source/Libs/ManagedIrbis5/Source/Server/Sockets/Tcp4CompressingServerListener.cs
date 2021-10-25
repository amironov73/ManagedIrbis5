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

/* Tcp4CompressingServerListener.cs -- серверный слушатель для TCP v4, умеющий сжимать трафик
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
    /// Серверный слушатель для TCP v4, умеющий сжимать трафик.
    /// </summary>
    public sealed class Tcp4CompressingServerListener
        : Tcp4ServerListener
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Tcp4CompressingServerListener
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
        public new static Tcp4CompressingServerListener ForPort
            (
                int portNumber,
                CancellationToken token
            )
        {
            Sure.InRange (portNumber, 1, 65535);

            var endPoint = new IPEndPoint (IPAddress.Any, portNumber);
            var result = new Tcp4CompressingServerListener (endPoint, token);

            return result;

        } // method ForPort

        #endregion

        #region IAsyncServerListener members

        /// <inheritdoc cref="IAsyncServerListener.AcceptClientAsync"/>
        public override async Task<IAsyncServerSocket?> AcceptClientAsync()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return null;
            }

            var client = await _listener.AcceptTcpClientAsync().ConfigureAwait (false);
            var result = new Tcp4CompressingServerSocket (client, _cancellationToken);

            return result;

        } // method AcceptClientAsync

        #endregion

    } // class Tcp4CompressingServerListener

} // namespace ManagedIrbis.Server.Sockets
