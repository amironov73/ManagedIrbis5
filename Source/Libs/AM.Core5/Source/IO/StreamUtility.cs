// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StreamUtility.cs -- работа с потоками ввода-вывода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Net;
using System.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Работа с потоками ввода-вывода.
/// </summary>
public static class StreamUtility
{
    #region Public methods

    /// <summary>
    /// Добавление содержимого первого потока (начиная с текущей позиции)
    /// в конец второго потока.
    /// </summary>
    public static unsafe void AppendTo
        (
            Stream sourceStream,
            Stream destinationStream,
            int chunkSize = 0
        )
    {
        Sure.NotNull (sourceStream);
        Sure.NotNull (destinationStream);

        if (chunkSize <= 0)
        {
            chunkSize = 4 * 1024;
        }

        var buffer = stackalloc byte[chunkSize];
        var span = new Span<byte> (buffer, chunkSize);
        destinationStream.Seek (0, SeekOrigin.End);
        while (true)
        {
            var read = sourceStream.Read (span);
            if (read <= 0)
            {
                break;
            }

            destinationStream.Write (span.Slice (0, read));
        }
    }

    /// <summary>
    /// Побайтовое сравнение двух потоков <see cref="Stream"/>,
    /// начиная с текущей позиции.
    /// </summary>
    public static unsafe int Compare
        (
            Stream firstStream,
            Stream secondStream
        )
    {
        Sure.NotNull (firstStream);
        Sure.NotNull (secondStream);

        const int bufferSize = 1024;
        var firstArray = stackalloc byte[bufferSize];
        var firstBuffer = new Span<byte> (firstArray, bufferSize);
        var secondArray = stackalloc byte[bufferSize];
        var secondBuffer = new Span<byte> (secondArray, bufferSize);
        while (true)
        {
            var firstRead = firstStream.Read (firstBuffer);
            var secondRead = secondStream.Read (secondBuffer);
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
                difference = firstBuffer[i] - secondBuffer[i];
                if (difference != 0)
                {
                    return difference;
                }
            }
        }
    }

    /// <summary>
    /// Считывание булевого <see cref="Boolean"/> значения из потока
    /// <see cref="Stream"/> в текущей позиции.
    /// </summary>
    public static bool ReadBoolean
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        return stream.ReadByte() switch
        {
            0 => false,
            -1 => throw new IOException(),
            _ => true
        };
    }

    /// <summary>
    /// Считывание не более указанного количества байтов.
    /// </summary>
    public static byte[]? ReadBytes
        (
            Stream stream,
            int count
        )
    {
        Sure.NotNull (stream);
        Sure.Positive (count);

        var result = new byte[count];
        var read = stream.Read (result, 0, count);
        if (read <= 0)
        {
            return null;
        }

        if (read != count)
        {
            Array.Resize (ref result, read);
        }

        return result;
    }

    /// <summary>
    /// Считывание значения типа <see cref="Int16"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static unsafe short ReadInt16
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        short value = default;
        var span = new Span<byte> ((byte*) &value, sizeof (short));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads <see cref="UInt16"/> value from the <see cref="Stream"/>.
    /// </summary>
    public static unsafe ushort ReadUInt16
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        ushort value = default;
        var span = new Span<byte> ((byte*)&value, sizeof (ushort));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads <see cref="Int32"/> value from the <see cref="Stream"/>.
    /// </summary>
    public static unsafe int ReadInt32
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        int value = default;
        var span = new Span<byte> ((byte*) &value, sizeof (int));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads <see cref="UInt32"/> value from the <see cref="Stream"/>.
    /// </summary>
    public static unsafe uint ReadUInt32
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        uint value = default;
        var span = new Span<byte> ((byte*) &value, sizeof (uint));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads <see cref="Int64"/> value from the <see cref="Stream"/>.
    /// </summary>
    public static unsafe long ReadInt64
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        long value = default;
        var span = new Span<byte> ((byte*) &value, sizeof (long));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads <see cref="UInt64"/> value from the <see cref="Stream"/>.
    /// </summary>
    public static unsafe ulong ReadUInt64
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        ulong value = default;
        var span = new Span<byte> ((byte*) &value, sizeof (ulong));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads <see cref="Single"/> value from the <see cref="Stream"/>.
    /// </summary>
    public static unsafe float ReadSingle
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        float value = default;
        var span = new Span<byte> ((byte*)&value, sizeof (float));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads <see cref="Double"/> value from the <see cref="Stream"/>.
    /// </summary>
    public static unsafe double ReadDouble
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        double value = default;
        var span = new Span<byte> ((byte*) &value, sizeof (double));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Считывание строки в указанной кодировке из потока
    /// в текущей позиции.
    /// </summary>
    public static string ReadString
        (
            Stream stream,
            Encoding encoding
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (encoding);

        var count = ReadInt32 (stream);
        var bytes = ReadExact (stream, count);
        var result = encoding.GetString (bytes);

        return result;
    }

    /// <summary>
    /// Считывание строки в кодировке UTF-8 из потока в текущей позиции.
    /// </summary>
    public static string ReadString
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        return ReadString (stream, Encoding.UTF8);
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="Int16"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static short[] ReadInt16Array
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var length = ReadInt32 (stream);
        var result = new short[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = ReadInt16 (stream);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="UInt16"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static ushort[] ReadUInt16Array
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var length = ReadInt32 (stream);
        var result = new ushort[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = ReadUInt16 (stream);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="Int32"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static int[] ReadInt32Array
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var length = ReadInt32 (stream);
        var result = new int[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = ReadInt32 (stream);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива значений типа <see cref="UInt32"/> из потока
    /// в текущей позиции.
    /// </summary>
    public static uint[] ReadUInt32Array
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var length = ReadInt32 (stream);
        var result = new uint[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = ReadUInt32 (stream);
        }

        return result;
    }

    /// <summary>
    /// Чтение массива строк в указанной кодировке из потока
    /// в текущей позиции.
    /// </summary>
    public static string[] ReadStringArray
        (
            Stream stream,
            Encoding encoding
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (encoding);

        var length = ReadInt32 (stream);
        var result = new string[length];
        for (var i = 0; i < length; i++)
        {
            result[i] = ReadString (stream, encoding);
        }

        return result;
    }

    /// <summary>
    /// Reads array of <see cref="String"/>'s from the <see cref="Stream"/>
    /// using UTF-8 <see cref="Encoding"/>.
    /// </summary>
    public static string[] ReadStringArray
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        return ReadStringArray (stream, Encoding.UTF8);
    }

    /// <summary>
    /// Считывание значения типа <see cref="Decimal"/> из потока <see cref="Stream"/>
    /// в текущей позиции.
    /// </summary>
    public static unsafe decimal ReadDecimal
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        decimal value = default;
        var span = new Span<byte> ((byte*)&value, sizeof (decimal));
        ReadExact (stream, span);
        return value;
    }

    /// <summary>
    /// Reads the date time.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public static DateTime ReadDateTime
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var binary = ReadInt64 (stream);

        return DateTime.FromBinary (binary);
    }

    /// <summary>
    /// Reads the date.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public static DateOnly ReadDateOnly
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var dayNumber = ReadInt32 (stream);

        return DateOnly.FromDayNumber (dayNumber);
    }

    /// <summary>
    /// Reads the time.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public static TimeOnly ReadTimeOnly
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        var microseconds = ReadDouble (stream);

        return TimeOnly.FromTimeSpan (TimeSpan.FromMicroseconds (microseconds));
    }

    /// <summary>
    /// Чтение точного числа байт.
    /// </summary>
    public static byte[] ReadExact
        (
            Stream stream,
            int length
        )
    {
        // TODO заменить на стандарт stream.ReadExactly

        Sure.NotNull (stream);
        Sure.NonNegative (length);

        var buffer = new byte[length];
        if (length != 0 &&
            stream.Read (buffer, 0, length) != length)
        {
            Magna.Logger.LogError
                (
                    nameof (StreamUtility) + "::" + nameof (ReadExact)
                    + ": unexpected end of stream"
                );

            throw new IOException ("Unexpected end of stream");
        }

        return buffer;
    }

    /// <summary>
    /// Чтение точного числа байт.
    /// </summary>
    public static void ReadExact
        (
            Stream stream,
            Span<byte> span
        )
    {
        // TODO заменить на стандартное stream.ReadExactly ???

        Sure.NotNull (stream);

        if (!span.IsEmpty &&
            stream.Read (span) != span.Length)
        {
            Magna.Logger.LogError
                (
                    nameof (StreamUtility) + "::" + nameof (ReadExact)
                    + ": unexpected end of stream"
                );

            throw new IOException ("Unexpected end of stream");
        }
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Boolean"/> в текущей позиции.
    /// </summary>
    public static void Write
        (
            Stream stream,
            bool value
        )
    {
        Sure.NotNull (stream);

        stream.WriteByte
            (
                value
                    ? (byte)1
                    : (byte)0
            );
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Int16"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            short value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (short));
        stream.Write (span);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="UInt16"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            ushort value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (ushort));
        stream.Write (span);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Int32"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            int value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (int));
        stream.Write (span);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="UInt32"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            uint value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (uint));
        stream.Write (span);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Int64"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            long value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (long));
        stream.Write (span);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="UInt64"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            ulong value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (ulong));
        stream.Write (span);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Single"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            float value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (float));
        stream.Write (span);
    }

    /// <summary>
    /// Вывод в поток значения типа <see cref="Double"/> в текущей позиции.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            double value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*)&value, sizeof (double));
        stream.Write (span);
    }

    /// <summary>
    /// Writes the <see cref="String"/> to the <see cref="Stream"/>
    /// using specified <see cref="Encoding"/>.
    /// </summary>
    public static void Write
        (
            Stream stream,
            string value,
            Encoding encoding
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (value);
        Sure.NotNull (encoding);

        var length = encoding.GetByteCount (value);
        using var owner = MemoryPool<byte>.Shared.Rent (length);
        var span = owner.Memory.Span[..length];
        encoding.GetBytes (value, span);
        Write (stream, length);
        stream.Write (span);
    }

    /// <summary>
    /// Writes the <see cref="String"/> to the <see cref="Stream"/>
    /// using UTF-8 <see cref="Encoding"/>.
    /// </summary>
    public static void Write
        (
            Stream stream,
            string value
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (value);

        Write (stream, value, Encoding.UTF8);
    }

    /// <summary>
    /// Writes the array of <see cref="Int16"/> to the <see cref="Stream"/>.
    /// </summary>
    public static void Write
        (
            Stream stream,
            short[] values
        )
    {
        // TODO ReadOnlySpan<short> ???

        Sure.NotNull (stream);
        Sure.NotNull (values);

        Write (stream, values.Length);

        for (var i = 0; i < values.Length; i++)
        {
            Write (stream, values[i]);
        }
    }

    /// <summary>
    /// Writes the array of <see cref="UInt16"/> to the <see cref="Stream"/>.
    /// </summary>
    public static void Write
        (
            Stream stream,
            ushort[] values
        )
    {
        // TODO ReadOnlySpan<ushort> ???

        Sure.NotNull (stream);
        Sure.NotNull (values);

        Write (stream, values.Length);

        for (var i = 0; i < values.Length; i++)
        {
            Write (stream, values[i]);
        }
    }

    /// <summary>
    /// Writes the array of <see cref="Int32"/> to the <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">Stream to write to.</param>
    /// <param name="values">Array of signed integer numbers.</param>
    /// <remarks>Value can be readed with
    /// <see cref="ReadInt32Array"/> or compatible method.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Either
    /// <paramref name="stream"/> or <paramref name="values"/>
    /// is <c>null</c>.</exception>
    /// <exception cref="IOException">An error during stream
    /// output happens.</exception>
    /// <see cref="Write(Stream,uint[])"/>
    /// <see cref="ReadInt32Array"/>
    public static void Write
        (
            Stream stream,
            int[] values
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (values);

        Write (stream, values.Length);

        for (var i = 0; i < values.Length; i++)
        {
            Write (stream, values[i]);
        }
    }

    /// <summary>
    /// Writes the array of <see cref="UInt32"/> to the <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">Stream to write to.</param>
    /// <param name="values">Array of unsigned integer numbers.</param>
    /// <remarks>Value can be readed with
    /// <see cref="ReadUInt32Array"/> or compatible method.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Either
    /// <paramref name="stream"/> or <paramref name="values"/>
    /// is <c>null</c>.</exception>
    /// <exception cref="IOException">An error during stream
    /// output happens.</exception>
    /// <seealso cref="Write(Stream,int[])"/>
    /// <see cref="ReadUInt32Array"/>
    public static void Write
        (
            Stream stream,
            uint[] values
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (values);

        Write (stream, values.Length);

        for (var i = 0; i < values.Length; i++)
        {
            Write (stream, values[i]);
        }
    }

    /// <summary>
    /// Writes the array of <see cref="String"/> to the <see cref="Stream"/>
    /// using specified <see cref="Encoding"/>.
    /// </summary>
    /// <param name="stream">Stream to write to.</param>
    /// <param name="values">Array of strings to write.</param>
    /// <param name="encoding">Encoding to use.</param>
    /// <remarks>Value can be readed with
    /// <see cref="ReadStringArray(Stream,Encoding)"/> or compatible method.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Either
    /// <paramref name="stream"/> or <paramref name="values"/>
    /// is <c>null</c>.</exception>
    /// <exception cref="IOException">An error during stream
    /// output happens.</exception>
    /// <seealso cref="Write(Stream,string[])"/>
    /// <see cref="ReadStringArray(Stream,Encoding)"/>
    public static void Write
        (
            Stream stream,
            string[] values,
            Encoding encoding
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (values);
        Sure.NotNull (encoding);

        Write (stream, values.Length);

        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < values.Length; i++)
        {
            Write (stream, values[i], encoding);
        }
    }

    /// <summary>
    /// Writes the array of <see cref="String"/> to the <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">Stream to write to.</param>
    /// <param name="values">Array of strings to write.</param>
    /// <remarks>Value can be readed with
    /// <see cref="ReadStringArray(Stream)"/> or compatible method.
    /// </remarks>
    /// <exception cref="ArgumentNullException">Either
    /// <paramref name="stream"/> or <paramref name="values"/>
    /// is <c>null</c>.</exception>
    /// <exception cref="IOException">An error during stream
    /// output happens.</exception>
    /// <seealso cref="Write(Stream,string[],Encoding)"/>
    /// <seealso cref="ReadStringArray(Stream)"/>
    public static void Write
        (
            Stream stream,
            string[] values
        )
    {
        Sure.NotNull (stream);
        Sure.NotNull (values);

        Write (stream, values, Encoding.UTF8);
    }

    /// <summary>
    /// Writes the <see cref="Decimal"/> to the specified <see cref="Stream"/>.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The value.</param>
    /// <remarks>Value can be readed with <see cref="ReadDecimal"/>
    /// or compatible method.</remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="stream"/> is <c>null</c>.</exception>
    /// <exception cref="IOException">Error during stream output
    /// happens.</exception>
    /// <seealso cref="ReadDecimal"/>
    public static unsafe void Write
        (
            Stream stream,
            decimal value
        )
    {
        Sure.NotNull (stream);

        var span = new Span<byte> ((byte*) &value, sizeof (decimal));
        stream.Write (span);
    }

    /// <summary>
    /// Writes the <see cref="DateTime"/> to the specified
    /// <see cref="Stream"/>.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            DateTime value
        )
    {
        Sure.NotNull (stream);

        var binary = value.ToBinary();
        var span = new Span<byte> ((byte*) &binary, sizeof (long));
        stream.Write (span);
    }

    /// <summary>
    /// Writes the <see cref="DateOnly"/> to the specified
    /// <see cref="Stream"/>.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            DateOnly value
        )
    {
        Sure.NotNull (stream);

        var binary = value.DayNumber;
        var span = new Span<byte> ((byte*) &binary, sizeof (int));
        stream.Write (span);
    }

    /// <summary>
    /// Writes the <see cref="TimeOnly"/> to the specified
    /// <see cref="Stream"/>.
    /// </summary>
    public static unsafe void Write
        (
            Stream stream,
            TimeOnly value
        )
    {
        Sure.NotNull (stream);

        var binary = value.ToTimeSpan().TotalMicroseconds;
        var span = new Span<byte> ((byte*) &binary, sizeof (double));
        stream.Write (span);
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    public static void NetworkToHost16
        (
            byte[] array,
            int offset
        )
    {
        Sure.NotNull (array);
        Sure.InRange (offset, array);
        Sure.InRange (offset + 1, array);

        if (BitConverter.IsLittleEndian)
        {
            (array[offset], array[offset + 1]) = (array[offset + 1], array[offset]);
        }
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    public static void NetworkToHost16
        (
            Span<byte> array,
            int offset
        )
    {
        Sure.InRange (offset, array);
        Sure.InRange (offset + 1, array);

        if (BitConverter.IsLittleEndian)
        {
            (array[offset], array[offset + 1]) = (array[offset + 1], array[offset]);
        }
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    public static unsafe void NetworkToHost16
        (
            byte* ptr
        )
    {
        Sure.NotNull (ptr);

        if (BitConverter.IsLittleEndian)
        {
            var temp = *ptr;
            *ptr = ptr[1];
            ptr[1] = temp;
        }
    }

    /// <summary>
    /// Преобразование беззнакового 32-битного числа из сетевой формы
    /// в локальную.
    /// </summary>
    public static uint NetworkToHost
        (
            uint value
        )
    {
        unchecked
        {
            return (uint) IPAddress.NetworkToHostOrder ((int) value);
        }
    }

    /// <summary>
    /// Преобразование беззнакового 32-битного числа из сетевой формы
    /// в локальную.
    /// </summary>
    public static uint HostToNetwork
        (
            uint value
        )
    {
        unchecked
        {
            return (uint) IPAddress.HostToNetworkOrder ((int) value);
        }
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    public static void NetworkToHost32
        (
            byte[] array,
            int offset
        )
    {
        Sure.NotNull (array);
        Sure.InRange (offset, array);

        if (BitConverter.IsLittleEndian)
        {
            var temp1 = array[offset];
            var temp2 = array[offset + 1];
            array[offset] = array[offset + 3];
            array[offset + 1] = array[offset + 2];
            array[offset + 3] = temp1;
            array[offset + 2] = temp2;
        }
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    public static void NetworkToHost32
        (
            Span<byte> span
        )
    {
        if (BitConverter.IsLittleEndian)
        {
            var temp1 = span[0];
            var temp2 = span[1];
            span[0] = span[3];
            span[1] = span[2];
            span[3] = temp1;
            span[2] = temp2;
        }
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    public static void HostToNetwork32
        (
            Span<byte> span
        )
    {
        if (BitConverter.IsLittleEndian)
        {
            var temp1 = span[0];
            var temp2 = span[1];
            span[0] = span[3];
            span[1] = span[2];
            span[3] = temp1;
            span[2] = temp2;
        }
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    public static unsafe void NetworkToHost32
        (
            byte* ptr
        )
    {
        Sure.NotNull (ptr);

        if (BitConverter.IsLittleEndian)
        {
            var temp1 = *ptr;
            var temp2 = ptr[1];
            *ptr = ptr[3];
            ptr[1] = ptr[2];
            ptr[3] = temp1;
            ptr[2] = temp2;
        }
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static long HostToNetwork
        (
            long value
        )
    {
        var bytes = BitConverter.GetBytes (value);
        HostToNetwork64 (bytes);
        var result = BitConverter.ToInt64 (bytes);

        return result;
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static long NetworkToHost
        (
            long value
        )
    {
        var bytes = BitConverter.GetBytes (value);
        NetworkToHost64 (bytes);
        var result = BitConverter.ToInt64 (bytes);

        return result;
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static ulong NetworkToHost
        (
            ulong value
        )
    {
        var bytes = BitConverter.GetBytes (value);
        NetworkToHost64 (bytes);
        var result = BitConverter.ToUInt64 (bytes);

        return result;
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static void NetworkToHost64
        (
            byte[] array,
            int offset
        )
    {
        NetworkToHost32 (array, offset);
        NetworkToHost32 (array, offset + 4);
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static void NetworkToHost64
        (
            Span<byte> span
        )
    {
        NetworkToHost32 (span);
        NetworkToHost32 (span.Slice (4));
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static unsafe void NetworkToHost64
        (
            byte* ptr
        )
    {
        Sure.NotNull (ptr);

        NetworkToHost32 (ptr);
        NetworkToHost32 (ptr + 4);
    }

    /// <summary>
    /// Host to network byte conversion.
    /// </summary>
    public static void HostToNetwork16
        (
            byte[] array,
            int offset
        )
    {
        Sure.NotNull (array);
        Sure.InRange (offset, array);
        Sure.InRange (offset + 1, array);

        if (BitConverter.IsLittleEndian)
        {
            (array[offset], array[offset + 1]) = (array[offset + 1], array[offset]);
        }
    }

    /// <summary>
    /// Host to network byte conversion.
    /// </summary>
    public static unsafe void HostToNetwork16
        (
            byte* ptr
        )
    {
        Sure.NotNull (ptr);

        if (BitConverter.IsLittleEndian)
        {
            var temp = *ptr;
            *ptr = ptr[1];
            ptr[1] = temp;
        }
    }

    /// <summary>
    /// Host to network byte conversion.
    /// </summary>
    public static void HostToNetwork32
        (
            byte[] array,
            int offset
        )
    {
        Sure.NotNull (array);
        Sure.InRange (offset, array);

        var temp1 = array[offset];
        var temp2 = array[offset + 1];
        array[offset] = array[offset + 3];
        array[offset + 1] = array[offset + 2];
        array[offset + 3] = temp1;
        array[offset + 2] = temp2;
    }

    /// <summary>
    /// Host to network byte conversion.
    /// </summary>
    public static unsafe void HostToNetwork32
        (
            byte* ptr
        )
    {
        Sure.NotNull (ptr);

        if (BitConverter.IsLittleEndian)
        {
            var temp1 = *ptr;
            var temp2 = ptr[1];
            *ptr = ptr[3];
            ptr[1] = ptr[2];
            ptr[3] = temp1;
            ptr[2] = temp2;
        }
    }

    /// <summary>
    /// Host to network byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static void HostToNetwork64
        (
            byte[] array,
            int offset
        )
    {
        Sure.NotNull (array);

        HostToNetwork32 (array, offset);
        HostToNetwork32 (array, offset + 4);
    }

    /// <summary>
    /// Network to host byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static void HostToNetwork64
        (
            Span<byte> span
        )
    {
        HostToNetwork32 (span);
        HostToNetwork32 (span.Slice (4));
    }

    /// <summary>
    /// Host to network byte conversion.
    /// </summary>
    /// <remarks>IRBIS64-oriented!</remarks>
    public static unsafe void HostToNetwork64
        (
            byte* ptr
        )
    {
        Sure.NotNull (ptr);

        HostToNetwork32 (ptr);
        HostToNetwork32 (ptr + 4);
    }

    /// <summary>
    /// Read integer in network byte order.
    /// </summary>
    public static unsafe short ReadInt16Network
        (
            this Stream stream
        )
    {
        Sure.NotNull (stream);

        var buffer = stackalloc byte[sizeof (short)];
        var span = new Span<byte> (buffer, sizeof (short));
        ReadExact (stream, span);

        return IPAddress.NetworkToHostOrder (*(short*)buffer);
    }

    /// <summary>
    /// Read integer in host byte order.
    /// </summary>
    public static unsafe short ReadInt16Host
        (
            this Stream stream
        )
    {
        Sure.NotNull (stream);

        var buffer = stackalloc byte[sizeof (short)];
        var span = new Span<byte> (buffer, sizeof (short));
        ReadExact (stream, span);
        return *(short*)buffer;
    }

    /// <summary>
    /// Read integer in network byte order.
    /// </summary>
    public static unsafe int ReadInt32Network
        (
            this Stream stream
        )
    {
        Sure.NotNull (stream);

        var buffer = stackalloc byte[sizeof (int)];
        var span = new Span<byte> (buffer, sizeof (int));
        ReadExact (stream, span);

        return IPAddress.NetworkToHostOrder (*(int*)buffer);
    }

    /// <summary>
    /// Read integer in host byte order.
    /// </summary>
    public static unsafe int ReadInt32Host
        (
            this Stream stream
        )
    {
        Sure.NotNull (stream);

        var buffer = stackalloc byte[sizeof (int)];
        var span = new Span<byte> (buffer, sizeof (int));
        ReadExact (stream, span);

        return *(int*)buffer;
    }

    /// <summary>
    /// Read integer in network byte order.
    /// </summary>
    public static unsafe long ReadInt64Network
        (
            this Stream stream
        )
    {
        Sure.NotNull (stream);

        var buffer = stackalloc byte[sizeof (long)];
        var span = new Span<byte> (buffer, sizeof (long));
        ReadExact (stream, span);
        NetworkToHost64 (buffer);

        return *(long*)buffer;
    }

    /// <summary>
    /// Read integer in host byte order.
    /// </summary>
    public static unsafe long ReadInt64Host
        (
            this Stream stream
        )
    {
        Sure.NotNull (stream);

        var buffer = stackalloc byte[sizeof (long)];
        var span = new Span<byte> (buffer, sizeof (long));
        ReadExact (stream, span);

        return *(long*)buffer;
    }

    /// <summary>
    /// Чтение потока до конца.
    /// </summary>
    /// <remarks>Полезно для считывания из сети (сервер высылает
    /// ответ, после чего закрывает соединение).</remarks>
    /// <param name="stream">Поток для чтения.</param>
    /// <returns>Массив прочитанных байт.</returns>
    public static byte[] ReadToEnd
        (
            this Stream stream
        )
    {
        Sure.NotNull (stream);

        using var result = new MemoryStream(); //-V3114
        var buffer = new byte[50 * 1024];
        while (true)
        {
            var read = stream.Read (buffer, 0, buffer.Length);
            if (read <= 0)
            {
                break;
            }

            result.Write (buffer, 0, read);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Write 16-bit integer to the stream in network byte order.
    /// </summary>
    public static unsafe void WriteInt16Network
        (
            this Stream stream,
            short value
        )
    {
        Sure.NotNull (stream);

        value = IPAddress.HostToNetworkOrder (value);
        var ptr = (byte*) &value;
        var span = new Span<byte> (ptr, sizeof (short));
        stream.Write (span);
    }

    /// <summary>
    /// Write 16-bit integer to the stream in network byte order.
    /// </summary>
    public static unsafe void WriteUInt16Network
        (
            this Stream stream,
            ushort value
        )
    {
        Sure.NotNull (stream);

        unchecked
        {
            value = (ushort) IPAddress.HostToNetworkOrder ((short) value);
            var ptr = (byte*) &value;
            var span = new Span<byte> (ptr, sizeof (ushort));
            stream.Write (span);
        }
    }

    /// <summary>
    /// Write 32-bit integer to the stream in network byte order.
    /// </summary>
    public static unsafe void WriteInt32Network
        (
            this Stream stream,
            int value
        )
    {
        Sure.NotNull (stream);

        value = IPAddress.HostToNetworkOrder (value);
        var ptr = (byte*) &value;
        var span = new Span<byte> (ptr, sizeof (int));
        stream.Write (span);
    }

    /// <summary>
    /// Write 32-bit integer to the stream in network byte order.
    /// </summary>
    public static unsafe void WriteUInt32Network
        (
            this Stream stream,
            uint value
        )
    {
        Sure.NotNull (stream);

        unchecked
        {
            value = (uint) IPAddress.HostToNetworkOrder ((int) value);
            var ptr = (byte*) &value;
            var span = new Span<byte> (ptr, sizeof (uint));
            stream.Write (span);
        }
    }

    /// <summary>
    /// Write 64-bit integer to the stream in network byte order.
    /// </summary>
    public static unsafe void WriteInt64Network
        (
            this Stream stream,
            long value
        )
    {
        Sure.NotNull (stream);

        var ptr = (byte*) &value;
        HostToNetwork64 (ptr);
        var span = new Span<byte> (ptr, sizeof (long));
        stream.Write (span);
    }

    /// <summary>
    /// Write 64-bit integer to the stream in network byte order.
    /// </summary>
    public static unsafe void WriteUInt64Network
        (
            this Stream stream,
            ulong value
        )
    {
        Sure.NotNull (stream);

        var ptr = (byte*) &value;
        HostToNetwork64 (ptr);
        var span = new Span<byte> (ptr, sizeof (ulong));
        stream.Write (span);
    }

    #endregion
}
