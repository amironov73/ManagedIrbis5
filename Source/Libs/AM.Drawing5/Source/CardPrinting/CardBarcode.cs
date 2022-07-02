// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* CardBarcode.cs -- штрих-код на читательском билете
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM.Drawing.Barcodes;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting;

/// <summary>
/// Штрих-код на читательском билете.
/// </summary>
public sealed class CardBarcode
    : CardItem
{
    #region Properties

    /// <summary>
    /// Ширина штрих-кода.
    /// </summary>
    [XmlElement ("width")]
    [DisplayName ("Ширина")]
    [JsonPropertyName ("width")]
    [Description ("Ширина штрих-кода")]
    public int Width { get; set; }

    /// <summary>
    /// Высота штрих-кода.
    /// </summary>
    [XmlElement ("height")]
    [DisplayName ("Высота")]
    [JsonPropertyName ("height")]
    [Description ("Высота штрих-кода")]
    public int Height { get; set; }

    /// <summary>
    /// Текст штрих-кода.
    /// </summary>
    [XmlElement ("text")]
    [DisplayName ("Текст")]
    [JsonPropertyName ("text")]
    [Description ("Текстовое представление штрих-кода")]
    public string? Text { get; set; }

    #endregion

    #region CardItem members

    /// <inheritdoc cref="CardItem.Draw"/>
    public override void Draw
        (
            DrawingContext context
        )
    {
        Sure.NotNull (context);

        var graphics = context.Graphics.ThrowIfNull();

        if (!string.IsNullOrEmpty (Text))
        {
            var text = context.ExpandText (Text);
            if (!string.IsNullOrEmpty (text))
            {
                var barcode = new Code39();
                var data = new BarcodeData
                {
                    Message = text
                };
                var barcodeContext = new BarcodeContext
                {
                    Data = data,
                    Bounds = new RectangleF (Left, Top, Width, Height),
                    Graphics = graphics
                };
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.None;
                barcode.DrawBarcode (barcodeContext);
            }
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"Штрих-код: {Text}";

    #endregion
}
