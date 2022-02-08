// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AnyAll.cs -- методы Any и All
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    /// <inheritdoc cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
    public static bool Any<T>
        (
            this IPoolingEnumerable<T> source
        )
    {
        var enumerator = source.GetEnumerator();
        var hasItems = enumerator.MoveNext();
        enumerator.Dispose();
        return hasItems;
    }

    /// <inheritdoc cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
    public static bool Any<T>
        (
            this IPoolingEnumerable<T> source,
            Func<T, bool> condition
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (condition);

        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (condition (enumerator.Current))
            {
                enumerator.Dispose();
                return true;
            }
        }

        enumerator.Dispose();
        return false;
    }

    /// <inheritdoc cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
    public static bool Any<T, TContext>
        (
            this IPoolingEnumerable<T> source,
            TContext? context,
            Func<TContext?, T, bool> condition
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (condition);

        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (condition (context, enumerator.Current))
            {
                enumerator.Dispose();
                return true;
            }
        }

        enumerator.Dispose();

        return false;
    }

    /// <inheritdoc cref="Enumerable.All{TSource}"/>
    public static bool All<T>
        (
            this IPoolingEnumerable<T> source,
            Func<T, bool> condition
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (condition);

        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!condition (enumerator.Current))
            {
                enumerator.Dispose();
                return false;
            }
        }

        enumerator.Dispose();

        return true;
    }

    /// <inheritdoc cref="Enumerable.All{TSource}"/>
    public static bool All<T, TContext>
        (
            this IPoolingEnumerable<T> source,
            TContext? context,
            Func<TContext?, T, bool> condition
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (condition);

        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!condition (context, enumerator.Current))
            {
                enumerator.Dispose();
                return false;
            }
        }

        enumerator.Dispose();

        return true;
    }
}
