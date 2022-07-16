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

/* JobObjectInfoType.cs -- limits for JobObject
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Win32;

/// <summary>
/// The information class for the limits to be set
/// for <see cref="Kernel32.SetInformationJobObject"/>.
/// This parameter can be one of the following values.
/// </summary>
public enum JobObjectInfoType
{
    /// <summary>
    /// The lpJobObjectInfo parameter is a pointer
    /// to a JOBOBJECT_ASSOCIATE_COMPLETION_PORT
    /// structure.
    /// </summary>
    AssociateCompletionPortInformation = 7,

    /// <summary>
    /// The lpJobObjectInfo parameter is a pointer
    /// to a JOBOBJECT_BASIC_LIMIT_INFORMATION
    /// structure.
    /// </summary>
    BasicLimitInformation = 2,

    /// <summary>
    /// The lpJobObjectInfo parameter is a pointer
    /// to a JOBOBJECT_BASIC_UI_RESTRICTIONS structure.
    /// </summary>
    BasicUiRestrictions = 4,

    /// <summary>
    /// The lpJobObjectInfo parameter is a pointer
    /// to a JOBOBJECT_END_OF_JOB_TIME_INFORMATION
    /// structure.
    /// </summary>
    EndOfJobTimeInformation = 6,

    /// <summary>
    /// The lpJobObjectInfo parameter is a pointer
    /// to a JOBOBJECT_EXTENDED_LIMIT_INFORMATION
    /// structure.
    /// </summary>
    ExtendedLimitInformation = 9,

    /// <summary>
    /// This flag is not supported. Applications
    /// must set security limitations individually
    /// for each process.
    /// </summary>
    SecurityLimitInformation = 5,

    /// <summary>
    /// The lpJobObjectInfo parameter is a pointer
    /// to a USHORT value that specifies the list
    /// of processor groups to assign the job to.
    /// The cbJobObjectInfoLength parameter is set
    /// to the size of the group data. Divide this
    /// value by sizeof(USHORT) to determine
    /// the number of groups.
    /// </summary>
    GroupInformation = 11
}
