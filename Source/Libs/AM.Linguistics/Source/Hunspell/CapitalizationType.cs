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

#nullable enable

namespace AM.Linguistics.Hunspell;

enum CapitalizationType : byte
{
    /// <summary>
    /// No letters capitalized.
    /// </summary>
    None = 0,

    /// <summary>
    /// Initial letter capitalized.
    /// </summary>
    Init = 1,

    /// <summary>
    /// All letters capitalized.
    /// </summary>
    All = 2,

    /// <summary>
    /// Mixed case.
    /// </summary>
    Huh = 3,

    /// <summary>
    /// Initial letter capitalized with mixed case.
    /// </summary>
    HuhInit = 4
}
