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

/* SingleOrDefault.cs --
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
    public static T Single<T>
        (
            this IPoolingEnumerable<T> source
        )
    {
        var wasFound = false;
        var element = default (T);
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (wasFound)
            {
                enumerator.Dispose();
                throw new InvalidOperationException ("Sequence should contain only one element");
            }

            wasFound = true;
            element = enumerator.Current;
        }

        enumerator.Dispose();
        return element!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static T Single<T>
        (
            this IPoolingEnumerable<T> source,
            Func<T, bool> condition
        )
    {
        var wasFound = false;
        var element = default (T);
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (condition (enumerator.Current))
            {
                if (wasFound)
                {
                    enumerator.Dispose();
                    throw new InvalidOperationException ("Sequence should contain only one element");
                }

                wasFound = true;
                element = enumerator.Current;
            }
        }

        enumerator.Dispose();
        return element!;
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
    public static T Single<T, TContext>
        (
            this IPoolingEnumerable<T> source,
            TContext context,
            Func<TContext, T, bool> condition
        )
    {
        var wasFound = false;
        var element = default (T);
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (condition (context, enumerator.Current))
            {
                if (wasFound)
                {
                    enumerator.Dispose();
                    throw new InvalidOperationException ("Sequence should contain only one element");
                }

                wasFound = true;
                element = enumerator.Current;
            }
        }

        enumerator.Dispose();
        return element!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T SingleOrDefault<T>
        (
            this IPoolingEnumerable<T> source
        )
    {
        var wasFound = false;
        var element = default (T);
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (wasFound)
            {
                enumerator.Dispose();
                return default!;
            }

            wasFound = true;
            element = enumerator.Current;
        }

        enumerator.Dispose();
        return element!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T SingleOrDefault<T>
        (
            this IPoolingEnumerable<T> source,
            Func<T, bool> condition
        )
    {
        var wasFound = false;
        var element = default (T);
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (condition (enumerator.Current))
            {
                if (wasFound)
                {
                    enumerator.Dispose();
                    return default!;
                }

                wasFound = true;
                element = enumerator.Current;
            }
        }

        enumerator.Dispose();
        return element!;
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
    public static T SingleOrDefault<T, TContext>
        (
            this IPoolingEnumerable<T> source,
            TContext context,
            Func<TContext, T, bool> condition
        )
    {
        var wasFound = false;
        var element = default (T);
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (condition (context, enumerator.Current))
            {
                if (wasFound)
                {
                    enumerator.Dispose();
                    return default!;
                }

                wasFound = true;
                element = enumerator.Current;
            }
        }

        enumerator.Dispose();
        return element!;
    }
}
