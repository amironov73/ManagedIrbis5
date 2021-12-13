// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* PrintNode.cs -- распечатка значений переменных и выражений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Распечатка значений переменных и выражений.
    /// </summary>
    sealed class PrintNode : StatementNode
    {
        public PrintNode (IEnumerable<AtomNode>? nodes, bool newLine)
        {
            _nodes = new List<AtomNode> ();
            if (nodes is not null)
            {
                _nodes.AddRange (nodes);
            }
            _newLine = newLine;
        }

        private readonly List<AtomNode> _nodes;
        private readonly bool _newLine;

        public override void Execute (Context context)
        {
            PreExecute (context);

            foreach (var node in _nodes)
            {
                context.Print (node);
            }

            if (_newLine)
            {
                context.Output.WriteLine();
            }
        }

        public override string ToString()
        {
            return "Print: " + string.Join (',', _nodes);
        }
    }
}
