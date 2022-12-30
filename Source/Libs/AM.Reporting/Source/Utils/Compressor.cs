// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    internal static class Compressor
    {
        public static Stream Decompress (Stream source, bool bidiStream)
        {
            if (IsStreamCompressed (source))
            {
                if (bidiStream)
                {
                    // create bidirectional stream
                    Stream stream = new MemoryStream();
                    using (var gzip = new GZipStream (source, CompressionMode.Decompress))
                    {
                        const int BUFFER_SIZE = 4096;
                        gzip.CopyTo (stream, BUFFER_SIZE);
                    }

                    stream.Position = 0;
                    return stream;
                }
                else
                {
                    return new GZipStream (source, CompressionMode.Decompress);
                }
            }

            return null;
        }

        public static Stream Compress (Stream dest)
        {
            return new GZipStream (dest, CompressionMode.Compress, true);
        }

        public static byte[] Compress (byte[] buffer)
        {
            using (var dest = new MemoryStream())
            {
                using (Stream gzipStream = new GZipStream (dest, CompressionMode.Compress, true))
                {
                    gzipStream.Write (buffer, 0, buffer.Length);
                }

                return dest.ToArray();
            }
        }

        public static string Compress (string source)
        {
            var srcBytes = Encoding.UTF8.GetBytes (source);
            var compressedBytes = Compress (srcBytes);
            return Convert.ToBase64String (compressedBytes);
        }

        public static byte[] Decompress (byte[] buffer)
        {
            using (var ms = new MemoryStream (buffer))
            {
                if (IsStreamCompressed (ms))
                {
                    using (var uncompressedStream = Decompress (ms, true) as MemoryStream)
                    {
                        return uncompressedStream.ToArray();
                    }
                }
                else
                {
                    return buffer;
                }
            }
        }

        public static string Decompress (string source)
        {
            var srcBytes = Convert.FromBase64String (source);
            var decompressedBytes = Decompress (srcBytes);
            return Encoding.UTF8.GetString (decompressedBytes);
        }

        public static bool IsStreamCompressed (Stream stream)
        {
            var byte1 = stream.ReadByte();
            var byte2 = stream.ReadByte();
            stream.Position -= 2;
            return byte1 == 0x1F && byte2 == 0x8B;
        }
    }
}
