// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TryNode.cs -- блок try-catch-finally
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Блок try-catch-finally
/// </summary>
internal sealed class TryNode
    : StatementBase,
    IStatementBlock
{
    #region NestedTypes

    internal sealed class CatchBlock
        : AstNode
    {
        #region Properties

        public string Name { get; }

        public StatementBase Body { get; }

        #endregion

        #region Construction

        public CatchBlock
            (
                string name,
                StatementBase body
            )
        {
            Name = name;
            Body = body;
        }

        #endregion
    }

    #endregion

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
    public TryNode
        (
            int line,
            StatementBase tryBlock,
            CatchBlock? catchBlock,
            StatementBase? finallyBlock
        )
        : base(line)
    {
        Sure.NotNull (tryBlock);

        _tryBlock = (BlockNode) tryBlock;
        _variableName = catchBlock?.Name;
        _catchBlock = (BlockNode?) catchBlock?.Body;
        _finallyBlock = (BlockNode?) finallyBlock;

        if (catchBlock is not null || finallyBlock is not null)
        {
            // сооружаем псевдоблок, хранящий в себе стейтменты из двух блоков
            var summary = new List<StatementBase>();
            summary.AddRange (((IStatementBlock) tryBlock).Statements);

            if (catchBlock is not null)
            {
                summary.AddRange (((IStatementBlock) catchBlock.Body).Statements);
            }

            if (finallyBlock is not null)
            {
                summary.AddRange (((IStatementBlock) finallyBlock).Statements);
            }

            _summary = new BlockNode (0, summary);
        }
        else
        {
            _summary = (BlockNode) tryBlock;
        }

        ((IStatementBlock) _summary).RefineStatements();
    }

    #endregion

    #region Private members

    private readonly BlockNode _tryBlock;
    private readonly string? _variableName;
    private readonly BlockNode? _catchBlock;
    private readonly BlockNode? _finallyBlock;
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

        try
        {
            _tryBlock.Execute (context);
        }
        catch (Exception exception)
        {
            var nestedContext = context.CreateChildContext();
            if (_catchBlock is not null)
            {
                if (!string.IsNullOrEmpty (_variableName))
                {
                    nestedContext.Variables[_variableName] = exception;
                }

                _catchBlock.Execute (nestedContext);
            }
        }
        finally
        {
            _finallyBlock?.Execute (context);
        }
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer);

        _tryBlock.DumpHierarchyItem ("Try", level + 1, writer);
        if (!string.IsNullOrEmpty (_variableName))
        {
            DumpHierarchyItem ("Variable", level + 1, writer, _variableName);
        }

        _catchBlock?.DumpHierarchyItem ("Catch", level + 1, writer);
        _finallyBlock?.DumpHierarchyItem ("Finally", level + 1, writer);
    }

    #endregion
}
