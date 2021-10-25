// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CompressionUtility.cs -- работа со сжатыми данными
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.IO.Compression;

#endregion

namespace AM.IO
{
    /// <summary>
    /// Работа со сжатыми данными.
    /// </summary>
    public static class CompressionUtility
    {
        #region Public methods

        /// <summary>
        /// Сжатие указанных данных.
        /// </summary>
        public static byte[] Compress
            (
                byte[] data
            )
        {
            var memory = new MemoryStream();
            using (var compressor = new DeflateStream
                (
                    memory,
                    CompressionMode.Compress
                ))
            {
                compressor.Write (data, 0, data.Length);
            }

            return memory.ToArray();

        } // method Compress

        /// <summary>
        /// Распаковка указанных данных.
        /// </summary>
        public static byte[] Decompress
            (
                byte[] data
            )
        {
            var memory = new MemoryStream (data);
            var result = new MemoryStream();
            using (var decompresser = new DeflateStream
                (
                    memory,
                    CompressionMode.Decompress
                ))
            {
                StreamUtility.AppendTo
                    (
                        decompresser,
                        result
                    );
            }

            return result.ToArray();

        } // method Decompress

        #endregion

    } // class CompressionUtility

} // namespace AM.IO
