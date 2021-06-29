// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedType.Global

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
        : IIterator<T>,
        IEquatable<UnsafeIterator<T>>
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

        #region Public methods

        /// <summary>
        /// Оператор инкремента.
        /// </summary>
        public static UnsafeIterator<T> operator ++(UnsafeIterator<T> iterator) =>
            new (iterator._pointer + 1);

        /// <summary>
        /// Оператор декремента.
        /// </summary>
        public static UnsafeIterator<T> operator --(UnsafeIterator<T> iterator) =>
            new (iterator._pointer - 1);

        /// <summary>
        /// Оператор сложения с целым числом.
        /// </summary>
        public static UnsafeIterator<T> operator + (UnsafeIterator<T> left, int right) =>
            new (left._pointer + right);

        /// <summary>
        /// Оператор вычитания целого числа.
        /// </summary>
        public static UnsafeIterator<T> operator - (UnsafeIterator<T> left, int right) =>
            new (left._pointer - right);

        /// <summary>
        /// Вычисление разности между двумя итераторами.
        /// </summary>
        public static int operator - (UnsafeIterator<T> left, UnsafeIterator<T> right) =>
            unchecked((int)(left._pointer - right._pointer));

        /// <summary>
        /// Сравнение двух итераторов.
        /// </summary>
        public static bool operator < (UnsafeIterator<T> left, UnsafeIterator<T> right) =>
            left._pointer < right._pointer;

        /// <summary>
        /// Сравнение двух итераторов.
        /// </summary>
        public static bool operator <= (UnsafeIterator<T> left, UnsafeIterator<T> right) =>
            left._pointer <= right._pointer;

        /// <summary>
        /// Сравнение двух итераторов.
        /// </summary>
        public static bool operator > (UnsafeIterator<T> left, UnsafeIterator<T> right) =>
            left._pointer > right._pointer;

        /// <summary>
        /// Сравнение двух итераторов.
        /// </summary>
        public static bool operator >= (UnsafeIterator<T> left, UnsafeIterator<T> right) =>
            left._pointer >= right._pointer;

        /// <summary>
        /// Сравнение двух итераторов.
        /// </summary>
        public static bool operator == (UnsafeIterator<T> left, UnsafeIterator<T> right) =>
            left._pointer == right._pointer;

        /// <summary>
        /// Сравнение двух итераторов.
        /// </summary>
        public static bool operator != (UnsafeIterator<T> left, UnsafeIterator<T> right) =>
            left._pointer != right._pointer;

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

        #region IEquatable members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(UnsafeIterator<T> other) =>
            _pointer == other._pointer;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object? obj) =>
            obj is UnsafeIterator<T> other && Equals(other);

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() => (int)_pointer;

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => ((int)_pointer).ToString("x8");

        #endregion

    } // struct UnsafeIterator

} // namespace AM.Collections
