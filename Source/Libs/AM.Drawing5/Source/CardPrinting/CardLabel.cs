// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CardLabel.cs --
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
    /// Однострочный текст
    /// </summary>
    public sealed class CardLabel
        : CardItem
    {
        #region Properties

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
                        g.DrawString(text, font, brush, Left, Top);
                    }
                }
            }
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            return string.Format("Однострочный текст: {0}", Text);
        }

        #endregion
    }
}
