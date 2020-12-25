﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* JobObjectExtendedLimitInformation.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Contains basic and extended limit information
    /// for a job object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
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
}
