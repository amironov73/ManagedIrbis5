// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* PropertyChain.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

#nullable enable

namespace AM.HtmlTags.Reflection;

/// <summary>
///
/// </summary>
public class PropertyChain
    : IAccessor
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string FieldName
    {
        get
        {
            var last = _valueGetters.Last();
            if (last is PropertyValueGetter)
            {
                return last.Name;
            }

            var previous = _valueGetters[^2];
            return previous.Name + last.Name;
        }
    }

    /// <inheritdoc cref="IAccessor.PropertyType"/>
    public Type? PropertyType => _valueGetters.Last().ValueType;

    /// <inheritdoc cref="IAccessor.InnerProperty"/>
    public PropertyInfo? InnerProperty => (_valueGetters.Last() as PropertyValueGetter)?.PropertyInfo;

    /// <inheritdoc cref="IAccessor.DeclaringType"/>
    public Type? DeclaringType => _chain[0].DeclaringType;

    /// <inheritdoc cref="IAccessor.PropertyNames"/>
    public string[] PropertyNames => _valueGetters.Select (x => x.Name).ToArray();

    /// <summary>
    ///
    /// </summary>
    public IValueGetter[] ValueGetters => _valueGetters;

    /// <inheritdoc cref="IAccessor.OwnerType"/>
    public Type? OwnerType
    {
        get
        {
            // Check if we're an indexer here
            var last = _valueGetters.Last();
            if (last is MethodValueGetter || last is IndexerValueGetter)
            {
                if (_chain.Reverse().Skip (1).FirstOrDefault() is PropertyValueGetter nextUp)
                {
                    return nextUp.PropertyInfo.PropertyType;
                }
            }

            var propertyGetter = _chain.Last() as PropertyValueGetter;

            return propertyGetter?.PropertyInfo.PropertyType ?? InnerProperty?.DeclaringType;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="valueGetters"></param>
    public PropertyChain
        (
            IValueGetter[] valueGetters
        )
    {
        Sure.NotNull (valueGetters);

        _chain = new IValueGetter[valueGetters.Length - 1];
        for (var i = 0; i < _chain.Length; i++)
        {
            _chain[i] = valueGetters[i];
        }

        _valueGetters = valueGetters;
    }

    #endregion

    #region Private members

    private readonly IValueGetter[] _chain;
    private readonly IValueGetter[] _valueGetters;

    #endregion

    #region IValueGetter members

    /// <inheritdoc cref="IAccessor.SetValue"/>
    public void SetValue
        (
            object target,
            object? propertyValue
        )
    {
        Sure.NotNull (target);

        var found = FindInnerMostTarget (target);
        if (found is not null)
        {
            SetValueOnInnerObject (found, propertyValue);
        }
    }

    /// <inheritdoc cref="IAccessor.GetValue"/>
    public object? GetValue
        (
            object target
        )
    {
        Sure.NotNull (target);

        var inner = FindInnerMostTarget (target);

        return inner is null ? null : _valueGetters.Last().GetValue (inner);
    }

    /// <inheritdoc cref="IAccessor.GetChildAccessor{T}"/>
    public IAccessor GetChildAccessor<T>
        (
            Expression<Func<T, object>> expression
        )
    {
        Sure.NotNull (expression);

        var accessor = expression.ToAccessor();
        var allGetters = Getters().Union (accessor.Getters()).ToArray();
        return new PropertyChain (allGetters);
    }


    /// <inheritdoc cref="IAccessor.ToExpression{T}"/>
    public Expression<Func<T, object>> ToExpression<T>()
    {
        var parameter = Expression.Parameter (typeof (T), "x");
        Expression body = parameter;

        _valueGetters.Each (getter => { body = getter.ChainExpression (body); });

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

        var list = new List<IValueGetter>
        {
            new PropertyValueGetter (property)
        };
        list.AddRange (_valueGetters);

        return new PropertyChain (list.ToArray());
    }

    /// <inheritdoc cref="IAccessor.Getters"/>
    public IEnumerable<IValueGetter> Getters() => _valueGetters;

    /// <summary>
    ///     Concatenated names of all the properties in the chain.
    ///     Case.Site.Name == "CaseSiteName"
    /// </summary>
    public string Name => _valueGetters.Select (x => x.Name).Join ("");

    #endregion

    #region Protected members

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="propertyValue"></param>
    protected virtual void SetValueOnInnerObject
        (
            object target,
            object? propertyValue
        )
    {
        Sure.NotNull (target);

        _valueGetters.Last().SetValue (target, propertyValue);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    protected object? FindInnerMostTarget
        (
            object target
        )
    {
        Sure.NotNull (target);

        var found = target;
        foreach (var info in _chain)
        {
            found = info.GetValue (found);
            if (found is null)
            {
                return null;
            }
        }

        return found;
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
            PropertyChain? other
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

        return _valueGetters.SequenceEqual (other._valueGetters);
    }

    #endregion

    #region Object members

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() =>
        _chain.First().DeclaringType!.FullName + _chain.Select (x => x.Name).Join (".");

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        if (ReferenceEquals (null, obj))
        {
            return false;
        }

        if (ReferenceEquals (this, obj))
        {
            return true;
        }

        return obj is PropertyChain chain && Equals (chain);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode() => _chain?.GetHashCode() ?? 0;

    #endregion
}
