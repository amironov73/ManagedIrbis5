// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* Buffer.cs -- dynamic byte buffer
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
/// Dynamic byte buffer.
/// </summary>
public class Buffer
{
    private byte[]? _data;
    private long _size;
    private long _offset;

    /// <summary>
    /// Is the buffer empty?
    /// </summary>
    public bool IsEmpty => (_data == null) || (_size == 0);

    /// <summary>
    /// Bytes memory buffer
    /// </summary>
    public byte[] Data => _data!;

    /// <summary>
    /// Bytes memory buffer capacity
    /// </summary>
    public long Capacity => _data?.Length ?? 0;

    /// <summary>
    /// Bytes memory buffer size
    /// </summary>
    public long Size => _size;

    /// <summary>
    /// Bytes memory buffer offset
    /// </summary>
    public long Offset => _offset;

    /// <summary>
    /// Buffer indexer operator.
    /// </summary>
    public byte this [int index] => _data![index];

    /// <summary>
    /// Initialize a new expandable buffer with zero capacity
    /// </summary>
    public Buffer()
    {
        _data = Array.Empty<byte>();
        _size = 0;
        _offset = 0;
    }

    /// <summary>
    /// Initialize a new expandable buffer with the given capacity
    /// </summary>
    public Buffer
        (
            long capacity
        )
    {
        _data = new byte[capacity];
        _size = 0;
        _offset = 0;
    }

    /// <summary>
    /// Initialize a new expandable buffer with the given data
    /// </summary>
    public Buffer (byte[] data)
    {
        _data = data;
        _size = data.Length;
        _offset = 0;
    }

    #region Memory buffer methods

    /// <summary>
    /// Get string from the current buffer
    /// </summary>
    public override string ToString()
    {
        return ExtractString (0, _size);
    }

    /// <summary>
    /// Clear the current buffer and its offset
    /// </summary>
    public void Clear()
    {
        _size = 0;
        _offset = 0;
    }

    /// <summary>
    /// Extract the string from buffer of the given offset and size
    /// </summary>
    public string ExtractString
        (
            long offset,
            long size
        )
    {
        Debug.Assert (((offset + size) <= Size), "Invalid offset & size!");
        if (offset + size > Size)
        {
            throw new ArgumentException ("Invalid offset & size!", nameof (offset));
        }

        return Encoding.UTF8.GetString (_data!, (int)offset, (int)size);
    }

    /// <summary>
    /// Remove the buffer of the given offset and size
    /// </summary>
    public void Remove (long offset, long size)
    {
        Debug.Assert (((offset + size) <= Size), "Invalid offset & size!");
        if ((offset + size) > Size)
        {
            throw new ArgumentException ("Invalid offset & size!", nameof (offset));
        }

        Array.Copy
            (
                sourceArray: _data!,
                sourceIndex: offset + size,
                destinationArray: _data!,
                destinationIndex: offset,
                length: _size - size - offset
            );
        _size -= size;
        if (_offset >= (offset + size))
        {
            _offset -= size;
        }
        else if (_offset >= offset)
        {
            _offset -= _offset - offset;
            if (_offset > Size)
            {
                _offset = Size;
            }
        }
    }

    /// <summary>
    /// Reserve the buffer of the given capacity
    /// </summary>
    public void Reserve
        (
            long capacity
        )
    {
        Debug.Assert ((capacity >= 0), "Invalid reserve capacity!");
        if (capacity < 0)
        {
            throw new ArgumentException ("Invalid reserve capacity!", nameof (capacity));
        }

        if (capacity > Capacity)
        {
            var data = new byte[Math.Max (capacity, 2 * Capacity)];
            Array.Copy
                (
                    sourceArray: _data!,
                    sourceIndex: 0,
                    destinationArray: data,
                    destinationIndex: 0,
                    length: _size
                );
            _data = data;
        }
    }

    /// <summary>
    /// Resize the current buffer
    /// </summary>
    public void Resize
        (
            long size
        )
    {
        Reserve (size);
        _size = size;
        if (_offset > _size)
        {
            _offset = _size;
        }
    }

    /// <summary>
    /// Shift the current buffer offset
    /// </summary>
    public void Shift
        (
            long offset
        )
    {
        _offset += offset;
    }

    /// <summary>
    /// Unshift the current buffer offset
    /// </summary>
    public void Unshift
        (
            long offset
        )
    {
        _offset -= offset;
    }

    #endregion

    #region Buffer I/O methods

    /// <summary>
    /// Append the given buffer
    /// </summary>
    /// <param name="buffer">Buffer to append</param>
    /// <returns>Count of append bytes</returns>
    public long Append
        (
            byte[] buffer
        )
    {
        Reserve (_size + buffer.Length);
        Array.Copy
            (
                sourceArray: buffer,
                sourceIndex: 0,
                destinationArray: _data!,
                destinationIndex: _size,
                length: buffer.Length
            );
        _size += buffer.Length;
        return buffer.Length;
    }

    /// <summary>
    /// Append the given buffer fragment
    /// </summary>
    /// <param name="buffer">Buffer to append</param>
    /// <param name="offset">Buffer offset</param>
    /// <param name="size">Buffer size</param>
    /// <returns>Count of append bytes</returns>
    public long Append
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        Reserve (_size + size);
        Array.Copy
            (
                sourceArray: buffer,
                sourceIndex: offset,
                destinationArray: _data!,
                destinationIndex: _size,
                length: size
            );
        _size += size;
        return size;
    }

    /// <summary>
    /// Append the given text in UTF-8 encoding
    /// </summary>
    /// <param name="text">Text to append</param>
    /// <returns>Count of append bytes</returns>
    public long Append
        (
            string text
        )
    {
        Reserve (_size + Encoding.UTF8.GetMaxByteCount (text.Length));
        long result = Encoding.UTF8.GetBytes
            (
                text,
                charIndex: 0,
                charCount: text.Length,
                _data!,
                byteIndex: (int)_size
            );
        _size += result;
        return result;
    }

    #endregion
}
