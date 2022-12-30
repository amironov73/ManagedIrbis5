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
using AM.Reporting.Matrix;
using AM.Reporting.Barcode;
using AM.Reporting.Gauge.Linear;
using AM.Reporting.Gauge.Simple;
using AM.Reporting.Data;
#if !CROSSPLATFORM
using System.CodeDom.Compiler;
#endif

#endregion

#nullable enable

namespace AM.Reporting.Import
{
    /// <summary>
    /// The components factory.
    /// </summary>
    public static partial class ComponentsFactory
    {
        #region Private Methods

        private static bool IsValidIdentifier (string identifier)
        {
#if !CROSSPLATFORM
            if (!CodeGenerator.IsValidLanguageIndependentIdentifier (identifier))
            {
                return false;
            }
#endif
            return true;
        }

        #endregion // Private Methods

        #region Pages

        /// <summary>
        /// Creates a ReportPage instance in the specified Report.
        /// </summary>
        /// <param name="report">The Report instance.</param>
        /// <returns>The ReportPage instance.</returns>
        public static ReportPage CreateReportPage (Report report)
        {
            var page = new ReportPage();
            report.Pages.Add (page);
            page.CreateUniqueName();
            return page;
        }

        /// <summary>
        /// Creates a ReportPage instance in the specified Report with the cpecified name.
        /// </summary>
        /// <param name="name">The name of page.</param>
        /// <param name="report">The Report instance.</param>
        /// <returns>The ReportPage instance.</returns>
        public static ReportPage CreateReportPage (string name, Report report)
        {
            var page = new ReportPage();
            page.Name = name;
            report.Pages.Add (page);
            if (!IsValidIdentifier (page.Name))
            {
                page.CreateUniqueName();
            }

            return page;
        }

        #endregion // Pages

        #region Bands

        /// <summary>
        /// Creates a ReportTitleBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The ReportTitleBand instance.</returns>
        public static ReportTitleBand CreateReportTitleBand (ReportPage page)
        {
            var reportTitle = new ReportTitleBand();
            page.ReportTitle = reportTitle;
            reportTitle.CreateUniqueName();
            return reportTitle;
        }

        /// <summary>
        /// Creates a ReportSummaryBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The ReportSummaryBand instance.</returns>
        public static ReportSummaryBand CreateReportSummaryBand (ReportPage page)
        {
            var reportSummary = new ReportSummaryBand();
            page.ReportSummary = reportSummary;
            reportSummary.CreateUniqueName();
            return reportSummary;
        }

        /// <summary>
        /// Creates a PageHeaderBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The PageHeaderBand instance.</returns>
        public static PageHeaderBand CreatePageHeaderBand (ReportPage page)
        {
            var pageHeader = new PageHeaderBand();
            page.PageHeader = pageHeader;
            pageHeader.CreateUniqueName();
            return pageHeader;
        }

        /// <summary>
        /// Creates a PageFooterBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The PageFooterBand instance.</returns>
        public static PageFooterBand CreatePageFooterBand (ReportPage page)
        {
            var pageFooter = new PageFooterBand();
            page.PageFooter = pageFooter;
            pageFooter.CreateUniqueName();
            return pageFooter;
        }

        /// <summary>
        /// Creates a ColumnHeaderBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The ColumnHeaderBand instance.</returns>
        public static ColumnHeaderBand CreateColumnHeaderBand (ReportPage page)
        {
            var columnHeader = new ColumnHeaderBand();
            page.ColumnHeader = columnHeader;
            columnHeader.CreateUniqueName();
            return columnHeader;
        }

        /// <summary>
        /// Creates a ColumnFooterBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The ColumnFooterBand instance.</returns>
        public static ColumnFooterBand CreateColumnFooterBand (ReportPage page)
        {
            var columnFooter = new ColumnFooterBand();
            page.ColumnFooter = columnFooter;
            columnFooter.CreateUniqueName();
            return columnFooter;
        }

        /// <summary>
        /// Creates a DataHeaderBand instance in the specified DataBand.
        /// </summary>
        /// <param name="data">The DataBand instance.</param>
        /// <returns>The DataHeaderBand instance.</returns>
        public static DataHeaderBand CreateDataHeaderBand (DataBand data)
        {
            var dataHeader = new DataHeaderBand();
            data.Header = dataHeader;
            dataHeader.CreateUniqueName();
            return dataHeader;
        }

        /// <summary>
        /// Creates a DataBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The DataBand instance.</returns>
        public static DataBand CreateDataBand (ReportPage page)
        {
            var band = new DataBand();
            page.Bands.Add (band);
            band.CreateUniqueName();
            return band;
        }

        /// <summary>
        /// Creates a DataFooterBand instance in the specified DataBand.
        /// </summary>
        /// <param name="data">The DataBand instance.</param>
        /// <returns>The DataFooterBand instance.</returns>
        public static DataFooterBand CreateDataFooterBand (DataBand data)
        {
            var dataFooter = new DataFooterBand();
            data.Footer = dataFooter;
            dataFooter.CreateUniqueName();
            return dataFooter;
        }

        /// <summary>
        /// Creates a GroupHeaderBand instance in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The GroupHeaderBand instance.</returns>
        public static GroupHeaderBand CreateGroupHeaderBand (ReportPage page)
        {
            var groupHeader = new GroupHeaderBand();
            page.Bands.Add (groupHeader);
            groupHeader.CreateUniqueName();
            return groupHeader;
        }

        /// <summary>
        /// Creates a GroupFooterBand instance in the cpecified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The GroupFooterBand instance.</returns>
        public static GroupFooterBand CreateGroupFooterBand (ReportPage page)
        {
            var groupFooter = new GroupFooterBand();
            page.Bands.Add (groupFooter);
            groupFooter.CreateUniqueName();
            return groupFooter;
        }

        /// <summary>
        /// Creates a GroupFooterBand instance in the cpecified GroupHeaderBand.
        /// </summary>
        /// <param name="groupHeaderBand">The GroupHeaderBand instance.</param>
        /// <returns>The GroupFooterBand instance.</returns>
        public static GroupFooterBand CreateGroupFooterBand (GroupHeaderBand groupHeaderBand)
        {
            var groupFooter = new GroupFooterBand();
            groupHeaderBand.GroupFooter = groupFooter;
            groupFooter.CreateUniqueName();
            return groupFooter;
        }

        /// <summary>
        /// Creates a ChildBand instance in the specified BandBase.
        /// </summary>
        /// <param name="parent">The BandBase instance.</param>
        /// <returns>The ChildBand instance.</returns>
        public static ChildBand CreateChildBand (BandBase parent)
        {
            var child = new ChildBand();
            parent.AddChild (child);
            child.CreateUniqueName();
            return child;
        }

        /// <summary>
        /// Creates an OverlayBand in the specified ReportPage.
        /// </summary>
        /// <param name="page">The ReportPage instance.</param>
        /// <returns>The OverlayBand instance.</returns>
        public static OverlayBand CreateOverlayBand (ReportPage page)
        {
            var overlay = new OverlayBand();
            page.Overlay = overlay;
            overlay.CreateUniqueName();
            return overlay;
        }

        #endregion // Bands

        #region Objects

        /// <summary>
        /// Creates a Style instance with specified name.
        /// </summary>
        /// <param name="name">The name of the Style instance.</param>
        /// <param name="report">The report to add style to.</param>
        /// <returns>The Style instance.</returns>
        public static Style CreateStyle (string name, Report report)
        {
            var style = new Style
            {
                Name = name
            };
            report.Styles.Add (style);
            return style;
        }

        /// <summary>
        /// Creates a TextObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the TextObject instance.</param>
        /// <param name="parent">The parent of the TextObject instance.</param>
        /// <returns>The TextObject instance.</returns>
        public static TextObject CreateTextObject (string name, Base parent)
        {
            var text = new TextObject();
            text.Name = name;
            if ((parent as IParent).CanContain (text))
            {
                text.Parent = parent;
            }

            if (!IsValidIdentifier (text.Name))
            {
                text.CreateUniqueName();
            }

            return text;
        }

        /// <summary>
        /// Creates a PictureObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the PictureObject instance.</param>
        /// <param name="parent">The parent of the PictureObject instance.</param>
        /// <returns>The PictureObject instance.</returns>
        public static PictureObject CreatePictureObject (string name, Base parent)
        {
            var picture = new PictureObject();
            picture.Name = name;
            if ((parent as IParent).CanContain (picture))
            {
                picture.Parent = parent;
            }

            if (!IsValidIdentifier (picture.Name))
            {
                picture.CreateUniqueName();
            }

            return picture;
        }

        /// <summary>
        /// Creates a LineObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the LineObject instance.</param>
        /// <param name="parent">The parent of the LineObject instance.</param>
        /// <returns>The LineObject instance.</returns>
        public static LineObject CreateLineObject (string name, Base parent)
        {
            var line = new LineObject();
            line.Name = name;
            if ((parent as IParent).CanContain (line))
            {
                line.Parent = parent;
            }

            if (!IsValidIdentifier (line.Name))
            {
                line.CreateUniqueName();
            }

            return line;
        }

        /// <summary>
        /// Creates a ShapeObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the ShapeObject instance.</param>
        /// <param name="parent">The parent of the ShapeObject instance.</param>
        /// <returns>The ShapeObject instance.</returns>
        public static ShapeObject CreateShapeObject (string name, Base parent)
        {
            var shape = new ShapeObject();
            shape.Name = name;
            if ((parent as IParent).CanContain (shape))
            {
                shape.Parent = parent;
            }

            if (!IsValidIdentifier (shape.Name))
            {
                shape.CreateUniqueName();
            }

            return shape;
        }

        /// <summary>
        /// Creates a PolyLineObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the PolyLineObject instance.</param>
        /// <param name="parent">The parent of the PolyLineObject instance.</param>
        /// <returns>The PolyLineObject instance.</returns>
        public static PolyLineObject CreatePolyLineObject (string name, Base parent)
        {
            var polyLine = new PolyLineObject();
            polyLine.Name = name;
            if ((parent as IParent).CanContain (polyLine))
            {
                polyLine.Parent = parent;
            }

            if (!IsValidIdentifier (polyLine.Name))
            {
                polyLine.CreateUniqueName();
            }

            return polyLine;
        }

        /// <summary>
        /// Creates a PolygonObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the PolygonObject instance.</param>
        /// <param name="parent">The parent of the PolygonObject instance.</param>
        /// <returns>The PolygonObject instance.</returns>
        public static PolygonObject CreatePolygonObject (string name, Base parent)
        {
            var polygon = new PolygonObject();
            polygon.Name = name;
            if ((parent as IParent).CanContain (polygon))
            {
                polygon.Parent = parent;
            }

            if (!IsValidIdentifier (polygon.Name))
            {
                polygon.CreateUniqueName();
            }

            return polygon;
        }

        /// <summary>
        /// Creates a SubreportObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the SubreportObject instance.</param>
        /// <param name="parent">The parent of the SubreportObject instance.</param>
        /// <returns>The SubreportObject instance.</returns>
        public static SubreportObject CreateSubreportObject (string name, Base parent)
        {
            var subreport = new SubreportObject();
            subreport.Name = name;
            if ((parent as IParent).CanContain (subreport))
            {
                subreport.Parent = parent;
            }

            if (!IsValidIdentifier (subreport.Name))
            {
                subreport.CreateUniqueName();
            }

            return subreport;
        }

        /// <summary>
        /// Creates a ContainerObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the ContainerObject instance.</param>
        /// <param name="parent">The parent of the ContainerObject instance.</param>
        /// <returns>The ContainerObject instance.</returns>
        public static ContainerObject CreateContainerObject (string name, Base parent)
        {
            var container = new ContainerObject();
            container.Name = name;
            if ((parent as IParent).CanContain (container))
            {
                container.Parent = parent;
            }

            if (!IsValidIdentifier (container.Name))
            {
                container.CreateUniqueName();
            }

            return container;
        }

        /// <summary>
        /// Creates a CheckBoxObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the CheckBoxObject instance.</param>
        /// <param name="parent">The parent of the CheckBoxObject instance.</param>
        /// <returns>The CheckBoxObject instance.</returns>
        public static CheckBoxObject CreateCheckBoxObject (string name, Base parent)
        {
            var checkBox = new CheckBoxObject();
            checkBox.Name = name;
            if ((parent as IParent).CanContain (checkBox))
            {
                checkBox.Parent = parent;
            }

            if (!IsValidIdentifier (checkBox.Name))
            {
                checkBox.CreateUniqueName();
            }

            return checkBox;
        }

        /// <summary>
        /// Creates a HtmlObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the HtmlObject instance.</param>
        /// <param name="parent">The parent of the HtmlObject instance.</param>
        /// <returns>The HtmlObject instance.</returns>
        public static HtmlObject CreateHtmlObject (string name, Base parent)
        {
            var html = new HtmlObject();
            html.Name = name;
            if ((parent as IParent).CanContain (html))
            {
                html.Parent = parent;
            }

            if (!IsValidIdentifier (html.Name))
            {
                html.CreateUniqueName();
            }

            return html;
        }

        /// <summary>
        /// Creates a TableObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the TableObject instance.</param>
        /// <param name="parent">The parent of the TableObject instance.</param>
        /// <returns>The TableObject instance.</returns>
        public static TableObject CreateTableObject (string name, Base parent)
        {
            var table = new TableObject();
            table.Name = name;
            if ((parent as IParent).CanContain (table))
            {
                table.Parent = parent;
            }

            if (!IsValidIdentifier (table.Name))
            {
                table.CreateUniqueName();
            }

            return table;
        }

        /// <summary>
        /// Creates a MatrixObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the MatrixObject instance.</param>
        /// <param name="parent">The parent of the MatrixObject instance.</param>
        /// <returns>The MatrixObject instance.</returns>
        public static MatrixObject CreateMatrixObject (string name, Base parent)
        {
            var matrix = new MatrixObject();
            matrix.Name = name;
            if ((parent as IParent).CanContain (matrix))
            {
                matrix.Parent = parent;
            }

            if (!IsValidIdentifier (matrix.Name))
            {
                matrix.CreateUniqueName();
            }

            return matrix;
        }

        /// <summary>
        /// Creates a BarcodeObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the BarcodeObject instance.</param>
        /// <param name="parent">The parent of the BarcodeObject instance.</param>
        /// <returns>The BarcodeObject instance.</returns>
        public static BarcodeObject CreateBarcodeObject (string name, Base parent)
        {
            var barcode = new BarcodeObject();
            barcode.Name = name;
            if ((parent as IParent).CanContain (barcode))
            {
                barcode.Parent = parent;
            }

            if (!IsValidIdentifier (barcode.Name))
            {
                barcode.CreateUniqueName();
            }

            return barcode;
        }

        /// <summary>
        /// Creates a ZipCodeObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the ZipCodeObject instance.</param>
        /// <param name="parent">The parent of the ZipCodeObject instance.</param>
        /// <returns>The ZipCodeObject instance.</returns>
        public static ZipCodeObject CreateZipCodeObject (string name, Base parent)
        {
            var zipCode = new ZipCodeObject();
            zipCode.Name = name;
            if ((parent as IParent).CanContain (zipCode))
            {
                zipCode.Parent = parent;
            }

            if (!IsValidIdentifier (zipCode.Name))
            {
                zipCode.CreateUniqueName();
            }

            return zipCode;
        }

        /// <summary>
        /// Creates a CellularTextObject instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the CellularTextObject instance.</param>
        /// <param name="parent">The parent ot the CellularTextObject instance.</param>
        /// <returns>The CellularTextObject instance.</returns>
        public static CellularTextObject CreateCellularTextObject (string name, Base parent)
        {
            var cellularText = new CellularTextObject();
            cellularText.Name = name;
            if ((parent as IParent).CanContain (cellularText))
            {
                cellularText.Parent = parent;
            }

            if (!IsValidIdentifier (cellularText.Name))
            {
                cellularText.CreateUniqueName();
            }

            return cellularText;
        }

        /// <summary>
        /// Creates a LinearGauge instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the LinearGauge instance.</param>
        /// <param name="parent">The parent of the LinearGauge instance.</param>
        /// <returns>The LinearGauge instance.</returns>
        public static LinearGauge CreateLinearGauge (string name, Base parent)
        {
            var gauge = new LinearGauge();
            gauge.Name = name;
            if ((parent as IParent).CanContain (gauge))
            {
                gauge.Parent = parent;
            }

            if (!IsValidIdentifier (gauge.Name))
            {
                gauge.CreateUniqueName();
            }

            return gauge;
        }

        /// <summary>
        /// Creates a SimpleGauge instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the SimpleGauge instance.</param>
        /// <param name="parent">The parent of the SimpleGauge instance.</param>
        /// <returns>The SimpleGauge instance.</returns>
        public static SimpleGauge CreateSimpleGauge (string name, Base parent)
        {
            var gauge = new SimpleGauge();
            gauge.Name = name;
            if ((parent as IParent).CanContain (gauge))
            {
                gauge.Parent = parent;
            }

            if (!IsValidIdentifier (gauge.Name))
            {
                gauge.CreateUniqueName();
            }

            return gauge;
        }

        /// <summary>
        /// Creates a RadialGauge instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the RadialGauge instance.</param>
        /// <param name="parent">The parent of the RadialGauge instance.</param>
        /// <returns>The RadialGauge instance.</returns>
        public static Gauge.Radial.RadialGauge CreateRadialGauge (string name, Base parent)
        {
            var gauge = new Gauge.Radial.RadialGauge();
            gauge.Name = name;
            if ((parent as IParent).CanContain (gauge))
            {
                gauge.Parent = parent;
            }

            if (!IsValidIdentifier (gauge.Name))
            {
                gauge.CreateUniqueName();
            }

            return gauge;
        }

        /// <summary>
        /// Creates a SimpleProgressGauge instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the SimpleProgressGauge instance.</param>
        /// <param name="parent">The parent of the SimpleProgressGauge instance.</param>
        /// <returns>The SimpleProgressGauge instance.</returns>
        public static Gauge.Simple.Progress.SimpleProgressGauge CreateSimpleProgressGauge (string name, Base parent)
        {
            var gauge = new Gauge.Simple.Progress.SimpleProgressGauge();
            gauge.Name = name;
            if ((parent as IParent).CanContain (gauge))
            {
                gauge.Parent = parent;
            }

            if (!IsValidIdentifier (gauge.Name))
            {
                gauge.CreateUniqueName();
            }

            return gauge;
        }

        #endregion // Objects

        #region Dictionary Elements

        /// <summary>
        /// Creates a Parameter instance with specified name and parent.
        /// </summary>
        /// <param name="name">The name of the Parameter instance.</param>
        /// <param name="report">The parent Report for the new Parameter.</param>
        /// <returns>The Parameter instance.</returns>
        public static Parameter CreateParameter (string name, Report report)
        {
            var parameter = new Parameter (name);
            report.Parameters.Add (parameter);
            return parameter;
        }

        #endregion Dictionary Elements
    }
}
