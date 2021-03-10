﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchQueryLexer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    ///
    /// </summary>
    public static class SearchQueryLexer
    {
        #region Private members

        static readonly char[] StopChars =
        {
            '(', '/', '\t', ' ', ',', ')', '+', '*'
        };

        private static string? _TermTail
            (
                TextNavigator navigator
            )
        {
            string result = navigator.ReadUntil(StopChars).ToString();
            while (navigator.PeekChar() == '/')
            {
                char c2 = navigator.LookAhead(1);
                if (c2 == '(' || c2 == '\0')
                {
                    break;
                }
                result = result
                    + navigator.ReadChar()
                    + navigator.ReadUntil(StopChars);
            }

            return result;
        }

        #endregion

        #region Public methods

        // =========================================================

        /// <summary>
        /// Tokenize the text.
        /// </summary>
        public static SearchTokenList Tokenize
            (
                string text
            )
        {
            Sure.NotNull(text, nameof(text));

            List<SearchToken> result = new List<SearchToken>();
            TextNavigator navigator = new TextNavigator(text);

            /*

            while (!navigator.IsEOF)
            {
                navigator.SkipWhitespace();
                if (navigator.IsEOF)
                {
                    break;
                }

                char c = navigator.ReadChar();
                string value;
                int position = navigator.Position;
                SearchTokenKind kind;
                switch (c)
                {
                    case '"':
                        value = navigator.ReadUntil('"');
                        kind = SearchTokenKind.Term;
                        if (navigator.ReadChar() != '"')
                        {
                            Magna.Error
                                (
                                    "SearchQueryLexer::Tokenize: "
                                    + "unclosed line"
                                );

                            throw new SearchSyntaxException();
                        }

                        TextPosition saved = navigator.SavePosition();
                        string tail = navigator.ReadUntil('"').ToString();
                        while (!ReferenceEquals(tail, null))
                        {
                            if (navigator.ReadChar() != '"')
                            {
                                navigator.RestorePosition(saved);
                                break;
                            }

                            string trimmed = tail.TrimStart();
                            char c2 = trimmed.FirstChar();
                            if (tail.StartsWith("(F)")
                                || tail.StartsWith("(G)")
                                || c2.OneOf('+', '*', '^', '.', ',', ')', '/'))
                            {
                                navigator.RestorePosition(saved);
                                break;
                            }
                            value = value + '"' + tail;
                            saved = navigator.SavePosition();
                            tail = navigator.ReadUntil('"');
                        }
                        break;

                    case '<':
                        if (navigator.PeekString(2) != ".>")
                        {
                            throw new SearchSyntaxException();
                        }

                        navigator.ReadChar();
                        navigator.ReadChar();

                        value = navigator.ReadUntil("<.>");
                        kind = SearchTokenKind.Term;
                        if (navigator.ReadString(3) != "<.>")
                        {
                            Magna.Error
                                (
                                    "SearchQueryLexer::Tokenize: "
                                    + "unclosed line"
                                );

                            throw new SearchSyntaxException();
                        }
                        break;

                    case '#':
                        value = navigator.ReadWhile('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')
                            .ThrowIfNull();
                        kind = SearchTokenKind.Hash;
                        break;

                    case '+':
                        value = c.ToString();
                        kind = SearchTokenKind.Plus;
                        break;

                    case '*':
                        value = c.ToString();
                        kind = SearchTokenKind.Star;
                        break;

                    case '^':
                        value = c.ToString();
                        kind = SearchTokenKind.Hat;
                        break;

                    case '.':
                        value = c.ToString();
                        kind = SearchTokenKind.Dot;
                        break;

                    case '/':
                        if (navigator.PeekChar().OneOf('(', '\0'))
                        {
                            value = c.ToString();
                            kind = SearchTokenKind.Slash;
                        }
                        else
                        {
                            value = c + _TermTail(navigator);
                            kind = SearchTokenKind.Term;
                        }
                        break;

                    case ',':
                        value = c.ToString();
                        kind = SearchTokenKind.Comma;
                        break;

                    case '(':
                        string preview = c + navigator.PeekString(2);
                        if (preview == "(G)" || preview == "(g)")
                        {
                            value = preview;
                            kind = SearchTokenKind.G;
                            navigator.ReadChar();
                            navigator.ReadChar();
                        }
                        else if (preview == "(F)" || preview == "(f)")
                        {
                            value = preview;
                            kind = SearchTokenKind.F;
                            navigator.ReadChar();
                            navigator.ReadChar();
                        }
                        else
                        {
                            value = c.ToString();
                            kind = SearchTokenKind.LeftParenthesis;
                        }
                        break;

                    case ')':
                        value = c.ToString();
                        kind = SearchTokenKind.RightParenthesis;
                        break;

                    default:
                        value = c + _TermTail(navigator);
                        kind = SearchTokenKind.Term;
                        break;
                }

                SearchToken token = new SearchToken(kind, position, value);

                result.Add(token);
            }

            */

            return new SearchTokenList(result);
        }

        #endregion

    } // class SearchQueryLexer

} // namespace ManagedIrbis.Infrastructure
