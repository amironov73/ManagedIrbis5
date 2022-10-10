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

/* IAsyncServerSocket.cs -- интерфейс асинхронного серверного сокета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Интерфейс асинхронного серверного сокета.
    /// </summary>
    public interface IAsyncServerSocket
        : IAsyncDisposable
    {

        /// <summary>
        /// Get remote address.
        /// </summary>
        string GetRemoteAddress();

        /// <summary>
        /// Receive all the data.
        /// </summary>
        Task<MemoryStream?> ReceiveAllAsync();

        /// <summary>
        /// Send the data.
        /// </summary>
        Task<bool> SendAsync
            (
                IEnumerable<ReadOnlyMemory<byte>> data
            );

    } // interface IAsyncServerSocket

} // namespace ManagedIrbis.Server.Sockets
