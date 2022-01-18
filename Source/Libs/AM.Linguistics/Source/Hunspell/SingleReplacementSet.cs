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
using System.Collections.Generic;
using System.Linq;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

public sealed class SingleReplacementSet : ArrayWrapper<SingleReplacement>
{
    public static readonly SingleReplacementSet Empty = TakeArray(Array.Empty<SingleReplacement>());

    public static SingleReplacementSet Create(IEnumerable<SingleReplacement> replacements) =>
        replacements == null ? Empty : TakeArray(replacements.ToArray());

    internal static SingleReplacementSet TakeArray(SingleReplacement[] replacements) =>
        replacements == null ? Empty : new SingleReplacementSet(replacements);

    private SingleReplacementSet(SingleReplacement[] replacements)
        : base(replacements)
    {
    }
}
