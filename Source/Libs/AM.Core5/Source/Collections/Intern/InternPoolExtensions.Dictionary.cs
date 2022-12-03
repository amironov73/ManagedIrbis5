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

/* InternPoolExtensions.Dictionary.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Collections.Intern;

/// <summary>
///
/// </summary>
public static partial class InternPoolExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static string[] ToInternedArray
        (
            this string[] array
        )
    {
        var pool = InternPool.Shared;

        var newArray = new string[array.Length];
        for (var i = 0; i < array.Length; i++)
        {
            newArray[i] = pool.Intern (array[i]);
        }

        return newArray;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static string Intern
        (
            this StringBuilder builder
        )
    {
        var count = builder.Length;
        char[]? array = null;
#if NET5_0 || NETCOREAPP3_1
            if (count > InternPool.StackAllocThresholdChars)
            {
                array = ArrayPool<char>.Shared.Rent(count);
            }
#if NET5_0
            Span<char> span = array is null ? stackalloc char[InternPool.StackAllocThresholdChars] : array;
#else
            Span<char> span = array is null ? stackalloc char[count] : array;
#endif
            sb.CopyTo(0, span, count);
            span = span.Slice(0, count);
#else
        array = ArrayPool<char>.Shared.Rent (count);
        builder.CopyTo (0, array, 0, count);

        var span = array.AsSpan (0, count);
#endif
        var result = InternPool.Shared.Intern (span);
        if (array != null)
        {
            ArrayPool<char>.Shared.Return (array);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<string> ToInternedList
        (
            this List<string> list
        )
    {
        var pool = InternPool.Shared;

        var internedList = new List<string> (list.Count);
        var count = list.Count;
        for (var i = 0; i < count; i++)
        {
            internedList.Add (pool.Intern (list[i]));
        }

        return internedList;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<string> ToInternedList
        (
            this IList<string> list
        )
    {
        var pool = InternPool.Shared;

        var internedList = new List<string> (list.Count);
        var count = list.Count;
        for (var i = 0; i < count; i++)
        {
            internedList.Add (pool.Intern (list[i]));
        }

        return internedList;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="collection"></param>
    /// <returns></returns>
    public static List<string> ToInternedList
        (
            this ICollection<string> collection
        )
    {
        var pool = InternPool.Shared;

        var internedList = new List<string> (collection.Count);
        foreach (var item in collection)
        {
            internedList.Add (pool.Intern (item));
        }

        return internedList;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ToInternedDictionary
        (
            this Dictionary<string, string> dict
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new Dictionary<string, string> (dict.Count, dict.Comparer);
        foreach (var kv in dict)
        {
            internedDict.Add (pool.Intern (kv.Key), pool.Intern (kv.Value));
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static Dictionary<string, TValue> ToInternedDictionary<TValue>
        (
            this Dictionary<string, TValue> dict
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new Dictionary<string, TValue> (dict.Count, dict.Comparer);
        foreach (var kv in dict)
        {
            internedDict.Add (pool.Intern (kv.Key), kv.Value);
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static Dictionary<TKey, string> ToInternedDictionary<TKey>
        (
            this Dictionary<TKey, string> dict
        )
        where TKey : notnull
    {
        var pool = InternPool.Shared;

        var internedDict = new Dictionary<TKey, string> (dict.Count, dict.Comparer);
        foreach (var kv in dict)
        {
            internedDict.Add (kv.Key, pool.Intern (kv.Value));
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static Dictionary<string, string> ToInternedDictionary
        (
            this IDictionary<string, string> dict,
            IEqualityComparer<string> comparer
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new Dictionary<string, string> (dict.Count, comparer);
        foreach (var kv in dict)
        {
            internedDict.Add (pool.Intern (kv.Key), pool.Intern (kv.Value));
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static Dictionary<string, TValue> ToInternedDictionary<TValue>
        (
            this IDictionary<string, TValue> dict,
            IEqualityComparer<string> comparer
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new Dictionary<string, TValue> (dict.Count, comparer);
        foreach (var kv in dict)
        {
            internedDict.Add (pool.Intern (kv.Key), kv.Value);
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static Dictionary<TKey, string> ToInternedDictionary<TKey>
        (
            this IDictionary<TKey, string> dict,
            IEqualityComparer<TKey> comparer
        )
        where TKey: notnull
    {
        var pool = InternPool.Shared;

        var internedDict = new Dictionary<TKey, string> (dict.Count, comparer);
        foreach (var kv in dict)
        {
            internedDict.Add (kv.Key, pool.Intern (kv.Value));
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static ConcurrentDictionary<string, string> ToInternedConcurrentDictionary
        (
            this Dictionary<string, string> dict
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new ConcurrentDictionary<string, string> (dict.Comparer);
        foreach (var kv in dict)
        {
            internedDict.TryAdd (pool.Intern (kv.Key)!, pool.Intern (kv.Value)!);
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static ConcurrentDictionary<string, TValue> ToInternedConcurrentDictionary<TValue>
        (
            this Dictionary<string, TValue> dict
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new ConcurrentDictionary<string, TValue> (dict.Comparer);
        foreach (var kv in dict)
        {
            internedDict.TryAdd (pool.Intern (kv.Key), kv.Value);
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static ConcurrentDictionary<TKey, string> ToInternedConcurrentDictionary<TKey>
        (
            this Dictionary<TKey, string> dict
        )
        where TKey: notnull
    {
        var pool = InternPool.Shared;

        var internedDict = new ConcurrentDictionary<TKey, string> (dict.Comparer);
        foreach (var kv in dict)
        {
            internedDict.TryAdd (kv.Key, pool.Intern (kv.Value));
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static ConcurrentDictionary<string, string> ToInternedConcurrentDictionary
        (
            this IDictionary<string, string> dict,
            IEqualityComparer<string> comparer
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new ConcurrentDictionary<string, string> (comparer);
        foreach (var kv in dict)
        {
            internedDict.TryAdd (pool.Intern (kv.Key), pool.Intern (kv.Value));
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static ConcurrentDictionary<string, TValue> ToInternedConcurrentDictionary<TValue>
        (
            this IDictionary<string, TValue> dict,
            IEqualityComparer<string> comparer
        )
    {
        var pool = InternPool.Shared;

        var internedDict = new ConcurrentDictionary<string, TValue> (comparer);
        foreach (var kv in dict)
        {
            internedDict.TryAdd (pool.Intern (kv.Key)!, kv.Value);
        }

        return internedDict;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="comparer"></param>
    /// <typeparam name="TKey"></typeparam>
    /// <returns></returns>
    public static ConcurrentDictionary<TKey, string> ToInternedConcurrentDictionary<TKey>
        (
            this IDictionary<TKey, string> dict,
            IEqualityComparer<TKey> comparer
        )
        where TKey : notnull
    {
        var pool = InternPool.Shared;

        var internedDict = new ConcurrentDictionary<TKey, string> (comparer);
        foreach (var kv in dict)
        {
            internedDict.TryAdd (kv.Key, pool.Intern (kv.Value));
        }

        return internedDict;
    }
}
