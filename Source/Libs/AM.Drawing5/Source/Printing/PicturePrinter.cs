// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PicturePrinter.cs -- простая распечатка изображений.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

#endregion

#nullable enable

namespace AM.Drawing.Printing
{
    /// <summary>
    /// Простая распечатка изображений.
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class PicturePrinter
        : Component
    {
        #region Properties

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        public PrintDocument? Document { get; set; }

        /// <summary>
        /// Gets or sets the picture.
        /// </summary>
        public Image? Image { get; set; }

        /// <summary>
        /// Gets or sets the image scale.
        /// </summary>
        [DefaultValue(1f)]
        public float ImageScale { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the image position.
        /// </summary>
        public ImagePosition ImagePosition { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string? Title { get; set; }

        #endregion

        #region Private members

        private void _Print(bool preview)
        {
            Document ??= new PrintDocument();

            if (preview)
            {
                Document.PrintController = new PreviewPrintController();
            }

            Document.DocumentName = Title ?? "Unnamed document";
            Document.PrintPage += _PrintPage;
            Document.Print();
        }

        private void _PrintPage
            (
                object sender,
                PrintPageEventArgs args
            )
        {
            var image = Image;
            if (image is null)
            {
                return;
            }

            var scale = ImageScale;
            var boundLeft = args.MarginBounds.Left / 100f;
            var boundTop = args.MarginBounds.Top / 100f;
            var boundWidth = args.MarginBounds.Width / 100f;
            var boundHeight = args.MarginBounds.Height / 100f;
            var imageWidth = image.Width / image.HorizontalResolution;
            var imageHeight = image.Height / image.VerticalResolution;
            if (scale <= 0f)
            {
                scale = Math.Min
                    (
                        boundWidth / imageWidth,
                        boundHeight / imageHeight
                    );
            }
            var location = new PointF(boundLeft, boundTop);
            var size = new SizeF
                (
                    imageWidth * scale,
                    imageHeight * scale
                );
            switch (ImagePosition)
            {
                case ImagePosition.PageCenter:
                    location.X += (boundWidth - size.Width) / 2;
                    location.Y += (boundHeight - size.Height) / 2;
                    break;

                case ImagePosition.TopLeftCorner:
                    break;

                default:
                    throw new ApplicationException();
            }

            var graphics = args.Graphics.ThrowIfNull("args.Graphics");
            graphics.PageUnit = GraphicsUnit.Inch;
            graphics.DrawImage
                (
                    image,
                    new RectangleF(location, size)
                );
            args.HasMorePages = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Previews the specified <see cref="P:Image"/>.
        /// </summary>
        public void Preview()
        {
            _Print(true);
        }

        /// <summary>
        /// Prints the specified <see cref="P:Image"/>.
        /// </summary>
        public void Print()
        {
            _Print(false);
        }

        #endregion

    } // class PicturePrinter

} // namespace AM.Drawing.Printing
