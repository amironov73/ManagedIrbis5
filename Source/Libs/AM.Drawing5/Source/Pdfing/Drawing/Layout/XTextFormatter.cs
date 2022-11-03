// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XTextFormatter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing.Layout;

/// <summary>
/// Represents a very simple text formatter.
/// If this class does not satisfy your needs on formatting paragraphs
/// I recommend to take a look at MigraDoc Foundation. Alternatively
/// you should copy this class in your own source code and modify it.
/// </summary>
public class XTextFormatter
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="XTextFormatter"/> class.
    /// </summary>
    public XTextFormatter
        (
            XGraphics graphics
        )
    {
        Sure.NotNull (graphics);

        _graphics = graphics;
    }

    #endregion

    private readonly XGraphics _graphics;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    public string? Text
    {
        get => _text;
        set => _text = value;
    }

    private string? _text;

    /// <summary>
    /// Gets or sets the font.
    /// </summary>
    public XFont? Font
    {
        get => _font;
        set
        {
            Sure.NotNull (value);
            _font = value!;

            _lineSpace = _font.GetHeight(); // old: _font.GetHeight(_gfx);
            _cyAscent = _lineSpace * _font.CellAscent / _font.CellSpace;
            _cyDescent = _lineSpace * _font.CellDescent / _font.CellSpace;

            // HACK in XTextFormatter
            _spaceWidth = _graphics.MeasureString ("x x", _font).Width;
            _spaceWidth -= _graphics.MeasureString ("xx", _font).Width;
        }
    }

    private XFont? _font;

    private double _lineSpace;
    private double _cyAscent;
    private double _cyDescent;
    private double _spaceWidth;

    /// <summary>
    /// Gets or sets the bounding box of the layout.
    /// </summary>
    public XRect LayoutRectangle
    {
        get => _layoutRectangle;
        set => _layoutRectangle = value;
    }

    private XRect _layoutRectangle;

    /// <summary>
    /// Gets or sets the alignment of the text.
    /// </summary>
    public XParagraphAlignment Alignment { get; set; } = XParagraphAlignment.Left;

    /// <summary>
    /// Draws the text.
    /// </summary>
    /// <param name="text">The text to be drawn.</param>
    /// <param name="font">The font.</param>
    /// <param name="brush">The text brush.</param>
    /// <param name="layoutRectangle">The layout rectangle.</param>
    public void DrawString (string text, XFont font, XBrush brush, XRect layoutRectangle)
    {
        DrawString (text, font, brush, layoutRectangle, XStringFormats.TopLeft);
    }

    /// <summary>
    /// Draws the text.
    /// </summary>
    /// <param name="text">The text to be drawn.</param>
    /// <param name="font">The font.</param>
    /// <param name="brush">The text brush.</param>
    /// <param name="layoutRectangle">The layout rectangle.</param>
    /// <param name="format">The format. Must be <c>XStringFormat.TopLeft</c></param>
    public void DrawString
        (
            string text,
            XFont font,
            XBrush brush,
            XRect layoutRectangle,
            XStringFormat format
        )
    {
        Sure.NotNull (text);
        Sure.NotNull (font);
        Sure.NotNull (brush);

        if (format.Alignment != XStringAlignment.Near || format.LineAlignment != XLineAlignment.Near)
        {
            throw new ArgumentException ("Only TopLeft alignment is currently implemented.");
        }

        Text = text;
        Font = font;
        LayoutRectangle = layoutRectangle;

        if (text.Length == 0)
        {
            return;
        }

        CreateBlocks();

        CreateLayout();

        var dx = layoutRectangle.Location.X;
        var dy = layoutRectangle.Location.Y + _cyAscent;
        var count = _blocks.Count;
        for (var idx = 0; idx < count; idx++)
        {
            var block = _blocks[idx];
            if (block.Stop)
            {
                break;
            }

            if (block.Type == BlockType.LineBreak)
            {
                continue;
            }

            _graphics.DrawString (block.Text, font, brush, dx + block.Location.X, dy + block.Location.Y);
        }
    }

    private void CreateBlocks()
    {
        _blocks.Clear();
        var length = _text.Length;
        var inNonWhiteSpace = false;
        int startIndex = 0, blockLength = 0;
        for (var idx = 0; idx < length; idx++)
        {
            var ch = _text[idx];

            // Treat CR and CRLF as LF
            if (ch == Chars.CR)
            {
                if (idx < length - 1 && _text[idx + 1] == Chars.LF)
                {
                    idx++;
                }

                ch = Chars.LF;
            }

            if (ch == Chars.LF)
            {
                if (blockLength != 0)
                {
                    var token = _text.Substring (startIndex, blockLength);
                    _blocks.Add (new Block (token, BlockType.Text,
                        _graphics.MeasureString (token, _font).Width));
                }

                startIndex = idx + 1;
                blockLength = 0;
                _blocks.Add (new Block (BlockType.LineBreak));
            }
            else if (char.IsWhiteSpace (ch))
            {
                if (inNonWhiteSpace)
                {
                    var token = _text.Substring (startIndex, blockLength);
                    _blocks.Add (new Block (token, BlockType.Text,
                        _graphics.MeasureString (token, _font).Width));
                    startIndex = idx + 1;
                    blockLength = 0;
                }
                else
                {
                    blockLength++;
                }
            }
            else
            {
                inNonWhiteSpace = true;
                blockLength++;
            }
        }

        if (blockLength != 0)
        {
            var token = _text.Substring (startIndex, blockLength);
            _blocks.Add (new Block (token, BlockType.Text,
                _graphics.MeasureString (token, _font).Width));
        }
    }

    private void CreateLayout()
    {
        var rectWidth = _layoutRectangle.Width;
        var rectHeight = _layoutRectangle.Height - _cyAscent - _cyDescent;
        var firstIndex = 0;
        double x = 0, y = 0;
        var count = _blocks.Count;
        for (var idx = 0; idx < count; idx++)
        {
            var block = _blocks[idx];
            if (block.Type == BlockType.LineBreak)
            {
                if (Alignment == XParagraphAlignment.Justify)
                {
                    _blocks[firstIndex].Alignment = XParagraphAlignment.Left;
                }

                AlignLine (firstIndex, idx - 1, rectWidth);
                firstIndex = idx + 1;
                x = 0;
                y += _lineSpace;
                if (y > rectHeight)
                {
                    block.Stop = true;
                    break;
                }
            }
            else
            {
                var width = block.Width;
                if ((x + width <= rectWidth || x == 0) && block.Type != BlockType.LineBreak)
                {
                    block.Location = new XPoint (x, y);
                    x += width + _spaceWidth;
                }
                else
                {
                    AlignLine (firstIndex, idx - 1, rectWidth);
                    firstIndex = idx;
                    y += _lineSpace;
                    if (y > rectHeight)
                    {
                        block.Stop = true;
                        break;
                    }

                    block.Location = new XPoint (0, y);
                    x = width + _spaceWidth;
                }
            }
        }

        if (firstIndex < count && Alignment != XParagraphAlignment.Justify)
        {
            AlignLine (firstIndex, count - 1, rectWidth);
        }
    }

    /// <summary>
    /// Align center, right, or justify.
    /// </summary>
    private void AlignLine (int firstIndex, int lastIndex, double layoutWidth)
    {
        var blockAlignment = _blocks[firstIndex].Alignment;
        if (Alignment == XParagraphAlignment.Left || blockAlignment == XParagraphAlignment.Left)
        {
            return;
        }

        var count = lastIndex - firstIndex + 1;
        if (count == 0)
        {
            return;
        }

        var totalWidth = -_spaceWidth;
        for (var idx = firstIndex; idx <= lastIndex; idx++)
            totalWidth += _blocks[idx].Width + _spaceWidth;

        var dx = Math.Max (layoutWidth - totalWidth, 0);

        //Debug.Assert(dx >= 0);
        if (Alignment != XParagraphAlignment.Justify)
        {
            if (Alignment == XParagraphAlignment.Center)
            {
                dx /= 2;
            }

            for (var idx = firstIndex; idx <= lastIndex; idx++)
            {
                var block = _blocks[idx];
                block.Location += new XSize (dx, 0);
            }
        }
        else if (count > 1) // case: justify
        {
            dx /= count - 1;
            for (int idx = firstIndex + 1, i = 1; idx <= lastIndex; idx++, i++)
            {
                var block = _blocks[idx];
                block.Location += new XSize (dx * i, 0);
            }
        }
    }

    private readonly List<Block> _blocks = new List<Block>();

    // TODO:
    // - more XStringFormat variations
    // - calculate bounding box
    // - left and right indent
    // - first line indent
    // - margins and paddings
    // - background color
    // - text background color
    // - border style
    // - hyphens, soft hyphens, hyphenation
    // - kerning
    // - change font, size, text color etc.
    // - line spacing
    // - underline and strike-out variation
    // - super- and sub-script
    // - ...
}
