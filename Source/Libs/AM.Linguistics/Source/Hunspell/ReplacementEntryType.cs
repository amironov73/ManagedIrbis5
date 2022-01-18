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

namespace AM.Linguistics.Hunspell;

[Flags]
public enum ReplacementValueType : byte
{
    /// <summary>
    /// Indicates that text can contain the pattern.
    /// </summary>
    Med = 0,
    /// <summary>
    /// Indicates that text can start with the pattern.
    /// </summary>
    Ini = 1,
    /// <summary>
    /// Indicates that text can end with the pattern.
    /// </summary>
    Fin = 2,
    /// <summary>
    /// Indicates that text must match the pattern exactly.
    /// </summary>
    Isol = Ini | Fin
}