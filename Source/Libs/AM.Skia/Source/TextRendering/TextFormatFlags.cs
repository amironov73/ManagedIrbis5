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

using System;

#endregion

#nullable enable

namespace AM.Skia.TextRendering;

/// <summary>
///
/// </summary>
[Flags]
public enum TextFormatFlags
{
    // Applies the default formatting, which is left-aligned.
    /// <summary>
    ///
    /// </summary>
    Default = 0,

    // Adds padding to the bounding rectangle to accommodate overhanging glyphs.
    /// <summary>
    ///
    /// </summary>
    GlyphOverhangPadding = 0,

    // Aligns the text on the left side of the clipping area.
    /// <summary>
    ///
    /// </summary>
    Left = 0,

    // Aligns the text on the top of the bounding rectangle.
    /// <summary>
    ///
    /// </summary>
    Top = 0,

    // Centers the text horizontally within the bounding rectangle.
    /// <summary>
    ///
    /// </summary>
    HorizontalCenter = 1,

    // Aligns the text on the right side of the clipping area.
    /// <summary>
    ///
    /// </summary>
    Right = 2,

    // Centers the text vertically, within the bounding rectangle.
    /// <summary>
    ///
    /// </summary>
    VerticalCenter = 4,

    // Aligns the text on the bottom of the bounding rectangle. Applied only when the text is a single line.
    /// <summary>
    ///
    /// </summary>
    Bottom = 8,

    // Breaks the text at the end of a word.
    /// <summary>
    ///
    /// </summary>
    WordBreak = 16,

    // Displays the text in a single line.
    /// <summary>
    ///
    /// </summary>
    SingleLine = 32,

    // Allows the overhanging parts of glyphs and unwrapped text reaching outside the formatting rectangle to show.
    /// <summary>
    ///
    /// </summary>
    NoClipping = 256,

    // Includes the font external leading in line height. Typically, external leading is not included in the height of a line of text.
    /// <summary>
    ///
    /// </summary>
    ExternalLeading = 512,

    // Trims the line to the nearest word and an ellipsis is placed at the end of a trimmed line.
    /// <summary>
    ///
    /// </summary>
    WordEllipsis = 262144,

    // Does not add padding to the bounding rectangle.
    /// <summary>
    ///
    /// </summary>
    NoPadding = 268435456,

    // Adds padding to both sides of the bounding rectangle.
    /// <summary>
    ///
    /// </summary>
    LeftAndRightPadding = 536870912
}
