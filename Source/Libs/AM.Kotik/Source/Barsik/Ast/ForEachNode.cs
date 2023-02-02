// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ForEachNode.cs -- цикл foreach
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Цикл foreach.
/// </summary>
internal sealed class ForEachNode
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
    public ForEachNode
        (
            int line,
            string variableName,
            AtomNode enumerable,
            StatementBase body,
            StatementBase? elseBody
        )
        : base (line)
    {
        Sure.NotNullNorEmpty (variableName);
        Sure.NotNull (enumerable);

        _variableName = variableName;
        _enumerable = enumerable;
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

    private readonly string _variableName;
    private readonly AtomNode _enumerable;
    private readonly StatementBase _body;
    private readonly StatementBase? _elseBody;
    private readonly BlockNode _summary;

    #endregion

    #region StatementNode members

    /// <inheritdoc cref="StatementBase.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        var enumerable = _enumerable.Compute (context);
        if (enumerable is null or not IEnumerable)
        {
            return;
        }

        try
        {
            var success = false;
            foreach (var value in enumerable)
            {
                success = true;
                context.Variables[_variableName] = value;
                try
                {
                    _body.Execute (context);
                }
                catch (ContinueException)
                {
                    Debug.WriteLine ("foreach-continue");
                }
            }

            if (!success && _elseBody is not null)
            {
                _elseBody.Execute (context);
            }
        }
        catch (BreakException)
        {
            Debug.WriteLine ("foreach-break");
        }
    }

    #endregion
}
