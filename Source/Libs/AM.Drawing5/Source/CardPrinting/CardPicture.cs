// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardPicture.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting
{
    /// <summary>
    /// Картинка, например, фото читателя.
    /// </summary>
    public sealed class CardPicture
        : CardItem
    {
        #region Properties

        [XmlElement("width")]
        [DisplayName("Ширина")]
        public int Width { get; set; }

        [XmlElement("height")]
        [DisplayName("Высота")]
        public int Height { get; set; }

        [XmlElement("source")]
        [DisplayName("Файл")]
        public string Source { get; set; }

        #endregion

        #region Private members

        /// <summary>
        /// Пропорционально масштабирует изображение так, чтобы оно
        /// вписывалось в указанные размеры.
        /// </summary>
        private static void _ProportionalResize
            (
                Graphics graphics,
                Image image,
                int x,
                int y,
                int width,
                int height
            )
        {
            double imageHeight = image.Height;
            double imageWidth = image.Width;
            double windowHeight = width;
            double windowWidth = height;
            double imageAspect = imageWidth / imageHeight;
            double panelAspect = windowWidth / windowHeight;
            double superAspect = imageAspect / panelAspect;
            double ratio = (superAspect > 1.0)
                ? windowWidth / imageWidth
                : windowHeight / imageHeight;
            imageWidth *= ratio;
            imageHeight *= ratio;
            //Bitmap result = new Bitmap(image, (int)imageWidth,
            //	(int)imageHeight);
            //return result;
            graphics.DrawImage
                (
                    image,
                    x,
                    y,
                    (float) imageWidth,
                    (float) imageHeight
                );
        }

        #endregion

        #region CardItem members

        public override void Draw(DrawingContext context)
        {
            /*

            Graphics g = context.Graphics;

            if ((Width > 0) && (Height > 0))
            {
                if (!string.IsNullOrEmpty(Source))
                {
                    string source = context.ExpandText(Source);
                    if (!string.IsNullOrEmpty(source))
                    {
                        using (Image bitmap = Image.FromFile(source))
                        {
                            _ProportionalResize
                            (
                                g,
                                bitmap,
                                Left,
                                Top,
                                Width,
                                Height
                            );
                        }
                    }
                }
                else if (context.Human.ActualImage != null)
                {
                    _ProportionalResize
                    (
                        g,
                        context.Human.ActualImage,
                        Left,
                        Top,
                        Width,
                        Height
                    );
                }
            }

            */
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format("Картинка: {0}", Source);
        }

        #endregion
    }
}
