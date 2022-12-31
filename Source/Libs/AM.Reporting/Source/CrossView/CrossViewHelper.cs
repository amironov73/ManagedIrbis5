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

using AM.Reporting.Table;
using AM.Reporting.Utils;

using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting.CrossView
{
    internal partial class CrossViewHelper
    {
        private bool designTime;
        private TableResult resultTable;
        private CrossViewDescriptor titleDescriptor;
        private CrossViewDescriptor xAxisFieldCaptionDescriptor;
        private CrossViewDescriptor yAxisFieldCaptionDescriptor;
        private CrossViewHeaderDescriptor noColumnsDescriptor;
        private CrossViewHeaderDescriptor noRowsDescriptor;
        private CrossViewCellDescriptor noCellsDescriptor;

        #region Properties

        public CrossViewObject CrossView { get; }

        public Report Report => CrossView.Report;

        public TableResult ResultTable => designTime ? resultTable : CrossView.ResultTable;

        public int HeaderHeight { get; private set; }

        public int HeaderWidth { get; private set; }

        public int TemplateBodyWidth { get; private set; }

        public int TemplateBodyHeight { get; private set; }

        public int ResultBodyWidth { get; private set; }

        public int ResultBodyHeight { get; private set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates HeaderWidth, HeaderHeight, BodyWidth, BodyHeight properties.
        /// </summary>
        private void UpdateTemplateSizes()
        {
            if (CrossView.Data.SourceAssigned)
            {
                HeaderWidth = CrossView.Data.YAxisFieldsCount;
                HeaderHeight = CrossView.Data.XAxisFieldsCount;
            }
            else
            {
                HeaderWidth = 1;
                HeaderHeight = 1;
            }

            if (CrossView.ShowXAxisFieldsCaption)
            {
                HeaderHeight++;
            }

            if (CrossView.ShowTitle)
            {
                HeaderHeight++;
            }

            if (HeaderHeight == 0)
            {
                if (CrossView.ShowYAxisFieldsCaption)
                {
                    HeaderHeight = 1;
                }
            }

            TemplateBodyWidth = 1 + CrossView.Data.XAxisFieldsCount;
            if (CrossView.Data.MeasuresInXAxis)
            {
                TemplateBodyWidth = (TemplateBodyWidth - 1) * CrossView.Data.MeasuresCount;
            }

            TemplateBodyHeight = 1 + CrossView.Data.YAxisFieldsCount;
            if (CrossView.Data.MeasuresInYAxis)
            {
                TemplateBodyHeight = (TemplateBodyHeight - 1) * CrossView.Data.MeasuresCount;
            }
        }

        private void UpdateColumnDescriptors()
        {
            var left = HeaderWidth;
            var top = 0;
            if (CrossView.ShowTitle)
            {
                top++;
            }

            if (CrossView.ShowXAxisFieldsCaption)
            {
                top++;
            }

            foreach (CrossViewHeaderDescriptor descr in CrossView.Data.Columns)
            {
                descr.TemplateColumn = CrossView.Columns[left + descr.cell];
                descr.TemplateRow = CrossView.Rows[top + descr.level];
                descr.TemplateCell = CrossView[left + descr.cell, top + descr.level];
            }
        }

        private void UpdateRowDescriptors()
        {
            var left = 0;
            var top = HeaderHeight;

            foreach (CrossViewHeaderDescriptor descr in CrossView.Data.Rows)
            {
                descr.TemplateColumn = CrossView.Columns[left + descr.level];
                descr.TemplateRow = CrossView.Rows[top + descr.cell];
                descr.TemplateCell = CrossView[left + descr.level, top + descr.cell];
            }
        }

        private void UpdateCellDescriptors()
        {
            var x = HeaderWidth;
            var y = HeaderHeight;

            foreach (CrossViewCellDescriptor descr in CrossView.Data.Cells)
            {
                descr.TemplateColumn = CrossView.Columns[x + descr.x];
                descr.TemplateRow = CrossView.Rows[y + descr.y];
                descr.TemplateCell = CrossView[x + descr.x, y + descr.y];
            }
        }

        private void UpdateOtherDescriptors()
        {
            titleDescriptor.TemplateColumn = null;
            titleDescriptor.TemplateRow = null;
            titleDescriptor.TemplateCell = null;
            if (CrossView.ShowTitle)
            {
                titleDescriptor.TemplateColumn = CrossView.Columns[HeaderWidth];
                titleDescriptor.TemplateRow = CrossView.Rows[0];
                titleDescriptor.TemplateCell = CrossView[HeaderWidth, 0];
            }

            xAxisFieldCaptionDescriptor.TemplateColumn = null;
            xAxisFieldCaptionDescriptor.TemplateRow = null;
            xAxisFieldCaptionDescriptor.TemplateCell = null;
            if (CrossView.ShowXAxisFieldsCaption)
            {
                xAxisFieldCaptionDescriptor.TemplateColumn = CrossView.Columns[HeaderWidth];
                xAxisFieldCaptionDescriptor.TemplateRow = CrossView.Rows[(CrossView.ShowTitle ? 1 : 0)];
                xAxisFieldCaptionDescriptor.TemplateCell = CrossView[HeaderWidth, (CrossView.ShowTitle ? 1 : 0)];
            }
        }

        private void ApplyStyle (TableCell cell, string styleName)
        {
            Style style = null;
            var styleIndex = CrossView.StyleSheet.IndexOf (CrossView.Style);
            if (styleIndex != -1)
            {
                var styles = CrossView.StyleSheet[styleIndex];
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
            cell.AllowExpressions = false;
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
            cell.AllowExpressions = false;
            cell.Font = DrawUtils.DefaultReportFont;
            cell.HorzAlign = HorzAlign.Right;
            cell.VertAlign = VertAlign.Center;
            ApplyStyle (cell, "Body");
            return cell;
        }

        private void SetHint (TableCell cell, string text)
        {
            cell.Assign (CrossView.Styles.DefaultStyle);
            cell.Text = text;
            cell.Font = DrawUtils.DefaultReportFont;
            cell.TextFill = new SolidFill (Color.Gray);
            cell.HorzAlign = HorzAlign.Center;
            cell.VertAlign = VertAlign.Center;
            cell.SetFlags (Flags.CanEdit, false);
        }

        private void InitTemplateTable()
        {
            TableColumn column;
            CrossViewDescriptor descr;
            for (var i = 0; i < HeaderWidth + TemplateBodyWidth; i++)
            {
                column = new TableColumn();
                descr = CrossView.Data.GetColumnDescriptor (i);
                if (descr.TemplateColumn != null)
                {
                    column.Assign (descr.TemplateColumn);
                }

                ResultTable.Columns.Add (column);
            }

            TableRow row;
            for (var i = 0; i < HeaderHeight + TemplateBodyHeight; i++)
            {
                row = new TableRow();
                var rowindex = i;
                if (CrossView.ShowTitle)
                {
                    rowindex--;
                }

                if (CrossView.ShowXAxisFieldsCaption)
                {
                    rowindex--;
                }

                if (rowindex >= 0)
                {
                    descr = CrossView.Data.GetRowDescriptor (rowindex);
                }
                else
                {
                    if (CrossView.ShowTitle)
                    {
                        if (i == 0)
                        {
                            descr = titleDescriptor;
                        }
                        else
                        {
                            descr = xAxisFieldCaptionDescriptor;
                        }
                    }
                    else
                    {
                        descr = xAxisFieldCaptionDescriptor;
                    }
                }

                if (descr.TemplateRow != null)
                {
                    row.Assign (descr.TemplateRow);
                }

                ResultTable.Rows.Add (row);
            }
        }

        private void InitResultTable()
        {
            //todo
            TableColumn column;
            CrossViewDescriptor descr;
            for (var i = 0; i < HeaderWidth + ResultBodyWidth; i++)
            {
                column = new TableColumn();

                descr = CrossView.Data.GetColumnDescriptor (0); // temp 0 !!!

                if (descr.TemplateColumn != null)
                {
                    column.Assign (descr.TemplateColumn);
                }

                ResultTable.Columns.Add (column);
            }

            TableRow row;
            for (var i = 0; i < HeaderHeight + ResultBodyHeight; i++)
            {
                row = new TableRow();
                var rowindex = i;
                if (CrossView.ShowTitle)
                {
                    rowindex--;
                }

                if (CrossView.ShowXAxisFieldsCaption)
                {
                    rowindex--;
                }

                if (rowindex >= 0)
                {
                    descr = CrossView.Data.GetRowDescriptor (0); // temp 0 !!! (rowindex)
                }
                else
                {
                    if (CrossView.ShowTitle)
                    {
                        if (i == 0)
                        {
                            descr = titleDescriptor;
                        }
                        else
                        {
                            descr = xAxisFieldCaptionDescriptor;
                        }
                    }
                    else
                    {
                        descr = xAxisFieldCaptionDescriptor;
                    }
                }

                if (descr.TemplateRow != null)
                {
                    row.Assign (descr.TemplateRow);
                }

                ResultTable.Rows.Add (row);
            }
        }

        private void PrintHeaders()
        {
            if (CrossView.ShowTitle)
            {
                PrintTitle();
            }

            PrintCorner();
            if (CrossView.ShowXAxisFieldsCaption)
            {
                PrintXAxisFieldsCaption();
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
            templateCell.GetData();
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
                templateCell = CrossView[0, 0];
            }

            resultCell = ResultTable.GetCellData (0, 0);
            templateCell.SaveState();
            templateCell.GetData();
            resultCell.RunTimeAssign (templateCell, true);
            templateCell.RestoreState();
            resultCell.ColSpan = HeaderWidth;
        }

        private void PrintCorner()
        {
            var left = 0;
            var top = (CrossView.ShowTitle ? 1 : 0);
            var templateTop = titleDescriptor.TemplateRow != null ? 1 : 0;
            if (CrossView.ShowYAxisFieldsCaption)
            {
                for (var i = 0; i < HeaderWidth; i++)
                {
                    TableCell templateCell = null;
                    var descr = CrossView.Data.GetColumnDescriptor (i);
                    if (descr.TemplateColumn != null)
                    {
                        templateCell = CrossView[0, templateTop];
                    }
                    else
                    {
                        templateCell = CreateCell (descr.GetName());
                    }

                    var resultCell = ResultTable.GetCellData (left, top);
                    templateCell.SaveState();
                    templateCell.GetData();
                    resultCell.RunTimeAssign (templateCell, true);
                    templateCell.RestoreState();

                    resultCell.RowSpan = HeaderHeight - top;
                    resultCell.Text = descr.GetName();
                    left++;
                }
            }
            else
            {
                var templateCell = CreateCell ("");
                var resultCell = ResultTable.GetCellData (left, top);
                templateCell.SaveState();
                templateCell.GetData();
                resultCell.RunTimeAssign (templateCell, true);
                templateCell.RestoreState();
                resultCell.ColSpan = HeaderWidth;
                resultCell.RowSpan = HeaderHeight - top;
            }
        }

        private void PrintXAxisFieldsCaption()
        {
            var top = (CrossView.ShowTitle ? 1 : 0);
            var templateTop = titleDescriptor.TemplateRow != null ? 1 : 0;
            var captions = "";
            for (var i = 0; i < CrossView.Data.XAxisFieldsCount; i++)
            {
                var descr = CrossView.Data.GetRowDescriptor (i);
                if (captions != "")
                {
                    captions += ", ";
                }

                captions += descr.GetName();
            }

            var templateCell = xAxisFieldCaptionDescriptor.TemplateCell;
            if (xAxisFieldCaptionDescriptor.TemplateCell == null)
            {
                templateCell = CreateCell (captions);
            }

            var resultCell = ResultTable.GetCellData (HeaderWidth, top);
            templateCell.SaveState();
            templateCell.GetData();
            resultCell.RunTimeAssign (templateCell, true);
            resultCell.ColSpan = ResultTable.ColumnCount - HeaderWidth;
            templateCell.RestoreState();
        }

        private void PrintXAxisTemplate()
        {
            var left = HeaderWidth;
            var top = (CrossView.ShowTitle ? 1 : 0) + (CrossView.ShowXAxisFieldsCaption ? 1 : 0);
            for (var i = 0; i < CrossView.Data.Columns.Count; i++)
            {
                var crossViewHeaderDescriptor = CrossView.Data.Columns[i];
                var resultCell = ResultTable.GetCellData (left + crossViewHeaderDescriptor.cell,
                    top + crossViewHeaderDescriptor.level);
                resultCell.ColSpan = crossViewHeaderDescriptor.cellsize;
                resultCell.RowSpan = crossViewHeaderDescriptor.levelsize;
                var templateCell = crossViewHeaderDescriptor.TemplateCell;
                if (templateCell != null)
                {
                    templateCell.Text = "[" + crossViewHeaderDescriptor.GetName() + "]";
                }
                else
                {
                    templateCell = CreateCell ("[" + crossViewHeaderDescriptor.GetName() + "]");
                }

                resultCell.RunTimeAssign (templateCell, true);
            }
        }

        private void PrintYAxisTemplate()
        {
            var left = 0;
            var top = HeaderHeight;
            for (var i = 0; i < CrossView.Data.Rows.Count; i++)
            {
                var crossViewHeaderDescriptor = CrossView.Data.Rows[i];
                var resultCell = ResultTable.GetCellData (left + crossViewHeaderDescriptor.level,
                    top + crossViewHeaderDescriptor.cell);
                resultCell.ColSpan = crossViewHeaderDescriptor.levelsize;
                resultCell.RowSpan = crossViewHeaderDescriptor.cellsize;
                var templateCell = crossViewHeaderDescriptor.TemplateCell;
                if (templateCell != null)
                {
                    templateCell.Text = "[" + crossViewHeaderDescriptor.GetName() + "]";
                }
                else
                {
                    templateCell = CreateCell ("[" + crossViewHeaderDescriptor.GetName() + "]");
                }

                resultCell.RunTimeAssign (templateCell, true);
            }
        }

        private void PrintDataTemplate()
        {
            CrossViewCellDescriptor crossViewCellDescriptor;
            var top = HeaderHeight;
            var left = HeaderWidth;
            TableCell templateCell = null;
            TableCellData resultCell = null;
            for (var i = 0; i < CrossView.Data.columnTerminalIndexes.Length; i++)
            {
                for (var j = 0; j < CrossView.Data.rowTerminalIndexes.Length; j++)
                {
                    crossViewCellDescriptor = CrossView.Data.Cells[i * CrossView.Data.rowTerminalIndexes.Length + j];
                    resultCell = ResultTable.GetCellData (left + i, top + j);

                    templateCell = crossViewCellDescriptor.TemplateCell;
                    if (templateCell == null)
                    {
                        templateCell = CreateDataCell();
                    }

                    templateCell.Text = "0";
                    resultCell.RunTimeAssign (templateCell, true);
                }
            }
        }

        #endregion

        #region Public Methods

        public void BuildTemplate()
        {
            // create temporary descriptors
            if (!CrossView.Data.SourceAssigned)
            {
                CrossView.Data.columnDescriptorsIndexes = new int[] { 0 };
                CrossView.Data.rowDescriptorsIndexes = new int[] { 0 };
                CrossView.Data.columnTerminalIndexes = new int[] { 0 };
                CrossView.Data.rowTerminalIndexes = new int[] { 0 };
                CrossView.Data.Columns.Add (noColumnsDescriptor);
                CrossView.Data.Rows.Add (noRowsDescriptor);
                CrossView.Data.Cells.Add (noCellsDescriptor);
            }

            UpdateTemplateSizes();

            // create the result table
            designTime = true;
            resultTable = new TableResult();
            InitTemplateTable();
            PrintHeaders();
            PrintXAxisTemplate();
            PrintYAxisTemplate();
            PrintDataTemplate();

            // copy the result table to the Matrix
            CrossView.ColumnCount = ResultTable.ColumnCount;
            CrossView.RowCount = ResultTable.RowCount;
            CrossView.FixedColumns = HeaderWidth;
            CrossView.FixedRows = HeaderHeight;
            CrossView.CreateUniqueNames();

            for (var x = 0; x < CrossView.ColumnCount; x++)
            {
                CrossView.Columns[x].Assign (ResultTable.Columns[x]);
            }

            for (var y = 0; y < CrossView.RowCount; y++)
            {
                CrossView.Rows[y].Assign (ResultTable.Rows[y]);
            }

            for (var x = 0; x < CrossView.ColumnCount; x++)
            {
                for (var y = 0; y < CrossView.RowCount; y++)
                {
                    var cell = CrossView[x, y];
                    cell.AssignAll (ResultTable[x, y]);
                    cell.SetFlags (Flags.CanEdit, true);
                }
            }

            UpdateDescriptors();
            resultTable.Dispose();

            // clear temporary descriptors, set hints
            if (!CrossView.Data.SourceAssigned)
            {
                SetHint (
                    CrossView[HeaderWidth, (CrossView.ShowTitle ? 1 : 0) + (CrossView.ShowXAxisFieldsCaption ? 1 : 0)],
                    Res.Get ("ComponentsMisc,CrossView,SetSource"));
                CrossView.Data.Columns.Clear();
                SetHint (CrossView[0, HeaderHeight], Res.Get ("ComponentsMisc,CrossView,SetSource"));
                CrossView.Data.Rows.Clear();
                SetHint (CrossView[HeaderWidth, HeaderHeight], Res.Get ("ComponentsMisc,CrossView,SetSource"));
                CrossView.Data.Cells.Clear();
                CrossView.Data.columnDescriptorsIndexes = new int[0];
                CrossView.Data.rowDescriptorsIndexes = new int[0];
                CrossView.Data.columnTerminalIndexes = new int[0];
                CrossView.Data.rowTerminalIndexes = new int[0];
            }
            else
            {
                noColumnsDescriptor.TemplateColumn = CrossView.Data.Columns[0].TemplateColumn;
                noColumnsDescriptor.TemplateRow = CrossView.Data.Columns[0].TemplateRow;
                noColumnsDescriptor.TemplateCell = CrossView.Data.Columns[0].TemplateCell;
                noRowsDescriptor.TemplateColumn = CrossView.Data.Rows[0].TemplateColumn;
                noRowsDescriptor.TemplateRow = CrossView.Data.Rows[0].TemplateRow;
                noRowsDescriptor.TemplateCell = CrossView.Data.Rows[0].TemplateCell;
                noCellsDescriptor.TemplateColumn = CrossView.Data.Cells[0].TemplateColumn;
                noCellsDescriptor.TemplateRow = CrossView.Data.Cells[0].TemplateRow;
                noCellsDescriptor.TemplateCell = CrossView.Data.Cells[0].TemplateCell;
            }
        }

        public void UpdateDescriptors()
        {
            var needClearTemporaryDescriptors = false;
            if (CrossView.Data is { SourceAssigned: false, Columns.Count: 0 })
            {
                needClearTemporaryDescriptors = true;
                CrossView.Data.columnDescriptorsIndexes = new int[] { 0 };
                CrossView.Data.rowDescriptorsIndexes = new int[] { 0 };
                CrossView.Data.columnTerminalIndexes = new int[] { 0 };
                CrossView.Data.rowTerminalIndexes = new int[] { 0 };
                CrossView.Data.Columns.Add (noColumnsDescriptor);
                CrossView.Data.Rows.Add (noRowsDescriptor);
                CrossView.Data.Cells.Add (noCellsDescriptor);
            }

            UpdateTemplateSizes();
            CrossView.FixedColumns = HeaderWidth;
            CrossView.FixedRows = HeaderHeight;
            UpdateColumnDescriptors();
            UpdateRowDescriptors();
            UpdateCellDescriptors();
            UpdateOtherDescriptors();

            // clear temporary descriptors
            if (needClearTemporaryDescriptors)
            {
                CrossView.Data.Columns.Clear();
                CrossView.Data.Rows.Clear();
                CrossView.Data.Cells.Clear();
                CrossView.Data.columnDescriptorsIndexes = new int[0];
                CrossView.Data.rowDescriptorsIndexes = new int[0];
                CrossView.Data.columnTerminalIndexes = new int[0];
                CrossView.Data.rowTerminalIndexes = new int[0];
            }
        }

        public void UpdateStyle()
        {
            for (var y = 0; y < CrossView.RowCount; y++)
            {
                for (var x = 0; x < CrossView.ColumnCount; x++)
                {
                    var style = x < HeaderWidth || y < HeaderHeight ? "Header" : "Body";
                    ApplyStyle (CrossView[x, y], style);
                }
            }
        }

        public void StartPrint()
        {
            //todo
            /*
                  if ((Matrix.Data.Columns.Count == 0 && Matrix.Data.Rows.Count == 0) || Matrix.Data.Cells.Count == 0)
                    throw new Exception(String.Format(Res.Get("Messages,MatrixError"), Matrix.Name));
            */
            designTime = false;
            /*
                  FNoColumns = Matrix.Data.Columns.Count == 0;
                  FNoRows = Matrix.Data.Rows.Count == 0;

                  // create temporary descriptors
                  if (FNoColumns)
                    Matrix.Data.Columns.Add(FNoColumnsDescriptor);
                  if (FNoRows)
                    Matrix.Data.Rows.Add(FNoRowsDescriptor);
            */
            OnProgressInternal();
            /*
                  Matrix.Data.Clear();
                  Matrix.OnManualBuild(EventArgs.Empty);
            */
            ResultBodyHeight = CrossView.Data.DataRowCount;
            ResultBodyWidth = CrossView.Data.DataColumnCount;
        }


        public void AddData()
        {
        }

        public void FinishPrint()
        {
            //todo
            UpdateDescriptors();
            ResultTable.FixedColumns = HeaderWidth;
            ResultTable.FixedRows = HeaderHeight;

            InitResultTable();

            //todo
            //temp
            PrintHeaders();
            PrintXAxisResult();
            PrintYAxisResult();
            PrintResultData();


            //      PrintData();
            /*
                  // clear temporary descriptors
                  if (FNoColumns)
                    Matrix.Data.Columns.Clear();
                  if (FNoRows)
                    Matrix.Data.Rows.Clear();
            */
        }

        private int axisLeft;
        private int axisTop;

        public bool XAxisDrawCellHandler (CrossViewAxisDrawCell crossViewAxisDrawCell)
        {
            var crossViewHeaderDescriptor = CrossView.Data.Columns[0]; // temp - 0 todoCUBE
            var resultCell = ResultTable.GetCellData (axisLeft + crossViewAxisDrawCell.Cell,
                axisTop + crossViewAxisDrawCell.Level);
            resultCell.ColSpan = crossViewAxisDrawCell.SizeCell;
            resultCell.RowSpan = crossViewAxisDrawCell.SizeLevel;
            var templateCell = crossViewHeaderDescriptor.TemplateCell;
            if (templateCell != null)
            {
                templateCell.Text = crossViewAxisDrawCell.Text;
            }
            else
            {
                templateCell = CreateCell (crossViewAxisDrawCell.Text);
            }

            resultCell.RunTimeAssign (templateCell, true);
            return false;
        }

        private void PrintXAxisResult()
        {
            axisLeft = HeaderWidth;
            axisTop = (CrossView.ShowTitle ? 1 : 0) + (CrossView.ShowXAxisFieldsCaption ? 1 : 0);

            CrossView.CubeSource.TraverseXAxis (XAxisDrawCellHandler);
        }

        public bool YAxisDrawCellHandler (CrossViewAxisDrawCell crossViewAxisDrawCell)
        {
            var crossViewHeaderDescriptor = CrossView.Data.Rows[0]; // temp - 0 todoCUBE
            var resultCell = ResultTable.GetCellData (axisLeft + crossViewAxisDrawCell.Level,
                axisTop + crossViewAxisDrawCell.Cell);
            resultCell.ColSpan = crossViewAxisDrawCell.SizeLevel;
            resultCell.RowSpan = crossViewAxisDrawCell.SizeCell;
            var templateCell = crossViewHeaderDescriptor.TemplateCell;
            if (templateCell != null)
            {
                templateCell.Text = crossViewAxisDrawCell.Text;
            }
            else
            {
                templateCell = CreateCell (crossViewAxisDrawCell.Text);
            }

            resultCell.RunTimeAssign (templateCell, true);
            return false;
        }

        private void PrintYAxisResult()
        {
            axisLeft = 0;
            axisTop = HeaderHeight;

            CrossView.CubeSource.TraverseYAxis (YAxisDrawCellHandler);
        }

        private void PrintResultData()
        {
            CrossViewCellDescriptor crossViewCellDescriptor;
            var top = HeaderHeight;
            var left = HeaderWidth;
            TableCell templateCell = null;
            TableCellData resultCell = null;
            CrossViewMeasureCell cubeMeasureCell;
            for (var i = 0; i < CrossView.Data.DataColumnCount; i++)
            {
                for (var j = 0; j < CrossView.Data.DataRowCount; j++)
                {
                    cubeMeasureCell = CrossView.CubeSource.GetMeasureCell (i, j);
                    crossViewCellDescriptor = CrossView.Data.Cells[0]; // todoCUBE temp 0 !!!
                    resultCell = ResultTable.GetCellData (left + i, top + j);

                    templateCell = crossViewCellDescriptor.TemplateCell;
                    if (templateCell == null)
                    {
                        templateCell = CreateDataCell();
                    }

                    templateCell.Text = cubeMeasureCell.Text;
                    resultCell.RunTimeAssign (templateCell, true);
                }
            }
        }

        internal void CreateOtherDescriptor()
        {
            titleDescriptor = new CrossViewDescriptor();
            xAxisFieldCaptionDescriptor = new CrossViewDescriptor();
            yAxisFieldCaptionDescriptor = new CrossViewDescriptor();
            noColumnsDescriptor = new CrossViewHeaderDescriptor
            {
                cell = 0,
                cellsize = 1,
                level = 0,
                levelsize = 1
            };
            noRowsDescriptor = new CrossViewHeaderDescriptor
            {
                cell = 0,
                cellsize = 1,
                level = 0,
                levelsize = 1
            };
            noCellsDescriptor = new CrossViewCellDescriptor();
        }

        #endregion

        public CrossViewHelper (CrossViewObject crossView)
        {
            this.CrossView = crossView;
            CreateOtherDescriptor();
        }
    }
}
