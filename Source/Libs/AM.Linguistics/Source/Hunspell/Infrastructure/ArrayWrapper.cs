// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ArrayWrapper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public class ArrayWrapper<T>
    : IReadOnlyList<T>
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="items"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected ArrayWrapper (T[] items)
    {
        this._items = items ?? throw new ArgumentNullException (nameof (items));
        IsEmpty = items.Length == 0;
    }

    internal readonly T[] _items;

    /// <summary>
    ///
    /// </summary>
    public bool IsEmpty { get; }

    /// <summary>
    ///
    /// </summary>
    public bool HasItems => !IsEmpty;

    /// <summary>
    ///
    /// </summary>
    /// <param name="index"></param>
    public ref readonly T this [int index] => ref _items[index];

    /// <summary>
    ///
    /// </summary>
    public int Count => _items.Length;

    T IReadOnlyList<T>.this [int index] => _items[index];

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public ReadOnlySpan<T>.Enumerator GetEnumerator()
    {
        return new ReadOnlySpan<T> (_items).GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return ((IEnumerable<T>)_items).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    public class ArrayWrapperComparer<TValue, TCollection> :
        IEqualityComparer<TCollection>
        where TCollection : ArrayWrapper<TValue>
        where TValue : IEquatable<TValue>
    {
        /// <summary>
        ///
        /// </summary>
        public ArrayWrapperComparer()
        {
            arrayComparer = ArrayComparer<TValue>.Default;
        }

        private readonly ArrayComparer<TValue> arrayComparer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals (TCollection? x, TCollection? y)
        {
            if (x == null) return y == null;
            if (y == null) return false;

            return arrayComparer.Equals (x._items, y._items);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode (TCollection? obj)
        {
            return obj == null ? 0 : arrayComparer.GetHashCode (obj._items);
        }
    }
}
