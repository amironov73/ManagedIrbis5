// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* MethodNode.cs -- вызов метода объекта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Вызов метода объекта.
/// </summary>
public sealed class MethodNode
    : UnaryNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MethodNode
        (
            AtomNode thisObject,
            string methodName,
            AtomNode[] arguments
        )
    {
        Sure.NotNull (thisObject);
        Sure.NotNullNorEmpty (methodName);

        _thisObject = thisObject;
        _methodName = methodName;
        _arguments = arguments;

    }

    #endregion

    #region Private members

    private readonly AtomNode _thisObject;
    private readonly string _methodName;
    private readonly AtomNode[] _arguments;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var thisValue = _thisObject.Compute (context);
        if (thisValue is null)
        {
            return null;
        }

        var argumentValues = new List<dynamic>();
        var argumentTypes = new List<Type>();
        foreach (var argument in _arguments)
        {
            var value = argument.Compute (context);
            argumentValues.Add (value);
            var argType = value is null
                ? typeof (object)
                : value.GetType();
            argumentTypes.Add (argType);
        }

        if (thisValue is Type type)
        {
            var staticMethod = type.GetMethod (_methodName, argumentTypes.ToArray());
            staticMethod ??= type.GetMethod (_methodName);
            if (staticMethod is null)
            {
                return null;
            }

            return staticMethod.Invoke (null, argumentValues.ToArray());
        }

        type = ((object) thisValue).GetType();
        var instanceMethod = type.GetMethod (_methodName, argumentTypes.ToArray());
        instanceMethod ??= type.GetMethod (_methodName);
        if (instanceMethod is null)
        {
            return null;
        }

        var result = instanceMethod.Invoke (thisValue, argumentValues.ToArray());

        return result;
    }

    #endregion
}
