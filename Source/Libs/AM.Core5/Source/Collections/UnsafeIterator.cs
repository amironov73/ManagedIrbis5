// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* UnsafeIterator.cs -- итератор по опасной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Итератор по опасной памяти.
    /// </summary>
    public unsafe struct UnsafeIterator<T>
        : IIterator<T>
        where T: unmanaged
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeIterator (T* pointer) => _pointer = pointer;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeIterator (IntPtr pointer) => _pointer = (T*)pointer.ToPointer();

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeIterator (UIntPtr pointer) => _pointer = (T*)pointer.ToPointer();

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeIterator(ReadOnlySpan<T> span)
        {
            fixed (T* temporary = span)
            {
                _pointer = temporary;
            }
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeIterator(T[] array)
        {
            fixed (T* temporary = array)
            {
                _pointer = temporary;
            }
        } // constructor

        #endregion

        #region Private members

        private T* _pointer;

        #endregion

        #region IIterator members

        /// <inheritdoc cref="IComparable{T}.CompareTo"/>
        public int CompareTo(IIterator<T>? other) =>
            other is UnsafeIterator<T> unsafeIterator
                ? unchecked((int)(_pointer - unsafeIterator._pointer))
                : throw new ArgumentException();

        /// <inheritdoc cref="IIterator{T}.Value"/>
        public ref T Value => ref *_pointer;

        /// <inheritdoc cref="IIterator{T}.Advance"/>
        public void Advance(int delta = 1) => _pointer += delta;

        #endregion

    } // struct UnsafeIterator

} // namespace AM.Collections
