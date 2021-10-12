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
        public ISyncProvider? Provider { get; set; }

        /// <summary>
        /// Источник записей.
        /// </summary>
        public ISyncRecordSource? RecordSource { get; set; }

        /// <summary>
        /// Приемник записей.
        /// </summary>
        public ISyncRecordSink? RecordSink { get; set; }

        /// <summary>
        /// Логгер.
        /// </summary>
        public GblLogger? Logger { get; set; }

        #endregion

    } // class GblContext

} // namespace ManagedIrbis.Gbl.Infrastructure
