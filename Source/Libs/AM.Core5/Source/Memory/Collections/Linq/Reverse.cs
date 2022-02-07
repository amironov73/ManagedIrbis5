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

#region Using directives

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

public static partial class PoolingEnumerable
{
    /// <summary>
    /// Returns sequence with backward direction. Complexity = 2 * O(N) (collect + return)
    /// </summary>
    public static IPoolingEnumerable<T> Reverse<T> (this IPoolingEnumerable<T> source)
    {
        var list = Pool<PoolingList<T>>.Get().Init();
        foreach (var item in source)
        {
            list.Add (item);
        }

        return Pool<ReverseExprEnumerable<T>>.Get().Init (list);
    }
}