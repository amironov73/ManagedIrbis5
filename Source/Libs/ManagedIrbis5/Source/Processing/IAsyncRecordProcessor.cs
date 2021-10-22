// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IAsyncRecordProcessor.cs -- интерфейс асинхронного процессора записей
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
    /// Интерфейс асинхронного процессора записей.
    /// </summary>
    public interface IAsyncRecordProcessor
        : IAsyncDisposable
    {
        /// <summary>
        /// Обработка одной записи.
        /// </summary>
        Task<ProtocolLine> ProcessOneRecordAsync (Record record);

        /// <summary>
        /// Обработка множества записей.
        /// </summary>
        Task<GblResult> ProcessRecordsAsync
            (
                ISyncRecordSource source,
                ISyncRecordSink sync
            );

    } // interface IAsyncRecordProcessor

} // namespace ManagedIrbis.Processing
