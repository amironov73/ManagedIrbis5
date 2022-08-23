// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

/* FLASHWINFO.cs -- flash status of a window
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// The FLASHWINFO structure contains the flash status for a
/// window and the number of times the system should flash
/// the window.
/// </summary>
[StructLayout (LayoutKind.Sequential, Size = StructureSize)]
public struct FLASHWINFO
{
    /// <summary>
    /// Structure size.
    /// </summary>
    public const int StructureSize = 20;

    /// <summary>
    /// Size of the structure, in bytes.
    /// </summary>
    public uint cbSize;

    /// <summary>
    /// Handle to the window to be flashed.
    /// The window can be either opened or minimized.
    /// </summary>
    public IntPtr hwnd;

    /// <summary>
    /// Flash status.
    /// </summary>
    public FlashWindowFlags dwFlags;

    /// <summary>
    /// Number of times to flash the window.
    /// </summary>
    public uint uCount;

    /// <summary>
    /// Rate at which the window is to be flashed, in milliseconds.
    /// If dwTimeout is zero, the function uses the default cursor
    /// blink rate.
    /// </summary>
    public uint dwTimeout;
}
