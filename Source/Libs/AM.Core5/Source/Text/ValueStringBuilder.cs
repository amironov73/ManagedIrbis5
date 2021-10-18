// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ValueStringBuilder.cs -- StringBuilder, оформленный как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Text
{
    //
    // Вдохновлено кодом из BCL:
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/Text/ValueStringBuilder.cs
    //

    /// <summary>
    /// Аналог системного <see cref="StringBuilder"/>, оформленный
    /// как структура.
    /// </summary>
    public ref struct ValueStringBuilder
    {
        #region Properties

        /// <summary>
        /// Емкость.
        /// </summary>
        public int Capacity => _characters.Length;

        /// <summary>
        /// Текущая длина.
        /// </summary>
        public int Length
        {
            get => _position;
            set
            {
                Sure.NonNegative (value, nameof (value));
                Sure.AssertState (value <= _characters.Length);
                _position = value;
            }

        } // property Length

        /// <summary>
        /// Сырой буфер.
        /// </summary>
        public ReadOnlySpan<char> RawCharacters => _characters;

        /// <summary>
        /// Доступ по индексу.
        /// </summary>
        public ref char this [int index] => ref _characters[index];

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="characters">Начальный буфер.</param>
        public ValueStringBuilder
            (
                Span<char> characters
            )
            : this()
        {
            _characters = characters;

        } // constructor

        #endregion

        #region Private members

        private char[]? _array;
        private Span<char> _characters;
        private int _position;

        #endregion

        #region Public methods

        /// <summary>
        /// Выдача построенного на данный момент значения как спана.
        /// </summary>
        public ReadOnlySpan<char> AsSpan() =>
            _characters.Slice (0, _position);

        /// <summary>
        /// Выдача построенного на данный момент значения как спана.
        /// </summary>
        public ReadOnlySpan<char> AsSpan (int start) =>
            _characters.Slice (start, _position - start);

        /// <summary>
        /// Выдача построенного на данный момент значения как спана.
        /// </summary>
        public ReadOnlySpan<char> AsSpan (int start, int length) =>
            _characters.Slice (start, length);

        /// <summary>
        /// Добавление одного символа.
        /// </summary>
        public void Append
            (
                char c
            )
        {
            if (_position == _characters.Length)
            {
                Grow (1);
            }

            _characters[_position] = c;
            ++_position;

        } // method Append

        /// <summary>
        /// Добавление спана символов.
        /// </summary>
        public void Append
            (
                ReadOnlySpan<char> text
            )
        {
            var newPosition = _position + text.Length;
            if (newPosition > _characters.Length)
            {
                Grow (text.Length);
            }

            text.CopyTo (_characters.Slice (_position));
            _position = newPosition;

        } // method Append

        /// <summary>
        /// Добавление пары спанов.
        /// </summary>
        public void Append
            (
                ReadOnlySpan<char> text1,
                ReadOnlySpan<char> text2
            )
        {
            var delta = text1.Length + text2.Length;
            var newPosition = _position + delta;
            if (newPosition > _characters.Length)
            {
                Grow (delta);
            }

            text1.CopyTo (_characters.Slice (_position));
            text2.CopyTo (_characters.Slice (_position + text1.Length));
            _position = newPosition;

        } // method Append

        /// <summary>
        /// Добавление трех спанов.
        /// </summary>
        public void Append
            (
                ReadOnlySpan<char> text1,
                ReadOnlySpan<char> text2,
                ReadOnlySpan<char> text3
            )
        {
            var delta = text1.Length + text2.Length + text3.Length;
            var newPosition = _position + delta;
            if (newPosition > _characters.Length)
            {
                Grow (delta);
            }

            text1.CopyTo (_characters.Slice (_position));
            text2.CopyTo (_characters.Slice (_position + text1.Length));
            text3.CopyTo (_characters.Slice (_position + text1.Length + text2.Length));
            _position = newPosition;

        } // method Append

        /// <summary>
        /// Добавление целого числа со знаком.
        /// </summary>
        public unsafe void Append
            (
                int value
            )
        {
            var remaining = _characters.Length - _position;
            if (remaining >= 10)
            {
                var buffer = _characters.Slice (_position);
                var written = FastNumber.Int32ToChars (value, buffer);
                _position += written;
            }
            else
            {
                Span<char> buffer = stackalloc char[10];
                var written = FastNumber.Int32ToChars (value, buffer);
                var newPosition = _position + written;
                if (newPosition > _characters.Length)
                {
                    Grow (written);
                }

                buffer.Slice (0, written).CopyTo (_characters.Slice (_position));
                _position = newPosition;
            }

        } // method Append

        /// <summary>
        /// Добавление перевода строки.
        /// </summary>
        public void AppendLine() => Append (Environment.NewLine);

        /// <summary>
        /// Освобождаем ресурсы, если были заняты.
        /// </summary>
        public void Dispose()
        {
            var borrowed = _array;
            this = default; // для спокойствия
            if (borrowed is not null)
            {
                ArrayPool<char>.Shared.Return (borrowed);
            }

        } // method Dispose

        /// <summary>
        /// Увеличение емкости, если необходимо.
        /// </summary>
        public void EnsureCapacity
            (
                int capacity
            )
        {
            if (capacity > _characters.Length)
            {
                Grow (capacity - _position);
            }

        } // method EnsureCapacity

        /// <summary>
        /// Увеличение емкости на указанное количество символов.
        /// </summary>
        public void Grow
            (
                int additional
            )
        {
            var newCapacity = (int)Math.Max
                (
                    (uint)(_position + additional),
                    (uint)(Capacity * 2)
                );
            var borrowed = ArrayPool<char>.Shared.Rent (newCapacity);
            _characters.Slice (0, _position).CopyTo (borrowed);
            if (_array is not null)
            {
                ArrayPool<char>.Shared.Return (_array);
            }

            _characters = _array = borrowed;

        } // method Grow

        /// <summary>
        /// Получение перечислителя.
        /// </summary>
        public ReadOnlySpan<char>.Enumerator GetEnumerator() =>
            AsSpan().GetEnumerator();

        /// <summary>
        /// Чтение строки непосредственно в <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="reader">Поток, из которого считывается строка.</param>
        /// <param name="appendNewLine">Добавлять перевод строки в конец?</param>
        /// <returns><c>false</c>, если достигнут конец потока.</returns>
        public bool ReadLine
            (
                TextReader reader,
                bool appendNewLine = false
            )
        {
            var first = true;
            while (true)
            {
                var chr = reader.Read();
                if (chr < 0)
                {
                    return !first;
                }

                if (chr == '\n')
                {
                    if (appendNewLine)
                    {
                        Append ((char)chr);
                    }

                    return true;
                }

                if (chr != '\r')
                {
                    Append ((char)chr);
                }

                first = false;
            }

        } // method ReadLine

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var result = AsSpan().ToString();
            Dispose();

            return result;

        } // method ToString

        #endregion

    } // ref struct ValueStringBuilder

} // namespace AM.Text
