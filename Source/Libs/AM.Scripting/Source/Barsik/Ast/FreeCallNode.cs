// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* FreeCallNode.cs -- вызов свободной функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Вызов свободной функции.
    /// </summary>
    sealed class FreeCallNode : AtomNode
    {
        #region Construction

        public FreeCallNode (string name, IEnumerable<AtomNode>? arguments)
        {
            _name = name;
            _arguments = new ();
            if (arguments is not null)
            {
                _arguments.AddRange (arguments);
            }
        }

        #endregion

        #region Private members

        private readonly string _name;
        private readonly List<AtomNode> _arguments;
        private FunctionDescriptor? _function;

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            _function ??= context.GetFunction (_name);

            var args = new List<dynamic?>();
            foreach (var node in _arguments)
            {
                var arg = node.Compute (context);
                args.Add (arg);
            }

            var result = _function.CallPoint (context, args.ToArray());

            return result;
        }

        #endregion

        #region Object members

        public override string ToString()
        {
            var builder = StringBuilderPool.Shared.Get();
            builder.Append ($"function '{_name}' (");
            var first = true;
            foreach (var node in _arguments)
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                builder.Append (node);

                first = false;
            }
            builder.Append (')');

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        #endregion
    }
}
