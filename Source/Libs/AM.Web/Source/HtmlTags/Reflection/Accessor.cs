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

public interface Accessor
{
    Type PropertyType { get; }
    PropertyInfo InnerProperty { get; }
    Type DeclaringType { get; }
    string Name { get; }
    Type OwnerType { get; }
    void SetValue (object target, object propertyValue);
    object GetValue (object target);

    Accessor GetChildAccessor<T> (Expression<Func<T, object>> expression);

    string[] PropertyNames { get; }

    Expression<Func<T, object>> ToExpression<T>();

    Accessor Prepend (PropertyInfo property);

    IEnumerable<IValueGetter> Getters();
}

public static class AccessorExtensions
{
    public static Accessor Prepend (this Accessor accessor, Accessor prefixedAccessor)
    {
        return new PropertyChain (prefixedAccessor.Getters().Union (accessor.Getters()).ToArray());
    }
}
