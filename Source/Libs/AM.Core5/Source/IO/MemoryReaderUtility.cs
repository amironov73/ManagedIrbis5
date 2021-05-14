// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable ReplaceSliceWithRangeIndexer

/* MemoryReaderUtility.cs -- методы расширения для MemoryReader
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Collections;

#endregion

#nullable enable

namespace AM.IO
{
    // Алиас для краткости
    using Chunk = ReadOnlyMemory<byte>;

    /// <summary>
    /// Методы расширения для MemoryReader.
    /// </summary>
    public static class MemoryReaderUtility
    {
        #region Public methods

        /// <summary>
        /// Чтение указанного объема памяти, возможно, кусками.
        /// </summary>
        /// <param name="reader">Читатель.</param>
        /// <param name="limit">Максимальное количество считываемых байт.</param>
        /// <returns>Последовательность чанков (возможно, пустая).</returns>
        public static IEnumerable<Chunk> Enumerate
            (
                this MemoryReader reader,
                int limit = int.MaxValue
            )
        {
            while (limit > 0 && !reader.IsEof)
            {
                var chunk = reader.ReadBlock(limit);
                limit -= chunk.Length;
                yield return chunk;
            }
        }

        /// <summary>
        /// Чтение непрерывного блока памяти.
        /// При необходимости куски склеиваются автоматически.
        /// </summary>
        /// <param name="reader">Читатель.</param>
        /// <param name="limit">Максимальное количество считываемых байт.
        /// </param>
        /// <returns>Блок.</returns>
        public static Chunk ReadContinuous
            (
                this MemoryReader reader,
                int limit = int.MaxValue
            )
        {
            var firstChunk = reader.ReadBlock(limit);
            if (firstChunk.IsEmpty || reader.IsEof)
            {
                return firstChunk;
            }

            var subsequentChunks = new SinglyLinkedList<Chunk>();
            limit -= firstChunk.Length;
            while (!reader.IsEof && limit > 0)
            {
                var nextChunk = reader.ReadBlock(limit);

                subsequentChunks.Add(nextChunk);
                limit -= nextChunk.Length;
            }

            if (subsequentChunks.Count == 0)
            {
                return firstChunk;
            }

            var offset = firstChunk.Length;
            var total = offset;
            foreach (var chunk in subsequentChunks)
            {
                total += chunk.Length;
            }

            var result = new byte [total].AsMemory();
            firstChunk.CopyTo(result);

            foreach (var chunk in subsequentChunks)
            {
                chunk.CopyTo(result.Slice(offset));
                offset += chunk.Length;
            }

            return result;
        } // method ReadContinuous

        /// <summary>
        /// Чтение непрерывного блока памяти вплоть до указанного
        /// байта-разделителя.
        /// При необходимости куски склеиваются автоматически.
        /// </summary>
        /// <param name="reader">Читатель.</param>
        /// <param name="limiter">Символ-ограничитель.</param>
        /// <returns>Блок.</returns>
        public static Chunk ReadContinuousTo
            (
                this MemoryReader reader,
                byte limiter
            )
        {
            var firstUnit = reader.ReadTo(limiter);
            if (firstUnit.Chunk.IsEmpty || firstUnit.Found || reader.IsEof)
            {
                return firstUnit.Chunk;
            }

            var subsequentUnits = new SinglyLinkedList<Chunk>();
            while (!reader.IsEof)
            {
                var nextUnit = reader.ReadTo(limiter);

                subsequentUnits.Add(nextUnit.Chunk);
                if (nextUnit.Found)
                {
                    break;
                }
            }

            if (subsequentUnits.Count == 0)
            {
                return firstUnit.Chunk;
            }

            var offset = firstUnit.Chunk.Length;
            var total = offset;
            foreach (var chunk in subsequentUnits)
            {
                total += chunk.Length;
            }

            var result = new byte [total].AsMemory();
            firstUnit.Chunk.CopyTo(result);

            foreach (var chunk in subsequentUnits)
            {
                chunk.CopyTo(result.Slice(offset));
                offset += chunk.Length;
            }

            return result;
        } // method ReadContinuousTo

        #endregion

    } // class MemoryReaderUtility

} // namespace AM.IO
