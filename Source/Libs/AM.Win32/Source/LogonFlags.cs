using System;

namespace AM.Win32;

[Flags]
internal enum LogonFlags : uint
{
    None = 0,
    LogonWithProfile = 1,
    LogonNetCredentialsOnly = 2
}
