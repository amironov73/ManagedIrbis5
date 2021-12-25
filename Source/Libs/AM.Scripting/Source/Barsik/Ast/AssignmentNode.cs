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
using System.Collections.Generic;

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
            AtomNode expression
        )
    {
        _target = target;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly string _target;
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
        var computedValue = _expression.Compute (context);

        context.SetVariable (variableName, computedValue);

        BarsikUtility.PrintObject (context.Output, computedValue);
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
