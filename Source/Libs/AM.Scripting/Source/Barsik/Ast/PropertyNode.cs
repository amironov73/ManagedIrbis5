// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
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

namespace AM.Scripting.Barsik;

/// <summary>
/// Обращение к свойству объекта.
/// </summary>
internal sealed class PropertyNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PropertyNode
        (
            AtomNode obj,
            AtomNode propertyName
        )
    {
        Sure.NotNull (obj);
        Sure.NotNull (propertyName);

        // TODO хранение имени свойства в AtomNode -- костыль!
        _obj = obj;
        _propertyName = propertyName;
    }

    #endregion

    #region Private members

    private readonly AtomNode _obj;
    private readonly AtomNode _propertyName;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var obj = _obj.Compute (context);
        if (obj is null)
        {
            return null;
        }

        string? propertyName = null;
        if (_propertyName is VariableNode variableNode)
        {
            propertyName = variableNode.Name;
        }
        else
        {
            propertyName = _propertyName.Compute (context);
        }

        if (string.IsNullOrEmpty (propertyName))
        {
            return null;
        }

        if (obj is Type type)
        {
            var propertyInfo = type.GetProperty (propertyName);
            if (propertyInfo is not null)
            {
                return propertyInfo.GetValue (null);
            }

            var fieldInfo = type.GetField (propertyName);
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
                    propertyName,
                    out var expandoResult
                );
            #pragma warning restore CS8619

            return expandoResult;
        }

        type = ((object) obj).GetType();
        var property = type.GetProperty (propertyName);
        if (property is not null)
        {
            return property.GetValue (obj);
        }

        var field = type.GetField (propertyName);
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
            string operation,
            dynamic? value
        )
    {
        var obj = _obj.Compute (context);
        if (obj is null)
        {
            return null;
        }

        string? propertyName = null;
        if (_propertyName is VariableNode variableNode)
        {
            propertyName = variableNode.Name;
        }
        else
        {
            propertyName = _propertyName.Compute (context);
        }

        if (string.IsNullOrEmpty (propertyName))
        {
            return null;
        }

        if (obj is Type type)
        {
            var propertyInfo = type.GetProperty (propertyName);
            if (propertyInfo is not null)
            {
                propertyInfo.SetValue (null, value);

                return value;
            }

            var fieldInfo = type.GetField (propertyName);
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
            ((IDictionary<string, object>) expando)[propertyName] = value!;
            #pragma warning restore CS8619

            return value;
        }

        type = ((object) obj).GetType();
        var property = type.GetProperty (propertyName);
        if (property is not null)
        {
            property.SetValue (obj, value);

            return value;
        }

        var field = type.GetField (propertyName);
        if (field is not null)
        {
            field.SetValue (obj, value);

            return value;
        }

        return null;
    }

    #endregion
}
