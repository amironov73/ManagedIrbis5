// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* FixedSizeBuffer64.cs -- буфер фиксированного размера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

namespace AM.FixedSizeBuffers;

/// <summary>
/// Буфер фиксированного размера 64.
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer</typeparam>
[StructLayout(LayoutKind.Sequential)]
public struct FixedSizeBuffer64<T>
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

    /// <summary>A slot in the buffer</summary>
    public T Item17;

    /// <summary>A slot in the buffer</summary>
    public T Item18;

    /// <summary>A slot in the buffer</summary>
    public T Item19;

    /// <summary>A slot in the buffer</summary>
    public T Item20;

    /// <summary>A slot in the buffer</summary>
    public T Item21;

    /// <summary>A slot in the buffer</summary>
    public T Item22;

    /// <summary>A slot in the buffer</summary>
    public T Item23;

    /// <summary>A slot in the buffer</summary>
    public T Item24;

    /// <summary>A slot in the buffer</summary>
    public T Item25;

    /// <summary>A slot in the buffer</summary>
    public T Item26;

    /// <summary>A slot in the buffer</summary>
    public T Item27;

    /// <summary>A slot in the buffer</summary>
    public T Item28;

    /// <summary>A slot in the buffer</summary>
    public T Item29;

    /// <summary>A slot in the buffer</summary>
    public T Item30;

    /// <summary>A slot in the buffer</summary>
    public T Item31;

    /// <summary>A slot in the buffer</summary>
    public T Item32;

    /// <summary>A slot in the buffer</summary>
    public T Item33;

    /// <summary>A slot in the buffer</summary>
    public T Item34;

    /// <summary>A slot in the buffer</summary>
    public T Item35;

    /// <summary>A slot in the buffer</summary>
    public T Item36;

    /// <summary>A slot in the buffer</summary>
    public T Item37;

    /// <summary>A slot in the buffer</summary>
    public T Item38;

    /// <summary>A slot in the buffer</summary>
    public T Item39;

    /// <summary>A slot in the buffer</summary>
    public T Item40;

    /// <summary>A slot in the buffer</summary>
    public T Item41;

    /// <summary>A slot in the buffer</summary>
    public T Item42;

    /// <summary>A slot in the buffer</summary>
    public T Item43;

    /// <summary>A slot in the buffer</summary>
    public T Item44;

    /// <summary>A slot in the buffer</summary>
    public T Item45;

    /// <summary>A slot in the buffer</summary>
    public T Item46;

    /// <summary>A slot in the buffer</summary>
    public T Item47;

    /// <summary>A slot in the buffer</summary>
    public T Item48;

    /// <summary>A slot in the buffer</summary>
    public T Item49;

    /// <summary>A slot in the buffer</summary>
    public T Item50;

    /// <summary>A slot in the buffer</summary>
    public T Item51;

    /// <summary>A slot in the buffer</summary>
    public T Item52;

    /// <summary>A slot in the buffer</summary>
    public T Item53;

    /// <summary>A slot in the buffer</summary>
    public T Item54;

    /// <summary>A slot in the buffer</summary>
    public T Item55;

    /// <summary>A slot in the buffer</summary>
    public T Item56;

    /// <summary>A slot in the buffer</summary>
    public T Item57;

    /// <summary>A slot in the buffer</summary>
    public T Item58;

    /// <summary>A slot in the buffer</summary>
    public T Item59;

    /// <summary>A slot in the buffer</summary>
    public T Item60;

    /// <summary>A slot in the buffer</summary>
    public T Item61;

    /// <summary>A slot in the buffer</summary>
    public T Item62;

    /// <summary>A slot in the buffer</summary>
    public T Item63;

    /// <summary>A slot in the buffer</summary>
    public T Item64;

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
            if (index < 0 || index >= 64)
            {
                throw new ArgumentOutOfRangeException (nameof (index));
            }
            return Unsafe.Add(ref Item1, index);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
            if (index < 0 || index >= 64)
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
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref Item1, 64);

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> representing the buffer.
    ///
    /// This method is <strong>unsafe</strong>.
    /// You must ensure the <see cref="ReadOnlySpan{T}"/> does not outlive the buffer itself.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> representing the buffer.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ReadOnlySpan<T> AsReadOnlySpan() => MemoryMarshal.CreateReadOnlySpan(ref Item1, 64);

    /// <summary>
    /// Call this method when you've finished using the buffer.
    ///
    /// Technically this method is a no-op, but calling it ensures that the
    /// buffer is not deallocated before you've finished working with it.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Dispose() { }
}
