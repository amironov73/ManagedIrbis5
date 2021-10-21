// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IAsyncRecordSink.cs -- интерфейс асинхронного приемника записей для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using ManagedIrbis.Gbl;

#endregion

#nullable enable

namespace ManagedIrbis.Processing
{
    /// <summary>
    /// Интерфейс асинхронного приемника записей для глобальной корректировки.
    /// </summary>
    public interface IAsyncRecordSink
        : IAsyncDisposable
    {
        /// <summary>
        /// Помещение в приемник модифицированной записи.
        /// Повторные попытки помещения одной и той же записи игнорируются.
        /// Сообщения, связанные с одной записью, склеиваются.
        /// </summary>
        Task PostRecordAsync
            (
                Record record,
                string? message = null
            );

        /// <summary>
        /// Сигнал окончания обработки записей.
        /// Может использоваться для пакетной отправки
        /// измененных записей на сервер.
        /// </summary>
        Task CompleteAsync();

        /// <summary>
        /// Получение протокола.
        /// </summary>
        Task<GblProtocolLine[]> GetProtocolAsync();

    } // interface IAsyncRecordSink

} // namespace ManagedIrbis.Processing
