// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IRecordSource.cs -- абстрактный источник записей
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Абстрактный источник записей для глобальной корректировки.
    /// </summary>
    public interface IRecordSource
    {
        #region Public methods

        /// <summary>
        /// Get next record (if any).
        /// </summary>
        public abstract Record? GetNextRecord();

        /// <summary>
        /// Get record count.
        /// </summary>
        public abstract int GetRecordCount();

        /// <summary>
        /// Reset the source.
        /// </summary>
        public abstract void Reset();

        /// <summary>
        /// Write back the modified record.
        /// </summary>
        public abstract void WriteRecord
            (
                Record record
            );

        #endregion

    } // class IRecordSource

} // namespace ManagedIrbis.Gbl.Infrastructure
