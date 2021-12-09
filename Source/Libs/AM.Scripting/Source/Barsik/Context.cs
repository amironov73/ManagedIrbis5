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
using AM.IO;

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
        /// Стандартный входной поток.
        /// </summary>
        public TextReader Input { get; }

        /// <summary>
        /// Стандартный выходной поток.
        /// </summary>
        public TextWriter Output { get; private set; }

        /// <summary>
        /// Выходной поток ошибок.
        /// </summary>
        public TextWriter Error { get; }

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
                TextReader input,
                TextWriter output,
                TextWriter error,
                Context? parent = null
            )
        {
            Parent = parent;
            Functions = new ();
            Variables = variables;
            Input = input;
            Output = output;
            Error = error;
            Namespaces = new ();
        }

        #endregion

        #region Private members

        /// <summary>
        /// Делаем контекст внимательным к выводу текста.
        /// </summary>
        internal void MakeAttentive()
        {
            if (Output is not AttentiveWriter)
            {
                Output = new AttentiveWriter (Output);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создание контекста-потомка.
        /// </summary>
        public Context CreateChild ()
        {
            return new Context
                (
                    new (),
                    Input,
                    Output,
                    Error,
                    this
                );
        }

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
        /// Получение типа по его имени.
        /// </summary>
        public Type? FindType
            (
                string name
            )
        {
            Type? result = Type.GetType (name, false);
            if (result is not null)
            {
                return result;
            }

            foreach (var ns in Namespaces.Keys)
            {
                var fullName = ns + "." + name;
                result = Type.GetType (fullName, false);
                if (result is not null)
                {
                    return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Поиск функции в текущем и в родительском контекстах.
        /// </summary>
        public FunctionDescriptor GetFunction
            (
                string name
            )
        {
            Sure.NotNullNorEmpty (name);

            if (Functions.TryGetValue (name, out var result))
            {
                return result;
            }

            for (var context = Parent; context is not null; context = context.Parent)
            {
                if (context.Functions.TryGetValue (name, out result))
                {
                    return result;
                }
            }

            if (Builtins.Registry.TryGetValue (name, out result))
            {
                return result;
            }

            throw new Exception ($"Function {name} not found");
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
