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
using System.IO;
using System.Drawing;

using AM.Reporting.Barcode;
using AM.Reporting.Table;
using AM.Reporting.Utils;

using System.Linq;

#endregion

#nullable enable

namespace AM.Reporting.Import.DevExpress
{
    /// <summary>
    /// Represents the DevExpess import plugin.
    /// </summary>
    public partial class DevExpressImport : ImportBase
    {
        #region Constants

        private const string TOP_MARGIN_BAND_MASK = "DevExpress.XtraReports.UI.TopMarginBand";
        private const string BOTTOM_MARGIN_BAND_MASK = "DevExpress.XtraReports.UI.BottomMarginBand";
        private const string DETAIL_REPORT_BAND_MASK = "DevExpress.XtraReports.UI.DetailReportBand";
        private const string REPORT_HEADER_BAND_MASK = "DevExpress.XtraReports.UI.ReportHeaderBand";
        private const string REPORT_FOOTER_BAND_MASK = "DevExpress.XtraReports.UI.ReportFooterBand";
        private const string DETAIL_BAND_MASK = "DevExpress.XtraReports.UI.DetailBand";
        private const string GROUP_HEADER_BAND_MASK = "DevExpress.XtraReports.UI.GroupHeaderBand";
        private const string GROUP_FOOTER_BAND_MASK = "DevExpress.XtraReports.UI.GroupFooterBand";


        private const string BAND_CHILD_DEFINITION = "new DevExpress.XtraReports.UI.XRControl[]";
        private const string BAND_CHILDBAND_DEFINITION = "new DevExpress.XtraReports.UI.Band[]";

        private const string DEV_EXPRESS_LABEL = "DevExpress.XtraReports.UI.XRLabel";
        private const string DEV_EXPRESS_LINE = "DevExpress.XtraReports.UI.XRLine";
        private const string DEV_EXPRESS_TABLE = "DevExpress.XtraReports.UI.XRTable";
        private const string DEV_EXPRESS_TABLE_ROW = "DevExpress.XtraReports.UI.XRTableRow";
        private const string DEV_EXPRESS_TABLE_CELL = "DevExpress.XtraReports.UI.XRTableCell";
        private const string DEV_EXPRESS_PICTURE = "DevExpress.XtraReports.UI.XRPictureBox";
        private const string DEV_EXPRESS_PAGE_INFO = "DevExpress.XtraReports.UI.XRPageInfo";
        private const string DEV_EXPRESS_SHAPE = "DevExpress.XtraReports.UI.XRShape";
        private const string DEV_EXPRESS_ZIP_CODE = "DevExpress.XtraReports.UI.XRZipCode";
        private const string DEV_EXPRESS_BAR_CODE = "DevExpress.XtraReports.UI.XRBarCode";
        private const string DEV_EXPRESS_RICH_TEXT = "DevExpress.XtraReports.UI.XRRichText";
        private const string DEV_EXPRESS_STYLE = "DevExpress.XtraReports.UI.XRControlStyle ";

        #endregion // Constants

        #region Fields

        private ReportPage page;
        private string devText;
        private List<string> outsideBands = new List<string>();

        #endregion // Fields

        private class TableInfo
        {
            public List<float> Column;
            public List<float> Row;

            public TableInfo()
            {
                Column = new List<float>();
                Row = new List<float>();
            }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DevExpressImport"/> class.
        /// </summary>
        public DevExpressImport() : base()
        {
            devText = "";
        }

        #endregion // Constructors

        #region Private Methods

        private void LoadReportCode()
        {
            if (devText.IndexOf ("namespace") >= 0)
            {
                devText = devText.Remove (0, devText.IndexOf ("namespace")).Replace (@"http://", "       ");
            }

            LoadReport();
            page = null;
        }

        private string FindObjectName (string mask)
        {
            var name = "";
            var start = devText.IndexOf (mask);
            if (start > -1)
            {
                start += mask.Length;
                var length = devText.IndexOf (";", start) - start;
                name = devText.Substring (start, length).Trim();
            }

            return name;
        }

        private void LoadOutsideBandsNames()
        {
            var bandsRagneStart = "this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[]";
            var start = devText.IndexOf (bandsRagneStart);
            if (start > -1)
            {
                start += bandsRagneStart.Length;
                var lenght = devText.IndexOf (";", start) - start;
                var bandsEnumeration = devText.Substring (start, lenght).Replace ("{", "").Replace ("}", "")
                    .Replace (")", "").Replace ("\n", "").Replace ("\r", "").Trim();
                string[] outBands = bandsEnumeration.Split (',');
                outsideBands.Clear();
                foreach (var bandString in outBands)
                {
                    // Trim() is not working :(
                    var band = string.Concat (bandString.Where (c => !char.IsWhiteSpace (c)));
                    if (band.StartsWith ("this."))
                    {
                        outsideBands.Add (band.Substring (5));
                    }
                    else
                    {
                        outsideBands.Add (band);
                    }
                }
            }
        }

        private string FindReportOutsideBandName (string mask)
        {
            var name = "";
            var start = devText.IndexOf (mask);
            while (start != -1)
            {
                if (start == -1)
                {
                    return string.Empty;
                }

                start += mask.Length;
                var length = devText.IndexOf (";", start) - start;
                name = devText.Substring (start, length).Trim();
                if (outsideBands.Contains (name))
                {
                    return name;
                }

                start = devText.IndexOf (mask, start + mask.Length);
            }

            return string.Empty;
        }

        private string GetObjectDescription (string name)
        {
            var description = "";
            var start = 0;
            while (start > -1)
            {
                start = devText.IndexOf (@"// " + name, start);
                if (devText.Substring (start, name.Length + 2 + 3).EndsWith ("\r\n"))
                {
                    break;
                }

                start += 1;
            }

            if (start > -1)
            {
                start = devText.IndexOf (@"//", start + 2);
                var length = devText.IndexOf (@"//", start + 2) - start + 2;
                description = devText.Substring (start, length);
            }

            return description;
        }

        private string GetPropertyValue (string name, string description)
        {
            var value = "";
            var start = description.IndexOf ("." + name + " ");
            if (start > -1)
            {
                start += name.Length + 3;
                var length = description.IndexOf (";", start) - start;
                value = description.Substring (start, length).Trim();
            }

            return value;
        }

        private bool ExistValue (string name, string description)
        {
            var start = description.IndexOf ("." + name + " ");
            return start > 0;
        }

        private int GetLevelPropValue (string description)
        {
            var level = "level = ";
            if (description == null)
            {
                return -1;
            }

            var start = description.ToLower().IndexOf (level);
            if (start > -1)
            {
                var value = description.Substring (start + level.Length, 1);
                try
                {
                    return int.Parse (value);
                }
                catch
                {
                    return -1;
                }

                //bool parsed = int.TryParse(value, out int resultValue);
                //if (parsed)
                //    return resultValue;
            }

            return -1;
        }

        private void LoadBand (BandBase band, string description)
        {
            if (ExistValue ("HeightF", description))
            {
                band.Height = UnitsConverter.SizeFToPixels (GetPropertyValue ("HeightF", description));
            }
            else
            {
                band.Height = UnitsConverter.SizeFToPixels ("100F");
            }

            band.FillColor = UnitsConverter.ConvertBackColor (GetPropertyValue ("BackColor", description));
        }

        private List<string> GetObjectNames (string description)
        {
            List<string> names = new List<string>();
            var start = description.IndexOf (BAND_CHILD_DEFINITION);
            if (start > -1)
            {
                start += BAND_CHILD_DEFINITION.Length;
                var end = description.IndexOf ("});", start);
                var namesStr = description.Substring (start, end - start + 1).Replace ("}", ",");
                var pos = 0;
                while (pos < end)
                {
                    pos = namesStr.IndexOf ("this.", pos);
                    if (pos < 0)
                    {
                        break;
                    }

                    names.Add (namesStr.Substring (pos + 5, namesStr.IndexOf (",", pos) - pos - 5));
                    pos += 5;
                }
            }

            return names;
        }

        private List<string> GetBandsNames (string description)
        {
            List<string> names = new List<string>();
            var start = description.IndexOf (BAND_CHILDBAND_DEFINITION);
            if (start > -1)
            {
                start += BAND_CHILDBAND_DEFINITION.Length;
                var end = description.IndexOf ("});", start);
                var namesStr = description.Substring (start, end - start + 1).Replace ("}", ",");
                var pos = 0;
                while (pos < end)
                {
                    pos = namesStr.IndexOf ("this.", pos);
                    if (pos < 0)
                    {
                        break;
                    }

                    names.Add (namesStr.Substring (pos + 5, namesStr.IndexOf (",", pos) - pos - 5));
                    pos += 5;
                }
            }

            return names;
        }

        private bool IsTypeOfBand (string name, string bandType)
        {
            var index = devText.IndexOf (name + " = new " + bandType);
            if (index > -1)
            {
                return true;
            }

            return false;
        }

        private string GetBandType (string name)
        {
            var type = string.Empty;
            var start = devText.IndexOf (name + " = new ");
            if (start > -1)
            {
                var end = devText.IndexOf (";", start);
                start += name.Length + 7;
                type = devText.Substring (start, end - start - 2);
            }

            return type;
        }

        private string GetObjectType (string name)
        {
            var str = "this." + name + " = new ";
            var start = devText.IndexOf (str) + str.Length;
            var end = devText.IndexOf ("();", start);
            return devText.Substring (start, end - start);
        }

        private void LoadComponent (string description, ComponentBase comp)
        {
            comp.Name = GetPropertyValue ("Name", description).Replace ("\"", "");
            var location = GetPropertyValue ("LocationFloat", description);
            if (!string.IsNullOrEmpty (location))
            {
                var start = location.IndexOf ("(");
                var comma = location.IndexOf (",", start);
                var end = location.IndexOf (")");
                comp.Left = UnitsConverter.SizeFToPixels (location.Substring (start + 1, comma - start));
                comp.Top = UnitsConverter.SizeFToPixels (location.Substring (comma + 2, end - comma - 1));
            }
        }

        private void LoadBorder (string description, Border border)
        {
            var borders = GetPropertyValue ("Borders", description);
            if (!string.IsNullOrEmpty (borders))
            {
                if (borders.IndexOf ("Left") > -1)
                {
                    border.Lines |= BorderLines.Left;
                }

                if (borders.IndexOf ("Top") > -1)
                {
                    border.Lines |= BorderLines.Top;
                }

                if (borders.IndexOf ("Right") > -1)
                {
                    border.Lines |= BorderLines.Right;
                }

                if (borders.IndexOf ("Bottom") > -1)
                {
                    border.Lines |= BorderLines.Bottom;
                }
            }

            var color = GetPropertyValue ("BorderColor", description);
            if (!string.IsNullOrEmpty (color))
            {
                border.Color = UnitsConverter.ConvertColor (color);
            }

            var style = GetPropertyValue ("BorderDashStyle", description);
            if (!string.IsNullOrEmpty (style))
            {
                border.Style = UnitsConverter.ConvertBorderDashStyle (style);
            }

            var width = GetPropertyValue ("BorderWidth", description);
            if (!string.IsNullOrEmpty (width))
            {
                border.Width = UnitsConverter.SizeFToPixels (width);
            }
        }

        private void LoadSize (string description, ComponentBase comp)
        {
            var size = GetPropertyValue ("SizeF", description);
            if (!string.IsNullOrEmpty (size))
            {
                var start = size.IndexOf ("(");
                var comma = size.IndexOf (",", start);
                var end = size.IndexOf (")");
                comp.Width = UnitsConverter.SizeFToPixels (size.Substring (start + 1, comma - start));
                comp.Height = UnitsConverter.SizeFToPixels (size.Substring (comma + 2, end - comma - 1));
            }
        }

        private Font LoadFont (string description)
        {
            var font = GetPropertyValue ("Font", description);
            if (!string.IsNullOrEmpty (font))
            {
                var start = font.IndexOf ("(");
                var comma = font.IndexOf (",", start);
                var secondComma = font.IndexOf (",", comma + 1);
                var fontFamily = font.Substring (start + 2, comma - start - 3);
                var fontSize = 10.0f;
                if (secondComma > -1)
                {
                    var str = font.Substring (comma + 2, secondComma - comma - 2);
                    fontSize = UnitsConverter.SizeFToPixelsFont (font.Substring (comma + 2, secondComma - comma - 2));
                    var fontStyle = FontStyle.Regular;
                    if (font.Contains ("Bold"))
                    {
                        fontStyle |= FontStyle.Bold;
                    }

                    if (font.Contains ("Italic"))
                    {
                        fontStyle |= FontStyle.Italic;
                    }

                    if (font.Contains ("Underline"))
                    {
                        fontStyle |= FontStyle.Underline;
                    }

                    if (font.Contains ("Strikeout"))
                    {
                        fontStyle |= FontStyle.Strikeout;
                    }

                    return new Font (fontFamily, fontSize, fontStyle);
                }
                else
                {
                    var str = font.Substring (comma + 2, font.IndexOf (")") - comma - 2);
                    fontSize = UnitsConverter.SizeFToPixelsFont (font.Substring (comma + 2,
                        font.IndexOf (")") - comma - 2));
                }
            }

            return new Font ("Arial", 10.0f, FontStyle.Regular);
        }

        private void LoadLabel (string name, Base parent)
        {
            var description = GetObjectDescription (name);
            var text = ComponentsFactory.CreateTextObject (name, parent);
            LoadComponent (description, text);
            LoadSize (description, text);
            LoadBorder (description, text.Border);
            text.Name = name;
            text.FillColor = UnitsConverter.ConvertBackColor (GetPropertyValue ("BackColor", description));
            text.TextColor = UnitsConverter.ConvertColor (GetPropertyValue ("ForeColor", description));
            text.Text = GetPropertyValue ("Text", description).Replace ("\"", "");
            text.HorzAlign =
                UnitsConverter.ConvertTextAlignmentToHorzAlign (GetPropertyValue ("TextAlignment", description));
            text.VertAlign =
                UnitsConverter.ConvertTextAlignmentToVertAlign (GetPropertyValue ("TextAlignment", description));
            text.Font = LoadFont (description);
            ApplyStyleByName (text, GetPropertyValue ("StyleName", description).Replace ("\"", ""));
        }

        private void LoadTableCell (string name, TableCell cell)
        {
            var description = GetObjectDescription (name);
            cell.Name = name;
            cell.FillColor = UnitsConverter.ConvertBackColor (GetPropertyValue ("BackColor", description));
            cell.TextColor = UnitsConverter.ConvertColor (GetPropertyValue ("ForeColor", description));
            cell.Text = GetPropertyValue ("Text", description).Replace ("\"", "");
            cell.HorzAlign =
                UnitsConverter.ConvertTextAlignmentToHorzAlign (GetPropertyValue ("TextAlignment", description));
            cell.VertAlign =
                UnitsConverter.ConvertTextAlignmentToVertAlign (GetPropertyValue ("TextAlignment", description));
            cell.Font = LoadFont (description);
            ApplyStyleByName (cell, GetPropertyValue ("StyleName", description).Replace ("\"", ""));
        }

        private void LoadLine (string name, Base parent)
        {
            var description = GetObjectDescription (name);
            var line = ComponentsFactory.CreateLineObject (name, parent);
            LoadComponent (description, line);
            LoadSize (description, line);
            line.Border.Color = UnitsConverter.ConvertColor (GetPropertyValue ("ForeColor", description));
            line.Border.Style = UnitsConverter.ConvertLineStyle (GetPropertyValue ("LineStyle", description));
            var width = GetPropertyValue ("LineWidth", description);
            if (!string.IsNullOrEmpty (width))
            {
                line.Border.Width = Convert.ToSingle (width);
            }

            line.Style = GetPropertyValue ("StyleName", description).Replace ("\"", "");
        }

        private void LoadPicture (string name, Base parent)
        {
            var description = GetObjectDescription (name);
            var picture = ComponentsFactory.CreatePictureObject (name, parent);
            LoadComponent (description, picture);
            LoadSize (description, picture);
            picture.SizeMode = UnitsConverter.ConvertImageSizeMode (GetPropertyValue ("Sizing", description));
            picture.Style = GetPropertyValue ("StyleName", description).Replace ("\"", "");
        }

        private void LoadShape (string name, Base parent)
        {
            var description = GetObjectDescription (name);
            var shape = ComponentsFactory.CreateShapeObject (name, parent);
            LoadComponent (description, shape);
            LoadSize (description, shape);
            shape.Border.Color = UnitsConverter.ConvertColor (GetPropertyValue ("ForeColor", description));
            shape.FillColor = UnitsConverter.ConvertBackColor (GetPropertyValue ("FillColor", description));
            shape.Shape = UnitsConverter.ConvertShape (GetPropertyValue ("Shape", description));
            var width = GetPropertyValue ("LineWidth", description);
            if (!string.IsNullOrEmpty (width))
            {
                shape.Border.Width = Convert.ToSingle (width);
            }

            shape.Style = GetPropertyValue ("StyleName", description).Replace ("\"", "");
        }

        private void LoadZipCode (string name, Base parent)
        {
            var description = GetObjectDescription (name);
            var zipCode = ComponentsFactory.CreateZipCodeObject (name, parent);
            LoadComponent (description, zipCode);
            LoadSize (description, zipCode);
            zipCode.FillColor = UnitsConverter.ConvertBackColor (GetPropertyValue ("BackColor", description));
            zipCode.Border.Color = UnitsConverter.ConvertColor (GetPropertyValue ("ForeColor", description));
            zipCode.Text = GetPropertyValue ("Text", description).Replace ("\"", "");
            zipCode.Style = GetPropertyValue ("StyleName", description).Replace ("\"", "");
        }

        private void LoadBarCode (string name, Base parent)
        {
            var description = GetObjectDescription (name);
            var barcode = ComponentsFactory.CreateBarcodeObject (name, parent);
            LoadComponent (description, barcode);
            LoadSize (description, barcode);
            LoadBorder (description, barcode.Border);
            barcode.FillColor = UnitsConverter.ConvertBackColor (GetPropertyValue ("BackColor", description));
            UnitsConverter.ConvertBarcodeSymbology (GetPropertyValue ("Symbology", description), barcode);
            barcode.Style = GetPropertyValue ("StyleName", description).Replace ("\"", "");
        }

        partial void LoadRichText (string name, Base parent);

        private List<string> GetChildNames (string childType, string description)
        {
            List<string> names = new List<string>();
            var start = description.IndexOf (childType + "[]");
            if (start > -1)
            {
                start += childType.Length + 2;
                var end = description.IndexOf ("});", start);
                var namesStr = description.Substring (start, end - start + 1).Replace ("}", ",");
                var pos = 0;
                while (pos < end)
                {
                    pos = namesStr.IndexOf ("this.", pos);
                    if (pos < 0)
                    {
                        break;
                    }

                    names.Add (namesStr.Substring (pos + 5, namesStr.IndexOf (",", pos) - pos - 5));
                    pos += 5;
                }
            }

            return names;
        }

        private void LoadTable (string name, Base parent)
        {
            var description = GetObjectDescription (name);
            var table = ComponentsFactory.CreateTableObject (name, parent);
            LoadComponent (description, table);
            LoadSize (description, table);
            LoadBorder (description, table.Border);
            table.Style = GetPropertyValue ("StyleName", description).Replace ("\"", "");
            List<string> rowNames = GetChildNames (DEV_EXPRESS_TABLE_ROW, description);

            var tableInfo = new TableInfo();

            // Create table columns.
            var columnsCount = 0;
            foreach (var rowName in rowNames)
            {
                var rowDescription = GetObjectDescription (rowName);
                List<string> cellNames = GetChildNames (DEV_EXPRESS_TABLE_CELL, rowDescription);

                if (columnsCount < cellNames.Count)
                {
                    columnsCount = cellNames.Count;
                    tableInfo.Column.Clear();
                    foreach (var cellName in cellNames)
                    {
                        tableInfo.Column.Add (GetWeight (cellName));
                    }
                }
            }

            for (var i = 0; i < rowNames.Count; i++)
            {
                tableInfo.Row.Add (GetWeight (rowNames[i]));
            }

            for (var i = 0; i < columnsCount; i++)
            {
                var column = new TableColumn();

                column.Width = GetRowColumnSize (tableInfo.Column, i, table.Width);
                table.Columns.Add (column);
                column.CreateUniqueName();
            }

            // Create table rows.
            for (var i = 0; i < rowNames.Count; i++)
            {
                var row = new TableRow();
                row.Name = rowNames[i];
                var rowDescription = GetObjectDescription (row.Name);

                row.Height = GetRowColumnSize (tableInfo.Row, i, table.Height);
                List<string> cellNames = GetChildNames (DEV_EXPRESS_TABLE_CELL, rowDescription);

                // Create table cell.
                foreach (var cellName in cellNames)
                {
                    var cell = new TableCell();
                    cell.Name = cellName;
                    LoadTableCell (cellName, cell);
                    row.AddChild (cell);
                }

                table.Rows.Add (row);
            }
        }

        private float GetWeight (string componentName)
        {
            var weight = GetPropertyValue ("Weight", GetObjectDescription (componentName));
            weight = weight.Substring (0, weight.Length - 1);
            return float.Parse (weight.Replace (".", ","));
        }

        private float GetRowColumnSize (List<float> allSizes, int index, float totalSize)
        {
            var size = totalSize / allSizes.Sum() * allSizes[index];
            if (allSizes.Count == 1)
            {
                return totalSize;
            }

            return size;
        }

        private void LoadObjects (string description, Base parent)
        {
            List<string> names = GetObjectNames (description);
            foreach (var name in names)
            {
                var type = GetObjectType (name);
                if (!string.IsNullOrEmpty (type))
                {
                    switch (type)
                    {
                        case DEV_EXPRESS_LABEL:
                            LoadLabel (name, parent);
                            break;
                        case DEV_EXPRESS_PAGE_INFO:
                            LoadLabel (name, parent);
                            break;
                        case DEV_EXPRESS_LINE:
                            LoadLine (name, parent);
                            break;
                        case DEV_EXPRESS_PICTURE:
                            LoadPicture (name, parent);
                            break;
                        case DEV_EXPRESS_SHAPE:
                            LoadShape (name, parent);
                            break;
                        case DEV_EXPRESS_ZIP_CODE:
                            LoadZipCode (name, parent);
                            break;
                        case DEV_EXPRESS_BAR_CODE:
                            LoadBarCode (name, parent);
                            break;
                        case DEV_EXPRESS_RICH_TEXT:
                            LoadRichText (name, parent);
                            break;
                        case DEV_EXPRESS_TABLE:
                            LoadTable (name, parent);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void LoadPageSize()
        {
            var height = GetPropertyValue ("PageHeight", devText);
            var width = GetPropertyValue ("PageWidth", devText);
            if (!string.IsNullOrEmpty (height))
            {
                page.PaperHeight = UnitsConverter.SizeFToPixels (height) / Units.Millimeters;
            }

            if (!string.IsNullOrEmpty (width))
            {
                page.PaperWidth = UnitsConverter.SizeFToPixels (width) / Units.Millimeters;
            }
        }

        private void CheckDpiZoom()
        {
            var dpi = GetPropertyValue ("Dpi", devText);
            if (!string.IsNullOrEmpty (dpi))
            {
                UnitsConverter.Ratio = float.Parse (dpi.Substring (0, dpi.Length - 1)) / 96;
            }
        }

        private List<string> GetStyleNames()
        {
            List<string> names = new List<string>();
            var start = devText.IndexOf (DEV_EXPRESS_STYLE);
            while (start > -1)
            {
                start += DEV_EXPRESS_STYLE.Length;
                var end = devText.IndexOf (";", start);
                names.Add (devText.Substring (start, end - start));
                start = end;
                start = devText.IndexOf (DEV_EXPRESS_STYLE, start);
            }

            return names;
        }

        private void LoadStyles()
        {
            List<string> names = GetStyleNames();
            foreach (var name in names)
            {
                var style = ComponentsFactory.CreateStyle (name, Report);
                var description = GetObjectDescription (name);
                LoadBorder (description, style.Border);
                style.TextFill =
                    new SolidFill (UnitsConverter.ConvertColor (GetPropertyValue ("ForeColor", description)));
                style.Fill =
                    new SolidFill (UnitsConverter.ConvertBackColor (GetPropertyValue ("BackColor", description)));
                style.Font = LoadFont (description);
            }
        }

        private void ApplyStyleByName (ReportComponentBase component, string styleName)
        {
            foreach (Style subStyle in Report.Styles)
            {
                if (subStyle.Name == styleName)
                {
                    var f = new Font ("Arial", 10, FontStyle.Regular);
                    if (component is TextObject textObject)
                    {
                        f = textObject.Font;
                    }

                    component.Style = styleName;
                    component.ApplyStyle (subStyle);

                    var descr = GetObjectDescription (component.Name);

                    var useFont = GetPropertyValue ("UseFont", descr);

                    if (useFont != string.Empty)
                    {
                        if (!UnitsConverter.ConvertBool (useFont))
                        {
                            (component as TextObject).Font = f;
                        }
                    }

                    return;
                }
            }
        }

        private void LoadTopMarginBand() // Page Header
        {
            var name = FindObjectName (TOP_MARGIN_BAND_MASK);
            if (!string.IsNullOrEmpty (name))
            {
                var header = ComponentsFactory.CreatePageHeaderBand (page);
                var description = GetObjectDescription (name);
                LoadBand (header, description);
                LoadObjects (description, header);
            }
        }

        private void LoadBottomMarginBand() // Page Footer
        {
            var name = FindObjectName (BOTTOM_MARGIN_BAND_MASK);
            if (!string.IsNullOrEmpty (name))
            {
                var footer = ComponentsFactory.CreatePageFooterBand (page);
                var description = GetObjectDescription (name);
                LoadBand (footer, description);
                LoadObjects (description, footer);
            }
        }

        private void LoadReportHeaderBand() // Report Title
        {
            var name = FindReportOutsideBandName (REPORT_HEADER_BAND_MASK);
            if (!string.IsNullOrEmpty (name))
            {
                var title = ComponentsFactory.CreateReportTitleBand (page);
                var description = GetObjectDescription (name);
                LoadBand (title, description);
                LoadObjects (description, title);
            }
        }

        private void LoadReportFooterBand() // Report Summary
        {
            var name = FindObjectName (REPORT_FOOTER_BAND_MASK);
            if (!string.IsNullOrEmpty (name))
            {
                var summary = ComponentsFactory.CreateReportSummaryBand (page);
                var description = GetObjectDescription (name);
                LoadBand (summary, description);
                LoadObjects (description, summary);
            }
        }

        #region DetailReportBand

        private void LoadDetailReportBand() // One More Data Band
        {
            //string name = FindObjectName(DETAIL_REPORT_BAND_MASK);

            List<string> detailReports = NamesOfDetailReportBand();

            for (var i = 0; i < detailReports.Count; i++)
            {
                var name = detailReports.Where (x => GetLevelPropValue (GetObjectDescription (x)) == i)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty (name))
                {
                    var data = ComponentsFactory.CreateDataBand (page);
                    var description = GetObjectDescription (name);
                    List<string> bandsInDetailReport = GetBandsNames (description);

                    LoadDetailBandInDetailReport (bandsInDetailReport, data);
                    LoadReportHeaderFooter (bandsInDetailReport, data);

                    LoadDetailReportGroupBands (bandsInDetailReport, data);
                }
            }
        }

        private List<string> NamesOfDetailReportBand()
        {
            List<string> names = new List<string>();
            var start = 0;
            while (start != -1)
            {
                start = devText.IndexOf (DETAIL_REPORT_BAND_MASK, start);
                if (start == -1)
                {
                    return names;
                }

                start += DETAIL_REPORT_BAND_MASK.Length + 1;
                var end = devText.IndexOf (";", start);
                var stringName = devText.Substring (start, end - start).Replace ("}", ",");
                if (!stringName.EndsWith (")"))
                {
                    names.Add (devText.Substring (start, end - start).Replace ("}", ","));
                }
            }

            return names;
        }

        private void LoadDetailBandInDetailReport (List<string> bands, BandBase data)
        {
            var detailName = bands.Where (x => IsTypeOfBand (x, DETAIL_BAND_MASK)).FirstOrDefault();
            if (string.IsNullOrEmpty (detailName))
            {
                return;
            }

            var detailDescription = GetObjectDescription (detailName);
            LoadBand (data, detailDescription);
            LoadObjects (detailDescription, data);
            bands.Remove (detailName);
        }

        private void LoadReportHeaderFooter (List<string> bands, DataBand data)
        {
            var header = LoadReportHeaderInDetailReport (bands, data);
            LoadReportFooterInDetailReport (bands, header);
        }

        private GroupHeaderBand LoadReportHeaderInDetailReport (List<string> bands, DataBand data)
        {
            var detailName = bands.Where (x => IsTypeOfBand (x, REPORT_HEADER_BAND_MASK)).FirstOrDefault();
            if (detailName == null)
            {
                return null;
            }

            // Load one more report header as GroupHeader, 'cause FR don't support more than one ReportTitle band

            var groupHeader = new GroupHeaderBand();
            groupHeader.Data = data;
            page.Bands.Add (groupHeader);

            var detailDescription = GetObjectDescription (detailName);
            LoadBand (groupHeader, detailDescription);
            LoadObjects (detailDescription, groupHeader);
            bands.Remove (detailName);
            return groupHeader;
        }

        private void LoadReportFooterInDetailReport (List<string> bands, GroupHeaderBand header)
        {
            if (header == null)
            {
                return;
            }

            var detailName = bands.Where (x => IsTypeOfBand (x, REPORT_FOOTER_BAND_MASK)).FirstOrDefault();
            if (string.IsNullOrEmpty (detailName))
            {
                return;
            }

            // Load one more report header as GroupHeader, 'cause FR don't support more than one ReportTitle band

            var groupFooter = new GroupFooterBand();
            header.GroupFooter = groupFooter;


            var detailDescription = GetObjectDescription (detailName);
            LoadBand (groupFooter, detailDescription);
            LoadObjects (detailDescription, groupFooter);
            bands.Remove (detailName);
        }

        private void LoadDetailReportGroupBands (List<string> subBands, DataBand data)
        {
            var curLevel = 0;

            for (var i = 0; i < subBands.Count; i++)
            {
                // Search header
                var headerName = "";
                if (i == 0)

                    // First header level may or may not contain a property "level", but the second level must contain a property with a value of 1
                {
                    headerName = subBands.Where (x =>
                        (GetLevelPropValue (GetObjectDescription (x)) == curLevel ||
                         GetLevelPropValue (GetObjectDescription (x)) == -1) &&
                        GetBandType (x).Equals (GROUP_HEADER_BAND_MASK)).FirstOrDefault();
                }
                else
                {
                    headerName = subBands.Where (x =>
                        GetLevelPropValue (GetObjectDescription (x)) == curLevel &&
                        GetBandType (x).Equals (GROUP_HEADER_BAND_MASK)).FirstOrDefault();
                }

                // Try to create header
                var groupHeader = LoadDetailReportGroupHeaderBand (data, headerName);
                if (groupHeader == null)
                {
                    curLevel++;
                    continue;
                }

                var headerLevel = GetLevelPropValue (GetObjectDescription (headerName));

                // The same situation with levels
                if (headerLevel == -1 && i != 0)
                {
                    headerLevel = -2;
                }

                var footerName = subBands.Where (x =>
                    GetLevelPropValue (GetObjectDescription (x)) == headerLevel &&
                    GetBandType (x).Equals (GROUP_FOOTER_BAND_MASK)).FirstOrDefault();
                LoadDetailReportGroupFooterBand (groupHeader, footerName);
                curLevel++;
            }
        }

        private GroupHeaderBand LoadDetailReportGroupHeaderBand (DataBand data, string bandName)
        {
            if (bandName == null)
            {
                return null;
            }

            var groupHeader = new GroupHeaderBand();
            if (data.Parent is GroupHeaderBand parent)
            {
                groupHeader.Data = data;
                parent.NestedGroup = groupHeader;
            }
            else
            {
                groupHeader.Data = data;
                page.Bands.Add (groupHeader);
            }


            var detailDescription = GetObjectDescription (bandName);
            LoadBand (groupHeader, detailDescription);
            LoadObjects (detailDescription, groupHeader);
            return groupHeader;
        }

        private void LoadDetailReportGroupFooterBand (GroupHeaderBand groupHeader, string bandName)
        {
            if (groupHeader == null || string.IsNullOrEmpty (bandName))
            {
                return;
            }

            var groupFooter = new GroupFooterBand();
            groupHeader.GroupFooter = groupFooter;

            var detailDescription = GetObjectDescription (bandName);
            LoadBand (groupFooter, detailDescription);
            LoadObjects (detailDescription, groupFooter);
        }

        #endregion

        private void LoadDetailBand() // Data
        {
            var name = FindObjectName (DETAIL_BAND_MASK);
            if (!string.IsNullOrEmpty (name))
            {
                var data = ComponentsFactory.CreateDataBand (page);
                var description = GetObjectDescription (name);
                LoadBand (data, description);
                LoadObjects (description, data);
                LoadGroupHeaderFooterBand (data);
            }
        }

        private void LoadGroupHeaderFooterBand (DataBand data)
        {
            var groupHeader = LoadGroupHeaderBand (data);
            LoadGroupFooterBand (groupHeader);
        }

        private GroupHeaderBand LoadGroupHeaderBand (DataBand data)
        {
            var name = FindReportOutsideBandName (GROUP_HEADER_BAND_MASK);
            if (!string.IsNullOrEmpty (name))
            {
                var groupHeader = new GroupHeaderBand();
                groupHeader.Data = data;
                page.Bands.Add (groupHeader);
                var description = GetObjectDescription (name);
                LoadBand (groupHeader, description);
                LoadObjects (description, groupHeader);
                return groupHeader;
            }

            return null;
        }

        private void LoadGroupFooterBand (GroupHeaderBand header)
        {
            if (header == null)
            {
                return;
            }

            var name = FindReportOutsideBandName (GROUP_FOOTER_BAND_MASK);
            if (!string.IsNullOrEmpty (name))
            {
                var groupFooter = new GroupFooterBand();
                header.GroupFooter = groupFooter;
                var Description = GetObjectDescription (name);
                LoadBand (groupFooter, Description);
                LoadObjects (Description, groupFooter);
            }
        }

        private void LoadReport()
        {
            CheckDpiZoom();
            LoadPageSize();
            LoadStyles();
            LoadOutsideBandsNames();
            LoadTopMarginBand();
            LoadBottomMarginBand();
            LoadReportHeaderBand();
            LoadReportFooterBand();
            LoadDetailBand();
            LoadDetailReportBand();
            UnitsConverter.Ratio = 1;
        }

        #endregion // Private Methods

        #region Public Methods

        /// <inheritdoc />
        public override void LoadReport (Report report, string filename)
        {
            using (var fs = new FileStream (filename, FileMode.Open, FileAccess.Read))
            {
                LoadReport (report, fs);
            }
        }

        /// <inheritdoc />
        public override void LoadReport (Report report, Stream content)
        {
            Report = report;
            Report.Clear();
            using (var sr = new StreamReader (content))
            {
                devText = sr.ReadToEnd();
            }

            try
            {
                devDoc = new System.Xml.XmlDocument();
                devDoc.LoadXml (devText);
            }
            catch
            {
            }

            page = ComponentsFactory.CreateReportPage (Report);

            if (devDoc.LastChild != null)
            {
                LoadReportXml();
            }
            else
            {
                LoadReportCode();
            }
        }

        #endregion // Public Methods
    }
}
