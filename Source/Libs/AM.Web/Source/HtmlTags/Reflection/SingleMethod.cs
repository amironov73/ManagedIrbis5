// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* SingleMethod.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public class SingleMethod
    : IAccessor
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string FieldName => _getter.Name;

    /// <inheritdoc cref="IAccessor.PropertyType"/>
    public Type PropertyType => _getter.ReturnType;

    /// <inheritdoc cref="IAccessor.DeclaringType"/>
    public Type? DeclaringType => _getter.DeclaringType;

    /// <inheritdoc cref="IAccessor.InnerProperty"/>
    public PropertyInfo? InnerProperty => null;

    /// <inheritdoc cref="IAccessor.OwnerType"/>
    public Type? OwnerType => _ownerType ?? DeclaringType;

    /// <inheritdoc cref="IAccessor.Name"/>
    public string Name => _getter.Name;

    /// <inheritdoc cref="IAccessor.PropertyNames"/>
    public string[] PropertyNames => new[] { Name };

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="getter"></param>
    public SingleMethod
        (
            MethodValueGetter getter
        )
    {
        Sure.NotNull (getter);

        _getter = getter;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="getter"></param>
    /// <param name="ownerType"></param>
    public SingleMethod
        (
            MethodValueGetter getter,
            Type ownerType
        )
    {
        Sure.NotNull (getter);
        Sure.NotNull (ownerType);

        _getter = getter;
        _ownerType = ownerType;
    }

    #endregion

    #region Private members

    private readonly MethodValueGetter _getter;
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

        throw new NotSupportedException ("Not supported with Methods");
    }

    /// <inheritdoc cref="IAccessor.ToExpression{T}"/>
    public Expression<Func<T, object>> ToExpression<T>()
    {
        throw new NotSupportedException ("Not yet supported with Methods");
    }

    /// <inheritdoc cref="IAccessor.Prepend"/>
    public IAccessor Prepend
        (
            PropertyInfo property
        )
    {
        Sure.NotNull (property);

        return new PropertyChain (new IValueGetter[]
                { new PropertyValueGetter (property), _getter });
    }

    /// <inheritdoc cref="IAccessor.Getters"/>
    public IEnumerable<IValueGetter> Getters()
    {
        yield return _getter;
    }

    /// <inheritdoc cref="IAccessor.SetValue"/>
    public virtual void SetValue
        (
            object target,
            object? propertyValue
        )
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IAccessor.GetValue"/>
    public object? GetValue
        (
            object target
        )
    {
        Sure.NotNull (target);

        return _getter.GetValue (target);
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
            SingleMethod? other
        )
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals (this, other) || Equals (other._getter, _getter);
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
               || obj is SingleMethod method && Equals (method);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => _getter.GetHashCode();

    #endregion
}
