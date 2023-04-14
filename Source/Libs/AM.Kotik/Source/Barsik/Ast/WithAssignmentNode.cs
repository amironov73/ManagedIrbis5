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

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// with-присваивание.
/// </summary>
internal sealed class WithAssignmentNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WithAssignmentNode
        (
            int line,
            string propertyName,
            AtomNode expression
        )
        : base (line)
    {
        Sure.NotNullNorEmpty (propertyName);
        Sure.NotNull (expression);

        _propertyName = propertyName;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly string _propertyName;
    private readonly AtomNode _expression;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var center = context.With;
        if (center is null)
        {
            context.Commmon.Error?.WriteLine ("Bad with block");
            return;
        }

        var objectValue = center.Compute (context);
        if (objectValue is null)
        {
            context.Commmon.Error?.WriteLine($"Can't assign to null");
            return;
        }

        var expressionValue = _expression.Compute (context);
        if (objectValue is Type type)
        {
            var propertyInfo = type.GetProperty (_propertyName);
            if (propertyInfo is not null)
            {
                propertyInfo.SetValue (null, expressionValue);

                return;
            }

            var fieldInfo = type.GetField (_propertyName);
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
            ((IDictionary<string, object>) expando)[_propertyName] = expressionValue!;
#pragma warning restore CS8619

            return;
        }

        type = ((object) objectValue).GetType();
        var property = type.GetProperty (_propertyName);
        if (property is not null)
        {
            property.SetValue (objectValue, expressionValue);

            return;
        }

        var field = type.GetField (_propertyName);
        if (field is not null)
        {
            field.SetValue (objectValue, expressionValue);

            return;
        }

        context.Commmon.Error?.WriteLine ("Can't handle assignment");
    }

    #endregion
}
