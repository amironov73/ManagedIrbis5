// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PropertyNode.cs -- обращение к свойству объекта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Обращение к свойству объекта.
/// </summary>
internal sealed class PropertyNode
    : UnaryNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PropertyNode
        (
            AtomNode target,
            string propertyName
        )
    {
        Sure.NotNull (target);
        Sure.NotNull (propertyName);

        _target = target;
        _propertyName = propertyName;
    }

    #endregion

    #region Private members

    private readonly AtomNode _target;
    private readonly string _propertyName;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var obj = _target.Compute (context);
        if (obj is null)
        {
            throw new BarsikException ("Can't get property or field of null object");
        }

        if (string.IsNullOrEmpty (_propertyName))
        {
            throw new BarsikException ("Property or field name not specified");
        }

        var descriptor = new MemberDescriptor
        {
            Name = _propertyName
        };
        
        if (obj is Type type)
        {
            descriptor.Type = type;
            var staticMember = context.Commmon.Resolver.ResolveMember (descriptor);
            if (staticMember is null)
            {
                throw new BarsikException ($"Can't resolve property or field {type}.{_propertyName}");
            }

            return staticMember.GetValue (null);
        }

        if (obj is ExpandoObject expando)
        {
            #pragma warning disable CS8619
            ((IDictionary<string, object>) expando).TryGetValue
                (
                    _propertyName,
                    out var expandoResult
                );
            #pragma warning restore CS8619

            return expandoResult;
        }

        type = ((object) obj).GetType();
        descriptor.Type = type;
        var instanceMember = context.Commmon.Resolver.ResolveMember (descriptor);
        if (instanceMember is null)
        {
            throw new BarsikException ($"Can't resolve property or field {type}.{_propertyName}");
        }

        return instanceMember.GetValue (obj);
    }

    /// <inheritdoc cref="AtomNode.Assign"/>
    public override dynamic? Assign
        (
            Context context,
            string? operation,
            dynamic? value
        )
    {
        var obj = _target.Compute (context);
        if (obj is null)
        {
            throw new BarsikException ($"Can't set property or field of null object");
        }

        if (string.IsNullOrEmpty (_propertyName))
        {
            throw new BarsikException ("Property or field name not specified");
        }

        var descriptor = new MemberDescriptor
        {
            Name = _propertyName
        };

        if (obj is Type type)
        {
            descriptor.Type = type;
            var staticMember = context.Commmon.Resolver.ResolveMember (descriptor);
            if (staticMember is null)
            {
                throw new BarsikException ($"Can't resolve property or field {type}.{_propertyName}");
            }
            
            staticMember.SetValue (null, value);
            return value;
        }

        if (obj is ExpandoObject expando)
        {
            #pragma warning disable CS8619
            ((IDictionary<string, object?>) expando)[_propertyName] = value;
            #pragma warning restore CS8619
            return value;
        }

        type = ((object) obj).GetType();
        descriptor.Type = type;
        var instanceMember = context.Commmon.Resolver.ResolveMember (descriptor);
        if (instanceMember is null)
        {
            throw new BarsikException ($"Can't resolve property or field {type}.{_propertyName}");
        }

        instanceMember.SetValue (obj, value);
        return value;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        _target.DumpHierarchyItem ("Target", level + 1, writer);
        DumpHierarchyItem ("Property", level + 1, writer, _propertyName);
    }

    #endregion
}
