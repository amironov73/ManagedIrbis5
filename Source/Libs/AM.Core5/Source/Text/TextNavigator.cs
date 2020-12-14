// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TextNavigator.cs -- навигатор по тексту
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
    /// Навигатор по тексту.
    /// </summary>
    public sealed class TextNavigator
    {
        #region Nested structures

        readonly struct StateHolder
            : IDisposable
        {
            private readonly TextNavigator _navigator;
            private readonly int _column;
            private readonly int _line;
            public readonly int Position;

            public StateHolder(TextNavigator navigator)
                : this()
            {
                _navigator = navigator;
                _column = navigator._column;
                _line = navigator._line;
                Position = navigator._position;
            }

            public void Dispose()
            {
                _navigator._column = _column;
                _navigator._line = _line;
                _navigator._position = Position;
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// Признак конца текста.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const char EOF = '\0';

        #endregion

        #region Properties

        /// <summary>
        /// Текущая колонка текста. Нумерация с 1.
        /// </summary>
        public int Column => _column;

        /// <summary>
        /// Текст закончился?
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool IsEOF => _position >= _length;

        /// <summary>
        /// Длина текста.
        /// </summary>
        public int Length => _length;

        /// <summary>
        /// Текущая строка текста. Нумерация с 1.
        /// </summary>
        public int Line => _line;

        /// <summary>
        /// Текущая позиция.
        /// </summary>
        public int Position => _position;

        /// <summary>
        /// Обрабатываемый текст.
        /// </summary>
        public string Text => _text.ToString();

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextNavigator
            (
                ReadOnlyMemory<char> text
            )
        {
            _text = text;
            _position = 0;
            _length = _text.Length;
            _line = 1;
            _column = 1;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TextNavigator
            (
                string text,
                bool normalize = false
            )
        {
            _text = (normalize ? text.Normalize() : text).AsMemory();
            _position = 0;
            _length = _text.Length;
            _line = 1;
            _column = 1;
        }

        #endregion

        #region Private members

        private readonly ReadOnlyMemory<char> _text;

        private readonly int _length;
        private int _position, _line, _column;

        private static ReadOnlyMemory<char> EmptySpan => new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the navigator.
        /// </summary>
        [Pure]
        public TextNavigator Clone()
        {
            var result = new TextNavigator(_text)
            {
                _column = _column,
                _line = _line,
                _position = _position
            };

            return result;
        }

        /// <summary>
        /// Навигатор по текстовому файлу.
        /// </summary>
        public static TextNavigator FromFile
            (
                string fileName,
                Encoding? encoding = default
            )
        {
            Sure.FileExists(fileName, nameof(fileName));

            encoding ??= Encoding.UTF8;
            var text = File.ReadAllText(fileName, encoding);
            var result = new TextNavigator(text);
            return result;
        }

        /// <summary>
        /// Выдать остаток текста.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        [Pure]
        public ReadOnlyMemory<char> GetRemainingText()
        {
            return IsEOF
                ? EmptySpan
                : _text.Slice(_position);
        }

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
            return newPosition >= _length
                ? EOF
                : _text.Span[newPosition];
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
            Sure.NonNegative(distance, nameof(distance));

            var newPosition = _position + distance;
            return newPosition >= _length
                ? EOF
                : _text.Span[newPosition];
        }

        /// <summary>
        /// Заглядывание назад.
        /// </summary>
        [Pure]
        public char LookBehind()
        {
            return _position == 0
                ? EOF
                : _text.Span[_position - 1];
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
            Sure.Positive(distance, nameof(distance));

            return _position < distance
                ? EOF
                : _text.Span[_position - distance];
        }

        /// <summary>
        /// Смещение указателя.
        /// </summary>
        public TextNavigator Move
            (
                int distance
            )
        {
            // TODO Some checks

            _position += distance;
            _column += distance;
            return this;
        }

        /// <summary>
        /// Подглядывание текущего символа.
        /// </summary>
        [Pure]
        public char PeekChar() => _position >= _length
                ? EOF
                : _text.Span[_position];

        /// <summary>
        /// Подглядывание строки вплоть до указанной длины.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> PeekString
            (
                int length
            )
        {
            Sure.Positive(length, nameof(length));

            if (IsEOF)
            {
                return EmptySpan;
            }

            using (new StateHolder(this))
            {
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
                return result;
            }
        }

        /// <summary>
        /// Подглядывание вплоть до указанного символа
        /// (включая его).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> PeekTo
            (
                char stopChar
            )
        {
            using (new StateHolder(this))
            {
                var result = ReadTo(stopChar);
                return result;
            }
        }

        /// <summary>
        /// Подглядывание вплоть до указанных символов
        /// (включая их).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> PeekTo
            (
                params char[] stopChars
            )
        {
            using (new StateHolder(this))
            {
                var result = ReadTo(stopChars);
                return result;
            }
        }

        /// <summary>
        /// Подглядывание вплоть до указанного символа.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> PeekUntil
            (
                char stopChar
            )
        {
            using (new StateHolder(this))
            {
                var result = ReadUntil(stopChar);
                return result;
            }
        }

        /// <summary>
        /// Подглядывание вплоть до указанных символов.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> PeekUntil
            (
                char[] stopChars
            )
        {
            using (new StateHolder(this))
            {
                var result = ReadUntil(stopChars);
                return result;
            }
        }

        /// <summary>
        /// Считывание символа.
        /// </summary>
        public char ReadChar()
        {
            if (_position >= _length)
            {
                return EOF;
            }

            var result = _text.Span[_position];
            _position++;
            if (result == '\n')
            {
                _line++;
                _column = 1;
            }
            else
            {
                _column++;
            }

            return result;
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
                                nameof(TextNavigator)
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
        }

        /// <summary>
        /// Считывание начиная с открывающего символа
        /// до закрывающего (включая их).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// Пустая строка, если нет открывающего
        /// или закрывающего символа.
        /// </returns>
        public ReadOnlyMemory<char> ReadFrom
            (
                char openChar,
                char closeChar
            )
        {
            if (IsEOF)
            {
                return EmptySpan;
            }

            var state = new StateHolder(this);
            var c = PeekChar();
            if (c != openChar)
            {
                state.Dispose();
                return EmptySpan;
            }
            ReadChar();

            while (true)
            {
                c = ReadChar();
                if (c == EOF)
                {
                    state.Dispose();
                    return EmptySpan;
                }
                if (c == closeChar)
                {
                    break;
                }
            }

            return Substring
                (
                    state.Position,
                    _position - state.Position
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
        public ReadOnlyMemory<char> ReadFrom
            (
                char[] openChars,
                char[] closeChars
            )
        {
            if (IsEOF)
            {
                return EmptySpan;
            }

            var state = new StateHolder(this);
            var c = PeekChar();
            if (Array.IndexOf(openChars, c) < 0)
            {
                state.Dispose();
                return EmptySpan;
            }
            ReadChar();

            while (true)
            {
                c = ReadChar();
                if (c == EOF)
                {
                    state.Dispose();
                    return EmptySpan;
                }
                if (Array.IndexOf(closeChars, c) >= 0)
                {
                    break;
                }
            }

            return Substring
                (
                    state.Position,
                    _position - state.Position
                );
        }

        /// <summary>
        /// Чтение беззнакового целого.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// Пустую строку, если не число.</returns>
        public ReadOnlyMemory<char> ReadInteger()
        {
            if (!IsDigit())
            {
                return EmptySpan;
            }

            var startPosition = _position;
            while (IsDigit())
            {
                ReadChar();
            }

            return Substring
                (
                    startPosition,
                    _position - startPosition
                );
        }

        /// <summary>
        /// Чтение до конца строки.
        /// </summary>
        public ReadOnlyMemory<char> ReadLine()
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
        public ReadOnlyMemory<char> ReadString
            (
                int length
            )
        {
            // Sure.Positive(length, nameof(length));

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
        }

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (включая его).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> ReadTo
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
        }

        /// <summary>
        /// Считывание вплоть до указанного разделителя
        /// (разделитель не помещается в возвращаемое значение,
        /// однако, считывается).
        /// </summary>
        public ReadOnlyMemory<char> ReadTo
            (
                string stopString
            )
        {
            // Sure.NotNullNorEmpty(stopString, nameof(stopString));

            var state = new StateHolder(this);
            var length = 0;
            var span = _text.Span;
            while (true)
            {
                AGAIN:
                var c = ReadChar();
                if (c == EOF)
                {
                    state.Dispose();
                    return EmptySpan;
                }

                length++;
                if (length >= stopString.Length)
                {
                    var start = _position - stopString.Length;
                    for (var i = 0; i < stopString.Length; i++)
                    {
                        if (span[start + i] != stopString[i])
                        {
                            goto AGAIN;
                        }
                    }
                    break;
                }
            }

            return Substring
                (
                    state.Position,
                    _position - state.Position - stopString.Length
                );
        }

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (включая один из них).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> ReadTo
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
        }

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (не включая его).
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> ReadUntil
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

            return Substring
                (
                    startPosition,
                    _position - startPosition
                );
        }

        /// <summary>
        /// Считывание вплоть до указанного разделителя
        /// (разделитель не помещается в возвращаемое значение
        /// и не считывается).
        /// </summary>
        public ReadOnlyMemory<char> ReadUntil
            (
                string stopString
            )
        {
            // Sure.NotNullNorEmpty(stopString, nameof(stopString));

            var state = new StateHolder(this);
            var length = 0;
            var span = _text.Span;
            while (true)
            {
                AGAIN:
                var c = ReadChar();
                if (c == EOF)
                {
                    state.Dispose();
                    return EmptySpan;
                }

                length++;
                if (length >= stopString.Length)
                {
                    var start = _position - stopString.Length;
                    for (var i = 0; i < stopString.Length; i++)
                    {
                        if (span[start + i] != stopString[i])
                        {
                            goto AGAIN;
                        }
                    }
                    break;
                }
            }

            var result = Substring
                (
                    state.Position,
                    _position - state.Position - stopString.Length
                );
            _position -= stopString.Length;
            return result;
        }

        /// <summary>
        /// Считывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <remarks><c>null</c>, если достигнут конец текста.
        /// </remarks>
        public ReadOnlyMemory<char> ReadUntil
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
        }

        /// <summary>
        /// Считывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <remarks><c>null</c>, если достигнут конец текста.
        /// </remarks>
        public ReadOnlyMemory<char> ReadUntil
            (
                char[] openChars,
                char[] closeChars,
                char[] stopChars
            )
        {
            var state = new StateHolder(this);
            var level = 0;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF)
                {
                    state.Dispose();
                    return EmptySpan;
                }

                if (c.IsOneOf(openChars))
                {
                    level++;
                }
                else if (c.IsOneOf(closeChars))
                {
                    if (level == 0
                        && c.IsOneOf(stopChars))
                    {
                        break;
                    }
                    level--;
                }
                else if (c.IsOneOf(stopChars))
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
                    state.Position,
                    _position - state.Position
                );
        }

        /// <summary>
        /// Считывание, пока встречается указанный символ.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> ReadWhile
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
        }

        /// <summary>
        /// Считывание, пока встречается указанные символы.
        /// </summary>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlyMemory<char> ReadWhile
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
        }

        /// <summary>
        /// Read word.
        /// </summary>
        public ReadOnlyMemory<char> ReadWord()
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
        }

        /// <summary>
        /// Read word.
        /// </summary>
        public ReadOnlyMemory<char> ReadWord
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
        }

        /// <summary>
        /// Get recent text.
        /// </summary>
        [Pure]
        public ReadOnlyMemory<char> RecentText
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

        /*
        /// <summary>
        /// Restore previously saved position.
        /// </summary>
        public void RestorePosition
            (
                TextPosition saved
            )
        {
            _column = saved.Column;
            _line = saved.Line;
            _position = saved.Position;
        }

        /// <summary>
        /// Save current position.
        /// </summary>
        [Pure]
        public TextPosition SavePosition()
        {
            return new TextPosition(this);
        }
        */

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
            if (Array.IndexOf(allowed, PeekChar()) >= 0)
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
                if (!char.IsLetterOrDigit(c))
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
                if (Array.IndexOf(skipChars, c) >= 0)
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
                if (Array.IndexOf(goodChars, c) < 0)
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

//        /// <summary>
//        /// Split text by given good characters.
//        /// </summary>
//        [ItemNotNull]
//        public string[] SplitByGoodCharacters
//            (
//                params char[] goodCharacters
//            )
//        {
//            List<string> result = new List<string>();
//
//            while (!IsEOF)
//            {
//                if (SkipWhileNot(goodCharacters))
//                {
//                    var word = ReadWhile(goodCharacters);
//                    if (!string.IsNullOrEmpty(word))
//                    {
//                        result.Add(word);
//                    }
//                }
//            }
//
//            return result.ToArray();
//        }
//
//        /// <summary>
//        /// Split the remaining text to array of words.
//        /// </summary>
//        [ItemNotNull]
//        public string[] SplitToWords()
//        {
//            var result = new List<string>();
//
//            while (true)
//            {
//                if (!SkipNonWord())
//                {
//                    break;
//                }
//
//                var word = ReadWord();
//                if (!string.IsNullOrEmpty(word))
//                {
//                    result.Add(word);
//                }
//            }
//
//            return result.ToArray();
//        }
//
//        /// <summary>
//        /// Split the remaining text to array of words.
//        /// </summary>
//        [ItemNotNull]
//        public string[] SplitToWords
//            (
//                params char[] additionalWordCharacters
//            )
//        {
//            var result = new List<string>();
//
//            while (true)
//            {
//                if (!SkipNonWord(additionalWordCharacters))
//                {
//                    break;
//                }
//
//                var word = ReadWord(additionalWordCharacters);
//                if (!string.IsNullOrEmpty(word))
//                {
//                    result.Add(word);
//                }
//            }
//
//            return result.ToArray();
//        }

        /// <summary>
        /// Get substring.
        /// </summary>
        [Pure]
        public ReadOnlyMemory<char> Substring
            (
                int offset
            )
        {
            return _text.Slice(offset);
        }

        /// <summary>
        /// Get substring.
        /// </summary>
        [Pure]
        public ReadOnlyMemory<char> Substring
            (
                int offset,
                int length
            )
        {
            return offset >= _text.Length || length <= 0
                ? EmptySpan
                : _text.Slice(offset, length);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        [Pure]
        public override string ToString()
        {
            return $"Line={Line}, Column={Column}";
        }

        #endregion
    }
}