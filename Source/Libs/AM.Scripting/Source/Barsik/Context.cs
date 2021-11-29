// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Context.cs -- контекст исполнения программы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Collections;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Контекст исполнения программы, в т. ч. значания переменных.
    /// </summary>
    public sealed class Context
    {
        #region Properties

        /// <summary>
        /// Родительский контекст.
        /// </summary>
        public Context? Parent { get; }

        /// <summary>
        /// Функции.
        /// </summary>
        public Dictionary<string, FunctionDescriptor> Functions { get; }

        /// <summary>
        /// Переменные.
        /// </summary>
        public Dictionary<string, dynamic?> Variables { get; }

        /// <summary>
        /// Выходной поток.
        /// </summary>
        public TextWriter Output { get; }

        /// <summary>
        /// Используемые пространства имен.
        /// </summary>
        public Dictionary<string, object?> Namespaces { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Context
            (
                Dictionary<string, dynamic?> variables,
                TextWriter output,
                Context? parent = null
            )
        {
            Parent = parent;
            Functions = new ();
            Variables = variables;
            Output = output;
            Namespaces = new ();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Дамп переменных.
        /// </summary>
        public void DumpVariables()
        {
            var keys = Variables.Keys.ToArray();
            if (keys.IsNullOrEmpty())
            {
                Output.WriteLine ("(no variables)");
                return;
            }

            Array.Sort (keys);
            foreach (var key in keys)
            {
                var value = Variables[key];
                if (value is null)
                {
                    Output.WriteLine ($"{key}: (null)");
                }
                else
                {
                    Output.WriteLine ($"{key}: {value.GetType().Name} = {value}");
                }
            }
        }

        /// <summary>
        /// Поиск переменной в текущем и в родительских контекстах.
        /// </summary>
        public bool TryGetVariable
            (
                string name,
                out dynamic? value
            )
        {
            Sure.NotNullNorEmpty (name);

            if (Variables.TryGetValue (name, out value))
            {
                return true;
            }

            for (var context = Parent; context is not null; context = context.Parent)
            {
                if (context.Variables.TryGetValue (name, out value))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
