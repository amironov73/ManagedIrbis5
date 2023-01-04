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

using AM.Reporting.Barcode;
using AM.Reporting.Table;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;

#endregion

#nullable enable

namespace AM.Reporting.Import.DevExpress
{
    partial class DevExpressImport
    {
        #region Fields

        XmlDocument devDoc;
        private XmlNode reportNode;
        private XmlNode bandsNode;
        private XmlNode localizationItemsNode;

        #endregion Fields

        #region Private Methods

        #region HelperMethods

        private bool AttributeExist (XmlNode node, string attributeName)
        {
            var attr = node.Attributes[attributeName];
            return attr != null;
        }

        private string GetAttribute (XmlNode node, string attributeName)
        {
            if (AttributeExist (node, attributeName))
            {
                return node.Attributes[attributeName].Value;
            }

            return string.Empty;
        }

        private XmlNode FindNode (XmlNode xmlNode, string nodeName, string attributeName)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                var type = GetAttribute (node, attributeName);
                if (string.IsNullOrEmpty (type))
                {
                    continue;
                }

                if (type == nodeName)
                {
                    return node;
                }
            }

            return null;
        }

        private XmlNode FindChildNoteByName (XmlNode xmlNode, string nodeName)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name.ToLower() == nodeName.ToLower())
                {
                    return node;
                }
            }

            return null;
        }

        private string GetObjectType (XmlNode node)
        {
            if (!AttributeExist (node, "ControlType"))
            {
                return string.Empty;
            }

            return node.Attributes["ControlType"].Value;
        }

        private XmlNode FindBandNode (XmlNode xmlNode, string nodeName)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                var type = GetAttribute (node, "ControlType");
                if (string.IsNullOrEmpty (type))
                {
                    continue;
                }

                if (type == nodeName)
                {
                    return node;
                }
            }

            return null;
        }

        private XmlNode FindBandNode (string nodeName)
        {
            return FindBandNode (bandsNode, nodeName);
        }

        private List<XmlNode> GetListOfBandsByName (XmlNode node, string bandType)
        {
            List<XmlNode> bands = new List<XmlNode>();
            foreach (XmlNode childNode in node.ChildNodes)
            {
                if (GetObjectType (childNode).Equals (bandType))
                {
                    bands.Add (childNode);
                }
            }

            return bands;
        }

        private void ApplyStyle (XmlNode node, ReportComponentBase component)
        {
            var stylename = GetAttribute (node, "StyleName");
            var stylePriority = FindChildNoteByName (node, "StylePriority");

            //if (stylePriority == null)
            //    return;
            var styleNode = GetStyleNode (stylename);
            foreach (Style style in Report.Styles)
            {
                if (style.Name == stylename)
                {
                    if (stylePriority == null || !AttributeExist (stylePriority, "UseFont"))
                    {
                        if (component as TextObject != null)
                        {
                            (component as TextObject).Font = style.Font;
                        }
                    }

                    if (stylePriority == null || !AttributeExist (stylePriority, "UseTextAlignment"))
                    {
                        if (component as TextObject != null)
                        {
                            (component as TextObject).HorizontalAlign =
                                UnitsConverter.ConvertTextAlignmentToHorzAlign (GetAttribute (styleNode,
                                    "TextAlignment"));
                            (component as TextObject).VerticalAlign =
                                UnitsConverter.ConvertTextAlignmentToVertAlign (GetAttribute (styleNode,
                                    "TextAlignment"));
                        }
                    }

                    if (stylePriority == null || !AttributeExist (stylePriority, "UseBorder"))
                    {
                        component.Border = style.Border;
                    }

                    if (stylePriority == null || !AttributeExist (stylePriority, "UseForeColor"))
                    {
                        if (component as TextObject != null)
                        {
                            (component as TextObject).TextColor = (style.TextFill as SolidFill).Color;
                        }
                    }
                }
            }
        }

        private XmlNode GetStyleNode (string stylename)
        {
            var styleNode = FindChildNoteByName (reportNode, "StyleSheet");
            if (styleNode == null)
            {
                return null;
            }

            var styles = styleNode.ChildNodes;
            foreach (XmlNode style in styles)
            {
                if (GetAttribute (style, "Name") == stylename)
                {
                    return style;
                }
            }

            return null;
        }

        #endregion

        #region LoadObjectsMethods

        private void LoadBorder (XmlNode node, Border border)
        {
            var sides = GetAttribute (node, "Sides");
            if (string.IsNullOrEmpty (sides))
            {
                sides = GetAttribute (node, "Borders");
            }

            border.Lines = UnitsConverter.ConvertBorderSides (sides, border);
            border.Color = UnitsConverter.ConvertColor (GetAttribute (node, "BorderColor"));
            border.Width = UnitsConverter.SizeFToPixels (GetAttribute (node, "BorderWidthSerializable"));
            border.Style = UnitsConverter.ConvertBorderDashStyle (GetAttribute (node, "BorderDashStyle"));
        }

        private Font LoadFontXml (string fontString)
        {
            if (string.IsNullOrEmpty (fontString))
            {
                return new Font ("Arial", 10, FontStyle.Regular);
            }

            string[] fontParts = fontString.Split (',');

            var fontFamily = fontParts[0];
            var fontSize = UnitsConverter.SizeFToPixels (fontParts[1].Substring (0, fontParts[1].Length - 2));

            if (fontString.IndexOf ("style=") != -1)
            {
                var styles = fontString.Substring (fontString.IndexOf ("style=") + 6);
                var fontStyle = FontStyle.Regular;
                if (styles.Contains ("Bold"))
                {
                    fontStyle |= FontStyle.Bold;
                }

                if (styles.Contains ("Italic"))
                {
                    fontStyle |= FontStyle.Italic;
                }

                if (styles.Contains ("Underline"))
                {
                    fontStyle |= FontStyle.Underline;
                }

                if (styles.Contains ("Strikeout"))
                {
                    fontStyle |= FontStyle.Strikeout;
                }

                return new Font (fontFamily, fontSize, fontStyle);
            }

            return new Font (fontFamily, fontSize, FontStyle.Regular);
        }

        private void LoadBand (XmlNode node, BandBase band)
        {
            if (AttributeExist (node, "HeightF"))
            {
                band.Height = UnitsConverter.SizeFToPixels (GetAttribute (node, "HeightF"));
            }
            else
            {
                band.Height = 100;
            }

            band.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
        }

        private void LoadObjects (XmlNode node, Base parent)
        {
            foreach (XmlNode nodeType in node.ChildNodes)
            {
                if (nodeType.Name.Equals ("Controls"))
                {
                    foreach (XmlNode control in nodeType.ChildNodes)
                    {
                        switch (GetObjectType (control))
                        {
                            case "XRLabel":
                                LoadLabel (control, parent);
                                break;
                            case "XRLine":
                                LoadLine (control, parent);
                                break;
                            case "XRPictureBox":
                                LoadPicture (control, parent);
                                break;
                            case "XRShape":
                                LoadShape (control, parent);
                                break;
                            case "XRZipCode":
                                LoadZipCode (control, parent);
                                break;
                            case "XRBarCode":
                                LoadBarcode (control, parent);
                                break;
                            case "XRRichText":
                                LoadRichText (control, parent);
                                break;
                            case "XRTable":
                                LoadTable (control, parent);
                                break;
                            case "XRCharacterComb":
                                LoadCellural (control, parent);
                                break;
                            case "XRPanel":
                                LoadPanel (control, parent);
                                break;
                            case "XRCheckBox":
                                LoadCheckBox (control, parent);
                                break;
                        }
                    }
                }
            }
        }

        private void LoadComponent (XmlNode node, ComponentBase comp)
        {
            comp.Name = GetAttribute (node, "Name");
            var location = GetAttribute (node, "LocationFloat");
            if (!string.IsNullOrEmpty (location))
            {
                string[] points = location.Split (',');
                comp.Left = UnitsConverter.SizeFToPixels (points[0]);
                comp.Top = UnitsConverter.SizeFToPixels (points[1]);
            }
        }

        private void LoadSize (XmlNode node, ComponentBase comp)
        {
            var size = GetAttribute (node, "SizeF");
            if (!string.IsNullOrEmpty (size))
            {
                string[] sizes = size.Split (',');
                comp.Width = UnitsConverter.SizeFToPixels (sizes[0]);
                comp.Height = UnitsConverter.SizeFToPixels (sizes[1]);
            }
        }

        private void LoadLabel (XmlNode node, Base parent)
        {
            var text = ComponentsFactory.CreateTextObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, text);
            LoadSize (node, text);
            LoadBorder (node, text.Border);
            text.Font = LoadFontXml (GetAttribute (node, "Font"));
            text.Text = GetAttribute (node, "Text");
            text.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
            text.TextColor = UnitsConverter.ConvertColor (GetAttribute (node, "ForeColor"));
            text.HorizontalAlign = UnitsConverter.ConvertTextAlignmentToHorzAlign (GetAttribute (node, "TextAlignment"));
            text.VerticalAlign = UnitsConverter.ConvertTextAlignmentToVertAlign (GetAttribute (node, "TextAlignment"));
            ApplyStyle (node, text);
        }

        private void LoadLine (XmlNode node, Base parent)
        {
            var line = ComponentsFactory.CreateLineObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, line);
            LoadSize (node, line);
            LoadBorder (node, line.Border);
            var width = GetAttribute (node, "LineWidth");
            if (!string.IsNullOrEmpty (width))
            {
                line.Border.Width = Convert.ToSingle (width);
            }
        }

        private void LoadPicture (XmlNode node, Base parent)
        {
            var picture = ComponentsFactory.CreatePictureObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, picture);
            LoadSize (node, picture);
            picture.SizeMode = UnitsConverter.ConvertImageSizeMode (GetAttribute (node, "Sizing"));
            picture.ImageAlign = UnitsConverter.ConvertImageAlignment (GetAttribute (node, "ImageAlignment"));
            ApplyStyle (node, picture);
        }

        private void LoadShape (XmlNode node, Base parent)
        {
            var shape = ComponentsFactory.CreateShapeObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, shape);
            LoadSize (node, shape);
            shape.Border.Color = UnitsConverter.ConvertColor (GetAttribute (node, "ForeColor"));
            shape.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
            shape.Shape = UnitsConverter.ConvertShape (GetAttribute (node, "Shape"));
            var width = GetAttribute (node, "Width");
            if (!string.IsNullOrEmpty (width))
            {
                shape.Border.Width = Convert.ToSingle (width);
            }

            ApplyStyle (node, shape);
        }

        private void LoadZipCode (XmlNode node, Base parent)
        {
            var zipCode = ComponentsFactory.CreateZipCodeObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, zipCode);
            LoadSize (node, zipCode);
            zipCode.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
            zipCode.Border.Color = UnitsConverter.ConvertColor (GetAttribute (node, "ForeColor"));
            zipCode.Text = GetAttribute (node, "Text").Replace ("\"", "");
            ApplyStyle (node, zipCode);
        }

        private void LoadBarcode (XmlNode node, Base parent)
        {
            var barcode = ComponentsFactory.CreateBarcodeObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            barcode.AutoSize = false;
            LoadComponent (node, barcode);
            LoadSize (node, barcode);
            LoadBorder (node, barcode.Border);
            barcode.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
            var symbology = GetAttribute (FindChildNoteByName (node, "symbology"), "Name");
            UnitsConverter.ConvertBarcodeSymbology (symbology, barcode);
            barcode.Text = GetAttribute (node, "Text");
            ApplyStyle (node, barcode);
            barcode.ShowText = !AttributeExist (node, "ShowText");
        }

        partial void LoadRichText (XmlNode node, Base parent);

        private void LoadTable (XmlNode node, Base parent)
        {
            var table = ComponentsFactory.CreateTableObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, table);
            LoadSize (node, table);
            LoadBorder (node, table.Border);
            ApplyStyle (node, table);
            var tableInfo = new TableInfo();
            var rowsNode = FindChildNoteByName (node, "Rows");

            var columnsCount = 0;

            foreach (XmlNode row in rowsNode.ChildNodes)
            {
                var cells = FindChildNoteByName (row, "Cells");

                if (columnsCount < cells.ChildNodes.Count)
                {
                    columnsCount = cells.ChildNodes.Count;

                    foreach (XmlNode cell in cells.ChildNodes)
                    {
                        AddLocalizationItemsAttributes (cell);
                        tableInfo.Column.Add (UnitsConverter.SizeFToPixels (GetAttribute (cell, "Weight")));
                    }
                }
            }

            foreach (XmlNode row in rowsNode)
            {
                AddLocalizationItemsAttributes (row);
                tableInfo.Row.Add (UnitsConverter.SizeFToPixels (GetAttribute (row, "Weight")));
            }

            for (var i = 0; i < columnsCount; i++)
            {
                var column = new TableColumn();
                column.Width = GetRowColumnSize (tableInfo.Column, i, table.Width);
                table.Columns.Add (column);
                column.CreateUniqueName();
            }

            for (var i = 0; i < rowsNode.ChildNodes.Count; i++)
            {
                var rowNode = rowsNode.ChildNodes[i];

                var row = new TableRow();
                row.Name = GetAttribute (rowNode, "Name");
                row.Height = GetRowColumnSize (tableInfo.Row, i, table.Height);
                var cells = FindChildNoteByName (rowNode, "Cells");

                for (var j = 0; j < cells.ChildNodes.Count; j++)
                {
                    var cellNode = cells.ChildNodes[j];
                    var cell = new TableCell();
                    LoadTableCell (cellNode, cell);
                    if (j == cells.ChildNodes.Count - 1 && cells.ChildNodes.Count < columnsCount)
                    {
                        cell.ColSpan = columnsCount - cells.ChildNodes.Count + 1;
                    }

                    row.AddChild (cell);
                }

                table.Rows.Add (row);
            }
        }

        private void LoadTableCell (XmlNode node, TableCell cell)
        {
            AddLocalizationItemsAttributes (node);
            cell.Name = GetAttribute (node, "Name");
            cell.Text = GetAttribute (node, "Text");
            cell.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
            cell.TextColor = UnitsConverter.ConvertColor (GetAttribute (node, "ForeColor"));
            cell.HorizontalAlign = UnitsConverter.ConvertTextAlignmentToHorzAlign (GetAttribute (node, "TextAlignment"));
            cell.VerticalAlign = UnitsConverter.ConvertTextAlignmentToVertAlign (GetAttribute (node, "TextAlignment"));
            cell.Font = LoadFontXml (GetAttribute (node, "Font"));
            ApplyStyle (node, cell);
            LoadObjects (node, cell);
        }

        private void LoadCellural (XmlNode node, Base parent)
        {
            var cellular = ComponentsFactory.CreateCellularTextObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, cellular);
            LoadSize (node, cellular);
            LoadBorder (node, cellular.Border);
            if (GetAttribute (node, "Borders").Equals (string.Empty))
            {
                cellular.Border.Lines = BorderLines.All;
            }

            cellular.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
            cellular.Text = GetAttribute (node, "Text");
        }

        private void LoadPanel (XmlNode node, Base parent)
        {
            var panel = ComponentsFactory.CreateContainerObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, panel);
            LoadSize (node, panel);
            LoadBorder (node, panel.Border);
            panel.FillColor = UnitsConverter.ConvertBackColor (GetAttribute (node, "BackColor"));
            LoadObjects (node, panel);
        }

        private void LoadCheckBox (XmlNode node, Base parent)
        {
            var checkBox = ComponentsFactory.CreateCheckBoxObject (node.Name, parent);
            AddLocalizationItemsAttributes (node);
            LoadComponent (node, checkBox);
            LoadSize (node, checkBox);
            LoadBorder (node, checkBox.Border);
            checkBox.Checked = UnitsConverter.ConvertBool (GetAttribute (node, "Checked"));
        }

        #endregion

        private void LoadReportXml()
        {
            reportNode = devDoc.ChildNodes[1];
            bandsNode = FindChildNoteByName (reportNode, "Bands");
            localizationItemsNode = FindChildNoteByName (reportNode, "LocalizationItems");
            LoadPageSizeXml();
            LoadStylesXml();
            LoadPageHeaderBandXml();
            LoadPageFooterBandXml();
            LoadReportHeaderBandXml();
            LoadReportFooterBandXml();
            LoadReportDetailBandXml();
            LoadDetailReports();
        }

        private void LoadPageSizeXml()
        {
            var height = GetAttribute (reportNode, "PageHeight");
            var width = GetAttribute (reportNode, "PageWidth");
            if (!string.IsNullOrEmpty (height))
            {
                page.PaperHeight = UnitsConverter.SizeFToPixels (height) / Utils.Units.Millimeters;
            }

            if (!string.IsNullOrEmpty (width))
            {
                page.PaperWidth = UnitsConverter.SizeFToPixels (width) / Utils.Units.Millimeters;
            }
        }

        private void LoadStylesXml()
        {
            var styleNode = FindChildNoteByName (reportNode, "StyleSheet");
            if (styleNode == null)
            {
                return;
            }

            var styles = styleNode.ChildNodes;
            foreach (XmlNode styleName in styles)
            {
                var style = ComponentsFactory.CreateStyle (GetAttribute (styleName, "Name"), Report);
                LoadBorder (styleName, style.Border);
                style.TextFill = new SolidFill (UnitsConverter.ConvertColor (GetAttribute (styleName, "ForeColor")));
                if (AttributeExist (styleName, "BackColor"))
                {
                    style.Fill =
                        new SolidFill (UnitsConverter.ConvertBackColor (GetAttribute (styleName, "BackColor")));
                }

                style.Font = LoadFontXml (GetAttribute (styleName, "Font"));
            }
        }

        private void LoadPageHeaderBandXml()
        {
            var node = FindBandNode ("PageHeaderBand");
            if (node == null)
            {
                return;
            }

            var header = ComponentsFactory.CreatePageHeaderBand (page);
            LoadBand (node, header);
            LoadObjects (node, header);
        }

        private void LoadPageFooterBandXml()
        {
            var node = FindBandNode ("PageFooterBand");
            if (node == null)
            {
                return;
            }

            var footer = ComponentsFactory.CreatePageFooterBand (page);
            LoadBand (node, footer);
            LoadObjects (node, footer);
        }

        private void LoadReportHeaderBandXml()
        {
            var node = FindBandNode ("ReportHeaderBand");
            if (node == null)
            {
                return;
            }

            var header = ComponentsFactory.CreateReportTitleBand (page);
            LoadBand (node, header);
            LoadObjects (node, header);
        }

        private void LoadReportFooterBandXml()
        {
            var node = FindBandNode ("ReportFooterBand");
            if (node == null)
            {
                return;
            }

            var summary = ComponentsFactory.CreateReportSummaryBand (page);
            LoadBand (node, summary);
            LoadObjects (node, summary);
        }

        private void LoadReportDetailBandXml()
        {
            var node = FindBandNode ("DetailBand");
            if (node == null)
            {
                return;
            }

            var data = ComponentsFactory.CreateDataBand (page);
            AddLocalizationItemsAttributes (node);
            LoadBand (node, data);
            LoadObjects (node, data);
            var subbands = FindChildNoteByName (node, "SubBands");
            if (subbands != null)
            {
                foreach (XmlNode subband in subbands.ChildNodes)
                {
                    LoadSubBand (subband, data);
                }
            }
        }

        private void LoadSubBand (XmlNode node, Base parent)
        {
            var child = ComponentsFactory.CreateChildBand (parent as BandBase);
            LoadBand (node, child);
            LoadObjects (node, child);
        }

        private void AddLocalizationItemsAttributes (XmlNode node)
        {
            if (localizationItemsNode == null)
            {
                return;
            }

            var nodeRef = GetAttribute (node, "Ref");
            List<XmlAttribute> attributes = new List<XmlAttribute>();
            foreach (XmlNode liNode in localizationItemsNode.ChildNodes)
            {
                if (GetAttribute (liNode, "Component") == ($"#Ref-{nodeRef}"))
                {
                    if (liNode.Attributes["Culture"].Value != "Default")
                    {
                        continue;
                    }

                    var attribute = devDoc.CreateAttribute (GetAttribute (liNode, "Path"));
                    attribute.Value = GetAttribute (liNode, "Data");
                    node.Attributes.Append (attribute);

                    //XmlAttribute attribute = devDoc.CreateAttribute(GetAttribute(liNode, "Path"));
                    //string value = GetAttribute(liNode, "Data");
                    //if (attribute.Name == "LocationFloat" || attribute.Name == "SizeF")
                    //{
                    //    if (value.Contains("E"))
                    //        attribute.Value = value.Substring(0, value.IndexOf(",", 2)).Replace(',', '.');
                    //    else
                    //        attribute.Value = value;
                    //}
                    //else
                    //    attribute.Value = value;

                    //if (AttributeExist(node, attribute.Name))
                    //{
                    //    if (attribute.Name == "LocationFloat" || attribute.Name == "SizeF")
                    //        node.Attributes[attribute.Name].Value += "," + value;
                    //    else
                    //        node.Attributes[attribute.Name].Value = value.Replace(',', '.');
                    //}
                    //else
                    //    node.Attributes.Append(attribute);
                }
            }
        }

        private void LoadDetailReports()
        {
            List<XmlNode> detailReports = GetListOfBandsByName (bandsNode, "DetailReportBand");

            var level = 0;
            for (var i = 0; i < detailReports.Count; i++)
            {
                var detail = detailReports.Where (x => GetAttribute (x, "Level") == level.ToString())
                    .FirstOrDefault();
                LoadDetailReport (detail);
                level++;
            }
        }

        private void LoadDetailReport (XmlNode detailReportNode)
        {
            var detailReportBands = FindChildNoteByName (detailReportNode, "Bands");
            if (detailReportBands == null)
            {
                return;
            }

            var node = FindBandNode (detailReportBands, "DetailBand");
            if (node == null)
            {
                return;
            }

            var data = ComponentsFactory.CreateDataBand (page);
            LoadBand (node, data);
            LoadObjects (node, data);
            LoadDetailReportHeaderFooter (detailReportBands, data);
            LoadDetailReportGroupBands (detailReportBands, data);
        }

        private void LoadDetailReportHeaderFooter (XmlNode detailReportBands, DataBand data)
        {
            var header = FindBandNode (detailReportBands, "ReportHeaderBand");
            var footer = FindBandNode (detailReportBands, "ReportFooterBand");

            var headerBand = LoadDetailReportGroupHeader (header, data);
            LoadDetailReportGroupFooter (footer, headerBand);
        }

        private GroupHeaderBand LoadDetailReportGroupHeader (XmlNode headerNode, DataBand data)
        {
            if (headerNode == null)
            {
                return null;
            }

            var header = new GroupHeaderBand();

            if (data.Parent is GroupHeaderBand parent)
            {
                header.Data = data;
                parent.NestedGroup = header;
            }
            else
            {
                header.Data = data;
                page.Bands.Add (header);
            }

            LoadBand (headerNode, header);
            LoadObjects (headerNode, header);

            return header;
        }

        private void LoadDetailReportGroupFooter (XmlNode footerNode, GroupHeaderBand header)
        {
            if (footerNode == null || header == null)
            {
                return;
            }

            var footer = new GroupFooterBand();
            header.GroupFooter = footer;
            LoadBand (footerNode, footer);
            LoadObjects (footerNode, footer);
        }

        private void LoadDetailReportGroupBands (XmlNode detailReportBands, DataBand data)
        {
            List<XmlNode> headers = GetListOfBandsByName (detailReportBands, "GroupHeaderBand");
            List<XmlNode> footers = GetListOfBandsByName (detailReportBands, "GroupFooterBand");
            var level = -1;
            for (var i = 0; i < headers.Count; i++)
            {
                level++;
                XmlNode header;

                if (i == 0)
                {
                    header = headers.Where (x => GetAttribute (x, "Level") == string.Empty).FirstOrDefault();
                }
                else
                {
                    header = headers.Where (x => GetAttribute (x, "Level") == level.ToString()).FirstOrDefault();
                }

                var headerBand = LoadDetailReportGroupHeader (header, data);

                if (header == null)
                {
                    continue;
                }

                var headerLevel = GetAttribute (header, "Level");

                var footer = footers.Where (x => GetAttribute (x, "Level").Equals (headerLevel)).FirstOrDefault();
                LoadDetailReportGroupFooter (footer, headerBand);
            }
        }

        #endregion Private Methods
    }
}
