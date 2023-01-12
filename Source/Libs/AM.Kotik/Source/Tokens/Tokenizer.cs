// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Tokenizer.cs -- генерализованный токенайзер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Генерализованный токенайзер.
/// </summary>
public class Tokenizer
{
    #region Private members

    // пространство имен нужно, чтобы не делать using
    // а если сделать using, то пересекутся имена классов
    // вроде Token
    private Text.TextNavigator _navigator = null!;

    private static readonly char[] _firstIdentifierLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
            + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
            + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
            + "_$"
        )
        .ToCharArray();

    private static readonly char[] _nextIdentifierLetter =
        (
            "abcdefghijklmnopqrstuvwxyz"
            + "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
            + "0123456789"
            + "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"
            + "АБСГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ"
            + "_$"
        )
        .ToCharArray();

    private static readonly string[] _knownTerms =
    {
        "!", ";", ":", ",", "(", ")", "+", "-", "*", "/", "[", "]",
        "{", "}", "|", "%", "~", "=", "++", "--", "+=", "-=", "*=",
        "/="
    };

    private bool IsEOF => _navigator.IsEOF;

    private char PeekChar() => _navigator.PeekChar();

    private char ReadChar() => _navigator.ReadChar();

    private bool SkipWhitespace() => _navigator.SkipWhitespace();

    #endregion

    #region Protected members

    /// <summary>
    /// Пропускаем комментарии.
    /// </summary>
    /// <returns><c>true</c>, если были встречены комментарии.
    /// </returns>
    protected virtual bool SkipComments()
    {
        if (PeekChar() == '/')
        {
            var nextChar = _navigator.LookAhead();


            // комментарий до конца строки
            if (nextChar == '/')
            {
                // съедаем всю текущую строку до конца
                _navigator.ReadLine();
                return true;
            }

            // многострочный комментарий
            if (nextChar == '*')
            {
                // проматываем всё до конца
                _navigator.ReadTo ("*/");
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Разбор единичного символа в кавычках.
    /// </summary>
    /// <returns><c>null</c>, если в текущей позиции не символ.</returns>
    protected virtual Token? ParseCharacter()
    {
        if (PeekChar() != '\'')
        {
            return null;
        }

        ReadChar(); // съедаем открывающий апостроф
        var result = ReadChar();
        if (ReadChar() != '\'')
        {
            throw new SyntaxException (_navigator);
        }

        return new Token (TokenKind.Char, result.ToString());
    }

    /// <summary>
    /// Разбор обычной строки (пока без экранирования).
    /// </summary>
    protected virtual Token? ParseString()
    {
        if (PeekChar() != '"')
        {
            return null;
        }

        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = new StringBuilder();
        while (!IsEOF)
        {
            chr = ReadChar();
            if (chr == '"')
            {
                break;
            }

            builder.Append (chr);
        }

        if (chr != '"')
        {
            throw new SyntaxException (_navigator);
        }

        return new Token (TokenKind.String, builder.ToString());
    }

    /// <summary>
    /// Разбор числа.
    /// </summary>
    protected virtual Token? ParseNumber()
    {
        char chr = PeekChar();
        if (!chr.IsArabicDigit())
        {
            return null;
        }

        var builder = new StringBuilder();
        while (!IsEOF)
        {
            chr = PeekChar();
            if (!chr.IsArabicDigit())
            {
                break;
            }

            builder.Append (ReadChar());
        }

        var isFloat = false;

        // дробное число
        if (chr == '.')
        {
            isFloat = true;
            builder.Append (ReadChar());

            while (!IsEOF)
            {
                chr = PeekChar();
                if (!chr.IsArabicDigit())
                {
                    break;
                }

                builder.Append (ReadChar());
            }
        }

        var isU = false;
        var isL = false;
        var isF = false;
        var isM = false;

        // суффиксы
        if (isFloat)
        {
            chr = PeekChar();
            if (chr is 'F' or 'f')
            {
                isF = true;
                builder.Append (ReadChar());
            }

            if (chr is 'M' or 'm')
            {
                isM = true;
                builder.Append (ReadChar());
            }
        }
        else
        {
            while (!IsEOF)
            {
                var canContinue = true;
                switch (chr)
                {
                    case 'u':
                    case 'U':
                        if (isU || isF || isFloat || isM)
                        {
                            // нельзя указывать больше одного раза
                            throw new FormatException();
                        }

                        isU = true;
                        break;

                    case 'l':
                    case 'L':
                        if (isL || isF || isFloat || isM)
                        {
                            // нельзя указывать больше одного раза
                            throw new FormatException();
                        }

                        isL = true;
                        break;

                    case 'f':
                    case 'F':
                        if (isU || isL || isM)
                        {
                            throw new FormatException();
                        }

                        isFloat = true;
                        isF = true;
                        break;

                    case 'm':
                    case 'M':
                        if (isU || isL || isF)
                        {
                            throw new FormatException();
                        }

                        isM = true;
                        break;

                    default:
                        canContinue = false;
                        break;
                }

                if (!canContinue)
                {
                    break;
                }

                builder.Append (ReadChar());
                chr = PeekChar();
            }
        }

        var kind = isFloat
            ? (
                isM
                    ? TokenKind.Decimal
                    : isF
                        ? TokenKind.Single
                        : TokenKind.Double
            )
            : (
                isM
                    ? TokenKind.Decimal
                    : isL
                        ? isU ? TokenKind.UInt64 : TokenKind.Int64
                        : isU
                            ? TokenKind.UInt32
                            : TokenKind.Int32
            );

        var result = new Token
            (
                kind,
                builder.ToString()
            );

        return result;
    }

    /// <summary>
    /// Разбор терма.
    /// </summary>
    protected virtual Token? ParseTerm()
    {
        string? previousGood = null;
        var line = _navigator.Line;
        var column = _navigator.Column;
        var builder = new StringBuilder();
        while (true)
        {
            var chr = _navigator.LookAhead (builder.Length);
            if (chr == Text.TextNavigator.EOF)
            {
                return MakeToken (previousGood);
            }

            builder.Append (chr);
            var text = builder.ToString();
            var count = 0;
            foreach (var known in _knownTerms)
            {
                if (known.StartsWith (text))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                return MakeToken (previousGood);
            }

            if (count == 1)
            {
                foreach (var known in _knownTerms)
                {
                    if (known == text)
                    {
                        return MakeToken (known);
                    }
                }
            }

            previousGood = null;
            foreach (var known in _knownTerms)
            {
                if (known == text)
                {
                    previousGood = known;
                    break;
                }
            }
        }

        Token? MakeToken (string? tokenValue)
        {
            if (tokenValue is null)
            {
                return null;
            }

            for (var i = 0; i < builder.Length; i++)
            {
                ReadChar();
            }

            return new Token
                (
                    TokenKind.Term,
                    tokenValue,
                    line,
                    column
                );
        }
    }

    /// <summary>
    /// Разбор идентификатора.
    /// </summary>
    /// <returns><c>null</c>, если в текущей позиции не идентификатор.</returns>
    protected virtual Token? ParseIdentifier()
    {
        var firstChar = PeekChar();
        if (Array.IndexOf (_firstIdentifierLetter, firstChar) < 0)
        {
            return null;
        }

        var builder = new StringBuilder();
        var line = _navigator.Line;
        var column = _navigator.Column;
        builder.Append (ReadChar());

        while (!IsEOF)
        {
            if (Array.IndexOf (_nextIdentifierLetter, PeekChar()) < 0)
            {
                break;
            }

            builder.Append (ReadChar());
        }

        var value = builder.ToString();

        return new Token
            (
                KotikUtility.IsReservedWord (value)
                    ? TokenKind.ReservedWord
                    : TokenKind.Identifier,
                value,
                line,
                column
            );
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор текста на токены.
    /// </summary>
    public List<Token> Tokenize
        (
            string text
        )
    {
        Sure.NotNull (text);

        var result = new List<Token>();
        _navigator = new Text.TextNavigator (text);

        while (!IsEOF)
        {
            if (!SkipWhitespace())
            {
                break;
            }

            if (SkipComments())
            {
                continue;
            }

            var token = ParseCharacter()
                        ?? ParseString()
                        ?? ParseNumber()
                        ?? ParseTerm()
                        ?? ParseIdentifier()
                        ?? throw new SyntaxException (_navigator);

            result.Add (token);
        }

        return result;
    }

    #endregion
}
