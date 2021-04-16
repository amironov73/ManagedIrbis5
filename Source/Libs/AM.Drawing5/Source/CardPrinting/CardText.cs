// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardText.cs --
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
    /// Многострочный текст.
    /// </summary>
    public sealed class CardText
        : CardItem
    {
        #region Properties

        [XmlElement("width")]
        [DisplayName("Ширина")]
        public int Width { get; set; }

        [XmlElement("height")]
        [DisplayName("Высота")]
        public int Height { get; set; }

        [XmlElement("font")]
        [DisplayName("Шрифт")]
        public string Font { get; set; }

        [XmlElement("color")]
        [DisplayName("Цвет")]
        public string Color { get; set; }

        [XmlElement("text")]
        [DisplayName("Текст")]
        public string Text { get; set; }

        #endregion

        #region CardItem members

        public override void Draw(DrawingContext context)
        {
            Graphics g = context.Graphics;

            if (!string.IsNullOrEmpty(Font)
                && !string.IsNullOrEmpty(Color)
                && !string.IsNullOrEmpty(Text))
            {
                FontConverter fontConverter = new FontConverter();
                using (Font font = (Font) fontConverter.ConvertFromString(Font))
                {
                    ColorConverter colorConverter = new ColorConverter();
                    // ReSharper disable PossibleNullReferenceException
                    Color color = (Color) colorConverter.ConvertFromString(Color);
                    // ReSharper restore PossibleNullReferenceException

                    string text = context.ExpandText(Text);

                    using (Brush brush = new SolidBrush(color))
                    {
                        Rectangle rectangle = new Rectangle(Left, Top, Width, Height);
                        g.DrawString(text, font, brush, rectangle);
                    }
                }
            }
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format("Многострочный текст: {0}", Text);
        }

        #endregion
    }
}
