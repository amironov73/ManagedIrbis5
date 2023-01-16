// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExpressionNode.cs -- выражение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Выражение.
/// </summary>
public /* не sealed */ class ExpressionNode
    : AtomNode
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ExpressionNode
        (
            string? variable,
            string? operation,
            AtomNode expression
        )
    {
        _variable = variable;
        _operation = operation;
        _expression = expression;
    }

    #endregion

    #region Private members

    private readonly string? _variable;
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
        _variable.NotUsed();
        _operation.NotUsed();
        return _expression.Compute (context);
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
        base.DumpHierarchyItem (name, level, writer, ToString());

        DumpHierarchyItem ("Variable", level + 1, writer, _variable.ToVisibleString());
        DumpHierarchyItem ("Operation", level + 1, writer, _operation.ToVisibleString());
        _expression.DumpHierarchyItem ("Expression", level + 1, writer);
    }

    #endregion
}
