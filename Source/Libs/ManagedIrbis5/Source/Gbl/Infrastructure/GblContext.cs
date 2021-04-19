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
    /// Контекст исполнения GBL-программы
    /// </summary>
    public sealed class GblContext
    {
        #region Properties

        /// <summary>
        /// Current record.
        /// </summary>
        public Record? CurrentRecord { get; set; }

        /// <summary>
        /// Provider.
        /// </summary>
        public ISyncIrbisProvider? Provider { get; set; }

        /// <summary>
        /// Record source.
        /// </summary>
        public IRecordSource? RecordSource { get; set; }

        /// <summary>
        /// Logger.
        /// </summary>
        public GblLogger? Logger { get; set; }

        #endregion

    } // class GblContext

} // namespace ManagedIrbis.Gbl.Infrastructure
