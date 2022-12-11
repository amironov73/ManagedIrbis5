// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MinMax.cs --
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
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Min
        (
            this IPoolingEnumerable<int> source
        )
    {
        Sure.NotNull ((object?) source);

        var value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x < value)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int? Min
        (
            this IPoolingEnumerable<int?> source
        )
    {
        Sure.NotNull ((object?) source);

        int? value = null;
        foreach (var x in source)
        {
            if (value == null || x < value)
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static long Min
        (
            this IPoolingEnumerable<long> source
        )
    {
        Sure.NotNull ((object?) source);

        long value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x < value)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static long? Min
        (
            this IPoolingEnumerable<long?> source
        )
    {
        Sure.NotNull ((object?) source);

        long? value = null;
        foreach (var x in source)
        {
            if (value == null || x < value)
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static float Min
        (
            this IPoolingEnumerable<float> source
        )
    {
        Sure.NotNull ((object?) source);

        float value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                // Normally NaN < anything is false, as is anything < NaN
                // However, this leads to some irksome outcomes in Min and Max.
                // If we use those semantics then Min(NaN, 5.0) is NaN, but
                // Min(5.0, NaN) is 5.0!  To fix this, we impose a total
                // ordering where NaN is smaller than every value, including
                // negative infinity.
                if (x < value || System.Single.IsNaN (x))
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static float? Min
        (
            this IPoolingEnumerable<float?> source
        )
    {
        Sure.NotNull ((object?) source);

        float? value = null;
        foreach (var x in source)
        {
            if (x == null)
            {
                continue;
            }

            if (value == null || x < value || System.Single.IsNaN ((float)x))
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static double Min
        (
            this IPoolingEnumerable<double> source
        )
    {
        Sure.NotNull ((object?) source);

        double value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x < value || Double.IsNaN (x))
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static double? Min
        (
            this IPoolingEnumerable<double?> source
        )
    {
        Sure.NotNull ((object?) source);

        double? value = null;
        foreach (var x in source)
        {
            if (x == null)
            {
                continue;
            }

            if (value == null || x < value || Double.IsNaN ((double)x))
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static decimal Min
        (
            this IPoolingEnumerable<decimal> source
        )
    {
        Sure.NotNull ((object?) source);

        decimal value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x < value)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static decimal? Min
        (
            this IPoolingEnumerable<decimal?> source
        )
    {
        Sure.NotNull ((object?) source);

        decimal? value = null;
        foreach (var x in source)
        {
            if (value == null || x < value)
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static TSource Min<TSource>
        (
            this IPoolingEnumerable<TSource> source
        )
    {
        Sure.NotNull ((object?) source);

        var comparer = Comparer<TSource>.Default;
        var value = default (TSource);
        if (value == null)
        {
            foreach (var x in source)
            {
                if (x != null && (value == null || comparer.Compare (x, value) < 0))
                {
                    value = x;
                }
            }

            return value!;
        }

        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (comparer.Compare (x, value) < 0)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static int Min<TSource> (this IPoolingEnumerable<TSource> source, Func<TSource, int> selector)
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static int? Min<TSource> (this IPoolingEnumerable<TSource> source, Func<TSource, int?> selector)
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static long Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, long> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static long? Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, long?> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static float Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, float> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static float? Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, float?> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static double Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, double> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static double? Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, double?> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static decimal Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, decimal> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static decimal? Min<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, decimal?> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult Min<TSource, TResult>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TResult> selector
        )
    {
        return source.Select (selector).Min();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Max
        (
            this IPoolingEnumerable<int> source
        )
    {
        Sure.NotNull ((object?) source);

        var value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x > value)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int? Max
        (
            this IPoolingEnumerable<int?> source
        )
    {
        Sure.NotNull ((object?) source);

        int? value = null;
        foreach (var x in source)
        {
            if (value == null || x > value)
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static long Max
        (
            this IPoolingEnumerable<long> source
        )
    {
        Sure.NotNull ((object?) source);

        long value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x > value)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static long? Max
        (
            this IPoolingEnumerable<long?> source
        )
    {
        Sure.NotNull ((object?) source);

        long? value = null;
        foreach (var x in source)
        {
            if (value == null || x > value)
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static double Max
        (
            this IPoolingEnumerable<double> source
        )
    {
        Sure.NotNull ((object?) source);

        double value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x > value || Double.IsNaN (value))
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static double? Max
        (
            this IPoolingEnumerable<double?> source
        )
    {
        Sure.NotNull ((object?) source);

        double? value = null;
        foreach (var x in source)
        {
            if (x == null)
            {
                continue;
            }

            if (value == null || x > value || Double.IsNaN ((double)value))
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static float Max
        (
            this IPoolingEnumerable<float> source
        )
    {
        Sure.NotNull ((object?) source);

        float value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x > value || Double.IsNaN (value))
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static float? Max
        (
            this IPoolingEnumerable<float?> source
        )
    {
        Sure.NotNull ((object?) source);

        float? value = null;
        foreach (var x in source)
        {
            if (x == null)
            {
                continue;
            }

            if (value == null || x > value || System.Single.IsNaN ((float)value))
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static decimal Max
        (
            this IPoolingEnumerable<decimal> source
        )
    {
        Sure.NotNull ((object?) source);

        decimal value = 0;
        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (x > value)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static decimal? Max
        (
            this IPoolingEnumerable<decimal?> source
        )
    {
        Sure.NotNull ((object?) source);

        decimal? value = null;
        foreach (var x in source)
        {
            if (value == null || x > value)
            {
                value = x;
            }
        }

        return value;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static TSource Max<TSource>
        (
            this IPoolingEnumerable<TSource> source
        )
    {
        Sure.NotNull ((object?) source);

        var comparer = Comparer<TSource>.Default;
        TSource value = default!;
        if (value == null!)
        {
            foreach (var x in source)
            {
                if (x != null && (value == null || comparer.Compare (x, value) > 0))
                {
                    value = x;
                }
            }

            return value!;
        }

        var hasValue = false;
        foreach (var x in source)
        {
            if (hasValue)
            {
                if (comparer.Compare (x, value) > 0)
                {
                    value = x;
                }
            }
            else
            {
                value = x;
                hasValue = true;
            }
        }

        if (hasValue)
        {
            return value;
        }

        throw new InvalidOperationException ("Sequence contains no elements");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static int Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, int> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static int? Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, int?> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static long Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, long> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static long? Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, long?> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static float Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, float> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static float? Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, float?> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static double Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, double> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static double? Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, double?> selector
        )
    {
        return Max (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static decimal Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, decimal> selector
        )
    {
        return source.Select (selector).Max();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static decimal? Max<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, decimal?> selector
        )
    {
        return source.Select (selector).Max();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static TResult Max<TSource, TResult>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, TResult> selector
        )
    {
        return source.Select (selector).Max();
    }
}
