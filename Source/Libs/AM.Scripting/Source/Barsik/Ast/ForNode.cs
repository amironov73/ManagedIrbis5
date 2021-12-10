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

using System.Collections;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable


namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Цикл for.
    /// </summary>
    sealed class ForNode : StatementNode
    {
        public ForNode(StatementNode init, AtomNode condition,
            StatementNode step, IEnumerable<StatementNode>? body)
        {
            _init = init;
            _condition = condition;
            _step = step;
            _body = new ();
            if (body is not null)
            {
                _body.AddRange (body);
            }
        }

        private readonly StatementNode _init;
        private readonly AtomNode _condition;
        private readonly StatementNode _step;
        private readonly List<StatementNode> _body;

        public override void Execute (Context context)
        {
            PreExecute (context);

            _init.Execute (context);
            while (_condition.Compute (context))
            {
                foreach (var statement in _body)
                {
                    statement.Execute (context);
                }

                _step.Execute (context);
            }
        }
    }
}
