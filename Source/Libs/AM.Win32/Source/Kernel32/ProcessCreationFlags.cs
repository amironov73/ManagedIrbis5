using System;

namespace AM.Win32;

[Flags]
public enum ProcessCreationFlags : uint
{
    None = 0,
    CreateNoWindow = 0x08000000,
    UnicodeEnvironment = 0x00000400,
    CreateNewConsole = 0x00000010,
    CreateSuspended = 0x00000004
}
