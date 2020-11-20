// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ChunkedBuffer.cs -- аналог MemoryStream на коротких чанках
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Аналог <see cref="System.IO.MemoryStream"/>,
    /// использующий короткие чанки с данными.
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
        /// Chunk of bytes.
        /// </summary>
        class Chunk
        {
            public readonly byte[] Buffer;

            public Chunk? Next;

            public Chunk
                (
                    int size
                )
            {
                Buffer = new byte[size];
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Chunk size.
        /// </summary>
        public int ChunkSize => _chunkSize;

        /// <summary>
        /// End of data?
        /// </summary>
        public bool Eof
        {
            get
            {
                if (ReferenceEquals(_current, null))
                {
                    return true;
                }

                if (ReferenceEquals(_current, _last))
                {
                    return _read >= _position;
                }

                return false;
            }
        }

        /// <summary>
        /// Total length.
        /// </summary>
        public int Length
        {
            get
            {
                var result = 0;

                for (
                        var chunk = _first;
                        !ReferenceEquals(chunk, null)
                        && !ReferenceEquals(chunk, _last);
                        chunk = chunk.Next
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
            Sure.Positive(chunkSize, nameof(chunkSize));

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
            if (ReferenceEquals(_current, _last))
            {
                return false;
            }

            if (ReferenceEquals(_current, null))
            {
                throw new ArsMagnaException("_current == null");
            }

            _current = _current.Next;
            _read = 0;

            return true;
        }

        private void _AppendChunk()
        {
            var newChunk = new Chunk(_chunkSize);
            if (ReferenceEquals(_first, null))
            {
                _first = newChunk;
                _current = newChunk;
            }
            else
            {
                if (ReferenceEquals(_last, null))
                {
                    throw new ArsMagnaException("_last == null");
                }

                _last.Next = newChunk;
            }
            _last = newChunk;
            _position = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Copy data from the stream.
        /// </summary>
        public void CopyFrom
            (
                Stream stream,
                int bufferSize
            )
        {
            Sure.Positive(bufferSize, nameof(bufferSize));

            var buffer = new byte[bufferSize];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                Write(buffer, 0, read);
            }
        }

        /// <summary>
        /// Peek one byte.
        /// </summary>
        public int Peek()
        {
            if (ReferenceEquals(_current, null))
            {
                return -1;
            }

            if (ReferenceEquals(_current, _last))
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

            return _current.Buffer[_read];
        }

        /// <summary>
        /// Read array of bytes.
        /// </summary>
        public int Read(byte[] buffer) => Read(buffer, 0, buffer.Length);

        /// <summary>
        /// Read bytes.
        /// </summary>
        public int Read
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            Sure.NonNegative(offset, nameof(offset));

            if (count <= 0)
            {
                return 0;
            }

            if (ReferenceEquals(_current, null))
            {
                return 0;
            }

            var total = 0;
            do
            {
                var remaining = ReferenceEquals(_current, _last)
                    ? _position - _read
                    : _chunkSize - _read;

                if (remaining <= 0)
                {
                    if (!_Advance())
                    {
                        break;
                    }
                }

                var portion = Math.Min(count, remaining);
                Array.Copy
                    (
                        _current.Buffer,
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
        /// Read one byte.
        /// </summary>
        public int ReadByte()
        {
            if (ReferenceEquals(_current, null))
            {
                return -1;
            }

            if (ReferenceEquals(_current, _last))
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

            return _current.Buffer[_read++];
        }

        /// <summary>
        /// Read one line from the current position.
        /// </summary>
        public string? ReadLine
            (
                Encoding encoding
            )
        {
            if (Eof)
            {
                return null;
            }

            if (ReferenceEquals(_current, null))
            {
                return null;
            }

            var result = new MemoryStream();
            byte found = 0;
            while (found == 0)
            {
                var buffer = _current.Buffer;
                int stop = ReferenceEquals(_current, _last)
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
                result.Write(buffer, _read, head - _read);
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

            return encoding.GetString(result.ToArray());
        }

        /// <summary>
        /// Rewind to the beginning.
        /// </summary>
        public void Rewind()
        {
            _current = _first;
            _read = 0;
        }

        /// <summary>
        /// Get internal buffers.
        /// </summary>
        public byte[][] ToArrays
            (
                int prefix
            )
        {
            var result = new List<byte[]>();

            for (var i = 0; i < prefix; i++)
            {
                result.Add(Array.Empty<byte>());
            }

            for (
                    var chunk = _first;
                    !ReferenceEquals(chunk, null)
                    && !ReferenceEquals(chunk, _last);
                    chunk = chunk.Next
                )
            {
                result.Add(chunk.Buffer);
            }

            if (_position != 0)
            {
                if (ReferenceEquals(_last, null))
                {
                    throw new ArsMagnaException("_last == null");
                }

                var chunk = new byte[_position];
                Array.Copy(_last.Buffer, 0, chunk, 0, _position);
                result.Add(chunk);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get all data as one big array of bytes.
        /// </summary>
        public byte[] ToBigArray()
        {
            var total = Length;
            var result = new byte[total];
            var offset = 0;
            for (
                    var chunk = _first;
                    !ReferenceEquals(chunk, null)
                    && !ReferenceEquals(chunk, _last);
                    chunk = chunk.Next
                )
            {
                Array.Copy(chunk.Buffer, 0, result, offset, _chunkSize);
                offset += _chunkSize;
            }

            if (_position != 0)
            {
                if (ReferenceEquals(_last, null))
                {
                    throw new ArsMagnaException("_last == null");
                }

                Array.Copy
                    (
                        _last.Buffer,
                        0,
                        result,
                        offset,
                        _position
                    );
            }

            return result;
        }

        /// <summary>
        /// Write a block of bytes to the current stream
        /// using data read from a buffer.
        /// </summary>
        public void Write(byte[] buffer) => Write(buffer, 0, buffer.Length);


        /// <summary>
        /// Write a block of bytes to the current stream
        /// using data read from a buffer.
        /// </summary>
        public void Write
            (
                byte[] buffer,
                int offset,
                int count
            )
        {
            Sure.NonNegative(offset, nameof(offset));

            if (count <= 0)
            {
                return;
            }

            if (ReferenceEquals(_first, null))
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

                if (ReferenceEquals(_last, null))
                {
                    throw new ArsMagnaException("_last == null");
                }

                var portion = Math.Min(count, free);
                Array.Copy
                    (
                        buffer,
                        offset,
                        _last.Buffer,
                        _position,
                        portion
                    );

                _position += portion;
                count -= portion;
                offset += portion;
            } while (count > 0);
        }

        /// <summary>
        /// Write the text with encoding.
        /// </summary>
        public void Write
            (
                string text,
                Encoding encoding
            )
        {
            var bytes = encoding.GetBytes(text);

            Write(bytes);
        }

        /// <summary>
        /// Write a byte to the current stream at the current position.
        /// </summary>
        public void WriteByte
            (
                byte value
            )
        {
            if (ReferenceEquals(_first, null))
            {
                _AppendChunk();
            }

            if (_position >= _chunkSize)
            {
                _AppendChunk();
            }

            if (ReferenceEquals(_last, null))
            {
                throw new ArsMagnaException("_last == null");
            }

            _last.Buffer[_position++] = value;
        }

        #endregion
    }
}
