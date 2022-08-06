// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* IfNode.cs -- условный оператор if-then-else
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable


namespace AM.Scripting.Barsik;

/// <summary>
/// Условный оператор if-then-else.
/// </summary>
internal sealed class IfNode
    : StatementNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IfNode
        (
            SourcePosition position,
            AtomNode condition,
            IEnumerable<StatementNode> thenBlock,
            IEnumerable<IfNode>? elseIfBlocks,
            IEnumerable<StatementNode>? elseBlock
        )
        : base (position)
    {
        _condition = condition;
        _thenBlock = new (thenBlock);
        if (elseIfBlocks is not null)
        {
            _elseIfBlocks = new (elseIfBlocks);
        }

        if (elseBlock is not null)
        {
            _elseBlock = new (elseBlock);
        }
    }

    #endregion

    #region Private members

    private readonly AtomNode _condition;
    private readonly List<StatementNode> _thenBlock;
    private readonly List<IfNode>? _elseIfBlocks;
    private readonly List<StatementNode>? _elseBlock;

    #endregion

    #region AstNode members

    /// <inheritdoc cref="StatementNode.Execute"/>
    public override void Execute
        (
            Context context
        )
    {
        PreExecute (context);

        if (BarsikUtility.ToBoolean (_condition.Compute (context)))
        {
            foreach (var statement in _thenBlock)
            {
                statement.Execute (context);
            }
        }
        else
        {
            var handled = false;

            if (_elseIfBlocks is not null)
            {
                foreach (var block in _elseIfBlocks)
                {
                    if (BarsikUtility.ToBoolean (block._condition.Compute (context)))
                    {
                        foreach (var statement in block._thenBlock)
                        {
                            statement.Execute (context);
                        }

                        handled = true;
                    }
                }
            }

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

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.AppendLine ($"If: {_condition}");
        builder.Append ("Then: ");
        foreach (var statement in _thenBlock)
        {
            builder.AppendLine (statement.ToString());
        }

        if (_elseIfBlocks is not null)
        {
            foreach (var block in _elseIfBlocks)
            {
                builder.AppendLine ($"Else if: {block._condition}");
                foreach (var statement in block._thenBlock)
                {
                    builder.AppendLine (statement.ToString());
                }
            }
        }

        if (_elseBlock is not null)
        {
            foreach (var statement in _elseBlock)
            {
                builder.AppendLine (statement.ToString());
            }
        }

        return builder.ReturnShared();
    }

    #endregion
}
