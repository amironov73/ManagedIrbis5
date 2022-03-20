﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DuplicateHandleFlags.cs -- options for DuplicateHandle function.
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Options for DuplicateHandle function.
/// </summary>
[Flags]
public enum DuplicateHandleFlags
{
    /// <summary>
    /// Closes the source handle. This occurs regardless of
    /// any error status returned.
    /// </summary>
    DUPLICATE_CLOSE_SOURCE = 0x00000001,

    /// <summary>
    /// Ignores the dwDesiredAccess parameter. The duplicate handle
    /// has the same access as the source handle.
    /// </summary>
    DUPLICATE_SAME_ACCESS = 0x00000002
}
