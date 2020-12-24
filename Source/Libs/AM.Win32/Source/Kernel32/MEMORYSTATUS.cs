// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

/* MEMORYSTATUS.cs -- information about the current state of both physical and virtual memory
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// The MEMORYSTATUS structure contains information about
    /// the current state of both physical and virtual memory.
    /// The GlobalMemoryStatus function stores information in
    /// a MEMORYSTATUS structure.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = StructureSize)]
    public struct MEMORYSTATUS
    {
        /// <summary>
        /// Structure size.
        /// </summary>
        public const int StructureSize = 32;

        /// <summary>
        /// Size of the MEMORYSTATUS data structure, in bytes.
        /// You do not need to set this member before calling the
        /// GlobalMemoryStatus function; the function sets it.
        /// </summary>
        public uint dwLength;

        /// <summary>
        /// <para>Approximate percentage of total physical memory
        /// that is in use.</para>
        /// <para>Windows NT:  Percentage of approximately the last
        /// 1000 pages of physical memory that is in use.</para>
        /// </summary>
        public uint dwMemoryLoad;

        /// <summary>
        /// Total size of physical memory, in bytes.
        /// </summary>
        public uint dwTotalPhys;

        /// <summary>
        /// Size of physical memory available, in bytes.
        /// </summary>
        public uint dwAvailPhys;

        /// <summary>
        /// Size of the committed memory limit, in bytes.
        /// </summary>
        public uint dwTotalPageFile;

        /// <summary>
        /// Size of available memory to commit, in bytes.
        /// </summary>
        public uint dwAvailPageFile;

        /// <summary>
        /// Total size of the user mode portion of the virtual address
        /// space of the calling process, in bytes.
        /// </summary>
        public uint dwTotalVirtual;

        /// <summary>
        /// Size of unreserved and uncommitted memory in the user mode
        /// portion of the virtual address space of the calling process,
        /// in bytes.
        /// </summary>
        public uint dwAvailVirtual;

    } // struct MEMORYSTATUS

} // namespace AM.Win32
