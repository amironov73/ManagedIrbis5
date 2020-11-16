// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* NonNullCollection.cs -- коллекция, не допускающая нулевых элементов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// <see cref="Collection{T}"/>, не допускающая нулевых элементов.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{" + nameof(Count) + "}")]
    public class NonNullCollection<T>
        : Collection<T>
        where T: class
    {
        #region Properties

        /// <summary>
        /// Емкость.
        /// </summary>
        public int Capacity => _GetInnerList().Capacity;

        #endregion

        #region Private members

        private List<T> _GetInnerList()
        {
            // ReSharper disable SuspiciousTypeConversion.Global
            var result = (List<T>)Items;
            // ReSharper restore SuspiciousTypeConversion.Global

            return result;
        }

        #endregion

        #region Public members

        /// <summary>
        /// Add capacity to eliminate reallocations.
        /// </summary>
        public void AddCapacity
            (
                int delta
            )
        {
            var innerList = _GetInnerList();
            var newCapacity = innerList.Count + delta;
            if (newCapacity > innerList.Capacity)
            {
                innerList.Capacity = newCapacity;
            }
        }

        /// <summary>
        /// Add several elements to the collection.
        /// </summary>
        public NonNullCollection<T> AddRange
            (
                IEnumerable<T> range
            )
        {
            Sure.NotNull(range, nameof(range));

            foreach (var item in range)
            {
                Add(item);
            }

            return this;
        }

        /// <summary>
        /// Add several elements to the collection.
        /// </summary>
        public NonNullCollection<T> AddRange
            (
                T[] array
            )
        {
            Sure.NotNull(array, nameof(array));

            AddCapacity(array.Length);
            foreach (var item in array)
            {
                Add(item);
            }

            return this;
        }

        /// <summary>
        /// Add several elements to the collection.
        /// </summary>
        public NonNullCollection<T> AddRange
            (
                IList<T> list
            )
        {
            Sure.NotNull(list, nameof(list));

            AddCapacity(list.Count);
            foreach (var item in list)
            {
                Add(item);
            }

            return this;
        }

        /// <summary>
        /// Ensure the capacity.
        /// </summary>
        public void EnsureCapacity
            (
                int capacity
            )
        {
            var innerList = _GetInnerList();
            if (innerList.Capacity < capacity)
            {
                innerList.Capacity = capacity;
            }
        }

        /// <summary>
        /// Converts the collection to <see cref="Array"/> of elements
        /// of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Array of items of type <typeparamref name="T"/>.
        /// </returns>
        public T[] ToArray() => _GetInnerList().ToArray();

        #endregion

        #region Collection<T> members

        /// <inheritdoc cref="Collection{T}.InsertItem" />
        protected override void InsertItem
            (
                int index,
                T item
            )
        {
            Sure.NotNull(item, nameof(item));

            base.InsertItem(index, item);
        }

        /// <inheritdoc cref="Collection{T}.SetItem" />
        protected override void SetItem
            (
                int index,
                T item
            )
        {
            Sure.NotNull(item, nameof(item));

            base.SetItem(index, item);
        }

        #endregion

    }
}
