// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* BreakSet.cs --
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

public sealed class BreakSet
    : ArrayWrapper<string>
{
    public static readonly BreakSet Empty = TakeArray (Array.Empty<string>());

    internal static BreakSet TakeArray (string[] breaks)
    {
        return breaks == null ? Empty : new BreakSet (breaks);
    }

    public static BreakSet Create (List<string> breaks)
    {
        return breaks == null ? Empty : TakeArray (breaks.ToArray());
    }

    public static BreakSet Create (IEnumerable<string> breaks)
    {
        return breaks == null ? Empty : TakeArray (breaks.ToArray());
    }

    #region Constructions

    /// <summary>
    /// Конструктор.
    /// </summary>
    private BreakSet
        (
            string[] breaks
        )
        : base (breaks)
    {
    }

    #endregion

    /// <summary>
    /// Calculate break points for recursion limit.
    /// </summary>
    internal int FindRecursionLimit (string scw)
    {
        var nbr = 0;

        if (!string.IsNullOrEmpty (scw))
            foreach (var breakEntry in items)
            {
                var pos = 0;
                while ((pos = scw.IndexOf (breakEntry, pos, StringComparison.Ordinal)) >= 0)
                {
                    nbr++;
                    pos += breakEntry.Length;
                }
            }

        return nbr;
    }
}
