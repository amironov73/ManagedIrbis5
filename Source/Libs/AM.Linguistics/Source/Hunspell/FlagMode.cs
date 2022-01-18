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

namespace AM.Linguistics.Hunspell;

/// <summary>
/// Indicates the method of encoding used for flag values.
/// </summary>
public enum FlagMode : byte
{
    /// <summary>
    /// Ispell's one-character flags (erfg -> e r f g).
    /// </summary>
    Char,

    /// <summary>
    /// Two-character flags (1x2yZz -> 1x 2y Zz).
    /// </summary>
    Long,

    /// <summary>
    /// Decimal numbers separated by comma (4521,23,233 -> 452123 233).
    /// </summary>
    Num,

    /// <summary>
    /// UTF-8 characters.
    /// </summary>
    Uni
}
