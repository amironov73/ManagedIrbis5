// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* AssignmentNode.cs -- присваивание переменной результата вычисления выражения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Присваивание переменной результата вычисления выражения.
/// </summary>
internal sealed class AssignmentNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AssignmentNode
        (
            string target,
            string operation,
            AtomNode expression
        )
    {
        Sure.NotNullNorEmpty (target);
        Sure.NotNullNorEmpty (operation);

        if (Array.IndexOf (BarsikUtility.Keywords, target) >= 0)
        {
            throw new BarsikException ($"Name {target} is reserved");
        }

        _target = target;
        _operation = operation;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly string _target;
    private readonly string _operation;
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

        var variableName = _target;
        dynamic? variableValue = null;
        if (_operation != "=")
        {
            if (!context.TryGetVariable (variableName, out variableValue))
            {
                context.Error.WriteLine ($"Variable {variableName} not found");
                return;
            }
        }

        var computedValue = _expression.Compute (context);
        computedValue = _operation switch
        {
            "=" => computedValue,
            "+=" => variableValue + computedValue,
            "-=" => variableValue - computedValue,
            "*=" => variableValue * computedValue,
            "/=" => variableValue / computedValue,
            "%=" => variableValue % computedValue,
            "&=" => variableValue & computedValue,
            "|=" => variableValue | computedValue,
            "^=" => variableValue ^ computedValue,
            "<<=" => variableValue << computedValue,
            ">>=" => variableValue >> computedValue,
            _ => throw new Exception()
        };

        context.SetVariable (variableName, computedValue);

        // BarsikUtility.PrintObject (context.Output, computedValue);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"Assignment: {_target} = {_expression};";
    }

    #endregion
}
