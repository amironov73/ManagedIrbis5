// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IAsyncClientSocket.cs -- интерфейс асинхронного клиентского сокета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets
{
    /// <summary>
    /// Интерфейс асинхронного клиентского сокета.
    /// </summary>
    public interface IAsyncClientSocket
    {
        /// <summary>
        /// Количество повторов при сетевом сбое.
        /// </summary>
        int RetryCount { get; set; }

        /// <summary>
        /// Задержка при повторе, миллисекунды.
        /// </summary>
        int RetryDelay { get; set; }

        /// <summary>
        /// Подключение.
        /// </summary>
        IAsyncConnection? Connection { get; set; }

        /// <summary>
        /// Собственно общение с сервером -- в асинхронном режиме.
        /// </summary>
        Task<Response?> TransactAsync
            (
                AsyncQuery asyncQuery
            );

    } // interface IAsyncClientSocket

} // namespace ManagedIrbis.Infrastructure.Sockets
