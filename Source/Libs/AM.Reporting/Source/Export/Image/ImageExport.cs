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
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Export.Image
{
    /// <summary>
    /// Specifies the image export format.
    /// </summary>
    public enum ImageExportFormat
    {
        /// <summary>
        /// Specifies the .bmp format.
        /// </summary>
        Bmp,

        /// <summary>
        /// Specifies the .png format.
        /// </summary>
        Png,

        /// <summary>
        /// Specifies the .jpg format.
        /// </summary>
        Jpeg,

        /// <summary>
        /// Specifies the .gif format.
        /// </summary>
        Gif,

        /// <summary>
        /// Specifies the .tif format.
        /// </summary>
        Tiff,

        /// <summary>
        /// Specifies the .emf format.
        /// </summary>
        Metafile
    }

    /// <summary>
    /// Represents the image export filter.
    /// </summary>
    public partial class ImageExport : ExportBase
    {
        private System.Drawing.Image masterTiffImage;
        private System.Drawing.Image bigImage;
        private Graphics bigGraphics;
        private float curOriginY;
        private bool firstPage;
        private int pageNumber;
        private System.Drawing.Image image;
        private Graphics g;
        private int height;
        private int width;
        private int widthK;
        private string fileSuffix;
        private float zoomX;
        private float zoomY;
        private System.Drawing.Drawing2D.GraphicsState state;

        #region Properties

        /// <summary>
        /// Gets or sets the image format.
        /// </summary>
        public ImageExportFormat ImageFormat { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether to generate separate image file
        /// for each exported page.
        /// </summary>
        /// <remarks>
        /// If this property is set to <b>false</b>, the export filter will produce one big image
        /// containing all exported pages. Be careful using this property with a big report
        /// because it may produce out of memory error.
        /// And also when using Memory Stream and the value is true, an exception will be thrown.
        /// </remarks>
        public bool SeparateFiles { get; set; }

        /// <summary>
        /// Gets or sets image resolution, in dpi.
        /// </summary>
        /// <remarks>
        /// By default this property is set to 96 dpi. Use bigger values (300-600 dpi)
        /// if you going to print the exported images.
        /// </remarks>
        public int Resolution
        {
            get => ResolutionX;
            set
            {
                ResolutionX = value;
                ResolutionY = value;
            }
        }

        /// <summary>
        /// Gets or sets horizontal image resolution, in dpi.
        /// </summary>
        /// <remarks>
        /// Separate horizontal and vertical resolution is used when exporting to TIFF. In other
        /// cases, use the <see cref="Resolution"/> property instead.
        /// </remarks>
        public int ResolutionX { get; set; }

        /// <summary>
        /// Gets or sets vertical image resolution, in dpi.
        /// </summary>
        /// <remarks>
        /// Separate horizontal and vertical resolution is used when exporting to TIFF. In other
        /// cases, use the <see cref="Resolution"/> property instead.
        /// </remarks>
        public int ResolutionY { get; set; }

        /// <summary>
        /// Gets or sets the jpg image quality.
        /// </summary>
        /// <remarks>
        /// This property is used if <see cref="ImageFormat"/> is set to <b>Jpeg</b>. By default
        /// it is set to 100. Use lesser value to decrease the jpg file size.
        /// </remarks>
        public int JpegQuality { get; set; }

        /// <summary>
        /// Gets or sets the value determines whether to produce multi-frame tiff file.
        /// </summary>
        public bool MultiFrameTiff { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether the Tiff export must produce monochrome image.
        /// </summary>
        /// <remarks>
        /// Monochrome tiff image is compressed using the compression method specified in the
        /// <see cref="MonochromeTiffCompression"/> property.
        /// </remarks>
        public bool MonochromeTiff { get; set; }

        /// <summary>
        /// Gets or sets the compression method for a monochrome TIFF image.
        /// </summary>
        /// <remarks>
        /// This property is used only when exporting to TIFF image, and the <see cref="MonochromeTiff"/> property
        /// is set to <b>true</b>.
        /// <para/>The valid values for this property are: <b>EncoderValue.CompressionNone</b>,
        /// <b>EncoderValue.CompressionLZW</b>, <b>EncoderValue.CompressionRle</b>,
        /// <b>EncoderValue.CompressionCCITT3</b>, <b>EncoderValue.CompressionCCITT4</b>.
        /// The default compression method is CCITT4.
        /// </remarks>
        public EncoderValue MonochromeTiffCompression { get; set; }

        /// <summary>
        /// Sets padding in non separate pages
        /// </summary>
        public int PaddingNonSeparatePages { get; set; }

        private bool IsMultiFrameTiff => ImageFormat == ImageExportFormat.Tiff && MultiFrameTiff;

        #endregion

        #region Private Methods

        private System.Drawing.Image CreateImage (int width, int height, string suffix)
        {
            widthK = width;
            if (ImageFormat == ImageExportFormat.Metafile)
            {
                return CreateMetafile (suffix);
            }

            return new Bitmap (width, height);
        }

        private System.Drawing.Image CreateMetafile (string suffix)
        {
            var extension = Path.GetExtension (FileName);
            var fileName = Path.ChangeExtension (FileName, suffix + extension);

            System.Drawing.Image image;
            using (var bmp = new Bitmap (1, 1))
            using (var g = Graphics.FromImage (bmp))
            {
                var hdc = g.GetHdc();
                if (suffix == "")
                {
                    image = new Metafile (Stream, hdc);
                }
                else
                {
                    image = new Metafile (fileName, hdc);
                    if (!GeneratedFiles.Contains (fileName))
                    {
                        GeneratedFiles.Add (fileName);
                    }
                }

                g.ReleaseHdc (hdc);
            }

            return image;
        }

        private Bitmap ConvertToBitonal (Bitmap original)
        {
            Bitmap source = null;

            // If original bitmap is not already in 32 BPP, ARGB format, then convert
            if (original.PixelFormat != PixelFormat.Format32bppArgb)
            {
                source = new Bitmap (original.Width, original.Height, PixelFormat.Format32bppArgb);
                source.SetResolution (original.HorizontalResolution, original.VerticalResolution);
                using (var g = Graphics.FromImage (source))
                {
                    g.DrawImageUnscaled (original, 0, 0);
                }
            }
            else
            {
                source = original;
            }

            // Lock source bitmap in memory
            var sourceData = source.LockBits (new Rectangle (0, 0, source.Width, source.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            // Copy image data to binary array
            var imageSize = sourceData.Stride * sourceData.Height;
            var sourceBuffer = new byte[imageSize];
            Marshal.Copy (sourceData.Scan0, sourceBuffer, 0, imageSize);

            // Unlock source bitmap
            source.UnlockBits (sourceData);

            // Create destination bitmap
            var destination = new Bitmap (source.Width, source.Height, PixelFormat.Format1bppIndexed);

            // Lock destination bitmap in memory
            var destinationData = destination.LockBits (
                new Rectangle (0, 0, destination.Width, destination.Height), ImageLockMode.WriteOnly,
                PixelFormat.Format1bppIndexed);

            // Create destination buffer
            imageSize = destinationData.Stride * destinationData.Height;
            var destinationBuffer = new byte[imageSize];

            var sourceIndex = 0;
            var destinationIndex = 0;
            var pixelTotal = 0;
            byte destinationValue = 0;
            var pixelValue = 128;
            var height = source.Height;
            var width = source.Width;
            var threshold = 500;

            // Iterate lines
            for (var y = 0; y < height; y++)
            {
                sourceIndex = y * sourceData.Stride;
                destinationIndex = y * destinationData.Stride;
                destinationValue = 0;
                pixelValue = 128;

                // Iterate pixels
                for (var x = 0; x < width; x++)
                {
                    // Compute pixel brightness (i.e. total of Red, Green, and Blue values)
                    pixelTotal = sourceBuffer[sourceIndex + 1] + sourceBuffer[sourceIndex + 2] +
                                 sourceBuffer[sourceIndex + 3];
                    if (pixelTotal > threshold)
                    {
                        destinationValue += (byte)pixelValue;
                    }

                    if (pixelValue == 1)
                    {
                        destinationBuffer[destinationIndex] = destinationValue;
                        destinationIndex++;
                        destinationValue = 0;
                        pixelValue = 128;
                    }
                    else
                    {
                        pixelValue >>= 1;
                    }

                    sourceIndex += 4;
                }

                if (pixelValue != 128)
                {
                    destinationBuffer[destinationIndex] = destinationValue;
                }
            }

            // Copy binary image data to destination bitmap
            Marshal.Copy (destinationBuffer, 0, destinationData.Scan0, imageSize);

            // Unlock destination bitmap
            destination.UnlockBits (destinationData);

            // Dispose of source if not originally supplied bitmap
            if (source != original)
            {
                source.Dispose();
            }

            // Return
            destination.SetResolution (ResolutionX, ResolutionY);
            return destination;
        }

        private void SaveImage (System.Drawing.Image image, string suffix)
        {
            // store the resolution in output file.
            // Call this method after actual draw because it may affect drawing the text
            if (image is Bitmap bitmap)
            {
                bitmap.SetResolution (ResolutionX, ResolutionY);
            }

            if (IsMultiFrameTiff)
            {
                // select the image encoder
                var info = ExportUtils.GetCodec ("image/tiff");
                var ep = new EncoderParameters (2);
                ep.Param[0] = new EncoderParameter (Encoder.Compression,
                    MonochromeTiff ? (long)MonochromeTiffCompression : (long)EncoderValue.CompressionLZW);

                if (image == masterTiffImage)
                {
                    // save the master bitmap
                    if (MonochromeTiff)
                    {
                        masterTiffImage = ConvertToBitonal (image as Bitmap);
                    }

                    ep.Param[1] = new EncoderParameter (Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                    masterTiffImage.Save (Stream, info, ep);
                }
                else
                {
                    // save the frame
                    if (MonochromeTiff)
                    {
                        var oldImage = image;
                        image = ConvertToBitonal (image as Bitmap);
                        oldImage.Dispose();
                    }

                    ep.Param[1] = new EncoderParameter (Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                    masterTiffImage.SaveAdd (image, ep);
                }
            }
            else if (ImageFormat != ImageExportFormat.Metafile)
            {
                var extension = Path.GetExtension (FileName);
                var fileName = Path.ChangeExtension (FileName, suffix + extension);

                // empty suffix means that we should use the Stream that was created in the ExportBase
                var stream = suffix == "" ? Stream : new FileStream (fileName, FileMode.Create);

                if (suffix != "")
                {
                    GeneratedFiles.Add (fileName);
                }

                if (ImageFormat == ImageExportFormat.Jpeg)
                {
                    ExportUtils.SaveJpeg (image, stream, JpegQuality);
                }
                else if (ImageFormat == ImageExportFormat.Tiff && MonochromeTiff)
                {
                    // handle monochrome tiff separately
                    var info = ExportUtils.GetCodec ("image/tiff");
                    var ep = new EncoderParameters();
                    ep.Param[0] = new EncoderParameter (Encoder.Compression, (long)MonochromeTiffCompression);

                    using (var bwImage = ConvertToBitonal (image as Bitmap))
                    {
                        bwImage.Save (stream, info, ep);
                    }
                }
                else
                {
                    var format = System.Drawing.Imaging.ImageFormat.Bmp;
                    switch (ImageFormat)
                    {
                        case ImageExportFormat.Gif:
                            format = System.Drawing.Imaging.ImageFormat.Gif;
                            break;

                        case ImageExportFormat.Png:
                            format = System.Drawing.Imaging.ImageFormat.Png;
                            break;

                        case ImageExportFormat.Tiff:
                            format = System.Drawing.Imaging.ImageFormat.Tiff;
                            break;
                    }

                    image.Save (stream, format);
                }

                if (suffix != "")
                {
                    stream.Dispose();
                }
            }

            if (image != masterTiffImage)
            {
                image.Dispose();
            }
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override string GetFileFilter()
        {
            var filter = ImageFormat.ToString();
            return Res.Get ("FileFilters," + filter + "File");
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            //init
            SeparateFiles = Stream is MemoryStream ? false : SeparateFiles;
            pageNumber = 0;
            height = 0;
            width = 0;
            image = null;
            g = null;
            zoomX = 1;
            zoomY = 1;
            state = null;

            curOriginY = 0;
            firstPage = true;

            if (!SeparateFiles && !IsMultiFrameTiff)
            {
                // create one big image. To do this, calculate max width and sum of pages height
                float w = 0;
                float h = 0;

                foreach (var pageNo in Pages)
                {
                    var size = Report.PreparedPages.GetPageSize (pageNo);
                    if (size.Width > w)
                    {
                        w = size.Width;
                    }

                    h += size.Height + PaddingNonSeparatePages * 2;
                }

                w += PaddingNonSeparatePages * 2;

                bigImage = CreateImage ((int)(w * ResolutionX / 96f), (int)(h * ResolutionY / 96f), "");
                bigGraphics = Graphics.FromImage (bigImage);
                bigGraphics.Clear (Color.Transparent);
            }

            pageNumber = 0;
        }


        /// <inheritdoc/>
        protected override void ExportPageBegin (ReportPage page)
        {
            base.ExportPageBegin (page);
            zoomX = ResolutionX / 96f;
            zoomY = ResolutionY / 96f;
            width = (int)(ExportUtils.GetPageWidth (page) * Units.Millimeters * zoomX);
            height = (int)(ExportUtils.GetPageHeight (page) * Units.Millimeters * zoomY);
            var suffixDigits = Pages[Pages.Length - 1].ToString().Length;
            fileSuffix = firstPage ? "" : (pageNumber + 1).ToString ("".PadLeft (suffixDigits, '0'));
            if (SeparateFiles || IsMultiFrameTiff)
            {
                image = CreateImage (width, height, fileSuffix);
                if (IsMultiFrameTiff && masterTiffImage == null)
                {
                    masterTiffImage = image;
                }
            }
            else
            {
                image = bigImage;
            }

            if (bigGraphics != null)
            {
                g = bigGraphics;
            }
            else
            {
                g = Graphics.FromImage (image);
            }

            state = g.Save();

            g.FillRegion (Brushes.Transparent, new Region (new RectangleF (0, curOriginY, width, height)));
            if (bigImage != null && curOriginY + height * 2 > bigImage.Height)
            {
                page.Fill.Draw (new FRPaintEventArgs (g, 1, 1, Report.GraphicCache),
                    new RectangleF (0, curOriginY, widthK, bigImage.Height - curOriginY));
            }
            else
            {
                page.Fill.Draw (new FRPaintEventArgs (g, 1, 1, Report.GraphicCache),
                    new RectangleF (0, curOriginY, widthK, height + PaddingNonSeparatePages * 2));
            }


            if (image == bigImage)
            {
                if (ImageFormat != ImageExportFormat.Metafile)
                {
                    g.TranslateTransform (image.Width / 2 - width / 2 + page.LeftMargin * Units.Millimeters * zoomX,
                        curOriginY + PaddingNonSeparatePages + page.TopMargin * Units.Millimeters * zoomY);
                }
                else
                {
                    g.TranslateTransform (widthK / 2 - width / 2 + page.LeftMargin * Units.Millimeters * zoomX,
                        curOriginY + PaddingNonSeparatePages + page.TopMargin * Units.Millimeters * zoomY);
                }
            }
            else
            {
                g.TranslateTransform (page.LeftMargin * Units.Millimeters * zoomX,
                    page.TopMargin * Units.Millimeters * zoomY);
            }

            g.ScaleTransform (1, zoomY / zoomX);

            // export bottom watermark
            if (page.Watermark is { Enabled: true, ShowImageOnTop: false })
            {
                AddImageWatermark (page);
            }

            if (page.Watermark is { Enabled: true, ShowTextOnTop: false })
            {
                AddTextWatermark (page);
            }
        }

        /// <inheritdoc/>
        protected override void ExportBand (BandBase band)
        {
            base.ExportBand (band);
            ExportObj (band);
            foreach (Base c in band.ForEachAllConvectedObjects (this))
            {
                if (!(c is Table.TableColumn || c is Table.TableCell || c is Table.TableRow))
                {
                    ExportObj (c);
                }
            }
        }

        private void ExportObj (Base obj)
        {
            if (obj is ReportComponentBase { Exportable: true } @base)
            {
                @base.Draw (new FRPaintEventArgs (g, zoomX, zoomX, Report.GraphicCache));
            }
        }

        /// <inheritdoc/>
        protected override void ExportPageEnd (ReportPage page)
        {
            // export top watermark
            if (page.Watermark is { Enabled: true, ShowImageOnTop: true })
            {
                AddImageWatermark (page);
            }

            if (page.Watermark is { Enabled: true, ShowTextOnTop: true })
            {
                AddTextWatermark (page);
            }

            g.Restore (state);
            if (g != bigGraphics)
            {
                g.Dispose();
            }

            if (SeparateFiles || IsMultiFrameTiff)
            {
                SaveImage (image, fileSuffix);
            }
            else
            {
                curOriginY += height + PaddingNonSeparatePages * 2;
            }

            firstPage = false;
            pageNumber++;
        }

        private void AddImageWatermark (ReportPage page)
        {
            page.Watermark.DrawImage (new FRPaintEventArgs (g, zoomX, zoomX, Report.GraphicCache),
                new RectangleF (-page.LeftMargin * Units.Millimeters, -page.TopMargin * Units.Millimeters,
                    width / zoomX, height / zoomY),
                page.Report, false);
        }

        private void AddTextWatermark (ReportPage page)
        {
            if (string.IsNullOrEmpty (page.Watermark.Text))
            {
                return;
            }

            page.Watermark.DrawText (new FRPaintEventArgs (g, zoomX, zoomX, Report.GraphicCache),
                new RectangleF (-page.LeftMargin * Units.Millimeters, -page.TopMargin * Units.Millimeters,
                    width / zoomX, height / zoomY),
                page.Report, false);
        }

        /// <inheritdoc/>
        protected override void Finish()
        {
            if (IsMultiFrameTiff)
            {
                // close the file.
                var ep = new EncoderParameters (1);
                ep.Param[0] = new EncoderParameter (Encoder.SaveFlag, (long)EncoderValue.Flush);
                masterTiffImage.SaveAdd (ep);
            }
            else if (!SeparateFiles)
            {
                bigGraphics.Dispose();
                bigGraphics = null;
                SaveImage (bigImage, "");
            }

            if (masterTiffImage != null)
            {
                masterTiffImage.Dispose();
                masterTiffImage = null;
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            base.Serialize (writer);
            writer.WriteValue ("ImageFormat", ImageFormat);
            writer.WriteBool ("SeparateFiles", SeparateFiles);
            writer.WriteInt ("ResolutionX", ResolutionX);
            writer.WriteInt ("ResolutionY", ResolutionY);
            writer.WriteInt ("JpegQuality", JpegQuality);
            writer.WriteBool ("MultiFrameTiff", MultiFrameTiff);
            writer.WriteBool ("MonochromeTiff", MonochromeTiff);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageExport"/> class.
        /// </summary>
        public ImageExport()
        {
            PaddingNonSeparatePages = 10;
            fileSuffix = string.Empty;
            HasMultipleFiles = true;
            ImageFormat = ImageExportFormat.Jpeg;
            SeparateFiles = true;
            Resolution = 96;
            JpegQuality = 100;
            MonochromeTiffCompression = EncoderValue.CompressionCCITT4;
        }
    }
}
