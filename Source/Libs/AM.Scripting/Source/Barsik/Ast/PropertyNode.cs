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
using System.Collections;
using System.Collections.Generic;

using AM.Text;

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

    // private dynamic? ComputeStatic (Context context)
    // {
    //     var type = context.FindType (_objectName);
    //     if (type is null)
    //     {
    //         context.Error.WriteLine ($"Variable or type {_objectName} not found");
    //
    //         return null;
    //     }
    //
    //     var property = type.GetProperty (_propertyName);
    //     if (property is not null)
    //     {
    //         return property.GetValue (null);
    //     }
    //
    //     var field = type.GetField (_propertyName);
    //     if (field is not null)
    //     {
    //         return field.GetValue (null);
    //     }
    //
    //     return null;
    // }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        // if (!context.TryGetVariable (_objectName, out var obj))
        // {
        //     return ComputeStatic (context);
        // }
        //

        var obj = _obj.Compute (context);
        if (obj is null)
        {
            return null;
        }
        var type = ((object) obj).GetType();
        var name = ((VariableNode) _propertyName).Name;
        var property = type.GetProperty (name);
        if (property is not null)
        {
            return property.GetValue (obj);
        }

        var field = type.GetField (name);
        if (field is not null)
        {
            return field.GetValue (obj);
        }

        return null;
    }

    #endregion
}
