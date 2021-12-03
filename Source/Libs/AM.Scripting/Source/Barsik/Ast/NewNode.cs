// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* NewNode.cs -- оператор new
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Оператор new.
    /// </summary>
    sealed class NewNode : AtomNode
    {
        public NewNode(string typeName, IEnumerable<AtomNode>? arguments)
        {
            _typeName = typeName;
            _arguments = new ();
            if (arguments is not null)
            {
                _arguments.AddRange (arguments);
            }
        }

        private readonly string _typeName;
        private readonly List<AtomNode> _arguments;

        public override dynamic? Compute (Context context)
        {
            var type = context.FindType (_typeName);
            if (type is null)
            {
                context.Error.WriteLine ($"Type {_typeName} not found");
                return null;
            }

            object? result;
            if (_arguments.Count == 0)
            {
                result = Activator.CreateInstance (type);
            }
            else
            {
                var parameters = new List<object?>();
                foreach (var argument in _arguments)
                {
                    var parameter = (object?) argument.Compute (context);
                    parameters.Add (parameter);
                }

                result = Activator.CreateInstance (type, parameters.ToArray());
            }

            return result;
        }
    }
}
