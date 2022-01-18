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

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

sealed class Deduper<T>
{
    public Deduper(IEqualityComparer<T> comparer)
    {
        lookup = new Dictionary<T, T>(comparer);
    }

    private readonly Dictionary<T, T> lookup;

    public T GetEqualOrAdd(T item)
    {
        if (lookup.TryGetValue(item, out T existing))
        {
            return existing;
        }
        else
        {
            lookup[item] = item;
            return item;
        }
    }

    public void Add(T item)
    {
        if (!lookup.ContainsKey(item))
        {
            lookup[item] = item;
        }
    }
}