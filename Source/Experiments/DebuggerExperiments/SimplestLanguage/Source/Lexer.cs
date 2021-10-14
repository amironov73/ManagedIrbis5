// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* Lexer.cs -- лексер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Лексер.
    /// </summary>
    public sealed class Lexer
    {
        #region Private members

        private static readonly char[] Identifier =
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
            'y', 'z',

            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
            'Y', 'Z',

            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_'
        };

        #endregion

        #region Public methods

        /// <summary>
        /// Разбиение исходного текста на токены.
        /// </summary>
        public TokenList Tokenize
            (
                string text
            )
        {
            var tokens = new List<Token>();
            var navigator = new ValueTextNavigator (text);

            while (!navigator.SkipWhitespace())
            {
                var c = navigator.ReadChar();
                Token? token = null;

                switch (c)
                {
                    case '+':
                        token = new Token (TokenKind.Plus);
                        break;

                    case '-':
                        token = new Token (TokenKind.Minus);
                        break;

                    case '=':
                        token = new Token (TokenKind.Equals);
                        break;

                    case '(':
                        token = new Token (TokenKind.LeftParenthesis);
                        break;

                    case ')':
                        token = new Token (TokenKind.RightParenthesis);
                        break;

                    case ';':
                        token = new Token (TokenKind.Semicolon);
                        break;

                    default:
                        if (c.IsArabicDigit())
                        {
                            navigator.Move (-1);
                            token = new Token
                                (
                                    TokenKind.NumericLiteral,
                                    navigator.ReadInteger().ToString()
                                );
                        }

                        else if (c.IsLatinLetter())
                        {
                            navigator.Move (-1);
                            token = new Token
                                (
                                    TokenKind.Identifier,
                                    navigator.ReadWhile (Identifier).ToString()
                                );
                        }

                        break;
                }

                if (token is null)
                {
                    throw new SyntaxException ("Syntax error");
                }

                tokens.Add (token);

            } // while

            return new TokenList (tokens);

        } // method Tokenize

        #endregion


    } // class Lexer

} // namespace SimplestLanguage
