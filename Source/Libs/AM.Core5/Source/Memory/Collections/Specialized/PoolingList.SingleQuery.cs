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

/* PoolingList.SingleQuery
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Memory.Collections.Specialized;

public static partial class AsSingleQueryList
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="src"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> AsSingleEnumerableList<T>
        (
            this IEnumerable<T> src
        )
    {
        var list = Pool<PoolingList<T>>.Get().Init();
        foreach (var item in src)
        {
            list.Add (item);
        }

        return Pool<EnumerableTyped<T>>.Get().Init (list);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="src"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>
        (
            this IEnumerable<T> src
        )
        where T : class
    {
        var list = Pool<PoolingListCanon<T>>.Get().Init();
        foreach (var item in src)
        {
            list.Add (item);
        }

        return Pool<EnumerableShared<T>>.Get().Init (list);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="src"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> AsSingleEnumerableList<T>
        (
            this IPoolingEnumerable<T> src
        )
    {
        var list = Pool<PoolingList<T>>.Get().Init();
        foreach (var item in src)
        {
            list.Add (item);
        }

        return Pool<EnumerableTyped<T>>.Get().Init (list);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="src"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> AsSingleEnumerableSharedList<T>
        (
            this IPoolingEnumerable<T> src
        )
        where T : class
    {
        var list = Pool<PoolingListCanon<T>>.Get().Init();
        foreach (var item in src)
        {
            list.Add (item);
        }

        return Pool<EnumerableShared<T>>.Get().Init (list);
    }
}
