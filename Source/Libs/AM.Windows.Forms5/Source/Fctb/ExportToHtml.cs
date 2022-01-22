// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExportToHtml.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;
using System.Drawing;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Exports colored text as HTML
/// </summary>
/// <remarks>At this time only TextStyle renderer is supported. Other styles is not exported.</remarks>
public class ExportToHtml
{
    #region Properties

    /// <summary>
    /// CSS для нумерации строк.
    /// </summary>
    public string lineNumbersCss = "<style type=\"text/css\"> .lineNumber{font-family : monospace; font-size : small; font-style : normal; font-weight : normal; color : Teal; background-color : ThreedFace;} </style>";

    /// <summary>
    /// Use nbsp; instead space
    /// </summary>
    public bool UseNbsp { get; set; }

    /// <summary>
    /// Use nbsp; instead space in beginning of line
    /// </summary>
    public bool UseForwardNbsp { get; set; }

    /// <summary>
    /// Use original font
    /// </summary>
    public bool UseOriginalFont { get; set; }

    /// <summary>
    /// Use style tag instead style attribute
    /// </summary>
    public bool UseStyleTag { get; set; }

    /// <summary>
    /// Use 'br' tag instead of '\n'
    /// </summary>
    public bool UseBr { get; set; }

    /// <summary>
    /// Нумерация строк нужна?
    /// </summary>
    public bool IncludeLineNumbers { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExportToHtml()
    {
        UseNbsp = true;
        UseOriginalFont = true;
        UseStyleTag = true;
        UseBr = true;
    }

    #endregion

    #region Private members

    private SyntaxTextBox? _textBox;

    private string GetCss
        (
            StyleIndex styleIndex
        )
    {
        var styles = new List<Style>();

        //find text renderer
        TextStyle textStyle = null;
        var mask = 1;
        var hasTextStyle = false;
        for (var i = 0; i < _textBox.Styles.Length; i++)
        {
            if (_textBox.Styles[i] != null && ((int)styleIndex & mask) != 0)
                if (_textBox.Styles[i].IsExportable)
                {
                    var style = _textBox.Styles[i];
                    styles.Add (style);

                    var isTextStyle = style is TextStyle;
                    if (isTextStyle)
                        if (!hasTextStyle || _textBox.AllowSeveralTextStyleDrawing)
                        {
                            hasTextStyle = true;
                            textStyle = style as TextStyle;
                        }
                }

            mask = mask << 1;
        }

        //add TextStyle css
        var result = "";

        if (!hasTextStyle)
        {
            //draw by default renderer
            result = _textBox.DefaultStyle.GetCSS();
        }
        else
        {
            result = textStyle.GetCSS();
        }

        //add no TextStyle css
        foreach (var style in styles)

//            if (style != textStyle)
            if (!(style is TextStyle))
                result += style.GetCSS();

        return result;
    }

    private string GetStyleName
        (
            StyleIndex styleIndex
        )
    {
        return styleIndex.ToString().Replace (" ", "").Replace (",", "");
    }

    private void Flush
        (
            StringBuilder builder,
            StringBuilder temporary,
            StyleIndex currentStyle
        )
    {
        //find textRenderer
        if (temporary.Length == 0)
        {
            return;
        }

        if (UseStyleTag)
        {
            builder.AppendFormat ("<font class=fctb{0}>{1}</font>", GetStyleName (currentStyle), temporary);
        }
        else
        {
            var css = GetCss (currentStyle);
            if (css != "")
            {
                builder.AppendFormat ("<font style=\"{0}\">", css);
            }

            builder.Append (temporary.ToString());
            if (css != "")
            {
                builder.Append ("</font>");
            }
        }

        temporary.Clear();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение HTML из текста.
    /// </summary>
    public string GetHtml
        (
            SyntaxTextBox textBox
        )
    {
        Sure.NotNull (textBox);

        _textBox = textBox;
        var sel = new TextRange (textBox);
        sel.SelectAll();
        return GetHtml (sel);
    }

    /// <summary>
    /// Получение HTML из текста.
    /// </summary>
    public string GetHtml
        (
            TextRange textRange
        )
    {
        Sure.NotNull (textRange);

        _textBox = textRange._textBox;
        var styles = new Dictionary<StyleIndex, object>();
        var builder = new StringBuilder();
        var temporary = new StringBuilder();
        var currentStyleId = StyleIndex.None;
        textRange.Normalize();
        var currentLine = textRange.Start.Line;
        styles[currentStyleId] = null;

        //
        if (UseOriginalFont)
        {
            builder.AppendFormat ("<font style=\"font-family: {0}, monospace; font-size: {1}pt; line-height: {2}px;\">",
                textRange._textBox.Font.Name, textRange._textBox.Font.SizeInPoints, textRange._textBox.CharHeight);
        }

        //
        if (IncludeLineNumbers)
        {
            temporary.AppendFormat ("<span class=lineNumber>{0}</span>  ", currentLine + 1);
        }

        //
        var hasNonSpace = false;
        foreach (var p in textRange)
        {
            var c = textRange._textBox[p.Line][p.Column];
            if (c.style != currentStyleId)
            {
                Flush (builder, temporary, currentStyleId);
                currentStyleId = c.style;
                styles[currentStyleId] = null;
            }

            if (p.Line != currentLine)
            {
                for (var i = currentLine; i < p.Line; i++)
                {
                    temporary.Append (UseBr ? "<br>" : "\r\n");
                    if (IncludeLineNumbers)
                        temporary.AppendFormat ("<span class=lineNumber>{0}</span>  ", i + 2);
                }

                currentLine = p.Line;
                hasNonSpace = false;
            }

            switch (c.c)
            {
                case ' ':
                    if ((hasNonSpace || !UseForwardNbsp) && !UseNbsp)
                        goto default;

                    temporary.Append ("&nbsp;");
                    break;

                case '<':
                    temporary.Append ("&lt;");
                    break;

                case '>':
                    temporary.Append ("&gt;");
                    break;

                case '&':
                    temporary.Append ("&amp;");
                    break;

                default:
                    hasNonSpace = true;
                    temporary.Append (c.c);
                    break;
            }
        }

        Flush (builder, temporary, currentStyleId);

        if (UseOriginalFont)
            builder.Append ("</font>");

        //build styles
        if (UseStyleTag)
        {
            temporary.Length = 0;
            temporary.Append ("<style type=\"text/css\">");
            foreach (var styleId in styles.Keys)
                temporary.AppendFormat (".fctb{0}{{ {1} }}\r\n", GetStyleName (styleId), GetCss (styleId));
            temporary.Append ("</style>");

            builder.Insert (0, temporary.ToString());
        }

        if (IncludeLineNumbers)
        {
            builder.Insert (0, lineNumbersCss);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Цвет в формате HTML.
    /// </summary>
    public static string GetColorAsString
        (
            Color color
        )
    {
        return color == Color.Transparent
            ? string.Empty
            : $"#{color.R:x2}{color.G:x2}{color.B:x2}";
    }

    #endregion
}
