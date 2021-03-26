// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ISyncClientSocket.cs -- интерфейс синхронного клиентского сокета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Интерфейс синхронного клиентского сокета.
    /// </summary>
    public interface ISyncClientSocket
    {
        /// <summary>
        /// Используемое подключение (для нотификаций).
        /// </summary>
        ISyncConnection? Connection { get; set; }

        /// <summary>
        /// Собственно общение с сервером.
        /// </summary>
        public abstract Response? TransactSync
            (
                ref ValueQuery query
            );

    } // interface ISyncClientSocket

} // namespace ManagedIrbis.Infrastructure.Sockets
