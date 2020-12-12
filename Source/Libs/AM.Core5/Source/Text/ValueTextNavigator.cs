// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ValueTextNavigator.cs -- навигатор по тексту, оформленный как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Навигатор по тексту, оформленный как структура.
    /// </summary>
    public ref struct ValueTextNavigator
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
        public bool IsEOF => _position >= _text.Length;

        /// <summary>
        /// Общая длина текста в символах.
        /// </summary>
        public int Length => _text.Length;

        /// <summary>
        /// Текущая позиция в тексте.
        /// </summary>
        public int Position => _position;

        /// <summary>
        /// Текст, хранимый в навигаторе.
        /// </summary>
        public string Text => _text.ToString();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="text">Текст</param>
        public ValueTextNavigator
            (
                ReadOnlySpan<char> text
            )
        {
            _text = text;
            _position = 0;
        } // constructor

        #endregion

        #region Private members

        private readonly ReadOnlySpan<char> _text;
        private int _position;

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the navigator.
        /// </summary>
        [Pure]
        public ValueTextNavigator Clone()
        {
            var result = new ValueTextNavigator(_text)
            {
                _position = _position
            };

            return result;
        } // method Clone

        /// <summary>
        /// Навигатор по текстовому файлу.
        /// </summary>
        public static ValueTextNavigator FromFile
            (
                string fileName,
                Encoding? encoding = default
            )
        {
            Sure.FileExists(fileName, nameof(fileName));

            encoding ??= Encoding.UTF8;
            var text = File.ReadAllText(fileName, encoding);
            var result = new ValueTextNavigator(text);

            return result;
        } // method FromFile

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
                : _text.Slice(_position);
        } // method GetRemainingText

        /// <summary>
        /// Управляющий символ?
        /// </summary>
        [Pure]
        public bool IsControl() => char.IsControl(PeekChar());

        /// <summary>
        /// Цифра?
        /// </summary>
        [Pure]
        public bool IsDigit() => char.IsDigit(PeekChar());

        /// <summary>
        /// Буква?
        /// </summary>
        [Pure]
        public bool IsLetter() => char.IsLetter(PeekChar());

        /// <summary>
        /// Буква или цифра?
        /// </summary>
        [Pure]
        public bool IsLetterOrDigit() => char.IsLetterOrDigit(PeekChar());

        /// <summary>
        /// Часть числа?
        /// </summary>
        [Pure]
        public bool IsNumber() => char.IsNumber(PeekChar());

        /// <summary>
        /// Знак пунктуации?
        /// </summary>
        [Pure]
        public bool IsPunctuation() => char.IsPunctuation(PeekChar());

        /// <summary>
        /// Разделитель?
        /// </summary>
        [Pure]
        public bool IsSeparator() => char.IsSeparator(PeekChar());

        /// <summary>
        /// Символ?
        /// </summary>
        [Pure]
        public bool IsSymbol() => char.IsSymbol(PeekChar());

        /// <summary>
        /// Пробельный символ?
        /// </summary>
        [Pure]
        public bool IsWhiteSpace() => char.IsWhiteSpace(PeekChar());

        /// <summary>
        /// Заглядывание вперёд на 1 позицию.
        /// </summary>
        /// <remarks>Это на 1 позицию дальше,
        /// чем <see cref="PeekChar()"/>
        /// </remarks>
        [Pure]
        public char LookAhead()
        {
            var newPosition = _position + 1;
            return newPosition >= _text.Length
                ? EOF
                : _text[newPosition];
        } // method LookAhead

        /// <summary>
        /// Заглядывание вперёд.
        /// </summary>
        [Pure]
        public char LookAhead
            (
                int distance
            )
        {
            Sure.NonNegative(distance, nameof(distance));

            var newPosition = _position + distance;
            return newPosition >= _text.Length
                ? EOF
                : _text[newPosition];
        } // method LookAhead

        /// <summary>
        /// Заглядывание назад.
        /// </summary>
        [Pure]
        public char LookBehind()
        {
            return _position == 0
                ? EOF
                : _text[_position - 1];
        } // method LookBehind

        /// <summary>
        /// Заглядывание назад.
        /// </summary>
        [Pure]
        public char LookBehind
            (
                int distance
            )
        {
            Sure.Positive(distance, nameof(distance));

            return _position < distance
                ? EOF
                : _text[_position - distance];
        } // method LookBehind

        /// <summary>
        /// Относительное мещение указателя на указанную дистанцию.
        /// </summary>
        public void Move
            (
                int distance
            )
        {
            _position = Math.Max (0, Math.Min(_position + distance, _text.Length));
        } // method Move

        /// <summary>
        /// Подглядывание текущего символа.
        /// </summary>
        [Pure]
        public char PeekChar() => _position >= _text.Length
                ? EOF
                : _text[_position];

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
            Sure.Positive(length, nameof(length));

            if (IsEOF)
            {
                return ReadOnlySpan<char>.Empty;
            }

            var start = _position;
            for (var i = 0; i < length; i++)
            {
                var c = ReadChar();
                if (c == EOF)
                {
                    break;
                }
            }

            var result = _text.Slice(start, _position - start);
            _position = start;

            return result;
        } // method PeekString

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
            var postion = _position;
            var result = ReadTo(stopChar);
            _position = postion;

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
            var position = _position;
            var result = ReadTo(stopChars);
            _position = position;

            return result;
        } // method PeekTo

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
            var position = _position;
            var result = ReadUntil(stopChar);
            _position = position;

            return result;
        } // method PeekUntil

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
            var position = _position;
            var result = ReadUntil(stopChars);
            _position = position;
            return result;
        } // metdho PeekUntil

        /// <summary>
        /// Считывание символа.
        /// </summary>
        public char ReadChar()
        {
            if (_position >= _text.Length)
            {
                return EOF;
            }

            return _text[_position++];
        } // method ReadChar

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

            var result = new StringBuilder();
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
                        Magna.Error
                            (
                                nameof(ValueTextNavigator)
                                + "::"
                                + nameof(ReadEscapedUntil)
                                + ": "
                                + "unexpected end of stream"
                            );

                        throw new FormatException();
                    }
                    result.Append(c);
                }
                else if (c == stopChar)
                {
                    break;
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        } // method ReadEscapedUntil

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

            var start = _position;
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
                    _position = start;
                    return ReadOnlySpan<char>.Empty;
                }
                if (c == closeChar)
                {
                    break;
                }
            }

            return _text.Slice
                (
                    start,
                    _position - start
                );
        } // method ReadFrom

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

            var start = _position;
            var c = PeekChar();
            if (!openChars.Contains(c))
            {
                return ReadOnlySpan<char>.Empty;
            }
            ReadChar();

            while (true)
            {
                c = ReadChar();
                if (c == EOF)
                {
                    _position = start;
                    return ReadOnlySpan<char>.Empty;
                }
                if (closeChars.Contains(c))
                {
                    break;
                }
            }

            return _text.Slice
                (
                    start,
                    _position - start
                );
        } // metdhod ReadFrom

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

            var startPosition = _position;
            while (IsDigit())
            {
                ReadChar();
            }

            return _text.Slice
                (
                    startPosition,
                    _position - startPosition
                );
        } // method ReadInteger

        /// <summary>
        /// Чтение до конца строки.
        /// </summary>
        public ReadOnlySpan<char> ReadLine()
        {
            var startPosition = _position;
            while (!IsEOF)
            {
                var c = PeekChar();
                if (c == '\r' || c == '\n')
                {
                    break;
                }
                ReadChar();
            }
            var stopPosition = _position;

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

            return _text.Slice
                (
                    startPosition,
                    stopPosition - startPosition
                );
        } // method ReadLine

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
            Sure.Positive(length, nameof(length));

            var startPosition = _position;
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
                    _position - startPosition
                );
        } // method ReadString

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
            var startPosition = _position;
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
                    startPosition,
                    length: _position - startPosition
                );
        } // method ReadTo

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

            var savePosition = _position;
            var length = 0;
            while (true)
            {
                AGAIN:
                var c = ReadChar();
                if (c == EOF)
                {
                    _position = savePosition;
                    return ReadOnlySpan<char>.Empty;
                }

                length++;
                if (length >= stopString.Length)
                {
                    var start = _position - stopString.Length;
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
                    _position - savePosition - stopString.Length
                );
        } // method ReadTo

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
            var start = _position;
            while (true)
            {
                var c = ReadChar();
                if (c == EOF
                    || Array.IndexOf(stopChars, c) >= 0)
                {
                    break;
                }
            }

            var result = _text.Slice
                (
                    start: start,
                    length: _position - start
                );
            return result;
        } // method ReadTo

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
            var startPosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF || c == stopChar)
                {
                    break;
                }
                ReadChar();
            }

            return _text.Slice
                (
                    startPosition,
                    _position - startPosition
                );
        } // method ReadUntil

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

            var position = _position;
            var length = 0;
            while (true)
            {
                AGAIN:
                var c = ReadChar();
                if (c == EOF)
                {
                    _position = position;
                    return ReadOnlySpan<char>.Empty;
                }

                length++;
                if (length >= stopString.Length)
                {
                    var start = _position - stopString.Length;
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
                    _position - position - stopString.Length
                );
            _position -= stopString.Length;
            return result;
        } // method ReadUntil

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
            var savePosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || Array.IndexOf(stopChars, c) >= 0)
                {
                    break;
                }
                ReadChar();
            }

            return Substring
                (
                    savePosition,
                    _position - savePosition
                );
        } // method ReadUntil

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
            var start = _position;
            var level = 0;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF)
                {
                    _position = start;
                    return ReadOnlySpan<char>.Empty;
                }

                if (openChars.Contains(c))
                {
                    level++;
                }
                else if (closeChars.Contains(c))
                {
                    if (level == 0
                        && stopChars.Contains(c))
                    {
                        break;
                    }
                    level--;
                }
                else if (stopChars.Contains(c))
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
                    _position - start
                );
        } // method ReadUntil

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
            var startPosition = _position;
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
                    _position - startPosition
                );
        } // method ReadWhile

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
            var startPosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || Array.IndexOf(goodChars, c) < 0)
                {
                    break;
                }
                ReadChar();
            }

            return Substring
                (
                    startPosition,
                    _position - startPosition
                );
        } // method ReadWhile

        /// <summary>
        /// Считываем слово под курсором.
        /// </summary>
        public ReadOnlySpan<char> ReadWord()
        {
            var startPosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || !char.IsLetterOrDigit(c))
                {
                    break;
                }
                ReadChar();
            }

            return Substring
                (
                    startPosition,
                    _position - startPosition
                );
        } // metdhod ReadWord

        /// <summary>
        /// Считываем слово под курсором.
        /// </summary>
        public ReadOnlySpan<char> ReadWord
            (
                params char[] additionalWordCharacters
            )
        {
            var savePosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || !char.IsLetterOrDigit(c)
                        && Array.IndexOf(additionalWordCharacters, c) < 0)
                {
                    break;
                }
                ReadChar();
            }

            return Substring
                (
                    savePosition,
                    _position - savePosition
                );
        } // method ReadWord

        /// <summary>
        /// Получаем недавно считанный текст указанной длины.
        /// </summary>
        [Pure]
        public ReadOnlySpan<char> RecentText
            (
                int length
            )
        {
            var start = _position - length;
            if (start < 0)
            {
                length += start;
                start = 0;
            }

            if (start >= _text.Length)
            {
                length = 0;
                start = _text.Length - 1;
            }

            if (length < 0)
            {
                length = 0;
            }

            return Substring (start, length);
        } // method RecentText

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
        } // method SkipChar

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
        } // method SkipChar

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
            if (Array.IndexOf(allowed, PeekChar()) >= 0)
            {
                ReadChar();
                return true;
            }

            return false;
        } // method SkipChar

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
        } // method SkipControl

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
        } // method SkipPunctuation

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
                if (!char.IsLetterOrDigit(c))
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        } // method SkipNonWord

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
                if (!char.IsLetterOrDigit(c)
                    && Array.LastIndexOf(additionalWordCharacters, c) < 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        } // method SkipNonWord

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
        } // method SkipRange

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
        } // method SkipWhile

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
                if (Array.IndexOf(skipChars, c) >= 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        } // method SkipWhile

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

                char c = PeekChar();
                if (c == stopChar)
                {
                    return true;
                }

                ReadChar();
            }
        } // method SkipTo

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
                if (Array.IndexOf(goodChars, c) < 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }
        } // method SkipWhileNot

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
        } // method SkiWhitespace

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
        } // method SkipWhitespaceAndPunctuation

        /// <summary>
        /// Get substring.
        /// </summary>
        [Pure]
        public ReadOnlySpan<char> Substring
            (
                int offset
            )
        {
            return _text.Slice(offset);
        } // method Substring

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
            return offset >= _text.Length || length <= 0
                ? ReadOnlySpan<char>.Empty
                : _text.Slice(offset, length);
        } // method Substring

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        [Pure]
        public override string ToString()
        {
            return $"Position={Position}";
        }

        #endregion

    } // struct ValueTextNavigator
} // namespace AM.Text
