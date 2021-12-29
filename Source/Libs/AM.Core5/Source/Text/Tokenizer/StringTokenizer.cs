// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* StringTokenizer.cs -- tokenizes text
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Text.Tokenizer;

/// <summary>
/// Tokenizes text.
/// </summary>
public sealed class StringTokenizer
    : IEnumerable<Token>
{
    #region Constants

    /// <summary>
    /// Признак конца текста.
    /// </summary>
    private const char EOF = '\0';

    #endregion

    #region Properties

    /// <summary>
    /// Tokenizer settings.
    /// </summary>
    public TokenizerSettings Settings { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public StringTokenizer
        (
            string text
        )
    {
        _text = text;
        _length = _text.Length;
        _position = 0;
        _line = 1;
        _column = 1;
        Settings = new TokenizerSettings();
    }

    #endregion

    #region Private members

    private readonly string _text;
    private readonly int _length;
    private int _position;
    private int _line;
    private int _column;

    private Token _CreateToken (TokenKind kind) => new (kind, string.Empty, _line, _column);

    private bool _IsWhitespace (char c) => char.IsWhiteSpace (c) && !(c == '\r' || c == '\n');

    private bool _IsWord (char c) => char.IsLetterOrDigit (c) || c == '_';

    private bool _IsSymbol (char c) => Array.IndexOf (Settings.SymbolChars, c) >= 0;

    private Token _SetTokenValue
        (
            Token token,
            int begin
        )
    {
        token.Value = _text.Substring (begin, _position - begin);
        return token;
    }

    private Token _ReadWhitespace()
    {
        var result = _CreateToken (TokenKind.Whitespace);
        var begin = _position;
        ReadChar();
        while (true)
        {
            var c = PeekChar();
            if (!_IsWhitespace (c))
            {
                break;
            }

            ReadChar();
        }

        return _SetTokenValue (result, begin);
    }

    private Token _ReadNumber()
    {
        var result = _CreateToken (TokenKind.Number);
        var begin = _position;
        ReadChar();
        var dotFound = _text[begin] == '.';
        char c;
        while (true)
        {
            c = PeekChar();
            if (c == '.' && !dotFound)
            {
                dotFound = true;
                ReadChar();
                continue;
            }

            if (c == 'E' || c == 'e')
            {
                ReadChar();
                c = PeekChar();
                if (c == '-')
                {
                    ReadChar();
                    c = PeekChar();
                }

                break;
            }

            if (!char.IsDigit (c))
            {
                goto DONE;
            }

            ReadChar();
        }

        if (!char.IsDigit (c))
        {
            Magna.Error
                (
                    "StringTokenizer::_ReadNumber: "
                    + "floating point format error"
                );

            throw new TokenizerException ("Floating point format error");
        }

        while (char.IsDigit (c))
        {
            ReadChar();
            c = PeekChar();
        }

        DONE:
        return _SetTokenValue (result, begin);
    }

    private Token _ReadString()
    {
        var result = _CreateToken (TokenKind.QuotedString);
        var begin = _position;
        var stop = ReadChar();
        while (true)
        {
            var c = PeekChar();
            if (c == EOF)
            {
                break;
            }

            if (c == '\\')
            {
                ReadChar();
                c = ReadChar(); // handle \t, \n etc
                if (c == 'x') // handle \x123
                {
                    c = ReadChar();
                    while (char.IsDigit (c))
                    {
                        ReadChar();
                        c = PeekChar();
                    }
                }
            }
            else if (c == stop)
            {
                ReadChar();
                c = PeekChar();
                if (c == stop)
                {
                    // Удвоение ограничителя означает его экранирование
                    ReadChar();
                }
                else
                {
                    break;
                }
            }

            ReadChar();
        }

        _SetTokenValue (result, begin);

        if (Settings.TrimQuotes
            && !string.IsNullOrEmpty (result.Value))
        {
            result.Value = result.Value.Unquote (stop);

            result.Value = result.Value.Replace
                (
                    stop.ToString() + stop,
                    stop.ToString()
                );
        }

        return result;
    }

    private Token _ReadWord()
    {
        var result = _CreateToken (TokenKind.Word);
        var begin = _position;
        ReadChar();
        while (true)
        {
            var c = PeekChar();
            if (!_IsWord (c))
            {
                break;
            }

            ReadChar();
        }

        return _SetTokenValue (result, begin);
    }

    private Token _ReadSymbol()
    {
        var result = _CreateToken (TokenKind.Symbol);
        var begin = _position;
        ReadChar();

        return _SetTokenValue (result, begin);
    }

    private Token _ReadUnknown()
    {
        var result = _CreateToken (TokenKind.Unknown);
        var begin = _position;
        ReadChar();

        return _SetTokenValue (result, begin);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Get all tokens.
    /// </summary>
    public Token[] GetAllTokens()
    {
        var result = new List<Token>();

        while (true)
        {
            var token = NextToken();
            if (token.IsEOF)
            {
                if (!Settings.IgnoreEOF)
                {
                    result.Add (token);
                }

                break;
            }

            result.Add (token);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Заглядывание на один символ вперед.
    /// </summary>
    /// <returns>EOF, если достигнут конец текста.</returns>
    public char PeekChar()
    {
        if (_position >= _length)
        {
            return EOF;
        }

        return _text[_position];
    }

    /// <summary>
    /// Чтение одного символа с продвижением вперед.
    /// </summary>
    /// <returns>EOF, если достигнут конец текста.</returns>
    public char ReadChar()
    {
        if (_position >= _length)
        {
            return EOF;
        }

        var result = _text[_position];
        _position++;
        _column++;
        return result;
    }

    /// <summary>
    /// Get the next token.
    /// </summary>
    public Token NextToken()
    {
        BEGIN:
        var c = PeekChar();
        Token result;

        if (c == EOF)
        {
            return _CreateToken (TokenKind.EOF);
        }

        if (c == '\r')
        {
            ReadChar();
            goto BEGIN;
        }

        if (c == '\n')
        {
            ReadChar();
            result = _CreateToken (TokenKind.EOL);
            _line++;
            _column = 1;
            if (!Settings.IgnoreNewLine)
            {
                return result;
            }

            goto BEGIN;
        }

        if (char.IsWhiteSpace (c))
        {
            result = _ReadWhitespace();
            if (!Settings.IgnoreWhitespace)
            {
                return result;
            }

            goto BEGIN;
        }

        if (char.IsDigit (c) || c == '.')
        {
            return _ReadNumber();
        }

        if (c == '"' || c == '\'')
        {
            return _ReadString();
        }

        if (_IsWord (c))
        {
            return _ReadWord();
        }

        if (_IsSymbol (c))
        {
            return _ReadSymbol();
        }

        return _ReadUnknown();
    }

    #endregion

    #region IEnumerable<Token> members

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<Token> GetEnumerator()
    {
        while (true)
        {
            var result = NextToken();
            yield return result;
            if (result.IsEOF)
            {
                yield break;
            }
        }
    }

    #endregion
}
