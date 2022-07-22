// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BinaryReader.cs -- вспомогательные методы для чтения двоичных данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM.Collections;
using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Вспомогательные методы для чтения двоичных данных.
/// </summary>
public static class BinaryReaderUtility
{
    #region Public methods

    /// <summary>
    /// Read <see cref="NonNullCollection{T}"/>
    /// </summary>
    public static NonNullCollection<T> ReadNonNullCollection<T>
        (
            this BinaryReader reader
        )
        where T : class, IHandmadeSerializable, new()
    {
        var array = reader.ReadArray<T>();
        var result = new NonNullCollection<T>();
        result.AddRange (array);

        return result;
    }

    /// <summary>
    /// Read array from stream
    /// </summary>
    public static T[] ReadArray<T>
        (
            this BinaryReader reader
        )
        where T : IHandmadeSerializable, new()
    {
        var count = reader.ReadPackedInt32();
        var result = new T[count];
        for (var i = 0; i < count; i++)
        {
            var item = new T();
            item.RestoreFromStream (reader);
            result[i] = item;
        }

        return result;
    }

    /// <summary>
    /// Read array of bytes.
    /// </summary>
    public static byte[] ReadByteArray
        (
            this BinaryReader reader
        )
    {
        var length = reader.ReadPackedInt32();
        var result = new byte[length];
        reader.Read (result, 0, length);

        return result;
    }

    /// <summary>
    /// Reads collection of items from the stream.
    /// </summary>
    public static BinaryReader ReadCollection<T>
        (
            this BinaryReader reader,
            NonNullCollection<T> collection
        )
        where T : class, IHandmadeSerializable, new()
    {
        collection.Clear();

        var count = reader.ReadPackedInt32();
        for (var i = 0; i < count; i++)
        {
            var item = new T();
            item.RestoreFromStream (reader);
            collection.Add (item);
        }

        return reader;
    }

    /// <summary>
    /// Read <see cref="DateTime"/> from the stream.
    /// </summary>
    public static DateTime ReadDateTime
        (
            this BinaryReader reader
        )
    {
        var result = DateTime.FromBinary (reader.ReadInt64());

        return result;
    }

    /// <summary>
    /// Read <see cref="DateTime"/> from the stream.
    /// </summary>
    public static DateTime? ReadNullableDateTime
        (
            this BinaryReader reader
        )
    {
        DateTime? result = null;

        var flag = reader.ReadBoolean();
        if (flag)
        {
            result = reader.ReadDateTime();
        }

        return result;
    }

    /// <summary>
    /// Read array of 16-bit integers.
    /// </summary>
    public static short[] ReadInt16Array
        (
            this BinaryReader reader
        )
    {
        var length = reader.ReadPackedInt32();
        var result = new short[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = reader.ReadInt16();
        }

        return result;
    }

    /// <summary>
    /// Read array of 32-bit integers.
    /// </summary>
    public static int[] ReadInt32Array
        (
            this BinaryReader reader
        )
    {
        var length = reader.ReadPackedInt32();
        var result = new int[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = reader.ReadInt32();
        }

        return result;
    }

    /// <summary>
    /// Read array of 64-bit integers.
    /// </summary>
    public static long[] ReadInt64Array
        (
            this BinaryReader reader
        )
    {
        var length = reader.ReadPackedInt32();
        var result = new long[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = reader.ReadInt64();
        }

        return result;
    }

    /// <summary>
    /// Создание с последующим чтением списка сериализованных
    /// элементов из потока.
    /// </summary>
    public static List<T> ReadList<T>
        (
            this BinaryReader reader
        )
        where T : IHandmadeSerializable, new()
    {
        var count = reader.ReadPackedInt32();
        var result = new List<T> (count);
        for (var i = 0; i < count; i++)
        {
            var item = new T();
            item.RestoreFromStream (reader);
            result.Add (item);
        }

        return result;
    }

    /// <summary>
    /// Чтение списка сериализованных элементов из потока.
    /// </summary>
    public static BinaryReader ReadList<T>
        (
            this BinaryReader reader,
            List<T> list
        )
        where T : IHandmadeSerializable, new()
    {
        var count = reader.ReadPackedInt32();
        for (var i = 0; i < count; i++)
        {
            var item = new T();
            item.RestoreFromStream (reader);
            list.Add (item);
        }

        return reader;
    }

    /// <summary>
    /// Считывание строки в виде ReadOnlyMemory.
    /// </summary>
    public static ReadOnlyMemory<char> ReadOnlyMemory
        (
            this BinaryReader reader
        )
    {
        var length = reader.ReadPackedInt32();
        var result = new char[length];
        var read = reader.Read (result, 0, length);
        if (read != length)
        {
            throw new IOException();
        }

        return result.AsMemory();
    }

    /// <summary>
    /// Read nullable byte.
    /// </summary>
    public static byte? ReadNullableByte
        (
            this BinaryReader reader
        )
    {
        var flag = reader.ReadBoolean();
        return flag
            ? (byte?)reader.ReadByte()
            : null;
    }

    /// <summary>
    /// Read nullable double precision number.
    /// </summary>
    public static double? ReadNullableDouble
        (
            this BinaryReader reader
        )
    {
        var flag = reader.ReadBoolean();
        return flag
            ? (double?)reader.ReadDouble()
            : null;
    }

    /// <summary>
    /// Read nullable decimal.
    /// </summary>
    public static decimal? ReadNullableDecimal
        (
            this BinaryReader reader
        )
    {
        var flag = reader.ReadBoolean();

        return flag
            ? (decimal?)reader.ReadDecimal()
            : null;
    }

    /// <summary>
    /// Read nullable 16-bit integer.
    /// </summary>
    public static short? ReadNullableInt16
        (
            this BinaryReader reader
        )
    {
        var flag = reader.ReadBoolean();
        return flag
            ? (short?)reader.ReadInt16()
            : null;
    }

    /// <summary>
    /// Read nullable 32-bit integer.
    /// </summary>
    public static int? ReadNullableInt32
        (
            this BinaryReader reader
        )
    {
        var flag = reader.ReadBoolean();
        return flag
            ? (int?)reader.ReadInt32()
            : null;
    }

    /// <summary>
    /// Read array of 32-bit integers.
    /// </summary>
    public static int[]? ReadNullableInt32Array
        (
            this BinaryReader reader
        )
    {
        var isNull = !reader.ReadBoolean();
        if (isNull)
        {
            return null;
        }

        var length = reader.ReadPackedInt32();
        var result = new int[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = reader.ReadInt32();
        }

        return result;
    }


    /// <summary>
    /// Read nullable 64-bit integer.
    /// </summary>
    public static long? ReadNullableInt64
        (
            this BinaryReader reader
        )
    {
        var flag = reader.ReadBoolean();
        return flag
            ? (long?)reader.ReadInt64()
            : null;
    }

    /// <summary>
    /// Read nullable string.
    /// </summary>
    public static string? ReadNullableString
        (
            this BinaryReader reader
        )
    {
        var flag = reader.ReadBoolean();
        return flag
            ? reader.ReadString()
            : null;
    }

    /// <summary>
    /// Read nullable array of strings.
    /// </summary>
    public static string[]? ReadNullableStringArray
        (
            this BinaryReader reader
        )
    {
        string[]? result = null;
        if (reader.ReadBoolean())
        {
            var count = reader.ReadPackedInt32();
            result = new string[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = reader.ReadString();
            }
        }

        return result;
    } // method ReadNullableStringArray

    /// <summary>
    /// Read nullable array of strings.
    /// </summary>
    public static ReadOnlyMemory<char>[]? ReadNullableReadOnlyMemoryArray
        (
            this BinaryReader reader
        )
    {
        ReadOnlyMemory<char>[]? result = null;
        if (reader.ReadBoolean())
        {
            var count = reader.ReadPackedInt32();
            result = new ReadOnlyMemory<char>[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = reader.ReadOnlyMemory();
            }
        }

        return result;
    } // method ReadNullableReadOnlyMemoryArray

    /// <summary>
    /// Read nullable array of objects.
    /// </summary>
    public static T[]? ReadNullableArray<T>
        (
            this BinaryReader reader
        )
        where T : IHandmadeSerializable, new()
    {
        T[]? result = null;

        if (reader.ReadBoolean())
        {
            var count = reader.ReadPackedInt32();
            result = new T[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = new T();
                result[i].RestoreFromStream (reader);
            }
        }

        return result;
    }

    /// <summary>
    /// Read 32-bit integer in packed format.
    /// </summary>
    /// <remarks>Borrowed from
    /// http://referencesource.microsoft.com/
    /// </remarks>
    public static int ReadPackedInt32
        (
            this BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        unchecked
        {
            var count = 0;
            var shift = 0;
            byte b;
            do
            {
                if (shift == 5 * 7)
                {
                    Magna.Logger.LogError
                        (
                            nameof (BinaryReaderUtility) + "::" + nameof (ReadPackedInt32)
                            + ": value too long"
                        );

                    throw new FormatException();
                }

                b = reader.ReadByte();
                count |= (b & 0x7F) << shift; //-V3134
                shift += 7;
            } while ((b & 0x80) != 0);

            return count;
        }
    }

    /// <summary>
    /// Read 64-bit integer in packed format.
    /// </summary>
    /// <remarks>Inspired by
    /// http://referencesource.microsoft.com/
    /// </remarks>
    public static long ReadPackedInt64
        (
            this BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        unchecked
        {
            long count = 0;
            var shift = 0;
            long b;
            do
            {
                b = reader.ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);

            return count;
        }
    }

    /// <summary>
    /// Read string with given length.
    /// </summary>
    public static string ReadString
        (
            this BinaryReader reader,
            int count
        )
    {
        Sure.Positive (count);

        var characters = reader.ReadChars (count);
        var result = new string (characters);

        return result;
    }

    /// <summary>
    /// Read array of strings.
    /// </summary>
    public static string[] ReadStringArray
        (
            this BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        var length = reader.ReadPackedInt32();
        var result = new string[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = reader.ReadString();
        }

        return result;
    }

    #endregion
}
