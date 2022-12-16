// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* FontAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Media;

using AM.Drawing.HtmlRenderer.Adapters;

#endregion

#nullable enable

namespace AM.Windows.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WPF Font.
/// </summary>
internal sealed class FontAdapter 
    : RFont
{
    #region Fields and Consts

    /// <summary>
    /// the underline win-forms font.
    /// </summary>
    private readonly Typeface _font;

    /// <summary>
    /// The glyph font for the font
    /// </summary>
    private readonly GlyphTypeface _glyphTypeface;

    /// <summary>
    /// the size of the font
    /// </summary>
    private readonly double _size;

    /// <summary>
    /// the vertical offset of the font underline location from the top of the font.
    /// </summary>
    private readonly double _underlineOffset = -1;

    /// <summary>
    /// Cached font height.
    /// </summary>
    private readonly double _height = -1;

    /// <summary>
    /// Cached font whitespace width.
    /// </summary>
    private double _whitespaceWidth = -1;

    #endregion


    /// <summary>
    /// Init.
    /// </summary>
    public FontAdapter(Typeface font, double size)
    {
        _font = font;
        _size = size;
        _height = 96d / 72d * _size * _font.FontFamily.LineSpacing;
        _underlineOffset = 96d / 72d * _size * (_font.FontFamily.LineSpacing + font.UnderlinePosition);

        GlyphTypeface typeface;
        if (font.TryGetGlyphTypeface(out typeface))
        {
            _glyphTypeface = typeface;
        }
        else
        {
            foreach (var sysTypeface in Fonts.SystemTypefaces)
            {
                if (sysTypeface.TryGetGlyphTypeface(out typeface))
                {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// the underline win-forms font.
    /// </summary>
    public Typeface Font => _font;

    public GlyphTypeface GlyphTypeface => _glyphTypeface;

    public override double Size => _size;

    public override double UnderlineOffset => _underlineOffset;

    public override double Height => _height;

    public override double LeftPadding => _height / 6f;

    public override double GetWhitespaceWidth(RGraphics graphics)
    {
        if (_whitespaceWidth < 0)
        {
            _whitespaceWidth = graphics.MeasureString(" ", this).Width;
        }
        return _whitespaceWidth;
    }
}