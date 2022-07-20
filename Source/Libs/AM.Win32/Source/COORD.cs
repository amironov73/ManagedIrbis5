﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global


/* COORD.cs -- координаты ячейки с символом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// The COORD structure defines the coordinates of a character cell
/// in a console screen buffer. The origin of the coordinate system
/// (0,0) is at the top, left cell of the buffer.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Explicit, Size = 4)]
public struct COORD
{
    /// <summary>
    /// Horizontal coordinate or column value.
    /// </summary>
    [FieldOffset (0)]
    public short X;

    /// <summary>
    /// Vertical coordinate or row value.
    /// </summary>
    [FieldOffset (2)]
    public short Y;
}
