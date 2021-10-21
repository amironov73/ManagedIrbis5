// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* GblContext.cs -- контекст исполнения GBL-программа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Processing;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Контекст исполнения программы глобальной корректировки записей.
    /// </summary>
    public sealed class GblContext
    {
        #region Properties

        /// <summary>
        /// Текущая запись (может отсутствовать).
        /// </summary>
        public Record? CurrentRecord { get; set; }

        /// <summary>
        /// Синхронный ИРБИС-провайдер.
        /// </summary>
        public ISyncProvider? SyncProvider { get; set; }

        /// <summary>
        /// Синхронный ИРБИС-провайдер.
        /// </summary>
        public IAsyncProvider? AsyncProvider { get; set; }

        /// <summary>
        /// Синхронный источник записей.
        /// </summary>
        public ISyncRecordSource? SyncRecordSource { get; set; }

        /// <summary>
        /// Синхронный приемник записей.
        /// </summary>
        public ISyncRecordSink? SyncRecordSink { get; set; }

        /// <summary>
        /// Асинхронный источник записей.
        /// </summary>
        public IAsyncRecordSource? AsyncRecordSource { get; set; }

        /// <summary>
        /// Асинхронный приемник записей.
        /// </summary>
        public IAsyncRecordSink? AsyncRecordSink { get; set; }

        /// <summary>
        /// Логгер.
        /// </summary>
        public ILogger? Logger { get; set; }

        #endregion

    } // class GblContext

} // namespace ManagedIrbis.Gbl.Infrastructure
