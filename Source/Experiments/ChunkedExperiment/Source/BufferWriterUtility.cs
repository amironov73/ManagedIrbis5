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
            var encoder = encoding.GetEncoder();
            while (!text.IsEmpty)
            {
                var span = writer.GetSpan();
                if (span.IsEmpty)
                {
                    throw new OutOfMemoryException();
                }

                var flushState = text.Length <= span.Length;
                try
                {
                    encoder.Convert(text, span, flushState, out var charsUsed, out var bytesUsed, out var _);
                    writer.Advance(bytesUsed);
                    text = text[charsUsed..];
                }
                catch (Exception)
                {
                    var bytes = encoding.GetBytes(text.ToString());
                    writer.Write(bytes.AsSpan());
                    break;
                }
            }

        } // method Write

        #endregion

    } // method BufferWriterUtility

} // namespace ChunkedExperiment
