// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ForNode.cs -- цикл for
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
public sealed class ForNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ForNode
        (
            int line,
            ExpressionNode init,
            ExpressionNode condition,
            ExpressionNode step,
            Block body
        )
        : base(line)
    {
        _init = init;
        _condition = condition;
        _step = step;
        _body = body;
    }

    #endregion

    #region Private members

    private readonly ExpressionNode _init;
    private readonly ExpressionNode _condition;
    private readonly ExpressionNode _step;
    private readonly Block _body;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        _init.Compute (context);
        // var success = false;
        while (KotikUtility.ToBoolean (_condition.Compute (context)))
        {
            // success = true;
            foreach (var statement in _body)
            {
                statement.Execute (context);
            }

            _step.Compute (context);
        }

        // if (!success && _else is not null)
        // {
        //     foreach (var statement in _else)
        //     {
        //         statement.Execute (context);
        //     }
        // }
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
        _step.DumpHierarchyItem ("Advance", level + 1, writer);
        _body.DumpHierarchyItem ("Block", level + 1, writer);
    }

    #endregion
}
