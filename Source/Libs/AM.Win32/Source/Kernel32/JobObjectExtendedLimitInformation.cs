// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* JobObjectExtendedLimitInformation.cs -- информация о предельных значениях для объекта задания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Содержит как базовую, так и расширенную информацию
/// о предельных значениях для объекта задания.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct JobObjectExtendedLimitInformation
{
    /// <summary>
    /// A JOBOBJECT_BASIC_LIMIT_INFORMATION structure
    /// that contains basic limit information.
    /// </summary>
    public JobObjectBasicLimitInformation BasicLimitInformation;

    /// <summary>
    /// Reserved.
    /// </summary>
    public IoCounters IoInfo;

    /// <summary>
    /// If the LimitFlags member of the
    /// JOBOBJECT_BASIC_LIMIT_INFORMATION structure
    /// specifies the JOB_OBJECT_LIMIT_PROCESS_MEMORY
    /// value, this member specifies the limit for
    /// the virtual memory that can be committed by
    /// a process. Otherwise, this member is ignored.
    /// </summary>
    public UIntPtr ProcessMemoryLimit;

    /// <summary>
    /// If the LimitFlags member of the
    /// JOBOBJECT_BASIC_LIMIT_INFORMATION structure
    /// specifies the JOB_OBJECT_LIMIT_JOB_MEMORY value,
    /// this member specifies the limit for
    /// the virtual memory that can be committed
    /// for the job. Otherwise, this member is ignored.
    /// </summary>
    public UIntPtr JobMemoryLimit;

    /// <summary>
    /// The peak memory used by any process ever
    /// associated with the job.
    /// </summary>
    public UIntPtr PeakProcessMemoryUsed;

    /// <summary>
    /// The peak memory usage of all processes
    /// currently associated with the job.
    /// </summary>
    public UIntPtr PeakJobMemoryUsed;
}
