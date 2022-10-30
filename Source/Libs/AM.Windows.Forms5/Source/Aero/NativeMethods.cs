// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NativeMethods.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AeroSuite;

/// <summary>
/// Provides some methods from the user32 and uxtheme libraries.
/// </summary>
internal static class NativeMethods
{
    private const string User32 = "user32.dll";
    private const string UxTheme = "uxtheme.dll";
    private const string DwmApi = "dwmapi.dll";

    [DllImport (User32, SetLastError = true)]
    public static extern IntPtr SendMessage (IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport (User32, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr SendMessage (IntPtr hWnd, UInt32 msg, IntPtr wParam, string lParam);

    [DllImport (User32, SetLastError = true)]
    public static extern IntPtr LoadCursor (IntPtr hInstance, int lpCursorName);

    [DllImport (User32, SetLastError = true)]
    public static extern IntPtr SetCursor (IntPtr hCursor);

    [DllImport (User32, SetLastError = true)]
    public static extern IntPtr LoadImage (IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired,
        uint fuLoad);

    [DllImport (User32, SetLastError = true)]
    public static extern bool SetWindowPos (IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
        int uFlags);

    [DllImport (User32, EntryPoint = "SetWindowLong", SetLastError = true)]
    public static extern int SetWindowLong32 (IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport (User32, EntryPoint = "SetWindowLongPtr", SetLastError = true)]
    public static extern IntPtr SetWindowLongPtr64 (IntPtr hWnd, int nIndex, IntPtr dwNewLong);


    [DllImport (UxTheme, CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int SetWindowTheme (IntPtr hWnd, string pszSubAppName, string? pszSubIdList);


    [DllImport (DwmApi, SetLastError = true)]
    public static extern int DwmExtendFrameIntoClientArea (IntPtr hwnd,
        ref Forms.BorderlessForm.Margins margins);

    [DllImport (DwmApi, PreserveSig = false, SetLastError = true)]
    public static extern bool DwmIsCompositionEnabled();
}
