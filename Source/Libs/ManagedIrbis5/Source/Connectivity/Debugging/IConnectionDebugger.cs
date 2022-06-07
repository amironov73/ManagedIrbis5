// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IConnectionDebugger.cs -- интерфейс отладчика подключений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Debugging;

/// <summary>
/// Интерфейс отладчика подключений.
/// </summary>
public interface IConnectionDebugger
{
    /// <summary>
    /// Отладка при отправке пакета.
    /// Вариант для синхронного сокета.
    /// </summary>
    void DebugOugoingPacket
        (
            ISyncClientSocket socket,
            ReadOnlyMemory<byte> packet
        );

    /// <summary>
    /// Отладка при получении пакета.
    /// Вариант для синхронного сокета.
    /// </summary>
    void DebugIncomingPacket
        (
            ISyncClientSocket socket,
            ReadOnlyMemory<byte> packet
        );

    /// <summary>
    /// Отладка при отправке пакета.
    /// Вариант для асинхронного сокета.
    /// </summary>
    Task DebugOugoingPacketAsync
        (
            IAsyncClientSocket socket,
            ReadOnlyMemory<byte> packet
        );

    /// <summary>
    /// Отладка при получении пакета.
    /// Вариант для асинхронного сокета.
    /// </summary>
    Task DebugIncomingPacketAsync
        (
            IAsyncClientSocket socket,
            ReadOnlyMemory<byte> packet
        );
}
