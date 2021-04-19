// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardPicture.cs -- картинка, например, фото читателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Text.Json.Serialization;
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

        /// <summary>
        /// Ширина.
        /// </summary>
        [XmlElement("width")]
        [DisplayName("Ширина")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        /// <summary>
        /// Высота.
        /// </summary>
        [XmlElement("height")]
        [DisplayName("Высота")]
        [JsonPropertyName("height")]
        public int Height { get; set; }

        /// <summary>
        /// Источник картинки, например, путь к файлу.
        /// </summary>
        [XmlElement("source")]
        [DisplayName("Файл")]
        [JsonPropertyName("source")]
        public string? Source { get; set; }

        #endregion

        #region Private members

        /// <summary>
        /// Пропорционально масштабирует изображение так, чтобы оно
        /// вписывалось в указанные размеры.
        /// </summary>
        private static void _ProportionalPrint
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
            double ratio = superAspect > 1.0
                ? windowWidth / imageWidth
                : windowHeight / imageHeight;
            imageWidth *= ratio;
            imageHeight *= ratio;

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
            Graphics graphics = context.Graphics.ThrowIfNull("context.Graphics");

            if (Width > 0 && Height > 0 && !string.IsNullOrEmpty(Source))
            {
                var source = context.ExpandText(Source);
                if (!string.IsNullOrEmpty(source))
                {
                    using Image bitmap = Image.FromFile(source);
                    _ProportionalPrint
                        (
                            graphics,
                            bitmap,
                            Left,
                            Top,
                            Width,
                            Height
                        );
                }
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"Картинка: {Source}";

        #endregion

    } // class CardPicture

} // namespace AM.Drawing.CardPrinting
