// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable UnusedMember.Local

/* PlatformType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public static class PlatformType
{
    /// <summary>
    ///
    /// </summary>
    const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;

    /// <summary>
    ///
    /// </summary>
    const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;

    /// <summary>
    ///
    /// </summary>
    const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;

    /// <summary>
    ///
    /// </summary>
    const ushort PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFFFF;

    [StructLayout (LayoutKind.Sequential)]
    struct SYSTEM_INFO
    {
        /// <summary>
        ///
        /// </summary>
        public ushort wProcessorArchitecture;

        /// <summary>
        ///
        /// </summary>
        public ushort wReserved;

        /// <summary>
        ///
        /// </summary>
        public uint dwPageSize;

        /// <summary>
        ///
        /// </summary>
        public IntPtr lpMinimumApplicationAddress;

        /// <summary>
        ///
        /// </summary>
        public IntPtr lpMaximumApplicationAddress;

        /// <summary>
        ///
        /// </summary>
        public UIntPtr dwActiveProcessorMask;

        /// <summary>
        ///
        /// </summary>
        public uint dwNumberOfProcessors;

        /// <summary>
        ///
        /// </summary>
        public uint dwProcessorType;

        /// <summary>
        ///
        /// </summary>
        public uint dwAllocationGranularity;

        /// <summary>
        ///
        /// </summary>
        public ushort wProcessorLevel;

        /// <summary>
        ///
        /// </summary>
        public ushort wProcessorRevision;
    };

    [DllImport ("kernel32.dll")]
    static extern void GetNativeSystemInfo (ref SYSTEM_INFO lpSystemInfo);

    [DllImport ("kernel32.dll")]
    static extern void GetSystemInfo (ref SYSTEM_INFO lpSystemInfo);

    /// <summary>
    ///
    /// </summary>
    public static Platform GetOperationSystemPlatform()
    {
        var sysInfo = new SYSTEM_INFO();

        // WinXP and older - use GetNativeSystemInfo
        if (Environment.OSVersion.Version.Major > 5 ||
            (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1))
        {
            GetNativeSystemInfo (ref sysInfo);
        }

        // else use GetSystemInfo
        else
        {
            GetSystemInfo (ref sysInfo);
        }

        switch (sysInfo.wProcessorArchitecture)
        {
            case PROCESSOR_ARCHITECTURE_IA64:
            case PROCESSOR_ARCHITECTURE_AMD64:
                return Platform.X64;

            case PROCESSOR_ARCHITECTURE_INTEL:
                return Platform.X86;

            default:
                return Platform.Unknown;
        }
    }
}

/// <summary>
///
/// </summary>
public enum Platform
{
    /// <summary>
    ///
    /// </summary>
    X86,

    /// <summary>
    ///
    /// </summary>
    X64,

    /// <summary>
    ///
    /// </summary>
    Unknown
}
