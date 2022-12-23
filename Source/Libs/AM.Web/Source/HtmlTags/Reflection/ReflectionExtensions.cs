// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ReflectionExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public static class ReflectionExtensions
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="root"></param>
    /// <param name="expression"></param>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <returns></returns>
    public static T2 ValueOrDefault<T1, T2>
        (
            this T1? root,
            Expression<Func<T1, T2>> expression
        )
        where T1: class
    {
        Sure.NotNull (expression);

        if (root is null)
        {
            return default!;
        }

        var accessor = ReflectionHelper.GetAccessor (expression);
        var result = accessor.GetValue (root);

        return ((T2?)result ?? default (T2))!;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetAllAttributes<T>
        (
            this IAccessor accessor
        )
        where T: Attribute
    {
        Sure.NotNull (accessor);

        return accessor.InnerProperty.ThrowIfNull().GetCustomAttributes<T>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void ForAttribute<T>
        (
            this IAccessor accessor,
            Action<T> action
        )
        where T: Attribute
    {
        Sure.NotNull (accessor);
        Sure.NotNull (action);

        var innerProperty = accessor.InnerProperty.ThrowIfNull();
        foreach (T attribute in innerProperty.GetCustomAttributes (typeof (T), true))
        {
            action (attribute);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="provider"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetAttribute<T>
        (
            this IAccessor provider
        )
        where T: Attribute
    {
        Sure.NotNull (provider);

        return provider.InnerProperty.ThrowIfNull().GetCustomAttribute<T>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="provider"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool HasAttribute<T>
        (
            this IAccessor provider
        )
        where T: Attribute
    {
        return provider.InnerProperty.ThrowIfNull().GetCustomAttribute<T>() is not null;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static IAccessor ToAccessor<T, TResult>
        (
            this Expression<Func<T, TResult>> expression
        )
    {
        Sure.NotNull (expression);

        return ReflectionHelper.GetAccessor (expression);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetName<T>
        (
            this Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        return ReflectionHelper.GetAccessor (expression).Name;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="action"></param>
    /// <typeparam name="T"></typeparam>
    public static void IfPropertyTypeIs<T>
        (
            this IAccessor accessor,
            Action action
        )
    {
        Sure.NotNull (accessor);
        Sure.NotNull (action);

        if (accessor.PropertyType == typeof (T))
        {
            action();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accessor"></param>
    /// <returns></returns>
    public static bool IsInteger
        (
            this IAccessor accessor
        )
    {
        Sure.NotNull (accessor);

        var propertyType = accessor.PropertyType.ThrowIfNull();
        return propertyType.IsTypeOrNullableOf<int>() ||
               propertyType.IsTypeOrNullableOf<long>();
    }

    #endregion
}
