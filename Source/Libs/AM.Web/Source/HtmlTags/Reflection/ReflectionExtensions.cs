// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
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

public static class ReflectionExtensions
{
    public static U ValueOrDefault<T, U> (this T root, Expression<Func<T, U>> expression)
        where T : class
    {
        if (root == null)
        {
            return default (U);
        }

        var accessor = ReflectionHelper.GetAccessor (expression);

        var result = accessor.GetValue (root);

        return (U)(result ?? default (U));
    }

    public static IEnumerable<T> GetAllAttributes<T> (this Accessor accessor) where T : Attribute =>
        accessor.InnerProperty.GetCustomAttributes<T>();

    public static void ForAttribute<T> (this Accessor accessor, Action<T> action) where T : Attribute
    {
        foreach (T attribute in accessor.InnerProperty.GetCustomAttributes (typeof (T), true))
        {
            action (attribute);
        }
    }

    public static T GetAttribute<T> (this Accessor provider) where T : Attribute =>
        provider.InnerProperty.GetCustomAttribute<T>();

    public static bool HasAttribute<T> (this Accessor provider) where T : Attribute =>
        provider.InnerProperty.GetCustomAttribute<T>() != null;

    public static Accessor ToAccessor<T, TResult> (this Expression<Func<T, TResult>> expression) =>
        ReflectionHelper.GetAccessor (expression);

    public static string GetName<T> (this Expression<Func<T, object>> expression) =>
        ReflectionHelper.GetAccessor (expression).Name;


    public static void IfPropertyTypeIs<T> (this Accessor accessor, Action action)
    {
        if (accessor.PropertyType == typeof (T))
        {
            action();
        }
    }

    public static bool IsInteger (this Accessor accessor) => accessor.PropertyType.IsTypeOrNullableOf<int>() ||
                                                             accessor.PropertyType.IsTypeOrNullableOf<long>();
}
