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

/// <summary>
///
/// </summary>
[Flags]
public enum AffixEntryOptions
    : byte
{
    /// <summary>
    ///
    /// </summary>
    None = 0,

    /// <summary>
    /// Indicates that both prefixes and suffixes can apply to the same subject.
    /// </summary>
    CrossProduct = 1 << 0,

    /// <summary>
    ///
    /// </summary>
    Utf8 = 1 << 1,

    /// <summary>
    ///
    /// </summary>
    AliasF = 1 << 2,

    /// <summary>
    ///
    /// </summary>
    AliasM = 1 << 3,

    /// <summary>
    ///
    /// </summary>
    LongCond = 1 << 4
}
