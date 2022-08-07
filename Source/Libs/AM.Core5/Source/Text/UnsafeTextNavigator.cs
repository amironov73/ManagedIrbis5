// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UnsafeTextNavigator.cs -- навигатор по тексту, небезопасный вариант
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.Contracts;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Навигатор по тексту, небезопасный вариант.
/// </summary>
public unsafe ref struct UnsafeTextNavigator
{
    #region Constants

    /// <summary>
    /// Признак конца текста.
    /// </summary>
    public const char EOF = '\0';

    #endregion

    #region Properties

    /// <summary>
    /// Текст закончился?
    /// </summary>
    [Pure]
    public bool IsEOF => Position >= _length;

    /// <summary>
    /// Общая длина текста в символах.
    /// </summary>
    [Pure]
    public int Length => _length;

    /// <summary>
    /// Текущая позиция в тексте.
    /// </summary>
    [Pure]
    public int Position { get; private set; }

    /// <summary>
    /// Хранимый текст в виде спана.
    /// </summary>
    [Pure]
    public ReadOnlySpan<char> AsSpan => new (_text, _length);

    /// <summary>
    /// Текст, хранимый в навигаторе.
    /// </summary>
    [Pure]
    public string Text => new (_text, 0, _length);

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="text">Указатель на первый символ.</param>
    /// <param name="length">Длина текста в символах.</param>
    public UnsafeTextNavigator
        (
            char* text,
            int length
        )
    {
        _text = text;
        _length = length;
        Position = 0;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="text">Строка текста.</param>
    public UnsafeTextNavigator
        (
            string text
        )
    {
        fixed (char* ptr = text)
        {
            _text = ptr;
        }

        _length = text.Length;
        Position = 0;
    }

    #endregion

    #region Private members

    private readonly char* _text;
    private readonly int _length;

    #endregion

    #region Public methods

    /// <summary>
    /// Клонирование навигатора.
    /// </summary>
    [Pure]
    public UnsafeTextNavigator Clone()
    {
        var result = new UnsafeTextNavigator (_text, _length)
        {
            Position = Position
        };

        return result;
    }

    /// <summary>
    /// Выдать остаток текста.
    /// </summary>
    /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
    /// </returns>
    [Pure]
    public ReadOnlySpan<char> GetRemainingText()
    {
        return IsEOF
            ? new ReadOnlySpan<char>()
            : Substring (Position);
    }

    /// <summary>
    /// Управляющий символ?
    /// </summary>
    [Pure]
    public bool IsControl()
    {
        return char.IsControl (PeekChar());
    }

    /// <summary>
    /// Цифра?
    /// </summary>
    [Pure]
    public bool IsDigit()
    {
        return char.IsDigit (PeekChar());
    }

    /// <summary>
    /// Буква?
    /// </summary>
    [Pure]
    public bool IsLetter()
    {
        return char.IsLetter (PeekChar());
    }

    /// <summary>
    /// Буква или цифра?
    /// </summary>
    [Pure]
    public bool IsLetterOrDigit()
    {
        return char.IsLetterOrDigit (PeekChar());
    }

    /// <summary>
    /// Часть числа?
    /// </summary>
    [Pure]
    public bool IsNumber()
    {
        return char.IsNumber (PeekChar());
    }

    /// <summary>
    /// Знак пунктуации?
    /// </summary>
    [Pure]
    public bool IsPunctuation()
    {
        return char.IsPunctuation (PeekChar());
    }

    /// <summary>
    /// Разделитель?
    /// </summary>
    [Pure]
    public bool IsSeparator()
    {
        return char.IsSeparator (PeekChar());
    }

    /// <summary>
    /// Символ?
    /// </summary>
    [Pure]
    public bool IsSymbol()
    {
        return char.IsSymbol (PeekChar());
    }

    /// <summary>
    /// Пробельный символ?
    /// </summary>
    [Pure]
    public bool IsWhiteSpace()
    {
        return char.IsWhiteSpace (PeekChar());
    }

    /// <summary>
    /// Заглядывание вперёд на 1 позицию.
    /// </summary>
    /// <remarks>Это на 1 позицию дальше,
    /// чем <see cref="PeekChar()"/>
    /// </remarks>
    [Pure]
    public char LookAhead()
    {
        var newPosition = Position + 1;
        return newPosition >= _length
            ? EOF
            : _text[newPosition];
    }

    /// <summary>
    /// Заглядывание вперёд.
    /// </summary>
    [Pure]
    public char LookAhead
        (
            int distance
        )
    {
        Sure.NonNegative (distance);

        var newPosition = Position + distance;
        return newPosition >= _length
            ? EOF
            : _text[newPosition];
    }

    /// <summary>
    /// Заглядывание назад.
    /// </summary>
    [Pure]
    public char LookBehind()
    {
        return Position == 0
            ? EOF
            : _text[Position - 1];
    }

    /// <summary>
    /// Заглядывание назад.
    /// </summary>
    [Pure]
    public char LookBehind
        (
            int distance
        )
    {
        Sure.Positive (distance, nameof (distance));

        return Position < distance
            ? EOF
            : _text[Position - distance];
    }

    /// <summary>
    /// Относительное мещение указателя на указанную дистанцию.
    /// </summary>
    public void Move
        (
            int distance
        )
    {
        Position = Math.Max (0, Math.Min (Position + distance, _length));
    }

    /// <summary>
    /// Подглядывание текущего символа.
    /// </summary>
    [Pure]
    public char PeekChar()
    {
        return Position >= _length
            ? EOF
            : _text[Position];
    }

    /// <summary>
    /// Подглядывание строки вплоть до указанной длины.
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> PeekString
        (
            int length
        )
    {
        Sure.Positive (length, nameof (length));

        if (IsEOF)
        {
            return ReadOnlySpan<char>.Empty;
        }

        var start = Position;
        for (var i = 0; i < length; i++)
        {
            var c = ReadChar();
            if (c == EOF)
            {
                break;
            }
        }

        var result = Substring (start, Position - start);
        Position = start;

        return result;
    }

    /// <summary>
    /// Подглядывание вплоть до указанного символа
    /// (включая его).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> PeekTo
        (
            char stopChar
        )
    {
        var postion = Position;
        var result = ReadTo (stopChar);
        Position = postion;

        return result;
    }

    /// <summary>
    /// Подглядывание вплоть до указанных символов
    /// (включая их).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> PeekTo
        (
            char[] stopChars
        )
    {
        var position = Position;
        var result = ReadTo (stopChars);
        Position = position;

        return result;
    }

    /// <summary>
    /// Подглядывание вплоть до указанного символа.
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> PeekUntil
        (
            char stopChar
        )
    {
        var position = Position;
        var result = ReadUntil (stopChar);
        Position = position;

        return result;
    }

    /// <summary>
    /// Подглядывание вплоть до указанных символов.
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> PeekUntil
        (
            params char[] stopChars
        )
    {
        var position = Position;
        var result = ReadUntil (stopChars);
        Position = position;
        return result;
    }

    /// <summary>
    /// Считывание символа.
    /// </summary>
    public char ReadChar()
    {
        if (Position >= _length)
        {
            return EOF;
        }

        return _text[Position++];
    }

    /// <summary>
    /// Считывание экранированной строки вплоть до разделителя
    /// (не включая его).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public string? ReadEscapedUntil
        (
            char escapeChar,
            char stopChar
        )
    {
        if (IsEOF)
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        while (true)
        {
            var c = ReadChar();
            if (c == EOF)
            {
                break;
            }

            if (c == escapeChar)
            {
                c = ReadChar();
                if (c == EOF)
                {
                    Magna.Logger.LogError
                        (
                            nameof (ValueTextNavigator) + "::" + nameof (ReadEscapedUntil)
                            + ": unexpected end of stream"
                        );

                    builder.DismissShared();

                    throw new FormatException();
                }

                builder.Append (c);
            }
            else if (c == stopChar)
            {
                break;
            }
            else
            {
                builder.Append (c);
            }
        }

        return builder.ReturnShared();
    }

    /// <summary>
    /// Считывание начиная с открывающего символа
    /// до закрывающего (включая их).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// Пустая строка, если нет открывающего
    /// или закрывающего символа.
    /// </returns>
    public ReadOnlySpan<char> ReadFrom
        (
            char openChar,
            char closeChar
        )
    {
        if (IsEOF)
        {
            return ReadOnlySpan<char>.Empty;
        }

        var start = Position;
        var c = PeekChar();
        if (c != openChar)
        {
            return ReadOnlySpan<char>.Empty;
        }

        ReadChar();

        while (true)
        {
            c = ReadChar();
            if (c == EOF)
            {
                Position = start;
                return ReadOnlySpan<char>.Empty;
            }

            if (c == closeChar)
            {
                break;
            }
        }

        return Substring
            (
                start,
                Position - start
            );
    }

    /// <summary>
    /// Считывание начиная с открывающего символа
    /// до закрывающего (включая их).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// Пустая строка, если нет открывающего
    /// или закрывающего символа.
    /// </returns>
    public ReadOnlySpan<char> ReadFrom
        (
            ReadOnlySpan<char> openChars,
            ReadOnlySpan<char> closeChars
        )
    {
        if (IsEOF)
        {
            return ReadOnlySpan<char>.Empty;
        }

        var start = Position;
        var c = PeekChar();
        if (!openChars.Contains (c))
        {
            return ReadOnlySpan<char>.Empty;
        }

        ReadChar();

        while (true)
        {
            c = ReadChar();
            if (c == EOF)
            {
                Position = start;
                return ReadOnlySpan<char>.Empty;
            }

            if (closeChars.Contains (c))
            {
                break;
            }
        }

        return Substring
            (
                start,
                Position - start
            );
    }

    /// <summary>
    /// Чтение беззнакового целого.
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// Пустую строку, если не число.</returns>
    public ReadOnlySpan<char> ReadInteger()
    {
        if (!IsDigit())
        {
            return ReadOnlySpan<char>.Empty;
        }

        var startPosition = Position;
        while (IsDigit())
        {
            ReadChar();
        }

        return Substring
            (
                startPosition,
                Position - startPosition
            );
    }

    /// <summary>
    /// Чтение до конца строки.
    /// </summary>
    public ReadOnlySpan<char> ReadLine()
    {
        var startPosition = Position;
        while (!IsEOF)
        {
            var c = PeekChar();
            if (c == '\r' || c == '\n')
            {
                break;
            }

            ReadChar();
        }

        var stopPosition = Position;

        if (!IsEOF)
        {
            var c = PeekChar();

            if (c == '\r')
            {
                ReadChar();
                c = PeekChar();
            }

            if (c == '\n')
            {
                ReadChar();
            }
        }

        return Substring
            (
                startPosition,
                stopPosition - startPosition
            );
    }

    /// <summary>
    /// Чтение строки вплоть до указанной длины.
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> ReadString
        (
            int length
        )
    {
        Sure.Positive (length, nameof (length));

        var startPosition = Position;
        for (var i = 0; i < length; i++)
        {
            var c = ReadChar();
            if (c == EOF)
            {
                break;
            }
        }

        return Substring
            (
                startPosition,
                Position - startPosition
            );
    }

    /// <summary>
    /// Считывание вплоть до указанного символа
    /// (включая его).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> ReadTo
        (
            char stopChar
        )
    {
        var start = Position;
        while (true)
        {
            var c = ReadChar();
            if (c == EOF || c == stopChar)
            {
                break;
            }
        }

        return Substring
            (
                start,
                length: Position - start
            );
    }

    /// <summary>
    /// Считывание вплоть до указанного разделителя
    /// (разделитель не помещается в возвращаемое значение,
    /// однако, считывается).
    /// </summary>
    public ReadOnlySpan<char> ReadToString
        (
            ReadOnlySpan<char> stopString
        )
    {
        // Sure.NotNullNorEmpty(stopString, nameof(stopString));

        var savePosition = Position;
        var length = 0;
        while (true)
        {
            AGAIN:
            var c = ReadChar();
            if (c == EOF)
            {
                Position = savePosition;
                return ReadOnlySpan<char>.Empty;
            }

            length++;
            if (length >= stopString.Length)
            {
                var start = Position - stopString.Length;
                for (var i = 0; i < stopString.Length; i++)
                {
                    if (_text[start + i] != stopString[i])
                    {
                        goto AGAIN;
                    }
                }

                break;
            }
        }

        return Substring
            (
                savePosition,
                Position - savePosition - stopString.Length
            );
    }

    /// <summary>
    /// Считывание вплоть до указанного символа
    /// (включая один из них).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> ReadTo
        (
            params char[] stopChars
        )
    {
        var start = Position;
        while (true)
        {
            var c = ReadChar();
            if (c == EOF
                || Array.IndexOf (stopChars, c) >= 0)
            {
                break;
            }
        }

        var result = Substring
            (
                start,
                length: Position - start
            );
        return result;
    }

    /// <summary>
    /// Считывание вплоть до указанного символа
    /// (не включая его).
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> ReadUntil
        (
            char stopChar
        )
    {
        var startPosition = Position;
        while (true)
        {
            var c = PeekChar();
            if (c == EOF || c == stopChar)
            {
                break;
            }

            ReadChar();
        }

        return Substring
            (
                startPosition,
                Position - startPosition
            );
    }

    /// <summary>
    /// Считывание вплоть до указанного разделителя
    /// (разделитель не помещается в возвращаемое значение
    /// и не считывается).
    /// </summary>
    public ReadOnlySpan<char> ReadUntilString
        (
            ReadOnlySpan<char> stopString
        )
    {
        // Sure.NotNullNorEmpty(stopString, nameof(stopString));

        var position = Position;
        var length = 0;
        while (true)
        {
            AGAIN:
            var c = ReadChar();
            if (c == EOF)
            {
                Position = position;
                return ReadOnlySpan<char>.Empty;
            }

            length++;
            if (length >= stopString.Length)
            {
                var start = Position - stopString.Length;
                for (var i = 0; i < stopString.Length; i++)
                {
                    if (_text[start + i] != stopString[i])
                    {
                        goto AGAIN;
                    }
                }

                break;
            }
        }

        var result = Substring
            (
                position,
                Position - position - stopString.Length
            );
        Position -= stopString.Length;
        return result;
    }

    /// <summary>
    /// Считывание вплоть до указанных символов
    /// (не включая их).
    /// </summary>
    /// <remarks><c>null</c>, если достигнут конец текста.
    /// </remarks>
    public ReadOnlySpan<char> ReadUntil
        (
            params char[] stopChars
        )
    {
        var savePosition = Position;
        while (true)
        {
            var c = PeekChar();
            if (c == EOF
                || Array.IndexOf (stopChars, c) >= 0)
            {
                break;
            }

            ReadChar();
        }

        return Substring
            (
                savePosition,
                Position - savePosition
            );
    }

    /// <summary>
    /// Считывание вплоть до указанных символов
    /// (не включая их).
    /// </summary>
    /// <remarks><c>null</c>, если достигнут конец текста.
    /// </remarks>
    public ReadOnlySpan<char> ReadUntil
        (
            ReadOnlySpan<char> openChars,
            ReadOnlySpan<char> closeChars,
            ReadOnlySpan<char> stopChars
        )
    {
        var start = Position;
        var level = 0;
        while (true)
        {
            var c = PeekChar();
            if (c == EOF)
            {
                Position = start;
                return ReadOnlySpan<char>.Empty;
            }

            if (openChars.Contains (c))
            {
                level++;
            }
            else if (closeChars.Contains (c))
            {
                if (level == 0
                    && stopChars.Contains (c))
                {
                    break;
                }

                level--;
            }
            else if (stopChars.Contains (c))
            {
                if (level == 0)
                {
                    break;
                }
            }

            ReadChar();
        }

        return Substring
            (
                start,
                Position - start
            );
    }

    /// <summary>
    /// Считывание, пока встречается указанный символ.
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> ReadWhile
        (
            char goodChar
        )
    {
        var startPosition = Position;
        while (true)
        {
            var c = PeekChar();
            if (c == EOF || c != goodChar)
            {
                break;
            }

            ReadChar();
        }

        return Substring
            (
                startPosition,
                Position - startPosition
            );
    }

    /// <summary>
    /// Считывание, пока встречается указанные символы.
    /// </summary>
    /// <returns><c>null</c>, если достигнут конец текста.
    /// </returns>
    public ReadOnlySpan<char> ReadWhile
        (
            params char[] goodChars
        )
    {
        var startPosition = Position;
        while (true)
        {
            var c = PeekChar();
            if (c == EOF
                || Array.IndexOf (goodChars, c) < 0)
            {
                break;
            }

            ReadChar();
        }

        return Substring
            (
                startPosition,
                Position - startPosition
            );
    }

    /// <summary>
    /// Считываем слово под курсором.
    /// </summary>
    public ReadOnlySpan<char> ReadWord()
    {
        var startPosition = Position;
        while (true)
        {
            var c = PeekChar();
            if (c == EOF
                || !char.IsLetterOrDigit (c))
            {
                break;
            }

            ReadChar();
        }

        return Substring
            (
                startPosition,
                Position - startPosition
            );
    }

    /// <summary>
    /// Считываем слово под курсором.
    /// </summary>
    public ReadOnlySpan<char> ReadWord
        (
            params char[] additionalWordCharacters
        )
    {
        var savePosition = Position;
        while (true)
        {
            var c = PeekChar();
            if (c == EOF
                || !char.IsLetterOrDigit (c)
                && Array.IndexOf (additionalWordCharacters, c) < 0)
            {
                break;
            }

            ReadChar();
        }

        return Substring
            (
                savePosition,
                Position - savePosition
            );
    }

    /// <summary>
    /// Получаем недавно считанный текст указанной длины.
    /// </summary>
    [Pure]
    public ReadOnlySpan<char> RecentText
        (
            int length
        )
    {
        var start = Position - length;
        if (start < 0)
        {
            length += start;
            start = 0;
        }

        if (start >= _length)
        {
            length = 0;
            start = _length - 1;
        }

        if (length < 0)
        {
            length = 0;
        }

        return Substring (start, length);
    }

    /// <summary>
    /// Пропускает один символ, если он совпадает с указанным.
    /// </summary>
    /// <returns><c>true</c>, если символ был съеден успешно
    /// </returns>
    public bool SkipChar
        (
            char c
        )
    {
        if (PeekChar() == c)
        {
            ReadChar();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Пропускает указанное число символов.
    /// </summary>
    /// <returns><c>true</c>, если ещё остались непрочитанные символы.
    /// </returns>
    public bool SkipChar
        (
            int n
        )
    {
        for (var i = 0; i < n; i++)
        {
            ReadChar();
        }

        return !IsEOF;
    }

    /// <summary>
    /// Пропускает один символ, если он совпадает с любым
    /// из указанных.
    /// </summary>
    /// <returns><c>true</c>, если символ был съеден успешно
    /// </returns>
    public bool SkipChar
        (
            params char[] allowed
        )
    {
        if (Array.IndexOf (allowed, PeekChar()) >= 0)
        {
            ReadChar();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Пропускаем управляющие символы.
    /// </summary>
    public bool SkipControl()
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            if (IsControl())
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропускаем пунктуацию.
    /// </summary>
    public bool SkipPunctuation()
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            if (IsPunctuation())
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Skip non-word characters.
    /// </summary>
    public bool SkipNonWord()
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            var c = PeekChar();
            if (!char.IsLetterOrDigit (c))
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Skip non-word characters.
    /// </summary>
    public bool SkipNonWord
        (
            params char[] additionalWordCharacters
        )
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            var c = PeekChar();
            if (!char.IsLetterOrDigit (c)
                && Array.LastIndexOf (additionalWordCharacters, c) < 0)
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропускаем произвольное количество символов
    /// из указанного диапазона.
    /// </summary>
    public bool SkipRange
        (
            char fromChar,
            char toChar
        )
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            var c = PeekChar();
            if (c >= fromChar && c <= toChar)
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропустить указанный символ.
    /// </summary>
    public bool SkipWhile
        (
            char skipChar
        )
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            var c = PeekChar();
            if (c == skipChar)
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропустить указанные символы.
    /// </summary>
    public bool SkipWhile
        (
            params char[] skipChars
        )
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            var c = PeekChar();
            if (Array.IndexOf (skipChars, c) >= 0)
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропустить, пока не встретится указанный символ.
    /// Сам символ не считывается.
    /// </summary>
    public bool SkipTo
        (
            char stopChar
        )
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            var c = PeekChar();
            if (c == stopChar)
            {
                return true;
            }

            ReadChar();
        }
    }

    /// <summary>
    /// Пропустить, пока не встретятся указанные символы.
    /// </summary>
    public bool SkipWhileNot
        (
            params char[] goodChars
        )
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            var c = PeekChar();
            if (Array.IndexOf (goodChars, c) < 0)
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропускаем пробельные символы.
    /// </summary>
    public bool SkipWhitespace()
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            if (IsWhiteSpace())
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропускаем пробельные символы и пунктуацию.
    /// </summary>
    public bool SkipWhitespaceAndPunctuation()
    {
        while (true)
        {
            if (IsEOF)
            {
                return false;
            }

            if (IsWhiteSpace() || IsPunctuation())
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Get substring.
    /// </summary>
    [Pure]
    public ReadOnlySpan<char> Substring
        (
            int offset
        )
    {
        return AsSpan.Slice (offset);
    }

    /// <summary>
    /// Get substring.
    /// </summary>
    [Pure]
    public ReadOnlySpan<char> Substring
        (
            int offset,
            int length
        )
    {
        return offset >= _length || length <= 0
            ? ReadOnlySpan<char>.Empty
            : AsSpan.Slice (offset, length);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    [Pure]
    public override string ToString()
    {
        return $"Position={Position}";
    }

    #endregion
}
