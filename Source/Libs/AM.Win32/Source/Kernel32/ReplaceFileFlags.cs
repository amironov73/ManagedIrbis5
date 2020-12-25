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

/* ReplaceFileFlags.cs -- file replacement options
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// File replacement options.
    /// </summary>
    [Flags]
    public enum ReplaceFileFlags
    {
        /// <summary>
        /// This value is not supported.
        /// </summary>
        REPLACEFILE_WRITE_THROUGH = 0x00000001,

        /// <summary>
        /// Ignores errors that occur while merging information
        /// (such as attributes and ACLs) from the replaced file
        /// to the replacement file. Therefore, if you specify
        /// this flag and do not have WRITE_DAC access,
        /// the function succeeds but the ACLs are not preserved.
        /// </summary>
        REPLACEFILE_IGNORE_MERGE_ERRORS = 0x00000002,

        /// <summary>
        /// Ignores errors that occur while merging ACL information
        /// from the replaced file to the replacement file.
        /// Therefore, if you specify this flag and do not have
        /// WRITE_DAC access, the function succeeds but the ACLs
        /// are not preserved. To compile an application that uses
        /// this value, define the _WIN32_WINNT macro as 0x0600 or later.
        /// Windows Server 2003 and Windows XP: This value is not supported.
        /// </summary>
        REPLACEFILE_IGNORE_ACL_ERRORS = 0x00000004

    } // enum ReplaceFileFlags

} // namespace AM.Win32
