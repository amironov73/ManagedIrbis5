// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ValueList.cs -- List<T>, оформленный как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// <see cref="List{T}"/>, оформленный как структура.
    /// </summary>
    public ref struct ValueList<T>
    {
        #region Properties

        public int Capacity => _values.Length;

        public int Length
        {
            get => _position;
            set
            {
                Sure.NonNegative(value, nameof(value));
                Sure.AssertState
                    (
                        value <= _values.Length,
                        nameof(value)
                    );
                _position = value;
            }
        }

        /// <summary>
        /// Сырой буфер.
        /// </summary>
        public ReadOnlySpan<T> RawBuffer => _values;

        /// <summary>
        /// Доступ по индексу.
        /// </summary>
        public ref T this[int index] => ref _values[index];

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="initialBuffer">Начальный буфер.</param>
        public ValueList
            (
                Span<T> initialBuffer
            )
            : this()
        {
            _values = initialBuffer;
        } // constructor

        #endregion

        #region Private members

        private T[]? _array;
        private Span<T> _values;
        private int _position;

        #endregion

        #region Public methods

        public ReadOnlySpan<T> AsSpan() =>
            _values.Slice(0, _position);

        public ReadOnlySpan<T> AsSpan(int start) =>
            _values.Slice(start, _position - start);

        public ReadOnlySpan<T> AsSpan(int start, int length) =>
            _values.Slice(start, length);

        /// <summary>
        /// Добавление одного символа.
        /// </summary>
        public void Append
            (
                T one
            )
        {
            if (_position == _values.Length)
            {
                Grow(1);
            }

            _values[_position] = one;
            ++_position;
        } // method Append

        /// <summary>
        /// Добавление спана значений.
        /// </summary>
        public void Append
            (
                ReadOnlySpan<T> values
            )
        {
            var newPosition = _position + values.Length;
            if (newPosition > _values.Length)
            {
                Grow(values.Length);
            }

            values.CopyTo(_values.Slice(_position));
            _position = newPosition;
        } // method Append

        /// <summary>
        /// Добавление пары спанов.
        /// </summary>
        public void Append
            (
                ReadOnlySpan<T> one,
                ReadOnlySpan<T> two
            )
        {
            var delta = one.Length + two.Length;
            var newPosition = _position + delta;
            if (newPosition > _values.Length)
            {
                Grow(delta);
            }

            one.CopyTo(_values.Slice(_position));
            two.CopyTo(_values.Slice(_position + one.Length));
            _position = newPosition;
        }

        /// <summary>
        /// Добавление трех спанов.
        /// </summary>
        public void Append
            (
                ReadOnlySpan<T> one,
                ReadOnlySpan<T> two,
                ReadOnlySpan<T> three
            )
        {
            var delta = one.Length + two.Length + three.Length;
            int newPosition = _position + delta;
            if (newPosition > _values.Length)
            {
                Grow(delta);
            }

            one.CopyTo(_values.Slice(_position));
            two.CopyTo(_values.Slice(_position + one.Length));
            three.CopyTo(_values.Slice(_position + one.Length
                + two.Length));
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
                ArrayPool<T>.Shared.Return(borrowed);
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
            if (capacity > _values.Length)
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
            var borrowed = ArrayPool<T>.Shared.Rent(newCapacity);
            _values.Slice(0, _position).CopyTo(borrowed);
            if (_array is not null)
            {
                ArrayPool<T>.Shared.Return(_array);
            }

            _values = _array = borrowed;
        }

        /// <summary>
        /// Превращение в массив.
        /// </summary>
        public T[] ToArray()
        {
            var result = AsSpan().ToArray();
            Dispose();

            return result;
        }

        /// <summary>
        /// Получение перечислителя.
        /// </summary>
        public ReadOnlySpan<T>.Enumerator GetEnumerator() =>
            AsSpan().GetEnumerator();

        #endregion

    } // class ValueList

} // namespace AM.Collections
