// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchQueryLexer.cs -- лексический анализатор для поискового запроса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Лексический анализатор для поискового запроса.
/// </summary>
public static class SearchQueryLexer
{
    #region Private members

    static readonly char[] _stopChars =
    {
        '(', '/', '\t', ' ', ',', ')', '+', '*'
    };

    private static string _TermTail
        (
            TextNavigator navigator
        )
    {
        var result = navigator.ReadUntil(_stopChars).ToString();
        while (navigator.PeekChar() == '/')
        {
            var c2 = navigator.LookAhead(1);
            if (c2 == '(' || c2 == '\0')
            {
                break;
            }
            result = result
                     + navigator.ReadChar()
                     + navigator.ReadUntil(_stopChars);
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

        var result = new List<SearchToken>();
        var navigator = new TextNavigator(text);

        while (!navigator.IsEOF)
        {
            navigator.SkipWhitespace();
            if (navigator.IsEOF)
            {
                break;
            }

            var c = navigator.ReadChar();
            string value;
            var position = navigator.Position;
            SearchTokenKind kind;
            switch (c)
            {
                case '"':
                    value = navigator.ReadUntil('"').ToString();
                    kind = SearchTokenKind.Term;
                    if (navigator.ReadChar() != '"')
                    {
                        Magna.Logger.LogError
                            (
                                nameof (SearchQueryLexer) + "::" + nameof (Tokenize)
                                + ": unclosed string: {Text}",
                                text
                            );

                        throw new SearchSyntaxException ($"Unclose string: {text}");
                    }

                    var saved = navigator.SavePosition();
                    var tail = navigator.ReadUntil('"').ToString();
                    while (!string.IsNullOrEmpty(tail))
                    {
                        if (navigator.ReadChar() != '"')
                        {
                            navigator.RestorePosition(saved);
                            break;
                        }

                        var trimmed = tail.TrimStart();
                        var c2 = trimmed.FirstChar();
                        if (tail.StartsWith("(F)")
                            || tail.StartsWith("(G)")
                            || c2.IsOneOf('+', '*', '^', '.', ',', ')', '/'))
                        {
                            navigator.RestorePosition(saved);
                            break;
                        }
                        value = value + '"' + tail;
                        saved = navigator.SavePosition();
                        while (navigator.PeekChar() == '"')
                        {
                            value += navigator.ReadChar();
                            saved = navigator.SavePosition();
                        }

                        tail = navigator.ReadUntil('"').ToString();
                    }
                    break;

                case '<':
                    if (navigator.PeekString(2).ToString() != ".>")
                    {
                        throw new SearchSyntaxException();
                    }

                    navigator.ReadChar();
                    navigator.ReadChar();

                    value = navigator.ReadUntil("<.>").ToString();
                    kind = SearchTokenKind.Term;
                    if (navigator.ReadString(3).ToString() != "<.>")
                    {
                        Magna.Logger.LogError
                            (
                                nameof (SearchQueryLexer) + "::" + nameof (Tokenize)
                                + ": unclosed string: {Text}",
                                text
                            );

                        throw new SearchSyntaxException ($"Unclosed string: {text}");
                    }
                    break;

                case '#':
                    value = navigator.ReadWhile('0', '1', '2', '3', '4', '5', '6', '7', '8', '9').ToString();
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
                    if (navigator.PeekChar().IsOneOf('(', '\0'))
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
                    string preview = c + navigator.PeekString(2).ToString();
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

            var token = new SearchToken(kind, position, value);

            result.Add(token);
        }

        return new SearchTokenList(result);
    }

    #endregion

} // class SearchQueryLexer

// namespace ManagedIrbis.Infrastructure
