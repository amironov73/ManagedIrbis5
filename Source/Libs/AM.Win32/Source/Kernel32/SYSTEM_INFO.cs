﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SYSTEM_INFO.cs -- информация о состоянии компьютерной системы
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// The SYSTEM_INFO structure contains information about the
/// current computer system. This includes the architecture
/// and type of the processor, the number of processors in
/// the system, the page size, and other such information.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Explicit, Size = 36)]
public struct SYSTEM_INFO
{
    /// <summary>
    /// <para>An obsolete member that is retained for compatibility
    /// with Windows NT 3.5 and earlier. New applications should use
    /// the wProcessorArchitecture branch of the union.</para>
    /// <para>Windows Me/98/95: The system always sets this member
    /// to zero, the value defined for PROCESSOR_ARCHITECTURE_INTEL.
    /// </para></summary>
    [FieldOffset (0)]
    public uint dwOemId;

    /// <summary>
    /// Архитектура процессора.
    /// </summary>
    [FieldOffset (0)]
    public ushort wProcessorArchitecture;

    /// <summary>
    /// Зарезервировано для использования в будущем.
    /// </summary>
    [FieldOffset (2)]
    public ushort wReserved;

    /// <summary>
    /// Page size and the granularity of page protection and
    /// commitment. This is the page size used by the VirtualAlloc
    /// function.
    /// </summary>
    [FieldOffset (4)]
    public uint dwPageSize;

    /// <summary>
    /// Pointer to the lowest memory address accessible to
    /// applications and dynamic-link libraries (DLLs).
    /// </summary>
    [FieldOffset (8)]
    public uint lpMinimumApplicationAddress;

    /// <summary>
    /// Pointer to the highest memory address accessible to
    /// applications and DLLs.
    /// </summary>
    [FieldOffset (12)]
    public uint lpMaximumApplicationAddress;

    /// <summary>
    /// Mask representing the set of processors configured into
    /// the system. Bit 0 is processor 0; bit 31 is processor 31.
    /// </summary>
    [FieldOffset (16)]
    public uint dwActiveProcessorMask;

    /// <summary>
    /// Number of processors in the system.
    /// </summary>
    [FieldOffset (20)]
    public uint dwNumberOfProcessors;

    /// <summary>
    /// An obsolete member that is retained for compatibility with
    /// Windows NT 3.5 and earlier. Use the wProcessorArchitecture,
    /// wProcessorLevel, and wProcessorRevision members to determine
    /// the type of processor.
    /// </summary>
    [FieldOffset (24)]
    public uint dwProcessorType;

    /// <summary>
    /// Granularity with which virtual memory is allocated. For
    /// example, a VirtualAlloc request to allocate 1 byte will
    /// reserve an address space of dwAllocationGranularity bytes.
    /// This value was hard coded as 64K in the past, but other
    /// hardware architectures may require different values.
    /// </summary>
    [FieldOffset (28)]
    public uint dwAllocationGranularity;

    /// <summary>
    /// System's architecture-dependent processor level. It should
    /// be used only for display purposes. To determine the feature
    /// set of a processor, use the IsProcessorFeaturePresent
    /// function.
    /// </summary>
    [FieldOffset (32)]
    public ushort wProcessorLevel;

    /// <summary>
    /// Architecture-dependent processor revision.
    /// </summary>
    [FieldOffset (34)]
    public ushort wProcessorRevision;
}
