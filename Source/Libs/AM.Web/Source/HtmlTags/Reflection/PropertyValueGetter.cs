// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PropertyValueGetter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public class PropertyValueGetter
    : IValueGetter
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public PropertyInfo PropertyInfo { get; }

    /// <inheritdoc cref="IValueGetter.Name"/>
    public string Name => PropertyInfo.Name;

    /// <inheritdoc cref="IValueGetter.DeclaringType"/>
    public Type? DeclaringType => PropertyInfo.DeclaringType;

    /// <inheritdoc cref="IValueGetter.ValueType"/>
    public Type ValueType => PropertyInfo.PropertyType;

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyInfo"></param>
    public PropertyValueGetter
        (
            PropertyInfo propertyInfo
        )
    {
        Sure.NotNull (propertyInfo);

        PropertyInfo = propertyInfo;
    }

    #endregion

    #region IValueGetter members

    /// <inheritdoc cref="IValueGetter.GetValue"/>
    public object? GetValue
        (
            object target
        )
    {
        Sure.NotNull (target);

        return PropertyInfo.GetValue (target, null);
    }

    /// <inheritdoc cref="IValueGetter.ChainExpression"/>
    public Expression ChainExpression
        (
            Expression body
        )
    {
        Sure.NotNull (body);

        var memberExpression = Expression.Property (body, PropertyInfo);
        if (!PropertyInfo.PropertyType.GetTypeInfo().IsValueType)
        {
            return memberExpression;
        }

        return Expression.Convert (memberExpression, typeof (object));
    }

    /// <inheritdoc cref="IValueGetter.SetValue"/>
    public void SetValue
        (
            object target,
            object? propertyValue
        )
    {
        Sure.NotNull (target);

        PropertyInfo.SetValue (target, propertyValue, null);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals
        (
            PropertyValueGetter? other
        )
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals (this, other))
        {
            return true;
        }

        return other.PropertyInfo.PropertyMatches (PropertyInfo);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals (this, obj))
        {
            return true;
        }

        return obj is PropertyValueGetter getter && Equals (getter);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => PropertyInfo.GetHashCode();

    #endregion
}
