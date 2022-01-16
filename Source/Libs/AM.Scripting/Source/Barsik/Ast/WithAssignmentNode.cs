// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* WithAssignmentNode.cs -- with-присваивание
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
/// With-присваивание.
/// </summary>
internal sealed class WithAssignmentNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WithAssignmentNode
        (
            SourcePosition startPosition,
            AtomNode propertyName,
            AtomNode expression
        )
        : base (startPosition)
    {
        Sure.NotNull (propertyName);
        Sure.NotNull (expression);

        _propertyName = propertyName;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly AtomNode _propertyName;
    private readonly AtomNode _expression;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var center = context.With;
        if (center is null)
        {
            context.Error.WriteLine ("Bad with block");
            return;
        }

        string? objectName;
        if (center is VariableNode varNode)
        {
            objectName = varNode.Name;
        }
        else
        {
            objectName = _propertyName.Compute (context);
        }

        if (string.IsNullOrEmpty (objectName))
        {
            context.Error.WriteLine ($"Bad with block");
            return;
        }

        if (!context.TryGetVariable (objectName, out var objectValue))
        {
            context.Error.WriteLine ($"Variable {objectName} not found");
            return;
        }

        if (objectValue is null)
        {
            context.Error.WriteLine($"Can't assign to null");
            return;
        }

        var expressionValue = _expression.Compute (context);

        string? propertyName;
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
            context.Error.WriteLine ("No property name");
            return;
        }

        if (objectValue is Type type)
        {
            var propertyInfo = type.GetProperty (propertyName);
            if (propertyInfo is not null)
            {
                propertyInfo.SetValue (null, expressionValue);

                return;
            }

            var fieldInfo = type.GetField (propertyName);
            if (fieldInfo is not null)
            {
                fieldInfo.SetValue (null, expressionValue);

                return;
            }

            return;
        }

        if (objectValue is ExpandoObject expando)
        {
#pragma warning disable CS8619
            ((IDictionary<string, object>) expando)[propertyName] = expressionValue!;
#pragma warning restore CS8619

            return;
        }

        type = ((object) objectValue).GetType();
        var property = type.GetProperty (propertyName);
        if (property is not null)
        {
            property.SetValue (objectValue, expressionValue);

            return;
        }

        var field = type.GetField (propertyName);
        if (field is not null)
        {
            field.SetValue (objectValue, expressionValue);

            return;
        }

        context.Error.WriteLine ("Can't handle assignment");
    }

    #endregion

}
