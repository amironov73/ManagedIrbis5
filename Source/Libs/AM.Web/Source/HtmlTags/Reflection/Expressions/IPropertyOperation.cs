// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IPropertyOperation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection.Expressions;

/// <summary>
///
/// </summary>
public interface IPropertyOperation
{
    /// <summary>
    ///
    /// </summary>
    string OperationName { get; }

    /// <summary>
    ///
    /// </summary>
    string Text { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyPath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Func<object, Expression<Func<T, bool>>> GetPredicateBuilder<T>
        (
            MemberExpression propertyPath
        );
}
