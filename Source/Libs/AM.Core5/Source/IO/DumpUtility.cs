// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DumpUtility.cs -- методы для дампа объектов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Методы для дампа объектов.
    /// </summary>
    public static class DumpUtility
    {
        #region Public methods

        /// <summary>
        /// Dump the array of data.
        /// </summary>
        public static void Dump<T>
            (
                TextWriter writer,
                T[] data
            )
            where T : struct
        {
            var format = " {0:X8}";

            if (data is byte[]
                || data is sbyte[])
            {
                format = " {0:X2}";
            }

            if (data is long[]
                || data is ulong[])
            {
                format = " {0:X16}";
            }

            var begin = true;
            for (var offset = 0; offset < data.Length; offset++)
            {
                if (offset % 16 == 0)
                {
                    if (!begin)
                    {
                        writer.WriteLine();
                    }
                    else
                    {
                        begin = false;
                    }
                    writer.Write("{0:X6}> ", offset);
                }

                if (offset % 4 == 0)
                {
                    writer.Write(" ");
                }

                var item = data[offset];
                writer.Write(format, item);
            }

            writer.WriteLine();
        }

        /// <summary>
        /// Dump the array of data.
        /// </summary>
        public static void Dump<T>
            (
                Stream stream,
                T[] data
            )
            where T : struct
        {
            var writer = new StreamWriter(stream, Encoding.Default);
            Dump(writer, data);
        }

        /// <summary>
        /// Dump the array of data to console output.
        /// </summary>
        public static void DumpToConsole<T>
            (
                T[] data
            )
            where T : struct
        {
            Dump(Console.Out, data);
        }

        //        /// <summary>
        //        /// Dump the array of data to <see cref="AbstractOutput"/>.
        //        /// </summary>
        //        public static void DumpToOutput<T>
        //            (
        //                AbstractOutput output,
        //                T[] data
        //            )
        //            where T : struct
        //        {
        //            string text = DumpToText(data);
        //            output.WriteLine(text);
        //        }

        /// <summary>
        /// Dump the array of data to string.
        /// </summary>
        public static string DumpToText<T>
            (
                T[] data
            )
            where T : struct
        {
            var writer = new StringWriter();
            Dump
                (
                    writer,
                    data
                );

            return writer.ToString();
        }

        #endregion
    }
}
