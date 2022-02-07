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
        public static bool Any<T>(this IPoolingEnumerable<T> source)
        {
            var enumerator = source.GetEnumerator();
            var hasItems = enumerator.MoveNext();
            enumerator.Dispose();
            return hasItems;
        }
        
        public static bool Any<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(enumerator.Current))
                {
                    enumerator.Dispose();
                    return true;
                }
            }
            enumerator.Dispose();
            return false;
        }
		
        public static bool Any<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (condition(context, enumerator.Current))
                {
                    enumerator.Dispose();
                    return true;
                }
            }
            enumerator.Dispose();
            return false;
        }
        
        public static bool All<T>(this IPoolingEnumerable<T> source, Func<T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!condition(enumerator.Current))
                {
                    enumerator.Dispose();
                    return false;
                }
            }
            enumerator.Dispose();
            return true;

        }
		
        public static bool All<T, TContext>(this IPoolingEnumerable<T> source, TContext context, Func<TContext, T, bool> condition)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (!condition(context, enumerator.Current))
                {
                    enumerator.Dispose();
                    return false;
                }
            }
            enumerator.Dispose();
            return true;
        }
    }
}