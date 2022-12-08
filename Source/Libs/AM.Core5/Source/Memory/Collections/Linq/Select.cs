// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Select.cs --
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
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="mutator"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TR"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<TR> Select<T, TR> (this IPoolingEnumerable<T> source, Func<T, TR> mutator)
    {
        return Pool<SelectExprEnumerable<T, TR>>.Get().Init (source, mutator);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="context"></param>
    /// <param name="mutator"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TR"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<TR> Select<T, TR, TContext> (this IPoolingEnumerable<T> source,
        TContext context, Func<TContext, T, TR> mutator)
    {
        return Pool<SelectExprWithContextEnumerable<T, TR, TContext>>.Get().Init (source, context, mutator);
    }
}
