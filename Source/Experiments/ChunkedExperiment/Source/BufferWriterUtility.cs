// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BufferWriterUtility.cs -- методы расширения для IBufferWriter
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Text;

#endregion

#nullable enable

namespace ChunkedExperiment
{
    /// <summary>
    /// Методы расширения для <see cref="IBufferWriter{T}"/>
    /// </summary>
    public static class BufferWriterUtility
    {
        #region Public methods

        /// <summary>
        /// Запись одного байта.
        /// </summary>
        public static void Write
            (
                this IBufferWriter<byte> writer,
                byte value
            )
        {
            var span = writer.GetSpan();
            if (span.IsEmpty)
            {
                throw new OutOfMemoryException();
            }

            span[0] = value;
            writer.Advance(1);

        } // method Write

        /// <summary>
        /// Запись в буфер строки в указанной кодировке.
        /// </summary>
        public static void Write
            (
                this IBufferWriter<byte> writer,
                ReadOnlySpan<char> text,
                Encoding encoding
            )
        {
            Span<byte> smallBuffer = stackalloc byte[10];
            var encoder = encoding.GetEncoder();
            while (!text.IsEmpty)
            {
                var span = writer.GetSpan();
                if (span.IsEmpty)
                {
                    throw new OutOfMemoryException();
                }

                if (!encoding.IsSingleByte && span.Length < 4)
                {
                    // если у нас вдруг UTF-8 или другая "широкая" кодировка
                    // и может оказаться мало байт для хотя бы одного символа
                    // В тексте UTF-8 любой байт со значением меньше 128 изображает
                    // символ ASCII с тем же кодом. Остальные символы Юникода
                    // изображаются последовательностями длиной от 2 до 6 байт
                    // (реально только до 4 байт, поскольку использование кодов
                    // больше 2^21 не планируется), в которых первый байт всегда
                    // имеет вид 11xxxxxx, а остальные — 10xxxxxx.

                    encoder.Convert(text, smallBuffer, false, out var charsUsed, out var bytesUsed, out var _);
                    writer.Write(smallBuffer[0..bytesUsed]);
                    text = text[charsUsed..];
                }
                else
                {
                    // можно копировать напрямую в буфер
                    // прикидываем, нужно ли делать flush

                    var remaining = encoder.GetByteCount(text, false);
                    var flushState = span.Length < remaining;
                    encoder.Convert(text, span, flushState, out var charsUsed, out var bytesUsed, out var _);
                    writer.Advance(bytesUsed);
                    text = text[charsUsed..];
                }
            }

        } // method Write

        #endregion

    } // method BufferWriterUtility

} // namespace ChunkedExperiment
