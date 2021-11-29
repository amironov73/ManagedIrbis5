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

        public FunctionDefinition(IEnumerable<StatementNode> body)
        {
            _body = new List<StatementNode> (body);
        }

        #endregion

        #region Private members

        // тело функции
        private readonly List<StatementNode> _body;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание точки вызова.
        /// </summary>
        public Func<dynamic?[], dynamic?> CreateCallPoint()
        {
            // TODO implement

            throw new NotImplementedException();
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
