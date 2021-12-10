// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ForEachNode.cs -- цикл foreach
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Цикл foreach.
    /// </summary>
    sealed class ForEachNode : StatementNode
    {
        public ForEachNode(string variableName, AtomNode enumerable,
            IEnumerable<StatementNode>? body)
        {
            _variableName = variableName;
            _enumerable = enumerable;
            _body = new ();
            if (body is not null)
            {
                _body.AddRange (body);
            }
        }

        private readonly string _variableName;
        private readonly AtomNode _enumerable;
        private readonly List<StatementNode> _body;

        public override void Execute (Context context)
        {
            PreExecute (context);

            var enumerable = _enumerable.Compute (context);
            if (enumerable is null || enumerable is not IEnumerable)
            {
                return;
            }

            foreach (var value in enumerable)
            {
                context.Variables[_variableName] = value;
                foreach (var statement in _body)
                {
                    statement.Execute (context);
                }
            }
        }
    }
}
