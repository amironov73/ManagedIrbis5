// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* VirtualGrid.cs -- грид, подгружающий данные по мере надобности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Грид, подгружающий данные по мере надобности.
    /// Работает только в режиме чтения.
    /// </summary>
    public class VirtualGrid
        : DataGridView
    {
        #region Properties

        /// <summary>
        /// Адаптер, поставляющий данные.
        /// </summary>
        public VirtualAdapter? Adapter { get; set; }

        /// <summary>
        /// Текущие строки.
        /// </summary>
        public object?[]? Lines => _cache;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public VirtualGrid ()
        {
            ReadOnly = true;
            VirtualMode = true;
            AutoGenerateColumns = false;
            RowHeadersVisible = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AllowUserToAddRows = false;
            AllowUserToResizeRows = false;
            AllowUserToDeleteRows = false;
            EnableHeadersVisualStyles = false;
            ColumnHeadersDefaultCellStyle.SelectionBackColor = ColumnHeadersDefaultCellStyle.BackColor;
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        } // constructor

        #endregion

        #region Private members

        private bool _busy;

        private int _firstLine, _lastLine, _lineCount, _rowCount;

        private object?[]? _cache;

        /// <summary>
        /// Забираем данные из адаптера.
        /// </summary>
        private bool PullData
            (
                int firstLine
            )
        {
            if (_busy)
            {
                return false;
            }

            try
            {
                if (Adapter is not { } adapter)
                {
                    return false;
                }

                _busy = true;

                var portion = Math.Max(128, adapter.PageSize);
                var data = adapter.PullData(firstLine, portion);
                if (data is null)
                {
                    return false;
                }

                _firstLine = data.FirstLine;
                _lineCount = data.LineCount;
                _lastLine = _firstLine + _lineCount - 1;
                _cache = data.Lines;
                if (data.TotalCount != 0)
                {
                    _rowCount = data.TotalCount;
                    RowCount = _rowCount;
                }

            }
            finally
            {
                _busy = false;
            }

            return true;

        } // method PullData

        /// <inheritdoc cref="DataGridView.OnCellValueNeeded(DataGridViewCellValueEventArgs)"/>
        protected override void OnCellValueNeeded
            (
                DataGridViewCellValueEventArgs e
            )
        {
            if (Adapter is not { } adapter)
            {
                return;
            }

            var rowIndex = e.RowIndex;
            var columnIndex = e.ColumnIndex;

            if (rowIndex < 0 || rowIndex >= RowCount
                || columnIndex < 0 || columnIndex >= ColumnCount)
            {
                return;
            }

            if (rowIndex < _firstLine || rowIndex > _lastLine || _cache is null)
            {
                var firstLine = rowIndex;

                if (rowIndex < _firstLine)
                {
                    firstLine = Math.Max(0, rowIndex - adapter.PageSize + 1);
                }

                if (!PullData(firstLine))
                {
                    return;
                }
            }

            if (_cache is null || rowIndex < _firstLine || rowIndex > _lastLine)
            {
                return;
            }

            var line = _cache [rowIndex - _firstLine];
            e.Value = adapter.ByIndex (line, columnIndex);

        } // method OnCellValueNeeded

        #endregion

        #region Public methods

        /// <summary>
        /// Очистка.
        /// </summary>
        public void Clear()
        {
            _firstLine = 0;
            _lineCount = 0;
            _cache = null;
            //RowCount = 0;
            Rows.Clear();
            Invalidate();
        }

        /// <summary>
        /// Наполнение указанными данными
        /// </summary>
        public void Fill
            (
                object?[] lines
            )
        {
            // TODO: реализовать

            Clear();
        }

        /// <summary>
        /// Начальное наполнение данными.
        /// </summary>
        public void InitialFill()
        {
            Clear();

            if (PullData(0))
            {
                Invalidate();
            }

        } // method InitialFill

        #endregion

    } // class VirtualGrid

} // namespace AM.Windows.Forms
