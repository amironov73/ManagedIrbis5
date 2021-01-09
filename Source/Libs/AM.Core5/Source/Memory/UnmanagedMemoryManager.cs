// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UnmanagedMemoryManager.cs -- простой менеджер для неуправляемой памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Memory
{
    /// <summary>
    /// Простой менеджер для неуправляемой памяти.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed unsafe class UnmanagedMemoryManager<T>
        : MemoryManager<T>
        where T: unmanaged
    {
        #region Private members

        private readonly T* _pointer;
        private readonly int _length;
        private readonly IntPtr _handle;

        #endregion

        #region Construction

        /// <summary>
        /// Create a new UnmanagedMemoryManager instance at the given
        /// pointer and size.
        /// </summary>
        ///
        /// <remarks>It is assumed that the span provided is already
        /// unmanaged or externally pinned.</remarks>
        public UnmanagedMemoryManager
            (
                Span<T> span
            )
        {
            fixed (T* ptr = &MemoryMarshal.GetReference(span))
            {
                _pointer = ptr;
                _length = span.Length;
            }

            _handle = IntPtr.Zero;
        }
        /// <summary>
        /// Create a new UnmanagedMemoryManager instance
        /// at the given pointer and size.
        /// </summary>
        public UnmanagedMemoryManager
            (
                T* pointer,
                int length
            )
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            _pointer = pointer;
            _length = length;
            _handle = IntPtr.Zero;
        }

        /// <summary>
        /// Создает менеджер для указанного дескриптора неуправвляемой
        /// памяти, полученного от <c>Marshal.AllocHGlobal</c>.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="length"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public UnmanagedMemoryManager
            (
                IntPtr handle,
                int length
            )
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            _handle = handle;
            _pointer = (T*) _handle.ToPointer();
            _length = length;
            Unsafe.InitBlock(_pointer, 0, unchecked((uint)_length));
        }

        /// <summary>
        /// Создает менеджер для неуправляемой памяти, полученной
        /// от <c>Marshal.AllocHGlobal.</c>
        /// </summary>
        /// <param name="length"></param>
        public UnmanagedMemoryManager
            (
                int length
            )
            : this(Marshal.AllocHGlobal(length), length)
        {
        }

        #endregion

        #region MemoryManager<T> members

        /// <summary>
        /// Obtains a span that represents the region
        /// </summary>
        public override Span<T> GetSpan() => new (_pointer, _length);

        /// <summary>
        /// Provides access to a pointer that represents the data (note: no actual pin occurs)
        /// </summary>
        public override MemoryHandle Pin
            (
                int elementIndex = 0
            )
        {
            if (elementIndex < 0 || elementIndex >= _length)
            {
                throw new ArgumentOutOfRangeException(nameof(elementIndex));
            }

            return new MemoryHandle(_pointer + elementIndex);
        }
        /// <summary>
        /// Has no effect
        /// </summary>
        public override void Unpin() { }

        /// <summary>
        /// Releases all resources associated with this object
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_handle != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_handle);
            }
        }

        #endregion
    }
}