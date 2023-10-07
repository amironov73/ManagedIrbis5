// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* BufferSegment.cs -- связанный список массивов байтов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;

#endregion

#pragma warning disable CA1819 // Properties should not return arrays

namespace AM.Buffers;

/// <summary>
/// Связанный список массивов байтов.
/// </summary>
public sealed class BufferSegment
    : ReadOnlySequenceSegment<byte>
{
    private byte[]? _array;

    /// <summary>
    /// Initialize the node with a byte array buffer.
    /// This node will use the byte array as the underlying
    /// storage until Reset is called.
    /// </summary>
    /// <param name="array">Byte array to be used by this node.</param>
    public BufferSegment
        (
            byte[] array
        )
    {
        Sure.NotNull (array);

        _array = array;
        Length = 0;
        Memory = array;
    }

    /// <summary>
    /// Gets the array this node is using.
    /// </summary>
    public byte[] Array => _array ?? throw new InvalidOperationException();

    /// <summary>
    /// Gets the available space in the buffer.
    /// </summary>
    internal int Available => (_array?.Length ?? throw new InvalidOperationException()) - Length;

    /// <summary>
    /// Gets the length of user data in the buffer.
    /// </summary>
    internal int Length { get; set; }

    /// <summary>
    /// Gets the total length of the buffer.
    /// </summary>
    internal int Capacity => _array?.Length ?? throw new InvalidOperationException();

    /// <summary>
    /// Gets the next node.
    /// </summary>
    internal new BufferSegment? Next => (BufferSegment?) base.Next;

    /// <summary>
    /// Sets the sum of node lengths before the current node.
    /// </summary>
    /// <param name="runningIndex">The sum of node lengths before the current node.</param>
    internal void SetRunningIndex
        (
            long runningIndex
        )
    {
        RunningIndex = runningIndex;
    }

    internal void UpdateMemory()
    {
        var array = _array;
        Memory = array?.AsMemory (0, Length) ?? default (ReadOnlyMemory<byte>);
    }

    /// <summary>
    /// Sets the next node.
    /// </summary>
    /// <param name="next">The next node.</param>
    internal void SetNext
        (
            BufferSegment? next
        )
    {
        base.Next = next;

        var runningIndex = RunningIndex;
        var current = this;

        while (next is not null)
        {
            runningIndex += current.Length;
            next.RunningIndex = runningIndex;
            current = next;
            next = next.Next;
        }
    }

    /// <summary>
    /// Clear all states associated with this instance.
    /// </summary>
    public void Reset()
    {
        _array = null;
        Length = 0;
        Memory = default;
        base.Next = null;
        RunningIndex = 0;
    }
}
