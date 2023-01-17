// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* IfNode.cs -- условный оператор if-then-else
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Условный оператор if-then-else.
/// </summary>
public sealed class IfNode
    : StatementBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IfNode
        (
            int line,
            AtomNode condition,
            Block thenBlock,
            Block? elseBlock
        )
        : base (line)
    {
        _condition = condition;
        _thenBlock = thenBlock;

        if (elseBlock is not null)
        {
            _elseBlock = elseBlock;
        }
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly Block _thenBlock;
    private readonly Block? _elseBlock;

    #endregion

    #region StatementBase members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        if (KotikUtility.ToBoolean (_condition.Compute (context)))
        {
            foreach (var statement in _thenBlock)
            {
                statement.Execute (context);
            }
        }
        else
        {
            var handled = false;

            // if (_elseIfBlocks is not null)
            // {
            //     foreach (var block in _elseIfBlocks)
            //     {
            //         if (BarsikUtility.ToBoolean (block._condition.Compute (context)))
            //         {
            //             foreach (var statement in block._thenBlock)
            //             {
            //                 statement.Execute (context);
            //             }
            //
            //             handled = true;
            //         }
            //     }
            // }

            if (!handled && _elseBlock is not null)
            {
                foreach (var statement in _elseBlock)
                {
                    statement.Execute (context);
                }
            }
        }
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

        _condition.DumpHierarchyItem ("Condition", level + 1, writer);
        _thenBlock.DumpHierarchyItem ("Then", level + 1, writer);
        if (_elseBlock is not null)
        {
            _elseBlock.DumpHierarchyItem ("Else", level + 1, writer);
        }
    }

    #endregion

}
