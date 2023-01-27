// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* PropertyNode.cs -- обращение к свойству объекта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Dynamic;

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
            return null;
        }

        if (string.IsNullOrEmpty (_propertyName))
        {
            return null;
        }

        if (obj is Type type)
        {
            var propertyInfo = type.GetProperty (_propertyName);
            if (propertyInfo is not null)
            {
                return propertyInfo.GetValue (null);
            }

            var fieldInfo = type.GetField (_propertyName);
            if (fieldInfo is not null)
            {
                return fieldInfo.GetValue (null);
            }

            return null;
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
        var property = type.GetProperty (_propertyName);
        if (property is not null)
        {
            return property.GetValue (obj);
        }

        var field = type.GetField (_propertyName);
        if (field is not null)
        {
            return field.GetValue (obj);
        }

        return null;
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
            return null;
        }

        if (string.IsNullOrEmpty (_propertyName))
        {
            return null;
        }

        if (obj is Type type)
        {
            var propertyInfo = type.GetProperty (_propertyName);
            if (propertyInfo is not null)
            {
                propertyInfo.SetValue (null, value);

                return value;
            }

            var fieldInfo = type.GetField (_propertyName);
            if (fieldInfo is not null)
            {
                fieldInfo.SetValue (null, value);

                return value;
            }

            return value;
        }

        if (obj is ExpandoObject expando)
        {
            #pragma warning disable CS8619
            ((IDictionary<string, object>) expando)[_propertyName] = value!;
            #pragma warning restore CS8619

            return value;
        }

        type = ((object) obj).GetType();
        var property = type.GetProperty (_propertyName);
        if (property is not null)
        {
            property.SetValue (obj, value);

            return value;
        }

        var field = type.GetField (_propertyName);
        if (field is not null)
        {
            field.SetValue (obj, value);

            return value;
        }

        return null;
    }

    #endregion
}
