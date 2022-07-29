// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ExportToRtf.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Drawing;
using System.Text;

using AM;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Exports colored text as RTF
/// </summary>
/// <remarks>At this time only TextStyle renderer is supported. Other styles are not exported.</remarks>
public class ExportToRtf
{
    #region Properties

    /// <summary>
    /// Нумерация строк нужна?
    /// </summary>
    public bool IncludeLineNumbers { get; set; }

    /// <summary>
    /// Use original font
    /// </summary>
    public bool UseOriginalFont { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExportToRtf()
    {
        UseOriginalFont = true;
    }

    #endregion

    #region Private members

    SyntaxTextBox? _textBox;

    private Dictionary<Color, int> colorTable = new ();

    private RtfStyleDescriptor GetRtfDescriptor
        (
            StyleIndex styleIndex
        )
    {
        var styles = new List<Style>();

        //find text renderer
        TextStyle? textStyle = null;
        var mask = 1;
        var hasTextStyle = false;
        for (var i = 0; i < _textBox!.Styles.Length; i++)
        {
            if (_textBox.Styles[i] != null && ((int)styleIndex & mask) != 0)
            {
                if (_textBox.Styles[i].IsExportable)
                {
                    var style = _textBox.Styles[i];
                    styles.Add (style);

                    var isTextStyle = style is TextStyle;
                    if (isTextStyle)
                    {
                        if (!hasTextStyle || _textBox.AllowSeveralTextStyleDrawing)
                        {
                            hasTextStyle = true;
                            textStyle = style as TextStyle;
                        }
                    }
                }
            }

            mask = mask << 1;
        }

        //add TextStyle css
        RtfStyleDescriptor? result = null;

        if (!hasTextStyle)
        {
            //draw by default renderer
            result = _textBox.DefaultStyle.GetRTF();
        }
        else
        {
            result = textStyle.GetRTF();
        }

        return result;
    }

    private int GetColorTableNumber
        (
            Color color
        )
    {
        if (color.A == 0)
        {
            return -1;
        }

        if (!colorTable.ContainsKey (color))
        {
            colorTable[color] = colorTable.Count + 1;
        }

        return colorTable[color];
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

        var desc = GetRtfDescriptor (currentStyle);
        var cf = GetColorTableNumber (desc.ForeColor);
        var cb = GetColorTableNumber (desc.BackColor);
        var tags = new StringBuilder();
        if (cf >= 0)
        {
            tags.AppendFormat (@"\cf{0}", cf);
        }

        if (cb >= 0)
        {
            tags.AppendFormat (@"\highlight{0}", cb);
        }

        if (!string.IsNullOrEmpty (desc.AdditionalTags))
        {
            tags.Append (desc.AdditionalTags.Trim());
        }

        if (tags.Length > 0)
        {
            builder.AppendFormat (@"{{{0} {1}}}", tags, temporary.ToString());
        }
        else
        {
            builder.Append (temporary.ToString());
        }

        temporary.Clear();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение RTF.
    /// </summary>
    public string GetRtf
        (
            SyntaxTextBox textBox
        )
    {
        Sure.NotNull (textBox);

        _textBox = textBox;
        var sel = new TextRange (textBox);
        sel.SelectAll();
        return GetRtf (sel);
    }

    /// <summary>
    /// Получение RTF.
    /// </summary>
    public string GetRtf
        (
            TextRange textRange
        )
    {
        Sure.NotNull (textRange);

        _textBox = textRange._textBox;
        var styles = new Dictionary<StyleIndex, object?>();
        var sb = new StringBuilder();
        var temporary = new StringBuilder();
        var currentStyleId = StyleIndex.None;
        textRange.Normalize();
        var currentLine = textRange.Start.Line;
        styles[currentStyleId] = null;
        colorTable.Clear();

        //
        var lineNumberColor = GetColorTableNumber (textRange._textBox.LineNumberColor);

        if (IncludeLineNumbers)
        {
            temporary.AppendFormat (@"{{\cf{1} {0}}}\tab", currentLine + 1, lineNumberColor);
        }

        //
        foreach (var p in textRange)
        {
            var c = textRange._textBox[p.Line][p.Column];
            if (c.style != currentStyleId)
            {
                Flush (sb, temporary, currentStyleId);
                currentStyleId = c.style;
                styles[currentStyleId] = null;
            }

            if (p.Line != currentLine)
            {
                for (var i = currentLine; i < p.Line; i++)
                {
                    temporary.AppendLine (@"\line");
                    if (IncludeLineNumbers)
                    {
                        temporary.AppendFormat (@"{{\cf{1} {0}}}\tab", i + 2, lineNumberColor);
                    }
                }

                currentLine = p.Line;
            }

            switch (c.c)
            {
                case '\\':
                    temporary.Append (@"\\");
                    break;

                case '{':
                    temporary.Append (@"\{");
                    break;

                case '}':
                    temporary.Append (@"\}");
                    break;

                default:
                    var ch = c.c;
                    var code = (int)ch;
                    if (code < 128)
                    {
                        temporary.Append (c.c);
                    }
                    else
                    {
                        temporary.AppendFormat (@"{{\u{0}}}", code);
                    }

                    break;
            }
        }

        Flush (sb, temporary, currentStyleId);

        //build color table
        var list = new SortedList<int, Color>();
        foreach (var pair in colorTable)
            list.Add (pair.Value, pair.Key);

        temporary.Length = 0;
        temporary.AppendFormat (@"{{\colortbl;");

        foreach (var pair in list)
            temporary.Append (GetColorAsString (pair.Value) + ";");
        temporary.AppendLine ("}");

        //
        if (UseOriginalFont)
        {
            sb.Insert (0, string.Format (@"{{\fonttbl{{\f0\fmodern {0};}}}}{{\fs{1} ",
                _textBox.Font.Name, (int)(2 * _textBox.Font.SizeInPoints), _textBox.CharHeight));
            sb.AppendLine (@"}");
        }

        sb.Insert (0, temporary.ToString());

        sb.Insert (0, @"{\rtf1\ud\deff0");
        sb.AppendLine (@"}");

        return sb.ToString();
    }

    /// <summary>
    /// Получение RTF-представления для цвета.
    /// </summary>
    public static string GetColorAsString
        (
            Color color
        )
    {
        return color == Color.Transparent
            ? string.Empty
            : $@"\red{color.R}\green{color.G}\blue{color.B}";
    }

    #endregion
}
