// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* AsyncStreamUtility.cs -- асинхронная работа с потоками ввода-вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AM.Memory;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Асинхронная работа с потоками ввода-вывода.
/// </summary>
public static class AsyncStreamUtility
{
    #region Public methods

    /// <summary>
    /// Добавление содержимого первого потока (начиная с текущей позиции)
    /// в конец второго потока.
    /// </summary>
    public static async ValueTask AppendAsync
        (
            Stream sourceStream,
            Stream destinationStream,
            int chunkSize = 0,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (sourceStream);
        Sure.NotNull (destinationStream);

        if (chunkSize <= 0)
        {
            chunkSize = 4 * 1024;
        }

        using var owner = MemoryPool<byte>.Shared.Rent (chunkSize);
        var buffer = owner.Memory;
        destinationStream.Seek (0, SeekOrigin.End);
        while (true)
        {
            var read = await sourceStream.ReadAsync (buffer, cancellationToken);
            if (read <= 0)
            {
                break;
            }

            await destinationStream.WriteAsync (buffer[..read], cancellationToken);
        }
    }

    /// <summary>
    /// Побайтовое сравнение двух потоков <see cref="Stream"/>,
    /// начиная с текущей позиции.
    /// </summary>
    public static async ValueTask<int> CompareAsync
        (
            Stream firstStream,
            Stream secondStream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (firstStream);
        Sure.NotNull (secondStream);

        const int bufferSize = 1024;
        using var firstOwner = MemoryPool<byte>.Shared.Rent (bufferSize);
        using var secondOwner = MemoryPool<byte>.Shared.Rent (bufferSize);
        var firstBuffer = firstOwner.Memory;
        var secondBuffer = secondOwner.Memory;
        while (true)
        {
            var firstRead = await firstStream.ReadAsync (firstBuffer, cancellationToken);
            var secondRead = await secondStream.ReadAsync (secondBuffer, cancellationToken);
            var difference = firstRead - secondRead;
            if (difference != 0)
            {
                return difference;
            }

            if (firstRead == 0)
            {
                return 0;
            }

            for (var i = 0; i < firstRead; i++)
            {
                difference = firstBuffer.Span[i] - secondBuffer.Span[i];
                if (difference != 0)
                {
                    return difference;
                }
            }
        }
    }

    /// <summary>
    /// Считывание не более указанного количества байтов.
    /// </summary>
    public static async ValueTask<byte[]?> ReadBytesAsync
        (
            Stream stream,
            int count,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);
        Sure.Positive (count);

        using var buffer = MemoryPool<byte>.Shared.Rent (count);
        var result = buffer.Memory;
        var read = await stream.ReadAsync (result, cancellationToken);
        if (read <= 0)
        {
            return null;
        }

        return result[..read].ToArray();
    }

    /// <summary>
    /// Считывание значения типа <see cref="Int16"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<short> ReadInt16Async
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (short)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, short> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="UInt16"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<ushort> ReadUInt16Async
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (ushort)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, ushort> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="Int32"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<int> ReadInt32Async
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (int)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, int> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="UInt32"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<uint> ReadUInt32Async
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (uint)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, uint> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="Int64"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<long> ReadInt64Async
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (long)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, long> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="UInt64"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<ulong> ReadUInt64Async
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (ulong)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, ulong> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="Single"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<float> ReadSingleAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (float)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, float> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="Double"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<double> ReadDoubleAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (double)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, double> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="Decimal"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<decimal> ReadDecimalAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (decimal)];
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = MemoryMarshal.Cast<byte, decimal> (memory.Span)[0];

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="DateTime"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<DateTime> ReadDateTimeAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        var binary = await ReadInt64Async (stream, cancellationToken);

        return DateTime.FromBinary (binary);
    }

    /// <summary>
    /// Считывание значения типа <see cref="DateOnly"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<DateOnly> ReadDateOnlyAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        var dayNumber = await ReadInt32Async (stream, cancellationToken);

        return DateOnly.FromDayNumber (dayNumber);
    }

    /// <summary>
    /// Считывание значения типа <see cref="TimeOnly"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<TimeOnly> ReadTimeOnlyAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        var microseconds = await ReadDoubleAsync (stream, cancellationToken);

        return TimeOnly.FromTimeSpan (TimeSpan.FromMicroseconds (microseconds));
    }

    /// <summary>
    /// Считывание строки в указанной кодировке из потока
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<string> ReadStringAsync
        (
            Stream stream,
            Encoding encoding,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (encoding);

        var count = await ReadInt32Async (stream, cancellationToken);
        using var owner = MemoryPool<byte>.Shared.Rent (count);
        var memory = owner.Memory;
        await ReadExactAsync (stream, memory, cancellationToken);
        var result = encoding.GetString (memory.Span);

        return result;
    }

    /// <summary>
    /// Считывание строки в кодировке UTF-8 из потока в текущей позиции.
    /// </summary>
    public static ValueTask<string> ReadStringAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        return ReadStringAsync (stream, Encoding.UTF8, cancellationToken);
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="Int16"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask <short[]> ReadInt16ArrayAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        var length = await ReadInt32Async (stream, cancellationToken);
        var result = new short[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = await ReadInt16Async (stream, cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="UInt16"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask <ushort[]> ReadUInt16ArrayAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        var length = await ReadInt32Async (stream, cancellationToken);
        var result = new ushort[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = await ReadUInt16Async (stream, cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="Int32"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask <int[]> ReadInt32ArrayAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        var length = await ReadInt32Async (stream, cancellationToken);
        var result = new int[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = await ReadInt32Async (stream, cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="UInt32"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask <uint[]> ReadUInt32ArrayAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        var length = await ReadInt32Async (stream, cancellationToken);
        var result = new uint[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = await ReadUInt32Async (stream, cancellationToken);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива строк в указанной кодировке из потока
    /// в текущей позиции.
    /// </summary>
    public static async ValueTask<string[]> ReadStringArrayAsync
        (
            Stream stream,
            Encoding encoding,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (encoding);

        var length = await ReadInt32Async (stream, cancellationToken);
        var result = new string[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = await ReadStringAsync (stream, encoding, cancellationToken);
        }

        return result;
    }


    /// <summary>
    /// Чтение массива строк в кодировке UTF-8 из потока
    /// в текущей позиции.
    /// </summary>
    public static ValueTask<string[]> ReadStringArrayAsync
        (
            Stream stream,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        return ReadStringArrayAsync (stream, Encoding.UTF8, cancellationToken);
    }

    /// <summary>
    /// Чтение точно указанного числа байт из потока в текущей позиции.
    /// </summary>
    public static async ValueTask ReadExactAsync
        (
            Stream stream,
            Memory<byte> span,
            CancellationToken cancellationToken = default
        )
    {
        // TODO заменить на стандартное stream.ReadExactly ???

        Sure.NotNull (stream);

        if (!span.IsEmpty &&
            await stream.ReadAsync (span, cancellationToken) != span.Length)
        {
            Magna.Logger.LogError
                (
                    nameof (StreamUtility) + "::" + nameof (ReadExactAsync)
                    + ": unexpected end of stream"
                );

            throw new IOException ("Unexpected end of stream");
        }
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Boolean"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            bool value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof(bool)];
        memory.Span[0] = value ? (byte) 0 : (byte) 1;

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Int16"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            short value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (short)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="UInt16"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            ushort value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (ushort)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Int32"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            int value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (int)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="UInt32"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            uint value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (uint)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Int64"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            long value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (long)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="UInt64"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            ulong value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (ulong)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Single"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            float value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (float)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Double"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            double value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);

        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (double)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Decimal"/> в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            decimal value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);


        using var owner = MemoryPool<byte>.Shared.Rent (16);
        var memory = owner.Memory[..sizeof (decimal)];
        MemoryMarshal.Write (memory.Span, ref value);

        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток строки в указанной кодировке в текущей позиции.
    /// </summary>
    public static async ValueTask WriteAsync
        (
            Stream stream,
            string value,
            Encoding encoding,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (encoding);
        Sure.NotNull (value);

        var length = encoding.GetByteCount (value);
        using var owner = MemoryPool<byte>.Shared.Rent (length);
        var memory = owner.Memory[..length];
        encoding.GetBytes (value, memory.Span);

        await WriteAsync (stream, length, cancellationToken);
        await stream.WriteAsync (memory, cancellationToken);
    }

    /// <summary>
    /// Вывод в поток строки в кодировке UTF-8 в текущей позиции.
    /// </summary>
    public static ValueTask WriteAsync
        (
            Stream stream,
            string value,
            CancellationToken cancellationToken = default
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (value);

        return WriteAsync (stream, value, Encoding.UTF8, cancellationToken);
    }

    #endregion
}
