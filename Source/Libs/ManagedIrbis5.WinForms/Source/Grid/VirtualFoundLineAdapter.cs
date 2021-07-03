// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

/* VirtualFoundLineAdapter.cs -- адаптер, предоставляющий записи для виртуального режима грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Адаптер, предоставляющий записи для виртуального режима грида.
    /// </summary>
    public sealed class VirtualFoundLineAdapter
        : IVirtualAdapter<FoundLine>
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

        /// <summary>
        /// Формат.
        /// </summary>
        public string Format { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="provider">Провайдер.</param>
        /// <param name="database">Текущая база данных.</param>
        /// <param name="format">Формат.</param>
        public VirtualFoundLineAdapter
            (
                ISyncProvider provider,
                string database,
                string format
            )
        {
            Provider = provider;
            Database = database;
            Format = format;

        } // constructor

        #endregion

        #region IVirtualAdapter<T> members

        /// <inheritdoc cref="IVirtualAdapter{T}.TotalLength"/>
        public int TotalLength => Provider.GetMaxMfn();

        /// <inheritdoc cref="IVirtualAdapter{T}.ReadData"/>
        public VirtualData<FoundLine> ReadData
            (
                int firstLine,
                int lineCount
            )
        {
            var batch = Enumerable.Range(firstLine, lineCount);
            Provider.Database = Database;
            var lines = FoundLine.Read(Provider, Format, batch);

            var result = new VirtualData<FoundLine>
            {
                FirstLine = firstLine,
                Length = lines.Length,
                Data = lines
            };

            return result;

        } // method ReadData

        #endregion

    } // class VirtualFoundLineAdapter

} // namespace ManagedIrbis.WinForms.Grid
