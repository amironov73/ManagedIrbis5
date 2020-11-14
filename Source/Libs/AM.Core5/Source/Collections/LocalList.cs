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
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// A fairly simple dynamic list implemented as a value type
    /// to reduce memory allocations.
    /// </summary>
    public struct LocalList<T>
    {
        #region Private members

        private const int InitialCapacity = 4;

        private T[]? _array;
        private int _size;

        private void _Extend(int newSize)
        {
            var newArray = new T[newSize];
            _array?.CopyTo(newArray, 0);

            _array = newArray;
        }

        private void _GrowAsNeeded()
        {
            if (_size >= _array!.Length)
            {
                _Extend(_size * 2);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalList
            (
                int capacity
            )
            : this()
        {
            // Sure.Positive(capacity, nameof(capacity));

            if (capacity > 0)
            {
                _Extend(capacity);
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _size; i++)
            {
                yield return _array![i];
            }
        }

        /// <inheritdoc cref="ICollection{T}.Add" />
        public void Add
            (
                T item
            )
        {
            if (ReferenceEquals(_array, null))
            {
                _Extend(InitialCapacity);
            }

            _GrowAsNeeded();
            _array![_size++] = item;
        }

        /// <summary>
        /// Add some items.
        /// </summary>
        public void AddRange
            (
                IEnumerable<T> items
            )
        {
            if (ReferenceEquals(_array, null))
            {
                _Extend(InitialCapacity);
            }

            foreach (var item in items)
            {
                _GrowAsNeeded();
                _array![_size++] = item;
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
            if (ReferenceEquals(_array, null))
            {
                return false;
            }

            var index = Array.IndexOf(_array, item, 0, _size);
            return index >= 0;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo" />
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            if (!ReferenceEquals(_array, null))
            {
                Array.Copy(_array, 0, array, arrayIndex, _size);
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove" />
        public bool Remove
            (
                T item
            )
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);

                return true;
            }

            return false;
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count => _size;

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        public bool IsReadOnly => false;

        /// <inheritdoc cref="IList{T}.IndexOf" />
        public int IndexOf
            (
                T item
            )
        {
            return ReferenceEquals(_array, null)
                ? -1
                : Array.IndexOf(_array, item, 0, _size);
        }

        /// <inheritdoc cref="IList{T}.Insert" />
        public void Insert
            (
                int index,
                T item
            )
        {
            if (ReferenceEquals(_array, null))
            {
                _Extend(InitialCapacity);
            }

            var array = _array!;

            if (_size != 0 && index != _size - 1)
            {
                Array.Copy
                    (
                        array,
                        index,
                        array,
                        index + 1,
                        _size - index - 1
                    );
            }

            array[index] = item;
            _size++;
        }

        /// <inheritdoc cref="IList{T}.RemoveAt" />
        public void RemoveAt
            (
                int index
            )
        {
            if (!ReferenceEquals(_array, null))
            {
                if (index != _size - 1)
                {
                    Array.Copy(_array, index + 1, _array, index, _size - index - 1);
                }

                _size--;
            }
        }

        /// <inheritdoc cref="IList{T}.this" />
        public T this[int index]
        {
            get
            {
                if (ReferenceEquals(_array, null))
                {
                    throw new IndexOutOfRangeException();
                }

                return _array[index];
            }
            set
            {
                if (ReferenceEquals(_array, null))
                {
                    throw new IndexOutOfRangeException();
                }

                _array[index] = value;
            }
        }

        /// <summary>
        /// Convert the list to array.
        /// </summary>
        public T[] ToArray()
        {
            if (ReferenceEquals(_array, null) || _size == 0)
            {
                return Array.Empty<T>();
            }

            if (_size == _array.Length)
            {
                return _array;
            }

            var result = new T[_size];
            Array.Copy(_array, result, _size);

            return result;
        }

        /// <summary>
        /// Convert the list to <see cref="List{T}"/>.
        /// </summary>
        public List<T> ToList()
        {
            if (ReferenceEquals(_array, null) || _size == 0)
            {
                return new List<T>();
            }

            var result = new List<T>(_size);
            for (var i = 0; i < _size; i++)
            {
                result.Add(_array[i]);
            }

            return result;
        }

        #endregion
    }
}