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

/* FILEGROUPDESCRIPTORW.cs -- CF_FILEGROUPDESCRIPTOR clipboard format
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Defines the CF_FILEGROUPDESCRIPTOR clipboard format.
/// </summary>
public struct FILEGROUPDESCRIPTORW
{
    /// <summary>
    /// Number of elements in fgd.
    /// </summary>
    public int cItems;

    /// <summary>
    /// Array of <see cref="FILEDESCRIPTORW"/>
    ///  structures that contain the file information.
    /// </summary>
    /// <remarks>
    /// SizeConst!!!
    /// </remarks>
    [MarshalAs (UnmanagedType.ByValArray, SizeConst = 1)]
    public FILEDESCRIPTORW[]? fgd;
}
