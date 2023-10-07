﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode

/* DefaultPooledMemoryStreamManager.cs -- реализация PooledMemoryStreamManager по умолчанию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;

#endregion

namespace AM.Buffers;

/// <summary>
/// Реализация <see cref="PooledMemoryStreamManager"/> по умолчанию,
/// которая берет массивы байт из <see cref="ArrayPool{T}"/>.
/// </summary>
public sealed class DefaultPooledMemoryStreamManager
    : PooledMemoryStreamManager
{
    private readonly PooledMemoryStreamOptions _options;

    /// <summary>
    /// Construct the <see cref="DefaultPooledMemoryStreamManager"/>.
    /// </summary>
    /// <param name="options">The options to be used when renting buffer.</param>
    public DefaultPooledMemoryStreamManager
        (
            PooledMemoryStreamOptions? options
        )
    {
        _options = options ?? new PooledMemoryStreamOptions();
    }

    /// <summary>
    /// Rent a byte array from <see cref="ArrayPool{Byte}"/> and construct <see cref="BufferSegment"/>.
    /// </summary>
    /// <param name="length">The minimum length of the buffer.</param>
    /// <returns>The constructed <see cref="BufferSegment"/></returns>
    protected override BufferSegment AllocateBufferSegment
        (
            int length
        )
    {
        length = Math.Max (Math.Min (length, _options.MinimumSegmentSize), _options.MaximumSegmentSize);
        return new BufferSegment (ArrayPool<byte>.Shared.Rent (length));
    }

    /// <summary>
    /// Return the byte array in the <paramref name="segment"/> to <see cref="ArrayPool{Byte}"/>
    /// </summary>
    /// <param name="segment">The byte array to return.</param>
    protected override void FreeBufferSegment
        (
            BufferSegment? segment
        )
    {
        if (segment is null)
        {
            throw new ArgumentNullException (nameof (segment));
        }

        ArrayPool<byte>.Shared.Return (segment.Array);
        segment.Reset();
    }
}
