// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* FunctionDefiniton.cs -- определение функции в скрипте Барсик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Определение функции в скрипте Барсик.
    /// </summary>
    sealed class FunctionDefinition
    {
        #region Construction

        public FunctionDefinition
            (
                string name,
                IEnumerable<string> arguments,
                IEnumerable<StatementNode> body
            )
        {
            _name = name;
            _arguments = arguments.ToArray();
            _body = new List<StatementNode> (body);
        }

        #endregion

        #region Private members

        // имя функции
        private readonly string _name;

        // имена аргументов
        private readonly string[] _arguments;

        // тело функции
        private readonly List<StatementNode> _body;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание точки вызова.
        /// </summary>
        public Func<Context, dynamic?[], dynamic?> CreateCallPoint()
        {
            // TODO implement

            return (context, _) =>
            {
                context.Output.WriteLine ("This is a stub");

                return "(null)";
            };
        }

        /// <summary>
        /// Выполнение функции.
        /// </summary>
        public void Execute (Context context)
        {
            foreach (var statement in _body)
            {
                statement.Execute (context);
            }
        }

        #endregion
    }
}
