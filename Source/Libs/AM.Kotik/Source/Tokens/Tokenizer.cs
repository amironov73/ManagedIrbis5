// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Tokenizer.cs -- генерализованный токенайзер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Генерализованный токенайзер.
/// </summary>
public sealed class Tokenizer
{
    #region Properties

    /// <summary>
    /// Настройки токенизации.
    /// </summary>
    public TokenizerSettings Settings { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Tokenizer()
    {
        Settings = TokenizerSettings.CreateDefault();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Tokenizer
        (
            TokenizerSettings settings
        )
    {
        Sure.NotNull (settings);

        Settings = settings;
    }

    #endregion

    #region Private members

    private TextNavigator _navigator = null!;

    private bool IsEof => _navigator.IsEOF;

    private char PeekChar() => _navigator.PeekChar();

    private char PeekChar (int delta) => _navigator.LookAhead (delta);

    private char ReadChar() => _navigator.ReadChar();

    private bool SkipWhitespace() => _navigator.SkipWhitespace();

    /// <summary>
    /// Проверка, не является ли указанный текст зарезервированным словом.
    /// </summary>
    private bool IsReservedWord
        (
            string text
        )
    {
        Sure.NotNull (text);

        foreach (var word in Settings.ReservedWords)
        {
            if (string.CompareOrdinal (word, text) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Пропускаем комментарии.
    /// </summary>
    /// <returns><c>true</c>, если были встречены комментарии.
    /// </returns>
    private bool SkipComments()
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
                var position = _navigator.Position;
                _navigator.ReadTo ("*/");
                if (_navigator.Position == position)
                {
                    throw new SyntaxException (_navigator);
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Разбор единичного символа в кавычках.
    /// </summary>
    /// <returns><c>null</c>, если в текущей позиции не символ.</returns>
    private Token? ParseCharacter()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        if (PeekChar() != '\'')
        {
            return null;
        }

        ReadChar(); // съедаем открывающий апостроф
        var result = ReadChar();
        var chr = ReadChar();
        if (chr == '\\')
        {
            // TODO реализовать
        }

        if (chr != '\'')
        {
            throw new SyntaxException (_navigator);
        }

        return new Token (TokenKind.Char, result.ToString(), line, column);
    }

    /// <summary>
    /// Разбор сырой строки формата """Строка""".
    /// </summary>
    /// <returns><c>null</c>, если в текущей позиции не сырая строка.</returns>
    private Token? ParseRawString()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;

        if (PeekChar() != '"' || PeekChar (1) != '"' || PeekChar (2) != '"')
        {
            return null;
        }

        ReadChar(); // съедаем открывающие кавычки
        ReadChar();
        ReadChar();

        var success = false;
        var builder = new StringBuilder();
        while (!IsEof)
        {
            var chr = ReadChar();
            if (chr == '"' && PeekChar() == '"' && PeekChar (1) == '"')
            {
                ReadChar();
                ReadChar();
                success = true;
                break;
            }

            builder.Append (chr);
        }

        if (!success)
        {
            throw new SyntaxException (_navigator);
        }

        return new Token (TokenKind.String, builder.ToString(), line, column);
    }

    /// <summary>
    /// Разбор обычной строки (пока без экранирования).
    /// </summary>
    private Token? ParseString()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;

        if (PeekChar() != '"')
        {
            return null;
        }

        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = new StringBuilder();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '\\')
            {
                builder.Append (chr);
                builder.Append (ReadChar());
                continue;
            }

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

        var text = builder.ToString();
        text = UnescapeText (text);

        return new Token (TokenKind.String, text, line, column);
    }

    /// <summary>
    /// Разбор строки формата.
    /// </summary>
    private Token? ParseFormat()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;

        if (PeekChar() != '$' || PeekChar (1) != '"')
        {
            return null;
        }

        ReadChar(); // съедаем доллар
        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = new StringBuilder();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '\\')
            {
                builder.Append (chr);
                builder.Append (ReadChar());
                continue;
            }

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

        var text = builder.ToString();
        text = UnescapeText (text);

        return new Token (TokenKind.Format, text, line, column);
    }

    /// <summary>
    /// Разбор числа.
    /// </summary>
    private Token? ParseNumber()
    {
        StringBuilder builder;
        var line = _navigator.Line;
        var column = _navigator.Column;
        var isFloat = false;
        char chr = PeekChar();
        if (!chr.IsArabicDigit())
        {
            if (chr is '.' && _navigator.LookAhead (1).IsArabicDigit())
            {
                builder = new StringBuilder();
                goto IS_FLOAT;
            }

            return null;
        }

        builder = new StringBuilder();

        // префикс '0x'
        if (chr == '0' && _navigator.LookAhead (1) is 'x')
        {
            ReadChar();
            ReadChar();

            var hex = TokenKind.Hex32;
            while (!IsEof)
            {
                chr = PeekChar();

                if (chr is '_')
                {
                    ReadChar();
                    continue;
                }

                if (!"0123456789ABCDEFabcdef".Contains (chr))
                {
                    break;
                }

                builder.Append (ReadChar());
            }

            if (chr is 'L' or 'l')
            {
                ReadChar();
                hex = TokenKind.Hex64;
            }

            return new Token (hex, builder.ToString(), line, column);
        }

        while (!IsEof)
        {
            chr = PeekChar();

            if (chr is '_')
            {
                ReadChar();
                continue;
            }

            if (!chr.IsArabicDigit())
            {
                break;
            }

            builder.Append (ReadChar());
        }

        // дробное число
        IS_FLOAT: if (chr == '.')
        {
            isFloat = true;
            builder.Append (ReadChar());

            while (!IsEof)
            {
                chr = PeekChar();

                if (chr is '_')
                {
                    ReadChar();
                    continue;
                }

                if (!chr.IsArabicDigit())
                {
                    break;
                }

                builder.Append (ReadChar());
            }
        }

        // экспонента
        if (chr is 'e' or 'E')
        {
            isFloat = true;
            builder.Append (ReadChar());
            chr = PeekChar();

            if (chr is '+' or '-')
            {
                builder.Append (ReadChar());
                chr = PeekChar();
                if (!chr.IsArabicDigit())
                {
                    throw new SyntaxException (_navigator);
                }
            }

            while (!IsEof)
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
                ReadChar();
            }

            if (chr is 'M' or 'm')
            {
                isM = true;
                ReadChar();
            }
        }
        else
        {
            while (!IsEof)
            {
                var canContinue = true;
                switch (chr)
                {
                    case 'u':
                    case 'U':
                        if (isU || isF || isFloat || isM)
                        {
                            // нельзя указывать больше одного раза
                            throw new SyntaxException (_navigator);
                        }

                        isU = true;
                        break;

                    case 'l':
                    case 'L':
                        if (isL || isF || isFloat || isM)
                        {
                            // нельзя указывать больше одного раза
                            throw new SyntaxException (_navigator);
                        }

                        isL = true;
                        break;

                    case 'f':
                    case 'F':
                        if (isU || isF || isL || isM)
                        {
                            throw new SyntaxException (_navigator);
                        }

                        isFloat = true;
                        isF = true;
                        break;

                    case 'm':
                    case 'M':
                        if (isU || isL || isF || isM)
                        {
                            throw new SyntaxException (_navigator);
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

                ReadChar();
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
                builder.ToString(),
                line,
                column
            );

        return result;
    }

    /// <summary>
    /// Разбор терма.
    /// </summary>
    private Token? ParseTerm()
    {
        string? previousGood = null;
        var line = _navigator.Line;
        var column = _navigator.Column;
        var builder = new StringBuilder();
        var knownTerms = Settings.KnownTerms;
        while (true)
        {
            var chr = _navigator.LookAhead (builder.Length);
            if (chr == TextNavigator.EOF)
            {
                return MakeToken (previousGood);
            }

            builder.Append (chr);
            var text = builder.ToString();
            var count = 0;

            foreach (var known in knownTerms)
            {
                if (known.StartsWith (text))
                {
                    count++;
                }
            }

            if (count == 0)
            {
                if (builder.Length != 0)
                {
                    builder.Length--;
                }

                return MakeToken (previousGood);
            }

            if (count == 1)
            {
                foreach (var known in knownTerms)
                {
                    if (known == text)
                    {
                        return MakeToken (known);
                    }
                }
            }

            previousGood = null;
            foreach (var known in knownTerms)
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
    private Token? ParseIdentifier()
    {
        var firstChar = PeekChar();
        var firstIdentifierLetter = Settings.FirstIdentifierLetter;
        var nextIdentifierLetter = Settings.NextIdentifierLetter;
        if (Array.IndexOf (firstIdentifierLetter, firstChar) < 0)
        {
            return null;
        }

        var builder = new StringBuilder();
        var line = _navigator.Line;
        var column = _navigator.Column;
        builder.Append (ReadChar());

        while (!IsEof)
        {
            if (Array.IndexOf (nextIdentifierLetter, PeekChar()) < 0)
            {
                break;
            }

            builder.Append (ReadChar());
        }

        var value = builder.ToString();

        return new Token
            (
                IsReservedWord (value)
                    ? TokenKind.ReservedWord
                    : TokenKind.Identifier,
                value,
                line,
                column
            );
    }

    private List<Token> Refine
        (
            IList<Token> source
        )
    {
        var result = new List<Token> (source.Count);
        for (var index = 0; index < source.Count; index++)
        {
            var token = source[index];
            if (token is { Kind: TokenKind.Term, Value: "-" })
            {
                var next = source!.SafeAt (index + 1);
                if (next is not null && next.IsSignedNumber())
                {
                    var prev = source!.SafeAt (index - 1)?.Kind;
                    if (prev is null or TokenKind.Term)
                    {
                        result.Add (next.WithNewValue ("-" + next.Value));
                        index++;
                    }
                    else
                    {
                        result.Add (token);
                    }
                }
                else
                {
                    result.Add (token);
                }
            }
            else
            {
                result.Add (token);
            }
        }

        return result;
    }

    /// <summary>
    /// Преобразует строку, содержащую escape-последовательности,
    /// к нормальному виду.
    /// </summary>
    public static string UnescapeText
        (
            string text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var navigator = new TextNavigator (text);
        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (text.Length);

        while (!navigator.IsEOF)
        {
            var c = navigator.ReadChar();
            if (c == '\\')
            {
                var c2 = navigator.ReadChar();
                c2 = c2 switch
                {
                    'a' => '\a',
                    'b' => '\b',
                    'f' => '\f',
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    'u' => ParseUnicode(),
                    'v' => '\v',
                    '\'' => '\'',
                    '"' => '"',
                    '\\' => '\\',
                    '0' => '\0',
                    _ => '?'
                };
                builder.Append (c2);
            }
            else
            {
                builder.Append (c);
            }
        }

        return builder.ReturnShared();

        char ParseUnicode()
        {
            return (char) int.Parse
                (
                    navigator.ReadString (4).Span,
                    NumberStyles.HexNumber
                );
        }
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
        _navigator = new TextNavigator (text);

        while (!IsEof)
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
                ?? ParseRawString()
                ?? ParseString()
                ?? ParseFormat()
                ?? ParseNumber()
                ?? ParseTerm()
                ?? ParseIdentifier()
                ?? throw new SyntaxException (_navigator);

            result.Add (token);
        }

        result = Refine (result);

        return result;
    }

    #endregion
}
