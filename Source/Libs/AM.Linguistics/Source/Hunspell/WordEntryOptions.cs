// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

[Flags]
public enum WordEntryOptions : byte
{
    /// <summary>
    /// Indicates there is optional morphological data.
    /// </summary>
    None = 0,

    /// <summary>
    /// Using alias compression.
    /// </summary>
    AliasM = 1 << 1,

    /// <summary>
    /// Indicates there is a "ph:" field in the morphological data.
    /// </summary>
    Phon = 1 << 2,

    /// <summary>
    /// Indicates the dictionary word is capitalized.
    /// </summary>
    InitCap = 1 << 3
}
