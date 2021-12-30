// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* PooledList.cs -- List<T> поверх пула массивов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// <see cref="List{T}"/> поверх пула массивов.
/// </summary>
public ref struct PooledList<T>
{
    #region Properties

    /// <summary>
    /// Текущая емкость списка.
    /// </summary>
    public int Capacity => _array?.Length ?? 0;

    /// <summary>
    /// Текущая длина списка.
    /// </summary>
    public int Length
    {
        get => _position;
        set
        {
            Sure.NonNegative (value);
            Sure.AssertState (value <= Capacity);
            _position = value;
        }
    }

    /// <summary>
    /// Сырой буфер.
    /// </summary>
    public ReadOnlySpan<T> RawBuffer => _array;

    /// <summary>
    /// Доступ по индексу.
    /// </summary>
    public ref T this [int index] => ref _array[index];

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PooledList()
    {
        _pool = ArrayPool<T>.Shared;
        _array = Array.Empty<T>();
        _position = 0;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PooledList
        (
            ArrayPool<T> pool
        )
    {
        _pool = pool;
        _array = Array.Empty<T>();
        _position = 0;
    }

    #endregion

    #region Private members

    private readonly ArrayPool<T> _pool;
    private T[] _array;
    private int _position;

    #endregion

    #region Public methods

        /// <summary>
    /// Выдача списка как спана.
    /// </summary>
    public ReadOnlySpan<T> AsSpan()
    {
        return _array.AsSpan().Slice (0, _position);
    }

    /// <summary>
    /// Выдача списка как спана.
    /// </summary>
    public ReadOnlySpan<T> AsSpan
        (
            int start
        )
    {
        return _array.AsSpan().Slice (start, _position - start);
    }

    /// <summary>
    /// Выдача списка как спана.
    /// </summary>
    public ReadOnlySpan<T> AsSpan
        (
            int start,
            int length
        )
    {
        return _array.AsSpan().Slice (start, length);
    }

    /// <summary>
    /// Добавление одного символа.
    /// </summary>
    public void Append
        (
            T one
        )
    {
        if (_position == _array.Length)
        {
            Grow (1);
        }

        _array[_position] = one;
        ++_position;
    }

    /// <summary>
    /// Добавление спана значений.
    /// </summary>
    public void Append
        (
            ReadOnlySpan<T> values
        )
    {
        var newPosition = _position + values.Length;
        if (newPosition > _array.Length)
        {
            Grow (values.Length);
        }

        values.CopyTo (_array.AsSpan().Slice (_position));
        _position = newPosition;
    }

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
        if (newPosition > _array.Length)
        {
            Grow (delta);
        }

        var span = _array.AsSpan();
        one.CopyTo (span.Slice (_position));
        two.CopyTo (span.Slice (_position + one.Length));
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
        if (newPosition > _array.Length)
        {
            Grow (delta);
        }

        var span = _array.AsSpan();
        one.CopyTo (span.Slice (_position));
        two.CopyTo (span.Slice (_position + one.Length));
        three.CopyTo (span.Slice (_position + one.Length + two.Length));
        _position = newPosition;
    }

    /// <summary>
    /// Увеличение емкости, если необходимо.
    /// </summary>
    public void EnsureCapacity
        (
            int capacity
        )
    {
        if (capacity > _array.Length)
        {
            Grow (capacity - _position);
        }
    }

    /// <summary>
    /// Увеличение емкости на указанное количество символов.
    /// </summary>
    public void Grow
        (
            int additionalCapacity
        )
    {
        Sure.Positive (additionalCapacity);

        var newCapacity = (int) Math.Max
            (
                (uint) (_position + additionalCapacity),
                (uint) (Capacity * 2)
            );
        var borrowed = ArrayPool<T>.Shared.Rent (newCapacity);
        _array.AsSpan().Slice (0, _position).CopyTo (borrowed);
        if (_array.Length != 0)
        {
            ArrayPool<T>.Shared.Return (_array);
        }

        _array = _array = borrowed;
    }

    /// <summary>
    /// Преобразование в массив.
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
    public ReadOnlySpan<T>.Enumerator GetEnumerator()
    {
        return AsSpan().GetEnumerator();
    }

    #endregion

    #region IDisposable members

    /// <summary>
    /// Освобождаем ресурсы, если были заняты.
    /// </summary>
    public void Dispose()
    {
        if (_array.Length != 0)
        {
            _pool.Return (_array);
            _array = Array.Empty<T>();
        }
    }

    #endregion
}
