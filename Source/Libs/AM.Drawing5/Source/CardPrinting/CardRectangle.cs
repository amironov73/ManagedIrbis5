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
        public int Width { get; set; }

        [XmlElement("height")]
        [DisplayName("Высота")]
        public int Height { get; set; }

        [XmlElement("fill")]
        [DisplayName("Цвет заливки")]
        public string FillColor { get; set; }

        [XmlElement("border")]
        [DisplayName("Цвет границы")]
        public string BorderColor { get; set; }

        [XmlElement("thickness")]
        [DisplayName("Толщина границы")]
        public int Thickness { get; set; }

        #endregion

        #region CardItem members

        public override void Draw(DrawingContext context)
        {
            ColorConverter converter = new ColorConverter();

            Graphics g = context.Graphics;
            if (!string.IsNullOrEmpty(FillColor))
            {
                // ReSharper disable PossibleNullReferenceException
                Color fillColor = (Color) converter
                    .ConvertFromString(FillColor);
                // ReSharper restore PossibleNullReferenceException
                using (Brush brush = new SolidBrush(fillColor))
                {
                    g.FillRectangle(brush, Left, Top, Width, Height);
                }
            }

            if (!string.IsNullOrEmpty(BorderColor)
                && (Thickness > 0))
            {
                // ReSharper disable PossibleNullReferenceException
                Color borderColor = (Color) converter
                    .ConvertFromString(BorderColor);
                // ReSharper restore PossibleNullReferenceException
                using (Pen pen = new Pen(borderColor, Thickness))
                {
                    g.DrawRectangle(pen, Left, Top, Width, Height);
                }
            }
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return "Прямоугольник";
        }

        #endregion
    }
}
