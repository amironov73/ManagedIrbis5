// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode

/* ArrayIterator.cs -- итератор по массиву
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Итератор по массиву.
/// </summary>
public struct ArrayIterator<T>
    : IIterator<T>,
    IEquatable<ArrayIterator<T>>
    where T : unmanaged
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ArrayIterator
        (
            T[] array,
            int index = 0
        )
    {
        Sure.NotNull (array);

        _array = array;
        _index = index;
    }

    #endregion

    #region Private members

    private readonly T[] _array;
    private int _index;

    #endregion

    #region Public methods

    /// <summary>
    /// Оператор инкремента.
    /// </summary>
    public static ArrayIterator<T> operator ++
        (
            ArrayIterator<T> iterator
        )
    {
        return new (iterator._array, iterator._index + 1);
    }

    /// <summary>
    /// Оператор декремента.
    /// </summary>
    public static ArrayIterator<T> operator --
        (
            ArrayIterator<T> iterator
        )
    {
        return new (iterator._array, iterator._index - 1);
    }

    /// <summary>
    /// Оператор сложения с целым числом.
    /// </summary>
    public static ArrayIterator<T> operator +
        (
            ArrayIterator<T> left,
            int right
        )
    {
        return new (left._array, left._index + right);
    }

    /// <summary>
    /// Оператор вычитания целого числа.
    /// </summary>
    public static ArrayIterator<T> operator -
        (
            ArrayIterator<T> left,
            int right
        )
    {
        return new (left._array, left._index - right);
    }

    /// <summary>
    /// Вычисление разности между двумя итераторами.
    /// </summary>
    public static int operator -
        (
            ArrayIterator<T> left,
            ArrayIterator<T> right
        )
    {
        return left._index - right._index;
    }

    /// <summary>
    /// Сравнение двух итераторов.
    /// </summary>
    public static bool operator <
        (
            ArrayIterator<T> left,
            ArrayIterator<T> right
        )
    {
        return left._index < right._index;
    }

    /// <summary>
    /// Сравнение двух итераторов.
    /// </summary>
    public static bool operator <=
        (
            ArrayIterator<T> left,
            ArrayIterator<T> right
        )
    {
        return left._index <= right._index;
    }

    /// <summary>
    /// Сравнение двух итераторов.
    /// </summary>
    public static bool operator >
        (
            ArrayIterator<T> left,
            ArrayIterator<T> right
        )
    {
        return left._index > right._index;
    }

    /// <summary>
    /// Сравнение двух итераторов.
    /// </summary>
    public static bool operator >=
        (
            ArrayIterator<T> left,
            ArrayIterator<T> right
        )
    {
        return left._index >= right._index;
    }

    /// <summary>
    /// Сравнение двух итераторов.
    /// </summary>
    public static bool operator ==
        (
            ArrayIterator<T> left,
            ArrayIterator<T> right
        )
    {
        return left._index == right._index;
    }

    /// <summary>
    /// Сравнение двух итераторов.
    /// </summary>
    public static bool operator !=
        (
            ArrayIterator<T> left,
            ArrayIterator<T> right
        )
    {
        return left._index != right._index;
    }

    #endregion

    #region IIterator members

    /// <inheritdoc cref="IComparable{T}.CompareTo"/>
    public int CompareTo
        (
            IIterator<T>? other
        )
    {
        return other is ArrayIterator<T> array
            ? _index - array._index
            : throw new ArgumentException();
    }

    /// <inheritdoc cref="IIterator{T}.Value"/>
    public ref T Value => ref _array[_index];

    /// <inheritdoc cref="IIterator{T}.Advance"/>
    public void Advance
        (
            int delta = 1
        )
    {
        _index += delta;
    }

    #endregion

    #region IEquatable members

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals
        (
            ArrayIterator<T> other
        )
    {
        return _array.Equals (other._array) && _index == other._index;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        return obj is ArrayIterator<T> other && Equals (other);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return HashCode.Combine (_array, _index);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return _index.ToInvariantString();
    }

    #endregion
}
