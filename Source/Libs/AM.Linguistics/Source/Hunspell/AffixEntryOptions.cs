// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* AffixEntryOptions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

[Flags]
public enum AffixEntryOptions
    : byte
{
    None = 0,

    /// <summary>
    /// Indicates that both prefixes and suffixes can apply to the same subject.
    /// </summary>
    CrossProduct = 1 << 0,

    Utf8 = 1 << 1,

    AliasF = 1 << 2,

    AliasM = 1 << 3,

    LongCond = 1 << 4
}
