// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LastLastOrDefault.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

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
    /// <exception cref="InvalidOperationException"></exception>
    public static T Last<T>
        (
            this IPoolingEnumerable<T> source
        )
    {
        var enumerator = source.GetEnumerator();
        T element = default!;
        var hasItems = false;
        while (enumerator.MoveNext())
        {
            element = enumerator.Current;
            hasItems = true;
        }

        enumerator.Dispose();
        return hasItems ? element : throw new InvalidOperationException ("Sequence is empty");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T Last<T>
        (
            this IPoolingEnumerable<T> source,
            Func<T, bool> condition
        )
    {
        var enumerator = source.GetEnumerator();
        T element = default!;
        var hasItems = false;
        while (enumerator.MoveNext())
        {
            if (!condition (enumerator.Current)) continue;

            element = enumerator.Current;
            hasItems = true;
        }

        enumerator.Dispose();
        return hasItems ? element : throw new InvalidOperationException ("Sequence is empty");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="context"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T Last<T, TContext>
        (
            this IPoolingEnumerable<T> source,
            TContext context,
            Func<TContext, T, bool> condition
        )
    {
        var enumerator = source.GetEnumerator();
        T element = default!;
        var hasItems = false;
        while (enumerator.MoveNext())
        {
            if (!condition (context, enumerator.Current)) continue;

            element = enumerator.Current;
            hasItems = true;
        }

        enumerator.Dispose();
        return hasItems ? element : throw new InvalidOperationException ("Sequence is empty");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LastOrDefault<T>
        (
            this IPoolingEnumerable<T> source
        )
    {
        var enumerator = source.GetEnumerator();
        T element = default!;
        var hasItems = false;
        while (enumerator.MoveNext())
        {
            element = enumerator.Current;
            hasItems = true;
        }

        enumerator.Dispose();
        return hasItems ? element : default!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LastOrDefault<T>
        (
            this IPoolingEnumerable<T> source,
            Func<T, bool> condition
        )
    {
        var enumerator = source.GetEnumerator();
        T element = default!;
        var hasItems = false;
        while (enumerator.MoveNext())
        {
            if (!condition (enumerator.Current)) continue;

            element = enumerator.Current;
            hasItems = true;
        }

        enumerator.Dispose();
        return hasItems ? element : default!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="context"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static T LastOrDefault<T, TContext>
        (
            this IPoolingEnumerable<T> source,
            TContext context,
            Func<TContext, T, bool> condition
        )
    {
        var enumerator = source.GetEnumerator();
        T element = default!;
        var hasItems = false;
        while (enumerator.MoveNext())
        {
            if (!condition (context, enumerator.Current)) continue;

            element = enumerator.Current;
            hasItems = true;
        }

        enumerator.Dispose();
        return hasItems ? element : default!;
    }
}
