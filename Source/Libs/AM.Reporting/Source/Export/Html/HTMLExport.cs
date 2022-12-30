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

using AM.Reporting.Utils;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Export.Html
{
    /// <summary>
    /// Represents the HTML export filter.
    /// </summary>
    public partial class HTMLExport : ExportBase
    {
        /// <summary>
        /// Draw any custom controls
        /// </summary>
        public event EventHandler<CustomDrawEventArgs> CustomDraw;

        /// <summary>
        /// Draw any custom controls.
        /// </summary>
        /// <param name="e"></param>
        public void OnCustomDraw (CustomDrawEventArgs e)
        {
            if (CustomDraw != null)
            {
                CustomDraw (this, e);
            }
        }

        #region Private fields

#if READONLY_STRUCTS
        private readonly struct HTMLData
#else
        private struct HTMLData
#endif
        {
            public readonly int ReportPage;
            public readonly int PageNumber;
            public readonly int CurrentPage;
            public readonly ReportPage page;
            public readonly Stream PagesStream;

            public HTMLData (int reportPage, int pageNumber, int currentPage, ReportPage page, Stream pagesStream)
            {
                ReportPage = reportPage;
                PageNumber = pageNumber;
                CurrentPage = currentPage;
                this.page = page;
                PagesStream = pagesStream;
            }
        }

#if READONLY_STRUCTS
        private readonly struct PicsArchiveItem
#else
        private struct PicsArchiveItem
#endif
        {
            public readonly string FileName;
            public readonly MemoryStream Stream;

            public PicsArchiveItem (string fileName, MemoryStream stream)
            {
                FileName = fileName;
                Stream = stream;
            }
        }

        /// <summary>
        /// Types of html export
        /// </summary>
        public enum ExportType
        {
            /// <summary>
            /// Simple export
            /// </summary>
            Export,

            /// <summary>
            /// Web preview mode
            /// </summary>
            WebPreview,

            /// <summary>
            /// Web print mode
            /// </summary>
            WebPrint
        }

        private MyRes res;
        private HtmlTemplates templates;
        private string targetPath;
        private string targetIndexPath;
        private string targetFileName;
        private string fileName;
        private string navFileName;

        //private string FOutlineFileName;
        private int pagesCount;

        private string documentTitle;
        private string prevWatermarkName;
        private long prevWatermarkSize;
        private string singlePageFileName;
        private string subFolderPath;
        private MemoryStream mimeStream;
        private string boundary;
        private List<PicsArchiveItem> picsArchive;
        private List<ExportIEMStyle> prevStyleList;
        private int prevStyleListIndex;
        private List<string> cssStyles;
        private float hPos;
        private NumberFormatInfo numberFormat;
        private string pageStyleName;

        private const string BODY_BEGIN = "</head>\r\n<body bgcolor=\"#FFFFFF\" text=\"#000000\">";
        private const string BODY_END = "</body>";

        private const string PRINT_JS =
            "<script language=\"javascript\" type=\"text/javascript\"> parent.focus(); parent.print();</script>";

        private const string NBSP = "&nbsp;";
        private int currentPage;
        private HTMLData d;
        private IGraphics htmlMeasureGraphics;
        private float maxWidth, maxHeight;
        private FastString css;
        private FastString htmlPage;
        private float leftMargin, topMargin;

        /// <summary>
        /// hash:base64Image
        /// </summary>
        private Dictionary<string, string> embeddedImages;

        #endregion Private fields

        #region Public properties

        /// <summary>
        /// Gets or sets images, embedded in html (hash:base64Image)
        /// </summary>
        public Dictionary<string, string> EmbeddedImages
        {
            get => embeddedImages;
            set => embeddedImages = value;
        }

        /// <summary>
        /// Sets a ID of report
        /// </summary>
        public string ReportID { get; set; }

        /// <summary>
        /// Sets an onclick template
        /// </summary>
        public string OnClickTemplate { get; set; } = string.Empty;

        /// <summary>
        /// Enable or disable layers export mode
        /// </summary>
        public bool Layers { get; set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public string StylePrefix { get; set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public string WebImagePrefix { get; set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public string WebImageSuffix { get; set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public List<HTMLPageData> PreparedPages { get; private set; }

        /// <summary>
        /// Enable or disable showing of print dialog in browser when html document is opened
        /// </summary>
        public bool Print { get; set; }

        /// <summary>
        /// Enable or disable a picture optimization.
        /// </summary>
        public bool HighQualitySVG { get; set; }

        /// <summary>
        /// Enable or disable preview in Web settings
        /// </summary>
        public bool Preview { get; set; }

        /// <summary>
        /// Enable or disable the breaks between pages in print preview when single page mode is enabled
        /// </summary>
        public bool PageBreaks { get; set; }

        /// <summary>
        /// Specifies the output format
        /// </summary>
        public HTMLExportFormat Format { get; set; }

        /// <summary>
        /// Specifies the width units in HTML export
        /// </summary>
        public HtmlSizeUnits WidthUnits { get; set; }

        /// <summary>
        /// Specifies the height units in HTML export
        /// </summary>
        public HtmlSizeUnits HeightUnits { get; set; }

        /// <summary>
        /// Enable or disable the pictures in HTML export
        /// </summary>
        public bool Pictures { get; set; }

        /// <summary>
        /// Enable or disable embedding pictures in HTML export
        /// </summary>
        public bool EmbedPictures { get; set; }

        /// <summary>
        /// Enable or disable the WEB mode in HTML export
        /// </summary>
        internal bool WebMode { get; set; }

        /// <summary>
        /// Gets or sets html export mode
        /// </summary>
        public ExportType ExportMode { get; set; }

        /// <summary>
        /// Enable or disable the single HTML page creation
        /// </summary>
        public bool SinglePage { get; set; }

        /// <summary>
        /// Enable or disable the page navigator in html export
        /// </summary>
        public bool Navigator { get; set; }

        /// <summary>
        /// Enable or disable the sub-folder for files of export
        /// </summary>
        public bool SubFolder { get; set; }

        /// <summary>
        ///  Gets or sets the Wysiwyg quality of export
        /// </summary>
        public bool Wysiwyg { get; set; }

        /// <summary>
        /// Gets or sets the image format.
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        /// <summary>
        /// Gets print page data
        /// </summary>
        public HTMLPageData PrintPageData { get; private set; }


        /// <summary>
        /// Enable or disable saving streams in GeneratedStreams collection.
        /// </summary>
        public bool SaveStreams { get; set; }

        /// <summary>
        /// Enable or disable margins for pages. Works only for Layers-mode.
        /// </summary>
        public bool EnableMargins { get; set; }

        /// <summary>
        /// Enable or disable export of vector objects such as Barcodes in SVG format.
        /// </summary>
        public bool EnableVectorObjects { get; set; } = true;

        /// <summary>
        /// Not rotate landscape page when print.
        /// </summary>
        public bool NotRotateLandscapePage { get; set; }

        #endregion Public properties

        #region Private methods

        private void GeneratedUpdate (string filename, Stream stream)
        {
            var i = GeneratedFiles.IndexOf (filename);
            if (i == -1)
            {
                GeneratedFiles.Add (filename);
                GeneratedStreams.Add (stream);
            }
            else
            {
                GeneratedStreams[i] = stream;
            }
        }

        private void ExportHTMLPageStart (FastString Page, int PageNumber, int CurrentPage)
        {
            if (WebMode)
            {
                if (!Layers)
                {
                    PreparedPages[CurrentPage].CSSText = Page.ToString();
                    Page.Clear();
                }

                PreparedPages[CurrentPage].PageNumber = PageNumber;
            }

            if (!WebMode && !SinglePage)
            {
                Page.AppendLine (BODY_BEGIN);
            }
        }

        private void ExportHTMLPageFinal (FastString CSS, FastString Page, HTMLData d, float MaxWidth, float MaxHeight)
        {
            if (!WebMode)
            {
                if (!SinglePage)
                {
                    Page.AppendLine (BODY_END);
                }

                if (d.PagesStream == null)
                {
                    var pageFileName = targetIndexPath + targetFileName + d.PageNumber.ToString() + ".html";
                    if (SaveStreams)
                    {
                        var fileName = SinglePage ? singlePageFileName : pageFileName;
                        var i = GeneratedFiles.IndexOf (fileName);
                        var outStream = (i == -1) ? new MemoryStream() : GeneratedStreams[i];
                        DoPage (outStream, documentTitle, CSS, Page);
                        GeneratedUpdate (fileName, outStream);
                    }
                    else
                    {
                        GeneratedFiles.Add (pageFileName);
                        using (var outStream = new FileStream (pageFileName, FileMode.Create))
                        {
                            DoPage (outStream, documentTitle, CSS, Page);
                        }
                    }
                }
                else
                {
                    DoPage (d.PagesStream, documentTitle, CSS, Page);
                }
            }
            else
            {
                ExportHTMLPageFinalWeb (CSS, Page, d, MaxWidth, MaxHeight);
            }
        }

        private void ExportHTMLPageFinalWeb (FastString CSS, FastString Page, HTMLData d, float MaxWidth,
            float MaxHeight)
        {
            CalcPageSize (PreparedPages[d.CurrentPage], MaxWidth, MaxHeight);

            if (Page != null)
            {
                PreparedPages[d.CurrentPage].PageText = Page.ToString();
                Page.Clear();
            }

            if (CSS != null)
            {
                PreparedPages[d.CurrentPage].CSSText = CSS.ToString();
                CSS.Clear();
            }

            PreparedPages[d.CurrentPage].PageEvent.Set();
        }

        private void CalcPageSize (HTMLPageData page, float MaxWidth, float MaxHeight)
        {
            if (!Layers)
            {
                page.Width = MaxWidth / Zoom;
                page.Height = MaxHeight / Zoom;
            }
            else
            {
                page.Width = MaxWidth;
                page.Height = MaxHeight;
            }
        }

        private void DoPage (Stream stream, string documentTitle, FastString CSS, FastString Page)
        {
            if (!SinglePage)
            {
                ExportUtils.Write (stream, string.Format (templates.PageTemplateTitle, documentTitle));
            }

            if (CSS != null)
            {
                ExportUtils.Write (stream, CSS.ToString());
                CSS.Clear();
            }

            if (Page != null)
            {
                ExportUtils.Write (stream, Page.ToString());
                Page.Clear();
            }

            if (!SinglePage)
            {
                ExportUtils.Write (stream, templates.PageTemplateFooter);
            }
        }

        private void ExportHTMLOutline (Stream OutStream)
        {
            if (!WebMode)
            {
                // under construction
            }
            else
            {
                // under construction
            }
        }

        private void DoPageStart (Stream stream, string documentTitle, bool print)
        {
            ExportUtils.Write (stream, string.Format (templates.PageTemplateTitle, documentTitle));
            if (print)
            {
                ExportUtils.WriteLn (stream, PRINT_JS);
            }

            ExportUtils.WriteLn (stream, BODY_BEGIN);
        }

        private void DoPageEnd (Stream stream)
        {
            ExportUtils.WriteLn (stream, BODY_END);
            ExportUtils.Write (stream, templates.PageTemplateFooter);
        }

        private void ExportHTMLIndex (Stream stream)
        {
            ExportUtils.Write (stream, string.Format (templates.IndexTemplate,
                new object[]
                {
                    documentTitle, ExportUtils.HtmlURL (navFileName),
                    ExportUtils.HtmlURL (targetFileName +
                                         (SinglePage ? ".main" : "1") + ".html")
                }));
        }

        private void ExportHTMLNavigator (Stream stream)
        {
            var prefix = ExportUtils.HtmlURL (fileName);

            //  {0} - pages count {1} - name of report {2} multipage document {3} prefix of pages
            //  {4} first caption {5} previous caption {6} next caption {7} last caption
            //  {8} total caption
            ExportUtils.Write (stream, string.Format (templates.NavigatorTemplate,
                new object[]
                {
                    pagesCount.ToString(),
                    documentTitle, (SinglePage ? "0" : "1"),
                    prefix, res.Get ("First"), res.Get ("Prev"),
                    res.Get ("Next"), res.Get ("Last"), res.Get ("Total")
                }));
        }

        private void Init()
        {
            htmlMeasureGraphics = Report.MeasureGraphics;
            cssStyles = new List<string>();
            hPos = 0;
            Count = Report.PreparedPages.Count;
            pagesCount = 0;
            prevWatermarkName = string.Empty;
            prevWatermarkSize = 0;
            prevStyleList = null;
            prevStyleListIndex = 0;
            picsArchive = new List<PicsArchiveItem>();
            GeneratedStreams = new List<Stream>();
        }

        private void StartMHT()
        {
            SubFolder = false;
            SinglePage = true;
            Navigator = false;
            mimeStream = new MemoryStream();
            boundary = ExportUtils.GetID();
        }

        private void StartWeb()
        {
            PreparedPages.Clear();
            for (var i = 0; i < Count; i++)
                PreparedPages.Add (new HTMLPageData());
        }

        private void StartSaveStreams()
        {
            if (SinglePage)
            {
                GeneratedUpdate ("index.html", null);
            }

            SubFolder = false;
            Navigator = false;
        }

        private void FinishMHT()
        {
            DoPageEnd (mimeStream);
            WriteMHTHeader (Stream, FileName);
            WriteMimePart (mimeStream, "text/html", "utf-8", "index.html");
            for (var i = 0; i < picsArchive.Count; i++)
            {
                var imagename = picsArchive[i].FileName;
                WriteMimePart (picsArchive[i].Stream, "image/" + imagename.Substring (imagename.LastIndexOf ('.') + 1),
                    "utf-8", imagename);
            }

            var last = "--" + boundary + "--";
            Stream.Write (Encoding.ASCII.GetBytes (last), 0, last.Length);
        }

        private void FinishSaveStreams()
        {
            // do append in memory stream
            var fileIndex = GeneratedFiles.IndexOf (singlePageFileName);
            ExportHTMLIndex (GeneratedStreams[fileIndex]);
            var outStream = new MemoryStream();
            ExportHTMLNavigator (outStream);
            GeneratedUpdate (targetIndexPath + navFileName, outStream);
        }

        #endregion Private methods

        #region Protected methods

        /// <inheritdoc/>
        protected override string GetFileFilter()
        {
            if (Format == HTMLExportFormat.HTML)
            {
                return new MyRes ("FileFilters").Get ("HtmlFile");
            }
            else
            {
                return new MyRes ("FileFilters").Get ("MhtFile");
            }
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            Init();

            if (SaveStreams)
            {
                StartSaveStreams();
            }

            if (!WebMode)
            {
                if (Format == HTMLExportFormat.MessageHTML)
                {
                    StartMHT();
                }

                if (FileName == "" && Stream != null)
                {
                    targetFileName = "html";
                    SinglePage = true;
                    SubFolder = false;
                    Navigator = false;
                    if (ExportMode == ExportType.WebPrint)
                    {
                        NotRotateLandscapePage = true;
                        for (var i = 0; i < Report.PreparedPages.Count; i++)
                        {
                            if (!Report.PreparedPages.GetPage (i).Landscape)
                            {
                                NotRotateLandscapePage = false;
                                break;
                            }
                        }
                    }

                    if (Format == HTMLExportFormat.HTML && !EmbedPictures)
                    {
                        Pictures = false;
                    }
                }
                else
                {
                    targetFileName = Path.GetFileNameWithoutExtension (FileName);
                    fileName = targetFileName;
                    targetIndexPath = !string.IsNullOrEmpty (FileName) ? Path.GetDirectoryName (FileName) : FileName;
                }

                if (!string.IsNullOrEmpty (targetIndexPath))
                {
                    targetIndexPath += Path.DirectorySeparatorChar;
                }

                if (Preview)
                {
                    Pictures = true;
                    PrintPageData = new HTMLPageData();
                }
                else if (SubFolder)
                {
                    subFolderPath = targetFileName + ".files" + Path.DirectorySeparatorChar;
                    targetPath = targetIndexPath + subFolderPath;
                    targetFileName = subFolderPath + targetFileName;
                    if (!Directory.Exists (targetPath))
                    {
                        Directory.CreateDirectory (targetPath);
                    }
                }
                else
                {
                    targetPath = targetIndexPath;
                }

                navFileName = targetFileName + ".nav.html";

                //FOutlineFileName = FTargetFileName + ".outline.html";
                documentTitle = (!string.IsNullOrEmpty (Report.ReportInfo.Name)
                    ? Report.ReportInfo.Name
                    : Path.GetFileNameWithoutExtension (FileName));

                if (SinglePage)
                {
                    if (Navigator)
                    {
                        singlePageFileName = targetIndexPath + targetFileName + ".main.html";
                        if (SaveStreams)
                        {
                            var pageStream = new MemoryStream();
                            DoPageStart (pageStream, documentTitle, Print);
                            GeneratedUpdate (singlePageFileName, pageStream);
                        }
                        else
                        {
                            using (Stream pageStream = new FileStream (singlePageFileName,
                                       FileMode.Create))
                            {
                                DoPageStart (pageStream, documentTitle, Print);
                            }
                        }
                    }
                    else
                    {
                        singlePageFileName = string.IsNullOrEmpty (FileName) ? "index.html" : FileName;
                        if (SaveStreams)
                        {
                            GeneratedUpdate (singlePageFileName, new MemoryStream());
                        }

                        DoPageStart ((Format == HTMLExportFormat.HTML) ? Stream : mimeStream, documentTitle, Print);
                    }
                }
            }
            else
            {
                StartWeb();
            }
        }

        /// <inheritdoc/>
        protected override void ExportPageBegin (ReportPage page)
        {
            if (ExportMode == ExportType.Export)
            {
                base.ExportPageBegin (page);
            }

            pagesCount++;
            if (!WebMode)
            {
                if (SinglePage)
                {
                    Stream stream;
                    if (Navigator)
                    {
                        if (SaveStreams)
                        {
                            stream = new MemoryStream();
                        }
                        else
                        {
                            stream = new FileStream (singlePageFileName, FileMode.Append);
                        }

                        d = new HTMLData (pagesCount, pagesCount, 0, page, stream);
                        ExportHTMLPageBegin (d);
                    }
                    else
                    {
                        if (Format == HTMLExportFormat.HTML)
                        {
                            stream = Stream;
                        }
                        else
                        {
                            stream = mimeStream;
                        }

                        d = new HTMLData (pagesCount, pagesCount, 0, page, stream);
                        ExportHTMLPageBegin (d);
                    }
                }
                else
                {
                    ProcessPageBegin (pagesCount - 1, pagesCount, page);
                }
            }
            else

                // Web
            {
                ProcessPageBegin (pagesCount - 1, pagesCount, page);
            }
        }

        /// <inheritdoc/>
        protected override void ExportPageEnd (ReportPage page)
        {
            if (!WebMode)
            {
                if (SinglePage)
                {
                    if (Navigator)
                    {
                        if (SaveStreams)
                        {
                            ExportHTMLPageEnd (d);
                            GeneratedUpdate (singlePageFileName, d.PagesStream);
                        }
                        else
                        {
                            ExportHTMLPageEnd (d);
                            d.PagesStream.Close();
                            d.PagesStream.Dispose();
                        }
                    }
                    else
                    {
                        ExportHTMLPageEnd (d);
                    }
                }
                else
                {
                    ProcessPageEnd (pagesCount - 1, pagesCount);
                }
            }
            else

                // Web
            {
                ProcessPageEnd (pagesCount - 1, pagesCount);
            }
        }

        /// <summary>
        /// Process Page with number p and real page ReportPage
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ReportPage"></param>
        /// <param name="page"></param>
        public void ProcessPageBegin (int p, int ReportPage, ReportPage page)
        {
            d = new HTMLData (ReportPage, pagesCount, p, page, null);
            ExportHTMLPageBegin (d);
        }

        /// <summary>
        /// Process Page with number p and real page ReportPage
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ReportPage"></param>
        public void ProcessPageEnd (int p, int ReportPage)
        {
            ExportHTMLPageEnd (d);
        }

        /// <inheritdoc/>
        protected override void Finish()
        {
            if (!WebMode)
            {
                if (Navigator)
                {
                    if (SaveStreams)
                    {
                        FinishSaveStreams();
                    }
                    else
                    {
                        if (SinglePage)
                        {
                            //if (saveStreams) // Commented because saveStreams is always false!!
                            //{
                            //    int fileIndex = GeneratedFiles.IndexOf(singlePageFileName);
                            //    DoPageEnd(generatedStreams[fileIndex]);
                            //}
                            //else
                            //{
                            using (Stream pageStream = new FileStream (singlePageFileName, FileMode.Append))
                            {
                                DoPageEnd (pageStream);
                            }

                            //} // Commented because saveStreams is always false!!
                        }

                        ExportHTMLIndex (Stream);
                        GeneratedFiles.Add (targetIndexPath + navFileName);
                        using (var outStream = new FileStream (targetIndexPath + navFileName, FileMode.Create))
                        {
                            ExportHTMLNavigator (outStream);
                        }

                        //GeneratedFiles.Add(FTargetIndexPath + FOutlineFileName);
                        //using (FileStream OutStream = new FileStream(FTargetIndexPath + FOutlineFileName, FileMode.Create))
                        //    ExportHTMLOutline(OutStream);
                    }
                }
                else if (Format == HTMLExportFormat.MessageHTML)
                {
                    FinishMHT();
                }
                else
                {
                    if (SaveStreams)
                    {
                        if (!string.IsNullOrEmpty (singlePageFileName))
                        {
                            var fileIndex = GeneratedFiles.IndexOf (singlePageFileName);
                            DoPageEnd (GeneratedStreams[fileIndex]);
                        }
                    }
                    else
                    {
                        if (!SinglePage)
                        {
                            DoPageStart (Stream, documentTitle, false);
                            var pageCounter = 0;
                            foreach (var genFile in GeneratedFiles)
                            {
                                var ext = Path.GetExtension (genFile);
                                if (ext == ".html" && genFile != FileName)
                                {
                                    var file = Path.GetFileName (genFile);
                                    if (SubFolder)
                                    {
                                        file = Path.Combine (subFolderPath, file);
                                    }

                                    ExportUtils.WriteLn (Stream,
                                        string.Format ("<a href=\"{0}\">Page {1}</a><br />", file, ++pageCounter));
                                }
                            }
                        }

                        DoPageEnd (Stream);
                    }
                }
            }
        }

        #endregion Protected methods

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            base.Serialize (writer);
            writer.WriteBool ("Layers", Layers);
            writer.WriteBool ("Wysiwyg", Wysiwyg);
            writer.WriteBool ("Pictures", Pictures);
            writer.WriteBool ("EmbedPictures", EmbedPictures);
            writer.WriteBool ("SubFolder", SubFolder);
            writer.WriteBool ("Navigator", Navigator);
            writer.WriteBool ("SinglePage", SinglePage);
            writer.WriteBool ("NotRotateLandscapePage", NotRotateLandscapePage);
            writer.WriteBool ("HighQualitySVG", HighQualitySVG);
        }

        /// <summary>
        /// For internal use only.
        /// </summary>
        public void Init_WebMode()
        {
            SubFolder = false;
            Navigator = false;
            ShowProgress = false;
            PreparedPages = new List<HTMLPageData>();
            WebMode = true;
            OpenAfterExport = false;
        }

        internal void Finish_WebMode()
        {
            PreparedPages.Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HTMLExport"/> class.
        /// </summary>
        public HTMLExport()
        {
            Zoom = 1.0f;
            HasMultipleFiles = true;
            Layers = true;
            Wysiwyg = true;
            Pictures = true;
            WebMode = false;
            SubFolder = true;
            Navigator = true;
            SinglePage = false;
            WidthUnits = HtmlSizeUnits.Pixel;
            HeightUnits = HtmlSizeUnits.Pixel;
            ImageFormat = ImageFormat.Png;
            templates = new HtmlTemplates();
            Format = HTMLExportFormat.HTML;
            prevStyleList = null;
            prevStyleListIndex = 0;
            PageBreaks = true;
            Print = false;
            Preview = false;
            SaveStreams = false;
            numberFormat = new NumberFormatInfo
            {
                NumberGroupSeparator = string.Empty,
                NumberDecimalSeparator = "."
            };
            ExportMode = ExportType.Export;
            res = new MyRes ("Export,Html");
            embeddedImages = new Dictionary<string, string>();
            NotRotateLandscapePage = false;
            HighQualitySVG = false;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="HTMLExport"/> class for WebPreview mode.
        /// </summary>
        public HTMLExport (bool webPreview) : this()
        {
            this.webPreview = webPreview;
            if (webPreview)
            {
                ExportMode = ExportType.WebPreview;
            }
        }
    }

    /// <summary>
    /// Event arguments for custom drawing of report objects.
    /// </summary>
    public class CustomDrawEventArgs : EventArgs
    {
        /// <summary>
        /// Report object
        /// </summary>
        public Report report;

        /// <summary>
        /// ReportObject.
        /// </summary>
        public ReportComponentBase reportObject;

        /// <summary>
        /// Resulting successfull drawing flag.
        /// </summary>
        public bool done = false;

        /// <summary>
        /// Resulting HTML string.
        /// </summary>
        public string html;

        /// <summary>
        /// Resulting CSS string.
        /// </summary>
        public string css;

        /// <summary>
        /// Layers mode when true or Table mode when false.
        /// </summary>
        public bool layers;

        /// <summary>
        /// Zoom value for scale position and sizes.
        /// </summary>
        public float zoom;

        /// <summary>
        /// Left position.
        /// </summary>
        public float left;

        /// <summary>
        /// Top position.
        /// </summary>
        public float top;

        /// <summary>
        /// Width of object.
        /// </summary>
        public float width;

        /// <summary>
        /// Height of object.
        /// </summary>
        public float height;
    }
}
