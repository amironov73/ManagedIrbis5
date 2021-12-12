// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* ProgramNode.cs -- корневой узел AST
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
    /// Корневой узел AST.
    /// </summary>
    public sealed class ProgramNode
        : AstNode
    {
        #region Properties

        /// <summary>
        /// Стейтменты программы.
        /// </summary>
        public List<StatementNode> Statements { get; }

        /// <summary>
        /// Директивы
        /// </summary>
        public List<Directive> Directives { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ProgramNode
            (
                IEnumerable<Directive>? directives,
                IEnumerable<StatementNode> statements
            )
        {
            Statements = new List<StatementNode> (statements);
            Directives = new ();
            if (directives is not null)
            {
                Directives.AddRange (directives);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Выполнение директив перед скриптом.
        /// </summary>
        internal void ExecuteDirectives
            (
                Context context
            )
        {
            foreach (var directive in Directives)
            {
                directive.Execute (context);
            }
        }

        #endregion

        #region AstNode members

        /// <summary>
        /// Выполнение действий, предусмотренных данной программой.
        /// </summary>
        /// <param name="context">Контекст исполнения программы.</param>
        public void Execute (Context context)
        {
            ExecuteDirectives (context);

            foreach (var statement in Statements)
            {
                statement.Execute (context);
            }
        }

        #endregion
    }
}
