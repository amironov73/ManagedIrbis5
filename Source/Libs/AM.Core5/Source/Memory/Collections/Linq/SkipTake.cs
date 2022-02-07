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

namespace AM.Memory.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Skip<T>(this IPoolingEnumerable<T> source, int count)
        {
            return Pool<SkipTakeExprPoolingEnumerable<T>>.Get().Init(source, false, count);
        }

        public static IPoolingEnumerable<T> Take<T>(this IPoolingEnumerable<T> source, int count)
        {
            return Pool<SkipTakeExprPoolingEnumerable<T>>.Get().Init(source, true, count);
        }
    }
}