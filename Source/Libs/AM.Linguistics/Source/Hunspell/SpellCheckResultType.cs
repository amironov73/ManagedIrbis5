// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* SpellCheckResultType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
[Flags]
public enum SpellCheckResultType
    : byte
{
    /// <summary>
    ///
    /// </summary>
    None = 0,

    /// <summary>
    ///
    /// </summary>
    Compound = 1 << 0,

    /// <summary>
    ///
    /// </summary>
    Forbidden = 1 << 1,

    /// <summary>
    ///
    /// </summary>
    AllCap = 1 << 2,

    /// <summary>
    ///
    /// </summary>
    NoCap = 1 << 3,

    /// <summary>
    ///
    /// </summary>
    InitCap = 1 << 4,

    /// <summary>
    ///
    /// </summary>
    OrigCap = 1 << 5,

    /// <summary>
    ///
    /// </summary>
    Warn = 1 << 6
}
