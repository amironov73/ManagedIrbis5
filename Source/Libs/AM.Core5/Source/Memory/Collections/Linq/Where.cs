// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Where.cs --
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
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> Where<T> (this IPoolingEnumerable<T> source, Func<T, bool> condition) =>
        Pool<WhereExprEnumerable<T>>.Get().Init (source, condition);

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <param name="context"></param>
    /// <param name="condition"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    /// <returns></returns>
    public static IPoolingEnumerable<T> Where<T, TContext> (this IPoolingEnumerable<T> source, TContext context,
        Func<TContext, T, bool> condition) =>
        Pool<WhereExprWithContextEnumerable<T, TContext>>.Get().Init (source, context, condition);
}
