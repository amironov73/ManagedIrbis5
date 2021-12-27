// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ForNode.cs -- цикл for
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Цикл for.
/// </summary>
sealed class ForNode : StatementNode
{
    public ForNode
        (
            StatementNode init,
            AtomNode condition,
            StatementNode step,
            IEnumerable<StatementNode>? body,
            IEnumerable<StatementNode>? elseBody
        )
    {
        _init = init;
        _condition = condition;
        _step = step;
        _body = new ();
        if (body is not null)
        {
            _body.AddRange (body);
        }

        if (elseBody is not null)
        {
            _else = new List<StatementNode> (elseBody);
        }
    }

    private readonly StatementNode _init;
    private readonly AtomNode _condition;
    private readonly StatementNode _step;
    private readonly List<StatementNode> _body;
    private readonly List<StatementNode>? _else;

    public override void Execute (Context context)
    {
        PreExecute (context);

        _init.Execute (context);
        var success = false;
        while (BarsikUtility.ToBoolean (_condition.Compute (context)))
        {
            success = true;
            foreach (var statement in _body)
            {
                statement.Execute (context);
            }

            _step.Execute (context);
        }

        if (!success && _else is not null)
        {
            foreach (var statement in _else)
            {
                statement.Execute (context);
            }
        }
    }
}
