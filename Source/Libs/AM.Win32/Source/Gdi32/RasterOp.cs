﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RasterOp.cs -- some common raster operation codes
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Some common raster operation codes.
/// </summary>
[Flags]
public enum RasterOp
{
    /// <summary>
    /// Fills the destination rectangle using the color associated
    /// with index 0 in the physical palette. (This color is black
    /// for the default physical palette.)
    /// </summary>
    BLACKNESS = 0x00000042,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: Includes any windows that are
    /// layered on top of your window in the resulting image. By default,
    /// the image only contains your window. Note that this generally
    /// cannot be used for printing device contexts.
    /// </summary>
    CAPTUREBLT = 0x40000000,

    /// <summary>
    /// Inverts the destination rectangle.
    /// </summary>
    DSTINVERT = 0x00550009,

    /// <summary>
    /// Merges the colors of the source rectangle with the brush currently
    /// selected in hdcDest, by using the Boolean AND operator.
    /// </summary>
    MERGECOPY = 0x00C000CA,

    /// <summary>
    /// Merges the colors of the inverted source rectangle with the colors
    /// of the destination rectangle by using the Boolean OR operator.
    /// </summary>
    MERGEPAINT = 0x00BB0226,

    /// <summary>
    /// Windows 98/Me, Windows 2000/XP: Prevents the bitmap from being
    /// mirrored.
    /// </summary>
    NOMIRRORBITMAP = -2147483648, // 0x80000000

    /// <summary>
    /// Copies the inverted source rectangle to the destination.
    /// </summary>
    NOTSRCCOPY = 0x00330008,

    /// <summary>
    /// Combines the colors of the source and destination rectangles
    /// by using the Boolean OR operator and then inverts the resultant
    /// color.
    /// </summary>
    NOTSRCERASE = 0x001100A6,

    /// <summary>
    /// Copies the brush currently selected in hdcDest, into the destination
    /// bitmap.
    /// </summary>
    PATCOPY = 0x00F00021,

    /// <summary>
    /// Combines the colors of the brush currently selected in hdcDest,
    /// with the colors of the destination rectangle by using the Boolean
    /// XOR operator.
    /// </summary>
    PATINVERT = 0x005A0049,

    /// <summary>
    /// Combines the colors of the brush currently selected in hdcDest,
    /// with the colors of the inverted source rectangle by using the
    /// Boolean OR operator. The result of this operation is combined
    /// with the colors of the destination rectangle by using the Boolean
    /// OR operator.
    /// </summary>
    PATPAINT = 0x00FB0A09,

    /// <summary>
    /// Combines the colors of the source and destination rectangles
    /// by using the Boolean AND operator.
    /// </summary>
    SRCAND = 0x008800C6,

    /// <summary>
    /// Copies the source rectangle directly to the destination rectangle.
    /// </summary>
    SRCCOPY = 0x00CC0020,

    /// <summary>
    /// Combines the inverted colors of the destination rectangle
    /// with the colors of the source rectangle by using the Boolean
    /// AND operator.
    /// </summary>
    SRCERASE = 0x00440328,

    /// <summary>
    /// Combines the colors of the source and destination rectangles
    /// by using the Boolean XOR operator.
    /// </summary>
    SRCINVERT = 0x00660046,

    /// <summary>
    /// Combines the colors of the source and destination rectangles
    /// by using the Boolean OR operator.
    /// </summary>
    SRCPAINT = 0x00EE0086,

    /// <summary>
    /// Fills the destination rectangle using the color associated
    /// with index 1 in the physical palette. (This color is white
    /// for the default physical palette.)
    /// </summary>
    WHITENESS = 0x00FF0062
}
