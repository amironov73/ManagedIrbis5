// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IConnectionDebugger.cs -- интерфейс отладчика подключений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Infrastructure.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Debugging
{
    /// <summary>
    /// Интерфейс отладчика подключений.
    /// </summary>
    public interface IConnectionDebugger
    {
        /// <summary>
        /// Отладка при отправке пакета.
        /// </summary>
        void DebugOugoingPacket
            (
                ISyncClientSocket socket,
                byte[] packet
            );

        /// <summary>
        /// Отладка при получении пакета.
        /// </summary>
        void DebugIncomingPacket
            (
                ISyncClientSocket socket,
                byte[] packet
            );

    } // interface IConnectionDebugger

} // namespace ManagedIrbis.Infrastructure.Debugging
