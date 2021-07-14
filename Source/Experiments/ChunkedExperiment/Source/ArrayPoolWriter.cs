// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ArrayPoolWriter.cs -- буфер поверх цепочки массивов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

#endregion

#nullable enable

namespace ChunkedExperiment
{
    /*

        Как работает IBufferWriter:

        1. Клиент вызывает GetSpan (или GetMemory, что ему удобнее),
           при этом указывает, какой длины регион непрерывной памяти
           ему нужен. Как правило 0, т. е. "давай сколько есть".
           Если IBufferWriter не может удовлетворить запрос
           (т. е. выдать непрерывный регион), он выбрасывает исключение.
           Выдавать меньший регион в ответ на запрос нельзя!

        2. Клиент что-то туда записывает и вызывает Advance,
           чтобы IBufferWriter сдвинул указатель у себя.

        3. Пункты 1 и 2 повторяются по мере необходимости.

     */

    /// <summary>
    /// Буфер поверх цепочки массивов.
    /// </summary>
    public sealed class ArrayPoolWriter
        : IBufferWriter<byte>,
        IDisposable
    {
        #region Constants

        /// <summary>
        /// Размер чанка по умолчанию.
        /// </summary>
        public const int DefaultChunkSize = 2048;

        #endregion

        #region Properties

        /// <summary>
        /// Размер фрагмента.
        /// </summary>
        public int ChunkSize => _chunkSize;

        /// <summary>
        /// Используемый пул.
        /// </summary>
        public ArrayPool<byte> Pool => _pool;

        /// <summary>
        /// Всего байт записано.
        /// </summary>
        public int TotalWritten => _total;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public ArrayPoolWriter()
            : this (ArrayPool<byte>.Shared, DefaultChunkSize)
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="arrayPool">Пул массивов.</param>
        /// <param name="chunkSize">Размер фрагмента.</param>
        public ArrayPoolWriter
            (
                ArrayPool<byte> arrayPool,
                int chunkSize
            )
        {
            if (chunkSize < 16)
            {
                throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "Value too small");
            }

            _pool = arrayPool;
            _chunkSize = chunkSize;
            _chunks = new SinglyLinkedList<byte[]>();

        } // constructor

        #endregion

        #region Private members

        private readonly int _chunkSize;
        private readonly ArrayPool<byte> _pool;
        private readonly SinglyLinkedList<byte[]> _chunks;
        private int _position;
        private int _total;

        /// <summary>
        /// Добавляем в конец цепочки новый фрагмент указанного размера.
        /// </summary>
        private void ResizeBuffer
            (
                int delta = 0
            )
        {
            int chunkSize;

            if (delta <= _chunkSize)
            {
                chunkSize = _chunkSize;
            }
            else
            {
                chunkSize = 16;
                while (chunkSize < delta)
                {
                    chunkSize *= 2;
                }
            }

            _chunks.Add(_pool.Rent(chunkSize));

        } // method ResizeBuffer

        #endregion

        #region Public methods

        /// <summary>
        /// Дамп содержимого.
        /// </summary>
        public void Dump
            (
                TextWriter output
            )
        {
            var counter = 0;
            foreach (var memory in this)
            {
                foreach (var b in memory.Span)
                {
                    if (counter % 16 == 0)
                    {
                        if (counter != 0)
                        {
                            output.WriteLine();
                        }

                        output.Write($"{counter:x8}>");
                    }

                    output.Write($" {b:x2}");
                    ++counter;
                }
            }

            output.WriteLine();

        } // method Dump

        #endregion

        #region IBufferWriter members

        /// <inheritdoc cref="IBufferWriter{T}.Advance"/>
        public void Advance
            (
                int count
            )
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Negative value");
            }

            var array = _chunks.Last?.Value;
            var chunkSize = array?.Length ?? _chunkSize;
            var delta = chunkSize - _position;
            if (count > delta)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Too far");
            }

            _position += count;
            _total += count;

        } // method Advance

        /// <inheritdoc cref="IBufferWriter{T}.GetMemory"/>
        public Memory<byte> GetMemory
            (
                int sizeHint = 0
            )
        {
            if (sizeHint < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sizeHint), sizeHint, "Negative value");
            }

            if (sizeHint == 0)
            {
                // ноль означает "дай буфер любой длины"
                sizeHint = 1;
            }

            var array = _chunks.Last?.Value;
            var chunkSize = array?.Length ?? _chunkSize;
            if (array is null)
            {
                ResizeBuffer(sizeHint);
                array = _chunks.Last!.Value;
                _position = 0;
            }
            else
            {
                if (_position == chunkSize)
                {
                    ResizeBuffer(_chunkSize);
                    array = _chunks.Last!.Value;
                    _position = 0;
                }
                else
                {
                    var delta = chunkSize - _position;
                    if (sizeHint > delta)
                    {
                        throw new ArgumentOutOfRangeException(nameof(sizeHint), sizeHint, "Value too big");
                    }
                }
            }

            return array.AsMemory(_position);

        } // method GetMemory

        /// <inheritdoc cref="IBufferWriter{T}.GetSpan"/>
        public Span<byte> GetSpan(int sizeHint = 0) => GetMemory(sizeHint).Span;

        #endregion

        #region IEnumerable members

        /// <summary>
        /// Получение перечислителя.
        /// </summary>
        public Enumerator GetEnumerator() => new (this);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            foreach (var array in _chunks)
            {
                _pool.Return(array);
            }

        } // method Dispose

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => _total.ToString(CultureInfo.InvariantCulture);

        #endregion

        #region Enumerator

        /// <summary>
        /// Перечисляет записанные фрагменты.
        /// </summary>
        public struct Enumerator
        {
            #region Construction

            /// <summary>
            /// Конструктор.
            /// </summary>
            public Enumerator(ArrayPoolWriter writer) : this() => _writer = writer;

            #endregion

            #region Private members

            private readonly ArrayPoolWriter _writer;
            private SinglyLinkedListNode<byte[]>? _current;

            #endregion

            #region IEnumerator members

            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            public bool MoveNext() =>
                (_current = _current is null ? _writer._chunks.First : _current.Next) is not null;

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            public ReadOnlyMemory<byte> Current
            {
                get
                {
                    if (_current is null)
                    {
                        return default;
                    }

                    var array = _current.Value;

                    return ReferenceEquals(_current, _writer._chunks.Last)
                        ? array.AsMemory(0, _writer._position)
                        : array.AsMemory();
                }
            }

            #endregion

        } // struct Enumerator

        #endregion

    } // method ArrayPoolWriter

} // namespace ChunkedExperiment
