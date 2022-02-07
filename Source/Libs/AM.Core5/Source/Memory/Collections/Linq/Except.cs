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

using System.Collections.Generic;

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except)
        {
            var exceptDict = Pool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in except) exceptDict[item] = 1;
            
            return Pool<ExceptExprEnumerable<T>>.Get().Init(source, exceptDict);
        } 
        
        public static IPoolingEnumerable<T> Except<T>(this IPoolingEnumerable<T> source, IPoolingEnumerable<T> except, IEqualityComparer<T> comparer)
        {
            var exceptDict = Pool<PoolingDictionary<T, int>>.Get().Init(0);
            foreach (var item in except) exceptDict[item] = 1;

            return Pool<ExceptExprEnumerable<T>>.Get().Init(source, exceptDict, comparer);
        }
    }
}