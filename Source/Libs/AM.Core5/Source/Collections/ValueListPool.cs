// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ValueListPool.cs -- пул объектов List{T}
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// High-performance implementation of IList with zero heap allocations.
/// </summary>
public ref struct ValueListPool<T>
    where T : IEquatable<T>
{
    #region Enums

    /// <summary>
    ///
    /// </summary>
    public enum SourceType
    {
        /// <summary>
        ///
        /// </summary>
        UseAsInitialBuffer,

        /// <summary>
        ///
        /// </summary>
        UseAsReferenceData,

        /// <summary>
        ///
        /// </summary>
        Copy
    }

    #endregion

    #region Properties

    /// <summary>
    /// Емкость списка.
    /// </summary>
    public int Capacity => _buffer.Length;

    /// <summary>
    /// Количество элементов в списке.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Список только для чтения?
    /// </summary>
    public bool IsReadOnly => false;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор с начальной емкостью.
    /// </summary>
    public ValueListPool
        (
            int capacity
        )
    {
        _disposableBuffer = ArrayPool<T>.Shared.Rent (Math.Max (capacity, MinimumCapacity));
        _buffer = _disposableBuffer;
        Count = 0;
    }

    /// <summary>
    ///     Construct the ValueListPool using the giving source.
    ///     It can use the source as initial buffer in order to reuse the array or
    ///     use the data and wrapper it inside the ListPool or copy the data into new pooled array.
    /// </summary>
    /// <param name="source">source/buffer</param>
    /// <param name="sourceType">Action to take with the source</param>
    public ValueListPool
        (
            Span<T> source,
            SourceType sourceType
        )
    {
        if (sourceType == SourceType.UseAsInitialBuffer)
        {
            _buffer = source;
            Count = 0;
            _disposableBuffer = null;
        }
        else if (sourceType == SourceType.UseAsReferenceData)
        {
            _buffer = source;
            Count = source.Length;
            _disposableBuffer = null;
        }
        else
        {
            var disposableBuffer =
                ArrayPool<T>.Shared.Rent (source.Length > MinimumCapacity ? source.Length : MinimumCapacity);

            source.CopyTo (disposableBuffer);
            _buffer = disposableBuffer;
            _disposableBuffer = disposableBuffer;
            Count = source.Length;
        }
    }

    #endregion

    #region Private members

    private const int MinimumCapacity = 16;
    private T[]? _disposableBuffer;
    private Span<T> _buffer;

    /// <summary>
    /// Добавление элемента с сопутствующим расширением буфера.
    /// </summary>
    private void AddWithResize
        (
            T item
        )
    {
        var arrayPool = ArrayPool<T>.Shared;
        if (_disposableBuffer == null)
        {
            var oldBuffer = _buffer;
            var newSize = oldBuffer.Length * 2;
            var newBuffer = arrayPool.Rent (Math.Max (newSize, MinimumCapacity));
            oldBuffer.CopyTo (newBuffer);
            newBuffer[oldBuffer.Length] = item;
            _disposableBuffer = newBuffer;
            _buffer = newBuffer;
            Count++;
        }
        else
        {
            var oldBuffer = _disposableBuffer;
            var newBuffer = arrayPool.Rent (oldBuffer.Length * 2);
            var count = oldBuffer.Length;

            Array.Copy (oldBuffer, 0, newBuffer, 0, count);

            newBuffer[count] = item;
            _disposableBuffer = newBuffer;
            _buffer = newBuffer;
            Count = count + 1;
            arrayPool.Return (oldBuffer);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление элемента.
    /// </summary>
    public void Add
        (
            T item
        )
    {
        var buffer = _buffer;
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

    /// <summary>
    /// Очистка списка.
    /// </summary>
    public void Clear() => Count = 0;

    /// <summary>
    /// Проверка наличия элемента в списке.
    /// </summary>
    public readonly bool Contains
        (
            T item
        )
    {
        return IndexOf (item) >= 0;
    }

    /// <summary>
    /// Поиск первого вхождения элемента в список.
    /// </summary>
    public readonly int IndexOf
        (
            T item
        )
    {
        return _buffer.Slice (0, Count).IndexOf (item);
    }

    /// <summary>
    /// Копирование списка в указанный диапазон памяти.
    /// </summary>
    public readonly void CopyTo
        (
            Span<T> array
        )
    {
        _buffer.Slice (0, Count).CopyTo (array);
    }

    /// <summary>
    /// Удаление элемента из списка.
    /// </summary>
    public bool Remove
        (
            [MaybeNull] T item
        )
    {
        if (((object?) item) is null)
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

    /// <summary>
    /// Вставка элемента в список в указанную позицию.
    /// </summary>
    public void Insert
        (
            int index,
            T item
        )
    {
        var count = Count;
        var buffer = _buffer;

        if (buffer.Length == count)
        {
            var newCapacity = count * 2;
            EnsureCapacity (newCapacity);
            buffer = _buffer;
        }

        if (index < count)
        {
            buffer.Slice (index, count).CopyTo (buffer.Slice (index + 1));
            buffer[index] = item;
            Count++;
        }
        else if (index == count)
        {
            buffer[index] = item;
            Count++;
        }
        else throw new ArgumentOutOfRangeException (nameof (index));
    }

    /// <summary>
    /// Удаление из списка элемента в указанной позиции.
    /// </summary>
    public void RemoveAt
        (
            int index
        )
    {
        var count = Count;
        var buffer = _buffer;

        if (index >= count)
        {
            throw new IndexOutOfRangeException (nameof (index));
        }

        count--;
        buffer.Slice (index + 1).CopyTo (buffer.Slice (index));
        Count = count;
    }

    /// <summary>
    /// Индексатор.
    /// </summary>
    [MaybeNull]
    public readonly ref T this [int index]
    {
        get
        {
            if (index >= Count)
            {
                throw new IndexOutOfRangeException (nameof (index));
            }

            return ref _buffer[index]!;
        }
    }

    /// <summary>
    /// Одновременное добавление нескольких элементов.
    /// </summary>
    public void AddRange
        (
            ReadOnlySpan<T> items
        )
    {
        var count = Count;
        var buffer = _buffer;

        var isCapacityEnough = buffer.Length - items.Length - count >= 0;
        if (!isCapacityEnough)
        {
            EnsureCapacity (buffer.Length + items.Length);
            buffer = _disposableBuffer;
        }

        items.CopyTo (buffer.Slice (count));
        Count += items.Length;
    }

    /// <summary>
    /// Одновременное добавление нескольких элементов.
    /// </summary>
    public void AddRange
        (
            T[] array
        )
    {
        var count = Count;
        var disposableBuffer = _disposableBuffer;
        var buffer = _buffer;

        var isCapacityEnough = buffer.Length - array.Length - count >= 0;
        if (!isCapacityEnough)
        {
            EnsureCapacity (buffer.Length + array.Length);
            disposableBuffer = _disposableBuffer;
            array.CopyTo (disposableBuffer!, count);
            Count += array.Length;
            return;
        }

        if (disposableBuffer != null)
        {
            array.CopyTo (disposableBuffer, count);
        }
        else
        {
            array.AsSpan().CopyTo (buffer.Slice (count));
        }

        Count += array.Length;
    }

    /// <summary>
    /// Доступ к внутреннему массиву как диапазону памяти.
    /// </summary>
    public readonly Span<T> AsSpan()
    {
        return _buffer.Slice (0, Count);
    }

    /// <summary>
    /// Ensures that the capacity of this list is the equal or bigger than the requested capacity.
    /// Indicating the capacity helps to avoid performance degradation produced by auto-growing
    /// </summary>
    /// <param name="capacity">Requested capacity</param>
    public void EnsureCapacity (int capacity)
    {
        if (capacity <= _buffer.Length) return;
        var arrayPool = ArrayPool<T>.Shared;
        var newBuffer = arrayPool.Rent (capacity);
        var oldBuffer = _buffer;

        oldBuffer.CopyTo (newBuffer);

        _buffer = newBuffer;
        if (_disposableBuffer != null)
        {
            arrayPool.Return (_disposableBuffer);
        }

        _disposableBuffer = newBuffer;
    }

    /// <summary>
    /// Получение перечислителя.
    /// </summary>
    public readonly Enumerator GetEnumerator()
    {
        return new Enumerator (_buffer.Slice (0, Count));
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Count = 0;
        var buffer = _disposableBuffer;
        if (buffer != null)
        {
            ArrayPool<T>.Shared.Return (buffer);
        }
    }

    #endregion

    /// <summary>
    /// Перечислитель.
    /// </summary>
    public ref struct Enumerator
    {
        private readonly Span<T> _source;
        private int _index;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Enumerator
            (
                Span<T> source
            )
        {
            _source = source;
            _index = -1;
        }

        /// <summary>
        /// Текущий элемент.
        /// </summary>
        [MaybeNull]
        public readonly ref T Current => ref _source[_index]!;

        /// <summary>
        /// Переход к следующему элементу.
        /// </summary>
        public bool MoveNext()
        {
            return unchecked (++_index < _source.Length);
        }

        /// <summary>
        /// Сброс в начальное состояние.
        /// </summary>
        public void Reset()
        {
            _index = -1;
        }
    }
}
