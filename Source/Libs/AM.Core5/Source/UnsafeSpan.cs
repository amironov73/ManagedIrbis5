// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Unsafe.cs -- опасный спан
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Опасный спан.
    /// </summary>
    public readonly unsafe struct UnsafeSpan<T>
        where T: unmanaged
    {
        #region Properties

        /// <summary>
        /// Указатель.
        /// </summary>
        public T* Pointer { get; }

        /// <summary>
        /// Количество элементов.
        /// </summary>
        public int Length { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeSpan
            (
                T* pointer,
                int length = 1
            )
        {
            Pointer = pointer;
            Length = length;

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeSpan
            (
                IntPtr pointer,
                int length = 1
            )
        {
            Pointer = (T*) pointer.ToPointer();
            Length = length;

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        public UnsafeSpan
            (
                Span<T> span
            )
        {
            ref T reference = ref span.GetPinnableReference();
            Pointer = (T*) Unsafe.AsPointer(ref reference);
            Length = span.Length;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Индексатор.
        /// </summary>
        public ref T this [int index] => ref Unsafe.AsRef<T>(Unsafe.Add<T>(Pointer, index));

        #endregion

        #region Operators

        /// <summary>
        /// Неявное преобразование в опасный спан.
        /// </summary>
        public static implicit operator UnsafeSpan<T>(Span<T> span) => new (span);

        /// <summary>
        /// Неявное преобразование в опасный спан.
        /// </summary>
        public static implicit operator UnsafeSpan<T>(Memory<T> memory) => new (memory.Span);

        /// <summary>
        /// Неявное преобразование в обычный спан.
        /// </summary>
        public static implicit operator Span<T>(UnsafeSpan<T> span) => new (span.Pointer, span.Length);

        #endregion

    } // struct UnsafeSpan

} // namespace AM
