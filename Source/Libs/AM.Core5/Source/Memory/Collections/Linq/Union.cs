// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Union.cs --
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
    /// <param name="second"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> Union<T>
        (
            this IPoolingEnumerable<T> source,
            IPoolingEnumerable<T> second
        )
    {
        var set = Pool<PoolingDictionary<T, int>>.Get()
            .Init (0);
        foreach (var item in source)
        {
            set[item] = 1;
        }

        foreach (var item in second)
        {
            set[item] = 1;
        }

        return Pool<UnionExprEnumerable<T>>.Get().Init (set);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="second"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> Union<T>
        (
            this IPoolingEnumerable<T> source,
            IPoolingEnumerable<T> second,
            IEqualityComparer<T> comparer
        )
    {
        var set = Pool<PoolingDictionary<T, int>>.Get()
            .Init (0, comparer);
        foreach (var item in source)
        {
            set[item] = 1;
        }

        foreach (var item in second)
        {
            set[item] = 1;
        }

        return Pool<UnionExprEnumerable<T>>.Get().Init (set);
    }
}
