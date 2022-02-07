// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* 
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
    /// Returns distinct elements from a sequence by using the default equality comparer to compare values. Complexity - O(N)  
    /// </summary>
    public static IPoolingEnumerable<T> Distinct<T> (this IPoolingEnumerable<T> source) =>
        Pool<DistinctExprEnumerable<T, T>>.Get().Init (source.GetEnumerator(), x => x);

    /// <summary>
    /// Returns distinct elements from a sequence by using the default equality comparer to compare values and selector to select key to compare by. Complexity - O(N)  
    /// </summary>
    public static IPoolingEnumerable<T> DistinctBy<T, TItem> (
        this IPoolingEnumerable<T> source,
        Func<T, TItem> selector) =>
        Pool<DistinctExprEnumerable<T, TItem>>.Get().Init (source.GetEnumerator(), selector);

    /// <summary>
    /// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values. Complexity - O(N)
    /// </summary>
    public static IPoolingEnumerable<T> Distinct<T> (
        this IPoolingEnumerable<T> source,
        IEqualityComparer<T> comparer) =>
        Pool<DistinctExprEnumerable<T, T>>.Get().Init (source.GetEnumerator(), x => x, comparer);

    /// <summary>
    /// Returns distinct elements from a sequence by using a specified <paramref name="comparer"/> to compare values and selector to select key to compare by. Complexity - O(N)
    /// </summary>
    public static IPoolingEnumerable<T> DistinctBy<T, TItem> (
        this IPoolingEnumerable<T> source,
        Func<T, TItem> selector,
        IEqualityComparer<TItem> comparer) =>
        Pool<DistinctExprEnumerable<T, TItem>>.Get().Init (source.GetEnumerator(), selector, comparer);
}