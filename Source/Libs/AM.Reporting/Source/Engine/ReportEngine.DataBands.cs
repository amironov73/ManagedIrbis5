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

using AM.Reporting.Data;
using AM.Reporting.Utils;

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Engine
{
    public partial class ReportEngine
    {
        #region Private Classes

        private class HierarchyItem
        {
            #region Fields

            public int rowNo;
            public List<HierarchyItem> items;
            public HierarchyItem parent;

            #endregion Fields

            #region Constructors

            public HierarchyItem()
            {
                items = new List<HierarchyItem>();
            }

            #endregion Constructors

            #region Public Methods

            public void Add (HierarchyItem item)
            {
                items.Add (item);
                item.parent = this;
            }

            #endregion Public Methods
        }

        #endregion Private Classes

        #region Private Methods

        private void RunDataBand (DataBand dataBand)
        {
            if (page.Columns.Count > 1 && Report.Engine.UnlimitedHeight)
            {
                dataBand.Columns.Count = page.Columns.Count;
            }

            dataBand.InitDataSource();
            dataBand.DataSource.First();

            var rowCount = dataBand.DataSource.RowCount;
            if (dataBand.IsDatasourceEmpty && dataBand.PrintIfDatasourceEmpty)
            {
                rowCount = 1;
            }

            if (dataBand.CollectChildRows && rowCount > 1)
            {
                rowCount = 1;
            }

            if (dataBand.MaxRows > 0 && rowCount > dataBand.MaxRows)
            {
                rowCount = dataBand.MaxRows;
            }

            var keepFirstRow = NeedKeepFirstRow (dataBand);
            var keepLastRow = NeedKeepLastRow (dataBand);

            RunDataBand (dataBand, rowCount, keepFirstRow, keepLastRow);

            // do not leave the datasource in EOF state to allow print something in the footer
            dataBand.DataSource.Prior();
        }

        private void RunDataBand (DataBand dataBand, int rowCount, bool keepFirstRow, bool keepLastRow)
        {
            if (dataBand.IsHierarchical)
            {
                ShowHierarchy (dataBand, rowCount);
                return;
            }

            var isFirstRow = true;
            var someRowsPrinted = false;
            dataBand.RowNo = 0;
            dataBand.IsFirstRow = false;
            dataBand.IsLastRow = false;

            // check if we have only one data row that should be kept with both header and footer
            var oneRow = rowCount == 1 && keepFirstRow && keepLastRow;

            // cycle through records
            for (var i = 0; i < rowCount; i++)
            {
                var isLastRow = i == rowCount - 1;
                if (!dataBand.IsDetailEmpty())
                {
                    dataBand.RowNo++;
                    dataBand.AbsRowNo++;
                    dataBand.IsFirstRow = isFirstRow;
                    dataBand.IsLastRow = isLastRow;

                    // keep header
                    if (isFirstRow && keepFirstRow)
                    {
                        StartKeep (dataBand);
                    }

                    // keep together
                    if (isFirstRow && dataBand.KeepTogether)
                    {
                        StartKeep (dataBand);
                    }

                    // keep detail
                    if (dataBand.KeepDetail)
                    {
                        StartKeep (dataBand);
                    }

                    // show header
                    if (isFirstRow)
                    {
                        ShowDataHeader (dataBand);
                    }

                    // keep footer
                    if (isLastRow && keepLastRow && dataBand.IsDeepmostDataBand)
                    {
                        StartKeep (dataBand);
                    }

                    // start block event
                    if (isFirstRow)
                    {
                        OnStateChanged (dataBand, EngineState.BlockStarted);
                    }

                    // show band
                    ShowDataBand (dataBand, rowCount);

                    // end keep header
                    if (isFirstRow && keepFirstRow && !oneRow)
                    {
                        EndKeep();
                    }

                    // end keep footer
                    if (isLastRow && keepLastRow && dataBand.IsDeepmostDataBand)
                    {
                        CheckKeepFooter (dataBand);
                    }

                    // show sub-bands
                    RunBands (dataBand.Bands);

                    // up the outline
                    OutlineUp (dataBand);

                    // end keep detail
                    if (dataBand.KeepDetail)
                    {
                        EndKeep();
                    }

                    isFirstRow = false;
                    someRowsPrinted = true;

                    if (dataBand.Columns.Count > 1)
                    {
                        break;
                    }
                }

                dataBand.DataSource.Next();
                if (Report.Aborted)
                {
                    break;
                }
            }

            // complete upto N rows
            var child = dataBand.Child;
            if (child != null && child.CompleteToNRows > rowCount)
            {
                for (var i = 0; i < child.CompleteToNRows - rowCount; i++)
                {
                    child.RowNo = rowCount + i + 1;
                    child.AbsRowNo = rowCount + i + 1;
                    ShowBand (child);
                }
            }

            // print child if databand is empty
            if (child != null && child.PrintIfDatabandEmpty && dataBand.IsDatasourceEmpty)
            {
                ShowBand (child);
            }

            if (someRowsPrinted)
            {
                // finish block event
                OnStateChanged (dataBand, EngineState.BlockFinished);

                // show footer
                ShowDataFooter (dataBand);

                // end KeepTogether
                if (dataBand.KeepTogether)
                {
                    EndKeep();
                }

                // end KeepLastRow
                if (keepLastRow)
                {
                    EndKeep();
                }
            }
        }

        private void ShowDataBand (DataBand dataBand, int rowCount)
        {
            if (dataBand.Columns.Count > 1)
            {
                dataBand.Width = dataBand.Columns.ActualWidth;
                RenderMultiColumnBand (dataBand, rowCount);
            }
            else
            {
                if (dataBand.ResetPageNumber && (dataBand.FirstRowStartsNewPage || dataBand.RowNo > 1))
                {
                    ResetLogicalPageNumber();
                }

                if (dataBand.Footer != null && dataBand.CanBreak)
                {
                    if (dataBand.Footer.KeepWithData && dataBand.Footer.Height + dataBand.Height > FreeSpace)
                    {
                        dataBand.AddLastToFooter (dataBand.Footer);
                    }
                }

                ShowBand (dataBand);
            }
        }

        private void RenderMultiColumnBand (DataBand dataBand, int rowCount)
        {
            if (dataBand.Columns.Layout == ColumnLayout.AcrossThenDown)
            {
                RenderBandAcrossThenDown (dataBand, rowCount);
            }
            else
            {
                var dataSource = dataBand.DataSource;
                var saveRow = dataSource.CurrentRowNo;

                // calc height of each data row. This list is shared across RenderBandDownThenAcross calls.
                var heights = new Hashtable();
                for (var i = 0; i < rowCount; i++)
                {
                    dataSource.CurrentRowNo = i + saveRow;
                    heights[i + saveRow] = CalcHeight (dataBand);
                }

                dataSource.CurrentRowNo = saveRow;
                while (rowCount > 0)
                {
                    rowCount = RenderBandDownThenAcross (dataBand, rowCount, heights);
                }
            }
        }

        private void RenderBandAcrossThenDown (DataBand dataBand, int rowCount)
        {
            var dataSource = dataBand.DataSource;
            var saveRow = dataSource.CurrentRowNo;
            var keepLastRow = NeedKeepLastRow (dataBand);

            // create output band
            using (var outputBand = new DataBand())
            {
                outputBand.SetReport (Report);

                var columnNo = 0;
                for (var i = 0; i < rowCount; i++)
                {
                    var isLastRow = i == rowCount - 1;
                    dataSource.CurrentRowNo = i + saveRow;
                    if (columnNo == 0)
                    {
                        outputBand.Clear();
                        outputBand.Assign (dataBand);
                        outputBand.OutlineExpression = "";
                        outputBand.Border = new Border();
                        outputBand.Fill = new SolidFill();
                    }

                    // write to the output band
                    if (Config.RightToLeft)
                    {
                        ShowBand (dataBand, outputBand,
                            dataBand.Columns.Positions[dataBand.Columns.Count - columnNo - 1], 0);
                    }
                    else
                    {
                        ShowBand (dataBand, outputBand, dataBand.Columns.Positions[columnNo], 0);
                    }

                    // add outline
                    AddBandOutline (dataBand);

                    // outline up
                    OutlineUp (dataBand);

                    dataBand.RowNo++;
                    dataBand.AbsRowNo++;
                    columnNo++;
                    if (columnNo == dataBand.Columns.Count || isLastRow)
                    {
                        columnNo = 0;

                        // keep footer
                        if (isLastRow && keepLastRow && dataBand.IsDeepmostDataBand)
                        {
                            StartKeep (outputBand);
                        }

                        // show output band itself
                        ShowBand (outputBand, false);

                        // end keep footer
                        if (isLastRow && keepLastRow && dataBand.IsDeepmostDataBand)
                        {
                            CheckKeepFooter (dataBand);
                        }
                    }
                }
            }

            dataSource.CurrentRowNo = saveRow + rowCount;
        }

        private int RenderBandDownThenAcross (DataBand dataBand, int rowCount, Hashtable heights)
        {
            var dataSource = dataBand.DataSource;
            var saveRow = dataSource.CurrentRowNo;

            // determine number of rows in a column. Do not take the height into account - it's too complex.
            var rowsPerColumn = (int)Math.Ceiling ((float)rowCount / dataBand.Columns.Count);
            if (rowsPerColumn < dataBand.Columns.MinRowCount)
            {
                rowsPerColumn = dataBand.Columns.MinRowCount;
            }

            // calculate max height of all columns to check the free space
            float maxHeight = 0;
            for (var i = 0; i < dataBand.Columns.Count; i++)
            {
                // calculate column height
                float columnHeight = 0;
                for (var j = 0; j < rowsPerColumn; j++)
                {
                    var rowIndex = j + i * rowsPerColumn + saveRow;
                    if (heights[rowIndex] == null)
                    {
                        break;
                    }

                    columnHeight += (float)heights[rowIndex];
                }

                if (columnHeight > maxHeight)
                {
                    maxHeight = columnHeight;
                }
            }

            var saveCurX = CurX;
            var startColumnY = CurY;
            var columnNo = 0;

            if (maxHeight > FreeSpace)
            {
                // not enough free space. Render rows down then across. After finishing the page,
                // run this method again to render remaining rows.

                for (var i = 0; i < rowCount; i++)
                {
                    dataSource.CurrentRowNo = i + saveRow;

                    if (Config.RightToLeft)
                    {
                        CurX = dataBand.Columns.Positions[dataBand.Columns.Count - columnNo - 1] + saveCurX;
                    }
                    else
                    {
                        CurX = dataBand.Columns.Positions[columnNo] + saveCurX;
                    }

                    // check free space.
                    if ((float)heights[i + saveRow] > FreeSpace && i != 0)
                    {
                        columnNo++;
                        if (columnNo == dataBand.Columns.Count)
                        {
                            // start the new page
                            columnNo = 0;
                            CurX = saveCurX;
                            EndPage();

                            // decrease number of available rows and call this method again
                            rowCount -= i;
                            return rowCount;
                        }
                        else
                        {
                            // start the new column
                            CurY = startColumnY;
                        }

                        // check free space again before show a band - we may be at a page end
                        i--;
                        continue;
                    }

                    if (Config.RightToLeft)
                    {
                        CurX = dataBand.Columns.Positions[dataBand.Columns.Count - columnNo - 1] + saveCurX;
                    }
                    else
                    {
                        CurX = dataBand.Columns.Positions[columnNo] + saveCurX;
                    }

                    // show a band
                    ShowBand (dataBand);
                    dataBand.RowNo++;
                    dataBand.AbsRowNo++;
                }

                // we shouldn't go here...
            }
            else
            {
                // we have enough space to render all rows.
                var maxY = CurY;
                var rowNo = 0;

                var carryoutLastRow = false;
                if (dataBand.IsDeepmostDataBand && NeedKeepLastRow (dataBand))
                {
                    var footersHeight = GetFootersHeight (dataBand);
                    if (footersHeight > FreeSpace - maxHeight)
                    {
                        carryoutLastRow = true;
                    }
                }

                for (var i = 0; i < rowCount; i++)
                {
                    dataSource.CurrentRowNo = i + saveRow;

                    // carry out the last row
                    var isLastRow = i == rowCount - 1;
                    if (isLastRow && carryoutLastRow)
                    {
                        CurX = saveCurX;
                        CurY = maxY;
                        EndColumn();
                        ShowBand (dataBand);
                        OutlineUp (dataBand);
                        dataSource.CurrentRowNo = saveRow + rowCount;
                        return 0;
                    }

                    if (Config.RightToLeft)
                    {
                        CurX = dataBand.Columns.Positions[dataBand.Columns.Count - columnNo - 1] + saveCurX;
                    }
                    else
                    {
                        CurX = dataBand.Columns.Positions[columnNo] + saveCurX;
                    }

                    // show a band
                    ShowBand (dataBand);

                    // outline up
                    OutlineUp (dataBand);
                    if (CurY > maxY)
                    {
                        maxY = CurY;
                    }

                    dataBand.RowNo++;
                    dataBand.AbsRowNo++;
                    rowNo++;

                    // check if we need to start a new column
                    if (rowNo >= rowsPerColumn)
                    {
                        columnNo++;
                        rowNo = 0;
                        CurY = startColumnY;
                    }
                }

                CurX = saveCurX;
                CurY = maxY;
            }

            dataSource.CurrentRowNo = saveRow + rowCount;
            return 0;
        }

        private float GetColumnHeight (Hashtable heights, int startIndex, int rowCount, int rowNo)
        {
            float result = 0;
            for (var i = 0; i < rowCount; i++)
            {
                if (i + startIndex >= heights.Count)
                {
                    break;
                }

                result += (float)heights[i + startIndex + rowNo];
            }

            return result;
        }

        private HierarchyItem MakeHierarchy (DataBand dataBand, int rowCount)
        {
            Dictionary<string, HierarchyItem> items = new Dictionary<string, HierarchyItem>();

            var idColumn = DataHelper.GetColumn (Report.Dictionary, dataBand.IdColumn);
            if (idColumn == null)
            {
                return null;
            }

            var parentIdColumn = DataHelper.GetColumn (Report.Dictionary, dataBand.ParentIdColumn);
            if (parentIdColumn == null)
            {
                return null;
            }

            for (var i = 0; i < rowCount; i++)
            {
                var idColumnValue = dataBand.DataSource[idColumn];
                var parentIdColumnValue = dataBand.DataSource[parentIdColumn];
                var id = (idColumnValue == null || idColumnValue is DBNull) ? "" : idColumnValue.ToString();
                var parentId = (parentIdColumnValue == null || parentIdColumnValue is DBNull)
                    ? ""
                    : parentIdColumnValue.ToString();

                // avoid wrong behavior if wrong data specified
                if (id == parentId)
                {
                    parentId = "";
                }

                HierarchyItem item = null;
                if (items.ContainsKey (id))
                {
                    item = items[id];
                }
                else
                {
                    item = new HierarchyItem();
                    items.Add (id, item);
                }

                item.rowNo = dataBand.DataSource.CurrentRowNo;

                HierarchyItem parentItem = null;
                if (items.ContainsKey (parentId))
                {
                    parentItem = items[parentId];
                }
                else
                {
                    parentItem = new HierarchyItem();
                    items.Add (parentId, parentItem);
                }

                parentItem.Add (item);

                dataBand.DataSource.Next();
            }

            // create the root item
            var rootItem = new HierarchyItem();
            foreach (var item in items.Values)
            {
                if (item.parent == null)
                {
                    foreach (var childItem in item.items)
                    {
                        rootItem.Add (childItem);
                    }
                }
            }

            return rootItem;
        }

        private void ShowHierarchy (DataBand dataBand, HierarchyItem rootItem, int level, string fullRowNo)
        {
            var saveLevel = HierarchyLevel;
            var saveIndent = hierarchyIndent;
            var saveRowNo = HierarchyRowNo;
            HierarchyLevel = level;
            hierarchyIndent = dataBand.Indent * (level - 1);

            try
            {
                if (rootItem.items.Count > 0)
                {
                    ShowBand (dataBand.Header);

                    var rowNo = 0;
                    foreach (var item in rootItem.items)
                    {
                        rowNo++;
                        dataBand.RowNo = rowNo;
                        dataBand.AbsRowNo++;
                        dataBand.DataSource.CurrentRowNo = item.rowNo;
                        HierarchyRowNo = fullRowNo + rowNo.ToString();

                        // show the main hierarchy band
                        ShowBand (dataBand);

                        // show sub-bands if any
                        RunBands (dataBand.Bands);

                        ShowHierarchy (dataBand, item, level + 1, HierarchyRowNo + ".");

                        // up the outline
                        OutlineUp (dataBand);
                    }

                    ShowBand (dataBand.Footer);
                }
            }
            finally
            {
                HierarchyLevel = saveLevel;
                hierarchyIndent = saveIndent;
                HierarchyRowNo = saveRowNo;
            }
        }

        private void ShowHierarchy (DataBand dataBand, int rowCount)
        {
            var rootItem = MakeHierarchy (dataBand, rowCount);
            if (rootItem == null)
            {
                return;
            }

            ShowHierarchy (dataBand, rootItem, 1, "");
        }

        private void ShowDataHeader (DataBand dataBand)
        {
            var header = dataBand.Header;
            if (header != null)
            {
                ShowBand (header);
                if (header.RepeatOnEveryPage)
                {
                    AddReprint (header);
                }
            }

            var footer = dataBand.Footer;
            if (footer != null)
            {
                if (footer.RepeatOnEveryPage)
                {
                    AddReprint (footer);
                }
            }
        }

        private void ShowDataFooter (DataBand dataBand)
        {
            dataBand.DataSource.Prior();

            var footer = dataBand.Footer;
            RemoveReprint (footer);
            ShowBand (footer);
            RemoveReprint (dataBand.Header);

            dataBand.DataSource.Next();
        }

        #endregion Private Methods
    }
}
