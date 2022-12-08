// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SelectMany.cs --
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
    public static IPoolingEnumerable<TR> SelectMany<T, TR> (
        this IPoolingEnumerable<T> source,
        Func<T, IPoolingEnumerable<TR>> mutator)
    {
        return Pool<SelectManyExprEnumerable<T, TR>>.Get().Init (source, mutator);
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
    public static IPoolingEnumerable<TR> SelectMany<T, TR, TContext> (
        this IPoolingEnumerable<T> source,
        TContext context,
        Func<T, TContext, IPoolingEnumerable<TR>> mutator)
    {
        return Pool<SelectManyExprWithContextEnumerable<T, TR, TContext>>.Get().Init (source, mutator, context);
    }
}
