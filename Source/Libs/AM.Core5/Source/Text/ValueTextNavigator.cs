// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ReplaceSliceWithRangeIndexer

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
    /// <example>
    /// Пример разбиения текста на слова.
    /// <code>
    /// <remarks>
    /// Все методы класса не потокобезопасные.
    /// </remarks>
    /// var text = "У попа была собака, он её любил.";
    /// var navigator = new ValueTextNavigator(text);
    ///
    /// while (true)
    /// {
    ///   var word = navigator.ReadWord();
    ///   if (word.IsEmpty)
    ///   {
    ///      break;
    ///   }
    ///
    ///   Console.WriteLine(word.ToString());
    ///
    ///   if (!navigator.SkipNonWord())
    ///   {
    ///      break;
    ///   }
    /// }
    /// </code>
    /// </example>
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
        [Pure]
        public bool IsEOF => _position >= _text.Length;

        /// <summary>
        /// Общая длина текста в символах.
        /// </summary>
        [Pure]
        public int Length => _text.Length;

        /// <summary>
        /// Текущая позиция в тексте.
        /// </summary>
        [Pure]
        public int Position => _position;

        /// <summary>
        /// Текст, хранимый в навигаторе.
        /// </summary>
        [Pure]
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
        /// Клонирование навигатора (включая текущую позицию в тексте).
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
        public ReadOnlySpan<char> GetRemainingText() => IsEOF
            ? new ReadOnlySpan<char>()
            : _text.Slice(_position);

        /// <summary>
        /// Текущий символ - управляющий?
        /// </summary>
        [Pure]
        public bool IsControl() => char.IsControl(PeekChar());

        /// <summary>
        /// Текущий символ - цифра?
        /// </summary>
        [Pure]
        public bool IsDigit() => char.IsDigit(PeekChar());

        /// <summary>
        /// Текущий символ - буква?
        /// </summary>
        [Pure]
        public bool IsLetter() => char.IsLetter(PeekChar());

        /// <summary>
        /// Текущий символ - буква или цифра?
        /// </summary>
        [Pure]
        public bool IsLetterOrDigit() => char.IsLetterOrDigit(PeekChar());

        /// <summary>
        /// Текущий символ - часть числа?
        /// </summary>
        [Pure]
        public bool IsNumber() => char.IsNumber(PeekChar());

        /// <summary>
        /// Текущий символ - знак пунктуации?
        /// </summary>
        [Pure]
        public bool IsPunctuation() => char.IsPunctuation(PeekChar());

        /// <summary>
        /// Текущий символ - разделитель?
        /// </summary>
        [Pure]
        public bool IsSeparator() => char.IsSeparator(PeekChar());

        /// <summary>
        /// Текущий символ принадлежит одной
        /// из Unicode категорий: MathSymbol,
        /// CurrencySymbol, ModifierSymbol либо OtherSymbol?
        /// </summary>
        [Pure]
        public bool IsSymbol() => char.IsSymbol(PeekChar());

        /// <summary>
        /// Текущий символ - пробельный?
        /// </summary>
        [Pure]
        public bool IsWhiteSpace() => char.IsWhiteSpace(PeekChar());

        /// <summary>
        /// Заглядывание вперёд на одну позицию.
        /// Движения по тексту не происходит.
        /// </summary>
        /// <remarks>Это на одну позицию дальше,
        /// чем <see cref="PeekChar()"/>
        /// </remarks>
        [Pure]
        public char LookAhead()
        {
            var ahead = _position + 1;
            return ahead >= _text.Length
                ? EOF
                : _text[ahead];

        } // method LookAhead

        /// <summary>
        /// Заглядывание вперёд на указанное количество символов.
        /// Движения по тексту не происходит.
        /// </summary>
        [Pure]
        public char LookAhead
            (
                int distance
            )
        {
            Sure.NonNegative(distance, nameof(distance));

            var ahead = _position + distance;
            return ahead >= _text.Length
                ? EOF
                : _text[ahead];

        } // method LookAhead

        /// <summary>
        /// Заглядывание назад на одну позицию.
        /// Движения по тексту не происходит.
        /// </summary>
        [Pure]
        public char LookBehind() => _position == 0 ? EOF : _text[_position - 1];

        /// <summary>
        /// Заглядывание назад на указанное число позиций.
        /// Движения по тексту не происходит.
        /// </summary>
        /// <param name="distance">Дистанция, на которую
        /// предполагается заглянуть - положительное число,
        /// означающее количество символов, на которые
        /// нужно "отмотать назад".</param>
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
        /// Относительное перемещение указателя на указанную дистанцию.
        /// </summary>
        /// <remarks>
        /// Переместить указатель за пределы текста не получится,
        /// он остановится в крайней (начальной или конечной) позиции.
        /// </remarks>
        public void Move (int distance) =>
            _position = Math.Max (0, Math.Min(_position + distance, _text.Length));

        /// <summary>
        /// Подглядывание текущего символа (т. е. символа в текущей позиции).
        /// </summary>
        [Pure]
        public char PeekChar() => _position >= _text.Length
                ? EOF
                : _text[_position];

        /// <summary>
        /// Подглядывание строки вплоть до указанной длины.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        /// <remarks>
        /// Символы перевода строки в данном методе
        /// считаются обычными символами и включаются в результат.
        /// </remarks>
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> PeekTo
            (
                char stopChar
            )
        {
            var position = _position;
            var result = ReadTo(stopChar);
            _position = position;

            return result;

        } // method PeekTo

        /// <summary>
        /// Подглядывание вплоть до указанных символов
        /// (включая их).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> PeekTo
            (
                params char[] stopChars
            )
        {
            var position = _position;
            var result = ReadTo(stopChars);
            _position = position;

            return result;

        } // method PeekTo

        /// <summary>
        /// Подглядывание вплоть до указанного символа
        /// (не включая его).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
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
        /// Подглядывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
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
        /// Считывание одного символа.
        /// Если достигнут конец текста, возвращается
        /// <see cref="EOF"/>.
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
        /// <param name="escapeChar">Экранирующий символ,
        /// как правило, '\\'.</param>
        /// <param name="stopChar">Стоп-символ (разделитель).</param>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        /// <exception cref="FormatException">Нарушен формат (есть символ
        /// экранирования, но за ним строка обрывается).
        /// </exception>
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или нет открывающего либо закрывающего символа.
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
            if (PeekChar() != openChar)
            {
                return ReadOnlySpan<char>.Empty;
            }
            ReadChar();

            while (true)
            {
                var c = ReadChar();
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или если нет открывающего либо закрывающего символа.
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
            if (!openChars.Contains(PeekChar()))
            {
                return ReadOnlySpan<char>.Empty;
            }
            ReadChar();

            while (true)
            {
                var c = ReadChar();
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не цифровой.</returns>
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
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
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </remarks>
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
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
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> ReadUntil
            (
                char stopChar
            )
        {
            var start = _position;
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
                    start,
                    _position - start
                );

        } // method ReadUntil

        /// <summary>
        /// Считывание вплоть до указанного разделителя
        /// (разделитель не помещается в возвращаемое значение
        /// и не считывается).
        /// </summary>
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </remarks>
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

            var result = _text.Slice
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
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста.
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

            return _text.Slice
                (
                    savePosition,
                    _position - savePosition
                );

        } // method ReadUntil

        /// <summary>
        /// Считывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не открывающий.
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

            return _text.Slice
                (
                    start,
                    _position - start
                );

        } // method ReadUntil

        /// <summary>
        /// Считывание, пока встречается указанный символ.
        /// </summary>
        /// <returns><c>Простой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не совпадает с указанным.
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

            return _text.Slice
                (
                    startPosition,
                    _position - startPosition
                );

        } // method ReadWhile

        /// <summary>
        /// Считывание, пока встречается указанные символы.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не входит в число "хороших".
        /// </returns>
        public ReadOnlySpan<char> ReadWhile
            (
                params char[] goodChars
            )
        {
            var start = _position;
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

            return _text.Slice
                (
                    start,
                    _position - start
                );

        } // method ReadWhile

        /// <summary>
        /// Считываем слово, начиная с текущей позиции.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ "не-словесный".
        /// </returns>
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

            return _text.Slice
                (
                    startPosition,
                    _position - startPosition
                );

        } // metdhod ReadWord

        /// <summary>
        /// Считываем слово под курсором.
        /// </summary>
        /// <param name="additionalWordCharacters">Дополнительные символы,
        /// которые мы полагаем "словесными".</param>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ "не-словесный".
        /// </returns>
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

            return _text.Slice
                (
                    savePosition,
                    _position - savePosition
                );

        } // method ReadWord

        /// <summary>
        /// Получаем недавно считанный текст указанной длины.
        /// </summary>
        /// <param name="length">Желаемая длина текста в символах
        /// (положительное целое).</param>
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
        /// <returns><c>true</c>, если символ был съеден успешно.
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
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// Пропускаем "не-словесные" символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// Пропускаем "не-словесные" символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// из указанного диапазона (например, от 'A' до 'Z').
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// Пропустить, пока не встретится указанный стоп-символ.
        /// Сам стоп-символ не считывается.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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

        } // method SkipTo

        /// <summary>
        /// Пропустить, пока не встретятся указанные символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
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
        /// Извлечение подстроки, начиная с указанного смещения.
        /// </summary>
        [Pure]
        public ReadOnlySpan<char> Substring
            (
                int offset
            )
        {
            return offset < 0
                || offset >= _text.Length
                ? ReadOnlySpan<char>.Empty
                : _text.Slice(offset);

        } // method Substring

        /// <summary>
        /// Извлечение подстроки.
        /// </summary>
        /// <remarks>Если параметры заданы неверно,
        /// метод может выбросить исключение
        /// <see cref="ArgumentOutOfRangeException"/>.</remarks>
        /// <param name="offset">Смещение до начала подстроки в символах.</param>
        /// <param name="length">Длина подстроки в символах.</param>
        [Pure]
        public ReadOnlySpan<char> Substring
            (
                int offset,
                int length
            )
        {
            return offset < 0
                || offset >= _text.Length
                || length <= 0
                ? ReadOnlySpan<char>.Empty
                : _text.Slice(offset, length);

        } // method Substring

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        [Pure]
        public override string ToString() => $"Position={Position}";

        #endregion

    } // struct ValueTextNavigator

} // namespace AM.Text
