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

// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* FILETIME.cs -- 64-bit time
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// The FILETIME structure is a 64-bit value representing the
/// number of 100-nanosecond intervals since January 1, 1601 (UTC).
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct FILETIME
{
    /// <summary>
    /// Low-order part of the file time.
    /// </summary>
    public uint dwLowDateTime;

    /// <summary>
    /// High-order part of the file time.
    /// </summary>
    public uint dwHighDateTime;
}
