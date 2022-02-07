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

using System;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq
{
    public static partial class PoolingEnumerable
    {
        public static T Last<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : throw new InvalidOperationException("Sequence is empty");
        }
        
        public static T Last<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : throw new InvalidOperationException("Sequence is empty");
        }
		
        public static T Last<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(context, enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : throw new InvalidOperationException("Sequence is empty");
        }
        
        public static T LastOrDefault<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : default;
        }
        
        public static T LastOrDefault<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : default;
        }
		
        public static T LastOrDefault<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            T element = default;
            var hasItems = false;
            while (enumerator.MoveNext())
            {
                if (!condition(context, enumerator.Current)) continue;
                
                element = enumerator.Current;
                hasItems = true;
            }
            enumerator.Dispose();
            return hasItems ? element : default;
        }
    }
}