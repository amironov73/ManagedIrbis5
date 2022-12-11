// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Sum.cs --
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
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int Sum
        (
            this IPoolingEnumerable<int> source
        )
    {
        Sure.NotNull ((object?) source);
        var sum = 0;
        checked
        {
            foreach (var v in source)
            {
                sum += v;
            }
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static int? Sum
        (
            this IPoolingEnumerable<int?> source
        )
    {
        Sure.NotNull ((object?) source);
        var sum = 0;
        checked
        {
            foreach (var v in source)
            {
                if (v != null)
                {
                    sum += v.GetValueOrDefault();
                }
            }
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static long Sum
        (
            this IPoolingEnumerable<long> source
        )
    {
        Sure.NotNull ((object?) source);

        long sum = 0;
        checked
        {
            foreach (var v in source)
            {
                sum += v;
            }
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static long? Sum
        (
            this IPoolingEnumerable<long?> source
        )
    {
        Sure.NotNull ((object?) source);

        long sum = 0;
        checked
        {
            foreach (var v in source)
            {
                if (v != null)
                {
                    sum += v.GetValueOrDefault();
                }
            }
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static float Sum
        (
            this IPoolingEnumerable<float> source
        )
    {
        Sure.NotNull ((object?) source);

        double sum = 0;
        foreach (var v in source)
        {
            sum += v;
        }

        return (float)sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static float? Sum
        (
            this IPoolingEnumerable<float?> source
        )
    {
        Sure.NotNull ((object?) source);

        double sum = 0;
        foreach (var v in source)
        {
            if (v != null)
            {
                sum += v.GetValueOrDefault();
            }
        }

        return (float)sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static double Sum
        (
            this IPoolingEnumerable<double> source
        )
    {
        Sure.NotNull ((object?) source);

        double sum = 0;
        foreach (var v in source)
        {
            sum += v;
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static double? Sum
        (
            this IPoolingEnumerable<double?> source
        )
    {
        Sure.NotNull ((object?) source);

        double sum = 0;
        foreach (var v in source)
        {
            if (v != null)
            {
                sum += v.GetValueOrDefault();
            }
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static decimal Sum
        (
            this IPoolingEnumerable<decimal> source
        )
    {
        Sure.NotNull ((object?) source);

        decimal sum = 0;
        foreach (var v in source)
        {
            sum += v;
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static decimal? Sum
        (
            this IPoolingEnumerable<decimal?> source
        )
    {
        Sure.NotNull ((object?) source);

        decimal sum = 0;
        foreach (var v in source)
        {
            if (v != null)
            {
                sum += v.GetValueOrDefault();
            }
        }

        return sum;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static int Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, int> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static int? Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, int?> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static long Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, long> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static long? Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, long?> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static float Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, float> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static float? Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, float?> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static double Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, double> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static double? Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, double?> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static decimal Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, decimal> selector
        )
    {
        return Sum (source.Select (selector));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <returns></returns>
    public static decimal? Sum<TSource>
        (
            this IPoolingEnumerable<TSource> source,
            Func<TSource, decimal?> selector
        )
    {
        return Sum (source.Select (selector));
    }
}
