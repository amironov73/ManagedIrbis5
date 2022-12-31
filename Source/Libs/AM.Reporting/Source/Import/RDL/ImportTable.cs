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

using AM.Reporting.Matrix;
using AM.Reporting.Table;

using System;
using System.Collections.Generic;
using System.Xml;

#endregion

#nullable enable

namespace AM.Reporting.Import.RDL
{
    // Represents the RDL tables import
    public partial class RDLImport
    {
        private void LoadTableColumn (XmlNode tableColumnNode, TableColumn column)
        {
            var nodeList = tableColumnNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Width")
                {
                    column.Width = UnitsConverter.SizeToPixels (node.InnerText);
                }
                else if (node.Name == "Visibility")
                {
                    LoadVisibility (node);
                }
            }
        }

        private void LoadTableColumns (XmlNode tableColumnsNode)
        {
            if (tableColumnsNode != null)
            {
                var nodeList = tableColumnsNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name is "TableColumn" or "TablixColumn")
                    {
                        if (component is TableObject tableObject)
                        {
                            var column = new TableColumn();
                            tableObject.Columns.Add (column);
                            LoadTableColumn (node, column);
                        }
                    }
                }
            }
        }

        private void LoadTableCell (XmlNode tableCellNode, ref int col)
        {
            var row = (component as TableObject).RowCount - 1;
            var nodeList = tableCellNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name is "ReportItems" or "CellContents")
                {
                    var tempParent = parent;
                    var tempComponent = component;
                    parent = (component as TableObject).GetCellData (col, row).Cell;
                    LoadReportItems (node);
                    component = tempComponent;
                    parent = tempParent;
                }
                else if (node.Name == "ColSpan")
                {
                    var colSpan = Convert.ToInt32 (node.InnerText);
                    (component as TableObject).GetCellData (col, row).Cell.ColSpan = colSpan;
                    col += colSpan - 1;
                }
            }
        }

        private void LoadTableCells (XmlNode tableCellsNode)
        {
            var col = 0;
            var nodeList = tableCellsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name is "TableCell" or "TablixCell")
                {
                    LoadTableCell (node, ref col);
                    col++;
                }
            }
        }

        private void LoadTableRow (XmlNode tableRowNode, TableRow row)
        {
            var nodeList = tableRowNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name is "TableCells" or "TablixCells")
                {
                    LoadTableCells (node);
                }
                else if (node.Name == "Height")
                {
                    row.Height = UnitsConverter.SizeToPixels (node.InnerText);
                }
                else if (node.Name == "Visibility")
                {
                    LoadVisibility (node);
                }
            }
        }

        private void LoadTableRows (XmlNode tableRowsNode)
        {
            var nodeList = tableRowsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name is "TableRow" or "TablixRow")
                {
                    if (component is TableObject tableObject)
                    {
                        var row = new TableRow();
                        tableObject.Rows.Add (row);
                        LoadTableRow (node, row);
                    }
                }
            }
        }

        private void LoadHeader (XmlNode headerNode)
        {
            if (headerNode != null)
            {
                var nodeList = headerNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name is "TableRows" or "TablixRows")
                    {
                        LoadTableRows (node);
                    }
                }
            }
        }

        private void LoadTableGroup (XmlNode tableGroupNode)
        {
            var nodeList = tableGroupNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Header")
                {
                    LoadHeader (node);
                }
                else if (node.Name == "Footer")
                {
                    LoadFooter (node);
                }
                else if (node.Name == "Visibility")
                {
                    LoadVisibility (node);
                }
            }
        }

        private void LoadTableGroups (XmlNode tableGroupsNode)
        {
            var nodeList = tableGroupsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "TableGroup")
                {
                    LoadTableGroup (node);
                }
            }
        }

        private void LoadDetails (XmlNode detailsNode)
        {
            if (detailsNode != null)
            {
                var nodeList = detailsNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "TableRows")
                    {
                        LoadTableRows (node);
                    }
                    else if (node.Name == "Visibility")
                    {
                        LoadVisibility (node);
                    }
                }
            }
        }

        private void LoadFooter (XmlNode footerNode)
        {
            if (footerNode != null)
            {
                var nodeList = footerNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "TableRows")
                    {
                        LoadTableRows (node);
                    }
                }
            }
        }

        private void LoadCorner (XmlNode cornerNode)
        {
            if (cornerNode != null)
            {
                var nodeList = cornerNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "ReportItems")
                    {
                        //LoadReportItems(node);
                    }
                }
            }
        }

        private void LoadDynamicColumns (XmlNode dynamicColumnsNode, List<XmlNode> dynamicColumns)
        {
            var nodeList = dynamicColumnsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Subtotal")
                {
                    var subtotalNodeList = node.ChildNodes;
                    foreach (XmlNode subtotalNode in subtotalNodeList)
                    {
                        if (subtotalNode.Name == "ReportItems")
                        {
                            dynamicColumns.Add (subtotalNode.Clone());
                        }
                    }
                }
                else if (node.Name == "ReportItems")
                {
                    dynamicColumns.Add (node.Clone());
                }
                else if (node.Name == "Visibility")
                {
                    LoadVisibility (node);
                }
            }
        }

        private XmlNode LoadStaticColumn (XmlNode staticColumnNode)
        {
            XmlNode staticColumn = null;
            var nodeList = staticColumnNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "ReportItems")
                {
                    staticColumn = node.Clone();
                }
            }

            return staticColumn;
        }

        private void LoadStaticColumns (XmlNode staticColumnsNode, List<XmlNode> staticColumns)
        {
            var nodeList = staticColumnsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "StaticColumn")
                {
                    staticColumns.Add (LoadStaticColumn (node));
                }
            }
        }

        private float LoadColumnGrouping (XmlNode columnGroupingNode, List<XmlNode> dynamicColumns,
            List<XmlNode> staticColumns)
        {
            var cornerHeight = 0.8f * Utils.Units.Centimeters;
            var nodeList = columnGroupingNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Height")
                {
                    cornerHeight = UnitsConverter.SizeToPixels (node.InnerText);
                }
                else if (node.Name == "DynamicColumns")
                {
                    LoadDynamicColumns (node, dynamicColumns);
                }
                else if (node.Name == "StaticColumns")
                {
                    LoadStaticColumns (node, staticColumns);
                }
            }

            return cornerHeight;
        }

        private float LoadColumnGroupings (XmlNode columnGroupingsNode, List<XmlNode> dynamicColumns,
            List<XmlNode> staticColumns)
        {
            var cornerHeight = 0.8f * Utils.Units.Centimeters;
            if (columnGroupingsNode != null)
            {
                var nodeList = columnGroupingsNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "ColumnGrouping")
                    {
                        if (component is MatrixObject)
                        {
                            cornerHeight = LoadColumnGrouping (node, dynamicColumns, staticColumns);
                        }
                    }
                }
            }

            return cornerHeight;
        }

        private void LoadDynamicRows (XmlNode dynamicRowsNode, List<XmlNode> dynamicRows)
        {
            var nodeList = dynamicRowsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Subtotal")
                {
                    var subtotalNodeList = node.ChildNodes;
                    foreach (XmlNode subtotalNode in subtotalNodeList)
                    {
                        if (subtotalNode.Name == "ReportItems")
                        {
                            dynamicRows.Add (subtotalNode.Clone());
                        }
                    }
                }
                else if (node.Name == "ReportItems")
                {
                    dynamicRows.Add (node.Clone());
                }
                else if (node.Name == "Visibility")
                {
                    LoadVisibility (node);
                }
            }
        }

        private XmlNode LoadStaticRow (XmlNode staticRowNode)
        {
            XmlNode staticRow = null;
            var nodeList = staticRowNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "ReportItems")
                {
                    staticRow = node.Clone();
                }
            }

            return staticRow;
        }

        private void LoadStaticRows (XmlNode staticRowsNode, List<XmlNode> staticRows)
        {
            var nodeList = staticRowsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "StaticRow")
                {
                    staticRows.Add (LoadStaticRow (node));
                }
            }
        }

        private float LoadRowGrouping (XmlNode rowGroupingNode, List<XmlNode> dynamicRows, List<XmlNode> staticRows)
        {
            var cornerWidth = 2.5f * Utils.Units.Centimeters;
            var nodeList = rowGroupingNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Width")
                {
                    cornerWidth = UnitsConverter.SizeToPixels (node.InnerText);
                }
                else if (node.Name == "DynamicRows")
                {
                    LoadDynamicRows (node, dynamicRows);
                }
                else if (node.Name == "StaticRows")
                {
                    LoadStaticRows (node, staticRows);
                }
            }

            return cornerWidth;
        }

        private float LoadRowGroupings (XmlNode rowGroupingsNode, List<XmlNode> dynamicRows, List<XmlNode> staticRows)
        {
            var cornerWidth = 2.5f * Utils.Units.Centimeters;
            if (rowGroupingsNode != null)
            {
                var nodeList = rowGroupingsNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "RowGrouping")
                    {
                        if (component is MatrixObject)
                        {
                            cornerWidth = LoadRowGrouping (node, dynamicRows, staticRows);
                        }
                    }
                }
            }

            return cornerWidth;
        }

        private void LoadMatrixCell (XmlNode matrixCellNode, MatrixCellDescriptor cell, int col)
        {
            var row = (component as MatrixObject).RowCount - 1;
            var nodeList = matrixCellNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "ReportItems")
                {
                }
            }
        }

        private void LoadMatrixCells (XmlNode matrixCellsNode)
        {
            var col = 0;
            var nodeList = matrixCellsNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "MatrixCell")
                {
                    if (component is MatrixObject matrixObject)
                    {
                        var cell = new MatrixCellDescriptor();
                        matrixObject.Data.Cells.Add (cell);
                        LoadMatrixCell (node, cell, col);
                        col++;
                    }
                }
            }
        }

        private float LoadMatrixRow (XmlNode matrixRowNode, MatrixHeaderDescriptor row)
        {
            var rowHeight = 0.8f * Utils.Units.Centimeters;
            var nodeList = matrixRowNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Height")
                {
                    rowHeight = UnitsConverter.SizeToPixels (node.InnerText);
                }
                else if (node.Name == "MatrixCells")
                {
                    LoadMatrixCells (node);
                }
            }

            return rowHeight;
        }

        private float LoadMatrixRows (XmlNode matrixRowsNode)
        {
            var rowHeight = 0.8f * Utils.Units.Centimeters;
            if (matrixRowsNode != null)
            {
                var nodeList = matrixRowsNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "MatrixRow")
                    {
                        if (component is MatrixObject matrixObject)
                        {
                            var row = new MatrixHeaderDescriptor();
                            matrixObject.Data.Rows.Add (row);
                            rowHeight = LoadMatrixRow (node, row);
                        }
                    }
                }
            }

            return rowHeight;
        }

        private float LoadMatrixColumn (XmlNode matrixColumnNode, MatrixHeaderDescriptor column)
        {
            var columnWidth = 2.5f * Utils.Units.Centimeters;
            var nodeList = matrixColumnNode.ChildNodes;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "Width")
                {
                    columnWidth = UnitsConverter.SizeToPixels (node.InnerText);
                }
            }

            return columnWidth;
        }

        private float LoadMatrixColumns (XmlNode matrixColumnsNode)
        {
            var columnWidth = 2.5f * Utils.Units.Centimeters;
            if (matrixColumnsNode != null)
            {
                var nodeList = matrixColumnsNode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    if (node.Name == "MatrixColumn")
                    {
                        if (component is MatrixObject matrixObject)
                        {
                            var column = new MatrixHeaderDescriptor();
                            matrixObject.Data.Columns.Add (column);
                            columnWidth = LoadMatrixColumn (node, column);
                        }
                    }
                }
            }

            return columnWidth;
        }

        private void LoadTable (XmlNode tableNode)
        {
            if (parent is TableCell)
            {
                if (curBand != null)
                {
                    parent = curBand;
                }
                else
                {
                    return;
                }
            }

            component = ComponentsFactory.CreateTableObject (tableNode.Attributes["Name"].Value, parent);
            var nodeList = tableNode.ChildNodes;
            LoadReportItem (nodeList);
            XmlNode tableColumnsNode = null;
            XmlNode headerNode = null;
            XmlNode detailsNode = null;
            XmlNode footerNode = null;

            XmlNode tableRowsNode = null;
            foreach (XmlNode node in nodeList)
            {
                if (node.Name == "TableColumns")
                {
                    tableColumnsNode = node.Clone();
                }
                else if (node.Name == "Header")
                {
                    headerNode = node.Clone();
                }
                else if (node.Name == "Details")
                {
                    detailsNode = node.Clone();
                }
                else if (node.Name == "Footer")
                {
                    footerNode = node.Clone();
                }
                else if (node is { Name: "TablixBody", HasChildNodes: true })
                {
                    foreach (XmlNode bodyChild in node.ChildNodes)
                    {
                        if (bodyChild.Name == "TablixColumns")
                        {
                            tableColumnsNode = bodyChild.Clone();
                        }
                        else if (bodyChild.Name == "TablixRows")
                        {
                            tableRowsNode = node.Clone();
                        }
                    }
                }
            }

            LoadTableColumns (tableColumnsNode);
            LoadHeader (headerNode != null ? headerNode : tableRowsNode);
            LoadDetails (detailsNode);
            LoadFooter (footerNode);
            (component as TableObject).CreateUniqueNames();
        }

        private bool IsTablixMatrix (XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode tablixItem in node.ChildNodes)
                {
                    if (tablixItem.Name == "TablixCorner")
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private void LoadMatrix (XmlNode matrixNode)
        {
            component = ComponentsFactory.CreateMatrixObject (matrixNode.Attributes["Name"].Value, parent);
            var matrix = component as MatrixObject;
            matrix.AutoSize = false;

            var nodeList = matrixNode.ChildNodes;
            LoadReportItem (nodeList);

            //XmlNode cornerNode = null;
            XmlNode columnGroupingsNode = null;
            XmlNode rowGroupingsNode = null;
            XmlNode matrixRowsNode = null;
            XmlNode matrixColumnsNode = null;
            foreach (XmlNode node in nodeList)
            {
                //if (node.Name == "Corner")
                //{
                //    cornerNode = node.Clone();
                //}
                /*else */
                if (node.Name == "ColumnGroupings")
                {
                    columnGroupingsNode = node.Clone();
                }
                else if (node.Name == "RowGroupings")
                {
                    rowGroupingsNode = node.Clone();
                }
                else if (node.Name == "MatrixColumns")
                {
                    matrixColumnsNode = node.Clone();
                }
                else if (node.Name == "MatrixRows")
                {
                    matrixRowsNode = node.Clone();
                }
            }

            //LoadCorner(cornerNode);

            List<XmlNode> dynamicColumns = new List<XmlNode>();
            List<XmlNode> staticColumns = new List<XmlNode>();
            LoadColumnGroupings (columnGroupingsNode, dynamicColumns, staticColumns);

            List<XmlNode> dynamicRows = new List<XmlNode>();
            List<XmlNode> staticRows = new List<XmlNode>();
            LoadRowGroupings (rowGroupingsNode, dynamicRows, staticRows);

            var columnWidth = LoadMatrixColumns (matrixColumnsNode);
            var rowHeight = LoadMatrixRows (matrixRowsNode);

            matrix.CreateUniqueNames();
            matrix.BuildTemplate();

            for (var i = 1; i < matrix.Columns.Count; i++)
            {
                matrix.Columns[i].Width = columnWidth;
            }

            for (var i = 1; i < matrix.Rows.Count; i++)
            {
                matrix.Rows[i].Height = rowHeight;
            }

            for (var i = 0; i < matrix.Columns.Count; i++)
            {
                for (var j = 0; j < matrix.Rows.Count; j++)
                {
                    matrix.GetCellData (i, j).Cell.Text = "";
                }
            }

            for (var i = 0; i < dynamicColumns.Count; i++)
            {
                var tempParent = parent;
                var tempComponent = component;
                parent = matrix.GetCellData (i + 1, 0).Cell;
                LoadReportItems (dynamicColumns[i]);
                component = tempComponent;
                parent = tempParent;
            }

            for (var i = 0; i < dynamicRows.Count; i++)
            {
                var tempParent = parent;
                var tempComponent = component;
                parent = matrix.GetCellData (0, i + 1).Cell;
                LoadReportItems (dynamicRows[i]);
                component = tempComponent;
                parent = tempParent;
            }
        }
    }
}
