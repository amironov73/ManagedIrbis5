// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CONSOLE_FONT_INFO.cs -- информация о консольном шрифте
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Содержит информацию о консольном шрифте.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Explicit, Size = 8)]
public struct CONSOLE_FONT_INFO
{
    /// <summary>
    /// Index of the font in the system's console font table.
    /// </summary>
    [FieldOffset (0)]
    public int nFont;

    /// <summary>
    /// A COORD structure that contains the width and height
    /// of each character in the font. The X member contains
    /// the width, while the Y member contains the height.
    /// </summary>
    [FieldOffset (4)]
    public COORD dwFontSize;
}
