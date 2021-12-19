// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IFixedSizeBuffer.cs -- интерфейс буфера фиксированного размера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.FixedSizeBuffers;

/// <summary>
/// An interface for operations on fixed size buffers.
///
/// NB: You probably don't want to mention this interface directly (don't box the buffer!).
/// Use it as a type parameter constraint instead (<c>where TBuffer : IFixedSizeBuffer&lt;T&gt;</c>).
/// </summary>
/// <typeparam name="T">The type of the elements in the buffer</typeparam>
public interface IFixedSizeBuffer<T> : IDisposable
{
    /// <summary>
    /// Gets or sets the element at offset <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The index</param>
    /// <exception cref="ArgumentOutOfRangeException">The index was outside the bounds of the buffer</exception>
    /// <returns>The element at offset <paramref name="index"/>.</returns>
    T this[int index] { get; set; }

    /// <summary>
    /// Returns a <see cref="Span{T}"/> representing the buffer.
    ///
    /// This method is <strong>unsafe</strong>.
    /// You must ensure the <see cref="Span{T}"/> does not outlive the buffer itself.
    /// </summary>
    /// <returns>A <see cref="Span{T}"/> representing the buffer.</returns>
    Span<T> AsSpan();

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> representing the buffer.
    ///
    /// This method is <strong>unsafe</strong>.
    /// You must ensure the <see cref="ReadOnlySpan{T}"/> does not outlive the buffer itself.
    /// </summary>
    /// <returns>A <see cref="ReadOnlySpan{T}"/> representing the buffer.</returns>
    ReadOnlySpan<T> AsReadOnlySpan();
}
