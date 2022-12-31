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
using System.Collections.Generic;
using System.ComponentModel;

using AM.Reporting.Utils;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a report page.
    /// </summary>
    /// <remarks>
    /// To get/set a paper size and orientation, use the <see cref="PaperWidth"/>, <see cref="PaperHeight"/>
    /// and <see cref="Landscape"/> properties. Note that paper size is measured in millimeters.
    /// <para/>Report page can contain one or several bands with report objects. Use the <see cref="ReportTitle"/>,
    /// <see cref="ReportSummary"/>, <see cref="PageHeader"/>, <see cref="PageFooter"/>,
    /// <see cref="ColumnHeader"/>, <see cref="ColumnFooter"/>, <see cref="Overlay"/> properties
    /// to get/set the page bands. The <see cref="Bands"/> property holds the list of data bands or groups.
    /// Thus you may add several databands to this property to create master-master reports, for example.
    /// <note type="caution">
    /// Report page can contain bands only. You cannot place report objects such as <b>TextObject</b> on a page.
    /// </note>
    /// </remarks>
    /// <example>
    /// This example shows how to create a page with one <b>ReportTitleBand</b> and <b>DataBand</b> bands and add
    /// it to the report.
    /// <code>
    /// ReportPage page = new ReportPage();
    /// // set the paper in millimeters
    /// page.PaperWidth = 210;
    /// page.PaperHeight = 297;
    /// // create report title
    /// page.ReportTitle = new ReportTitleBand();
    /// page.ReportTitle.Name = "ReportTitle1";
    /// page.ReportTitle.Height = Units.Millimeters * 10;
    /// // create data band
    /// DataBand data = new DataBand();
    /// data.Name = "Data1";
    /// data.Height = Units.Millimeters * 10;
    /// // add data band to the page
    /// page.Bands.Add(data);
    /// // add page to the report
    /// report.Pages.Add(page);
    /// </code>
    /// </example>
    public partial class ReportPage : PageBase, IParent
    {
        #region Constants

        private const float MAX_PAPER_SIZE_MM = 2000000000;

        #endregion // Constants

        #region Fields

        private bool landscape;
        private FillBase fill;
        private Watermark watermark;
        private PageHeaderBand pageHeader;
        private ReportTitleBand reportTitle;
        private ColumnHeaderBand columnHeader;
        private ReportSummaryBand reportSummary;
        private ColumnFooterBand columnFooter;
        private PageFooterBand pageFooter;
        private OverlayBand overlay;

        private bool unlimitedHeight;
        private bool printOnRollPaper;
        private float unlimitedHeightValue;

        #endregion

        #region Properties

        /// <summary>
        /// This event occurs when the report engine starts this page.
        /// </summary>
        public event EventHandler StartPage;

        /// <summary>
        /// This event occurs when the report engine finished this page.
        /// </summary>
        public event EventHandler FinishPage;

        /// <summary>
        /// This event occurs when the report engine is about to print databands in this page.
        /// </summary>
        public event EventHandler ManualBuild;

        /// <summary>
        /// Gets or sets a width of the paper, in millimeters.
        /// </summary>
        [Category ("Paper")]
        [TypeConverter ("AM.Reporting.TypeConverters.PaperConverter, AM.Reporting")]
        public float PaperWidth { get; set; }

        /// <summary>
        /// Gets or sets the page name on export
        /// </summary>
        [Category ("Paper")]
        public string ExportAlias { get; set; }

        /// <summary>
        /// Gets or sets a height of the paper, in millimeters.
        /// </summary>
        [Category ("Paper")]
        [TypeConverter ("AM.Reporting.TypeConverters.PaperConverter, AM.Reporting")]
        public float PaperHeight { get; set; }

        /// <summary>
        /// Gets or sets the raw index of a paper size.
        /// </summary>
        /// <remarks>
        /// This property stores the RawKind value of a selected papersize. It is used to distiguish
        /// between several papers with the same size (for ex. "A3" and "A3 with no margins") used in some
        /// printer drivers.
        /// <para/>It is not obligatory to set this property. AM.Reporting will select the
        /// necessary paper using the <b>PaperWidth</b> and <b>PaperHeight</b> values.
        /// </remarks>
        [Category ("Paper")]
        [DefaultValue (0)]
        public int RawPaperSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page has unlimited height.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Paper")]
        public bool UnlimitedHeight
        {
            get => unlimitedHeight;
            set
            {
                unlimitedHeight = value;
                if (!unlimitedHeight)
                {
                    printOnRollPaper = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the unlimited page should be printed on roll paper.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Paper")]
        public bool PrintOnRollPaper
        {
            get => printOnRollPaper;
            set
            {
                if (unlimitedHeight)
                {
                    printOnRollPaper = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page has unlimited width.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Paper")]
        public bool UnlimitedWidth { get; set; }

        /// <summary>
        /// Get or set the current height of unlimited page.
        /// </summary>
        [Browsable (false)]
        public float UnlimitedHeightValue
        {
            get => unlimitedHeightValue;
            set
            {
                unlimitedHeightValue = value;
                if (printOnRollPaper)
                {
                    PaperHeight = unlimitedHeightValue / Units.Millimeters;
                }
            }
        }

        /// <summary>
        /// Get or set the current width of unlimited page.
        /// </summary>
        [Browsable (false)]
        public float UnlimitedWidthValue { get; set; }

        /// <summary>
        /// Gets the current page height in pixels.
        /// </summary>
        [Browsable (false)]
        public float HeightInPixels => UnlimitedHeight ? UnlimitedHeightValue : PaperHeight * Units.Millimeters;

        /// <summary>
        /// Gets the current page width in pixels.
        /// </summary>
        [Browsable (false)]
        public float WidthInPixels
        {
            get
            {
                if (UnlimitedWidth)
                {
                    if (!IsDesigning)
                    {
                        return UnlimitedWidthValue;
                    }
                }

                return PaperWidth * Units.Millimeters;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that page should be in landscape orientation.
        /// </summary>
        /// <remarks>
        /// When you change this property, it will automatically swap paper width and height, as well as paper margins.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Paper")]
        public bool Landscape
        {
            get => landscape;
            set
            {
                if (landscape != value)
                {
                    var e = PaperWidth;
                    PaperWidth = PaperHeight;
                    PaperHeight = e;

                    var m1 = LeftMargin; //     m3
                    var m2 = RightMargin; //  m1    m2
                    var m3 = TopMargin; //     m4
                    var m4 = BottomMargin; //

                    if (value)
                    {
                        LeftMargin = m3; // rotate counter-clockwise
                        RightMargin = m4;
                        TopMargin = m2;
                        BottomMargin = m1;
                    }
                    else
                    {
                        LeftMargin = m4; // rotate clockwise
                        RightMargin = m3;
                        TopMargin = m1;
                        BottomMargin = m2;
                    }
                }

                landscape = value;
            }
        }

        /// <summary>
        /// Gets or sets the left page margin, in millimeters.
        /// </summary>
        [Category ("Paper")]
        [TypeConverter ("AM.Reporting.TypeConverters.PaperConverter, AM.Reporting")]
        public float LeftMargin { get; set; }

        /// <summary>
        /// Gets or sets the top page margin, in millimeters.
        /// </summary>
        [Category ("Paper")]
        [TypeConverter ("AM.Reporting.TypeConverters.PaperConverter, AM.Reporting")]
        public float TopMargin { get; set; }

        /// <summary>
        /// Gets or sets the right page margin, in millimeters.
        /// </summary>
        [Category ("Paper")]
        [TypeConverter ("AM.Reporting.TypeConverters.PaperConverter, AM.Reporting")]
        public float RightMargin { get; set; }

        /// <summary>
        /// Gets or sets the bottom page margin, in millimeters.
        /// </summary>
        [Category ("Paper")]
        [TypeConverter ("AM.Reporting.TypeConverters.PaperConverter, AM.Reporting")]
        public float BottomMargin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that even pages should swap its left and right margins when
        /// previewed or printed.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool MirrorMargins { get; set; }

        /// <summary>
        /// Gets the page columns settings.
        /// </summary>
        [Category ("Appearance")]
        public PageColumns Columns { get; }

        /// <summary>
        /// Gets or sets the page border that will be printed inside the page printing area.
        /// </summary>
        [Category ("Appearance")]
        public Border Border { get; set; }


        /// <summary>
        /// Gets or sets the page background fill.
        /// </summary>
        [Category ("Appearance")]
        [Editor ("AM.Reporting.TypeEditors.FillEditor, AM.Reporting", typeof (UITypeEditor))]
        public FillBase Fill
        {
            get => fill;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException ("Fill");
                }

                fill = value;
            }
        }

        /// <summary>
        /// Gets or sets the page watermark.
        /// </summary>
        /// <remarks>
        /// To enabled watermark, set its <b>Enabled</b> property to <b>true</b>.
        /// </remarks>
        [Category ("Appearance")]
        public Watermark Watermark
        {
            get => watermark;
            set
            {
                if (watermark != value)
                {
                    if (watermark != null)
                    {
                        watermark.Dispose();
                    }
                }

                watermark = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that <b>ReportTitle</b> band should be printed before the
        /// <b>PageHeader</b> band.
        /// </summary>
        [DefaultValue (true)]
        [Category ("Behavior")]
        public bool TitleBeforeHeader { get; set; }

        /// <summary>
        /// Gets or sets an outline expression.
        /// </summary>
        /// <remarks>
        /// For more information, see <see cref="BandBase.OutlineExpression"/> property.
        /// </remarks>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string OutlineExpression { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to start to print this page on a free space of the previous page.
        /// </summary>
        /// <remarks>
        /// This property can be used if you have two or more pages in the report template.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool PrintOnPreviousPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating that AM.Reporting engine must reset page numbers before printing this page.
        /// </summary>
        /// <remarks>
        /// This property can be used if you have two or more pages in the report template.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool ResetPageNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page has extra width in the report designer.
        /// </summary>
        /// <remarks>
        /// This property may be useful if you work with such objects as Matrix and Table.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Design")]
        public bool ExtraDesignWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this page will start on an odd page only.
        /// </summary>
        /// <remarks>
        /// This property is useful to print booklet-type reports. Setting this property to <b>true</b>
        /// means that this page will start to print on an odd page only. If necessary, an empty page
        /// will be added to the prepared report before this page will be printed.
        /// </remarks>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool StartOnOddPage { get; set; }

        /// <summary>
        /// Uses this page as a back page for previously printed pages.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Behavior")]
        public bool BackPage { get; set; }

        /// <summary>
        /// Gets or sets a report title band.
        /// </summary>
        [Browsable (false)]
        public ReportTitleBand ReportTitle
        {
            get => reportTitle;
            set
            {
                SetProp (reportTitle, value);
                reportTitle = value;
            }
        }

        /// <summary>
        /// Gets or sets a report summary band.
        /// </summary>
        [Browsable (false)]
        public ReportSummaryBand ReportSummary
        {
            get => reportSummary;
            set
            {
                SetProp (reportSummary, value);
                reportSummary = value;
            }
        }

        /// <summary>
        /// Gets or sets a page header band.
        /// </summary>
        [Browsable (false)]
        public PageHeaderBand PageHeader
        {
            get => pageHeader;
            set
            {
                SetProp (pageHeader, value);
                pageHeader = value;
            }
        }

        /// <summary>
        /// Gets or sets a page footer band.
        /// </summary>
        [Browsable (false)]
        public PageFooterBand PageFooter
        {
            get => pageFooter;
            set
            {
                SetProp (pageFooter, value);
                pageFooter = value;
            }
        }

        /// <summary>
        /// Gets or sets a column header band.
        /// </summary>
        [Browsable (false)]
        public ColumnHeaderBand ColumnHeader
        {
            get => columnHeader;
            set
            {
                SetProp (columnHeader, value);
                columnHeader = value;
            }
        }

        /// <summary>
        /// Gets or sets a column footer band.
        /// </summary>
        [Browsable (false)]
        public ColumnFooterBand ColumnFooter
        {
            get => columnFooter;
            set
            {
                SetProp (columnFooter, value);
                columnFooter = value;
            }
        }

        /// <summary>
        /// Gets or sets an overlay band.
        /// </summary>
        [Browsable (false)]
        public OverlayBand Overlay
        {
            get => overlay;
            set
            {
                SetProp (overlay, value);
                overlay = value;
            }
        }

        /// <summary>
        /// Gets the collection of data bands or group header bands.
        /// </summary>
        /// <remarks>
        /// The <b>Bands</b> property holds the list of data bands or group headers.
        /// Thus you may add several databands to this property to create master-master reports, for example.
        /// </remarks>
        [Browsable (false)]
        public BandCollection Bands { get; }

        /// <summary>
        /// Gets or sets the page guidelines.
        /// </summary>
        /// <remarks>
        /// This property hold all vertical guidelines. The horizontal guidelines are owned by the bands (see
        /// <see cref="BandBase.Guides"/> property).
        /// </remarks>
        [Browsable (false)]
        public FloatCollection Guides { get; set; }

        /// <summary>
        /// Gets or sets the reference to a parent <b>SubreportObject</b> that owns this page.
        /// </summary>
        /// <remarks>
        /// This property is <b>null</b> for regular report pages. See the <see cref="SubreportObject"/> for details.
        /// </remarks>
        [Browsable (false)]
        public SubreportObject Subreport { get; set; }

        /// <summary>
        /// Gets or sets a script event name that will be fired when the report engine starts this page.
        /// </summary>
        [Category ("Build")]
        public string StartPageEvent { get; set; }

        /// <summary>
        /// Gets or sets a script event name that will be fired when the report engine finished this page.
        /// </summary>
        [Category ("Build")]
        public string FinishPageEvent { get; set; }

        /// <summary>
        /// Gets or sets a script event name that will be fired when the report engine is about
        /// to print databands in this page.
        /// </summary>
        [Category ("Build")]
        public string ManualBuildEvent { get; set; }

        internal bool IsManualBuild => !string.IsNullOrEmpty (ManualBuildEvent) || ManualBuild != null;

        #endregion

        #region Private Methods

        private void DrawBackground (FRPaintEventArgs e, RectangleF rect)
        {
            rect.Width *= e.ScaleX;
            rect.Height *= e.ScaleY;
            Brush brush = null;
            if (Fill is SolidFill)
            {
                brush = e.Cache.GetBrush ((Fill as SolidFill).Color);
            }
            else
            {
                brush = Fill.CreateBrush (rect, e.ScaleX, e.ScaleY);
            }

            e.Graphics.FillRectangle (brush, rect.Left, rect.Top, rect.Width, rect.Height);
            if (Fill is not SolidFill)
            {
                brush.Dispose();
            }
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                if (Subreport != null)
                {
                    Subreport.ReportPage = null;
                }

                if (Watermark != null)
                {
                    Watermark.Dispose();
                    Watermark = null;
                }
            }

            base.Dispose (disposing);
        }

        #endregion

        #region IParent

        /// <inheritdoc/>
        public virtual void GetChildObjects (ObjectCollection list)
        {
            if (TitleBeforeHeader)
            {
                list.Add (reportTitle);
                list.Add (pageHeader);
            }
            else
            {
                list.Add (pageHeader);
                list.Add (reportTitle);
            }

            list.Add (columnHeader);
            foreach (BandBase band in Bands)
            {
                list.Add (band);
            }

            list.Add (reportSummary);
            list.Add (columnFooter);
            list.Add (pageFooter);
            list.Add (overlay);
        }

        /// <inheritdoc/>
        public virtual bool CanContain (Base child)
        {
            if (IsRunning)
            {
                return child is BandBase;
            }

            return (child is PageHeaderBand || child is ReportTitleBand || child is ColumnHeaderBand ||
                    child is DataBand || child is GroupHeaderBand || child is ColumnFooterBand ||
                    child is ReportSummaryBand || child is PageFooterBand || child is OverlayBand);
        }

        /// <inheritdoc/>
        public virtual void AddChild (Base child)
        {
            if (IsRunning)
            {
                Bands.Add (child as BandBase);
                return;
            }

            if (child is PageHeaderBand band)
            {
                PageHeader = band;
            }

            if (child is ReportTitleBand titleBand)
            {
                ReportTitle = titleBand;
            }

            if (child is ColumnHeaderBand headerBand)
            {
                ColumnHeader = headerBand;
            }

            if (child is DataBand || child is GroupHeaderBand)
            {
                Bands.Add (child as BandBase);
            }

            if (child is ReportSummaryBand summaryBand)
            {
                ReportSummary = summaryBand;
            }

            if (child is ColumnFooterBand footerBand)
            {
                ColumnFooter = footerBand;
            }

            if (child is PageFooterBand pageFooterBand)
            {
                PageFooter = pageFooterBand;
            }

            if (child is OverlayBand overlayBand)
            {
                Overlay = overlayBand;
            }
        }

        /// <inheritdoc/>
        public virtual void RemoveChild (Base child)
        {
            if (IsRunning)
            {
                Bands.Remove (child as BandBase);
                return;
            }

            if (child is PageHeaderBand band && pageHeader == band)
            {
                PageHeader = null;
            }

            if (child is ReportTitleBand titleBand && reportTitle == titleBand)
            {
                ReportTitle = null;
            }

            if (child is ColumnHeaderBand headerBand && columnHeader == headerBand)
            {
                ColumnHeader = null;
            }

            if (child is DataBand || child is GroupHeaderBand)
            {
                Bands.Remove (child as BandBase);
            }

            if (child is ReportSummaryBand summaryBand && reportSummary == summaryBand)
            {
                ReportSummary = null;
            }

            if (child is ColumnFooterBand footerBand && columnFooter == footerBand)
            {
                ColumnFooter = null;
            }

            if (child is PageFooterBand pageFooterBand && pageFooter == pageFooterBand)
            {
                PageFooter = null;
            }

            if (child is OverlayBand overlayBand && overlay == overlayBand)
            {
                Overlay = null;
            }
        }

        /// <inheritdoc/>
        public virtual int GetChildOrder (Base child)
        {
            return Bands.IndexOf (child as BandBase);
        }

        /// <inheritdoc/>
        public virtual void SetChildOrder (Base child, int order)
        {
            if (order > Bands.Count)
            {
                order = Bands.Count;
            }

            var oldOrder = child.ZOrder;
            if (oldOrder != -1 && order != -1 && oldOrder != order)
            {
                if (oldOrder <= order)
                {
                    order--;
                }

                Bands.Remove (child as BandBase);
                Bands.Insert (order, child as BandBase);
            }
        }

        /// <inheritdoc/>
        public virtual void UpdateLayout (float dx, float dy)
        {
            // do nothing
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as ReportPage;
            ExportAlias = src.ExportAlias;
            Landscape = src.Landscape;
            PaperWidth = src.PaperWidth;
            PaperHeight = src.PaperHeight;
            RawPaperSize = src.RawPaperSize;
            LeftMargin = src.LeftMargin;
            TopMargin = src.TopMargin;
            RightMargin = src.RightMargin;
            BottomMargin = src.BottomMargin;
            MirrorMargins = src.MirrorMargins;
            AssignPreview (src);
            Columns.Assign (src.Columns);
            Guides.Assign (src.Guides);
            Border = src.Border.Clone();
            Fill = src.Fill.Clone();
            Watermark.Assign (src.Watermark);
            TitleBeforeHeader = src.TitleBeforeHeader;
            OutlineExpression = src.OutlineExpression;
            PrintOnPreviousPage = src.PrintOnPreviousPage;
            ResetPageNumber = src.ResetPageNumber;
            ExtraDesignWidth = src.ExtraDesignWidth;
            BackPage = src.BackPage;
            StartOnOddPage = src.StartOnOddPage;
            StartPageEvent = src.StartPageEvent;
            FinishPageEvent = src.FinishPageEvent;
            ManualBuildEvent = src.ManualBuildEvent;
            UnlimitedHeight = src.UnlimitedHeight;
            PrintOnRollPaper = src.PrintOnRollPaper;
            UnlimitedWidth = src.UnlimitedWidth;
            UnlimitedHeightValue = src.UnlimitedHeightValue;
            UnlimitedWidthValue = src.UnlimitedWidthValue;
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as ReportPage;
            base.Serialize (writer);
            if (ExportAlias != c.ExportAlias)
            {
                writer.WriteStr ("ExportAlias", ExportAlias);
            }

            if (Landscape != c.Landscape)
            {
                writer.WriteBool ("Landscape", Landscape);
            }

            if (FloatDiff (PaperWidth, c.PaperWidth) || Landscape != c.Landscape)
            {
                writer.WriteFloat ("PaperWidth", PaperWidth);
            }

            if (FloatDiff (PaperHeight, c.PaperHeight) || Landscape != c.Landscape)
            {
                writer.WriteFloat ("PaperHeight", PaperHeight);
            }

            if (RawPaperSize != c.RawPaperSize)
            {
                writer.WriteInt ("RawPaperSize", RawPaperSize);
            }

            if (FloatDiff (LeftMargin, c.LeftMargin))
            {
                writer.WriteFloat ("LeftMargin", LeftMargin);
            }

            if (FloatDiff (TopMargin, c.TopMargin))
            {
                writer.WriteFloat ("TopMargin", TopMargin);
            }

            if (FloatDiff (RightMargin, c.RightMargin))
            {
                writer.WriteFloat ("RightMargin", RightMargin);
            }

            if (FloatDiff (BottomMargin, c.BottomMargin))
            {
                writer.WriteFloat ("BottomMargin", BottomMargin);
            }

            if (MirrorMargins != c.MirrorMargins)
            {
                writer.WriteBool ("MirrorMargins", MirrorMargins);
            }

            WritePreview (writer, c);
            Columns.Serialize (writer, c.Columns);
            if (Guides.Count > 0)
            {
                writer.WriteValue ("Guides", Guides);
            }

            Border.Serialize (writer, "Border", c.Border);
            Fill.Serialize (writer, "Fill", c.Fill);
            Watermark.Serialize (writer, "Watermark", c.Watermark);
            if (TitleBeforeHeader != c.TitleBeforeHeader)
            {
                writer.WriteBool ("TitleBeforeHeader", TitleBeforeHeader);
            }

            if (OutlineExpression != c.OutlineExpression)
            {
                writer.WriteStr ("OutlineExpression", OutlineExpression);
            }

            if (PrintOnPreviousPage != c.PrintOnPreviousPage)
            {
                writer.WriteBool ("PrintOnPreviousPage", PrintOnPreviousPage);
            }

            if (ResetPageNumber != c.ResetPageNumber)
            {
                writer.WriteBool ("ResetPageNumber", ResetPageNumber);
            }

            if (ExtraDesignWidth != c.ExtraDesignWidth)
            {
                writer.WriteBool ("ExtraDesignWidth", ExtraDesignWidth);
            }

            if (StartOnOddPage != c.StartOnOddPage)
            {
                writer.WriteBool ("StartOnOddPage", StartOnOddPage);
            }

            if (BackPage != c.BackPage)
            {
                writer.WriteBool ("BackPage", BackPage);
            }

            if (StartPageEvent != c.StartPageEvent)
            {
                writer.WriteStr ("StartPageEvent", StartPageEvent);
            }

            if (FinishPageEvent != c.FinishPageEvent)
            {
                writer.WriteStr ("FinishPageEvent", FinishPageEvent);
            }

            if (ManualBuildEvent != c.ManualBuildEvent)
            {
                writer.WriteStr ("ManualBuildEvent", ManualBuildEvent);
            }

            if (UnlimitedHeight != c.UnlimitedHeight)
            {
                writer.WriteBool ("UnlimitedHeight", UnlimitedHeight);
            }

            if (PrintOnRollPaper != c.PrintOnRollPaper)
            {
                writer.WriteBool ("PrintOnRollPaper", PrintOnRollPaper);
            }

            if (UnlimitedWidth != c.UnlimitedWidth)
            {
                writer.WriteBool ("UnlimitedWidth", UnlimitedWidth);
            }

            if (FloatDiff (UnlimitedHeightValue, c.UnlimitedHeightValue))
            {
                writer.WriteFloat ("UnlimitedHeightValue", UnlimitedHeightValue);
            }

            if (FloatDiff (UnlimitedWidthValue, c.UnlimitedWidthValue))
            {
                writer.WriteFloat ("UnlimitedWidthValue", UnlimitedWidthValue);
            }
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs e)
        {
            if (IsDesigning)
            {
                return;
            }

            var g = e.Graphics;
            var pageRect = new RectangleF (0, 0,
                WidthInPixels - 1 / e.ScaleX, HeightInPixels - 1 / e.ScaleY);
            var printableRect = new RectangleF (
                LeftMargin * Units.Millimeters,
                TopMargin * Units.Millimeters,
                (PaperWidth - LeftMargin - RightMargin) * Units.Millimeters,
                (PaperHeight - TopMargin - BottomMargin) * Units.Millimeters);

            // Fix System.OverflowException when drawing unlimited page without preparing.
            if ((UnlimitedHeight || UnlimitedWidth) && !(IsRunning || IsPrinting))
            {
                pageRect = printableRect;
            }

            DrawBackground (e, pageRect);
            Border.Draw (e, printableRect);
            if (Watermark.Enabled)
            {
                if (!Watermark.ShowImageOnTop)
                {
                    Watermark.DrawImage (e, pageRect, Report, IsPrinting);
                }

                if (!Watermark.ShowTextOnTop)
                {
                    Watermark.DrawText (e, pageRect, Report, IsPrinting);
                }
            }

            float leftMargin = (int)Math.Round (LeftMargin * Units.Millimeters * e.ScaleX);
            float topMargin = (int)Math.Round (TopMargin * Units.Millimeters * e.ScaleY);
            g.TranslateTransform (leftMargin, topMargin);

            try
            {
                foreach (Base c in AllObjects)
                {
                    if (c is ReportComponentBase @base && @base.HasFlag (Flags.CanDraw))
                    {
                        if (!IsPrinting)
                        {
#if !MONO
                            if (!@base.IsVisible (e))
                            {
                                continue;
                            }
#endif
                        }
                        else
                        {
                            if (!@base.Printable)
                            {
                                continue;
                            }
                            else if (@base.Parent is BandBase { Printable: false })
                            {
                                continue;
                            }
                        }

                        @base.SetDesigning (false);
                        @base.SetPrinting (IsPrinting);
                        @base.Draw (e);
                        @base.SetPrinting (false);
                    }
                }
            }
            finally
            {
                g.TranslateTransform (-leftMargin, -topMargin);
            }

            if (Watermark.Enabled)
            {
                if (Watermark.ShowImageOnTop)
                {
                    Watermark.DrawImage (e, pageRect, Report, IsPrinting);
                }

                if (Watermark.ShowTextOnTop)
                {
                    Watermark.DrawText (e, pageRect, Report, IsPrinting);
                }
            }
        }

        internal void InitializeComponents()
        {
            var allObjects = AllObjects;
            foreach (Base obj in allObjects)
            {
                if (obj is ReportComponentBase @base)
                {
                    @base.InitializeComponent();
                }
            }
        }

        internal void FinalizeComponents()
        {
            var allObjects = AllObjects;
            foreach (Base obj in allObjects)
            {
                if (obj is ReportComponentBase @base)
                {
                    @base.FinalizeComponent();
                }
            }
        }

        /// <inheritdoc/>
        public override string[] GetExpressions()
        {
            List<string> expressions = new List<string>();

            if (!string.IsNullOrEmpty (OutlineExpression))
            {
                expressions.Add (OutlineExpression);
            }

            return expressions.ToArray();
        }

        /// <inheritdoc/>
        public override void ExtractMacros()
        {
            Watermark.Text = ExtractDefaultMacros (Watermark.Text);
        }

        /// <summary>
        /// This method fires the <b>StartPage</b> event and the script code connected to the <b>StartPageEvent</b>.
        /// </summary>
        public void OnStartPage (EventArgs e)
        {
            if (StartPage != null)
            {
                StartPage (this, e);
            }

            InvokeEvent (StartPageEvent, e);
        }

        /// <summary>
        /// This method fires the <b>FinishPage</b> event and the script code connected to the <b>FinishPageEvent</b>.
        /// </summary>
        public void OnFinishPage (EventArgs e)
        {
            if (FinishPage != null)
            {
                FinishPage (this, e);
            }

            InvokeEvent (FinishPageEvent, e);
        }

        /// <summary>
        /// This method fires the <b>ManualBuild</b> event and the script code connected to the <b>ManualBuildEvent</b>.
        /// </summary>
        public void OnManualBuild (EventArgs e)
        {
            if (ManualBuild != null)
            {
                ManualBuild (this, e);
            }

            InvokeEvent (ManualBuildEvent, e);
        }

        /// <summary>
        /// Updates width of all bands on this page according to page's paper settings.
        /// </summary>
        public void UpdateBandsWidth()
        {
            var pageWidth = (PaperWidth - LeftMargin - RightMargin) * Units.Millimeters;
            var columnWidth = Columns.Width * Units.Millimeters;

            foreach (Base c in AllObjects)
            {
                if (c is BandBase b)
                {
                    if (Columns.Count > 1 && b.IsColumnDependentBand)
                    {
                        b.Width = columnWidth;
                    }
                    else
                    {
                        b.Width = pageWidth;
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportPage"/> class with default settings.
        /// </summary>
        public ReportPage()
        {
            PaperWidth = 210;
            PaperHeight = 297;
            LeftMargin = 10;
            TopMargin = 10;
            RightMargin = 10;
            BottomMargin = 10;
            InitPreview();
            Bands = new BandCollection (this);
            Guides = new FloatCollection();
            Columns = new PageColumns (this);
            Border = new Border();
            fill = new SolidFill (Color.White);
            watermark = new Watermark();
            TitleBeforeHeader = true;
            StartPageEvent = "";
            FinishPageEvent = "";
            ManualBuildEvent = "";
            BaseName = "Page";
            unlimitedHeight = false;
            printOnRollPaper = false;
            UnlimitedWidth = false;
            unlimitedHeightValue = MAX_PAPER_SIZE_MM * Units.Millimeters;
            UnlimitedWidthValue = MAX_PAPER_SIZE_MM * Units.Millimeters;
        }
    }
}
