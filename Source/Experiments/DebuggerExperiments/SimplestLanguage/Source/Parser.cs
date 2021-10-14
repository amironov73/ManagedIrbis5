// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* Parser.cs -- парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Парсер.
    /// </summary>
    public sealed class Parser
    {
        #region Properties

        /// <summary>
        /// Обрабатываемые токены.
        /// </summary>
        public TokenList Tokens { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Parser
            (
                TokenList tokens
            )
        {
            Tokens = tokens;
        }

        #endregion

        #region Private members

        private AstVariableReference ParseVariable()
        {
            return new AstVariableReference(new Token(TokenKind.None));
        }

        private AstOperation ParseExpression()
        {
            var leftHand = ParseVariable();
            var operationCode = '+';
            var rightHand = ParseVariable();

            return new AstOperation (leftHand, operationCode, rightHand);

        } // method ParseExpression

        private AstAssignment ParseAssignment()
        {
            var targetName = ParseVariable().Name;
            var expression = ParseExpression();

            return new AstAssignment
                (
                    targetName,
                    expression
                );

        } // method ParseAssignment

        private AstNode ParseStatement()
        {
            return ParseAssignment();

        } // method ParseStatement

        private AstNode ParseCall()
        {
            return new AstCall("print", ArraySegment<AstValue>.Empty)
                ;
        } // method ParseCall

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор программы (начиная с самого верхнего уровня).
        /// </summary>
        public LanguageProgram Parse()
        {
            var result = new LanguageProgram();

            while (!Tokens.IsEof)
            {
                var statement = ParseStatement();
                result.Statements.Add (statement);
            }

            return result;

        } // method Parse

        #endregion

    } // class Parser

} // namespace SimplestLanguage
