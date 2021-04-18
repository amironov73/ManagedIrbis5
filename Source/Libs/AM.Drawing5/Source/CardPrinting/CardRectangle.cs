// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardRectangle.cs --
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
    /// Простой прямоугольник (с границей или без).
    /// Заливка может быть прозрачной.
    /// </summary>
    public sealed class CardRectangle
        : CardItem
    {
        #region Properties

        [XmlElement("width")]
        [DisplayName("Ширина")]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [XmlElement("height")]
        [DisplayName("Высота")]
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [XmlElement("fill")]
        [DisplayName("Цвет заливки")]
        [JsonPropertyName("fill")]
        public string? FillColor { get; set; }

        [XmlElement("border")]
        [DisplayName("Цвет границы")]
        [JsonPropertyName("border")]
        public string? BorderColor { get; set; }

        [XmlElement("thickness")]
        [DisplayName("Толщина границы")]
        [JsonPropertyName("thickness")]
        public int Thickness { get; set; }

        #endregion

        #region CardItem members

        public override void Draw
            (
                DrawingContext context
            )
        {
            var converter = new ColorConverter();

            var graphics = context.Graphics;
            if (!string.IsNullOrEmpty(FillColor))
            {
                var fillColor = (Color) converter.ConvertFromString(FillColor).ThrowIfNull("Color");
                using Brush brush = new SolidBrush(fillColor);
                graphics.FillRectangle(brush, Left, Top, Width, Height);
            }

            if (!string.IsNullOrEmpty(BorderColor) && Thickness > 0)
            {
                var borderColor = (Color) converter.ConvertFromString(BorderColor).ThrowIfNull("Border");
                using var pen = new Pen(borderColor, Thickness);
                graphics.DrawRectangle(pen, Left, Top, Width, Height);
            }
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => "Прямоугольник";

        #endregion
    }
}
