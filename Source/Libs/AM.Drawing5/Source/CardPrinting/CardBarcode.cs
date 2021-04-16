// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardBarcode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;

using AM.Drawing.Barcodes;

#endregion

#nullable enable

namespace AM.Drawing.CardPrinting
{
    /// <summary>
    /// Штрих-код EAN-13
    /// </summary>
    public sealed class CardBarcode
        : CardItem
    {
        #region Properties

        [XmlElement("width")]
        [DisplayName("Ширина")]
        public int Width { get; set; }

        [XmlElement("height")]
        [DisplayName("Высота")]
        public int Height { get; set; }

        [XmlElement("text")]
        [DisplayName("Текст")]
        public String Text { get; set; }

        #endregion

        #region CardItem members

        public override void Draw
            (
                DrawingContext context
            )
        {
            var g = context.Graphics;

            if (!string.IsNullOrEmpty(Text))
            {
                var text = context.ExpandText(Text);
                if (!string.IsNullOrEmpty(text))
                {
                    var barcode = new Code39();
                    var data = new BarcodeData
                    {
                        Message = text
                    };
                    var barcodeContext = new BarcodeContext
                    {
                        Data = data,
                        Bounds = new RectangleF(Left, Top, Width, Height),
                        Graphics = g
                    };
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.SmoothingMode = SmoothingMode.None;
                    g.PixelOffsetMode = PixelOffsetMode.None;
                    barcode.DrawBarcode(barcodeContext);
                }
            }
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format("Штрих-код: {0}", Text);
        }

        #endregion
    }
}
