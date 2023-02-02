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

using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Условный оператор if-then-else.
/// </summary>
/// <remarks>
/// Структура условного оператора:
/// <code>
/// if (condition)
/// {
///  // блок then
/// }
/// else if (condition)
/// {
///  // произвольное количество (включая 0) блоков else if
/// }
/// else
/// {
///   опциональный блок else
/// }
/// </code>
/// </remarks>
internal sealed class IfNode
    : StatementBase,
    IStatementBlock
{
    #region Properties

    /// <inheritdoc cref="IStatementBlock.Directives"/>
    IList<DirectiveNode> IStatementBlock.Directives
    {
        get => _summary.Directives;
        set => _summary.Directives = value;
    }

    /// <inheritdoc cref="IStatementBlock.Functions"/>
    IList<FunctionDefinitionNode> IStatementBlock.Functions
    {
        get => _summary.Functions;
        set => _summary.Functions = value;
    }

    /// <inheritdoc cref="IStatementBlock.Locals"/>
    IList<LocalNode> IStatementBlock.Locals
    {
        get => _summary.Locals;
        set => _summary.Locals = value;
    }

    /// <inheritdoc cref="IStatementBlock.Statements"/>
    IList<StatementBase> IStatementBlock.Statements
    {
        get => _summary.Statements;
        set => _summary.Statements = value;
    }

    #endregion
    
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IfNode
        (
            int line,
            AtomNode condition,
            StatementBase thenBlock,
            IList<IfNode>? elseIfArray,
            StatementBase? elseBlock
        )
        : base (line)
    {
        Sure.NotNull (condition);
        Sure.NotNull (thenBlock);

        _condition = condition;
        _elseIfArray = elseIfArray;
        _thenBlock = thenBlock;
        _elseBlock = elseBlock;

        if (elseBlock is not null || elseIfArray is not null)
        {
            // сооружаем псевдоблок, хранящий в себе стейтменты из двух блоков
            var summary = new List<StatementBase>();
            summary.AddRange (((IStatementBlock) thenBlock).Statements);
            if (elseIfArray is not null)
            {
                foreach (var node in elseIfArray)
                {
                    summary.AddRange (((IStatementBlock) node).Statements);
                }
            }

            if (elseBlock is not null)
            {
                summary.AddRange (((IStatementBlock) elseBlock).Statements);
            }
            
            _summary = new BlockNode (0, summary);
        }
        else
        {
            _summary = (BlockNode) thenBlock;
        }

        ((IStatementBlock) _summary).RefineStatements();
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly StatementBase _thenBlock;
    private readonly IList<IfNode>? _elseIfArray;
    private readonly StatementBase? _elseBlock;
    private readonly BlockNode _summary;

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
            _thenBlock.Execute (context);
        }
        else
        {
            var handled = false;

            if (_elseIfArray is not null)
            {
                foreach (var block in _elseIfArray)
                {
                    if (KotikUtility.ToBoolean (block._condition.Compute (context)))
                    {
                        block._thenBlock.Execute (context);
                        handled = true;
                    }
                }
            }

            if (!handled)
            {
                _elseBlock?.Execute (context);
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
        if (_elseIfArray is not null)
        {
            foreach (var elseIf in _elseIfArray)
            {
                elseIf.DumpHierarchyItem ("ElseIf", level + 1, writer);
            }
        }

        _elseBlock?.DumpHierarchyItem ("Else", level + 1, writer);
    }

    #endregion
}
