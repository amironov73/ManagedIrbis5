// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* GroupBy.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<IPoolingGrouping<TKey, TSource>> GroupBy<TSource, TKey>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector
        )
    {
        return Pool<GroupedEnumerable<TSource, TKey, TSource>>.Get()
            .Init (source, keySelector, x => x, null!);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<IPoolingGrouping<TKey, TSource>> GroupBy<TSource, TKey>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer
        )
    {
        return Pool<GroupedEnumerable<TSource, TKey, TSource>>.Get()
            .Init (source, keySelector, x => x, comparer);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="elementSelector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<IPoolingGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector
        )
    {
        return Pool<GroupedEnumerable<TSource, TKey, TElement>>.Get()
            .Init (source, keySelector, elementSelector, null!);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="elementSelector"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TElement"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<IPoolingGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer
        )
    {
        return Pool<GroupedEnumerable<TSource, TKey, TElement>>.Get()
            .Init (source, keySelector, elementSelector, comparer);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="resultSelector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<TResult> GroupBy<TSource, TKey, TResult>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IPoolingEnumerable<TSource>, TResult> resultSelector
        )
    {
        return Pool<GroupedResultEnumerable<TSource, TKey, TResult>>.Get()
            .Init (source, keySelector, resultSelector, null!);
    }

    // public static IPoolingEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector) =>
    //     new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, null);

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="resultSelector"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<TResult> GroupBy<TSource, TKey, TResult>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IPoolingEnumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer
        )
    {
        return Pool<GroupedResultEnumerable<TSource, TKey, TResult>>.Get()
            .Init (source, keySelector, resultSelector, comparer);
    }

    // public static IEnumerable<TResult> GroupBy<TSource, TKey, TElement, TResult>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer) =>
    //     new GroupedResultEnumerable<TSource, TKey, TElement, TResult>(source, keySelector, elementSelector, resultSelector, comparer);
}
