// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ArrayIterator.cs -- итератор по массиву
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Итератор по массиву.
    /// </summary>
    public struct ArrayIterator<T>
        : IIterator<T>
        where T: unmanaged
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
            _array = array;
            _index = index;
        }

        #endregion

        #region Private members

        private readonly T[] _array;
        private int _index;

        #endregion

        #region Public methods


        #endregion

        #region IIterator members

        /// <inheritdoc cref="IComparable{T}.CompareTo"/>
        public int CompareTo(IIterator<T>? other) =>
            other is ArrayIterator<T> array
                ? _index - array._index
                : throw new ArgumentException();

        /// <inheritdoc cref="IIterator{T}.Value"/>
        public ref T Value => ref _array[_index];

        /// <inheritdoc cref="IIterator{T}.Advance"/>
        public void Advance(int delta = 1) => _index += delta;

        #endregion

    } // struct ArrayIterator

} // namespace AM.Collections
