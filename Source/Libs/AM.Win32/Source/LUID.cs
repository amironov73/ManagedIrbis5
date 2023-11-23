using System.Runtime.InteropServices;

namespace AM.Win32;

[StructLayout (LayoutKind.Sequential)]
public struct LUID
{
    public uint LowPart;
    public int HighPart;
}
