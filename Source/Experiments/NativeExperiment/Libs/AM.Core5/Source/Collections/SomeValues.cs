// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SomeValues.cs -- содержит 0, 1 или много значений класса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Collections;

//
// Источник вдохновения:
// https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Primitives/src/StringValues.cs
//

/// <summary>
/// Содержит 0, 1 или много экземпляров ссылочного типа.
/// </summary>
public readonly struct SomeValues<T>
    : IList<T>
    where T : class
{
    #region Construction

    /// <summary>
    /// Конструктор: одно значение.
    /// </summary>
    public SomeValues
        (
            T value
        )
        : this()
    {
        _values = value;
    }

    /// <summary>
    /// Конструктор: не одно значение: либо 0 (в т. ч. <c>null</c>), либо много.
    /// </summary>
    public SomeValues
        (
            T[]? values
        )
        : this()
    {
        if (values is not null && values.Length == 1)
        {
            _values = values[0];
        }
        else
        {
            _values = values;
        }
    }

    #endregion

    #region Private members

    private readonly object? _values;

    #endregion

    #region Public methods

    /// <summary>
    /// Выдать значение как единственное.
    /// </summary>
    public T? AsSingle()
    {
        // Take local copy of _values so type checks remain
        // valid even if the StringValues is overwritten in memory
        var value = _values;

        if (value is T result1)
        {
            return result1;
        }

        if (value is null)
        {
            return null;
        }

        // Not T, not null, can only be T[]
        var result2 = Unsafe.As<T[]> (value);

        return result2.Length == 0 ? null : result2[0];
    }

    /// <summary>
    /// Выдать значение как массив.
    /// </summary>
    public T[] AsArray()
    {
        // Take local copy of _values so type checks remain
        // valid even if the SomeValues is overwritten in memory
        var value = _values;

        if (value is T result1)
        {
            return new[] { result1 };
        }

        if (value is null)
        {
            return Array.Empty<T>();
        }

        // Not T, not null, can only be T[]
        return Unsafe.As<T[]> (value);
    }

    /// <summary>
    /// Контейнер пуст?
    /// </summary>
    public bool IsNullOrEmpty()
    {
        // Take local copy of _values so type checks remain
        // valid even if the SomeValues is overwritten in memory
        var value = _values;

        if (value is T)
        {
            return false;
        }

        if (value is null)
        {
            return true;
        }

        // Not T, not null, can only be T[]
        var array = Unsafe.As<T[]> (value);

        return array.Length == 0;
    }

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator SomeValues<T> (T value)
        => new (value);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator SomeValues<T> (T[] values)
        => new (values);

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T? (SomeValues<T> values)
        => values.AsSingle();

    /// <summary>
    /// Оператор неявного преобразования.
    /// </summary>
    public static implicit operator T[] (SomeValues<T> values)
        => values.AsArray();

    #endregion

    #region IList<T> members

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<T> GetEnumerator()
    {
        // Take local copy of _values so type checks remain
        // valid even if the SomeValues is overwritten in memory
        var value = _values;

        if (value is T result1)
        {
            yield return result1;
        }
        else if (value is not null)
        {
            // Not T, not null, can only be T[]
            var array = Unsafe.As<T[]> (value);

            foreach (var one in array)
            {
                yield return one;
            }
        }
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void Add (T item) => throw new NotImplementedException();

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void Clear() => throw new NotImplementedException();

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains (T item)
    {
        // Take local copy of _values so type checks remain
        // valid even if the SomeValues is overwritten in memory
        var value = _values;
        var comparer = EqualityComparer<T>.Default;

        if (value is T one)
        {
            return comparer.Equals (item, one);
        }

        if (value is null)
        {
            return false;
        }

        // Not T, not null, can only be T[]
        var array = Unsafe.As<T[]> (value);
        foreach (var other in array)
        {
            if (comparer.Equals (item, other))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo
        (
            T[] array,
            int arrayIndex
        )
    {
        // Take local copy of _values so type checks remain
        // valid even if the SomeValues is overwritten in memory
        var value = _values;

        if (value is T one)
        {
            array[arrayIndex] = one;
        }
        else if (value is not null)
        {
            // Not T, not null, can only be T[]
            var many = Unsafe.As<T[]> (value);
            Array.Copy (many, 0, array, arrayIndex, many.Length);
        }
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public bool Remove (T item) => throw new NotImplementedException();

    /// <summary>
    /// <inheritdoc cref="ICollection{T}.Count"/>
    /// </summary>
    public int Count
    {
        get
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;

            if (value is T)
            {
                return 1;
            }

            if (value is null)
            {
                return 0;
            }

            // Not T, not null, can only be T[]
            return Unsafe.As<T[]> (value).Length;
        }
    }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool IsReadOnly => true;

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    public int IndexOf
        (
            T item
        )
    {
        // Take local copy of _values so type checks remain
        // valid even if the SomeValues is overwritten in memory
        var value = _values;
        var comparer = EqualityComparer<T>.Default;

        if (value is T one)
        {
            return comparer.Equals (item, one) ? 0 : -1;
        }

        if (value is null)
        {
            return -1;
        }

        // Not T, not null, can only be T[]
        var array = Unsafe.As<T[]> (value);
        for (var index = 0; index < array.Length; index++)
        {
            if (comparer.Equals (item, array[index]))
            {
                return index;
            }
        }

        return -1;
    }

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void Insert (int index, T item) => throw new NotImplementedException();

    /// <summary>
    /// Not implemented.
    /// </summary>
    public void RemoveAt (int index) => throw new NotImplementedException();

    /// <inheritdoc cref="IList{T}.this"/>
    public T this [int index]
    {
        get
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;

            if (value is T value1)
            {
                return index == 0
                    ? value1
                    : throw new IndexOutOfRangeException();
            }

            if (value is null)
            {
                throw new IndexOutOfRangeException();
            }

            // Not T, not null, can only be T[]
            return Unsafe.As<T[]> (value)[index];
        }
        set => throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
        => AsSingle()?.ToString() ?? string.Empty;

    #endregion
}
