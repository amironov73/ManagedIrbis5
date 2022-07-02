// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* CardText.cs -- многострочный текст
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting;

/// <summary>
/// Многострочный текст.
/// </summary>
public sealed class CardText
    : CardItem
{
    #region Properties

    /// <summary>
    /// Ширина текста.
    /// </summary>
    [XmlElement ("width")]
    [DisplayName ("Ширина")]
    [JsonPropertyName ("width")]
    public int Width { get; set; }

    /// <summary>
    /// Высота текста.
    /// </summary>
    [XmlElement ("height")]
    [DisplayName ("Высота")]
    [JsonPropertyName ("height")]
    public int Height { get; set; }

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

        if (string.IsNullOrEmpty (Font))
        {
            Magna.Warning ("Font isn't specified");
        }

        if (string.IsNullOrEmpty (Color))
        {
            Magna.Warning ("Color isn't specified");
        }

        if (!string.IsNullOrEmpty (Font)
            && !string.IsNullOrEmpty (Color)
            && !string.IsNullOrEmpty (Text))
        {
            var fontConverter = new FontConverter();
            using var font = (Font)fontConverter.ConvertFromString (Font)
                .ThrowIfNull();
            var colorConverter = new ColorConverter();
            var color = (Color)colorConverter.ConvertFromString (Color)!;
            var text = context.ExpandText (Text);

            using var brush = new SolidBrush (color);
            var rectangle = new Rectangle (Left, Top, Width, Height);
            graphics.DrawString (text, font, brush, rectangle);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"Многострочный текст: {Text}";

    #endregion
}
