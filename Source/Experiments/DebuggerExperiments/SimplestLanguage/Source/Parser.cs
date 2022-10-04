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
using System.Collections.Generic;
using System.Runtime.Serialization;

using AM;

#endregion

#nullable enable

namespace SimplestLanguage;

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
        Sure.NotNull (tokens);

        Tokens = tokens;
    }

    #endregion

    #region Private members

    private AstVariableReference ParseVariable()
    {
        return new AstVariableReference (new Token (TokenKind.None));
    }

    private AstValue ParseValue()
    {
        Tokens.RequireNext();

        return Tokens.Current.Kind switch
        {
            TokenKind.Identifier => ParseVariable(),
            TokenKind.NumericLiteral => new AstNumber(Tokens.Current),
            _ => throw new SyntaxException()
        };
    }

    private AstOperation ParseExpression()
    {
        var leftHand = ParseVariable();
        var operationCode = '+';
        var rightHand = ParseVariable();

        return new AstOperation (leftHand, operationCode, rightHand);
    }

    private AstAssignment ParseAssignment()
    {
        var targetName = ParseVariable().Name;
        var expression = ParseExpression();

        return new AstAssignment
            (
                targetName,
                expression
            );
    }

    /// <summary>
    /// Разбор вызова процедуры.
    /// </summary>
    private AstNode ParseCall()
    {
        Tokens.RequireNext (TokenKind.Identifier);
        var arguments = new List<AstValue>();
        var procedureName = Tokens.Current.Text.ThrowIfNullOrEmpty();
        Tokens.RequireNext (TokenKind.LeftParenthesis);
        while (Tokens.Peek() != TokenKind.RightParenthesis)
        {
            Tokens.RequireNext();
        }
        Tokens.RequireNext (TokenKind.RightParenthesis);
        Tokens.RequireNext (TokenKind.Semicolon);

        return new AstCall (procedureName, arguments);
    }

    /// <summary>
    /// Разбор выражения верхнего уровня.
    /// </summary>
    private AstNode ParseStatement() =>
        Tokens.CheckThat (t => t.Length >= 2).Peek (1)
            switch
            {
                TokenKind.Equals => ParseAssignment(),
                _ => ParseCall()
            };

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
    }

    #endregion
}
