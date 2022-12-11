// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FirstFirstOrDefault.cs --
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
    /// Gets first element from sequence. Complexity = O(1)
    /// </summary>
    public static T First<T> (this IPoolingEnumerable<T> source)
    {
        var enumerator = source.GetEnumerator();
        var hasItems = enumerator.MoveNext();
        if (!hasItems)
        {
            throw new InvalidOperationException ("Sequence is empty");
        }

        var element = enumerator.Current;
        enumerator.Dispose();
        return element;
    }

    /// <summary>
    /// Gets first element from sequence by given <paramref name="condition"/>. Complexity = O(1) - O(N)
    /// </summary>
    public static T First<T> (this IPoolingEnumerable<T> source, Func<T, bool> condition)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (condition (enumerator.Current))
            {
                var item = enumerator.Current;
                enumerator.Dispose();
                return item;
            }
        }

        enumerator.Dispose();
        throw new InvalidOperationException ("Sequence is empty");
    }

    /// <summary>
    /// Gets first element from sequence by given <paramref name="condition"/>. Complexity = O(1) - O(N)
    /// </summary>
    public static T First<T, TContext> (this IPoolingEnumerable<T> source, TContext context,
        Func<TContext, T, bool> condition)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!condition (context, enumerator.Current)) continue;

            var item = enumerator.Current;
            enumerator.Dispose();
            return item;
        }

        enumerator.Dispose();
        throw new InvalidOperationException ("Sequence is empty");
    }

    /// <summary>
    /// Gets first element from sequence. Complexity = O(1)
    /// </summary>
    public static T FirstOrDefault<T> (this IPoolingEnumerable<T> source)
    {
        var enumerator = source.GetEnumerator();
        var hasItem = enumerator.MoveNext();

        var item = hasItem ? enumerator.Current : default;
        enumerator.Dispose();

        return item!;
    }

    /// <summary>
    /// Gets first element from sequence by given <paramref name="condition"/>. Complexity = O(1) - O(N)
    /// </summary>
    public static T FirstOrDefault<T> (this IPoolingEnumerable<T> source, Func<T, bool> condition)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!condition (enumerator.Current)) continue;

            var elem = enumerator.Current;
            enumerator.Dispose();
            return elem;
        }

        enumerator.Dispose();
        return default!;
    }

    /// <summary>
    /// Gets first element from sequence by given <paramref name="condition"/>. Complexity = O(1) - O(N)
    /// </summary>
    public static T FirstOrDefault<T, TContext> (this IPoolingEnumerable<T> source, TContext context,
        Func<TContext, T, bool> condition)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (!condition (context, enumerator.Current)) continue;

            var elem = enumerator.Current;
            enumerator.Dispose();
            return elem;
        }

        enumerator.Dispose();
        return default!;
    }
}
