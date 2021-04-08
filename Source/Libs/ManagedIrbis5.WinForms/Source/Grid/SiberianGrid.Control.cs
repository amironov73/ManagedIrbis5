// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianGrid.Control.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    [System.ComponentModel.DesignerCategory("Code")]
    public partial class SiberianGrid
    {
        #region Control members

        /// <inheritdoc />
        protected override Size DefaultSize
        {
            get { return new Size(640, 375); }
        }

        /// <inheritdoc />
        protected override bool IsInputKey
            (
                Keys keyData
            )
        {
            // Enable all the keys.
            return true;
        }

        /// <inheritdoc/>
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);

            if (!ReferenceEquals(_horizontalScroll, null))
            {
                _horizontalScroll.Dispose();
            }

            if (!ReferenceEquals(_verticalScroll, null))
            {
                _verticalScroll.Dispose();
            }

            if (!ReferenceEquals(_toolTip, null))
            {
                _toolTip.Dispose();
            }
        }

        /// <inheritdoc/>
        protected override void OnKeyDown
            (
                KeyEventArgs e
            )
        {
            base.OnKeyDown(e);

            if (e.Modifiers == 0)
            {
                e.Handled = true;

                switch (e.KeyCode)
                {
                    case Keys.Up:
                        MoveOneLineUp();
                        break;

                    case Keys.Down:
                        MoveOneLineDown();
                        break;

                    case Keys.Left:
                        MoveOneColumnLeft();
                        break;

                    case Keys.Right:
                        MoveOneColumnRight();
                        break;

                    case Keys.PageUp:
                        MoveOnePageUp();
                        break;

                    case Keys.PageDown:
                        MoveOnePageDown();
                        break;

                    case Keys.Enter:
                        OpenEditor(true, null);
                        break;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnKeyPress
            (
                KeyPressEventArgs e
            )
        {
            base.OnKeyPress(e);

            if (!char.IsControl(e.KeyChar))
            {
                OpenEditor
                    (
                        false,
                        e.KeyChar.ToString()
                    );
                e.Handled = true;
            }
        }

        /// <inheritdoc />
        protected override void OnMouseClick
            (
                MouseEventArgs e
            )
        {
            CloseEditor(false);
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Left)
            {
                var cell = FindCell(e.X, e.Y);

                if (!ReferenceEquals(cell, null))
                {
                    var eventArgs = new SiberianClickEventArgs
                    {
                        Grid = this,
                        Row = cell.Row,
                        Column = cell.Column,
                        Cell = cell
                    };

                    OnClick(eventArgs);
               }
                else
                {
                    var row = FindRow(e.Y);
                    if (!ReferenceEquals(row, null)
                        && !ReferenceEquals(CurrentColumn, null))
                    {
                        Goto
                            (
                                CurrentColumn.Index,
                                row.Index
                            );
                    }
                }
            }
        }

        ///// <inheritdoc />
        //protected override void OnMouseDoubleClick
        //    (
        //        MouseEventArgs e
        //    )
        //{
        //    CloseEditor(false);
        //    base.OnMouseDoubleClick(e);

        //    if (e.Button == MouseButtons.Left)
        //    {
        //        SiberianCell cell = FindCell(e.X, e.Y);
        //        if (!ReferenceEquals(cell, null))
        //        {
        //            if (ReferenceEquals(cell, CurrentCell))
        //            {
        //                OpenEditor(true, null);
        //            }
        //        }
        //    }
        //}

        /// <inheritdoc/>
        protected override void OnMouseMove
            (
                MouseEventArgs e
            )
        {
            base.OnMouseMove(e);

            var cell = FindCell(e.X, e.Y);
            if (!ReferenceEquals(cell, null))
            {
                var eventArgs = new SiberianToolTipEventArgs
                {
                    Grid = this,
                    X = e.X,
                    Y = e.Y
                };

                cell.HandleToolTip(eventArgs);

                if (eventArgs.ToolTipText != _previousToolTipText)
                {
                    _previousToolTipText = eventArgs.ToolTipText;
                    _toolTip.SetToolTip(this, _previousToolTipText);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseWheel
            (
                MouseEventArgs e
            )
        {
            base.OnMouseWheel(e);

            MoveRelative
                (
                    0,
                    - e.Delta / 10
                );
        }

        /// <inheritdoc/>
        protected override void OnPaint
            (
                PaintEventArgs paintEvent
            )
        {
            var graphics = paintEvent.Graphics;
            var clip = paintEvent.ClipRectangle;

            using (Brush brush = new SolidBrush(BackColor))
            {
                graphics.FillRectangle(brush, clip);
            }

            var usableSize = UsableSize;

            var x = 0;
            var y = usableSize.Height;
            PaintEventArgs args;

            // Рисуем заголовки колонок
            for (var columnIndex = _leftColumn; columnIndex < Columns.Count; columnIndex++)
            {
                var column = Columns[columnIndex];
                var height = HeaderHeight;

                clip = new Rectangle
                    (
                        x,
                        0,
                        column.Width,
                        height
                    );
                args = new PaintEventArgs
                    (
                        graphics,
                        clip
                    );
                column.PaintHeader(args);

                clip = new Rectangle
                    (
                        x,
                        height,
                        column.Width,
                        y - height
                    );
                args = new PaintEventArgs
                    (
                        graphics,
                        clip
                    );
                column.Paint(args);

                x += column.Width;
            }

            x = 0;
            using (Brush lineBrush = new SolidBrush(Palette.LineColor))
            using (var pen = new Pen(lineBrush))
            {
                // Рисуем линию, отделяющую заголовки от содержимого колонок
                var x2 = usableSize.Width;
                graphics.DrawLine(pen, 0, HeaderHeight, x2, HeaderHeight);

                // Рисуем вертикальные разделители между колонками
                for (var columnIndex = _leftColumn; columnIndex < Columns.Count; columnIndex++)
                {
                    var column = Columns[columnIndex];
                    x += column.Width;
                    graphics.DrawLine(pen, x, 0, x, y);
                }

                // Рисуем горизонтальные разделители между строками
                x = usableSize.Width;
                y = HeaderHeight;
                for (var rowIndex = _topRow; rowIndex < Rows.Count; rowIndex++)
                {
                    var row = Rows[rowIndex];
                    args = new PaintEventArgs
                        (
                            graphics,
                            new Rectangle
                            (
                                0,
                                y,
                                x,
                                row.Height
                            )
                        );
                    row.Paint(args);

                    graphics.DrawLine(pen, 0, y, x, y);
                    y += row.Height;

                    if (y >= usableSize.Height)
                    {
                        break;
                    }
                }

                // Рисуем ячейки
                x = 0;
                for (var columnIndex = _leftColumn; columnIndex < Columns.Count; columnIndex++)
                {
                    var column = Columns[columnIndex];
                    var dx = column.Width;

                    y = HeaderHeight;
                    for (var rowIndex = _topRow; rowIndex < Rows.Count; rowIndex++)
                    {
                        var row = Rows[rowIndex];
                        var dy = row.Height;

                        args = new PaintEventArgs
                            (
                                graphics,
                                new Rectangle
                                    (
                                        x + 1,
                                        y + 1,
                                        dx - 2,
                                        dy - 2
                                    )
                            );
                        var cell = row.Cells[columnIndex];
                        cell.Paint(args);
                        y += dy;
                    }
                    x += dx;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnResize
            (
                EventArgs e
            )
        {
            base.OnResize(e);

            AutoSizeColumns();
            MoveRelative(0, 0);
        }

        #endregion
    }
}
