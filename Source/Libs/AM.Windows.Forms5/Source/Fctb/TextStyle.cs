// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TextSytle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Style for chars rendering
/// This renderer can draws chars, with defined fore and back colors
/// </summary>
public class TextStyle : Style
{
    public Brush ForeBrush { get; set; }
    public Brush BackgroundBrush { get; set; }

    public FontStyle FontStyle { get; set; }

    //public readonly Font Font;
    public StringFormat stringFormat;

    public TextStyle (Brush foreBrush, Brush backgroundBrush, FontStyle fontStyle)
    {
        this.ForeBrush = foreBrush;
        this.BackgroundBrush = backgroundBrush;
        this.FontStyle = fontStyle;
        stringFormat = new StringFormat (StringFormatFlags.MeasureTrailingSpaces);
    }

    public override void Draw (Graphics graphics, Point position, TextRange range)
    {
        //draw background
        if (BackgroundBrush != null)
            graphics.FillRectangle (BackgroundBrush, position.X, position.Y,
                (range.End.Column - range.Start.Column) * range.tb.CharWidth, range.tb.CharHeight);

        //draw chars
        using (var f = new Font (range.tb.Font, FontStyle))
        {
            var line = range.tb[range.Start.Line];
            float dx = range.tb.CharWidth;
            float y = position.Y + range.tb.LineInterval / 2;
            float x = position.X - range.tb.CharWidth / 3;

            if (ForeBrush == null)
                ForeBrush = new SolidBrush (range.tb.ForeColor);

            if (range.tb.ImeAllowed)
            {
                //IME mode
                for (var i = range.Start.Column; i < range.End.Column; i++)
                {
                    var size = SyntaxTextBox.GetCharSize (f, line[i].c);

                    var gs = graphics.Save();
                    var k = size.Width > range.tb.CharWidth + 1 ? range.tb.CharWidth / size.Width : 1;
                    graphics.TranslateTransform (x, y + (1 - k) * range.tb.CharHeight / 2);
                    graphics.ScaleTransform (k, (float)Math.Sqrt (k));
                    graphics.DrawString (line[i].c.ToString(), f, ForeBrush, 0, 0, stringFormat);
                    graphics.Restore (gs);
                    x += dx;
                }
            }
            else
            {
                //classic mode
                for (var i = range.Start.Column; i < range.End.Column; i++)
                {
                    //draw char
                    graphics.DrawString (line[i].c.ToString(), f, ForeBrush, x, y, stringFormat);
                    x += dx;
                }
            }
        }
    }

    public override string GetCSS()
    {
        var result = "";

        if (BackgroundBrush is SolidBrush)
        {
            var s = ExportToHTML.GetColorAsString ((BackgroundBrush as SolidBrush).Color);
            if (s != "")
                result += "background-color:" + s + ";";
        }

        if (ForeBrush is SolidBrush)
        {
            var s = ExportToHTML.GetColorAsString ((ForeBrush as SolidBrush).Color);
            if (s != "")
                result += "color:" + s + ";";
        }

        if ((FontStyle & FontStyle.Bold) != 0)
            result += "font-weight:bold;";
        if ((FontStyle & FontStyle.Italic) != 0)
            result += "font-style:oblique;";
        if ((FontStyle & FontStyle.Strikeout) != 0)
            result += "text-decoration:line-through;";
        if ((FontStyle & FontStyle.Underline) != 0)
            result += "text-decoration:underline;";

        return result;
    }

    public override RTFStyleDescriptor GetRTF()
    {
        var result = new RTFStyleDescriptor();

        if (BackgroundBrush is SolidBrush)
            result.BackColor = (BackgroundBrush as SolidBrush).Color;

        if (ForeBrush is SolidBrush)
            result.ForeColor = (ForeBrush as SolidBrush).Color;

        if ((FontStyle & FontStyle.Bold) != 0)
            result.AdditionalTags += @"\b";
        if ((FontStyle & FontStyle.Italic) != 0)
            result.AdditionalTags += @"\i";
        if ((FontStyle & FontStyle.Strikeout) != 0)
            result.AdditionalTags += @"\strike";
        if ((FontStyle & FontStyle.Underline) != 0)
            result.AdditionalTags += @"\ul";

        return result;
    }
}
