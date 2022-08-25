// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MSLLHOOKSTRUCT.cs -- low-level keyboard input event
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Contains information about a low-level keyboard input event.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct MSLLHOOKSTRUCT
{
    /// <summary>
    /// Specifies a POINT structure that contains the x- and
    /// y-coordinates of the cursor, in screen coordinates.
    /// </summary>
    public Point pt;

    /// <summary>
    /// Data.
    /// </summary>
    public int mouseData;

    /// <summary>
    /// Specifies the event-injected flag.
    /// </summary>
    public LowLevelMouseHookFlags flags;

    /// <summary>
    /// Specifies the time stamp for this message.
    /// </summary>
    public int time;

    /// <summary>
    /// Specifies extra information associated with the message.
    /// </summary>
    public IntPtr dwExtraInfo;
}
