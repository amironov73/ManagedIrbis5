// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* ChunkedBuffer.cs -- аналог MemoryStream на коротких блоках
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Аналог <see cref="System.IO.MemoryStream"/>,
/// использующий короткие блоки с данными.
/// </summary>
public sealed class ChunkedBuffer
{
    #region Constants

    /// <summary>
    /// Default chunk size.
    /// </summary>
    public const int DefaultChunkSize = 2048;

    #endregion

    #region Nested classes

    /// <summary>
    /// Непрерывный блок памяти.
    /// </summary>
    private sealed class Chunk
    {
        public readonly byte[] buffer;
        public Chunk? next;

        public Chunk (int size) => buffer = new byte[size];
    }

    #endregion

    #region Properties

    /// <summary>
    /// Размер блоков.
    /// </summary>
    public int ChunkSize => _chunkSize;

    /// <summary>
    /// Достигнут конец?
    /// </summary>
    public bool Eof
    {
        get
        {
            if (_current is null)
            {
                return true;
            }

            if (ReferenceEquals (_current, _last))
            {
                return _read >= _position;
            }

            return false;
        }
    }

    /// <summary>
    /// Общая длина записанных данных в байтах.
    /// </summary>
    public int Length
    {
        get
        {
            var result = 0;

            for (
                    var chunk = _first;
                    chunk is not null && !ReferenceEquals (chunk, _last);
                    chunk = chunk.next
                )
            {
                result += _chunkSize;
            }

            result += _position;

            return result;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ChunkedBuffer
        (
            int chunkSize = DefaultChunkSize
        )
    {
        Sure.Positive (chunkSize);

        _chunkSize = chunkSize;
    }

    #endregion

    #region Private members

    private Chunk? _first;
    private Chunk? _current;
    private Chunk? _last;
    private readonly int _chunkSize;
    private int _position, _read;

    private bool _Advance()
    {
        if (ReferenceEquals (_current, _last))
        {
            return false;
        }

        if (_current is null)
        {
            throw new ArsMagnaException ("_current is null");
        }

        _current = _current.next;
        _read = 0;

        return true;
    }

    private void _AppendChunk()
    {
        var newChunk = new Chunk (_chunkSize);
        if (_first is null)
        {
            _first = newChunk;
            _current = newChunk;
        }
        else
        {
            if (_last is null)
            {
                throw new ArsMagnaException ("_last is null");
            }

            _last.next = newChunk;
        }

        _last = newChunk;
        _position = 0;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Вычитывание данных из заданного потока.
    /// </summary>
    public void CopyFrom
        (
            Stream stream,
            int bufferSize
        )
    {
        Sure.Positive (bufferSize);

        var buffer = new byte[bufferSize];
        int read;
        while ((read = stream.Read (buffer, 0, buffer.Length)) > 0)
        {
            Write (buffer, 0, read);
        }
    }

    /// <summary>
    /// Подглядывание следующего байта.
    /// </summary>
    public int Peek()
    {
        if (_current is null)
        {
            return -1;
        }

        if (ReferenceEquals (_current, _last))
        {
            if (_read >= _position)
            {
                return -1;
            }
        }
        else
        {
            if (_read >= _chunkSize)
            {
                _Advance();
            }
        }

        return _current.buffer[_read];
    }

    /// <summary>
    /// Чтение данных в указанный массив.
    /// </summary>
    public int Read (byte[] buffer) => Read (buffer, 0, buffer.Length);

    /// <summary>
    /// Чтение данных в указанный массив.
    /// </summary>
    public int Read
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.NonNegative (offset);

        if (count <= 0)
        {
            return 0;
        }

        if (_current is null)
        {
            return 0;
        }

        var total = 0;
        do
        {
            var remaining = ReferenceEquals (_current, _last)
                ? _position - _read
                : _chunkSize - _read;

            if (remaining <= 0)
            {
                if (!_Advance())
                {
                    break;
                }
            }

            var portion = Math.Min (count, remaining);
            Array.Copy
                (
                    _current.buffer,
                    _read,
                    buffer,
                    offset,
                    portion
                );
            _read += portion;
            offset += portion;
            count -= portion;
            total += portion;
        } while (count > 0);

        return total;
    }

    /// <summary>
    /// Побайтовое чтение.
    /// </summary>
    public int ReadByte()
    {
        if (_current is null)
        {
            return -1;
        }

        if (ReferenceEquals (_current, _last))
        {
            if (_read >= _position)
            {
                return -1;
            }
        }
        else
        {
            if (_read >= _chunkSize)
            {
                _Advance();
            }
        }

        return _current.buffer[_read++];
    }

    /// <summary>
    /// Чтение строки.
    /// </summary>
    public string? ReadLine
        (
            Encoding encoding
        )
    {
        Sure.NotNull (encoding);

        if (Eof)
        {
            return null;
        }

        if (_current is null)
        {
            return null;
        }

        var result = new MemoryStream();
        byte found = 0;
        while (found == 0)
        {
            var buffer = _current.buffer;
            int stop = ReferenceEquals (_current, _last)
                ? _position
                : _chunkSize;
            var head = _read;
            for (; head < stop; head++)
            {
                byte c = buffer[head];
                if (c == '\r' || c == '\n')
                {
                    found = c;
                    break;
                }
            }

            result.Write (buffer, _read, head - _read);
            _read = head;
            if (found != 0)
            {
                _read++;
            }
            else
            {
                if (!_Advance())
                {
                    break;
                }
            }
        }

        if (found == '\r')
        {
            if (Peek() == '\n')
            {
                ReadByte();
            }
        }

        return encoding.GetString (result.ToArray());
    }

    /// <summary>
    /// Перемотка к началу.
    /// </summary>
    public void Rewind()
    {
        _current = _first;
        _read = 0;
    }

    /// <summary>
    /// Доступ к внутренним буферам данных.
    /// </summary>
    public byte[][] ToArrays
        (
            int prefix
        )
    {
        Sure.NonNegative (prefix);

        var result = new List<byte[]>();

        for (var i = 0; i < prefix; i++)
        {
            result.Add (Array.Empty<byte>());
        }

        for (
                var chunk = _first;
                chunk is not null && !ReferenceEquals (chunk, _last);
                chunk = chunk.next
            )
        {
            result.Add (chunk.buffer);
        }

        if (_position != 0)
        {
            if (_last is null)
            {
                throw new ArsMagnaException ("_last is null");
            }

            var chunk = new byte[_position];
            Array.Copy (_last.buffer, 0, chunk, 0, _position);
            result.Add (chunk);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Получение всех записанных данных в виде одного большого массива.
    /// </summary>
    public byte[] ToBigArray()
    {
        var total = Length;
        var result = new byte[total];
        var offset = 0;
        for (
                var chunk = _first;
                chunk is not null && !ReferenceEquals (chunk, _last);
                chunk = chunk.next
            )
        {
            Array.Copy (chunk.buffer, 0, result, offset, _chunkSize);
            offset += _chunkSize;
        }

        if (_position != 0)
        {
            if (_last is null)
            {
                throw new ArsMagnaException ("_last is null");
            }

            Array.Copy
                (
                    _last.buffer,
                    0,
                    result,
                    offset,
                    _position
                );
        }

        return result;
    }

    /// <summary>
    /// Вывод массива байтов в текущую позицию.
    /// </summary>
    public void Write (byte[] buffer) => Write (buffer, 0, buffer.Length);


    /// <summary>
    /// Вывод массива байтов в текущую позицию.
    /// </summary>
    public void Write
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        Sure.NotNull (buffer);
        Sure.NonNegative (offset);

        if (count <= 0)
        {
            return;
        }

        if (_first is null)
        {
            _AppendChunk();
        }

        do
        {
            var free = _chunkSize - _position;
            if (free == 0)
            {
                _AppendChunk();
                free = _chunkSize;
            }

            if (_last is null)
            {
                throw new ArsMagnaException ("_last is null");
            }

            var portion = Math.Min (count, free);
            Array.Copy
                (
                    buffer,
                    offset,
                    _last.buffer,
                    _position,
                    portion
                );

            _position += portion;
            count -= portion;
            offset += portion;
        } while (count > 0);
    }

    /// <summary>
    /// Вывод в поток текста с указанной кодировкой.
    /// </summary>
    public void Write
        (
            string text,
            Encoding encoding
        )
    {
        Sure.NotNull (text);
        Sure.NotNull (encoding);

        var bytes = encoding.GetBytes (text);

        Write (bytes);
    }

    /// <summary>
    /// Вывод в поток байта в текущей позиции.
    /// </summary>
    public void WriteByte
        (
            byte value
        )
    {
        if (_first is null)
        {
            _AppendChunk();
        }

        if (_position >= _chunkSize)
        {
            _AppendChunk();
        }

        if (_last is null)
        {
            throw new ArsMagnaException ("_last is null");
        }

        _last.buffer[_position++] = value;
    }

    #endregion
}
