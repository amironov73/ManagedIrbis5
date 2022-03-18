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

/* FontResourceFlags.cs -- specifies characteristics of the font to be added to the system
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Specifies characteristics of the font to be added to the system.
/// </summary>
[Flags]
public enum FontResourceFlags
{
    /// <summary>
    /// Specifies that only the process that called the AddFontResourceEx
    /// function can use this font. When the font name matches a public
    /// font, the private font will be chosen. When the process terminates,
    /// the system will remove all fonts installed by the process with
    /// the AddFontResourceEx function.
    /// </summary>
    FR_PRIVATE = 0x10,

    /// <summary>
    /// Specifies that no process, including the process that called
    /// the AddFontResourceEx function, can enumerate this font.
    /// </summary>
    FR_NOT_ENUM = 0x20
}
