// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* SingleProperty.cs --
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
public class SingleProperty
    : IAccessor
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string FieldName => InnerProperty.Name;

    /// <inheritdoc cref="IAccessor.PropertyType"/>
    public Type PropertyType => InnerProperty.PropertyType;

    /// <inheritdoc cref="IAccessor.DeclaringType"/>
    public Type? DeclaringType => InnerProperty.DeclaringType;

    /// <inheritdoc cref="IAccessor.InnerProperty"/>
    public PropertyInfo InnerProperty { get; }

    /// <inheritdoc cref="IAccessor.OwnerType"/>
    public Type? OwnerType => _ownerType ?? DeclaringType;

    /// <inheritdoc cref="IAccessor.PropertyNames"/>
    public string[] PropertyNames => new[] { InnerProperty.Name };

    /// <inheritdoc cref="IAccessor.Name"/>
    public string Name => InnerProperty.Name;

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="property"></param>
    public SingleProperty
        (
            PropertyInfo property
        )
    {
        Sure.NotNull (property);

        InnerProperty = property;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="property"></param>
    /// <param name="ownerType"></param>
    public SingleProperty
        (
            PropertyInfo property,
            Type ownerType
        )
    {
        Sure.NotNull (property);
        Sure.NotNull (ownerType);

        InnerProperty = property;
        _ownerType = ownerType;
    }

    #endregion

    #region Private members

    private readonly Type? _ownerType;

    #endregion

    #region IAccessor members

    /// <inheritdoc cref="IAccessor.GetChildAccessor{T}"/>
    public IAccessor GetChildAccessor<T>
        (
            Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        var property = ReflectionHelper.GetProperty (expression);
        return new PropertyChain (new[]
        {
            new PropertyValueGetter (InnerProperty),
            new PropertyValueGetter (property)

        });
    }

    /// <inheritdoc cref="IAccessor.ToExpression{T}"/>
    public Expression<Func<T, object>> ToExpression<T>()
    {
        var parameter = Expression.Parameter (typeof (T), "x");
        Expression body = Expression.Property (parameter, InnerProperty);
        if (InnerProperty.PropertyType.GetTypeInfo().IsValueType)
        {
            body = Expression.Convert (body, typeof (object));
        }


        var delegateType = typeof (Func<,>).MakeGenericType (typeof (T), typeof (object));
        return (Expression<Func<T, object>>)Expression.Lambda (delegateType, body, parameter);
    }

    /// <inheritdoc cref="IAccessor.Prepend"/>
    public IAccessor Prepend
        (
            PropertyInfo property
        )
    {
        Sure.NotNull (property);

        return new PropertyChain (new IValueGetter[]
        {
            new PropertyValueGetter (property),
            new PropertyValueGetter (InnerProperty)

        });
    }

    /// <inheritdoc cref="IAccessor.Getters"/>
    public IEnumerable<IValueGetter> Getters()
    {
        yield return new PropertyValueGetter (InnerProperty);
    }

    /// <inheritdoc cref="IAccessor.SetValue"/>
    public virtual void SetValue
        (
            object target,
            object? propertyValue
        )
    {
        Sure.NotNull (target);

        if (InnerProperty.CanWrite)
        {
            InnerProperty.SetValue (target, propertyValue, null);
        }
    }

    /// <inheritdoc cref="IAccessor.GetValue"/>
    public object? GetValue
        (
            object target
        )
    {
        Sure.NotNull (target);

        return InnerProperty.GetValue (target, null);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="expression"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static SingleProperty Build<T>
        (
            Expression<Func<T, object>> expression
        )
    {
        var property = ReflectionHelper.GetProperty (expression);
        return new SingleProperty (property);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="propertyName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static SingleProperty Build<T>
        (
            string propertyName
        )
    {
        var property = typeof (T).GetProperty (propertyName);
        return new SingleProperty (property!);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals
        (
            SingleProperty? other
        )
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals (this, other)
               || InnerProperty.PropertyMatches (other.InnerProperty);
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

        return ReferenceEquals (this, obj)
               || obj is SingleProperty property && Equals (property);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() =>
        (InnerProperty.DeclaringType?.FullName + "." + InnerProperty.Name).GetHashCode();

    #endregion
}
