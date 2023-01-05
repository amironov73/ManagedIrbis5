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
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

using AM.Reporting.Utils;

using System.Windows.Forms;

using PaintEventArgs = AM.Reporting.Utils.PaintEventArgs;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a Picture object that can display pictures.
    /// </summary>
    /// <remarks>
    /// The Picture object can display the following kind of pictures:
    /// <list type="bullet">
    ///   <item>
    ///     <description>picture that is embedded in the report file. Use the <see cref="Image"/>
    ///     property to do this;</description>
    ///   </item>
    ///   <item>
    ///     <description>picture that is stored in the database BLOb field. Use the DataColumn
    ///     property to specify the name of data column you want to show;</description>
    ///   </item>
    ///   <item>
    ///     <description>picture that is stored in the local disk file. Use the ImageLocation
    ///     property to specify the name of the file;</description>
    ///   </item>
    ///   <item>
    ///     <description>picture that is stored in the Web. Use the ImageLocation"
    ///     property to specify the picture's URL.</description>
    ///   </item>
    /// </list>
    /// <para/>Use the SizeMode property to specify a size mode. The MaxWidth
    /// and MaxHeight properties can be used to restrict the image size if <b>SizeMode</b>
    /// is set to <b>AutoSize</b>.
    /// <para/>The <see cref="TransparentColor"/> property can be used to display an image with
    /// transparent background. Use the <see cref="Transparency"/> property if you want to display
    /// semi-transparent image.
    /// </remarks>
    public partial class PictureObject : PictureObjectBase
    {
        #region Fields

        private Image? _image;

        private int _imageIndex;

        private Color _transparentColor;
        private float _transparency;
        private byte[]? _imageData;
        private Bitmap? _grayscaleBitmap;
        private ImageFormat _imageFormat;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <remarks>
        /// By default, image that you assign to this property is never disposed - you should
        /// take care about it. If you want to dispose the image when this <b>PictureObject</b> is disposed,
        /// set the <see cref="ShouldDisposeImage"/> property to <b>true</b> right after you assign an image:
        /// <code>
        /// myPictureObject.Image = new Bitmap("file.bmp");
        /// myPictureObject.ShouldDisposeImage = true;
        /// </code>
        /// </remarks>
        [Category ("Data")]
        public virtual Image? Image
        {
            get => _image;
            set
            {
                _image = value;
                _imageData = null;
                UpdateAutoSize();
                UpdateTransparentImage();
                ResetImageIndex();
                _imageFormat = CheckImageFormat();
                ShouldDisposeImage = false;
            }
        }

        /// <summary>
        /// Gets or sets the extension of image.
        /// </summary>
        [Category ("Data")]
        public virtual ImageFormat ImageFormat
        {
            get => _imageFormat;
            set
            {
                if (_image == null)
                {
                    return;
                }

                var wasC = false;
                using (var stream = new MemoryStream())
                {
                    wasC = ImageHelper.SaveAndConvert (Image!, stream, value);
                    _imageData = stream.ToArray();
                }

                if (!wasC)
                {
                    return;
                }

                ForceLoadImage();
                _imageFormat = CheckImageFormat();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating that the image should be displayed in grayscale mode.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Appearance")]
        public override bool Grayscale
        {
            get => base.Grayscale;
            set
            {
                base.Grayscale = value;
                if (!value && _grayscaleBitmap != null)
                {
                    _grayscaleBitmap.Dispose();
                    _grayscaleBitmap = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets a hash of grayscale svg image
        /// </summary>
        [Browsable (false)]
        public int GrayscaleHash { get; set; }


        /// <summary>
        /// Gets or sets the color of the image that will be treated as transparent.
        /// </summary>
        [Category ("Appearance")]
        public Color TransparentColor
        {
            get => _transparentColor;
            set
            {
                _transparentColor = value;
                UpdateTransparentImage();
            }
        }

        /// <summary>
        /// Gets or sets the transparency of the PictureObject.
        /// </summary>
        /// <remarks>
        /// Valid range of values is 0..1. Default value is 0.
        /// </remarks>
        [DefaultValue (0f)]
        [Category ("Appearance")]
        public float Transparency
        {
            get => _transparency;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > 1)
                {
                    value = 1;
                }

                _transparency = value;
                UpdateTransparentImage();
            }
        }


        /// <summary>
        /// Gets or sets a value indicating that the image should be tiled.
        /// </summary>
        [DefaultValue (false)]
        [Category ("Appearance")]
        public bool Tile { get; set; }


        /// <summary>
        /// Gets or sets a value indicating that the image stored in the <see cref="Image"/>
        /// property should be disposed when this object is disposed.
        /// </summary>
        /// <remarks>
        /// By default, image assigned to the <see cref="Image"/> property is never disposed - you should
        /// take care about it. If you want to dispose the image when this <b>PictureObject</b> is disposed,
        /// set this property to <b>true</b> right after you assign an image to the <see cref="Image"/> property.
        /// </remarks>
        [Browsable (false)]
        public bool ShouldDisposeImage { get; set; }


        /// <summary>
        /// Gets or sets a bitmap transparent image
        /// </summary>
        [Browsable (false)]
        public Bitmap TransparentImage { get; set; }

        /// <inheritdoc/>
        [Browsable (false)]
        protected override float ImageWidth
        {
            get
            {
                if (Image == null)
                {
                    return 0;
                }

                return Image.Width;
            }
        }

        /// <inheritdoc/>
        [Browsable (false)]
        protected override float ImageHeight
        {
            get
            {
                if (Image == null)
                {
                    return 0;
                }

                return Image.Height;
            }
        }

        #endregion

        #region Private Methods

        private ImageFormat CheckImageFormat()
        {
            if (Image == null || Image.RawFormat == null)
            {
                return null;
            }

            ImageFormat format = null;
            if (ImageFormat.Jpeg.Equals (_image.RawFormat))
            {
                format = ImageFormat.Jpeg;
            }
            else if (ImageFormat.Gif.Equals (_image.RawFormat))
            {
                format = ImageFormat.Gif;
            }
            else if (ImageFormat.Png.Equals (_image.RawFormat))
            {
                format = ImageFormat.Png;
            }
            else if (ImageFormat.Emf.Equals (_image.RawFormat))
            {
                format = ImageFormat.Emf;
            }
            else if (ImageFormat.Icon.Equals (_image.RawFormat))
            {
                format = ImageFormat.Icon;
            }
            else if (ImageFormat.Tiff.Equals (_image.RawFormat))
            {
                format = ImageFormat.Tiff;
            }
            else if (ImageFormat.Bmp.Equals (_image.RawFormat) || ImageFormat.MemoryBmp.Equals (_image.RawFormat))
            {
                format = ImageFormat.Bmp;
            }
            else if (ImageFormat.Wmf.Equals (_image.RawFormat))
            {
                format = ImageFormat.Wmf;
            }

            if (format != null)
            {
                return format;
            }

            return ImageFormat.Bmp;
        }

        private void UpdateTransparentImage()
        {
            if (TransparentImage != null)
            {
                TransparentImage.Dispose();
            }

            TransparentImage = null;
            if (Image is Bitmap)
            {
                if (TransparentColor != Color.Transparent)
                {
                    TransparentImage = new Bitmap (Image);
                    TransparentImage.MakeTransparent (TransparentColor);
                }
                else if (Transparency != 0)
                {
                    TransparentImage = ImageHelper.GetTransparentBitmap (Image, Transparency);
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                DisposeImage();
            }

            base.Dispose (disposing);
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            if (source is PictureObject src)
            {
                TransparentColor = src.TransparentColor;
                Transparency = src.Transparency;
                Tile = src.Tile;
                Image = src.Image == null ? null : src.Image.Clone() as Image;
                if (src.Image == null && src._imageData != null)
                {
                    _imageData = src._imageData;
                }

                ShouldDisposeImage = true;
                ImageFormat = src.ImageFormat;
            }
        }

        /// <summary>
        /// Draws the image.
        /// </summary>
        /// <param name="e">Paint event args.</param>
        public override void DrawImage (PaintEventArgs e)
        {
            var g = e.Graphics;
            if (Image == null)
            {
                ForceLoadImage();
            }

            if (Image == null)
            {
                DrawErrorImage (g, e);
                return;
            }

            var drawLeft = (AbsLeft + Padding.Left) * e.ScaleX;
            var drawTop = (AbsTop + Padding.Top) * e.ScaleY;
            var drawWidth = (Width - Padding.Horizontal) * e.ScaleX;
            var drawHeight = (Height - Padding.Vertical) * e.ScaleY;

            var drawRect = new RectangleF (
                drawLeft,
                drawTop,
                drawWidth,
                drawHeight);

            var state = g.Save();
            try
            {
                //if (Config.IsRunningOnMono) // strange behavior of mono - we need to reset clip before we set new one
                g.ResetClip();
                g.SetClip (drawRect);
                var report = Report;
                if (report is { SmoothGraphics: true })
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                }

                if (!Tile)
                {
                    DrawImageInternal (e, drawRect);
                }
                else
                {
                    var y = drawRect.Top;
                    var width = Image.Width * e.ScaleX;
                    var height = Image.Height * e.ScaleY;
                    while (y < drawRect.Bottom)
                    {
                        var x = drawRect.Left;
                        while (x < drawRect.Right)
                        {
                            if (TransparentImage != null)
                            {
                                g.DrawImage (TransparentImage, x, y, width, height);
                            }
                            else
                            {
                                g.DrawImage (Image, x, y, width, height);
                            }

                            x += width;
                        }

                        y += height;
                    }
                }
            }
            finally
            {
                g.Restore (state);
            }

            if (IsPrinting)
            {
                DisposeImage();
            }
        }

        protected override void DrawImageInternal2 (IGraphics graphics, PointF upperLeft, PointF upperRight,
            PointF lowerLeft)
        {
            var image = TransparentImage != null ? TransparentImage.Clone() as Image : Image.Clone() as Image;
            if (image == null)
            {
                return;
            }

            if (Grayscale)
            {
                if (GrayscaleHash != image.GetHashCode() || _grayscaleBitmap == null)
                {
                    if (_grayscaleBitmap != null)
                    {
                        _grayscaleBitmap.Dispose();
                    }

                    _grayscaleBitmap = ImageHelper.GetGrayscaleBitmap (image);
                    GrayscaleHash = image.GetHashCode();
                }

                image = _grayscaleBitmap;
            }

            //graphics.DrawImage(image, new PointF[] { upperLeft, upperRight, lowerLeft });

            DrawImage3Points (graphics, image, upperLeft, upperRight, lowerLeft);
            image.Dispose();
        }

        // This is analogue of graphics.DrawImage(image, PointF[] points) method.
        // The original gdi+ method does not work properly in mono on linux/macos.
        private void DrawImage3Points (IGraphics g, Image image, PointF p0, PointF p1, PointF p2)
        {
            // Skip drawing image, when height or width of the image equal zero.
            if (image == null || image.Width == 0 || image.Height == 0)
            {
                return;
            }

            // Skip drawing image, when height or width of the parallelogram for drawing equal zero.
            if (p0 == p1 || p0 == p2)
            {
                return;
            }

            var rect = new RectangleF (0, 0, image.Width, image.Height);
            var m11 = (p1.X - p0.X) / rect.Width;
            var m12 = (p1.Y - p0.Y) / rect.Width;
            var m21 = (p2.X - p0.X) / rect.Height;
            var m22 = (p2.Y - p0.Y) / rect.Height;
            g.MultiplyTransform (new System.Drawing.Drawing2D.Matrix (m11, m12, m21, m22, p0.X, p0.Y),
                MatrixOrder.Prepend);
            g.DrawImage (image, rect);
        }

        /// <summary>
        /// Sets image data to FImageData
        /// </summary>
        /// <param name="data"></param>
        public void SetImageData (byte[] data)
        {
            _imageData = data;

            // if autosize is on, load the image.
            if (SizeMode == PictureBoxSizeMode.AutoSize)
            {
                ForceLoadImage();
            }
        }

        /// <inheritdoc/>
        public override void Serialize (ReportWriter writer)
        {
            var c = writer.DiffObject as PictureObject;
            base.Serialize (writer);

#if PRINT_HOUSE
      writer.WriteStr("ImageLocation", ImageLocation);
#endif
            if (TransparentColor != c.TransparentColor)
            {
                writer.WriteValue ("TransparentColor", TransparentColor);
            }

            if (FloatDiff (Transparency, c.Transparency))
            {
                writer.WriteFloat ("Transparency", Transparency);
            }

            if (Tile != c.Tile)
            {
                writer.WriteBool ("Tile", Tile);
            }

            if (ImageFormat != c.ImageFormat)
            {
                writer.WriteValue ("ImageFormat", ImageFormat);
            }

            // store image data
            if (writer.SerializeTo != SerializeTo.SourcePages)
            {
                if (writer.SerializeTo == SerializeTo.Preview ||
                    (string.IsNullOrEmpty (ImageLocation) && string.IsNullOrEmpty (DataColumn)))
                {
                    if (writer.BlobStore != null)
                    {
                        // check FImageIndex >= writer.BlobStore.Count is needed when we close the designer
                        // and run it again, the BlobStore is empty, but FImageIndex is pointing to
                        // previous BlobStore item and is not -1.
                        if (_imageIndex == -1 || _imageIndex >= writer.BlobStore.Count)
                        {
                            var bytes = _imageData;
                            if (bytes == null)
                            {
                                using (var stream = new MemoryStream())
                                {
                                    ImageHelper.Save (Image, stream, _imageFormat);
                                    bytes = stream.ToArray();
                                }
                            }

                            if (bytes != null)
                            {
                                var imgHash = BitConverter.ToString (new Murmur3().ComputeHash (bytes));
                                _imageIndex = writer.BlobStore.AddOrUpdate (bytes, imgHash);
                            }
                        }
                    }
                    else
                    {
                        if (Image == null && _imageData != null)
                        {
                            writer.WriteStr ("Image", Convert.ToBase64String (_imageData));
                        }
                        else if (!writer.AreEqual (Image, c.Image))
                        {
                            writer.WriteValue ("Image", Image);
                        }
                    }

                    if (writer.BlobStore != null || writer.SerializeTo == SerializeTo.Undo)
                    {
                        writer.WriteInt ("ImageIndex", _imageIndex);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void Deserialize (ReportReader reader)
        {
            base.Deserialize (reader);
            if (reader.HasProperty ("ImageIndex"))
            {
                _imageIndex = reader.ReadInt ("ImageIndex");
                if (reader.BlobStore != null && _imageIndex != -1)
                {
                    //int saveIndex = FImageIndex;
                    //Image = ImageHelper.Load(reader.BlobStore.Get(FImageIndex));
                    //FImageIndex = saveIndex;
                    SetImageData (reader.BlobStore.Get (_imageIndex));
                }
            }
        }


        //static int number = 0;


        /// <summary>
        /// Loads image
        /// </summary>
        public override void LoadImage()
        {
            if (!string.IsNullOrEmpty (ImageLocation))
            {
                //
                try
                {
                    var uri = CalculateUri();
                    if (uri.IsFile)
                    {
                        SetImageData (ImageHelper.Load (uri.LocalPath));
                    }
                    else
                    {
                        SetImageData (ImageHelper.LoadURL (uri.ToString()));
                    }
                }
                catch
                {
                    Image = null;
                }

                ShouldDisposeImage = true;
            }
        }

        /// <summary>
        /// Disposes image
        /// </summary>
        public void DisposeImage()
        {
            if (Image != null && ShouldDisposeImage)
            {
                Image.Dispose();
            }

            Image = null;
        }

        protected override void ResetImageIndex()
        {
            _imageIndex = -1;
        }

        #endregion

        #region Report Engine

        /// <inheritdoc/>
        public override void InitializeComponent()
        {
            base.InitializeComponent();
            ResetImageIndex();
        }

        /// <inheritdoc/>
        public override void FinalizeComponent()
        {
            base.FinalizeComponent();
            ResetImageIndex();
        }


        /// <inheritdoc/>
        public override void GetData()
        {
            base.GetData();
            if (!string.IsNullOrEmpty (DataColumn))
            {
                // reset the image
                Image = null;
                _imageData = null;

                var data = Report.GetColumnValueNullable (DataColumn);
                if (data is byte[] bytes)
                {
                    SetImageData (bytes);
                }
                else if (data is Image data1)
                {
                    Image = data1;
                }
                else if (data is string)
                {
                    ImageLocation = data.ToString();
                }
            }
        }

        /// <summary>
        /// Forces loading the image from a data column.
        /// </summary>
        /// <remarks>
        /// Call this method in the <b>AfterData</b> event handler to force loading an image
        /// into the <see cref="Image"/> property. Normally, the image is stored internally as byte[] array
        /// and never loaded into the <b>Image</b> property, to save the time. The side effect is that you
        /// can't analyze the image properties such as width and height. If you need this, call this method
        /// before you access the <b>Image</b> property. Note that this will significantly slow down the report.
        /// </remarks>
        public void ForceLoadImage()
        {
            if (_imageData == null)
            {
                return;
            }

            var saveImageData = _imageData;

            // FImageData will be reset after this line, keep it
            Image = ImageHelper.Load (_imageData);
            _imageData = saveImageData;
            ShouldDisposeImage = true;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PictureObject"/> class with default settings.
        /// </summary>
        public PictureObject()
        {
            _transparentColor = Color.Transparent;
            SetFlags (Flags.HasSmartTag, true);
            ResetImageIndex();
        }
    }
}
