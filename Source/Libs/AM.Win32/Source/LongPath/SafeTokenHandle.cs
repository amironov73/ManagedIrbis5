// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Security;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

using System.Runtime.ConstrainedExecution;

#endregion

#nullable enable

namespace AM.Win32.LongPath;

internal class SafeTokenHandle
    : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeTokenHandle() 
        : base (true)
    {
        // пустое тело конструктора
    }

    // 0 is an Invalid Handle
    internal SafeTokenHandle (IntPtr handle)
        : base (true)
    {
        SetHandle (handle);
    }

    internal static SafeTokenHandle InvalidHandle => new (IntPtr.Zero);

    [DllImport ("kernel32.dll", SetLastError = true)]
    [SuppressUnmanagedCodeSecurity]
    private static extern bool CloseHandle (IntPtr handle);

    protected override bool ReleaseHandle()
    {
        return CloseHandle (handle);
    }
}
