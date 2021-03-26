// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IRecord.cs -- маркерный интерфейс для записи
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Маркерный интерфейс для записи.
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// База данных, в которой хранится запись.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN (порядковый номер в базе данных) записи.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Версия записи.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Статус записи.
        /// </summary>
        public RecordStatus Status { get; set; }

    } // interface IRecord

} // namespace ManagedIrbis.Infrastructure
