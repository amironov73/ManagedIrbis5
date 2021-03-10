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
                Sure.NonNegative(value, nameof(value));
                Sure.AssertState
                    (
                        value <= _characters.Length,
                        nameof(value)
                    );
                _position = value;
            }
        }

        /// <summary>
        /// Сырой буфер.
        /// </summary>
        public ReadOnlySpan<char> RawCharacters => _characters;

        /// <summary>
        /// Символ по индексу.
        /// </summary>
        public ref char this[int index] => ref _characters[index];

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="characters">Начальный буфер.</param>
        public ValueStringBuilder
            (
                Span<char> characters
            )
            : this()
        {
            _characters = characters;
        }

        #endregion

        #region Private members

        private char[]? _array;
        private Span<char> _characters;
        private int _position;

        #endregion

        #region Public methods

        public ReadOnlySpan<char> AsSpan() =>
            _characters.Slice(0, _position);

        public ReadOnlySpan<char> AsSpan(int start) =>
            _characters.Slice(start, _position - start);

        public ReadOnlySpan<char> AsSpan(int start, int length) =>
            _characters.Slice(start, length);

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
                Grow(1);
            }

            _characters[_position] = c;
            ++_position;
        }

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
                Grow(text.Length);
            }

            text.CopyTo(_characters.Slice(_position));
            _position = newPosition;
        }

        /// <summary>
        /// Добавление пары спанов.
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        public void Append
            (
                ReadOnlySpan<char> text1,
                ReadOnlySpan<char> text2
            )
        {
            var delta = text1.Length + text2.Length;
            int newPosition = _position + delta;
            if (newPosition > _characters.Length)
            {
                Grow(delta);
            }

            text1.CopyTo(_characters.Slice(_position));
            text2.CopyTo(_characters.Slice(_position + text1.Length));
            _position = newPosition;
        }

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
            int newPosition = _position + delta;
            if (newPosition > _characters.Length)
            {
                Grow(delta);
            }

            text1.CopyTo(_characters.Slice(_position));
            text2.CopyTo(_characters.Slice(_position + text1.Length));
            text3.CopyTo(_characters.Slice(_position + text1.Length
                + text2.Length));
            _position = newPosition;
        }

        /// <summary>
        /// Освобождаем ресурсы, если были заняты.
        /// </summary>
        public void Dispose()
        {
            var borrowed = _array;
            this = default; // для спокойствия
            if (borrowed is not null)
            {
                ArrayPool<char>.Shared.Return(borrowed);
            }
        }

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
                Grow(capacity - _position);
            }
        }

        /// <summary>
        /// Увеличение емкости на указанное количество символов.
        /// </summary>
        public void Grow
            (
                int additional
            )
        {
            var newCapacity = (int) Math.Max
                (
                    (uint)(_position + additional),
                    (uint)(Capacity * 2)
                );
            var borrowed = ArrayPool<char>.Shared.Rent(newCapacity);
            _characters.Slice(0, _position).CopyTo(borrowed);
            if (_array is not null)
            {
                ArrayPool<char>.Shared.Return(_array);
            }

            _characters = _array = borrowed;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var result = AsSpan().ToString();
            Dispose();

            return result;
        }

        #endregion

    } // ref struct ValueStringBuilder

} // namespace AM.Text
