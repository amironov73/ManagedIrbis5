// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

#endregion

#nullable enable

namespace AM.Skia.TextRendering;

/// <summary>
///
/// </summary>
public enum FontStyle
{
    /// <summary>
    ///
    /// </summary>
    Regular,

    /// <summary>
    ///
    /// </summary>
    Bold,

    /// <summary>
    ///
    /// </summary>
    Italic,

    /// <summary>
    ///
    /// </summary>
    Underline,

    /// <summary>
    ///
    /// </summary>
    Strikeout
}

/// <summary>
///
/// </summary>
public class Font
{
    /// <summary>
    ///
    /// </summary>
    public SKTypeface Typeface { get; }

    /// <summary>
    ///
    /// </summary>
    public float Size { get; }

    /// <summary>
    ///
    /// </summary>
    public FontStyle Style { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="typeface"></param>
    /// <param name="size"></param>
    /// <param name="style"></param>
    public Font (SKTypeface typeface, float size, FontStyle style = FontStyle.Regular)
    {
        Typeface = typeface;
        Size = size;
        Style = style;
    }
}
