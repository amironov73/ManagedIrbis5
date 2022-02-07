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
        /// <summary>
        /// Casts all elements to the given type. Complexity = O(N)
        /// </summary>
        public static IPoolingEnumerable<TR> Cast<TR>(this IPoolingEnumerable source)
        {
	        if (source is IPoolingEnumerable<TR> res) return res;
            return Pool<CastExprEnumerable<TR>>.Get().Init(source);
        }
    }
}