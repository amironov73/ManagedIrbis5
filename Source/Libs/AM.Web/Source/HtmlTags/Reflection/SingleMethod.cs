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

#endregion

namespace AM.HtmlTags.Reflection;

public class SingleMethod : IAccessor
{
    private readonly MethodValueGetter _getter;
    private readonly Type _ownerType;

    public SingleMethod (MethodValueGetter getter)
    {
        _getter = getter;
    }

    public SingleMethod (MethodValueGetter getter, Type ownerType)
    {
        _getter = getter;
        _ownerType = ownerType;
    }


    public string FieldName => _getter.Name;

    public Type PropertyType => _getter.ReturnType;

    public Type DeclaringType => _getter.DeclaringType;


    public PropertyInfo InnerProperty => null;

    public IAccessor GetChildAccessor<T> (Expression<Func<T, object>> expression)
    {
        throw new NotSupportedException ("Not supported with Methods");
    }

    public string[] PropertyNames => new[] { Name };

    public Expression<Func<T, object>> ToExpression<T>()
    {
        throw new NotSupportedException ("Not yet supported with Methods");
    }

    public IAccessor Prepend (PropertyInfo property)
    {
        return
            new PropertyChain (new IValueGetter[]
                { new PropertyValueGetter (property), _getter });
    }

    public IEnumerable<IValueGetter> Getters()
    {
        yield return _getter;
    }

    public string Name => _getter.Name;

    public virtual void SetValue (object target, object propertyValue)
    {
        // no-op
    }

    public object GetValue (object target) => _getter.GetValue (target);

    public Type OwnerType => _ownerType ?? DeclaringType;


    public bool Equals (SingleMethod other)
    {
        if (ReferenceEquals (null, other))
        {
            return false;
        }

        if (ReferenceEquals (this, other))
        {
            return true;
        }

        return Equals (other._getter, _getter);
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals (null, obj))
        {
            return false;
        }

        if (ReferenceEquals (this, obj))
        {
            return true;
        }

        if (obj.GetType() != typeof (SingleMethod))
        {
            return false;
        }

        return Equals ((SingleMethod)obj);
    }

    public override int GetHashCode() => _getter?.GetHashCode() ?? 0;
}
