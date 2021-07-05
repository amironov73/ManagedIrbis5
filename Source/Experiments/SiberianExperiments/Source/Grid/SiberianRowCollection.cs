// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* SiberianRowCollection.cs -- коллекция строк грида (реализация по умолчанию)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Коллекция строк грида (реализация по умолчанию).
    /// Фактически не хранит строки, а предоставляет их по требованию.
    /// </summary>
    public class SiberianRowCollection
        : ISiberianRowCollection
    {
        #region Properties

        /// <summary>
        /// Высота строки по умолчанию.
        /// </summary>
        public int RowHeight { get; set; }

        /// <summary>
        /// Провайдер данных.
        /// </summary>
        public ISiberianProvider Provider { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SiberianRowCollection
            (
                ISiberianGrid grid,
                ISiberianProvider provider
            )
        {
            Grid = grid;
            Provider = provider;
            RowHeight = 20;

        } // constructor

        #endregion

        #region ISiberianRowCollection members

        /// <inheritdoc cref="ISiberianRowCollection.Add"/>
        public void Add
            (
                ISiberianRow row
            )
        {
            Provider.AddData(row.Data);

        } // method Add

        /// <inheritdoc cref="ISiberianRowCollection.Count"/>
        public int Count => Provider.DataLength;

        /// <inheritdoc cref="ISiberianRowCollection.Grid"/>
        public ISiberianGrid Grid { get; }

        /// <inheritdoc cref="ISiberianRowCollection.this"/>
        public ISiberianRow this[int index]
        {
            get
            {
                var cells = new SiberianCellCollection(Grid, null!);
                var result = new SiberianRow(Grid, cells)
                {
                    Data = Provider.GetData(index),
                    Index = index,
                    Height = RowHeight
                };

                cells.Row = result;

                return result;
            }

        } // property this

#endregion

    } // class SiberianRowCollection

} // namespace ManagedIrbis.WinForms.Grid
