// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

/* VirtualRecordAdapter.cs -- адаптер, предоставляющий записи для виртуального режима грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Адаптер, предоставляющий записи для виртуального режима грида.
    /// </summary>
    public sealed class VirtualRecordAdapter
        : IVirtualAdapter<Record>
    {
        #region Properties

        /// <summary>
        /// Провайдер.
        /// </summary>
        public ISyncProvider Provider { get; }

        /// <summary>
        /// Текущая база данных.
        /// </summary>
        public string Database { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="provider">Провайдер.</param>
        /// <param name="database">Текущая база данных.</param>
        public VirtualRecordAdapter
            (
                ISyncProvider provider,
                string database
            )
        {
            Provider = provider;
            Database = database;

        } // constructor

        #endregion

        #region IVirtualAdapter<T> members

        /// <inheritdoc cref="IVirtualAdapter{T}.TotalLength"/>
        public int TotalLength => Provider.GetMaxMfn();

        /// <inheritdoc cref="IVirtualAdapter{T}.ReadData"/>
        public VirtualData<Record>? ReadData
            (
                int firstLine,
                int lineCount
            )
        {
            var batch = Enumerable.Range(firstLine, lineCount);
            var records = Provider.ReadRecords(Database, batch);
            if (records is null)
            {
                return null;
            }

            var result = new VirtualData<Record>
            {
                FirstLine = firstLine,
                Length = records.Length,
                Data = records
            };

            return result;

        } // method ReadData

        #endregion

    } // class VirtualRecordAdapter

} // namespace ManagedIrbis.WinForms.Grid
