// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PooledMemoryStreamManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Buffers;

/// <summary>
/// A stream manager for constructing <see cref="PooledMemoryStream"/>.
/// </summary>
public abstract class PooledMemoryStreamManager
{
    /// <summary>
    /// A shared implement of <see cref="PooledMemoryStreamManager"/>.
    /// </summary>
    public static PooledMemoryStreamManager Shared { get; } = new DefaultPooledMemoryStreamManager (null);

    /// <summary>
    /// Construct a new <see cref="PooledMemoryStream"/> whose buffer will be managed by this instance.
    /// </summary>
    /// <returns></returns>
    public PooledMemoryStream GetStream()
    {
        return new PooledMemoryStream (this);
    }

    /// <summary>
    /// Allocate or rent a new <see cref="BufferSegment"/>.
    /// </summary>
    /// <param name="length">The minimum length for the buffer.</param>
    /// <returns>The allocated <see cref="BufferSegment"/>. </returns>
    protected abstract BufferSegment AllocateBufferSegment (int length);

    /// <summary>
    /// Free or return a <see cref="BufferSegment"/>
    /// </summary>
    /// <param name="segment">The object to free or return to the pool</param>
    protected abstract void FreeBufferSegment (BufferSegment segment);

    internal BufferSegment Allocate
        (
            int length
        )
    {
        length = Math.Max (length, 0);

        return AllocateBufferSegment (length);
    }

    internal void Free
        (
            BufferSegment segment
        )
    {
        Sure.NotNull (segment);

        FreeBufferSegment (segment);
    }
}
