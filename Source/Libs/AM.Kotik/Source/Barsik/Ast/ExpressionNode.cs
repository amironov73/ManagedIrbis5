// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExpressionNode.cs -- выражение (возможно, с присваиванием значения переменной)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.Kotik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Выражение (возможно, с присваиванием значения переменной).
/// </summary>
internal sealed class ExpressionNode
    : AtomNode
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExpressionNode
        (
            AtomNode? target,
            string? operation,
            AtomNode expression
        )
    {
        _target = target;
        _operation = operation;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly AtomNode? _target;
    private readonly string? _operation;
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
        if (_target is not null)
        {
            value = _target.Assign (context, _operation, value);
        }

        return value;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        var whatIam = ToString();
        if (_operation is not null)
        {
            whatIam = "Assignment";
        }
        base.DumpHierarchyItem (name, level, writer, whatIam);

        if (_target is not null)
        {
            DumpHierarchyItem ("Variable", level + 1, writer, _target.ToVisibleString());
        }

        if (_operation is not null)
        {
            DumpHierarchyItem ("Operation", level + 1, writer, _operation.ToVisibleString());
        }

        _expression.DumpHierarchyItem ("Expression", level + 1, writer);
    }

    #endregion
}
