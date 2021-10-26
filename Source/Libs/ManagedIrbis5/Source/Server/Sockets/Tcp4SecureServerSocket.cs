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

/* Tcp4SecureServerSocket.cs -- простой серверный сокет для TCP v4
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
// using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

using AM.Security;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Простой серверный (обслуживающий подключенного клиента)
    /// сокет для TCP v4.
    /// Ничего не сжимает, не шифрует, не переиспользуется.
    /// </summary>
    public sealed class Tcp4SecureServerSocket
        : Tcp4ServerSocket
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Tcp4SecureServerSocket
            (
                TcpClient client,
                CancellationToken cancellationToken
            )
            : base (client, cancellationToken)
        {
            var certificate = SecurityUtility.GetSslCertificate();
            _sslStream = new SslStream (client.GetStream(), false);
            _sslStream.AuthenticateAsServer (certificate);

        } // constructor

        #endregion

        #region Private members

        private readonly SslStream _sslStream;

        // private bool _ValidateServerCertificate
        //     (
        //         object sender,
        //         X509Certificate certificate,
        //         X509Chain chain,
        //         SslPolicyErrors sslPolicyErrors
        //     )
        // {
        //     if (sslPolicyErrors == SslPolicyErrors.None)
        //     {
        //         return true;
        //     }
        //
        //     // Console.WriteLine ("Certificate error: {0}", sslPolicyErrors);
        //
        //     return false;
        // }

        #endregion

        #region IAsyncServerSocket members

        /// <inheritdoc cref="IAsyncServerSocket.ReceiveAllAsync"/>
        public override async Task<MemoryStream?> ReceiveAllAsync()
        {
            var result = new MemoryStream();

            var buffer = new byte[50 * 1024];
            while (true)
            {
                var read = await _sslStream.ReadAsync (buffer, 0, buffer.Length);
                if (read <= 0)
                {
                    break;
                }
                result.Write (buffer, 0, read);
            }

            result.Position = 0;

            return result;

        } // method ReadAllAsync

        /// <inheritdoc cref="IAsyncServerSocket.SendAsync"/>
        public override async Task<bool> SendAsync
            (
                IEnumerable<ReadOnlyMemory<byte>> data
            )
        {
            foreach (var bytes in data)
            {
                await _sslStream.WriteAsync (bytes);
            }

            return true;

        } // method SendAsync

        #endregion

    } // class Tcp4SecureServerSocket

} // namespace ManagedIrbis.Server.Sockets
