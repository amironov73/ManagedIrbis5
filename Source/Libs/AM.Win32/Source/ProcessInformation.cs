using System;
using System.Runtime.InteropServices;

namespace AM.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct ProcessInformation
{
    public IntPtr Process;
    public IntPtr Thread;
    public int ProcessId;
    public int ThreadId;
}
