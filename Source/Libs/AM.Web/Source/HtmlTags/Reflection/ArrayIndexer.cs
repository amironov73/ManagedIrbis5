// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ArrayIndexer.cs --
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
public class ArrayIndexer
    : IAccessor
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string FieldName => _getter.Name;

    /// <summary>
    ///
    /// </summary>
    public Type? PropertyType => _getter.ValueType;

    /// <inheritdoc cref="IAccessor.InnerProperty"/>
    public PropertyInfo? InnerProperty => null;

    /// <inheritdoc cref="IAccessor.DeclaringType"/>
    public Type? DeclaringType => _getter.DeclaringType;

    /// <inheritdoc cref="IAccessor.Name"/>
    public string Name => _getter.Name;

    /// <inheritdoc cref="IAccessor.OwnerType"/>
    public Type? OwnerType => DeclaringType;

    /// <inheritdoc cref="IAccessor.PropertyNames"/>
    public string[] PropertyNames => new[] { Name };

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="getter"></param>
    public ArrayIndexer
        (
            IndexerValueGetter getter
        )
    {
        Sure.NotNull (getter);

        _getter = getter;
    }

    #endregion

    #region Private members

    private readonly IndexerValueGetter _getter;

    #endregion

    #region IAccessor members

    /// <inheritdoc cref="IAccessor.SetValue"/>
    public void SetValue
        (
            object target,
            object? propertyValue
        )
    {
        Sure.NotNull (target);

        _getter.SetValue (target, propertyValue);
    }

    ///<inheritdoc cref="IAccessor.GetValue"/>
    public object? GetValue
        (
            object target
        )
    {
        Sure.NotNull (target);

        return _getter.GetValue (target);
    }

    /// <inheritdoc cref="IAccessor.GetChildAccessor{T}"/>
    public IAccessor GetChildAccessor<T>
        (
            Expression<Func<T, object>> expression
        )
    {
        throw new NotSupportedException ("Not supported in arrays");
    }

    /// <inheritdoc cref="IAccessor.ToExpression{T}"/>
    public Expression<Func<T, object>> ToExpression<T>()
    {
        var parameter = Expression.Parameter (typeof (T), "x");
        Expression body = Expression.ArrayIndex
            (
                parameter,
                Expression.Constant (_getter.Index, typeof (int))
            );
        if (_getter.ValueType!.GetTypeInfo().IsValueType)
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

        return new PropertyChain
            (
                new IValueGetter[]
                {
                    new PropertyValueGetter (property),
                    _getter
                }
            );
    }

    /// <inheritdoc cref="IAccessor.Getters"/>
    public IEnumerable<IValueGetter> Getters()
    {
        yield return _getter;
    }

    #endregion
}
