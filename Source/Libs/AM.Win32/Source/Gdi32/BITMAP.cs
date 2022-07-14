// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BITMAP.cs -- defines the type, width, height, color format, and bit values of a bitmap
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// The BITMAP structure defines the type, width, height,
/// color format, and bit values of a bitmap.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential, Pack = 1)]
public struct BITMAP
{
    /// <summary>
    /// Specifies the bitmap type. This member must be zero.
    /// </summary>
    public int bmType;

    /// <summary>
    /// Specifies the width, in pixels, of the bitmap.
    /// The width must be greater than zero.
    /// </summary>
    public int bmWidth;

    /// <summary>
    /// Specifies the height, in pixels, of the bitmap.
    /// The height must be greater than zero.
    /// </summary>
    public int bmHeight;

    /// <summary>
    /// Specifies the number of bytes in each scan line.
    /// This value must be divisible by 2, because the system
    /// assumes that the bit values of a bitmap form an array
    /// that is word aligned.
    /// </summary>
    public int bmWidthBytes;

    /// <summary>
    /// Specifies the count of color planes.
    /// </summary>
    public ushort bmPlanes;

    /// <summary>
    /// Specifies the number of bits required to indicate
    /// the color of a pixel.
    /// </summary>
    public ushort bmBitsPixel;

    /// <summary>
    /// Pointer to the location of the bit values for the bitmap.
    /// The bmBits member must be a long pointer to an array of
    /// character (1-byte) values.
    /// </summary>
    public IntPtr bmBits;
}
