// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IoCounters.cs -- счетчики ввода-вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Contains I/O accounting information for a process
/// or a job object. For a job object, the counters
/// include all operations performed by all processes
/// that have ever been associated with the job,
/// in addition to all processes currently associated
/// with the job.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct IoCounters
{
    /// <summary>
    /// The number of read operations performed.
    /// </summary>
    public ulong ReadOperationCount;

    /// <summary>
    /// The number of write operations performed.
    /// </summary>
    public ulong WriteOperationCount;

    /// <summary>
    /// The number of I/O operations performed,
    /// other than read and write operations.
    /// </summary>
    public ulong OtherOperationCount;

    /// <summary>
    /// The number of bytes read.
    /// </summary>
    public ulong ReadTransferCount;

    /// <summary>
    /// The number of bytes written.
    /// </summary>
    public ulong WriteTransferCount;

    /// <summary>
    /// The number of bytes transferred during
    /// operations other than read and write operations.
    /// </summary>
    public ulong OtherTransferCount;
}
