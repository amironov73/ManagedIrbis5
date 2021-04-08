﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianGrid.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public partial class SiberianGrid
        : Control
    {
        #region Events

        /// <summary>
        /// Fired on click.
        /// </summary>
        public event EventHandler<SiberianClickEventArgs>? GridClick;

        /// <summary>
        /// Fired on navigation.
        /// </summary>
        public event EventHandler<SiberianNavigationEventArgs>? Navigation;

        #endregion

        #region Properties

        /// <inheritdoc/>
        public override Color BackColor
        {
            get { return Palette.BackColor; }
            set
            {
                Palette.BackColor = value;
                OnBackColorChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <inheritdoc/>
        public override Color ForeColor
        {
            get { return Palette.ForeColor; }
            set
            {
                Palette.ForeColor = value;
                OnForeColorChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <summary>
        /// Columns.
        /// </summary>
        public NonNullCollection<SiberianColumn> Columns { get; private set; }

        /// <summary>
        /// Rows.
        /// </summary>
        public NonNullCollection<SiberianRow> Rows { get; private set; }

        /// <summary>
        /// Current column.
        /// </summary>
        [Browsable(false)]
        public SiberianColumn? CurrentColumn { get; private set; }

        /// <summary>
        /// Current row.
        /// </summary>
        [Browsable(false)]
        public SiberianRow? CurrentRow { get; private set; }

        /// <summary>
        /// Current cell.
        /// </summary>
        [Browsable(false)]
        public SiberianCell? CurrentCell { get; private set; }

        /// <summary>
        /// Whether the whole grid itself is read-only.
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Current editor (if any).
        /// </summary>
        [Browsable(false)]
        public Control? Editor { get; internal set; }

        /// <summary>
        /// Header height.
        /// </summary>
        public int HeaderHeight { get; set; }

        /// <summary>
        /// Palette.
        /// </summary>
        public SiberianPalette Palette { get; set; }

        /// <summary>
        /// Usable size of the control.
        /// </summary>
        public Size UsableSize
        {
            get
            {
                var result = ClientSize;

                if (!ReferenceEquals(_verticalScroll, null))
                {
                    result.Width -= _verticalScroll.Width;
                }

                if (!ReferenceEquals(_horizontalScroll, null))
                {
                    result.Height -= _horizontalScroll.Height;
                }

                return result;
            }
        }

        /// <summary>
        /// Count of visible rows.
        /// </summary>
        public int VisibleRows { get { return _visibleRows; } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SiberianGrid()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor

            Palette = SiberianPalette.DefaultPalette.Clone();

            Columns = new NonNullCollection<SiberianColumn>();
            Rows = new NonNullCollection<SiberianRow>();

            CreateScrollBars();

            DoubleBuffered = true;

            _toolTip = new ToolTip();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Selectable, true);
            SetStyle(ControlStyles.StandardClick, true);
            SetStyle(ControlStyles.StandardDoubleClick, true);
            SetStyle(ControlStyles.UserMouse, true);

            BackColor = Color.DarkGray;
            Palette.LineColor = Color.Gray;
            ForeColor = Color.Black;

            HeaderHeight = SiberianRow.DefaultHeight;
        }

        #endregion

        #region Private members

        private ScrollBar? _horizontalScroll;
        private ScrollBar? _verticalScroll;

        private int _leftColumn;
        private int _topRow;

        private bool _autoSizeWatch;

        private readonly ToolTip _toolTip;
        private string _previousToolTipText;

        private int _visibleRows;

        internal void AutoSizeColumns()
        {
            if (_autoSizeWatch)
            {
                return;
            }

            var usableSize = UsableSize;

            try
            {
                _autoSizeWatch = true;

                var fixedColumns = Columns
                    .Where(column => column.FillWidth <= 0)
                    .ToArray();
                var needResize = Columns
                    .Where(column => column.FillWidth > 0)
                    .ToArray();

                if (needResize.Length != 0)
                {
                    var fixedSum = fixedColumns
                        .Sum(column => column.Width);
                    var fillSum = needResize
                        .Sum(column => column.FillWidth);

                    var remaining = usableSize.Width - 1 - fixedSum;

                    foreach (var column in needResize)
                    {
                        column.Width = Math.Max
                            (
                                column.MinWidth,
                                remaining * column.FillWidth / fillSum
                            );
                    }
                }
            }
            finally
            {
                _autoSizeWatch = false;
            }
        }

        /// <summary>
        /// Create row.
        /// </summary>
        protected virtual SiberianRow CreateRow()
        {
            var result = new SiberianRow();

            return result;
        }

        /// <summary>
        /// Create scroll bars.
        /// </summary>
        protected virtual void CreateScrollBars()
        {
            _horizontalScroll = new HScrollBar
            {
                AutoSize = false,
                Dock = DockStyle.Bottom,
                SmallChange = 1
            };
            Controls.Add(_horizontalScroll);
            _horizontalScroll.Scroll += _horizontalScroll_Scroll;

            _verticalScroll = new VScrollBar
            {
                AutoSize = false,
                Dock = DockStyle.Right,
                SmallChange = 1
            };
            Controls.Add(_verticalScroll);
            _verticalScroll.Scroll += _verticalScroll_Scroll;
        }

        private int _DoScroll
            (
                ScrollBar scrollBar,
                ScrollEventArgs args
            )
        {
            var result = args.OldValue;

            if (!ReferenceEquals(scrollBar, null))
            {
                switch (args.Type)
                {
                    case ScrollEventType.First:
                        result = scrollBar.Minimum;
                        break;

                    case ScrollEventType.LargeDecrement:
                        result -= scrollBar.LargeChange;
                        break;

                    case ScrollEventType.LargeIncrement:
                        result += scrollBar.LargeChange;
                        break;

                    case ScrollEventType.Last:
                        result = scrollBar.Maximum;
                        break;

                    case ScrollEventType.SmallDecrement:
                        result -= scrollBar.SmallChange;
                        break;

                    case ScrollEventType.SmallIncrement:
                        result += scrollBar.SmallChange;
                        break;

                    case ScrollEventType.EndScroll:
                        result = args.NewValue;
                        break;

                    case ScrollEventType.ThumbPosition:
                        result = args.NewValue;
                        break;

                    case ScrollEventType.ThumbTrack:
                        result = args.NewValue;
                        break;
                }

                result = Math.Max(result, scrollBar.Minimum);
                result = Math.Min(result, scrollBar.Maximum);
            }

            return result;
        }

        private void _horizontalScroll_Scroll
            (
                object sender,
                ScrollEventArgs e
            )
        {
            if (ReferenceEquals(CurrentRow, null))
            {
                return;
            }

            var value = _DoScroll
                (
                    _horizontalScroll,
                    e
                );

            e.NewValue = value;

            Goto
                (
                    value,
                    CurrentRow.Index
                );
        }

        private void _verticalScroll_Scroll
            (
                object sender,
                ScrollEventArgs e
            )
        {
            if (ReferenceEquals(CurrentColumn, null))
            {
                return;
            }

            var value = _DoScroll
                (
                    _verticalScroll,
                    e
                );

            e.NewValue = value;

            Goto
                (
                    CurrentColumn.Index,
                    value
                );
        }

        /// <summary>
        /// Handle navigation (can cancel).
        /// </summary>
        protected internal virtual bool HandleNavigation
            (
                ref int column,
                ref int row
            )
        {
            var oldColumn = ReferenceEquals(CurrentColumn, null)
                ? -1
                : CurrentColumn.Index;
            var oldRow = ReferenceEquals(CurrentRow, null)
                ? -1
                : CurrentRow.Index;

            var handler = Navigation;
            if (!ReferenceEquals(handler, null))
            {
                var eventArgs = new SiberianNavigationEventArgs
                {
                    OldColumn = oldColumn,
                    OldRow = oldRow,
                    NewColumn = column,
                    NewRow = row
                };

                handler(this, eventArgs);

                column = eventArgs.NewColumn;
                row = eventArgs.NewRow;

                if (eventArgs.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Close current editor.
        /// </summary>
        public void CloseEditor
            (
                bool accept
            )
        {
            if (ReferenceEquals(CurrentCell, null))
            {
                return;
            }

            CurrentCell.CloseEditor(accept);
            Invalidate();
        }

        /// <summary>
        /// Get count of visible rows.
        /// </summary>
        public int CountVisibleRows()
        {
            var usableSize = UsableSize;

            var result = (usableSize.Height - HeaderHeight)
                         / SiberianRow.DefaultHeight;
            result = Math.Max(result, 1);

            return result;
        }

        /// <summary>
        /// Create column of specified type.
        /// </summary>
        public T CreateColumn<T>()
            where T : SiberianColumn, new()
        {
            var result = new T
            {
                Grid = this,
                Index = Columns.Count
            };

            if (ReferenceEquals(CurrentColumn, null))
            {
                CurrentColumn = result;
            }

            foreach (var row in Rows)
            {
                var cell = result.CreateCell();
                cell.Row = row;
                row.Cells.Add(cell);
            }

            Columns.Add(result);

            if (!ReferenceEquals(_horizontalScroll, null))
            {
                _horizontalScroll.Maximum = Columns.Count;
                _horizontalScroll.Value = CurrentColumn.Index;
            }

            if (ReferenceEquals(CurrentCell, null))
            {
                if (!ReferenceEquals(CurrentRow, null))
                {
                    CurrentCell = GetCell
                    (
                        CurrentColumn.Index,
                        CurrentRow.Index
                    );
                }
            }

            AutoSizeColumns();
            Invalidate();

            return result;
        }

        /// <summary>
        /// Open editor for current cell.
        /// </summary>
        public Control? OpenEditor
            (
                bool edit,
                object state
            )
        {
            if (!ReferenceEquals(Editor, null))
            {
                return Editor;
            }

            if (ReadOnly)
            {
                return null;
            }

            if (ReferenceEquals(CurrentCell, null))
            {
                return null;
            }

            if (ReferenceEquals(CurrentColumn, null)
                || CurrentColumn.ReadOnly)
            {
                return null;
            }

            Editor = CurrentColumn.CreateEditor
                (
                    CurrentCell,
                    edit,
                    state
                );

            return Editor;
        }

        /// <summary>
        /// Create row.
        /// </summary>
        public SiberianRow CreateRow
            (
                object data
            )
        {
            var result = CreateRow();
            result.Data = data;
            result.Index = Rows.Count;
            result.Grid = this;

            if (ReferenceEquals(CurrentRow, null))
            {
                CurrentRow = result;
            }

            foreach (var column in Columns)
            {
                var cell = column.CreateCell();
                cell.Row = result;
                result.Cells.Add(cell);
            }

            Rows.Add(result);

            if (!ReferenceEquals(_verticalScroll, null))
            {
                _verticalScroll.Maximum = Rows.Count;
                _verticalScroll.Value = CurrentRow.Index;
            }

            if (ReferenceEquals(CurrentCell, null))
            {
                if (!ReferenceEquals(CurrentColumn, null))
                {
                    CurrentCell = GetCell
                    (
                        CurrentColumn.Index,
                        CurrentRow.Index
                    );
                }
            }

            Invalidate();

            return result;
        }

        /// <summary>
        /// Find cell under given mouse position.
        /// </summary>
        public SiberianCell? FindCell
            (
                int x,
                int y
            )
        {
            var column = FindColumn(x);
            var row = FindRow(y);

            if (!ReferenceEquals(column, null)
                && !ReferenceEquals(row, null))
            {
                var result = GetCell
                    (
                        column.Index,
                        row.Index
                    );

                return result;
            }

            return null;
        }

        /// <summary>
        /// Find column under given mouse position.
        /// </summary>
        public SiberianColumn? FindColumn
            (
                int x
            )
        {
            var left = 0;

            for (
                    var columnIndex = _leftColumn;
                    columnIndex < Columns.Count;
                    columnIndex++
                )
            {
                var column = Columns[columnIndex];
                var right = left + column.Width;
                if (x >= left && x <= right)
                {
                    return column;
                }
                left = right;
            }

            return null;
        }

        /// <summary>
        /// Find row under given mouse position.
        /// </summary>
        public SiberianRow? FindRow
            (
                int y
            )
        {
            var top = HeaderHeight;
            for (
                    var rowIndex = _topRow;
                    rowIndex < Rows.Count;
                    rowIndex++
                )
            {
                var row = Rows[rowIndex];
                var bottom = top + row.Height;
                if (y >= top && y <= bottom)
                {
                    return row;
                }
                top = bottom;
            }

            return null;
        }


        /// <summary>
        /// Get cell for given column and row.
        /// </summary>
        public SiberianCell? GetCell
            (
                int column,
                int row
            )
        {
            if (column >= 0 && column < Columns.Count
                && row >= 0 && row < Rows.Count)
            {
                return Rows[row].Cells[column];
            }

            return null;
        }

        /// <summary>
        /// Get cell rectangle.
        /// </summary>
        public Rectangle GetCellRectangle
            (
                SiberianCell cell
            )
        {
            var column = cell.Column.Index;
            var x = 0;
            for (var i = _leftColumn; i < column; i++)
            {
                x += Columns[i].Width;
            }

            var row = cell.Row.Index;
            var y = HeaderHeight;
            for (var i = _topRow; i < row; i++)
            {
                y += Rows[i].Height;
            }
            var width = cell.Column.Width;
            var height = cell.Row.Height;

            var result = new Rectangle
                (
                    x,
                    y,
                    width,
                    height
                );

            return result;
        }

        /// <summary>
        /// Go to specified cell.
        /// </summary>
        public SiberianCell? Goto
            (
                int column,
                int row
            )
        {
            CloseEditor(true);

            SiberianCell result;

            if (!HandleNavigation
                (
                    ref column,
                    ref row
                ))
            {
                result = GetCell(column, row);

                return result;
            }

            if (column >= Columns.Count)
            {
                column = Columns.Count - 1;
            }
            if (column < 0)
            {
                column = 0;
            }
            if (row >= Rows.Count)
            {
                row = Rows.Count - 1;
            }
            if (row < 0)
            {
                row = 0;
            }

            result = GetCell(column, row);

            if (!ReferenceEquals(result, null))
            {
                var usableSize = UsableSize;

                CurrentRow = result.Row;
                CurrentColumn = result.Column;
                CurrentCell = result;

                if (!ReferenceEquals(_horizontalScroll, null))
                {
                    _horizontalScroll.Maximum = Columns.Count;
                    _horizontalScroll.Value = CurrentColumn.Index;
                }

                if (!ReferenceEquals(_verticalScroll, null))
                {
                    _verticalScroll.Maximum = Rows.Count;
                    _verticalScroll.Value = CurrentRow.Index;
                }

                if (CurrentColumn.Index < _leftColumn)
                {
                    _leftColumn = CurrentColumn.Index;
                }

                if (CurrentRow.Index < _topRow)
                {
                    _topRow = CurrentRow.Index;
                }

                // Adjust left column
                while (Columns.Count != 0)
                {
                    var x = 0;

                    for (var i = _leftColumn; i < CurrentColumn.Index; i++)
                    {
                        x += Columns[i].Width;
                    }
                    x += Columns[CurrentColumn.Index].Width;

                    if (x < usableSize.Width)
                    {
                        break;
                    }

                    _leftColumn++;
                    if (_leftColumn >= Columns.Count)
                    {
                        _leftColumn = Math.Max(0, Columns.Count - 1);
                        break;
                    }
                }

                // Adjust top row
                while (Rows.Count != 0)
                {
                    var y = HeaderHeight;

                    for (var i = _topRow; i < CurrentRow.Index; i++)
                    {
                        y += Rows[i].Height;
                    }
                    y += Rows[CurrentRow.Index].Height;

                    if (y < usableSize.Height)
                    {
                        break;
                    }

                    _topRow++;
                    if (_topRow >= Rows.Count)
                    {
                        _topRow = Math.Max(0, Rows.Count - 1);
                        break;
                    }
                }

                _visibleRows = CountVisibleRows();
                if (!ReferenceEquals(_verticalScroll, null))
                {
                    _verticalScroll.LargeChange = _visibleRows;
                }

                Invalidate();
            }

            return result;
        }

        /// <summary>
        /// Move one column left.
        /// </summary>
        public SiberianCell? MoveOneColumnLeft()
        {
            if (ReferenceEquals(CurrentColumn, null))
            {
                return CurrentCell;
            }

            var index = CurrentColumn.Index;
            var newIndex = index - 1;

            for (; newIndex >= 0; newIndex--)
            {
                if (!Columns[newIndex].ReadOnly)
                {
                    break;
                }
            }

            if (newIndex < 0)
            {
                return CurrentCell;
            }

            var result = MoveRelative
                (
                    newIndex - index,
                    0
                );

            return result;
        }

        /// <summary>
        /// Move one column right.
        /// </summary>
        public SiberianCell? MoveOneColumnRight()
        {
            var result = MoveRelative(1, 0);

            return result;
        }

        /// <summary>
        /// Move one row down.
        /// </summary>
        public SiberianCell? MoveOneLineDown()
        {
            var result = MoveRelative(0, 1);

            return result;
        }

        /// <summary>
        /// Move one row down.
        /// </summary>
        public SiberianCell? MoveOneLineUp()
        {
            var result = MoveRelative(0, -1);

            return result;
        }

        /// <summary>
        /// Move one page down.
        /// </summary>
        public SiberianCell? MoveOnePageDown()
        {
            var delta = ReferenceEquals(_verticalScroll, null)
                ? 10
                : _verticalScroll.LargeChange;

            var result = MoveRelative(0, delta);

            return result;
        }

        /// <summary>
        /// Move one page up.
        /// </summary>
        public SiberianCell? MoveOnePageUp()
        {
            var delta = ReferenceEquals(_verticalScroll, null)
                ? 10
                : _verticalScroll.LargeChange;

            var result = MoveRelative(0, delta);

            return result;
        }

        /// <summary>
        /// Relative movement.
        /// </summary>
        public SiberianCell? MoveRelative
            (
                int columnDelta,
                int rowDelta
            )
        {
            var current = CurrentCell;
            if (ReferenceEquals(current, null))
            {
                return null;
            }

            var column = current.Column.Index + columnDelta;
            var row = current.Row.Index + rowDelta;

            var result = Goto(column, row);

            return result;
        }

        /// <summary>
        /// Handles click on the cell.
        /// </summary>
        public virtual void OnClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            GridClick?.Invoke(this, eventArgs);

            var cell = eventArgs.Cell;
            if (!ReferenceEquals(cell, null))
            {
                cell.HandleClick(eventArgs);

                if (cell.Column.ReadOnly)
                {
                    cell = cell.Row.GetFirstEditableCell();
                    if (!ReferenceEquals(cell, null))
                    {
                        Goto
                            (
                                cell.Column.Index,
                                cell.Row.Index
                            );
                    }
                }
                else
                {
                    Goto
                        (
                            cell.Column.Index,
                            cell.Row.Index
                        );
                }
            }

            eventArgs.Column?.HandleClick(eventArgs);
            eventArgs.Row?.HandleClick(eventArgs);
        }

        #endregion
    }
}
