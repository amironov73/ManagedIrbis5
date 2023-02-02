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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Цикл for.
/// </summary>
internal sealed class ForNode
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
    public ForNode
        (
            int line,
            AtomNode init,
            AtomNode condition,
            AtomNode step,
            StatementBase body,
            StatementBase? elseBody
        )
        : base(line)
    {
        _init = init;
        _condition = condition;
        _step = step;
        _body = body;
        _elseBody = elseBody;

        if (elseBody is not null)
        {
            // сооружаем псевдоблок, хранящий в себе стейтменты из двух блоков
            var summary = new List<StatementBase>();
            summary.AddRange (((IStatementBlock) body).Statements);
            summary.AddRange (((IStatementBlock) elseBody).Statements);
            _summary = new BlockNode (0, summary);
        }
        else
        {
            _summary = (BlockNode) body;
        }

        ((IStatementBlock) _summary).RefineStatements();
    }

    #endregion

    #region Private members

    private readonly AtomNode _init;
    private readonly AtomNode _condition;
    private readonly AtomNode _step;
    private readonly StatementBase _body;
    private readonly StatementBase? _elseBody;
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

        _init.Compute (context);
        var success = false;
        try
        {
            while (KotikUtility.ToBoolean (_condition.Compute (context)))
            {
                success = true;
                try
                {
                    _body.Execute (context);
                }
                catch (ContinueException)
                {
                    Debug.WriteLine ("for-continue");
                }

                _step.Compute (context);
            }

            if (!success && _elseBody is not null)
            {
                _elseBody.Execute (context);
            }

        }
        catch (BreakException)
        {
            Debug.WriteLine ("for-break");
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

        _init.DumpHierarchyItem ("Init", level + 1, writer);
        _condition.DumpHierarchyItem ("Condition", level + 1, writer);
        _step.DumpHierarchyItem ("Advance", level + 1, writer);
        _body.DumpHierarchyItem ("Block", level + 1, writer);
    }

    #endregion
}
