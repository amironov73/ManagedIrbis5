// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DEBUGHOOKINFO.cs -- information passed to WH_DEBUG hook
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Contains debugging information passed to a WH_DEBUG
/// hook procedure, DebugProc.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct DEBUGHOOKINFO
{
    /// <summary>
    /// Handle to the thread containing the filter function.
    /// </summary>
    public int idThread;

    /// <summary>
    /// Handle to the thread that installed the debugging
    /// filter function.
    /// </summary>
    public int idThreadInstaller;

    /// <summary>
    /// Specifies the value to be passed to the hook in the
    /// lParam parameter of the DebugProc callback function.
    /// </summary>
    public int lParam;

    /// <summary>
    /// Specifies the value to be passed to the hook in the
    /// wParam parameter of the DebugProc callback function.
    /// </summary>
    public int wParam;

    /// <summary>
    /// Specifies the value to be passed to the hook in the
    /// nCode parameter of the DebugProc callback function.
    /// </summary>
    public int code;
}
