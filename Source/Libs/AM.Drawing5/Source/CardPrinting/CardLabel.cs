// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardLabel.cs -- однострочный текст
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
    /// Однострочный текст.
    /// </summary>
    public sealed class CardLabel
        : CardItem
    {
        #region Properties

        /// <summary>
        /// Шрифт.
        /// </summary>
        [XmlElement ("font")]
        [DisplayName ("Шрифт")]
        [JsonPropertyName ("font")]
        public string? Font { get; set; }

        /// <summary>
        /// Цвет текста.
        /// </summary>
        [XmlElement ("color")]
        [DisplayName ("Цвет")]
        [JsonPropertyName ("color")]
        public string? Color { get; set; }

        /// <summary>
        /// Собственно текст.
        /// </summary>
        [XmlElement ("text")]
        [DisplayName ("Текст")]
        [JsonPropertyName ("text")]
        public string? Text { get; set; }

        #endregion

        #region CardItem members

        /// <inheritdoc cref="CardItem.Draw"/>
        public override void Draw
            (
                DrawingContext context
            )
        {
            var graphics = context.Graphics.ThrowIfNull();

            if (!string.IsNullOrEmpty (Font)
                && !string.IsNullOrEmpty (Color)
                && !string.IsNullOrEmpty (Text))
            {
                var fontConverter = new FontConverter();
                using var font = (Font) fontConverter.ConvertFromString (Font).ThrowIfNull();
                var colorConverter = new ColorConverter();
                var color = (Color) colorConverter.ConvertFromString (Color).ThrowIfNull();

                var text = context.ExpandText (Text);
                if (!string.IsNullOrEmpty (text))
                {
                    using var brush = new SolidBrush (color);
                    graphics.DrawString (text, font, brush, Left, Top);
                }

            } // if

        } // method Draw

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"Однострочный текст: {Text}";

        #endregion

    } // class CardLabel

} // namespace AM.Drawing.CardPrinting
