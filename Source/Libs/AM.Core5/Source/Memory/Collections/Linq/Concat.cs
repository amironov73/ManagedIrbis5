// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Concat.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    /// <summary>
    /// Returns all elements from <paramref name="source"/> and all --
    /// from <paramref name="second"/>. Complexity = O(N+M)
    /// </summary>
    public static IPoolingEnumerable<T> Concat<T>
        (
            this IPoolingEnumerable<T> source,
            IPoolingEnumerable<T> second
        )
    {
        return Pool<ConcatExprEnumerable<T>>.Get().Init (source, second);
    }
}
