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
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AssignmentNode
        (
            AtomNode target,
            string operation,
            AtomNode expression
        )
    {
        Sure.NotNull (target);
        Sure.NotNullNorEmpty (operation);
        Sure.NotNull (expression);

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

    private readonly AtomNode _target;
    private readonly string _operation;
    private readonly AtomNode _expression;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var value = _expression.Compute (context);
        value = _target.Assign (context, _operation, value);

        return value;
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
