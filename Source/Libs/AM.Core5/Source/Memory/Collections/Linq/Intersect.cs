// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Intersect.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="intersectWith"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> Intersect<T>
        (
            this IPoolingEnumerable<T> source,
            IPoolingEnumerable<T> intersectWith
        )
    {
        var second = Pool<PoolingDictionary<T, int>>.Get().Init (0);
        foreach (var item in intersectWith) second[item] = 1;

        return Pool<IntersectExprEnumerable<T>>.Get().Init (source, second);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="intersectWith"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> Intersect<T>
        (
            this IPoolingEnumerable<T> source,
            IPoolingEnumerable<T> intersectWith,
            IEqualityComparer<T> comparer
        )
    {
        var second = Pool<PoolingDictionary<T, int>>.Get().Init (0);
        foreach (var item in intersectWith) second[item] = 1;

        return Pool<IntersectExprEnumerable<T>>.Get().Init (source, second, comparer);
    }
}
