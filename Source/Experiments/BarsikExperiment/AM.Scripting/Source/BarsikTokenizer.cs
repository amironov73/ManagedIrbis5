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
public static class BarsikTokenizer
{
    #region Private members

    private static readonly Dictionary<char, string> _simple = new ()
    {
        ['.'] = ".",
        [','] = ",",
        [':'] = ":",
        [';'] = ";",
        ['!'] = "!",
        ['?'] = "?",
        ['('] = "(",
        [')'] = ")",
        ['['] = "[",
        [']'] = "]",
        ['{'] = "{",
        ['}'] = "}",
        ['<'] = "<",
        ['>'] = ">",
        ['='] = "=",
        ['+'] = "+",
        ['-'] = "-",
        ['*'] = "*",
        ['/'] = "/",
        ['%'] = "%",
        ['&'] = "&",
        ['^'] = "^",
        ['`'] = "`",
        ['|'] = "|",
        ['#'] = "#",
        ['~'] = "~",
        ['\\'] = "\\"
    };

    private static readonly string[] _digraphs =
    {
        "+", "=", "+=",
        "+", "+", "++",
        "-", "=", "-=",
        "-", "-", "--",
        "*", "=", "*=",
        "/", "=", "/=",
        "=", "=", "==",
        "==", "=", "===",
        "<", "=", "<=",
        "<", "<", "<<",
        "<", ">", "<>",
        ">", "=", ">=",
        ">", ">", ">>",
        "!", "=", "!=",
        "&", "=", "&=",
        "&", "&", "&&",
        "|", "=", "|=",
        "|", "|", "||",
        "%", "=", "%=",
        "/", "/", "//"
    };

    private static BarsikToken _ParseNumber
        (
            TextNavigator navigator
        )
    {
        var start = navigator.Position - 1;
        char chr = default;

        while (!navigator.IsEOF)
        {
            chr = navigator.PeekChar();
            if (!chr.IsArabicDigit())
            {
                break;
            }

            navigator.ReadChar();
        }

        var isFloat = false;

        // дробное число
        if (chr == '.')
        {
            isFloat = true;
            navigator.ReadChar();

            while (!navigator.IsEOF)
            {
                chr = navigator.PeekChar();
                if (!chr.IsArabicDigit())
                {
                    break;
                }

                navigator.ReadChar();
            }
        }

        var kind = string.Empty;
        var isU = false;
        var isL = false;
        var isF = false;

        // суффиксы
        if (isFloat)
        {
            chr = navigator.PeekChar();
            if (chr == 'F' || chr == 'f')
            {
                isF = true;
                navigator.ReadChar();
            }
        }
        else
        {
            while (!navigator.IsEOF)
            {
                var canContinue = true;
                switch (chr)
                {
                    case 'u':
                    case 'U':
                        if (isU)
                        {
                            // нельзя указывать больше одного раза
                            throw new FormatException();
                        }

                        isU = true;
                        break;

                    case 'l':
                    case 'L':
                        if (isL)
                        {
                            // нельзя указывать больше одного раза
                            throw new FormatException();
                        }

                        isL = true;
                        break;

                    default:
                        canContinue = false;
                        break;
                }

                if (!canContinue)
                {
                    break;
                }

                navigator.ReadChar();
                chr = navigator.PeekChar();
            }
        }

        kind = isFloat
            ?
                (
                    isF
                    ? BarsikToken.Single
                    : BarsikToken.Double
                )
            :
                (
                    isL
                    ? isU ? BarsikToken.UInt64 : BarsikToken.Int64
                    : isU ? BarsikToken.UInt32 : BarsikToken.Int32
                );

        var result = new BarsikToken
            (
                kind,
                navigator.Substring (start, navigator.Position - start)
            );

        return result;
    }

    private static readonly char[] _firstLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
            + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
            + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
            + "_$"
        )
        .ToCharArray();

    private static readonly char[] _secondLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
            + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            + "0123456789"
            + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
            + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
            + "_$"
        )
        .ToCharArray();

    private static BarsikToken _ParseCharacter
        (
            TextNavigator navigator
        )
    {
        var character = navigator.ReadChar();
        var quotationMark = navigator.ReadChar();
        if (quotationMark != '\'')
        {
            throw new FormatException();
        }

        var value = character.ToString();
        var result = new BarsikToken
            (
                BarsikToken.Char,
                value.AsMemory()
            );

        return result;
    }

    private static BarsikToken _ParseString
        (
            TextNavigator navigator
        )
    {
        var start = navigator.Position;
        char chr = default;

        while (!navigator.IsEOF)
        {
            chr = navigator.ReadChar();
            if (chr == '"')
            {
                break;
            }
        }

        if (chr != '"')
        {
            throw new FormatException();
        }

        var end = navigator.Position - 1;
        var value = navigator.Substring (start, end - start);
        var result = new BarsikToken
            (
                BarsikToken.String,
                value
            );

        return result;
    }

    private static BarsikToken _ParseIdentifier
        (
            TextNavigator navigator
        )
    {
        var start = navigator.Position - 1;

        var chr = navigator.ReadChar();
        if (Array.IndexOf (_firstLetter, chr) < 0)
        {
            throw new FormatException();
        }

        while (!navigator.IsEOF)
        {
            chr = navigator.PeekChar();
            if (Array.IndexOf (_secondLetter, chr) < 0)
            {
                break;
            }

            navigator.ReadChar();
        }

        var end = navigator.Position;
        var value = navigator.Text.AsMemory (start, end - start);
        var result = new BarsikToken (BarsikToken.Identifier, value);

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
            if (chr == '\'')
            {
                token = _ParseCharacter (navigator);
                result.Add (token);
                continue;
            }

            if (chr == '"')
            {
                token = _ParseString (navigator);
                result.Add (token);
                continue;
            }

            if (chr.IsArabicDigit())
            {
                token = _ParseNumber (navigator);
                result.Add (token);
                continue;
            }

            if (_simple.TryGetValue (chr, out var kind))
            {
                token = new BarsikToken
                    (
                        kind,
                        text.AsMemory (navigator.Position - 1, 1)
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
