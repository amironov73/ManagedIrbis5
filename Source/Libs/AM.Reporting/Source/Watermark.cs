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
using System.ComponentModel;
using System.Drawing;

using AM.Reporting.Utils;

using System.Windows.Forms;
using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Specifies the watermark image size mode.
    /// </summary>
    public enum WatermarkImageSize
    {
        /// <summary>
        /// Specifies the normal (original) size.
        /// </summary>
        Normal,

        /// <summary>
        /// Specifies the centered image.
        /// </summary>
        Center,

        /// <summary>
        /// Specifies the stretched image.
        /// </summary>
        Stretch,

        /// <summary>
        /// Specifies the stretched image that keeps its aspect ratio.
        /// </summary>
        Zoom,

        /// <summary>
        /// Specifies the tiled image.
        /// </summary>
        Tile
    }

    /// <summary>
    /// Specifies the watermark text rotation.
    /// </summary>
    public enum WatermarkTextRotation
    {
        /// <summary>
        /// Specifies a horizontal text.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Specifies a vertical text.
        /// </summary>
        Vertical,

        /// <summary>
        /// Specifies a diagonal text.
        /// </summary>
        ForwardDiagonal,

        /// <summary>
        /// Specifies a backward diagonal text.
        /// </summary>
        BackwardDiagonal
    }

    /// <summary>
    /// Represents the report page watermark.
    /// </summary>
    /// <remarks>
    /// Watermark can draw text and/or image behind the page objects on in front of them. To enable
    /// watermark, set its <b>Enabled</b> property to <b>true</b>.
    /// </remarks>
    [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
    [EditorAttribute ("AM.Reporting.TypeEditors.WatermarkEditor, AM.Reporting", typeof (UITypeEditor))]
    public class Watermark : IDisposable
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets avalue indicating that watermark is enabled.
        /// </summary>
        [DefaultValue (false)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the watermark image.
        /// </summary>
        public Image Image
        {
            get => PictureObject.Image;
            set => PictureObject.Image = value;
        }

        /// <summary>
        /// Gets or sets the watermark image size mode.
        /// </summary>
        [DefaultValue (WatermarkImageSize.Zoom)]
        public WatermarkImageSize ImageSize { get; set; }

        /// <summary>
        /// Gets or sets an image transparency.
        /// </summary>
        /// <remarks>
        /// Valid values are 0..1. 1 means totally transparent image.
        /// </remarks>
        [DefaultValue (0f)]
        public float ImageTransparency
        {
            get => PictureObject.Transparency;
            set => PictureObject.Transparency = value;
        }

        /// <summary>
        /// Gets or sets the watermark text.
        /// </summary>
        public string Text
        {
            get => TextObject.Text;
            set => TextObject.Text = value;
        }

        /// <summary>
        /// Gets or sets a font of the watermark text.
        /// </summary>
        public Font Font
        {
            get => TextObject.Font;
            set => TextObject.Font = value;
        }

        /// <summary>
        /// Gets or sets a text fill.
        /// </summary>
        [Editor ("AM.Reporting.TypeEditors.FillEditor, AM.Reporting", typeof (UITypeEditor))]
        public FillBase TextFill
        {
            get => TextObject.TextFill;
            set => TextObject.TextFill = value;
        }

        /// <summary>
        /// Gets or sets a text rotation.
        /// </summary>
        [DefaultValue (WatermarkTextRotation.ForwardDiagonal)]
        public WatermarkTextRotation TextRotation { get; set; }

        /// <summary>
        /// Gets or sets a value indicates that the text should be displayed on top of all page objects.
        /// </summary>
        [DefaultValue (true)]
        public bool ShowTextOnTop { get; set; }

        /// <summary>
        /// Gets or sets a value indicates that the image should be displayed on top of all page objects.
        /// </summary>
        [DefaultValue (false)]
        public bool ShowImageOnTop { get; set; }

        internal TextObject TextObject { get; }

        /// <summary>
        ///
        /// </summary>
        public PictureObject PictureObject { get; set; }

        #endregion

        #region Private Methods

        //private bool ShouldSerializeFont()
        //{
        //    return Font.Name != DrawUtils.DefaultReportFont.Name || Font.Size != 60 || Font.Style != FontStyle.Regular;
        //}

        private bool ShouldSerializeTextFill()
        {
            return TextFill is not SolidFill || (TextFill as SolidFill).Color != Color.LightGray;
        }

        private bool ShouldSerializeImage()
        {
            return Image != null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Draws watermark image.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="displayRect"></param>
        /// <param name="report"></param>
        /// <param name="isPrinting"></param>
        public virtual void DrawImage (FRPaintEventArgs e, RectangleF displayRect, Report report, bool isPrinting)
        {
            PictureObject.SetReport (report);
            PictureObject.Bounds = displayRect;
            var sizeMode = PictureBoxSizeMode.Normal;
            if (ImageSize == WatermarkImageSize.Stretch)
            {
                sizeMode = PictureBoxSizeMode.StretchImage;
            }
            else if (ImageSize == WatermarkImageSize.Zoom)
            {
                sizeMode = PictureBoxSizeMode.Zoom;
            }
            else if (ImageSize == WatermarkImageSize.Center)
            {
                sizeMode = PictureBoxSizeMode.CenterImage;
            }

            PictureObject.SizeMode = sizeMode;
            PictureObject.Tile = ImageSize == WatermarkImageSize.Tile;
            PictureObject.SetPrinting (isPrinting);
            PictureObject.DrawImage (e);
        }

        /// <summary>
        /// Draws watermark text.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="displayRect"></param>
        /// <param name="report"></param>
        /// <param name="isPrinting"></param>
        public void DrawText (FRPaintEventArgs e, RectangleF displayRect, Report report, bool isPrinting)
        {
            TextObject.SetReport (report);
            TextObject.Bounds = displayRect;
            var angle = 0;
            switch (TextRotation)
            {
                case WatermarkTextRotation.Horizontal:
                    angle = 0;
                    break;
                case WatermarkTextRotation.Vertical:
                    angle = 270;
                    break;
                case WatermarkTextRotation.ForwardDiagonal:
                    angle = 360 - (int)(Math.Atan (displayRect.Height / displayRect.Width) * (180 / Math.PI));
                    break;
                case WatermarkTextRotation.BackwardDiagonal:
                    angle = (int)(Math.Atan (displayRect.Height / displayRect.Width) * (180 / Math.PI));
                    break;
            }

            TextObject.Angle = angle;
            TextObject.SetPrinting (isPrinting);
            TextObject.DrawText (e);
        }

        /// <summary>
        /// Serializes the watermark.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        /// <param name="prefix">The watermark property name.</param>
        /// <param name="c">Another Watermark object to compare with.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public void Serialize (FRWriter writer, string prefix, Watermark c)
        {
            if (Enabled != c.Enabled)
            {
                writer.WriteBool (prefix + ".Enabled", Enabled);
            }

            if (!writer.AreEqual (Image, c.Image))
            {
                writer.WriteValue (prefix + ".Image", Image);
            }

            if (ImageSize != c.ImageSize)
            {
                writer.WriteValue (prefix + ".ImageSize", ImageSize);
            }

            if (ImageTransparency != c.ImageTransparency)
            {
                writer.WriteFloat (prefix + ".ImageTransparency", ImageTransparency);
            }

            if (Text != c.Text)
            {
                writer.WriteStr (prefix + ".Text", Text);
            }

            if ((writer.SerializeTo != SerializeTo.Preview || !writer.AreEqual (Font, c.Font)) &&
                writer.ItemName != "inherited")
            {
                writer.WriteValue (prefix + ".Font", Font);
            }

            TextFill.Serialize (writer, prefix + ".TextFill", c.TextFill);
            if (TextRotation != c.TextRotation)
            {
                writer.WriteValue (prefix + ".TextRotation", TextRotation);
            }

            if (ShowTextOnTop != c.ShowTextOnTop)
            {
                writer.WriteBool (prefix + ".ShowTextOnTop", ShowTextOnTop);
            }

            if (ShowImageOnTop != c.ShowImageOnTop)
            {
                writer.WriteBool (prefix + ".ShowImageOnTop", ShowImageOnTop);
            }
        }

        /// <summary>
        /// Disposes resources used by the watermark.
        /// </summary>
        public void Dispose()
        {
            PictureObject.Dispose();
            TextObject.Dispose();
        }

        /// <summary>
        /// Assigns values from another source.
        /// </summary>
        /// <param name="source">Source to assign from.</param>
        public void Assign (Watermark source)
        {
            Enabled = source.Enabled;
            Image = source.Image == null ? null : source.Image.Clone() as Image;
            ImageSize = source.ImageSize;
            ImageTransparency = source.ImageTransparency;
            Text = source.Text;
            Font = source.Font;
            TextFill = source.TextFill.Clone();
            TextRotation = source.TextRotation;
            ShowTextOnTop = source.ShowTextOnTop;
            ShowImageOnTop = source.ShowImageOnTop;
        }

        /// <summary>
        /// Creates exact copy of this <b>Watermark</b>.
        /// </summary>
        /// <returns>Copy of this watermark.</returns>
        public Watermark Clone()
        {
            var result = new Watermark();
            result.Assign (this);
            return result;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Watermark"/> class with default settings.
        /// </summary>
        public Watermark()
        {
            PictureObject = new PictureObject();
            TextObject = new TextObject();

            PictureObject.ShowErrorImage = false;
            TextObject.HorzAlign = HorzAlign.Center;
            TextObject.VertAlign = VertAlign.Center;
            ImageSize = WatermarkImageSize.Zoom;
            Font = new Font (DrawUtils.DefaultReportFont.Name, 60);
            TextFill = new SolidFill (Color.FromArgb (40, Color.Gray));
            TextRotation = WatermarkTextRotation.ForwardDiagonal;
            ShowTextOnTop = true;
        }
    }
}
