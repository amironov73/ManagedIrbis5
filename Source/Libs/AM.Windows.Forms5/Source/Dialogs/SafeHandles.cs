// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
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

#nullable enable

namespace AM.Windows.Forms.Dialogs;

[SecurityPermission (SecurityAction.Demand, UnmanagedCode = true)]
class ActivationContextSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    public ActivationContextSafeHandle()
        : base (true)
    {
    }

    [ReliabilityContract (Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
        NativeMethods.ReleaseActCtx (handle);
        return true;
    }
}

[SecurityPermission (SecurityAction.Demand, UnmanagedCode = true)]
class SafeGDIHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal SafeGDIHandle()
        : base (true)
    {
    }

    internal SafeGDIHandle (IntPtr existingHandle, bool ownsHandle)
        : base (ownsHandle)
    {
        SetHandle (existingHandle);
    }

    protected override bool ReleaseHandle()
    {
        return NativeMethods.DeleteObject (handle);
    }
}

[SecurityPermission (SecurityAction.Demand, UnmanagedCode = true)]
class SafeDeviceHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal SafeDeviceHandle()
        : base (true)
    {
    }

    [SuppressMessage ("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    internal SafeDeviceHandle (IntPtr existingHandle, bool ownsHandle)
        : base (ownsHandle)
    {
        SetHandle (existingHandle);
    }

    protected override bool ReleaseHandle()
    {
        return NativeMethods.DeleteDC (handle);
    }
}

class SafeModuleHandle : SafeHandle
{
    public SafeModuleHandle()
        : base (IntPtr.Zero, true)
    {
    }

    public override bool IsInvalid
    {
        get { return handle == IntPtr.Zero; }
    }

    [ReliabilityContract (Consistency.WillNotCorruptState, Cer.MayFail)]
    protected override bool ReleaseHandle()
    {
        return NativeMethods.FreeLibrary (handle);
    }
}