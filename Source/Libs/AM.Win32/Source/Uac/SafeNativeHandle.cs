using System;
using System.Security;

using Microsoft.Win32.SafeHandles;

namespace AM.Win32;

[SecurityCritical]
internal sealed class SafeNativeHandle
    : SafeHandleZeroOrMinusOneIsInvalid
{
    public static readonly SafeNativeHandle InvalidHandle = new (new IntPtr (-1));

    public SafeNativeHandle()
        : base(true)
    {
        // пустое тело конструктора
    }

    public SafeNativeHandle(IntPtr handle)
        : base(true)
    {
        SetHandle(handle);
    }

    public static bool CloseHandle (IntPtr handle) => Kernel32.CloseHandle(handle);

    [SecurityCritical]
    protected override bool ReleaseHandle() => CloseHandle(handle);
}
