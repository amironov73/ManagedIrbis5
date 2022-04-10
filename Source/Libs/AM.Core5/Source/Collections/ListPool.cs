// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseNullableAnnotationInsteadOfAttribute

/* ListPool.cs -- пул объектов List{T}
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

#endregion

#nullable enable

// Компилятор жалуется на nullability
#pragma warning disable CS8766
#pragma warning disable CS8768
#pragma warning disable CS8769

namespace AM.Collections;

/// <summary>
/// Overhead free implementation of IList using ArrayPool.
/// With overhead being the class itself regardless the size of the underlying array.
/// </summary>
[Serializable]
public sealed class ListPool<T>
    : IList<T>, IList, IReadOnlyList<T>, IDisposable
{
    #region Properties

    /// <summary>
    /// Емкость списка.
    /// </summary>
    public int Capacity => _items.Length;

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count { get; private set; }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool IsReadOnly => false;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ListPool()
    {
        _items = _arrayPool.Rent (MinimumCapacity);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="capacity">Начальная емкость.</param>
    public ListPool
        (
            int capacity
        )
    {
        _items = _arrayPool.Rent (Math.Max (capacity, MinimumCapacity));
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="source">Начальное наполнение.</param>
    public ListPool
         (
             IEnumerable<T> source
         )
    {
        Sure.NotNull ((object?) source);

        if (source is ICollection<T> collection)
        {
            Count = collection.Count;
            var buffer = _arrayPool.Rent (Math.Max (Count, MinimumCapacity));
            collection.CopyTo (buffer, 0);
            _items = buffer;
        }
        else
        {
            _items = _arrayPool.Rent (MinimumCapacity);
            var buffer = _items;
            Count = 0;
            var count = 0;
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (count < buffer.Length)
                {
                    buffer[count] = enumerator.Current;
                    count++;
                }
                else
                {
                    Count = count;
                    AddWithResize (enumerator.Current);
                    count++;
                    buffer = _items;
                }
            }

            Count = count;
        }
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="source">Начальное наполнение.</param>
    public ListPool
        (
            T[] source
        )
    {
        Sure.NotNull (source);

        var capacity = Math.Max (source.Length, MinimumCapacity);
        var buffer = _arrayPool.Rent (capacity);
        source.CopyTo (buffer, 0);
        _items = buffer;
        Count = source.Length;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="source">Начальное наполнение.</param>
    public ListPool
        (
            ReadOnlySpan<T> source
        )
    {
        var capacity = Math.Max (source.Length, MinimumCapacity);
        var buffer = _arrayPool.Rent (capacity);
        source.CopyTo (buffer);
        _items = buffer;
        Count = source.Length;
    }

    #endregion

    #region Private members

    private const int MinimumCapacity = 32;
    private T[] _items;

    [NonSerialized]
    private object? _syncRoot;

    private readonly ArrayPool<T> _arrayPool = ArrayPool<T>.Shared;

    /// <summary>
    /// Добавление элемента, совмещенное с увеличением емкости.
    /// </summary>
    private void AddWithResize
        (
            T item
        )
    {
        var arrayPool = _arrayPool;
        var oldBuffer = _items;
        var newBuffer = arrayPool.Rent (oldBuffer.Length * 2);
        var count = oldBuffer.Length;

        Array.Copy (oldBuffer, 0, newBuffer, 0, count);

        newBuffer[count] = item;
        _items = newBuffer;
        Count = count + 1;
        arrayPool.Return (oldBuffer);
    }

    #endregion

    #region Public methods

    /// <inheritdoc cref="ICollection{T}.Add"/>
    public void Add
        (
            T item
        )
    {
        var buffer = _items;
        var count = Count;

        if (count < buffer.Length)
        {
            buffer[count] = item;
            Count = count + 1;
        }
        else
        {
            AddWithResize (item);
        }
    }

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public void Clear()
    {
        Count = 0;
    }

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains
        (
            T item
        )
    {
        return IndexOf (item) > -1;
    }

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    public int IndexOf
        (
            T item
        )
    {
        return Array.IndexOf (_items, item, 0, Count);
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo
        (
            T[] array,
            int arrayIndex
        )
    {
        Array.Copy (_items, 0, array, arrayIndex, Count);
    }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove
        (
            T item
        )
    {
        if (item is null)
        {
            return false;
        }

        var index = IndexOf (item);
        if (index == -1)
        {
            return false;
        }

        RemoveAt (index);

        return true;
    }

    /// <inheritdoc cref="IList{T}.Insert"/>
    public void Insert
        (
            int index,
            T item
        )
    {
        var count = Count;
        var buffer = _items;

        if (buffer.Length == count)
        {
            var newCapacity = count * 2;
            EnsureCapacity (newCapacity);
            buffer = _items;
        }

        if (index < count)
        {
            Array.Copy (buffer, index, buffer, index + 1, count - index);
            buffer[index] = item;
            Count++;
        }
        else if (index == count)
        {
            buffer[index] = item;
            Count++;
        }
        else
        {
            throw new IndexOutOfRangeException (nameof (index));
        }
    }

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    public void RemoveAt
        (
            int index
        )
    {
        var count = Count;
        var buffer = _items;

        if (index >= count)
        {
            throw new IndexOutOfRangeException (nameof (index));
        }

        count--;
        Array.Copy (buffer, index + 1, buffer, index, count - index);
        Count = count;
    }

    /// <summary>
    /// Индексатор.
    /// </summary>
    [MaybeNull]
    public T this [int index]
    {
        get
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException (nameof (index));
            }

            return _items[index];
        }
        set
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException (nameof (index));
            }

            _items[index] = value;
        }
    }

    /// <summary>
    /// Добавление нескольких элементов одновременно.
    /// </summary>
    public void AddRange
        (
            Span<T> items
        )
    {
        var count = Count;
        var buffer = _items;

        var isCapacityEnough = buffer.Length - items.Length - count >= 0;
        if (!isCapacityEnough)
        {
            EnsureCapacity (buffer.Length + items.Length);
            buffer = _items;
        }

        items.CopyTo (buffer.AsSpan().Slice (count));
        Count += items.Length;
    }

    /// <summary>
    /// Добавление нескольких элементов одновременно.
    /// </summary>
    public void AddRange
        (
            ReadOnlySpan<T> items
        )
    {
        var count = Count;
        var buffer = _items;

        var isCapacityEnough = buffer.Length - items.Length - count >= 0;
        if (!isCapacityEnough)
        {
            EnsureCapacity (buffer.Length + items.Length);
            buffer = _items;
        }

        items.CopyTo (buffer.AsSpan().Slice (count));
        Count += items.Length;
    }

    /// <summary>
    /// Добавление нескольких элементов одновременно.
    /// </summary>
    public void AddRange
        (
            T[] items
        )
    {
        Sure.NotNull (items);

        var count = Count;
        var buffer = _items;

        var isCapacityEnough = buffer.Length - items.Length - count >= 0;
        if (!isCapacityEnough)
        {
            EnsureCapacity (buffer.Length + items.Length);
            buffer = _items;
        }

        Array.Copy (items, 0, buffer, count, items.Length);
        Count += items.Length;
    }

    /// <summary>
    /// Добавление нескольких элементов одновременно.
    /// </summary>
    public void AddRange
        (
            IEnumerable<T> items
        )
    {
        Sure.NotNull ((object?) items);

        var count = Count;
        var buffer = _items;

        if (items is ICollection<T> collection)
        {
            var isCapacityEnough = buffer.Length - collection.Count - count >= 0;
            if (!isCapacityEnough)
            {
                EnsureCapacity (buffer.Length + collection.Count);
                buffer = _items;
            }

            collection.CopyTo (buffer, count);
            Count += collection.Count;
        }
        else
        {
            foreach (var item in items)
            {
                if (count < buffer.Length)
                {
                    buffer[count] = item;
                    count++;
                }
                else
                {
                    Count = count;
                    AddWithResize (item);
                    count++;
                    buffer = _items;
                }
            }

            Count = count;
        }
    }

    /// <summary>
    ///     Get span of the items added
    /// </summary>
    /// <returns></returns>
    public Span<T> AsSpan() => _items.AsSpan (0, Count);

    /// <summary>
    ///     Get memory of the items added
    /// </summary>
    /// <returns></returns>
    public Memory<T> AsMemory() => _items.AsMemory (0, Count);

    /// <summary>
    /// Ensures that the capacity of this list is the equal or bigger than the requested capacity.
    /// Indicating the capacity helps to avoid performance degradation produced by auto-growing
    /// </summary>
    /// <param name="capacity">Requested capacity</param>
    public void EnsureCapacity
        (
            int capacity
        )
    {
        if (capacity <= Capacity) return;
        var arrayPool = _arrayPool;
        var newBuffer = arrayPool.Rent (capacity);
        var oldBuffer = _items;

        Array.Copy (oldBuffer, 0, newBuffer, 0, oldBuffer.Length);

        _items = newBuffer;
        arrayPool.Return (oldBuffer);
    }

    /// <summary>
    /// Returns internal buffer. Use when an array is required, and do not hold the reference.
    /// When ListPool grows or is disposed it returns the buffer to the pool.
    /// After updating the internal buffer manually you need update the offset using the method SetOffsetManually(int offset)
    /// </summary>
    public T[] GetRawBuffer()
    {
        return _items;
    }

    /// <summary>
    /// Update the ListPool internal offset, use when you update manually the raw buffer to add new items
    /// or if you want to shrink the content.
    /// </summary>
    public void SetOffsetManually (int offset)
    {
        Count = offset;
    }

    #endregion

    #region ICollection members

    /// <inheritdoc cref="ICollection.Count"/>
    int ICollection.Count => Count;

    /// <inheritdoc cref="ICollection.IsSynchronized"/>
    bool ICollection.IsSynchronized => false;

    /// <inheritdoc cref="ICollection.SyncRoot"/>
    object ICollection.SyncRoot
    {
        get
        {
            if (_syncRoot is null)
            {
                Interlocked.CompareExchange<object> (ref _syncRoot!, new object(), null!);
            }

            return _syncRoot;
        }
    }

    /// <inheritdoc cref="ICollection.CopyTo"/>
    void ICollection.CopyTo
        (
            Array array,
            int arrayIndex
        )
    {
        Array.Copy (_items, 0, array, arrayIndex, Count);
    }

    #endregion

    #region IList members

    /// <inheritdoc cref="IList.IsFixedSize"/>
    bool IList.IsFixedSize => false;

    /// <inheritdoc cref="IList.IsReadOnly"/>
    bool IList.IsReadOnly => false;

    /// <inheritdoc cref="IList.Add"/>
    int IList.Add
        (
            object? item
        )
    {
        if (item is T itemAsTSource)
        {
            Add (itemAsTSource);
        }
        else
        {
            throw new ArgumentException
                (
                    $"Wrong value type. Expected {typeof (T)}, got: '{item}'.",
                    nameof (item)
                );
        }

        return Count - 1;
    }

    /// <inheritdoc cref="IList.Contains"/>
    bool IList.Contains
        (
            object? item
        )
    {
        if (item is T itemAsTSource)
        {
            return Contains (itemAsTSource);
        }

        throw new ArgumentException
            (
                $"Wrong value type. Expected {typeof (T)}, got: '{item}'.",
                nameof (item)
            );
    }

    /// <inheritdoc cref="IList.IndexOf"/>
    int IList.IndexOf
        (
            object? item
        )
    {
        if (item is T itemAsTSource)
        {
            return IndexOf (itemAsTSource);
        }

        throw new ArgumentException
            (
                $"Wrong value type. Expected {typeof (T)}, got: '{item}'.",
                nameof (item)
            );
    }

    /// <inheritdoc cref="IList.Remove"/>
    void IList.Remove
        (
            object? item
        )
    {
        if (item is T itemAsTSource)
        {
            Remove (itemAsTSource);
        }
        else if (item is not null)
        {
            throw new ArgumentException
                (
                    $"Wrong value type. Expected {typeof (T)}, got: '{item}'.",
                    nameof (item)
                );
        }
    }

    /// <inheritdoc cref="IList.Insert"/>
    void IList.Insert
        (
            int index,
            object? item
        )
    {
        if (item is T itemAsTSource)
        {
            Insert (index, itemAsTSource);
        }
        else if (item is not null)
        {
            throw new ArgumentException
                (
                    $"Wrong value type. Expected {typeof (T)}, got: '{item}'.",
                    nameof (item)
                );
        }
    }

    /// <inheritdoc cref="IList.this"/>
    [MaybeNull]
    object IList.this [int index]
    {
        get
        {
            if (index >= Count)
                throw new IndexOutOfRangeException (nameof (index));

            return _items[index];
        }
        set
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException (nameof (index));
            }

            if (value is T valueAsTSource)
            {
                _items[index] = valueAsTSource;
            }
            else
            {
                throw new ArgumentException
                    (
                        $"Wrong value type. Expected {typeof (T)}, got: '{value}'.",
                        nameof (value)
                    );
            }
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Count = 0;
        _arrayPool.Return (_items);
    }

    #endregion

    #region IEnumerable members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator (_items, Count);

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() => new Enumerator (_items, Count);

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    public Enumerator GetEnumerator() => new (_items, Count);

    #endregion

    #region Nested classes

    /// <summary>
    /// Перечислитель
    /// </summary>
    public struct Enumerator
        : IEnumerator<T>
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Enumerator
            (
                T[] source,
                int itemsCount
            )
        {
            _source = source;
            _itemsCount = itemsCount;
            _index = -1;
        }

        #endregion

        #region Private members

        private readonly T[] _source;
        private readonly int _itemsCount;
        private int _index;

        #endregion

        /// <summary>
        /// Ссылка на текущий элемент.
        /// </summary>
        [MaybeNull]
        public readonly ref T Current => ref _source[_index]!;

        /// <inheritdoc cref="IEnumerator{T}.Current"/>
        [MaybeNull]
        readonly T IEnumerator<T>.Current => _source[_index];

        /// <summary>
        /// Текущий элемент.
        /// </summary>
        [MaybeNull]
        readonly object IEnumerator.Current => _source[_index];

        /// <inheritdoc cref="IEnumerator.MoveNext"/>
        public bool MoveNext()
        {
            return unchecked (++_index < _itemsCount);
        }

        /// <inheritdoc cref="IEnumerator.Reset"/>
        public void Reset()
        {
            _index = -1;
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public readonly void Dispose()
        {
        }

        #endregion
    }
}
