// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BinaryWriterUtility.cs -- вспомогательные методы для записи двоичных данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Collections;
using AM.Runtime;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Вспомогательные методы для записи двоичных данных.
    /// </summary>
    public static class BinaryWriterUtility
    {
        #region Public methods

        /// <summary>
        /// Write the <see cref="NonNullCollection{T}"/>
        /// to the stream.
        /// </summary>
        public static BinaryWriter Write<T>
            (
                this BinaryWriter writer,
                NonNullCollection<T> collection
            )
            where T : class, IHandmadeSerializable, new()
        {
            writer.WriteArray(collection.ToArray());

            return writer;
        }

        /// <summary>
        /// Write nullable 8-bit integer.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                byte? value
            )
        {
            if (value.HasValue)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable 16-bit integer.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                short? value
            )
        {
            if (value.HasValue)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable 32-bit integer.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                int? value
            )
        {
            if (value.HasValue)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable 64-bit integer.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                long? value
            )
        {
            if (value.HasValue)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable decimal number.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                decimal? value
            )
        {
            if (value.HasValue)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write <see cref="DateTime"/>.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                DateTime value
            )
        {
            var ticks = value.ToBinary();
            writer.Write(ticks);

            return writer;
        }

        /// <summary>
        /// Write nullable DateTime.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                DateTime? value
            )
        {
            if (value.HasValue)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable double precision number.
        /// </summary>
        public static BinaryWriter Write
            (
                this BinaryWriter writer,
                double? value
            )
        {
            if (value.HasValue)
            {
                writer.Write(true);
                writer.Write(value.Value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write array of bytes.
        /// </summary>
        public static BinaryWriter WriteArray
            (
                this BinaryWriter writer,
                byte[] array
            )
        {
            writer.WritePackedInt32(array.Length);
            writer.Write(array);

            return writer;
        }

        /// <summary>
        /// Write array of 16-bit integers.
        /// </summary>
        public static BinaryWriter WriteArray
            (
                this BinaryWriter writer,
                short[] array
            )
        {
            writer.WritePackedInt32(array.Length);
            for (var i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write array of 32-bit integers.
        /// </summary>
        public static BinaryWriter WriteArray
            (
                this BinaryWriter writer,
                int[] array
            )
        {
            writer.WritePackedInt32(array.Length);
            for (var i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write array of 64-bit integers.
        /// </summary>
        public static BinaryWriter WriteArray
            (
                this BinaryWriter writer,
                long[] array
            )
        {
            writer.WritePackedInt32(array.Length);
            for (var i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write array of strings.
        /// </summary>
        public static BinaryWriter WriteArray
            (
                this BinaryWriter writer,
                string[] array
            )
        {
            writer.WritePackedInt32(array.Length);
            for (var i = 0; i < array.Length; i++)
            {
                writer.Write(array[i]);
            }

            return writer;
        }

        /// <summary>
        /// Write the array.
        /// </summary>
        public static BinaryWriter WriteArray<T>
            (
                this BinaryWriter writer,
                T[] array
            )
            where T : IHandmadeSerializable, new()
        {
            writer.WritePackedInt32(array.Length);
            for (var i = 0; i < array.Length; i++)
            {
                var item = array[i];
                item.SaveToStream(writer);
            }

            return writer;
        }

        /// <summary>
        /// Write the list to the stream.
        /// </summary>
        public static BinaryWriter WriteList<T>
            (
                this BinaryWriter writer,
                List<T> list
            )
            where T : IHandmadeSerializable, new()
        {
            writer.WritePackedInt32(list.Count);
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                item.SaveToStream(writer);
            }

            return writer;
        }

        /// <summary>
        /// Write nullable string.
        /// </summary>
        public static BinaryWriter WriteNullable
            (
                this BinaryWriter writer,
                string? value
            )
        {
            if (!ReferenceEquals(value, null))
            {
                writer.Write(true);
                writer.Write(value);
            }
            else
            {
                writer.Write(false);
            }

            return writer;
        }

        /// <summary>
        /// Write array of 32-bit integers.
        /// </summary>
        public static BinaryWriter WriteNullableArray
            (
                this BinaryWriter writer,
                int[]? array
            )
        {
            if (ReferenceEquals(array, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.WritePackedInt32(array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    writer.Write(array[i]);
                }
            }

            return writer;
        }

        /// <summary>
        /// Write array of 32-bit integers.
        /// </summary>
        public static BinaryWriter WriteNullableArray
            (
                this BinaryWriter writer,
                string[]? array
            )
        {
            if (ReferenceEquals(array, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.WritePackedInt32(array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    writer.Write(array[i]);
                }
            }

            return writer;
        }

        /// <summary>
        /// Write array of objects.
        /// </summary>
        public static BinaryWriter WriteNullableArray<T>
            (
                this BinaryWriter writer,
                T[]? array
            )
            where T : IHandmadeSerializable
        {
            if (ReferenceEquals(array, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                writer.WritePackedInt32(array.Length);
                for (var i = 0; i < array.Length; i++)
                {
                    array[i].SaveToStream(writer);
                }
            }

            return writer;
        }

        /// <summary>
        /// Write 32-bit integer in packed format.
        /// </summary>
        /// <remarks>Borrowed from
        /// http://referencesource.microsoft.com/
        /// </remarks>
        public static BinaryWriter WritePackedInt32
            (
                this BinaryWriter writer,
                int value
            )
        {
            unchecked
            {
                var v = (uint)value;
                while (v >= 0x80)
                {
                    writer.Write((byte)(v | 0x80));
                    v >>= 7;
                }
                writer.Write((byte)v);

                return writer;
            }
        }

        /// <summary>
        /// Write 64-bit integer in packed format.
        /// </summary>
        /// <remarks>Inspired by
        /// http://referencesource.microsoft.com/
        /// </remarks>
        public static BinaryWriter WritePackedInt64
            (
                this BinaryWriter writer,
                long value
            )
        {
            unchecked
            {
                var v = (ulong)value;
                while (v >= 0x80)
                {
                    writer.Write((byte)(v | 0x80));
                    v >>= 7;
                }
                writer.Write((byte)v);

                return writer;
            }
        }

        #endregion
    }
}
