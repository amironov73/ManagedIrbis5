// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* SiberianGrid.cs -- самописный грид для редактирования MARC-записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Самописный грид для редактирования записей.
    /// </summary>
    public partial class SiberianGrid
        : Control
    {
        #region Events

        /// <summary>
        /// Событие, возникающее при щелчке мышкой по гриду.
        /// </summary>
        public event EventHandler<SiberianClickEventArgs>? GridClick;

        /// <summary>
        /// Событие, возникающее при навигации.
        /// </summary>
        public event EventHandler<SiberianNavigationEventArgs>? Navigation;

        #endregion

        #region Properties

        /// <inheritdoc cref="Control.BackColor" />
        public override Color BackColor
        {
            get => Palette.BackColor;
            set
            {
                Palette.BackColor = value;
                OnBackColorChanged(EventArgs.Empty);
                Invalidate();
            }
        } // property BackColor

        /// <inheritdoc cref="Control.ForeColor" />
        public override Color ForeColor
        {
            get => Palette.ForeColor;
            set
            {
                Palette.ForeColor = value;
                OnForeColorChanged(EventArgs.Empty);
                Invalidate();
            }
        } // property ForeColor

        /// <summary>
        /// Коллекция колонок.
        /// </summary>
        public ISiberianColumnCollection Columns { get; }

        /// <summary>
        /// Коллекция строк.
        /// </summary>
        public ISiberianRowCollection Rows { get; }

        /// <summary>
        /// Текущая колонка.
        /// </summary>
        [Browsable(false)]
        public SiberianColumn? CurrentColumn { get; private set; }

        /// <summary>
        /// Текущая строка.
        /// </summary>
        [Browsable(false)]
        public virtual SiberianRow? CurrentRow { get; private set; }

        /// <summary>
        /// Текущая ячейка.
        /// </summary>
        [Browsable(false)]
        public virtual SiberianCell? CurrentCell { get; private set; }

        /// <summary>
        /// Определяет режим работы грида: только для чтения
        /// или разрешена запись.
        /// </summary>
        public bool ReadOnly { get; private set; }

        /// <summary>
        /// Текущий редактор (если есть).
        /// </summary>
        [Browsable(false)]
        public Control? Editor { get; internal set; }

        /// <summary>
        /// Размер заголовка в пикселах.
        /// </summary>
        public int HeaderHeight { get; set; }

        /// <summary>
        /// Палитра цветов.
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

        } // property UsableSize

        /// <summary>
        /// Количество видимых строк.
        /// </summary>
        public int VisibleRows => _visibleRows;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SiberianGrid()
            : this (new SiberianColumnCollection(), new SiberianRowCollection())
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SiberianGrid
            (
                ISiberianColumnCollection columns,
                ISiberianRowCollection rows
            )
        {
            Columns = columns;
            Rows = rows;
            Palette = SiberianPalette.DefaultPalette.Clone();

            CreateScrollBars(); //-V3068

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

        } // constructor

        #endregion

        #region Private members

        private ScrollBar? _horizontalScroll;
        private ScrollBar? _verticalScroll;

        private int _leftColumn;
        private int _topRow;

        private bool _autoSizeWatch;

        private readonly ToolTip _toolTip;
        private string? _previousToolTipText;

        private int _visibleRows;

        /// <summary>
        /// Автоматическое установление размера колонок.
        /// </summary>
        public void AutoSizeColumns()
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

        } // method AutoSizeColumns

        /// <summary>
        /// Создание новой строки (в самом низу).
        /// </summary>
        protected virtual SiberianRow CreateRow()
        {
            var result = new SiberianRow();

            return result;

        } // method CreateRow

        /// <summary>
        /// Создание полос прокрутки.
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

        } // method CreateScrollBar

        private int _DoScroll
            (
                ScrollBar? scrollBar,
                ScrollEventArgs args
            )
        {
            var result = args.OldValue;

            if (scrollBar is not null)
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
                    case ScrollEventType.ThumbPosition:
                    case ScrollEventType.ThumbTrack:
                        result = args.NewValue;
                        break;
                }

                result = Math.Max(result, scrollBar.Minimum);
                result = Math.Min(result, scrollBar.Maximum);
            }

            return result;

        } // method _DoScroll

        private void _horizontalScroll_Scroll
            (
                object? sender,
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
                object? sender,
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
            var oldColumn = CurrentColumn?.Index ?? -1;
            var oldRow = CurrentRow?.Index ?? -1;

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
        /// Закрывает текущий редактор (если есть).
        /// </summary>
        public void CloseEditor
            (
                bool accept
            )
        {
            if (CurrentCell is not null)
            {
                CurrentCell.CloseEditor(accept);
                Invalidate();
            }

        } // method CloseEditor

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

            CurrentColumn ??= result;

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
                object? state
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
                object? data
            )
        {
            var result = CreateRow();
            result.Data = data;
            result.Index = Rows.Count;
            result.Grid = this;

            CurrentRow ??= result;

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
            var column = cell.Column;
            var row = cell.Row;
            if (column is null || row is null)
            {
                return default;
            }

            var columnIndex = column.Index;
            var left = 0;
            for (var i = _leftColumn; i < columnIndex; i++)
            {
                left += Columns[i].Width;
            }

            var rowIndex = row.Index;
            var top = HeaderHeight;
            for (var i = _topRow; i < rowIndex; i++)
            {
                top += Rows[i].Height;
            }

            var result = new Rectangle
                (
                    left,
                    top,
                    column.Width,
                    row.Height
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

            SiberianCell? result;
            if (!HandleNavigation ( ref column, ref row ))
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
                    if (CurrentColumn is not null)
                    {
                        _horizontalScroll.Value = CurrentColumn.Index;
                    }
                }

                if (!ReferenceEquals(_verticalScroll, null))
                {
                    _verticalScroll.Maximum = Rows.Count;
                    if (CurrentRow is not null)
                    {
                        _verticalScroll.Value = CurrentRow.Index;
                    }
                }

                if (CurrentColumn is not null
                    && CurrentColumn.Index < _leftColumn)
                {
                    _leftColumn = CurrentColumn.Index;
                }

                if (CurrentRow is not null
                    && CurrentRow.Index < _topRow)
                {
                    _topRow = CurrentRow.Index;
                }

                // Adjust left column
                while (Columns.Count != 0)
                {
                    var x = 0;
                    var currentColumnIndex = CurrentColumn?.Index ?? -1;

                    for (var i = _leftColumn; i < currentColumnIndex; i++)
                    {
                        x += Columns[i].Width;
                    }

                    if (CurrentColumn != null)
                    {
                        x += Columns[CurrentColumn.Index].Width;
                    }

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
                    var currentRowIndex = CurrentRow?.Index ?? -1;

                    for (var i = _topRow; i < currentRowIndex; i++)
                    {
                        y += Rows[i].Height;
                    }

                    if (CurrentRow is not null)
                    {
                        y += Rows[CurrentRow.Index].Height;
                    }

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
            var delta = _verticalScroll?.LargeChange ?? 10;
            var result = MoveRelative(0, delta);

            return result;

        } // method MoveOnePageDown

        /// <summary>
        /// Move one page up.
        /// </summary>
        public SiberianCell? MoveOnePageUp()
        {
            var delta = - (_verticalScroll?.LargeChange ?? 10);
            var result = MoveRelative(0, delta);

            return result;

        } // method MoveOnePageUp

        /// <summary>
        /// Relative movement.
        /// </summary>
        public SiberianCell? MoveRelative
            (
                int columnDelta,
                int rowDelta
            )
        {
            var currentCell = CurrentCell;
            if (ReferenceEquals(currentCell, null))
            {
                return default;
            }

            var currentColumn = currentCell.Column;
            if (currentColumn is null)
            {
                return default;
            }

            var columnIndex = currentColumn.Index + columnDelta;

            var currentRow = currentCell.Row;
            if (currentRow is null)
            {
                return default;
            }

            var rowIndex = currentRow.Index + rowDelta;

            var result = Goto(columnIndex, rowIndex);

            return result;
        }

        /// <summary>
        /// Обрабатывает клик по гриду.
        /// </summary>
        public virtual void OnClick
            (
                SiberianClickEventArgs eventArgs
            )
        {
            GridClick.Raise(this, eventArgs);

            if (eventArgs.Cell is { } cell)
            {
                cell.HandleClick(eventArgs);

                var column = cell.EnsureColumn();
                var row = cell.EnsureRow();
                if (column.ReadOnly)
                {

                    cell = row.GetFirstEditableCell();
                    if (!ReferenceEquals(cell, null))
                    {
                        Goto
                            (
                                cell.Column!.Index,
                                cell.Row!.Index
                            );
                    }
                }
                else
                {
                    Goto
                        (
                            column.Index,
                            row.Index
                        );
                }
            }

            eventArgs.Column?.HandleClick(eventArgs);
            eventArgs.Row?.HandleClick(eventArgs);

        } // method OnClick

        #endregion

    } // class SiberianGrid

} // namespace ManagedIrbis.WinForms.Grid
