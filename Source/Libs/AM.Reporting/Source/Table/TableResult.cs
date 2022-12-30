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
using System.Collections.Generic;
using System.Text;

using AM.Reporting.Engine;
using AM.Reporting.Preview;

using System.Drawing;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Table
{
    /// <summary>
    /// Represents a result table.
    /// </summary>
    /// <remarks>
    /// Do not use this class directly. It is used by the <see cref="TableObject"/> and
    /// <see cref="AM.Reporting.Matrix.MatrixObject"/> objects to render a result.
    /// </remarks>
    public class TableResult : TableBase
    {
        private bool isFirstRow;

        /// <summary>
        /// Occurs after calculation of table bounds.
        /// </summary>
        /// <remarks>
        /// You may use this event to change automatically calculated rows/column sizes. It may be useful
        /// if you need to fit dynamically printed table on a page.
        /// </remarks>
        public event EventHandler AfterCalcBounds;

        internal bool Skip { get; set; }

        internal List<TableRow> RowsToSerialize { get; }

        internal List<TableColumn> ColumnsToSerialize { get; }

        private float GetRowsHeight (int startRow, int count)
        {
            float height = 0;

            // include row header
            if (startRow != 0 && (RepeatHeaders || RepeatColumnHeaders))
            {
                for (var i = 0; i < FixedRows; i++)
                {
                    height += Rows[i].Height;
                }
            }

            for (var i = 0; i < count; i++)
            {
                height += Rows[startRow + i].Height;
            }

            return height;
        }

        private float GetColumnsWidth (int startColumn, int count)
        {
            float width = 0;

            // include column header
            if (startColumn != 0 && (RepeatHeaders || RepeatRowHeaders))
            {
                for (var i = 0; i < FixedColumns; i++)
                {
                    width += Columns[i].Width;
                }
            }

            for (var i = 0; i < count; i++)
            {
                if (i == count - 1)
                {
                    width += Math.Max (Columns[startColumn + i].Width, Columns[startColumn + i].MinimumBreakWidth);
                }
                else
                {
                    width += Columns[startColumn + i].Width;
                }
            }

            return width;
        }

        private int GetRowsFit (int startRow, float freeSpace)
        {
            var rowsFit = 0;
            var rowsToKeep = 0;
            var rowsKept = 0;
            var saveRowsFit = 0;
            var keeping = false;

            while (startRow + rowsFit < Rows.Count &&
                   (rowsFit == 0 || !Rows[startRow + rowsFit].PageBreak) &&
                   (!CanBreak | GetRowsHeight (startRow, rowsFit + 1) <= freeSpace + 0.1f))
            {
                if (keeping)
                {
                    rowsKept++;
                    if (rowsKept >= rowsToKeep)
                    {
                        keeping = false;
                    }
                }
                else if (Rows[startRow + rowsFit].KeepRows > 1)
                {
                    rowsToKeep = Rows[startRow + rowsFit].KeepRows;
                    rowsKept = 1;
                    saveRowsFit = rowsFit;
                    keeping = true;
                }

                rowsFit++;
            }

            if (keeping)
            {
                rowsFit = saveRowsFit;
            }

            // case if the row header does not fit on a page (at the start of table)
            if (startRow == 0 && rowsFit < FixedRows)
            {
                rowsFit = 0;
            }

            return rowsFit;
        }

        private int GetColumnsFit (int startColumn, float freeSpace)
        {
            var columnsFit = 0;
            var columnsToKeep = 0;
            var columnsKept = 0;
            var saveColumnsFit = 0;
            var keeping = false;

            while (startColumn + columnsFit < Columns.Count &&
                   (columnsFit == 0 || !Columns[startColumn + columnsFit].PageBreak) &&
                   GetColumnsWidth (startColumn, columnsFit + 1) <= freeSpace + 0.1f)
            {
                if (keeping)
                {
                    columnsKept++;
                    if (columnsKept >= columnsToKeep)
                    {
                        keeping = false;
                    }
                }
                else if (Columns[startColumn + columnsFit].KeepColumns > 1)
                {
                    columnsToKeep = Columns[startColumn + columnsFit].KeepColumns;
                    columnsKept = 1;
                    saveColumnsFit = columnsFit;
                    keeping = true;
                }

                columnsFit++;
            }

            if (keeping)
            {
                columnsFit = saveColumnsFit;
            }

            return columnsFit;
        }

        private void ProcessDuplicates (TableCell cell, int startX, int startY, List<Rectangle> list)
        {
            var cellAlias = cell.Alias;
            var cellData = cell.CellData;
            var cellText = cell.Text;
            var cellDuplicates = cell.CellDuplicates;

            Func<int, int> func = (row) =>
            {
                var span = 0;
                for (var x = startX; x < ColumnCount; x++)
                {
                    var c = this[x, row];
                    if (IsInsideSpan (c, list))
                    {
                        break;
                    }

                    if (c.Alias == cellAlias)
                    {
                        if (c.Text == cellText)
                        {
                            span++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                return span;
            };

            var colSpan = func (startY);
            var rowSpan = 1;
            for (var y = startY + 1; y < RowCount; y++)
            {
                var span = func (y);
                if (span < cellData.ColSpan)
                {
                    break;
                }

                rowSpan++;
            }

            if (cellDuplicates == CellDuplicates.Clear)
            {
                for (var x = 0; x < colSpan; x++)
                for (var y = 0; y < rowSpan; y++)
                    if (!(x == 0 && y == 0))
                    {
                        GetCellData (x + startX, y + startY).Text = "";
                    }
            }
            else if (cellDuplicates == CellDuplicates.Merge ||
                     (cellDuplicates == CellDuplicates.MergeNonEmpty && !string.IsNullOrEmpty (cellText)))
            {
                cellData.ColSpan = colSpan;
                cellData.RowSpan = rowSpan;
            }

            list.Add (new Rectangle (startX, startY, colSpan, rowSpan));
        }

        private bool IsInsideSpan (TableCell cell, List<Rectangle> list)
        {
            var address = cell.Address;
            foreach (var span in list)
            {
                if (span.Contains (address))
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessDuplicates()
        {
            var list = new List<Rectangle>();
            for (var x = 0; x < ColumnCount; x++)
            {
                for (var y = 0; y < RowCount; y++)
                {
                    var cell = this[x, y];
                    if (cell.CellDuplicates != CellDuplicates.Show && !IsInsideSpan (cell, list))
                    {
                        ProcessDuplicates (cell, x, y, list);
                    }
                }
            }
        }

        internal void GeneratePages (object sender, EventArgs e)
        {
            isFirstRow = false;
            if (Skip)
            {
                Skip = false;
                return;
            }


            // check if band contains several tables
            if (sender is BandBase senderBand)
            {
                isFirstRow = senderBand.IsFirstRow;
                SortedList<float, TableBase> tables = new SortedList<float, TableBase>();
                foreach (Base obj in senderBand.Objects)
                {
                    if (obj is TableBase table && table.ResultTable != null)
                    {
                        try
                        {
                            tables.Add (table.Left, table);
                        }
                        catch (ArgumentException)
                        {
                            throw new ArgumentException (Res.Get ("Messages,MatrixLayoutError"));
                        }
                    }
                }

                // render tables side-by-side
                if (tables.Count > 1)
                {
                    var engine = Report.Engine;
                    var info = new TableLayoutInfo
                    {
                        startPage = engine.CurPage,
                        tableSize = new Size (1, 1),
                        startX = tables.Values[0].Left
                    };

                    var startPage = info.startPage;
                    var saveCurY = engine.CurY;
                    var maxPage = 0;
                    float maxCurY = 0;

                    for (var i = 0; i < tables.Count; i++)
                    {
                        var table = tables.Values[i];

                        // do not allow table to render itself in the band.AfterPrint event
                        table.ResultTable.Skip = true;

                        // render using the down-then-across mode
                        table.Layout = TableLayout.DownThenAcross;

                        engine.CurPage = info.startPage + (info.tableSize.Width - 1) * info.tableSize.Height;
                        engine.CurY = saveCurY;
                        float addLeft = 0;
                        if (i > 0)
                        {
                            addLeft = table.Left - tables.Values[i - 1].Right;
                        }

                        table.ResultTable.Left = info.startX + addLeft;

                        // calculate cells' bounds
                        table.ResultTable.CalcBounds();

                        // generate pages
                        Report.PreparedPages.AddPageAction = AddPageAction.WriteOver;
                        info = table.ResultTable.GeneratePagesDownThenAcross();

                        if (engine.CurPage > maxPage)
                        {
                            maxPage = engine.CurPage;
                            maxCurY = engine.CurY;
                        }
                        else if (engine.CurPage == maxPage && engine.CurY > maxCurY)
                        {
                            maxCurY = engine.CurY;
                        }
                    }

                    engine.CurPage = maxPage;
                    engine.CurY = maxCurY;

                    Skip = false;
                    return;
                }
            }

            // calculate cells' bounds
            CalcBounds();

            // manage duplicates
            ProcessDuplicates();

            if (Report.Engine.UnlimitedHeight || Report.Engine.UnlimitedWidth)
            {
                if (!Report.Engine.UnlimitedWidth)
                {
                    GeneratePagesWrapped();
                }
                else if (!Report.Engine.UnlimitedHeight)
                {
                    GeneratePagesDownThenAcross();
                }
                else
                {
                    GeneratePagesAcrossThenDown();
                }
            }
            else if (Layout == TableLayout.AcrossThenDown)
            {
                GeneratePagesAcrossThenDown();
            }
            else if (Layout == TableLayout.DownThenAcross)
            {
                GeneratePagesDownThenAcross();
            }
            else
            {
                GeneratePagesWrapped();
            }
        }

        internal void AddToParent (Base parent)
        {
            // calculate cells' bounds
            CalcBounds();

            // manage duplicates
            ProcessDuplicates();

            // copy everything to regular table because TableResult is not suitable for this
            var cloneTable = new TableBase();
            cloneTable.Assign (this);

            foreach (TableColumn c in Columns)
            {
                var cloneColumn = new TableColumn();
                cloneColumn.Assign (c);
                cloneTable.Columns.Add (cloneColumn);
            }

            foreach (TableRow r in Rows)
            {
                var cloneRow = new TableRow();
                cloneRow.Assign (r);
                cloneTable.Rows.Add (cloneRow);

                for (var columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    var cloneCell = new TableCell();

                    // this is the point why we have to do the cloning manually. r[columnIndex] may return shared instance of TableCell.
                    cloneCell.AssignAll (r[columnIndex]);
                    cloneCell.Parent = cloneRow;
                }
            }

            cloneTable.Parent = parent;
        }

        internal void CalcBounds()
        {
            // allow row/column manipulation from a script
            LockCorrectSpans = false;

            // fire AfterData event
            OnAfterData();

            // calculate cells' bounds
            Height = CalcHeight();

            // fire AfterCalcBounds event
            OnAfterCalcBounds();
        }

        private void OnAfterCalcBounds()
        {
            if (AfterCalcBounds != null)
            {
                AfterCalcBounds (this, EventArgs.Empty);
            }
        }

        private void GeneratePagesAcrossThenDown()
        {
            var engine = Report.Engine;
            var preparedPages = Report.PreparedPages;
            preparedPages.CanUploadToCache = false;
            preparedPages.AddPageAction = AddPageAction.WriteOver;

            var spans = GetSpanList();
            var startRow = 0;
            var addNewPage = false;
            var freeSpace = engine.FreeSpace;
            Top = 0;

            while (startRow < Rows.Count)
            {
                if (addNewPage)
                {
                    engine.StartNewPage();
                    freeSpace = engine.FreeSpace;
                }

                var startColumn = 0;
                var rowsFit = GetRowsFit (startRow, freeSpace);
                if (startRow == 0 && engine.IsKeeping && rowsFit < RowCount && isFirstRow && engine.KeepCurY > 0)
                {
                    engine.EndColumn();
                    freeSpace = engine.FreeSpace;
                    rowsFit = GetRowsFit (startRow, freeSpace);
                }

                // avoid the infinite loop if there is not enough space for one row
                if (rowsFit == 0)
                {
                    rowsFit = 1;
                }

                var saveCurPage = engine.CurPage;
                var saveLeft = Left;
                var saveCurY = engine.CurY;
                var curY = engine.CurY;

                if (rowsFit != 0)
                {
                    while (startColumn < Columns.Count)
                    {
                        var columnsFit = GetColumnsFit (startColumn, engine.PageWidth - Left);

                        // avoid the infinite loop if there is not enough space for one column
                        if (startColumn > 0 && columnsFit == 0)
                        {
                            columnsFit = 1;
                        }

                        engine.CurY = saveCurY;
                        curY = GeneratePage (startColumn, startRow, columnsFit, rowsFit,
                            new RectangleF (0, 0, engine.PageWidth, CanBreak ? freeSpace : Height), spans) + saveCurY;

                        Left = 0;
                        startColumn += columnsFit;
                        if (startColumn < Columns.Count)
                        {
                            // if we have something to print, start a new page
                            engine.StartNewPage();
                        }
                        else if (engine.CurPage > saveCurPage)
                        {
                            // finish the last printed page in case it is not the start page
                            engine.EndPage (false);
                        }

                        if (Report.Aborted)
                        {
                            break;
                        }
                    }
                }

                startRow += rowsFit;
                Left = saveLeft;
                engine.CurPage = saveCurPage;
                engine.CurY = curY;
                preparedPages.AddPageAction = AddPageAction.Add;
                addNewPage = true;

                if (Report.Aborted)
                {
                    break;
                }
            }
        }

        private TableLayoutInfo GeneratePagesDownThenAcross()
        {
            var engine = Report.Engine;
            var preparedPages = Report.PreparedPages;
            preparedPages.CanUploadToCache = false;

            var info = new TableLayoutInfo
            {
                startPage = engine.CurPage
            };
            var spans = GetSpanList();
            var startColumn = 0;
            var addNewPage = false;
            var saveCurY = engine.CurY;
            float lastCurY = 0;
            var lastPage = 0;
            Top = 0;

            while (startColumn < Columns.Count)
            {
                if (addNewPage)
                {
                    engine.StartNewPage();
                }

                var startRow = 0;
                var columnsFit = GetColumnsFit (startColumn, engine.PageWidth - Left);

                // avoid the infinite loop if there is not enough space for one column
                if (startColumn > 0 && columnsFit == 0)
                {
                    columnsFit = 1;
                }

                engine.CurY = saveCurY;
                info.tableSize.Width++;
                info.tableSize.Height = 0;

                if (columnsFit > 0)
                {
                    while (startRow < Rows.Count)
                    {
                        var rowsFit = GetRowsFit (startRow, engine.FreeSpace);
                        if (startRow == 0 && engine.IsKeeping && rowsFit < RowCount && isFirstRow &&
                            engine.KeepCurY > 0)
                        {
                            engine.EndColumn();

                            rowsFit = GetRowsFit (startRow, engine.FreeSpace);
                        }

                        // avoid the infinite loop if there is not enough space for one row
                        if (startRow > 0 && rowsFit == 0)
                        {
                            rowsFit = 1;
                        }

                        engine.CurY += GeneratePage (startColumn, startRow, columnsFit, rowsFit,
                            new RectangleF (0, 0, engine.PageWidth, engine.FreeSpace), spans);
                        info.tableSize.Height++;

                        startRow += rowsFit;
                        if (startRow < Rows.Count)
                        {
                            // if we have something to print, start a new page
                            engine.StartNewPage();
                        }
                        else if (startColumn > 0)
                        {
                            // finish the last printed page in case it is not a start page
                            engine.EndPage (false);
                        }

                        if (Report.Aborted)
                        {
                            break;
                        }
                    }
                }

                info.startX = Left + GetColumnsWidth (startColumn, columnsFit);
                startColumn += columnsFit;
                Left = 0;
                preparedPages.AddPageAction = AddPageAction.Add;
                addNewPage = true;

                if (lastPage == 0)
                {
                    lastPage = engine.CurPage;
                    lastCurY = engine.CurY;
                }

                if (Report.Aborted)
                {
                    break;
                }
            }

            engine.CurPage = lastPage;
            engine.CurY = lastCurY;
            return info;
        }

        private void GeneratePagesWrapped()
        {
            var engine = Report.Engine;
            var preparedPages = Report.PreparedPages;
            preparedPages.CanUploadToCache = false;

            var spans = GetSpanList();
            var startColumn = 0;
            Top = 0;

            while (startColumn < Columns.Count)
            {
                var startRow = 0;
                var columnsFit = GetColumnsFit (startColumn, engine.PageWidth - Left);

                // avoid the infinite loop if there is not enough space for one column
                if (startColumn > 0 && columnsFit == 0)
                {
                    columnsFit = 1;
                }

                while (startRow < Rows.Count)
                {
                    var rowsFit = GetRowsFit (startRow, engine.FreeSpace);
                    if (startRow == 0 && engine.IsKeeping && rowsFit < RowCount && isFirstRow && engine.KeepCurY > 0)
                    {
                        engine.EndColumn();

                        rowsFit = GetRowsFit (startRow, engine.FreeSpace);
                    }

                    if (rowsFit == 0)
                    {
                        engine.StartNewPage();
                        rowsFit = GetRowsFit (startRow, engine.FreeSpace);
                    }

                    engine.CurY += GeneratePage (startColumn, startRow, columnsFit, rowsFit,
                        new RectangleF (0, 0, engine.PageWidth, engine.FreeSpace), spans);

                    startRow += rowsFit;

                    if (Report.Aborted)
                    {
                        break;
                    }
                }

                startColumn += columnsFit;
                if (startColumn < Columns.Count)
                {
                    engine.CurY += WrappedGap;
                }

                if (Report.Aborted)
                {
                    break;
                }
            }
        }

        private void AdjustSpannedCellWidth (TableCellData cell)
        {
            if (!AdjustSpannedCellsWidth)
            {
                return;
            }

            // check that spanned cell has enough width
            float columnsWidth = 0;
            for (var i = 0; i < cell.ColSpan; i++)
            {
                columnsWidth += Columns[cell.Address.X + i].Width;
            }

            // if cell is bigger than sum of column width, increase the last column width
            var cellWidth = cell.CalcWidth();
            if (columnsWidth < cellWidth)
            {
                Columns[cell.Address.X + cell.ColSpan - 1].Width += cellWidth - columnsWidth;
            }
        }

        private float GeneratePage (int startColumn, int startRow, int columnsFit, int rowsFit,
            RectangleF bounds, List<Rectangle> spans)
        {
            // break spans
            foreach (var span in spans)
            {
                var spannedCell = GetCellData (span.Left, span.Top);
                TableCellData newSpannedCell = null;
                if (span.Left < startColumn && span.Right > startColumn)
                {
                    if ((RepeatHeaders || RepeatRowHeaders) && span.Left < FixedColumns)
                    {
                        spannedCell.ColSpan =
                            Math.Min (span.Right, startColumn + columnsFit) - startColumn + FixedColumns;
                    }
                    else
                    {
                        newSpannedCell = GetCellData (startColumn, span.Top);
                        newSpannedCell.RunTimeAssign (spannedCell.Cell, true);
                        newSpannedCell.ColSpan = Math.Min (span.Right, startColumn + columnsFit) - startColumn;
                        newSpannedCell.RowSpan = spannedCell.RowSpan;
                        AdjustSpannedCellWidth (newSpannedCell);
                    }
                }

                if (span.Left < startColumn + columnsFit && span.Right > startColumn + columnsFit)
                {
                    spannedCell.ColSpan = startColumn + columnsFit - span.Left;
                    AdjustSpannedCellWidth (spannedCell);
                }

                if (span.Top < startRow && span.Bottom > startRow)
                {
                    if ((RepeatHeaders || RepeatColumnHeaders) && span.Top < FixedRows)
                    {
                        spannedCell.RowSpan = Math.Min (span.Bottom, startRow + rowsFit) - startRow + FixedRows;
                    }
                }

                if (span.Top < startRow + rowsFit && span.Bottom > startRow + rowsFit)
                {
                    spannedCell.RowSpan = startRow + rowsFit - span.Top;

                    newSpannedCell = GetCellData (span.Left, startRow + rowsFit);
                    newSpannedCell.RunTimeAssign (spannedCell.Cell, true);
                    newSpannedCell.ColSpan = spannedCell.ColSpan;
                    newSpannedCell.RowSpan = span.Bottom - (startRow + rowsFit);

                    // break the cell text
                    var cell = spannedCell.Cell;
                    using (var tempObject = new TextObject())
                    {
                        if (!cell.Break (tempObject))
                        {
                            cell.Text = "";
                        }

                        if (cell.CanBreak)
                        {
                            newSpannedCell.Text = tempObject.Text;
                        }
                    }

                    // fix the row height
                    var textHeight = newSpannedCell.Cell.CalcHeight();
                    float rowsHeight = 0;
                    for (var i = 0; i < newSpannedCell.RowSpan; i++)
                    {
                        rowsHeight += Rows[i + startRow + rowsFit].Height;
                    }

                    if (rowsHeight < textHeight)
                    {
                        // fix the last row's height
                        Rows[startRow + rowsFit + newSpannedCell.RowSpan - 1].Height += textHeight - rowsHeight;
                    }
                }
            }

            // set visible columns
            ColumnsToSerialize.Clear();
            if (RepeatHeaders || RepeatRowHeaders)
            {
                for (var i = 0; i < FixedColumns; i++)
                {
                    if (Columns[i].Visible)
                    {
                        ColumnsToSerialize.Add (Columns[i]);
                    }
                }

                if (startColumn < FixedColumns)
                {
                    columnsFit -= FixedColumns - startColumn;
                    startColumn = FixedColumns;
                }
            }

            // calc visible columns and last X coordinate of table for unlimited page width
            var tableEndX = Columns[0].Width;
            for (var i = startColumn; i < startColumn + columnsFit; i++)
            {
                if (Columns[i].Visible)
                {
                    ColumnsToSerialize.Add (Columns[i]);
                    tableEndX += Columns[i].Width;
                }
            }

            // set visible rows
            RowsToSerialize.Clear();
            if (RepeatHeaders || RepeatColumnHeaders)
            {
                for (var i = 0; i < FixedRows; i++)
                {
                    if (Rows[i].Visible)
                    {
                        RowsToSerialize.Add (Rows[i]);
                    }
                }

                if (startRow < FixedRows)
                {
                    rowsFit -= FixedRows - startRow;
                    startRow = FixedRows;
                }
            }

            // calc visible rows and last Y coordinate of table for unlimited page height
            var tableEndY = Rows[0].Top;
            for (var i = startRow; i < startRow + rowsFit; i++)
            {
                if (Rows[i].Visible)
                {
                    RowsToSerialize.Add (Rows[i]);
                    tableEndY += Rows[i].Height;
                }
            }

            // include row header
            if (startRow != 0 && (RepeatHeaders || RepeatColumnHeaders))
            {
                for (var i = 0; i < FixedRows; i++)
                {
                    tableEndY += Rows[i].Height;
                }
            }


            // generate unlimited page
            if (Report.Engine.UnlimitedHeight || Report.Engine.UnlimitedWidth)
            {
                if (Report.Engine.UnlimitedHeight)
                {
                    bounds.Height = tableEndY;
                }

                if (Report.Engine.UnlimitedWidth)
                {
                    bounds.Width = tableEndX;
                }
            }

            var band = new DataBand();
            band.Bounds = bounds;
            band.Objects.Add (this);
            Report.Engine.AddToPreparedPages (band);

            return GetRowsHeight (startRow, rowsFit);
        }

        /// <inheritdoc/>
        protected override void Dispose (bool disposing)
        {
            LockCorrectSpans = true;
            base.Dispose (disposing);
        }

        /// <inheritdoc/>
        public override void GetChildObjects (ObjectCollection list)
        {
            foreach (var column in ColumnsToSerialize)
            {
                list.Add (column);
            }

            foreach (var row in RowsToSerialize)
            {
                list.Add (row);
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="TableResult"/> class.
        /// </summary>
        public TableResult()
        {
            LockCorrectSpans = true;
            RowsToSerialize = new List<TableRow>();
            ColumnsToSerialize = new List<TableColumn>();
        }


        private class TableLayoutInfo
        {
            public int startPage;
            public Size tableSize;
            public float startX;
        }
    }
}
