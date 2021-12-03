// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* MethodNode.cs -- вызов функции-члена
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Вызов функции-члена.
    /// </summary>
    sealed class MethodNode : AtomNode
    {
        #region Construction

        public MethodNode(string thisName, string methodName, IEnumerable<AtomNode>? arguments)
        {
            _thisName = thisName;
            _methodName = methodName;
            _arguments = new ();
            if (arguments is not null)
            {
                _arguments.AddRange (arguments);
            }
        }

        #endregion

        #region Private members

        private readonly string _thisName;
        private readonly string _methodName;
        private readonly List<AtomNode> _arguments;

        private dynamic? ComputeStatic (Context context)
        {
            var type = context.FindType (_thisName);
            if (type is null)
            {
                context.Error.WriteLine ($"Variable or type {_thisName} not found");

                return null;
            }

            var method = type
                .GetMethods (BindingFlags.Public | BindingFlags.Static)
                .FirstOrDefault (info => info.Name == _methodName
                                         && info.GetParameters().Length == _arguments.Count);

            if (method is null)
            {
                return null;
            }

            var parameters = new List<object?>();
            foreach (var argument in _arguments)
            {
                var parameter = (object?) argument.Compute (context);
                parameters.Add (parameter);
            }

            var result = method.Invoke (null, parameters.ToArray());

            return result;
        }

        #endregion

        #region AtomNode members

        public override dynamic? Compute (Context context)
        {
            if (!context.TryGetVariable (_thisName, out var thisValue))
            {
                return ComputeStatic (context);
            }

            if (thisValue is null)
            {
                return null;
            }

            var argumentValues = new List<dynamic>();
            var argumentTypes = new List<Type>();
            foreach (var argument in _arguments)
            {
                var value = argument.Compute (context);
                argumentValues.Add (value);
                var argType = value is null
                    ? typeof (object)
                    : value.GetType();
                argumentTypes.Add (argType);
            }

            var type = ((object) thisValue).GetType();
            var method = type.GetMethod (_methodName, argumentTypes.ToArray());
            method ??= type.GetMethod (_methodName);
            if (method is null)
            {
                return null;
            }

            var result = method.Invoke (thisValue, argumentValues.ToArray());

            return result;
        }

        #endregion
    }
}
