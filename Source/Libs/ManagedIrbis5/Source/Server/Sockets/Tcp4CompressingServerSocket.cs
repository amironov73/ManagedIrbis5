// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertToUsingDeclaration
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global
// ReSharper disable UseAwaitUsing

/* Tcp4ServerSocket.cs -- серверный сокет для TCP v4, умеющий сжимать трафик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Серверный сокет для TCP v4, умеющий сжимать трафик.
    /// </summary>
    public sealed class Tcp4CompressingServerSocket
        : Tcp4ServerSocket
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Tcp4CompressingServerSocket
            (
                TcpClient client,
                CancellationToken cancellationToken
            )
            : base (client, cancellationToken)
        {
        } // constructor

        #endregion

        #region IAsyncServerSocket members

        /// <inheritdoc cref="IAsyncServerSocket.ReceiveAllAsync"/>
        public override async Task<MemoryStream?> ReceiveAllAsync()
        {
            var compressed = await base.ReceiveAllAsync();
            if (compressed is null)
            {
                return null;
            }

            var result = new MemoryStream();
            using (var decompresser = new DeflateStream (compressed, CompressionMode.Decompress))
            {
                StreamUtility.AppendTo (decompresser, result);
            }

            return result;

        } // method ReadAllAsync

        /// <inheritdoc cref="IAsyncServerSocket.SendAsync"/>
        public override async Task<bool> SendAsync
            (
                IEnumerable<ReadOnlyMemory<byte>> data
            )
        {
            var memory = new MemoryStream();
            using (var compressor = new DeflateStream (memory, CompressionMode.Compress))
            {
                foreach (var chunk in data)
                {
                    compressor.Write (chunk.Span);
                }
            }

            var outgoing = new ReadOnlyMemory<byte>[] { memory.ToArray() };

            return await base.SendAsync (outgoing);

        } // method SendAsync

        #endregion

    } // class Tcp4CompressingServerSocket

} // namespace ManagedIrbis.Server.Sockets
