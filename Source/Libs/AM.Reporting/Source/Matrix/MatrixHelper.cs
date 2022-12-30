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

using AM.Reporting.Data;
using AM.Reporting.Table;
using AM.Reporting.Utils;

using System.Drawing;
using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Matrix
{
    internal class MatrixHelper
    {
        private bool designTime;
        private TableResult resultTable;
        private MatrixDescriptor titleDescriptor;
        private MatrixDescriptor cellHeaderDescriptor;
        private MatrixHeaderDescriptor noColumnsDescriptor;
        private MatrixHeaderDescriptor noRowsDescriptor;
        private MatrixCellDescriptor noCellsDescriptor;
        private int[] evenStyleIndices;
        private bool noColumns;
        private bool noRows;

        #region Properties

        public MatrixObject Matrix { get; }

        public Report Report => Matrix.Report;

        public TableResult ResultTable => designTime ? resultTable : Matrix.ResultTable;

        public int HeaderHeight { get; private set; }

        public int HeaderWidth { get; private set; }

        public int BodyWidth { get; private set; }

        public int BodyHeight { get; private set; }

        public object[] CellValues { get; private set; }

        #endregion

        #region Private Methods

        private string ExtractColumnName (string complexName)
        {
            if (complexName.StartsWith ("[") && complexName.EndsWith ("]"))
            {
                complexName = complexName.Substring (1, complexName.Length - 2);
            }

            if (Report == null)
            {
                return complexName;
            }

            var column = DataHelper.GetColumn (Report.Dictionary, complexName);
            if (column == null)
            {
                return complexName;
            }

            return column.Alias;
        }

        /// <summary>
        /// Updates HeaderWidth, HeaderHeight, BodyWidth, BodyHeight properties.
        /// </summary>
        private void UpdateTemplateSizes()
        {
            HeaderWidth = Matrix.Data.Rows.Count;
            if (HeaderWidth == 0)
            {
                HeaderWidth = 1;
            }

            if (Matrix.Data.Cells.Count > 1 && !Matrix.CellsSideBySide)
            {
                HeaderWidth++;
            }

            HeaderHeight = Matrix.Data.Columns.Count;
            if (HeaderHeight == 0)
            {
                HeaderHeight = 1;
            }

            if (Matrix.Data.Cells.Count > 1 && Matrix.CellsSideBySide)
            {
                HeaderHeight++;
            }

            if (Matrix.ShowTitle)
            {
                HeaderHeight++;
            }

            BodyWidth = 1;
            foreach (MatrixHeaderDescriptor descr in Matrix.Data.Columns)
            {
                if (descr.Totals)
                {
                    BodyWidth++;
                }
            }

            if (Matrix.CellsSideBySide && Matrix.Data.Cells.Count > 1)
            {
                BodyWidth *= Matrix.Data.Cells.Count;
            }

            BodyHeight = 1;
            foreach (MatrixHeaderDescriptor descr in Matrix.Data.Rows)
            {
                if (descr.Totals)
                {
                    BodyHeight++;
                }
            }

            if (!Matrix.CellsSideBySide && Matrix.Data.Cells.Count > 1)
            {
                BodyHeight *= Matrix.Data.Cells.Count;
            }
        }

        private void UpdateColumnDescriptors()
        {
            var left = HeaderWidth;
            var top = 0;
            var width = BodyWidth;
            var dataWidth = 1;

            if (Matrix.Data.Cells.Count > 1 && Matrix.CellsSideBySide)
            {
                dataWidth = Matrix.Data.Cells.Count;
            }

            if (Matrix.ShowTitle)
            {
                top++;
            }

            foreach (MatrixHeaderDescriptor descr in Matrix.Data.Columns)
            {
                if (descr.Totals)
                {
                    descr.TemplateTotalColumn = Matrix.Columns[left + width - dataWidth];
                    descr.TemplateTotalRow = Matrix.Rows[top];
                    descr.TemplateTotalCell = Matrix[left + width - dataWidth, top];
                    width -= dataWidth;
                }
                else
                {
                    descr.TemplateTotalColumn = null;
                    descr.TemplateTotalRow = null;
                    descr.TemplateTotalCell = null;
                }

                descr.TemplateColumn = Matrix.Columns[left];
                descr.TemplateRow = Matrix.Rows[top];
                descr.TemplateCell = Matrix[left, top];
                top++;
            }
        }

        private void UpdateRowDescriptors()
        {
            var left = 0;
            var top = HeaderHeight;
            var height = BodyHeight;
            var dataHeight = 1;

            if (Matrix.Data.Cells.Count > 1 && !Matrix.CellsSideBySide)
            {
                dataHeight = Matrix.Data.Cells.Count;
            }

            foreach (MatrixHeaderDescriptor descr in Matrix.Data.Rows)
            {
                if (descr.Totals)
                {
                    descr.TemplateTotalColumn = Matrix.Columns[left];
                    descr.TemplateTotalRow = Matrix.Rows[top + height - dataHeight];
                    descr.TemplateTotalCell = Matrix[left, top + height - dataHeight];
                    height -= dataHeight;
                }
                else
                {
                    descr.TemplateTotalColumn = null;
                    descr.TemplateTotalRow = null;
                    descr.TemplateTotalCell = null;
                }

                descr.TemplateColumn = Matrix.Columns[left];
                descr.TemplateRow = Matrix.Rows[top];
                descr.TemplateCell = Matrix[left, top];
                left++;
            }
        }

        private void UpdateCellDescriptors()
        {
            var x = HeaderWidth;
            var y = HeaderHeight;

            foreach (MatrixCellDescriptor descr in Matrix.Data.Cells)
            {
                descr.TemplateColumn = Matrix.Columns[x];
                descr.TemplateRow = Matrix.Rows[y];
                descr.TemplateCell = Matrix[x, y];

                if (Matrix.CellsSideBySide)
                {
                    x++;
                }
                else
                {
                    y++;
                }
            }
        }

        private void UpdateOtherDescriptors()
        {
            titleDescriptor.TemplateColumn = null;
            titleDescriptor.TemplateRow = null;
            titleDescriptor.TemplateCell = null;
            if (Matrix.ShowTitle)
            {
                titleDescriptor.TemplateColumn = Matrix.Columns[HeaderWidth];
                titleDescriptor.TemplateRow = Matrix.Rows[0];
                titleDescriptor.TemplateCell = Matrix[HeaderWidth, 0];
            }

            cellHeaderDescriptor.TemplateColumn = null;
            cellHeaderDescriptor.TemplateRow = null;
            cellHeaderDescriptor.TemplateCell = null;
            if (Matrix.Data.Cells.Count > 1)
            {
                if (Matrix.CellsSideBySide)
                {
                    cellHeaderDescriptor.TemplateColumn = Matrix.Columns[HeaderWidth];
                    cellHeaderDescriptor.TemplateRow = Matrix.Rows[HeaderHeight - 1];
                }
                else
                {
                    cellHeaderDescriptor.TemplateColumn = Matrix.Columns[HeaderWidth - 1];
                    cellHeaderDescriptor.TemplateRow = Matrix.Rows[HeaderHeight];
                }
            }
        }

        private void ApplyStyle (TableCell cell, string styleName)
        {
            Style style = null;
            var styleIndex = Matrix.StyleSheet.IndexOf (Matrix.Style);
            if (styleIndex != -1)
            {
                var styles = Matrix.StyleSheet[styleIndex];
                style = styles[styles.IndexOf (styleName)];
            }

            if (style != null)
            {
                cell.ApplyStyle (style);
            }
        }

        private TableCell CreateCell (string text)
        {
            var cell = new TableCell();
            cell.Font = DrawUtils.DefaultReportFont;
            cell.Text = text;
            cell.HorzAlign = HorzAlign.Center;
            cell.VertAlign = VertAlign.Center;
            ApplyStyle (cell, "Header");
            return cell;
        }

        private TableCell CreateDataCell()
        {
            var cell = new TableCell();
            cell.Font = DrawUtils.DefaultReportFont;
            cell.HorzAlign = HorzAlign.Right;
            cell.VertAlign = VertAlign.Center;
            ApplyStyle (cell, "Body");
            return cell;
        }

        private void SetHint (TableCell cell, string text)
        {
            cell.Assign (Matrix.Styles.DefaultStyle);
            cell.Text = text;
            cell.Font = DrawUtils.DefaultReportFont;
            cell.TextFill = new SolidFill (Color.Gray);
            cell.HorzAlign = HorzAlign.Center;
            cell.VertAlign = VertAlign.Center;
            cell.SetFlags (Flags.CanEdit, false);
        }

        private Point GetBodyLocation()
        {
            // determine the template's body location. Do not rely on HeaderWidth, HeaderHeight -
            // the template may be empty
            var result = new Point();

            foreach (MatrixHeaderDescriptor descr in Matrix.Data.Columns)
            {
                if (descr.TemplateColumn != null)
                {
                    result.X = descr.TemplateColumn.Index;
                }
            }

            foreach (MatrixHeaderDescriptor descr in Matrix.Data.Rows)
            {
                if (descr.TemplateRow != null)
                {
                    result.Y = descr.TemplateRow.Index;
                }
            }

            return result;
        }

        private void InitResultTable (bool isTemplate)
        {
            Matrix.Data.Columns.AddTotalItems (isTemplate);
            Matrix.Data.Rows.AddTotalItems (isTemplate);
            List<MatrixHeaderItem> columnTerminalItems = Matrix.Data.Columns.RootItem.GetTerminalItems();
            List<MatrixHeaderItem> rowTerminalItems = Matrix.Data.Rows.RootItem.GetTerminalItems();

            // create corner
            List<MatrixDescriptor> descrList = new List<MatrixDescriptor>();

            if (Matrix.ShowTitle)
            {
                descrList.Add (titleDescriptor);
            }

            descrList.AddRange (Matrix.Data.Columns.ToArray());
            if (Matrix.Data.Cells.Count > 1 && Matrix.CellsSideBySide)
            {
                descrList.Add (cellHeaderDescriptor);
            }

            foreach (var descr in descrList)
            {
                var row = new TableRow();
                if (descr.TemplateRow != null)
                {
                    row.Assign (descr.TemplateRow);
                }

                ResultTable.Rows.Add (row);
            }

            descrList.Clear();
            descrList.AddRange (Matrix.Data.Rows.ToArray());
            if (Matrix.Data.Cells.Count > 1 && !Matrix.CellsSideBySide)
            {
                descrList.Add (cellHeaderDescriptor);
            }

            foreach (var descr in descrList)
            {
                var column = new TableColumn();
                if (descr.TemplateColumn != null)
                {
                    column.Assign (descr.TemplateColumn);
                }

                ResultTable.Columns.Add (column);
            }

            // determine the body location
            var bodyLocation = GetBodyLocation();

            // create columns
            foreach (var item in columnTerminalItems)
            {
                foreach (MatrixCellDescriptor descr in Matrix.Data.Cells)
                {
                    var column = new TableColumn();
                    if (item.TemplateColumn != null && descr.TemplateColumn != null)
                    {
                        column.Assign (
                            Matrix.Columns[item.TemplateColumn.Index + (descr.TemplateColumn.Index - bodyLocation.X)]);
                    }

                    ResultTable.Columns.Add (column);

                    if (!Matrix.CellsSideBySide)
                    {
                        break;
                    }
                }
            }

            // create rows
            foreach (var item in rowTerminalItems)
            {
                foreach (MatrixCellDescriptor descr in Matrix.Data.Cells)
                {
                    var row = new TableRow();
                    if (item.TemplateRow != null && descr.TemplateRow != null)
                    {
                        row.Assign (Matrix.Rows[item.TemplateRow.Index + (descr.TemplateRow.Index - bodyLocation.Y)]);
                    }

                    ResultTable.Rows.Add (row);

                    if (Matrix.CellsSideBySide)
                    {
                        break;
                    }
                }
            }
        }

        private void PrintHeaderCell (MatrixHeaderItem item, TableCellData resultCell, bool isEven)
        {
            var templateCell = item.TemplateCell;
            if (templateCell != null)
            {
                if (designTime)
                {
                    if (!item.IsTotal)
                    {
                        templateCell.Text = item.Value.ToString();
                    }

                    resultCell.RunTimeAssign (templateCell, true);
                }
                else
                {
                    if (Matrix.DataSource != null)
                    {
                        Matrix.DataSource.CurrentRowNo = item.DataRowNo;
                    }

                    templateCell.SetValue (item.Value);
                    if (!item.IsTotal)
                    {
                        templateCell.Text = templateCell.Format.FormatValue (item.Value);
                    }

                    templateCell.SaveState();
                    if (isEven)
                    {
                        templateCell.ApplyEvenStyle();
                    }

                    templateCell.GetData();
                    if (string.IsNullOrEmpty (templateCell.Hyperlink.Expression) &&
                        (templateCell.Hyperlink.Kind == HyperlinkKind.DetailReport ||
                         templateCell.Hyperlink.Kind == HyperlinkKind.DetailPage ||
                         templateCell.Hyperlink.Kind == HyperlinkKind.Custom))
                    {
                        templateCell.Hyperlink.Value = templateCell.Text;
                    }

                    resultCell.RunTimeAssign (templateCell, true);
                    templateCell.RestoreState();
                }
            }
            else
            {
                templateCell =
                    CreateCell (item.IsTotal ? Res.Get ("ComponentsMisc,Matrix,Total") : item.Value.ToString());
                resultCell.RunTimeAssign (templateCell, true);
            }
        }

        private void PrintColumnHeader (MatrixHeaderItem root, int left, int top, int level)
        {
            var dataWidth = 1;
            var height = HeaderHeight;
            if (Matrix.Data.Cells.Count > 1 && Matrix.CellsSideBySide)
            {
                dataWidth = Matrix.Data.Cells.Count;
                height--;
            }

            foreach (var item in root.Items)
            {
                Matrix.ColumnValues = item.Values;
                var resultCell = ResultTable.GetCellData (left, top);
                var span = item.Span * dataWidth;

                var dy = 1;
                if (item.IsTotal)
                {
                    dy = height - top;

                    // correct evenStyleIndices
                    for (var i = level + 1; i < evenStyleIndices.Length; i++)
                        evenStyleIndices[i]++;
                }

                PrintHeaderCell (item, resultCell, evenStyleIndices[level] % 2 != 0);
                PrintColumnHeader (item, left, top + dy, level + 1);

                if (item.PageBreak && left > HeaderWidth)
                {
                    ResultTable.Columns[left].PageBreak = true;
                }

                left += span;
                evenStyleIndices[level]++;

                // set spans after we have printed the cell. This will handle the cell internal objects layout correctly
                resultCell.ColSpan = span;
                if (item.IsTotal)
                {
                    resultCell.RowSpan = height - top;
                }
            }

            // print cell header
            if (root.Items.Count == 0 && dataWidth > 1)
            {
                foreach (MatrixCellDescriptor descr in Matrix.Data.Cells)
                {
                    TableCell templateCell = null;
                    var resultCell = ResultTable.GetCellData (left, top);

                    if (root.TemplateColumn != null && descr.TemplateColumn != null &&
                        cellHeaderDescriptor.TemplateColumn != null && cellHeaderDescriptor.TemplateRow != null)
                    {
                        templateCell = Matrix[
                            root.TemplateColumn.Index +
                            (descr.TemplateColumn.Index - cellHeaderDescriptor.TemplateColumn.Index),
                            cellHeaderDescriptor.TemplateRow.Index];
                    }
                    else
                    {
                        templateCell = CreateCell (ExtractColumnName (descr.Expression));
                    }

                    templateCell.SaveState();
                    templateCell.GetData();
                    resultCell.RunTimeAssign (templateCell, true);
                    templateCell.RestoreState();
                    left++;
                }
            }
        }

        private void PrintColumnHeader()
        {
            Matrix.RowValues = null;
            evenStyleIndices = new int[Matrix.Data.Columns.Count];
            PrintColumnHeader (Matrix.Data.Columns.RootItem, HeaderWidth, Matrix.ShowTitle ? 1 : 0, 0);
        }

        private void PrintRowHeader (MatrixHeaderItem root, int left, int top, int level)
        {
            var dataHeight = 1;
            var width = HeaderWidth;
            if (Matrix.Data.Cells.Count > 1 && !Matrix.CellsSideBySide)
            {
                dataHeight = Matrix.Data.Cells.Count;
                width--;
            }

            for (var index = 0; index < root.Items.Count; index++)
            {
                var item = root.Items[index];
                Matrix.RowValues = item.Values;
                var resultCell = ResultTable.GetCellData (left, top);
                var span = item.Span * dataHeight;
                if (Matrix.SplitRows)
                {
                    var duplicate = new MatrixHeaderItem (root)
                    {
                        IsSplitted = true,
                        Value = item.Value,
                        TemplateRow = item.TemplateRow,
                        TemplateCell = item.TemplateCell,
                        TemplateColumn = item.TemplateColumn
                    };

                    for (var i = 1; i < span; i++)
                    {
                        root.Items.Insert (index + 1, duplicate);
                    }

                    span = 1;
                }

                var dx = 1;
                if (item.IsTotal)
                {
                    dx = width - left;

                    // correct evenStyleIndices
                    for (var i = level + 1; i < evenStyleIndices.Length; i++)
                        evenStyleIndices[i]++;
                }

                PrintHeaderCell (item, resultCell, evenStyleIndices[level] % 2 != 0);
                PrintRowHeader (item, left + dx, top, level + 1);

                if (item.PageBreak && top > HeaderHeight)
                {
                    ResultTable.Rows[top].PageBreak = true;
                }

                top += span;
                evenStyleIndices[level]++;

                // set spans after we have printed the cell. This will handle the cell internal objects layout correctly
                resultCell.RowSpan = span;
                if (item.IsTotal)
                {
                    resultCell.ColSpan = width - left;
                }
            }

            // print cell header
            if (root.Items.Count == 0 && dataHeight > 1)
            {
                foreach (MatrixCellDescriptor descr in Matrix.Data.Cells)
                {
                    TableCell templateCell = null;
                    var resultCell = ResultTable.GetCellData (left, top);

                    if (root.TemplateRow != null && descr.TemplateRow != null &&
                        cellHeaderDescriptor.TemplateColumn != null && cellHeaderDescriptor.TemplateRow != null)
                    {
                        templateCell = Matrix[cellHeaderDescriptor.TemplateColumn.Index,
                            root.TemplateRow.Index +
                            (descr.TemplateRow.Index - cellHeaderDescriptor.TemplateRow.Index)];
                    }
                    else
                    {
                        templateCell = CreateCell (ExtractColumnName (descr.Expression));
                    }

                    templateCell.SaveState();
                    templateCell.GetData();
                    resultCell.RunTimeAssign (templateCell, true);
                    templateCell.RestoreState();
                    top++;
                }
            }
        }

        private void PrintRowHeader()
        {
            Matrix.ColumnValues = null;
            evenStyleIndices = new int[Matrix.Data.Rows.Count];
            PrintRowHeader (Matrix.Data.Rows.RootItem, 0, HeaderHeight, 0);
        }

        private void PrintCorner()
        {
            List<MatrixDescriptor> rowDescr = new List<MatrixDescriptor>();
            List<MatrixDescriptor> colDescr = new List<MatrixDescriptor>();
            rowDescr.AddRange (Matrix.Data.Rows.ToArray());
            colDescr.AddRange (Matrix.Data.Columns.ToArray());
            if (Matrix.Data.Cells.Count > 1)
            {
                if (Matrix.CellsSideBySide)
                {
                    colDescr.Add (cellHeaderDescriptor);
                }
                else
                {
                    rowDescr.Add (cellHeaderDescriptor);
                }
            }

            var top = 0;
            var firstRow = true;
            foreach (var col in colDescr)
            {
                var left = 0;
                foreach (var row in rowDescr)
                {
                    TableCell templateCell = null;
                    if (row.TemplateColumn != null && col.TemplateRow != null)
                    {
                        templateCell = Matrix[row.TemplateColumn.Index, col.TemplateRow.Index];
                    }
                    else if (firstRow)
                    {
                        templateCell = CreateCell (ExtractColumnName (row.Expression));
                    }

                    if (templateCell != null)
                    {
                        var resultCell = ResultTable.GetCellData (left, top + (Matrix.ShowTitle ? 1 : 0));
                        templateCell.SaveState();
                        if (!designTime)
                        {
                            templateCell.GetData();
                        }

                        resultCell.RunTimeAssign (templateCell, true);
                        templateCell.RestoreState();

                        // do not allow spans to go outside corner area
                        resultCell.ColSpan = Math.Min (templateCell.ColSpan, rowDescr.Count - left);
                        resultCell.RowSpan = Math.Min (templateCell.RowSpan, colDescr.Count - top);
                    }

                    left++;
                }

                firstRow = false;
                top++;
            }
        }

        private void PrintTitle()
        {
            var templateCell = titleDescriptor.TemplateCell;
            if (titleDescriptor.TemplateCell == null)
            {
                templateCell = CreateCell (Res.Get ("ComponentsMisc,Matrix,Title"));
            }

            var resultCell = ResultTable.GetCellData (HeaderWidth, 0);
            templateCell.SaveState();
            if (!designTime)
            {
                templateCell.GetData();
            }

            resultCell.RunTimeAssign (templateCell, true);
            resultCell.ColSpan = ResultTable.ColumnCount - HeaderWidth;
            templateCell.RestoreState();

            // print left-top cell
            if (titleDescriptor.TemplateCell == null)
            {
                templateCell.Text = "";
            }
            else
            {
                templateCell = Matrix[0, 0];
            }

            resultCell = ResultTable.GetCellData (0, 0);
            templateCell.SaveState();
            if (!designTime)
            {
                templateCell.GetData();
            }

            resultCell.RunTimeAssign (templateCell, true);
            templateCell.RestoreState();
            resultCell.ColSpan = HeaderWidth;
        }

        private void PrintHeaders()
        {
            PrintColumnHeader();
            PrintRowHeader();
            if (Matrix.ShowTitle)
            {
                PrintTitle();
            }

            PrintCorner();
        }

        private void PrintData_CalcTotals (int pass)
        {
            List<MatrixHeaderItem> columnTerminalItems = Matrix.Data.Columns.RootItem.GetTerminalItems();
            List<MatrixHeaderItem> rowTerminalItems = Matrix.Data.Rows.RootItem.GetTerminalItems();
            var dataCount = Matrix.Data.Cells.Count;
            CellValues = new object[dataCount];

            foreach (var rowItem in rowTerminalItems)
            {
                foreach (var columnItem in columnTerminalItems)
                {
                    if ((pass == 1 && !(rowItem.IsTotal || columnItem.IsTotal)) ||
                        (pass == 2 && (rowItem.IsTotal || columnItem.IsTotal)))
                    {
                        continue;
                    }

                    // at first we calc cells with non-custom functions
                    // prepare FCellValues array which will be used when calculating custom functions
                    for (var cellIndex = 0; cellIndex < dataCount; cellIndex++)
                    {
                        if (Matrix.Data.Cells[cellIndex].Function != MatrixAggregateFunction.Custom)
                        {
                            CellValues[cellIndex] = GetCellValue (columnItem, rowItem, cellIndex);
                        }
                    }

                    // at second we calc cells with custom functions
                    // (to allow the custom function to use other cells' values)
                    for (var cellIndex = 0; cellIndex < dataCount; cellIndex++)
                    {
                        if (Matrix.Data.Cells[cellIndex].Function == MatrixAggregateFunction.Custom)
                        {
                            CellValues[cellIndex] = GetCellValue (columnItem, rowItem, cellIndex);
                        }
                    }

                    Matrix.Data.Cells.SetValues (columnItem.Index, rowItem.Index, CellValues);
                }
            }
        }

        private void PrintData_CalcPercents()
        {
            var dataCount = Matrix.Data.Cells.Count;

            // check if we need to do this
            var hasPercents = false;
            for (var cellIndex = 0; cellIndex < dataCount; cellIndex++)
            {
                if (Matrix.Data.Cells[cellIndex].Percent != MatrixPercent.None)
                {
                    hasPercents = true;
                }
            }

            if (!hasPercents)
            {
                return;
            }

            List<MatrixHeaderItem> columnTerminalItems = Matrix.Data.Columns.RootItem.GetTerminalItems();
            List<MatrixHeaderItem> rowTerminalItems = Matrix.Data.Rows.RootItem.GetTerminalItems();
            CellValues = new object[dataCount];

            foreach (var rowItem in rowTerminalItems)
            {
                foreach (var columnItem in columnTerminalItems)
                {
                    for (var cellIndex = 0; cellIndex < dataCount; cellIndex++)
                    {
                        var cellValue = Matrix.Data.Cells.GetValue (columnItem.Index, rowItem.Index, cellIndex);
                        object totalValue = null;
                        object value = null;

                        var totalColumnIndex = Matrix.Data.Columns[0].TotalsFirst ? 0 : columnTerminalItems.Count - 1;
                        var totalRowIndex = Matrix.Data.Rows[0].TotalsFirst ? 0 : rowTerminalItems.Count - 1;

                        switch (Matrix.Data.Cells[cellIndex].Percent)
                        {
                            case MatrixPercent.None:
                                value = cellValue;
                                break;

                            case MatrixPercent.ColumnTotal:
                                totalValue = Matrix.Data.Cells.GetValue (columnTerminalItems[totalColumnIndex].Index,
                                    rowItem.Index, cellIndex);
                                break;

                            case MatrixPercent.RowTotal:
                                totalValue = Matrix.Data.Cells.GetValue (columnItem.Index,
                                    rowTerminalItems[totalRowIndex].Index, cellIndex);
                                break;

                            case MatrixPercent.GrandTotal:
                                totalValue = Matrix.Data.Cells.GetValue (columnTerminalItems[totalColumnIndex].Index,
                                    rowTerminalItems[totalRowIndex].Index, cellIndex);
                                break;
                        }

                        if (cellValue != null && totalValue != null)
                        {
                            value = ((Variant)(new Variant (cellValue) / new Variant (totalValue))).Value;
                        }

                        CellValues[cellIndex] = value;
                    }

                    Matrix.Data.Cells.SetValues (columnItem.Index, rowItem.Index, CellValues);
                }
            }
        }

        private void PrintData()
        {
            // use two passes to calc cell values. This is necessary because this calculation
            // replaces an array of cell values by the single (aggregated) value.
            // At the first pass we calc total values only (so they include all cell values, not aggregated ones);
            // at the second pass we calc other values except total.
            PrintData_CalcTotals (1);
            PrintData_CalcTotals (2);

            // calc percents
            PrintData_CalcPercents();

            // fire AfterTotals event
            Matrix.OnAfterTotals (EventArgs.Empty);

            List<MatrixHeaderItem> columnTerminalItems = Matrix.Data.Columns.RootItem.GetTerminalItems();
            List<MatrixHeaderItem> rowTerminalItems = Matrix.Data.Rows.RootItem.GetTerminalItems();
            var dataCount = Matrix.Data.Cells.Count;
            var top = HeaderHeight;
            var bodyLocation = GetBodyLocation();
            var firstTimePrintingData = true;
            CellValues = new object[dataCount];
            Matrix.RowIndex = 0;

            foreach (var rowItem in rowTerminalItems)
            {
                var left = HeaderWidth;
                Matrix.RowValues = rowItem.Values;
                Matrix.ColumnIndex = 0;

                foreach (var columnItem in columnTerminalItems)
                {
                    Matrix.ColumnValues = columnItem.Values;

                    for (var cellIndex = 0; cellIndex < dataCount; cellIndex++)
                    {
                        TableCell templateCell = null;
                        TableCellData resultCell = null;
                        var descr = Matrix.Data.Cells[cellIndex];

                        if (Matrix.CellsSideBySide)
                        {
                            if (columnItem.TemplateColumn != null && rowItem.TemplateRow != null &&
                                descr.TemplateColumn != null)
                            {
                                templateCell = Matrix[
                                    columnItem.TemplateColumn.Index + (descr.TemplateColumn.Index - bodyLocation.X),
                                    rowItem.TemplateRow.Index];
                            }
                            else
                            {
                                templateCell = CreateDataCell();
                            }

                            resultCell = ResultTable.GetCellData (left + cellIndex, top);
                        }
                        else
                        {
                            if (columnItem.TemplateColumn != null && rowItem.TemplateRow != null &&
                                descr.TemplateColumn != null)
                            {
                                templateCell = Matrix[columnItem.TemplateColumn.Index,
                                    rowItem.TemplateRow.Index + (descr.TemplateRow.Index - bodyLocation.Y)];
                            }
                            else
                            {
                                templateCell = CreateDataCell();
                            }

                            resultCell = ResultTable.GetCellData (left, top + cellIndex);
                        }

                        if (designTime)
                        {
                            if (firstTimePrintingData)
                            {
                                templateCell.Text = "[" + ExtractColumnName (descr.Expression) + "]";
                            }
                            else
                            {
                                templateCell.Text = "";
                            }

                            resultCell.RunTimeAssign (templateCell, true);
                        }
                        else
                        {
                            var value = Matrix.Data.GetValue (columnItem.Index, rowItem.Index, cellIndex);
                            CellValues[cellIndex] = value;
                            templateCell.Text = templateCell.FormatValue (value);
                            templateCell.SaveState();
                            if (string.IsNullOrEmpty (templateCell.Hyperlink.Expression) &&
                                (templateCell.Hyperlink.Kind == HyperlinkKind.DetailReport ||
                                 templateCell.Hyperlink.Kind == HyperlinkKind.DetailPage ||
                                 templateCell.Hyperlink.Kind == HyperlinkKind.Custom))
                            {
                                var hyperlinkValue = "";
                                var separator = templateCell.Hyperlink.ValuesSeparator;
                                foreach (var obj in Matrix.ColumnValues)
                                {
                                    hyperlinkValue += obj.ToString() + separator;
                                }

                                foreach (var obj in Matrix.RowValues)
                                {
                                    hyperlinkValue += obj.ToString() + separator;
                                }

                                templateCell.Hyperlink.Value =
                                    hyperlinkValue.Substring (0, hyperlinkValue.Length - separator.Length);
                            }

                            var evenStyleIndex = Matrix.MatrixEvenStylePriority == MatrixEvenStylePriority.Rows
                                ? Matrix.RowIndex
                                : Matrix.ColumnIndex;
                            if ((evenStyleIndex + 1) % 2 == 0)
                            {
                                templateCell.ApplyEvenStyle();
                            }

                            templateCell.GetData();
                            resultCell.RunTimeAssign (templateCell, true);
                            templateCell.RestoreState();
                        }
                    }

                    firstTimePrintingData = false;
                    Matrix.ColumnIndex++;
                    if (Matrix.CellsSideBySide)
                    {
                        if (Matrix.KeepCellsSideBySide)
                        {
                            ResultTable.Columns[left].KeepColumns = dataCount;
                        }

                        left += dataCount;
                    }
                    else
                    {
                        left++;
                    }
                }

                Matrix.RowIndex++;
                if (Matrix.CellsSideBySide)
                {
                    top++;
                }
                else
                {
                    top += dataCount;
                }
            }
        }

        private object GetCellValue (MatrixHeaderItem columnItem, MatrixHeaderItem rowItem, int cellIndex)
        {
            if (columnItem.IsTotal || rowItem.IsTotal)
            {
                return GetAggregatedTotalValue (columnItem, rowItem, cellIndex);
            }
            else
            {
                return GetAggregatedValue (Matrix.Data.GetValues (columnItem.Index, rowItem.Index, cellIndex),
                    cellIndex);
            }
        }

        private object GetAggregatedTotalValue (MatrixHeaderItem column, MatrixHeaderItem row, int cellIndex)
        {
            var list = new ArrayList();

            if (column.IsTotal)
            {
                column = column.Parent;
            }

            if (row.IsTotal)
            {
                row = row.Parent;
            }

            List<MatrixHeaderItem> columnTerminalItems = column.GetTerminalItems();
            List<MatrixHeaderItem> rowTerminalItems = row.GetTerminalItems();

            // collect all values in the specified items
            foreach (var rowItem in rowTerminalItems)
            {
                if (rowItem.IsTotal)
                {
                    continue;
                }

                foreach (var columnItem in columnTerminalItems)
                {
                    if (columnItem.IsTotal)
                    {
                        continue;
                    }

                    var values = Matrix.Data.GetValues (columnItem.Index, rowItem.Index, cellIndex);
                    if (values != null)
                    {
                        list.AddRange (values);
                    }
                }
            }

            return GetAggregatedValue (list, cellIndex);
        }

        private object GetAggregatedValue (ArrayList list, int cellIndex)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }

            var function = Matrix.Data.Cells[cellIndex].Function;

            // custom function - just calculate the expression
            if (function == MatrixAggregateFunction.Custom)
            {
                return Report.Calc (Matrix.Data.Cells[cellIndex].Expression);
            }

            // Count function - just return the number of values
            if (function == MatrixAggregateFunction.Count)
            {
                return list.Count;
            }

            if (function == MatrixAggregateFunction.CountDistinct)
            {
                var distinctValues = new Hashtable();
                foreach (var value in list)
                {
                    distinctValues[value] = 1;
                }

                return distinctValues.Keys.Count;
            }

            // aggregated value
            var aggrValue = new Variant();

            for (var i = 0; i < list.Count; i++)
            {
                var value = list[i];
                if (i == 0)
                {
                    // assign the first value to aggrValue
                    aggrValue = new Variant (value);
                }
                else
                {
                    // handle other values
                    switch (function)
                    {
                        case MatrixAggregateFunction.Sum:
                        case MatrixAggregateFunction.Avg:
                            aggrValue += new Variant (value);
                            break;

                        case MatrixAggregateFunction.Min:
                            if (new Variant (value) < aggrValue)
                            {
                                aggrValue = new Variant (value);
                            }

                            break;

                        case MatrixAggregateFunction.Max:
                            if (new Variant (value) > aggrValue)
                            {
                                aggrValue = new Variant (value);
                            }

                            break;
                    }
                }
            }

            // finish Avg calculation
            if (function == MatrixAggregateFunction.Avg)
            {
                aggrValue = aggrValue / list.Count;
            }

            return aggrValue.Value;
        }

        #endregion

        #region Public Methods

        public void BuildTemplate()
        {
            var noColumns = Matrix.Data.Columns.Count == 0;
            var noRows = Matrix.Data.Rows.Count == 0;
            var noCells = Matrix.Data.Cells.Count == 0;

            // create temporary descriptors
            if (noColumns)
            {
                Matrix.Data.Columns.Add (noColumnsDescriptor);
            }

            if (noRows)
            {
                Matrix.Data.Rows.Add (noRowsDescriptor);
            }

            if (noCells)
            {
                Matrix.Data.Cells.Add (noCellsDescriptor);
            }

            UpdateTemplateSizes();

            // prepare data for the result table
            string[] columnValues = new string[Matrix.Data.Columns.Count];
            string[] rowValues = new string[Matrix.Data.Rows.Count];
            object[] cellValues = new object[Matrix.Data.Cells.Count];
            for (var i = 0; i < Matrix.Data.Columns.Count; i++)
            {
                columnValues[i] = "[" + ExtractColumnName (Matrix.Data.Columns[i].Expression) + "]";
            }

            for (var i = 0; i < Matrix.Data.Rows.Count; i++)
            {
                rowValues[i] = "[" + ExtractColumnName (Matrix.Data.Rows[i].Expression) + "]";
            }

            Matrix.Data.Clear();
            Matrix.Data.AddValue (columnValues, rowValues, cellValues, 0);

            // create the result table
            designTime = true;
            resultTable = new TableResult();
            InitResultTable (true);
            PrintHeaders();
            PrintData();

            // copy the result table to the Matrix
            Matrix.ColumnCount = ResultTable.ColumnCount;
            Matrix.RowCount = ResultTable.RowCount;
            Matrix.FixedColumns = HeaderWidth;
            Matrix.FixedRows = HeaderHeight;
            Matrix.CreateUniqueNames();

            for (var x = 0; x < Matrix.ColumnCount; x++)
            {
                Matrix.Columns[x].Assign (ResultTable.Columns[x]);
            }

            for (var y = 0; y < Matrix.RowCount; y++)
            {
                Matrix.Rows[y].Assign (ResultTable.Rows[y]);
            }

            for (var x = 0; x < Matrix.ColumnCount; x++)
            {
                for (var y = 0; y < Matrix.RowCount; y++)
                {
                    var cell = Matrix[x, y];

                    // call AssignAll with assignName parameter to preserve names of objects contained in a cell
                    ResultTable[x, y].Name = cell.Name;
                    cell.AssignAll (ResultTable[x, y], true);
                    cell.SetFlags (Flags.CanEdit, true);
                }
            }

            UpdateDescriptors();
            resultTable.Dispose();

            // clear temporary descriptors, set hints
            if (noColumns)
            {
                SetHint (Matrix[HeaderWidth, Matrix.ShowTitle ? 1 : 0], Res.Get ("ComponentsMisc,Matrix,NewColumn"));
                Matrix.Data.Columns.Clear();
            }
            else
            {
                noColumnsDescriptor.TemplateColumn = Matrix.Data.Columns[0].TemplateColumn;
                noColumnsDescriptor.TemplateRow = Matrix.Data.Columns[0].TemplateRow;
                noColumnsDescriptor.TemplateCell = Matrix.Data.Columns[0].TemplateCell;
            }

            if (noRows)
            {
                SetHint (Matrix[0, HeaderHeight], Res.Get ("ComponentsMisc,Matrix,NewRow"));
                Matrix.Data.Rows.Clear();
            }
            else
            {
                noRowsDescriptor.TemplateColumn = Matrix.Data.Rows[0].TemplateColumn;
                noRowsDescriptor.TemplateRow = Matrix.Data.Rows[0].TemplateRow;
                noRowsDescriptor.TemplateCell = Matrix.Data.Rows[0].TemplateCell;
            }

            if (noCells)
            {
                SetHint (Matrix[HeaderWidth, HeaderHeight], Res.Get ("ComponentsMisc,Matrix,NewCell"));
                Matrix.Data.Cells.Clear();
            }
            else
            {
                noCellsDescriptor.TemplateColumn = Matrix.Data.Cells[0].TemplateColumn;
                noCellsDescriptor.TemplateRow = Matrix.Data.Cells[0].TemplateRow;
                noCellsDescriptor.TemplateCell = Matrix.Data.Cells[0].TemplateCell;
            }
        }

        public void UpdateDescriptors()
        {
            var noColumns = Matrix.Data.Columns.Count == 0;
            var noRows = Matrix.Data.Rows.Count == 0;
            var noCells = Matrix.Data.Cells.Count == 0;

            // create temporary descriptors
            if (noColumns)
            {
                Matrix.Data.Columns.Add (noColumnsDescriptor);
            }

            if (noRows)
            {
                Matrix.Data.Rows.Add (noRowsDescriptor);
            }

            if (noCells)
            {
                Matrix.Data.Cells.Add (noCellsDescriptor);
            }

            UpdateTemplateSizes();
            Matrix.FixedColumns = HeaderWidth;
            Matrix.FixedRows = HeaderHeight;
            UpdateColumnDescriptors();
            UpdateRowDescriptors();
            UpdateCellDescriptors();
            UpdateOtherDescriptors();

            // clear temporary descriptors
            if (noColumns)
            {
                Matrix.Data.Columns.Clear();
            }

            if (noRows)
            {
                Matrix.Data.Rows.Clear();
            }

            if (noCells)
            {
                Matrix.Data.Cells.Clear();
            }
        }

        public void StartPrint()
        {
            if ((Matrix.Data.Columns.Count == 0 && Matrix.Data.Rows.Count == 0) || Matrix.Data.Cells.Count == 0)
            {
                throw new Exception (string.Format (Res.Get ("Messages,MatrixError"), Matrix.Name));
            }

            designTime = false;
            noColumns = Matrix.Data.Columns.Count == 0;
            noRows = Matrix.Data.Rows.Count == 0;

            // create temporary descriptors
            if (noColumns)
            {
                Matrix.Data.Columns.Add (noColumnsDescriptor);
            }

            if (noRows)
            {
                Matrix.Data.Rows.Add (noRowsDescriptor);
            }

            Config.ReportSettings.OnProgress (Report, Res.Get ("ComponentsMisc,Matrix,FillData"), 0, 0);

            Matrix.Data.Clear();
            Matrix.OnManualBuild (EventArgs.Empty);
        }

        public void AddDataRow()
        {
            object[] columnValues = new object[Matrix.Data.Columns.Count];
            object[] rowValues = new object[Matrix.Data.Rows.Count];
            object[] cellValues = new object[Matrix.Data.Cells.Count];
            var expression = "";

            for (var i = 0; i < Matrix.Data.Columns.Count; i++)
            {
                expression = Matrix.Data.Columns[i].Expression;
                columnValues[i] = string.IsNullOrEmpty (expression) ? null : Report.Calc (expression);
            }

            for (var i = 0; i < Matrix.Data.Rows.Count; i++)
            {
                expression = Matrix.Data.Rows[i].Expression;
                rowValues[i] = string.IsNullOrEmpty (expression) ? null : Report.Calc (expression);
            }

            for (var i = 0; i < Matrix.Data.Cells.Count; i++)
            {
                // skip custom function calculation; it will be calculated when we print the value
                if (Matrix.Data.Cells[i].Function == MatrixAggregateFunction.Custom)
                {
                    cellValues[i] = 0;
                }
                else
                {
                    cellValues[i] = Report.Calc (Matrix.Data.Cells[i].Expression);
                }
            }

            Matrix.Data.AddValue (columnValues, rowValues, cellValues, Matrix.DataSource.CurrentRowNo);
        }

        public void AddEmptyDataRow()
        {
            object[] columnValues = new object[Matrix.Data.Columns.Count];
            object[] rowValues = new object[Matrix.Data.Rows.Count];
            object[] cellValues = new object[Matrix.Data.Cells.Count];

            Matrix.Data.AddValue (columnValues, rowValues, cellValues, 0);
        }

        public void AddDataRows()
        {
            if (Matrix.DataSource != null)
            {
                Matrix.DataSource.Init (Matrix.Filter);
                while (Matrix.DataSource.HasMoreRows)
                {
                    AddDataRow();
                    Matrix.DataSource.Next();
                }
            }
        }

        public void FinishPrint()
        {
            if (!Matrix.Data.IsEmpty || Matrix.PrintIfEmpty)
            {
                if (Matrix.Data.IsEmpty)
                {
                    AddEmptyDataRow();
                }

                UpdateDescriptors();
                ResultTable.FixedColumns = HeaderWidth;
                ResultTable.FixedRows = HeaderHeight;

                InitResultTable (false);
                PrintHeaders();
                PrintData();
            }

            // clear temporary descriptors
            if (noColumns)
            {
                Matrix.Data.Columns.Clear();
            }

            if (noRows)
            {
                Matrix.Data.Rows.Clear();
            }
        }

        public void UpdateStyle()
        {
            for (var y = 0; y < Matrix.RowCount; y++)
            {
                for (var x = 0; x < Matrix.ColumnCount; x++)
                {
                    var style = x < HeaderWidth || y < HeaderHeight ? "Header" : "Body";
                    ApplyStyle (Matrix[x, y], style);
                }
            }
        }

        #endregion

        public MatrixHelper (MatrixObject matrix)
        {
            this.Matrix = matrix;
            titleDescriptor = new MatrixDescriptor();
            cellHeaderDescriptor = new MatrixDescriptor
            {
                Expression = "Data"
            };
            noColumnsDescriptor = new MatrixHeaderDescriptor ("", false);
            noRowsDescriptor = new MatrixHeaderDescriptor ("", false);
            noCellsDescriptor = new MatrixCellDescriptor();
        }
    }
}
