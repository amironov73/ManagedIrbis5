// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* JobObjectBasicLimitInformation.cs -- базовая информация о предельных значениях для объекта задания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Содержит как базовую информацию о предельных значениях
/// для объекта задания.
/// </summary>
[StructLayout (LayoutKind.Sequential)]
public struct JobObjectBasicLimitInformation
{
    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_PROCESS_TIME, this member
    /// is the per-process user-mode execution time
    /// limit, in 100-nanosecond ticks.
    /// Otherwise, this member is ignored.
    /// </summary>
    public long PerProcessUserTimeLimit;

    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_JOB_TIME, this member
    /// is the per-job user-mode execution time limit,
    /// in 100-nanosecond ticks. Otherwise,
    /// this member is ignored.
    /// </summary>
    public long PerJobUserTimeLimit;

    /// <summary>
    /// The limit flags that are in effect.
    /// This member is a bitfield that determines
    /// whether other structure members are used.
    /// </summary>
    public uint LimitFlags;

    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_WORKINGSET, this member
    /// is the minimum working set size in bytes
    /// for each process associated with the job.
    /// Otherwise, this member is ignored.
    /// </summary>
    public UIntPtr MinimumWorkingSetSize;

    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_WORKINGSET, this member
    /// is the maximum working set size in bytes
    /// for each process associated with the job.
    /// Otherwise, this member is ignored.
    /// </summary>
    public UIntPtr MaximumWorkingSetSize;

    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_ACTIVE_PROCESS, this member
    /// is the active process limit for the job.
    /// Otherwise, this member is ignored.
    /// </summary>
    public uint ActiveProcessLimit;

    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_AFFINITY, this member
    /// is the processor affinity for all processes
    /// associated with the job.
    /// Otherwise, this member is ignored.
    /// </summary>
    public UIntPtr Affinity;

    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_PRIORITY_CLASS, this member
    /// is the priority class for all processes
    /// associated with the job. Otherwise,
    /// this member is ignored.
    /// </summary>
    public uint PriorityClass;

    /// <summary>
    /// If LimitFlags specifies
    /// JOB_OBJECT_LIMIT_SCHEDULING_CLASS, this member
    /// is the scheduling class for all processes
    /// associated with the job. Otherwise,
    /// this member is ignored.
    /// </summary>
    public uint SchedulingClass;
}
