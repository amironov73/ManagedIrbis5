// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ActivationContextSafeHandle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

using Microsoft.Win32.SafeHandles;

#endregion

#pragma warning disable SYSLIB0003
#pragma warning disable SYSLIB0004

#nullable enable

namespace AM.Windows.Forms.Dialogs;

[SecurityPermission (SecurityAction.Demand, UnmanagedCode = true)]
internal sealed class ActivationContextSafeHandle
    : SafeHandleZeroOrMinusOneIsInvalid
{
    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ActivationContextSafeHandle()
        : base (true)
    {
        // пустое тело конструктора
    }

    /// <inheritdoc cref="SafeHandle.ReleaseHandle"/>
    [ReliabilityContract (Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
        NativeMethods.ReleaseActCtx (handle);
        return true;
    }
}

[SecurityPermission (SecurityAction.Demand, UnmanagedCode = true)]
internal sealed class SafeGdiHandle
    : SafeHandleZeroOrMinusOneIsInvalid
{
    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SafeGdiHandle()
        : base (true)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SafeGdiHandle (IntPtr existingHandle, bool ownsHandle)
        : base (ownsHandle)
    {
        SetHandle (existingHandle);
    }

    /// <inheritdoc cref="SafeHandle.ReleaseHandle"/>
    protected override bool ReleaseHandle()
    {
        return NativeMethods.DeleteObject (handle);
    }
}

[SecurityPermission (SecurityAction.Demand, UnmanagedCode = true)]
internal class SafeDeviceHandle
    : SafeHandleZeroOrMinusOneIsInvalid
{
    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SafeDeviceHandle()
        : base (true)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    [SuppressMessage ("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public SafeDeviceHandle (IntPtr existingHandle, bool ownsHandle)
        : base (ownsHandle)
    {
        SetHandle (existingHandle);
    }

    /// <inheritdoc cref="SafeHandle.ReleaseHandle"/>
    protected override bool ReleaseHandle()
    {
        return NativeMethods.DeleteDC (handle);
    }
}

internal class SafeModuleHandle
    : SafeHandle
{
    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SafeModuleHandle()
        : base (IntPtr.Zero, true)
    {
        // пустое тело конструктора
    }

    /// <inheritdoc cref="SafeHandle.IsInvalid"/>
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <inheritdoc cref="SafeHandle.ReleaseHandle"/>
    [ReliabilityContract (Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
        return NativeMethods.FreeLibrary (handle);
    }
}
