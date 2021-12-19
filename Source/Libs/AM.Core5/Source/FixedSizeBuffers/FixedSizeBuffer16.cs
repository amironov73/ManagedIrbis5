// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* FixedSizeBuffer16.cs -- буфер фиксированного размера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace AM.FixedSizeBuffers;

/// <summary>
/// Буфер фиксированного размера 16.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct FixedSizeBuffer16<T>
    : IFixedSizeBuffer<T>
{
    /// <summary>A slot in the buffer</summary>
    public T Item1;

    /// <summary>A slot in the buffer</summary>
    public T Item2;

    /// <summary>A slot in the buffer</summary>
    public T Item3;

    /// <summary>A slot in the buffer</summary>
    public T Item4;

    /// <summary>A slot in the buffer</summary>
    public T Item5;

    /// <summary>A slot in the buffer</summary>
    public T Item6;

    /// <summary>A slot in the buffer</summary>
    public T Item7;

    /// <summary>A slot in the buffer</summary>
    public T Item8;

    /// <summary>A slot in the buffer</summary>
    public T Item9;

    /// <summary>A slot in the buffer</summary>
    public T Item10;

    /// <summary>A slot in the buffer</summary>
    public T Item11;

    /// <summary>A slot in the buffer</summary>
    public T Item12;

    /// <summary>A slot in the buffer</summary>
    public T Item13;

    /// <summary>A slot in the buffer</summary>
    public T Item14;

    /// <summary>A slot in the buffer</summary>
    public T Item15;

    /// <summary>A slot in the buffer</summary>
    public T Item16;

    /// <summary>
    /// Gets or sets the element at offset <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index</param>
    /// <exception cref="ArgumentOutOfRangeException">The index was outside the bounds of the buffer</exception>
    /// <returns>The element at offset <paramref name="index"/>.</returns>
    public T this[int index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= 16)
            {
                throw new ArgumentOutOfRangeException (nameof (index));
            }
            return Unsafe.Add(ref Item1, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (index < 0 || index >= 16)
            {
                throw new ArgumentOutOfRangeException (nameof (index));
            }
            Unsafe.Add(ref Item1, index) = value;
        }
    }

    /// <summary>
    /// Returns a <see cref="Span{T}"/> representing the buffer.
    ///
    /// This method is <strong>unsafe</strong>.
    /// You must ensure the <see cref="Span{T}"/> does not outlive the buffer itself.
    /// </summary>
    /// <returns>A <see cref="Span{T}"/> representing the buffer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref Item1, 16);

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> representing the buffer.
    ///
    /// This method is <strong>unsafe</strong>.
    /// You must ensure the <see cref="ReadOnlySpan{T}"/> does not outlive the buffer itself.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> representing the buffer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsReadOnlySpan() => MemoryMarshal.CreateReadOnlySpan(ref Item1, 16);

    /// <summary>
    /// Call this method when you've finished using the buffer.
    ///
    /// Technically this method is a no-op, but calling it ensures that the
    /// buffer is not deallocated before you've finished working with it.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Dispose() { }
}
