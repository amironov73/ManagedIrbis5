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
public enum SpellCheckResultType : byte
{
    None = 0,
    Compound = 1 << 0,
    Forbidden = 1 << 1,
    AllCap = 1 << 2,
    NoCap = 1 << 3,
    InitCap = 1 << 4,
    OrigCap = 1 << 5,
    Warn = 1 << 6
}