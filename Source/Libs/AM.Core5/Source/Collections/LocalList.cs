// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* LocalList.cs -- простой динамический список
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
    /// A fairly simple dynamic list implemented as a value type
    /// to reduce memory allocations.
    /// </summary>
    public ref struct LocalList<T>
        where T : IEquatable<T>
    {
        #region Private members

        private T[]? _arrayFromPool;
        private Span<T> _array;
        private int _size;

        private void _Extend(int newSize)
        {
            var newArray = ArrayPool<T>.Shared.Rent(newSize);
            _array.CopyTo(newArray);
            if (_arrayFromPool is not null)
            {
                ArrayPool<T>.Shared.Return(_arrayFromPool);
            }

            _arrayFromPool = newArray;
            _array = newArray;
        }

        private void _GrowAsNeeded()
        {
            if (_size >= _array.Length)
            {
                var newSize = _size * 2;
                if (newSize < 4)
                {
                    newSize = 4;
                }
                _Extend(newSize);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalList
            (
                Span<T> initialSpan
            )
            : this()
        {
            //_arrayFromPool = null;
            //_size = 0;
            _array = initialSpan;
        }

        #endregion

        #region Public methods

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public Span<T>.Enumerator GetEnumerator()
        {
            return _array.Slice(0, _size).GetEnumerator();
        }

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                T item
            )
        {
            _GrowAsNeeded();
            _array[_size++] = item;
        }

        /// <summary>
        /// Add some items.
        /// </summary>
        public void AddRange
            (
                IEnumerable<T> items
            )
        {
            foreach (var item in items)
            {
                _GrowAsNeeded();
                _array[_size++] = item;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Clear" />
        public void Clear()
        {
            _size = 0;
        }

        /// <inheritdoc cref="ICollection{T}.Contains" />
        public bool Contains
            (
                T item
            )
        {
            return _array.Slice(0, _size).Contains(item);
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            if (_size != 0)
            {
                _array.Slice(0, _size).CopyTo(array.AsSpan().Slice(arrayIndex));
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove
            (
                T item
            )
        {
            var result = false;

            while (true)
            {
                var index = IndexOf(item);
                if (index < 0)
                {
                    break;
                }

                RemoveAt(index);
                result = true;
            }

            return result;
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count => _size;

        /// <summary>
        /// Емкость.
        /// </summary>
        public int Capacity => _array.Length;

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly => false;

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf
            (
                T item
            )
        {
            return _array.Slice(0, _size).IndexOf(item);
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert
            (
                int index,
                T item
            )
        {
            _GrowAsNeeded();
            if (index != _size)
            {
                for (int i = _size - 1; i != index; --i)
                {
                    _array[i + 1] = _array[i];
                }

                _array[index + 1] = _array[index];
            }

            _array[index] = item;
            ++_size;
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt
            (
                int index
            )
        {
            if (index != _size - 1)
            {
                _array.Slice(index + 1, _size - index - 1)
                    .CopyTo(_array.Slice(index, _size - index));
            }

            --_size;
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get => _array.Slice(0, _size)[index];
            set => _array.Slice(0, _size)[index] = value;
        }

        /// <summary>
        /// Convert the list to array.
        /// </summary>
        public T[] ToArray() => _array.Slice(0, _size).ToArray();

        /// <summary>
        /// Convert the list to <see cref="List{T}"/>.
        /// </summary>
        public List<T> ToList()
        {
            var result = new List<T>(_size);
            for (var i = 0; i < _size; i++)
            {
                result.Add(_array[i]);
            }

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_arrayFromPool is not null)
            {
                ArrayPool<T>.Shared.Return(_arrayFromPool);
                _arrayFromPool = null;
            }

            _array = Span<T>.Empty;
        }

        #endregion
    }
}