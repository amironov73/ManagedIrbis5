// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WithAssignmentNode.cs -- with-присваивание
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Dynamic;

using Microsoft.Extensions.Logging;

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

        var logger = Magna.Logger;
        var errorOutput = context.Commmon.Error;
        var center = context.With;
        if (center is null)
        {
            logger.LogInformation ("Bad with block at line {Line}", Line);
            errorOutput?.WriteLine ("Bad with block at line {Line}");
            return;
        }

        var objectValue = center.Compute (context);
        if (objectValue is null)
        {
            logger.LogInformation ("Can't assign to null: {Center} at line {Line}", center, Line);
            errorOutput?.WriteLine ($"Can't assign to null '{center}' at line {Line}");
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

            logger.LogInformation ("Can't handle assignment");
            logger.LogInformation ("Line number: {Line}", Line);
            logger.LogInformation ("Type: {Type}", type);
            logger.LogInformation ("Property name: {Property}", _propertyName);

            if (errorOutput is not null)
            {
                errorOutput.WriteLine ("Can't handle assignment");
                errorOutput.WriteLine ($"Line number: {Line}");
                errorOutput.WriteLine ($"Type: {type}");
                errorOutput.WriteLine ($"Property name: {_propertyName}");
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

        logger.LogInformation ("Can't handle assignment");
        logger.LogInformation ("Line number: {Line}", Line);
        logger.LogInformation ("Object: {Object}", (object) objectValue);
        logger.LogInformation ("Property name: {Property}", _propertyName);

        if (errorOutput is not null)
        {
            errorOutput.WriteLine ("Can't handle assignment");
            errorOutput.WriteLine ($"Line number: {Line}");
            errorOutput.WriteLine ($"Object: {objectValue}");
            errorOutput.WriteLine ($"Property name: {_propertyName}");
        }
    }

    #endregion
}
