// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ForStatement.cs -- цикл for
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Цикл for.
/// </summary>
public sealed class ForStatement
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ForStatement
        (
            int line,
            ExpressionNode init,
            ExpressionNode condition,
            ExpressionNode advance,
            StatementBase block
        )
        : base(line)
    {
        _init = init;
        _condition = condition;
        _advance = advance;
        _block = block;
    }

    #endregion

    #region Private members

    private readonly ExpressionNode _init;
    private readonly ExpressionNode _condition;
    private readonly ExpressionNode _advance;
    private readonly StatementBase _block;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        // TODO реализовать
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

        _init.DumpHierarchyItem ("Init", level + 1, writer);
        _condition.DumpHierarchyItem ("Condition", level + 1, writer);
        _advance.DumpHierarchyItem ("Advance", level + 1, writer);
        _block.DumpHierarchyItem ("Block", level + 1, writer);
    }

    #endregion
}
