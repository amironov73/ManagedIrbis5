// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    public static IPoolingEnumerable<T> Prepend<T> (this IPoolingEnumerable<T> source, T element) =>
        Pool<PrependExprEnumerable<T>>.Get().Init (source, element);

    public static IPoolingEnumerable<T> Append<T> (this IPoolingEnumerable<T> source, T element) =>
        Pool<AppendExprEnumerable<T>>.Get().Init (source, element);
}