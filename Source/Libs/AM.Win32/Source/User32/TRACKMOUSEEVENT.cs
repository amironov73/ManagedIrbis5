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

/* TRACKMOUSEEVENT.cs -- structure used by the TrackMouseEvent function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// The TRACKMOUSEEVENT structure is used by the TrackMouseEvent
/// function to track when the mouse pointer leaves a window or
/// hovers over a window for a specified amount of time.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct TRACKMOUSEEVENT
{
    /// <summary>
    /// Specifies the size of the TRACKMOUSEEVENT structure.
    /// </summary>
    public int cbSize;

    /// <summary>
    /// Specifies the services requested.
    /// </summary>
    public TrackMouseEventFlags dwFlags;

    /// <summary>
    /// Specifies a handle to the window to track.
    /// </summary>
    public IntPtr hwndTrack;

    /// <summary>
    /// Specifies the hover time-out (if TME_HOVER was specified in
    /// dwFlags), in milliseconds. Can be HOVER_DEFAULT, which means
    /// to use the system default hover time-out.
    /// </summary>
    public int dwHoverTime;
}
