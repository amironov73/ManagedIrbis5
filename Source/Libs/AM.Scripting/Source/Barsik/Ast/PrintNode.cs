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

using System.Collections;
using System.Collections.Generic;

using AM.Text;

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

        private void Print (IEnumerable sequence, Context context)
        {
            var first = true;
            foreach (var item in sequence)
            {
                if (!first)
                {
                    context.Output.Write (", ");
                }

                context.Output.Write (item);
                first = false;
            }
        }

        private void Print (AtomNode node, Context context)
        {
            var value = node.Compute (context);
            if (value is null)
            {
                context.Output.Write ("(null)");
                return;
            }

            if (value is string)
            {
                context.Output.Write (value);
                return;
            }

            var type = ((object) value).GetType();
            if (type.IsPrimitive)
            {
                context.Output.Write (value);
                return;
            }

            switch (value)
            {
                case IEnumerable sequence:
                    Print (sequence, context);
                    break;

                default:
                    context.Output.Write (value);
                    break;
            }
        }

        public override void Execute (Context context)
        {
            foreach (var node in _nodes)
            {
                Print (node, context);
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
