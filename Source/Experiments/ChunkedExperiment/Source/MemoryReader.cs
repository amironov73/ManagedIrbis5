// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable UnusedMember.Global

/* MemoryReader.cs -- позволяет как бы непрерывно считывать память, разбросанную по фрагментам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ChunkedExperiment
{
    // Алиас для краткости
    using Chunk = ReadOnlyMemory<byte>;

    /// <summary>
    /// Позволяет как бы непрерывно считывать память,
    /// разбросанную по фрагментам.
    /// </summary>
    public sealed class MemoryReader
    {
        #region Properties

        /// <summary>
        /// Количество байт, доступных для считывания.
        /// </summary>
        public int Available
        {
            get
            {
                if (_current is null)
                {
                    return 0;
                }

                var first = true;
                var result = 0;
                for (var unit = _current; unit is not null; unit = unit.Next)
                {
                    var delta = unit.Value.Length;
                    if (first)
                    {
                        delta -= _position;
                    }

                    result += delta;
                    first = false;
                }

                return result;
            }
        } // property Available

        /// <summary>
        /// Достигнут конец?
        /// </summary>
        public bool IsEof => !_Advance();

        /// <summary>
        /// Всего прочитано байт.
        /// </summary>
        public int TotalRead => _total;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public MemoryReader()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="chunks">Чанки.</param>
        public MemoryReader
            (
                IEnumerable<Chunk> chunks
            )
        {
            foreach (var chunk in chunks)
            {
                if (!chunk.IsEmpty)
                {
                    _chunks.Add(chunk);
                }
            }

            _current = _chunks.First;

        } // constructor

        #endregion

        #region Private members

        private readonly SinglyLinkedList<Chunk> _chunks = new ();
        private SinglyLinkedListNode<Chunk>? _current;
        private int _position;
        private int _total;

        /// <summary>
        /// Продвигается по цепочке блоков.
        /// </summary>
        /// <returns><c>false</c>, если продвинуться не удалось.
        /// </returns>
        private bool _Advance()
        {
            while (true)
            {
                if (_current is null)
                {
                    return false;
                }

                if (_position < _current.Value.Length)
                {
                    return true;
                }

                _current = _current.Next;
                _position = 0;
            }
        }

        /// <summary>
        /// Получение текущего чанка (с текущей позиции).
        /// </summary>
        private Chunk GetCurrentChunk() =>
            _Advance() ? _current!.Value.Slice(_position) : default;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление чанка в конец списка.
        /// </summary>
        /// <param name="chunk">Чанк.</param>
        public void Add
            (
                Chunk chunk
            )
        {
            if (!chunk.IsEmpty)
            {
                _chunks.Add(chunk);
            }

            _current ??= _chunks.First;

        } // method Add

        /// <summary>
        /// Подхватываем память от буфера записи.
        /// </summary>
        public static MemoryReader FromWriter
            (
                ArrayPoolWriter writer
            )
        {
            var result = new MemoryReader();
            foreach (var chunk in writer)
            {
                result.Add(chunk);
            }

            return result;

        } // method FromWriter

        /// <summary>
        /// Считывание непрерывного блока памяти.
        /// </summary>
        /// <returns>
        /// Прочитанный блок (возможно, меньшей, чем запрошено,
        /// но не нулевой, длины).
        /// По достижении конца возвращается блок нулевой длины.
        /// </returns>
        public Chunk ReadBlock
            (
                int limit = Int32.MaxValue
            )
        {
            if (limit <= 0 || !_Advance())
            {
                return default;
            }

            var unit = _current!.Value;
            var piece = Math.Min(limit, unit.Length - _position);
            var result = unit.Slice(_position, piece);
            _position += piece;
            _total += piece;

            return result;

        } // method ReadBlock

        /// <summary>
        /// Считывание одного байта.
        /// </summary>
        /// <returns>Прочитанный байт либо -1.</returns>
        public int ReadByte()
        {
            if (!_Advance())
            {
                return -1;
            }

            ++_total;
            return _current!.Value.Span[_position++];

        } // method ReadByte

        /// <summary>
        /// Считывание вплоть до указанного байта.
        /// Сам этот байт считывается, но в результат не помещается.
        /// </summary>
        /// <param name="limiter">Байт-ограничитель,
        /// например, символ перевода строки.</param>
        /// <returns>Чанк и признак того, что байт-ограничитель найден.
        /// </returns>
        public (Chunk Chunk, bool Found) ReadTo
            (
                byte limiter
            )
        {
            var chunk = GetCurrentChunk();
            if (chunk.IsEmpty)
            {
                return (chunk, false);
            }

            var index = chunk.Span.IndexOf(limiter);
            var found = index >= 0;
            var piece = found ? index : chunk.Length;
            var result = found ? chunk.Slice(0, piece) : chunk;
            _total += piece;
            _position += piece;
            if (found)
            {
                ++_total;
                ++_position;
            }

            return (result, found);

        } // method ReadTo

        #endregion

    } // class MemoryReader

} // namespace ChunkedExperiment
