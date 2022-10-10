// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* SerializationUtility.cs -- возня вокруг сериализации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.IO.Compression;

using AM.IO;

#endregion

#nullable enable

namespace AM.Runtime;

/// <summary>
/// Возня вокруг сериализации.
/// </summary>
public static class SerializationUtility
{
    #region Public methods

    /// <summary>
    /// Чтение массива сериализованных объектов из потока.
    /// </summary>
    public static T[] RestoreArray<T>
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
    /// Чтение массива сериализованных объектов из потока.
    /// </summary>
    public static T[] RestoreArrayFromFile<T>
        (
            string fileName
        )
        where T : IHandmadeSerializable, new()
    {
        Sure.FileExists (fileName);

        using var stream = File.OpenRead (fileName);
        using var reader = new BinaryReader (stream);

        return reader.RestoreArray<T>();
    }

    /// <summary>
    /// Чтение массива сериализованных объектов из сжатого файла.
    /// </summary>
    public static T[] RestoreArrayFromZipFile<T>
        (
            string fileName
        )
        where T : IHandmadeSerializable, new()
    {
        Sure.FileExists (fileName);

        using Stream stream = File.OpenRead (fileName);
        using var deflate = new DeflateStream
            (
                stream,
                CompressionMode.Decompress
            );
        using var reader = new BinaryReader (deflate);
        return reader.RestoreArray<T>();
    }

    /// <summary>
    /// Считывание массива сериализованных объектов из памяти.
    /// </summary>
    public static T[] RestoreArrayFromMemory<T>
        (
            this byte[] array
        )
        where T : IHandmadeSerializable, new()
    {
        using Stream stream = new MemoryStream (array);
        using var reader = new BinaryReader (stream);

        return reader.ReadArray<T>();
    }

    /// <summary>
    /// Считывание массива сериализованных объектов из сжатой памяти.
    /// </summary>
    public static T[] RestoreArrayFromZipMemory<T>
        (
            this byte[] array
        )
        where T : IHandmadeSerializable, new()
    {
        using Stream stream = new MemoryStream (array);
        using var deflate = new DeflateStream
            (
                stream,
                CompressionMode.Decompress
            );
        using var reader = new BinaryReader (deflate);
        return reader.ReadArray<T>();
    }

    /// <summary>
    /// Считывание сериализованного nullable-объекта из потока.
    /// </summary>
    public static T? RestoreNullable<T>
        (
            this BinaryReader reader
        )
        where T : class, IHandmadeSerializable, new()
    {
        var isNull = !reader.ReadBoolean();
        if (isNull)
        {
            return null;
        }

        var result = new T();
        result.RestoreFromStream (reader);

        return result;
    }

    /// <summary>
    /// Read non-nullable object from the file.
    /// </summary>
    public static T RestoreObjectFromFile<T>
        (
            string fileName
        )
        where T : IHandmadeSerializable, new()
    {
        using var stream = File.OpenRead (fileName);
        using var reader = new BinaryReader (stream);
        var result = new T();
        result.RestoreFromStream (reader);

        return result;
    }

    /// <summary>
    /// Считывание объекта из файла.
    /// </summary>
    public static T RestoreObjectFromZipFile<T>
        (
            string fileName
        )
        where T : IHandmadeSerializable, new()
    {
        Sure.FileExists (fileName);

        using Stream stream = File.OpenRead (fileName);
        using var deflate = new DeflateStream
            (
                stream,
                CompressionMode.Decompress
            );
        using var reader = new BinaryReader (deflate);
        var result = new T();
        result.RestoreFromStream (reader);

        return result;
    }

    /// <summary>
    /// Read nullable object from the memory.
    /// </summary>
    public static T? RestoreObjectFromMemory<T>
        (
            this byte[] array
        )
        where T : class, IHandmadeSerializable, new()
    {
        using var stream = new MemoryStream (array);
        using var reader = new BinaryReader (stream);

        return reader.RestoreNullable<T>();
    }

    /// <summary>
    /// Считывание объекта из строки.
    /// </summary>
    public static T RestoreObjectFromString<T>
        (
            this string text
        )
        where T : class, IHandmadeSerializable, new()
    {
        var bytes = Convert.FromBase64String (text);
        var result = bytes.RestoreObjectFromZipMemory<T>();

        return result;
    }

    /// <summary>
    /// Считывание объекта из памяти.
    /// </summary>
    public static T RestoreObjectFromZipMemory<T>
        (
            this byte[] array
        )
        where T : class, IHandmadeSerializable, new()
    {
        using Stream stream = new MemoryStream (array);
        using var deflate = new DeflateStream (stream, CompressionMode.Decompress);
        using var reader = new BinaryReader (deflate);
        var result = new T();
        result.RestoreFromStream (reader);

        return result;
    }

    /// <summary>
    /// Сохранение в поток массива элементов.
    /// </summary>
    public static void SaveToStream<T>
        (
            this T[] array,
            BinaryWriter writer
        )
        where T : IHandmadeSerializable
    {
        writer.WritePackedInt32 (array.Length);
        foreach (var item in array)
        {
            item.SaveToStream (writer);
        }
    }

    /// <summary>
    /// Сохранение в поток массива элементов.
    /// </summary>
    public static void SaveToStream
        (
            this ReadOnlySpan<char> span,
            BinaryWriter writer
        )
    {
        writer.WritePackedInt32 (span.Length);
        writer.Write (span);
    }

    /// <summary>
    /// Сохранение в поток массива элементов.
    /// </summary>
    public static void SaveToStream
        (
            this ReadOnlyMemory<byte> memory,
            BinaryWriter writer
        )
    {
        writer.WritePackedInt32 (memory.Length);
        writer.Write (memory.Span);
    }

    /// <summary>
    /// Сохранение в поток массива элементов.
    /// </summary>
    public static void SaveToStream
        (
            this ReadOnlySpan<byte> span,
            BinaryWriter writer
        )
    {
        writer.WritePackedInt32 (span.Length);
        writer.Write (span);
    }

    /// <summary>
    /// Сохранение в файл объекта,
    /// умеющего сериализоваться вручную.
    /// </summary>
    public static void SaveToFile<T>
        (
            this T obj,
            string fileName
        )
        where T : class, IHandmadeSerializable, new()
    {
        Sure.NotNullNorEmpty (fileName);

        using Stream stream = File.Create (fileName);
        using var writer = new BinaryWriter (stream);
        obj.SaveToStream (writer);
    }

    /// <summary>
    /// Сохранение в файл объекта,
    /// умеющего сериализоваться вручную.
    /// </summary>
    public static void SaveToZipFile<T>
        (
            this T obj,
            string fileName
        )
        where T : class, IHandmadeSerializable, new()
    {
        Sure.NotNullNorEmpty (fileName);

        using Stream stream = File.Create (fileName);
        using var deflate = new DeflateStream (stream, CompressionMode.Compress);
        using var writer = new BinaryWriter (deflate);
        obj.SaveToStream (writer);
    }

    /// <summary>
    /// Сохранение в файл массива объектов,
    /// умеющих сериализоваться вручную.
    /// </summary>
    public static void SaveToFile<T>
        (
            this T[] array,
            string fileName
        )
        where T : IHandmadeSerializable
    {
        Sure.NotNullNorEmpty (fileName);

        using var stream = File.Create (fileName);
        using var writer = new BinaryWriter (stream);
        array.SaveToStream (writer);
    }

    /// <summary>
    /// Сериализация единичного объекта.
    /// </summary>
    public static byte[] SaveToMemory<T>
        (
            this T obj
        )
        where T : class, IHandmadeSerializable, new()
    {
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter (stream))
        {
            writer.WriteNullable (obj);
        }

        return stream.ToArray();
    }

    /// <summary>
    /// Сериализация массива объектов.
    /// </summary>
    public static byte[] SaveToMemory<T>
        (
            this T[] array
        )
        where T : IHandmadeSerializable, new()
    {
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter (stream))
        {
            writer.WriteArray (array);
        }

        return stream.ToArray();
    }

    /// <summary>
    /// Сохранение объекта в строке.
    /// </summary>
    public static string SaveToString<T>
        (
            this T obj
        )
        where T : class, IHandmadeSerializable, new()
    {
        var bytes = obj.SaveToZipMemory();
        var result = Convert.ToBase64String (bytes);

        return result;
    }

    /// <summary>
    /// Сохранение в файл массива объектов
    /// с одновременной упаковкой.
    /// </summary>
    public static void SaveToZipFile<T>
        (
            this T[] array,
            string fileName
        )
        where T : IHandmadeSerializable, new()
    {
        Sure.NotNullNorEmpty (fileName);

        using Stream stream = File.Create (fileName);
        using var deflate = new DeflateStream (stream, CompressionMode.Compress);
        using var writer = new BinaryWriter (deflate);
        writer.WriteArray (array);
    }

    /// <summary>
    /// Сохранение массива объектов.
    /// </summary>
    public static byte[] SaveToZipMemory<T>
        (
            this T obj
        )
        where T : class, IHandmadeSerializable, new()
    {
        using var stream = new MemoryStream();
        using var deflate = new DeflateStream
            (
                stream,
                CompressionMode.Compress
            );
        using (var writer = new BinaryWriter (deflate))
        {
            obj.SaveToStream (writer);
        }

        return stream.ToArray();
    }

    /// <summary>
    /// Сохранение массива объектов.
    /// </summary>
    public static byte[] SaveToZipMemory<T>
        (
            this T[] array
        )
        where T : IHandmadeSerializable, new()
    {
        using var stream = new MemoryStream();
        using var deflate = new DeflateStream
            (
                stream,
                CompressionMode.Compress
            );
        using (var writer = new BinaryWriter (deflate))
        {
            writer.WriteArray (array);
        }

        return stream.ToArray();
    }

    /// <summary>
    /// Сохранение в поток обнуляемого объекта.
    /// </summary>
    public static BinaryWriter WriteNullable<T>
        (
            this BinaryWriter writer,
            T? obj
        )
        where T : class, IHandmadeSerializable, new()
    {
        if (obj is null)
        {
            writer.Write (false);
        }
        else
        {
            writer.Write (true);
            obj.SaveToStream (writer);
        }

        return writer;
    }

    #endregion
}
