// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* BarsikTokenizer.cs -- токенайзер для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Токенайзер для Барсика.
/// </summary>
public static class BarsikTokinizer
{
    #region Private members

    private static readonly Dictionary<char, TokenKind> _simple = new ()
    {
        ['.'] = TokenKind.Dot,
        [','] = TokenKind.Comma,
        [':'] = TokenKind.Colon,
        [';'] = TokenKind.Semicolon,
        ['!'] = TokenKind.Bang,
        ['?'] = TokenKind.Question,
        ['('] = TokenKind.OpenBracket,
        [')'] = TokenKind.CloseBracket,
        ['['] = TokenKind.OpenSquareBracket,
        [']'] = TokenKind.CloseSquareBracket,
        ['{'] = TokenKind.OpenBrace,
        ['}'] = TokenKind.CloseBrace,
        ['<'] = TokenKind.Less,
        ['>'] = TokenKind.More,
        ['='] = TokenKind.Equal,
        ['+'] = TokenKind.Plus,
        ['-'] = TokenKind.Minus,
        ['*'] = TokenKind.Star,
        ['/'] = TokenKind.Slash,
        ['%'] = TokenKind.Percent,
        ['&'] = TokenKind.Ampersand,
        ['@'] = TokenKind.At,
        ['^'] = TokenKind.Circumflex,
        ['_'] = TokenKind.Underscore,
        ['`'] = TokenKind.Grave,
        ['|'] = TokenKind.VerticalLine,
        ['$'] = TokenKind.Dollar,
        ['#'] = TokenKind.NumberSign,
        ['~'] = TokenKind.Tilda,
        ['\\'] = TokenKind.Backslash
    };

    private static readonly TokenKind[] _digraphs =
    {
        TokenKind.Plus, TokenKind.Equal, TokenKind.PlusEqual,
        TokenKind.Plus, TokenKind.Plus, TokenKind.PlusPlus,
        TokenKind.Minus, TokenKind.Equal, TokenKind.MinusEqual,
        TokenKind.Minus, TokenKind.Minus, TokenKind.Minus,
        TokenKind.Star, TokenKind.Equal, TokenKind.StarEqual,
        TokenKind.Slash, TokenKind.Equal, TokenKind.SlashEqual,
        TokenKind.Equal, TokenKind.Equal, TokenKind.EqualEqual,
        TokenKind.Less, TokenKind.Equal, TokenKind.LessEqual,
        TokenKind.More, TokenKind.Equal, TokenKind.MoreEqual,
        TokenKind.Less, TokenKind.More, TokenKind.LessMore,
        TokenKind.Bang, TokenKind.Equal, TokenKind.NotEqual,
        TokenKind.Less, TokenKind.Less, TokenKind.LessLess,
        TokenKind.More, TokenKind.More, TokenKind.MoreMore,
        TokenKind.Ampersand, TokenKind.Equal, TokenKind.AmpersandEqual,
        TokenKind.Ampersand, TokenKind.Ampersand, TokenKind.AmpersandAmpersand,
        TokenKind.VerticalLine, TokenKind.Equal, TokenKind.VerticalEqual,
        TokenKind.VerticalLine, TokenKind.VerticalLine, TokenKind.VerticalVertical,
        TokenKind.Percent, TokenKind.PercentEqual, TokenKind.Percent,
        TokenKind.EqualEqual, TokenKind.Equal, TokenKind.EqualEqualEqual
    };

    private static BarsikToken _ParseIdentifier
        (
            TextNavigator navigator
        )
    {
        var start = navigator.Position;
        while (!navigator.IsEOF)
        {
            var chr = navigator.PeekChar();
            if (chr <= ' ')
            {
                break;
            }

            navigator.ReadChar();
        }

        var end = navigator.Position;
        var value = navigator.Text.AsMemory (start, end - start + 1);
        var result = new BarsikToken (TokenKind.Identifier, value);

        return result;
    }

    private static void _FilterTokens
        (
            List<BarsikToken> tokens
        )
    {
        int step;
        for (var i = 0; i < tokens.Count - 1; i += step)
        {
            var current = tokens[i];
            var next = tokens[i + 1].Kind;

            step = 1;
            for (var j = 0; j < _digraphs.Length; j += 3)
            {
                if (current.Kind == _digraphs[j] && next == _digraphs[j + 1])
                {
                    current.Kind = _digraphs[j + 2];
                    tokens.RemoveAt (i + 1);
                    step = 0;
                    break;
                }
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбиение исходного текста на токены.
    /// </summary>
    public static List<BarsikToken> Tokenize
        (
            string text
        )
    {
        var navigator = new TextNavigator (text);
        var result = new List<BarsikToken>();

        while (!navigator.IsEOF)
        {
            BarsikToken? token;

            navigator.SkipWhitespace();
            if (navigator.IsEOF)
            {
                break;
            }

            var chr = navigator.ReadChar();
            if (_simple.TryGetValue (chr, out var kind))
            {
                token = new BarsikToken
                    (
                        kind,
                        text.AsMemory(navigator.Position - 1, 1)
                    );
                result.Add (token);
                continue;
            }

            token = _ParseIdentifier (navigator);
            result.Add (token);
        }

        _FilterTokens (result);

        return result;
    }

    #endregion
}
