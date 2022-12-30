// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting.Table
{
    /// <summary>
    /// Represents data of the table cell.
    /// </summary>
    public class TableCellData : IDisposable
    {
        #region Fields

        private int colSpan;
        private int rowSpan;
        private bool updatingLayout;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets parent table of the cell.
        /// </summary>
        public TableBase Table { get; set; }

        /// <summary>
        /// Gets or sets objects collection of the cell.
        /// </summary>
        public ReportComponentCollection Objects { get; set; }

        /// <summary>
        /// Gets or sets text of the table cell.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets value of the table cell.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets hyperlink value of the table cell.
        /// </summary>
        public string HyperlinkValue { get; set; }

        /// <summary>
        /// Gets or sets column span of the table cell.
        /// </summary>
        public int ColSpan
        {
            get => colSpan;
            set
            {
                if (colSpan != value)
                {
                    var oldWidth = Width;
                    colSpan = value;
                    if (Table != null)
                    {
                        Table.ResetSpanList();
                        UpdateLayout (oldWidth, Height, Width - oldWidth, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets row span of the table cell.
        /// </summary>
        public int RowSpan
        {
            get => rowSpan;
            set
            {
                if (rowSpan != value)
                {
                    var oldHeight = Height;
                    rowSpan = value;
                    if (Table != null)
                    {
                        Table.ResetSpanList();
                        UpdateLayout (Width, oldHeight, 0, Height - oldHeight);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the address of the table cell.
        /// </summary>
        public Point Address { get; set; }

        /// <summary>
        /// Gets the table cell.
        /// </summary>
        public TableCell Cell
        {
            get
            {
                if (Table.IsResultTable)
                {
                    var cell0 = Style;
                    if (cell0 == null)
                    {
                        cell0 = Table.Styles.DefaultStyle;
                    }

                    if (OriginalCell != null)
                    {
                        cell0.Alias = OriginalCell.Alias;
                        cell0.OriginalComponent = OriginalCell.OriginalComponent;
                    }

                    // handling dock/anchor of cell objects correctly: detach old celldata, update size, attach new one.
                    cell0.CellData = null;
                    cell0.Width = Width;
                    cell0.Height = Height;
                    cell0.CellData = this;
                    cell0.Hyperlink.Value = HyperlinkValue;

                    return cell0;
                }

                if (OriginalCell == null)
                {
                    OriginalCell = new TableCell();
                    OriginalCell.CellData = this;
                }

                return OriginalCell;
            }
        }

        /// <summary>
        /// Gets style of table cell.
        /// </summary>
        public TableCell Style { get; private set; }

        /// <summary>
        /// Gets original the table cell.
        /// </summary>
        public TableCell OriginalCell { get; private set; }

        /// <summary>
        /// Gets width of the table cell.
        /// </summary>
        public float Width
        {
            get
            {
                if (Table == null)
                {
                    return 0;
                }

                float result = 0;
                for (var i = 0; i < ColSpan; i++)
                {
                    if (Address.X + i < Table.Columns.Count)
                    {
                        result += Table.Columns[Address.X + i].Width;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets height of the table cell.
        /// </summary>
        public float Height
        {
            get
            {
                if (Table == null)
                {
                    return 0;
                }

                float result = 0;
                for (var i = 0; i < RowSpan; i++)
                {
                    if (Address.Y + i < Table.Rows.Count)
                    {
                        result += Table.Rows[Address.Y + i].Height;
                    }
                }

                return result;
            }
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableCellData"/> class.
        /// </summary>
        public TableCellData()
        {
            colSpan = 1;
            rowSpan = 1;
            Text = "";
            HyperlinkValue = "";
        }

        #endregion // Constructors

        #region Public Methods

        /// <summary>
        /// Attaches the specified table cell.
        /// </summary>
        /// <param name="cell">The table cell instance.</param>
        /// <remarks>This method is called when we load the table.</remarks>
        public void AttachCell (TableCell cell)
        {
            if (this.OriginalCell != null)
            {
                this.OriginalCell.CellData = null;
                this.OriginalCell.Dispose();
            }

            Text = cell.Text;
            ColSpan = cell.ColSpan;
            RowSpan = cell.RowSpan;
            Objects = cell.Objects;
            Style = null;
            this.OriginalCell = cell;
            cell.CellData = this;
        }

        /// <summary>
        /// Assigns another <see cref="TableCellData"/> instance.
        /// </summary>
        /// <param name="source">The table cell data that used as a source.</param>
        /// <remarks>This method is called when we copy cells or clone columns/rows in a designer.</remarks>
        public void Assign (TableCellData source)
        {
            AttachCell (source.Cell);
        }

        /// <summary>
        /// Assigns another <see cref="TableCellData"/> instance at run time.
        /// </summary>
        /// <param name="cell">The table cell data that used as a source.</param>
        /// <param name="copyChildren">This flag shows should children be copied or not.</param>
        /// <remarks>This method is called when we print a table. We should create a copy of the cell and set the style.</remarks>
        public void RunTimeAssign (TableCell cell, bool copyChildren)
        {
            Text = cell.Text;
            Value = cell.Value;
            HyperlinkValue = cell.Hyperlink.Value;

            // don't copy ColSpan, RowSpan - they will be handled in the TableHelper.
            //ColSpan = cell.ColSpan;
            //RowSpan = cell.RowSpan;

            // clone objects
            Objects = null;
            if (cell.Objects != null && copyChildren)
            {
                Objects = new ReportComponentCollection();
                foreach (ReportComponentBase obj in cell.Objects)
                {
                    if (obj.Visible)
                    {
                        var cloneObj = Activator.CreateInstance (obj.GetType()) as ReportComponentBase;
                        cloneObj.AssignAll (obj);
                        cloneObj.Name = obj.Name;
                        Objects.Add (cloneObj);
                    }
                }
            }

            // add the cell to the style list. If the list contains such style,
            // return the existing style; in other case, create new style based
            // on the given cell.
            SetStyle (cell);

            // cell is used to reference the original cell. It is necessary to use Alias, OriginalComponent
            this.OriginalCell = cell;

            // reset object's size as if we set ColSpan and RowSpan to 1.
            // It is nesessary when printing spanned cells because the span of such cells will be corrected
            // when print new rows/columns and thus will move cell objects.
            if (Objects != null)
            {
                UpdateLayout (cell.Width, cell.Height, Width - cell.Width, Height - cell.Height);
            }
        }

        /// <summary>
        /// Sets style of the table cell.
        /// </summary>
        /// <param name="style">The new style of the table cell.</param>
        public void SetStyle (TableCell style)
        {
            this.Style = Table.Styles.Add (style);
        }

        /// <summary>
        /// Disposes the <see cref="TableCellData"/> instance.
        /// </summary>
        public void Dispose()
        {
            if (Style == null && OriginalCell != null)
            {
                OriginalCell.Dispose();
            }

            OriginalCell = null;
            Style = null;
        }

        /// <summary>
        /// Calculates width of the table cell.
        /// </summary>
        /// <returns>The value of the table cell width.</returns>
        public float CalcWidth()
        {
            var cell = Cell;
            cell.SetReport (Table.Report);
            return cell.CalcWidth();
        }

        /// <summary>
        /// Calculates height of the table cell.
        /// </summary>
        /// <param name="width">The width of the table cell.</param>
        /// <returns>The value of the table cell height.</returns>
        public float CalcHeight (float width)
        {
            var cell = Cell;
            cell.SetReport (Table.Report);
            cell.Width = width;
            var cellHeight = cell.CalcHeight();

            if (Objects != null)
            {
                // pasted from BandBase.cs

                // sort objects by Top
                var sortedObjects = Objects.SortByTop();

                // calc height of each object
                var heights = new float[sortedObjects.Count];
                for (var i = 0; i < sortedObjects.Count; i++)
                {
                    var obj = sortedObjects[i];
                    var height = obj.Height;
                    if (obj.CanGrow || obj.CanShrink)
                    {
                        var height1 = obj.CalcHeight();
                        if ((obj.CanGrow && height1 > height) || (obj.CanShrink && height1 < height))
                        {
                            height = height1;
                        }
                    }

                    heights[i] = height;
                }

                // calc shift amounts
                var shifts = new float[sortedObjects.Count];
                for (var i = 0; i < sortedObjects.Count; i++)
                {
                    var parent = sortedObjects[i];
                    var shift = heights[i] - parent.Height;
                    if (shift == 0)
                    {
                        continue;
                    }

                    for (var j = i + 1; j < sortedObjects.Count; j++)
                    {
                        var child = sortedObjects[j];
                        if (child.ShiftMode == ShiftMode.Never)
                        {
                            continue;
                        }

                        if (child.Top >= parent.Bottom - 1e-4)
                        {
                            if (child.ShiftMode == ShiftMode.WhenOverlapped &&
                                (child.Left > parent.Right - 1e-4 || parent.Left > child.Right - 1e-4))
                            {
                                continue;
                            }

                            var parentShift = shifts[i];
                            var childShift = shifts[j];
                            if (shift > 0)
                            {
                                childShift = Math.Max (shift + parentShift, childShift);
                            }
                            else
                            {
                                childShift = Math.Min (shift + parentShift, childShift);
                            }

                            shifts[j] = childShift;
                        }
                    }
                }

                // update location and size of each component, calc max height
                float maxHeight = 0;
                for (var i = 0; i < sortedObjects.Count; i++)
                {
                    var obj = sortedObjects[i];
                    obj.Height = heights[i];
                    obj.Top += shifts[i];
                    if (obj.Bottom > maxHeight)
                    {
                        maxHeight = obj.Bottom;
                    }
                }

                if (cellHeight < maxHeight)
                {
                    cellHeight = maxHeight;
                }

                // perform grow to bottom
                foreach (ReportComponentBase obj in Objects)
                {
                    if (obj.GrowToBottom)
                    {
                        obj.Height = cellHeight - obj.Top;
                    }
                }

                // -----------------------
            }

            return cellHeight;
        }

        internal void UpdateLayout (float dx, float dy)
        {
            UpdateLayout (Width, Height, dx, dy);
        }

        internal void UpdateLayout (float width, float height, float dx, float dy)
        {
            if (updatingLayout || Objects == null)
            {
                return;
            }

            updatingLayout = true;
            try
            {
                var remainingBounds = new RectangleF (0, 0, width, height);
                remainingBounds.Width += dx;
                remainingBounds.Height += dy;
                foreach (ReportComponentBase c in Objects)
                {
                    if ((c.Anchor & AnchorStyles.Right) != 0)
                    {
                        if ((c.Anchor & AnchorStyles.Left) != 0)
                        {
                            c.Width += dx;
                        }
                        else
                        {
                            c.Left += dx;
                        }
                    }
                    else if ((c.Anchor & AnchorStyles.Left) == 0)
                    {
                        c.Left += dx / 2;
                    }

                    if ((c.Anchor & AnchorStyles.Bottom) != 0)
                    {
                        if ((c.Anchor & AnchorStyles.Top) != 0)
                        {
                            c.Height += dy;
                        }
                        else
                        {
                            c.Top += dy;
                        }
                    }
                    else if ((c.Anchor & AnchorStyles.Top) == 0)
                    {
                        c.Top += dy / 2;
                    }

                    switch (c.Dock)
                    {
                        case DockStyle.Left:
                            c.Bounds = new RectangleF (remainingBounds.Left, remainingBounds.Top, c.Width,
                                remainingBounds.Height);
                            remainingBounds.X += c.Width;
                            remainingBounds.Width -= c.Width;
                            break;

                        case DockStyle.Top:
                            c.Bounds = new RectangleF (remainingBounds.Left, remainingBounds.Top, remainingBounds.Width,
                                c.Height);
                            remainingBounds.Y += c.Height;
                            remainingBounds.Height -= c.Height;
                            break;

                        case DockStyle.Right:
                            c.Bounds = new RectangleF (remainingBounds.Right - c.Width, remainingBounds.Top, c.Width,
                                remainingBounds.Height);
                            remainingBounds.Width -= c.Width;
                            break;

                        case DockStyle.Bottom:
                            c.Bounds = new RectangleF (remainingBounds.Left, remainingBounds.Bottom - c.Height,
                                remainingBounds.Width, c.Height);
                            remainingBounds.Height -= c.Height;
                            break;

                        case DockStyle.Fill:
                            c.Bounds = remainingBounds;
                            remainingBounds.Width = 0;
                            remainingBounds.Height = 0;
                            break;
                    }
                }
            }
            finally
            {
                updatingLayout = false;
            }
        }

        #endregion // Public Methods
    }
}
