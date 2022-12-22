// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IAccessor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public interface IAccessor
{
    /// <summary>
    /// /
    /// </summary>
    Type? PropertyType { get; }

    /// <summary>
    ///
    /// </summary>
    PropertyInfo? InnerProperty { get; }

    /// <summary>
    ///
    /// </summary>
    Type? DeclaringType { get; }

    /// <summary>
    ///
    /// </summary>
    string Name { get; }

    /// <summary>
    ///
    /// </summary>
    Type? OwnerType { get; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="propertyValue"></param>
    void SetValue (object target, object? propertyValue);

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    object? GetValue (object target);

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IAccessor GetChildAccessor<T> (Expression<Func<T, object>> expression);

    /// <summary>
    ///
    /// </summary>
    string[] PropertyNames { get; }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Expression<Func<T, object>> ToExpression<T>();

    /// <summary>
    ///
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    IAccessor Prepend (PropertyInfo property);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    IEnumerable<IValueGetter> Getters();
}

/// <summary>
/// /
/// </summary>
public static class AccessorExtensions
{
    #region Public emthods

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="prefixedAccessor"></param>
    /// <returns></returns>
    public static IAccessor Prepend
        (
            this IAccessor accessor,
            IAccessor prefixedAccessor
        )
    {
        Sure.NotNull (accessor);
        Sure.NotNull (prefixedAccessor);

        return new PropertyChain (prefixedAccessor.Getters().Union (accessor.Getters()).ToArray());
    }

    #endregion
}
