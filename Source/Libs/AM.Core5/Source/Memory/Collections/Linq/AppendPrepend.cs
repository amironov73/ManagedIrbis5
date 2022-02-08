// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AppendPrepend.cs -- методы Append и Prepend
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    /// <inheritdoc cref="Enumerable.Prepend{TSource}"/>
    public static IPoolingEnumerable<T> Prepend<T>
        (
            this IPoolingEnumerable<T> source,
            T element
        )
    {
        Sure.NotNull (source);

        return Pool<PrependExprEnumerable<T>>.Get().Init (source, element);
    }

    /// <inheritdoc cref="Enumerable.Append{TSource}"/>
    public static IPoolingEnumerable<T> Append<T>
        (
            this IPoolingEnumerable<T> source,
            T element
        )
    {
        Sure.NotNull (source);

        return Pool<AppendExprEnumerable<T>>.Get().Init (source, element);
    }
}
