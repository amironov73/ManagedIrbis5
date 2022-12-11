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

/* AsPoolingCollections.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static PoolingList<T> AsPoolingList<T>
        (
            this IEnumerable<T> source
        )
    {
        var collection = Pool<PoolingList<T>>.Get().Init();
        collection.AddRange (source);
        return collection;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static PoolingList<T> AsPoolingList<T>
        (
            this IPoolingEnumerable<T> source
        )
    {
        var collection = Pool<PoolingList<T>>.Get().Init();
        collection.AddRange (source);
        return collection;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    /// <returns></returns>
    public static PoolingDictionary<TK, TV> AsPoolingDictionary<TK, TV>
        (
            this IEnumerable<KeyValuePair<TK, TV>> source
        )
    {
        return AsPoolingDictionary (source.AsPooling());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    /// <returns></returns>
    public static PoolingDictionary<TK, TV> AsPoolingDictionary<TK, TV>
        (
            this IPoolingEnumerable<KeyValuePair<TK, TV>> source
        )
    {
        var collection = Pool<PoolingDictionary<TK, TV>>.Get().Init();
        collection.AddRange (source);
        return collection;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="valueSelector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static PoolingDictionary<TKey, TValue> AsPoolingDictionary<TSource, TKey, TValue>
        (
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector
        )
    {
        return AsPoolingDictionary (source.AsPooling(), keySelector, valueSelector);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <param name="valueSelector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static PoolingDictionary<TKey, TValue> AsPoolingDictionary<TSource, TKey, TValue>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TValue> valueSelector
        )
    {
        var collection = Pool<PoolingDictionary<TKey, TValue>>.Get().Init();
        collection.AddRange
            (
                source
                    .Select ((keySelector, valueSelector),
                        (ctx, x) => new KeyValuePair<TKey, TValue> (ctx.keySelector (x), ctx.valueSelector (x)))
            );
        return collection;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="src"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddRange<T>
        (
            this PoolingList<T> target,
            IEnumerable<T> src
        )
    {
        foreach (var item in src)
        {
            target.Add (item);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="src"></param>
    /// <typeparam name="T"></typeparam>
    public static void AddRange<T>
        (
            this PoolingList<T> target,
            IPoolingEnumerable<T> src
        )
    {
        foreach (var item in src)
        {
            target.Add (item);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="src"></param>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public static void AddRange<TK, TV>
        (
            this PoolingDictionary<TK, TV> target,
            IEnumerable<KeyValuePair<TK, TV>> src
        )
    {
        foreach (var item in src)
        {
            target.Add (item.Key, item.Value);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="src"></param>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public static void AddRange<TK, TV>
        (
            this PoolingDictionary<TK, TV> target,
            IPoolingEnumerable<KeyValuePair<TK, TV>> src
        )
    {
        foreach (var item in src)
        {
            target.Add (item.Key, item.Value);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="src"></param>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public static void AddRangeSafe<TK, TV>
        (
            this PoolingDictionary<TK, TV> target,
            IEnumerable<KeyValuePair<TK, TV>> src
        )
    {
        foreach (var item in src)
        {
            target[item.Key] = item.Value;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="src"></param>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    public static void AddRangeSafe<TK, TV>
        (
            this PoolingDictionary<TK, TV> target,
            IPoolingEnumerable<KeyValuePair<TK, TV>> src
        )
    {
        foreach (var item in src)
        {
            target[item.Key] = item.Value;
        }
    }
}
