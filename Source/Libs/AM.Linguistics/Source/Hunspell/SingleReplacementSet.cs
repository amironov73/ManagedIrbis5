// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StringReplacementSet.cs --
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

/// <summary>
///
/// </summary>
public sealed class SingleReplacementSet
    : ArrayWrapper<SingleReplacement>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly SingleReplacementSet Empty = TakeArray (Array.Empty<SingleReplacement>());

    /// <summary>
    ///
    /// </summary>
    /// <param name="replacements"></param>
    /// <returns></returns>
    public static SingleReplacementSet Create (IEnumerable<SingleReplacement> replacements)
    {
        return replacements == null! ? Empty : TakeArray (replacements.ToArray());
    }

    internal static SingleReplacementSet TakeArray (SingleReplacement[] replacements)
    {
        return replacements == null! ? Empty : new SingleReplacementSet (replacements);
    }

    private SingleReplacementSet (SingleReplacement[] replacements)
        : base (replacements)
    {
        // пустое тело конструктора
    }
}
