// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Aggregate.cs -- агрегация
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

/// <summary>
/// Набор расширений для <see cref="IPoolingEnumerable{T}"/>
/// </summary>
public static partial class PoolingEnumerable
{
    /// <inheritdoc cref="Enumerable.Aggregate{TSource}"/>
    public static TSource Aggregate<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TSource, TSource> func
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (func);

        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException ("Sequence contains no elements");
        }

        var result = enumerator.Current;
        while (enumerator.MoveNext())
        {
            result = func (result, enumerator.Current);
        }

        return result;
    }

    /// <inheritdoc cref="Enumerable.Aggregate{TSource}"/>
    public static TAccumulate Aggregate<TSource, TAccumulate>
        (
            this IPoolingEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (func);

        var result = seed;
        foreach (var element in source)
        {
            result = func (result, element);
        }

        return result;
    }

    /// <inheritdoc cref="Enumerable.Aggregate{TSource}"/>
    public static TResult Aggregate<TSource, TAccumulate, TResult>
        (
            this IPoolingEnumerable<TSource> source,
            TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func,
            Func<TAccumulate, TResult> resultSelector
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (func);
        Sure.NotNull (resultSelector);

        var result = seed;
        foreach (var element in source)
        {
            result = func (result, element);
        }

        return resultSelector (result);
    }
}
