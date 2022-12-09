// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* BinarySearcher.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linguistics;

/// <summary>
///
/// </summary>
public static class BinarySearcher
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <param name="comparer"></param>
    /// <param name="filter"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindOne<T>
        (
            this List<T> items,
            T item,
            IComparer<T> comparer,
            Predicate<T> filter
        )
    {
        var i = items.BinarySearch (item, comparer);
        if (i < 0 || i > items.Count - 1)
        {
            return default!;
        }

        while (i > 0 && comparer.Compare (items[i - 1], item) == 0)
        {
            i--;
        }

        if (filter (items[i]))
        {
            return items[i];
        }

        while (i < items.Count && comparer.Compare (items[i - 1], item) == 0)
        {
            if (filter (items[i]))
            {
                return items[i];
            }

            i++;
        }

        return default!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> FindAll<T> (this List<T> items, T item, IComparer<T> comparer)
    {
        var i = items.BinarySearch (item, comparer);
        if (i < 0 || i > items.Count - 1)
        {
            yield break;
        }

        while (i > 0 && comparer.Compare (items[i - 1], item) == 0)
        {
            i--;
        }

        while (i < items.Count && comparer.Compare (items[i], item) == 0)
        {
            yield return items[i];
            i++;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <param name="comparer"></param>
    /// <param name="filter"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? FindSimilar<T>
        (
            this List<T> items,
            T item, IComparer<T> comparer,
            Predicate<T> filter
        )
    {
        if (items.Count == 0)
        {
            return default;
        }

        var i = items.BinarySearch (item, comparer);
        if (i >= 0 && i < items.Count)
        {
            if (filter (items[i]))
            {
                return FindOne (items, item, comparer, filter);
            }
        }
        else
        {
            i = -i - 1;
        }

        //go top
        var candidate1 = -1;
        var j = i - 1;
        while (j >= 0)
        {
            if (filter (items[j]))
            {
                candidate1 = j;
                break;
            }

            j--;
        }

        //go down
        var candidate2 = -1;
        while (i < items.Count)
        {
            if (filter (items[i]))
            {
                candidate2 = i;
                break;
            }

            i++;
        }

        //choose better
        if (candidate1 == -1 && candidate2 == -1)
        {
            return default;
        }

        if (candidate1 == -1)
        {
            return Math.Abs (comparer.Compare (items[candidate2], item)) > 1 ? items[candidate2] : default;
        }

        if (candidate2 == -1)
        {
            return Math.Abs (comparer.Compare (items[candidate1], item)) > 1 ? items[candidate1] : default;
        }

        var like1 = Math.Abs (comparer.Compare (items[candidate1], item));
        var like2 = Math.Abs (comparer.Compare (items[candidate2], item));

        var candidate = like1 > like2 ? candidate1 : candidate2;

        return Math.Max (like1, like2) > 1 ? items[candidate] : default;
    }
}
