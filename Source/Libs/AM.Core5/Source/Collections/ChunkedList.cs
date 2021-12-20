// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ChunkedList.cs -- фрагментированный динамический список
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Фрагментированный динамический список.
/// </summary>
public class ChunkedList<T>
    : IList<T>
{
    #region Constants

    /// <summary>
    /// Chunk size.
    /// </summary>
    public const int ChunkSize = 512;

    #endregion

    #region Properties

    /// <summary>
    /// Емкость списка.
    /// </summary>
    public int Capacity => _capacity;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ChunkedList()
    {
        _chunks = Array.Empty<T[]>();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChunkedList
        (
            int capacity
        )
        : this()
    {
        Sure.NonNegative (capacity, nameof (capacity));

        EnsureCapacity (capacity);
    }

    #endregion

    #region Private members

    private T[][] _chunks;

    private int _capacity, _count;

    private void EnsureCapacity
        (
            int capacity
        )
    {
        if (_capacity >= capacity)
        {
            return;
        }

        var oldCount = _chunks.Length;

        var newCount = (capacity + ChunkSize - 1) / ChunkSize;
        _capacity = newCount * ChunkSize;
        var newChunks = new T[newCount][];
        for (var i = 0; i < oldCount; i++)
        {
            newChunks[i] = _chunks[i];
        }

        for (var i = oldCount; i < newCount; i++)
        {
            newChunks[i] = new T[ChunkSize];
        }

        _chunks = newChunks;
    }

    #endregion

    #region ICollection<T> members

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable<T>)this).GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<T> GetEnumerator()
    {
        var nchunks = _count / ChunkSize;
        for (var i = 0; i < nchunks; i++)
        {
            var chunk = _chunks[i];
            foreach (var t in chunk)
            {
                yield return t;
            }
        }

        var xtra = _count % ChunkSize;
        if (xtra != 0)
        {
            var chunk = _chunks[nchunks];
            for (var i = 0; i < xtra; i++)
            {
                yield return chunk[i];
            }
        }
    }

    /// <inheritdoc cref="ICollection{T}.Add" />
    public void Add
        (
            T item
        )
    {
        EnsureCapacity (_count + 1);
        _chunks[_count / ChunkSize][_count % ChunkSize] = item;
        _count = _count + 1;
    }

    /// <inheritdoc cref="ICollection{T}.Clear" />
    public void Clear()
    {
        _count = 0;
    }

    /// <inheritdoc cref="ICollection{T}.Contains" />
    public bool Contains
        (
            T item
        )
    {
        return IndexOf (item) >= 0;
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo" />
    public void CopyTo
        (
            T[] array,
            int arrayIndex
        )
    {
        var nchunks = _count / ChunkSize;
        for (var i = 0; i < nchunks; i++)
        {
            Array.Copy (_chunks[i], 0, array, arrayIndex, ChunkSize);
            arrayIndex += ChunkSize;
        }

        var xtra = _count % ChunkSize;
        if (xtra != 0)
        {
            Array.Copy (_chunks[nchunks], 0, array, arrayIndex, xtra);
        }
    }

    /// <inheritdoc cref="ICollection{T}.Remove" />
    public bool Remove
        (
            T item
        )
    {
        var index = IndexOf (item);
        if (index < 0)
        {
            return false;
        }

        RemoveAt (index);

        return true;
    }

    /// <inheritdoc cref="ICollection{T}.Count" />
    public int Count => _count;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
    public bool IsReadOnly => false;

    #endregion

    #region IList<T> members

    /// <inheritdoc cref="IList{T}.IndexOf" />
    public int IndexOf
        (
            T item
        )
    {
        var comparer = EqualityComparer<T>.Default;
        var nchunks = _count / ChunkSize;
        var index = 0;
        for (var i = 0; i < nchunks; i++)
        {
            var chunk = _chunks[i];
            foreach (var t in chunk)
            {
                if (comparer.Equals (item, t))
                {
                    return index;
                }

                index++;
            }
        }

        var xtra = _count % ChunkSize;
        if (xtra != 0)
        {
            var chunk = _chunks[nchunks];
            for (var i = 0; i < xtra; i++)
            {
                if (comparer.Equals (item, chunk[i]))
                {
                    return index;
                }

                index++;
            }
        }

        return -1;
    }

    /// <inheritdoc cref="IList{T}.Insert" />
    public void Insert
        (
            int index,
            T item
        )
    {
        var count = _count + 1;

        //Sure.ValidIndex(index, "index", count);
        EnsureCapacity (count);

        // TODO implement properly

        _count = count;
        for (var i = _count - 1; i > index; i--)
        {
            this[i] = this[i - 1];
        }

        this[index] = item;
    }

    /// <inheritdoc cref="IList{T}.RemoveAt" />
    public void RemoveAt
        (
            int index
        )
    {
        //Sure.ValidIndex(index, "index", _count);

        // TODO implement properly
        var count = _count - 1;
        for (var i = index; i < count; i++)
        {
            this[i] = this[i + 1];
        }

        _count = count;
    }

    /// <inheritdoc cref="IList{T}.this" />
    public T this [int index]
    {
        get
        {
            //Sure.ValidIndex(index, "index", _count);

            return _chunks[index / ChunkSize][index % ChunkSize];
        }
        set
        {
            //Sure.ValidIndex(index, "index", _count);

            _chunks[index / ChunkSize][index % ChunkSize] = value;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Преобразование в массив.
    /// </summary>
    public T[] ToArray()
    {
        if (_count == 0)
        {
            return Array.Empty<T>();
        }

        var result = new T[_count];
        CopyTo (result, 0);

        return result;
    }

    #endregion
}
