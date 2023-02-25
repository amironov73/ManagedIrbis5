// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

/* Utf8StringBuilder.cs -- собирает строку в кодировке UTF-8
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Buffers.Text;

/// <summary>
/// Собирает строку в кодировке UTF-8.
/// </summary>
[PublicAPI]
public partial struct Utf8ValueStringBuilder
    : IDisposable, IBufferWriter<byte>, IResettableBufferWriter<byte>
{
    #region Constants

    private const int ThreadStaticBufferSize = 64444;
    private const int DefaultBufferSize = 65536; // use 64K default buffer.

    #endregion
    
    #region Delegates

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public delegate bool TryFormat<in T> 
        (
            T value,
            Span<byte> destination,
            out int written,
            StandardFormat format
        );
    
    #endregion

    #region Nested classes

    /// <summary>
    /// Кеш.
    /// </summary>
    public static class FormatterCache<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public static TryFormat<T> TryFormatDelegate;

        static FormatterCache()
        {
            var formatter = (TryFormat<T>?)CreateFormatter (typeof (T));
            if (formatter == null)
            {
                if (typeof (T).IsEnum)
                {
                    formatter = EnumUtil<T>.TryFormatUtf8;
                }
                else
                {
                    formatter = TryFormatDefault;
                }
            }

            TryFormatDelegate = formatter;
        }

        private static bool TryFormatDefault
            (
                T value,
                Span<byte> dest,
                out int written,
                StandardFormat format
            )
        {
            if (value == null)
            {
                written = 0;
                return true;
            }

            var s = typeof (T) == typeof (string)
                ? Unsafe.As<string> (value)
                :
                (value is IFormattable formattable && format != default)
                    ?
                    formattable.ToString (format.ToString(), null)
                    :
                    value.ToString();

            // also use this length when result is false.
            written = UTF8NoBom.GetMaxByteCount (s.ThrowIfNull().Length);
            if (dest.Length < written)
            {
                return false;
            }

            written = UTF8NoBom.GetBytes (s.AsSpan(), dest);
            return true;
        }
    }
    
    #endregion

    static Utf8ValueStringBuilder()
    {
        var newLine = UTF8NoBom.GetBytes (Environment.NewLine);
        if (newLine.Length == 1)
        {
            // cr or lf
            _newLine1 = newLine[0];
            _crlf = false;
        }
        else
        {
            // crlf(windows)
            _newLine1 = newLine[0];
            _newLine2 = newLine[1];
            _crlf = true;
        }
    }

    #region Private members

    private static readonly Encoding UTF8NoBom = new UTF8Encoding (false);

    private static readonly byte _newLine1;
    private static readonly byte _newLine2;
    private static readonly bool _crlf;
    
    [ThreadStatic] private static byte[]? _scratchBuffer;
    [ThreadStatic] internal static bool scratchBufferUsed;
    
    private byte[]? _buffer;
    private int _index;
    private readonly bool _disposeImmediately;

    static TryFormat<T?> CreateNullableFormatter<T>() 
        where T: struct
    {
        return (T? x, Span<byte> destination, out int written, StandardFormat format) =>
        {
            if (x == null)
            {
                written = 0;
                return true;
            }

            return FormatterCache<T>.TryFormatDelegate (x.Value, destination, out written, format);
        };
    }

    #endregion

    #region Public methods

    /// <summary>Length of written buffer.</summary>
    public int Length => _index;

    /// <summary>Get the written buffer data.</summary>
    public ReadOnlySpan<byte> AsSpan() => _buffer.AsSpan (0, _index);

    /// <summary>Get the written buffer data.</summary>
    public ReadOnlyMemory<byte> AsMemory() => _buffer.AsMemory (0, _index);

    /// <summary>Get the written buffer data.</summary>
    public ArraySegment<byte> AsArraySegment() => new (_buffer.ThrowIfNull(), 0, _index);

    /// <summary>
    /// Initializes a new instance
    /// </summary>
    /// <param name="disposeImmediately">
    /// If true uses thread-static buffer that is faster but must return immediately.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// This exception is thrown when <c>new StringBuilder(disposeImmediately: true)</c> or <c>ZString.CreateUtf8StringBuilder(notNested: true)</c> is nested.
    /// See the README.md
    /// </exception>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public Utf8ValueStringBuilder 
        (
            bool disposeImmediately
        )
    {
        if (disposeImmediately && scratchBufferUsed)
        {
            ThrowNestedException();
        }

        byte[]? buf;
        if (disposeImmediately)
        {
            buf = _scratchBuffer ??= new byte[ThreadStaticBufferSize];

            scratchBufferUsed = true;
        }
        else
        {
            buf = ArrayPool<byte>.Shared.Rent (DefaultBufferSize);
        }

        _buffer = buf;
        _index = 0;
        _disposeImmediately = disposeImmediately;
    }

    /// <summary>
    /// Return the inner buffer to pool.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        if (_buffer != null)
        {
            if (_buffer.Length != ThreadStaticBufferSize)
            {
                ArrayPool<byte>.Shared.Return (_buffer);
            }

            _buffer = null;
            _index = 0;
            if (_disposeImmediately)
            {
                scratchBufferUsed = false;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public void Clear()
    {
        _index = 0;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sizeHint"></param>
    public void TryGrow (int sizeHint)
    {
        if (_buffer!.Length < _index + sizeHint)
        {
            Grow (sizeHint);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sizeHint"></param>
    public void Grow (int sizeHint)
    {
        var nextSize = _buffer!.Length * 2;
        if (sizeHint != 0)
        {
            nextSize = Math.Max (nextSize, _index + sizeHint);
        }

        var newBuffer = ArrayPool<byte>.Shared.Rent (nextSize);

        _buffer.CopyTo (newBuffer, 0);
        if (_buffer.Length != ThreadStaticBufferSize)
        {
            ArrayPool<byte>.Shared.Return (_buffer);
        }

        _buffer = newBuffer;
    }

    /// <summary>Appends the default line terminator to the end of this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine()
    {
        if (_crlf)
        {
            if (_buffer!.Length - _index < 2) Grow (2);
            _buffer[_index] = _newLine1;
            _buffer[_index + 1] = _newLine2;
            _index += 2;
        }
        else
        {
            if (_buffer!.Length - _index < 1) Grow (1);
            _buffer[_index] = _newLine1;
            _index += 1;
        }
    }

    /// <summary>Appends the string representation of a specified value to this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public unsafe void Append (char value)
    {
        var maxLen = UTF8NoBom.GetMaxByteCount (1);
        if (_buffer!.Length - _index < maxLen)
        {
            Grow (maxLen);
        }

        fixed (byte* bp = &_buffer[_index])
        {
            _index += UTF8NoBom.GetBytes (&value, 1, bp, maxLen);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append (char value, int repeatCount)
    {
        if (repeatCount < 0)
        {
            throw new ArgumentOutOfRangeException (nameof (repeatCount));
        }

        if (value <= 0x7F) // ASCII
        {
            GetSpan (repeatCount).Fill ((byte)value);
            Advance (repeatCount);
        }
        else
        {
            var maxLen = UTF8NoBom.GetMaxByteCount (1);
            Span<byte> utf8Bytes = stackalloc byte[maxLen];
            ReadOnlySpan<char> chars = stackalloc char[1] { value };

            var len = UTF8NoBom.GetBytes (chars, utf8Bytes);

            TryGrow (len * repeatCount);

            for (var i = 0; i < repeatCount; i++)
            {
                utf8Bytes.CopyTo (GetSpan (len));
                Advance (len);
            }
        }
    }

    /// <summary>Appends the string representation of a specified value followed by the default line terminator to the end of this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine (char value)
    {
        Append (value);
        AppendLine();
    }

    /// <summary>
    /// 
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append (string? value, int startIndex, int count)
    {
        if (value == null)
        {
            if (startIndex == 0 && count == 0)
            {
                return;
            }
            else
            {
                throw new ArgumentNullException (nameof (value));
            }
        }

        Append (value.AsSpan (startIndex, count));
    }

    /// <summary>Appends the string representation of a specified value to this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append (string value)
    {
        Append (value.AsSpan());
    }

    /// <summary>Appends the string representation of a specified value followed by the default line terminator to the end of this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine (string value)
    {
        Append (value);
        AppendLine();
    }

    /// <summary>Appends a contiguous region of arbitrary memory to this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append (ReadOnlySpan<char> value)
    {
        var maxLen = UTF8NoBom.GetMaxByteCount (value.Length);
        if (_buffer!.Length - _index < maxLen)
        {
            Grow (maxLen);
        }

        _index += UTF8NoBom.GetBytes (value, _buffer.AsSpan (_index));
    }

    /// <summary>
    /// 
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine (ReadOnlySpan<char> value)
    {
        Append (value);
        AppendLine();
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendLiteral (ReadOnlySpan<byte> value)
    {
        if ((_buffer!.Length - _index) < value.Length)
        {
            Grow (value.Length);
        }

        value.CopyTo (_buffer.AsSpan (_index));
        _index += value.Length;
    }

    /// <summary>Appends the string representation of a specified value to this instance.</summary>
    public void Append<T> (T value)
    {
        if (!FormatterCache<T>.TryFormatDelegate (value, _buffer.AsSpan (_index), out var written, default))
        {
            Grow (written);
            if (!FormatterCache<T>.TryFormatDelegate (value, _buffer.AsSpan (_index), out written, default))
            {
                ThrowArgumentException (nameof (value));
            }
        }

        _index += written;
    }

    /// <summary>Appends the string representation of a specified value followed by the default line terminator to the end of this instance.</summary>
    public void AppendLine<T> (T value)
    {
        Append (value);
        AppendLine();
    }

    // Output

    /// <summary>Copy inner buffer to the bufferWriter.</summary>
    public void CopyTo (IBufferWriter<byte> bufferWriter)
    {
        var destination = bufferWriter.GetSpan (_index);
        TryCopyTo (destination, out var written);
        bufferWriter.Advance (written);
    }

    /// <summary>Copy inner buffer to the destination span.</summary>
    public bool TryCopyTo (Span<byte> destination, out int bytesWritten)
    {
        if (destination.Length < _index)
        {
            bytesWritten = 0;
            return false;
        }

        bytesWritten = _index;
        _buffer.AsSpan (0, _index).CopyTo (destination);
        return true;
    }

    /// <summary>Write inner buffer to stream.</summary>
    public Task WriteToAsync (Stream stream)
    {
        return stream.WriteAsync (_buffer.ThrowIfNull(), 0, _index);
    }

    /// <summary>Write inner buffer to stream.</summary>
    public Task WriteToAsync (Stream stream, CancellationToken cancellationToken)
    {
        return stream.WriteAsync (_buffer.ThrowIfNull(), 0, _index, cancellationToken);
    }

    /// <summary>Encode the innner utf8 buffer to a System.String.</summary>
    public override string ToString()
    {
        if (_index == 0)
            return string.Empty;

        return UTF8NoBom.GetString (_buffer.ThrowIfNull(), 0, _index);
    }

    // IBufferWriter

    /// <summary>IBufferWriter.GetMemory.</summary>
    public Memory<byte> GetMemory 
        (
            int sizeHint
        )
    {
        if ((_buffer!.Length - _index) < sizeHint)
        {
            Grow (sizeHint);
        }

        return _buffer.AsMemory (_index);
    }

    /// <summary>IBufferWriter.GetSpan.</summary>
    public Span<byte> GetSpan (int sizeHint)
    {
        if ((_buffer!.Length - _index) < sizeHint)
        {
            Grow (sizeHint);
        }

        return _buffer.AsSpan (_index);
    }

    /// <summary>IBufferWriter.Advance.</summary>
    public void Advance (int count)
    {
        _index += count;
    }

    void IResettableBufferWriter<byte>.Reset()
    {
        _index = 0;
    }

    void ThrowArgumentException (string paramName)
    {
        throw new ArgumentException ("Can't format argument.", paramName);
    }

    void ThrowFormatException()
    {
        throw new FormatException (
            "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
    }

    static void ThrowNestedException()
    {
        throw new NestedStringBuilderCreationException (nameof (Utf16ValueStringBuilder));
    }

    private void AppendFormatInternal<T> (T arg, int width, StandardFormat format, string argName)
    {
        if (width <= 0) // leftJustify
        {
            width *= -1;

            if (!FormatterCache<T>.TryFormatDelegate (arg, _buffer.AsSpan (_index), out var charsWritten, format))
            {
                Grow (charsWritten);
                if (!FormatterCache<T>.TryFormatDelegate (arg, _buffer.AsSpan (_index), out charsWritten, format))
                {
                    ThrowArgumentException (argName);
                }
            }

            _index += charsWritten;

            var padding = width - charsWritten;
            if (width > 0 && padding > 0)
            {
                Append (' ', padding); // TODO Fill Method is too slow.
            }
        }
        else // rightJustify
        {
            if (typeof (T) == typeof (string))
            {
                var s = Unsafe.As<string> (arg);
                var padding = width - s.ThrowIfNull().Length;
                if (padding > 0)
                {
                    Append (' ', padding); // TODO Fill Method is too slow.
                }

                Append (s.ThrowIfNull());
            }
            else
            {
                Span<byte> s = stackalloc byte[typeof (T).IsValueType ? Unsafe.SizeOf<T>() * 8 : 1024];

                if (!FormatterCache<T>.TryFormatDelegate (arg, s, out var charsWritten, format))
                {
                    s = stackalloc byte[s.Length * 2];
                    if (!FormatterCache<T>.TryFormatDelegate (arg, s, out charsWritten, format))
                    {
                        ThrowArgumentException (argName);
                    }
                }

                var padding = width - charsWritten;
                if (padding > 0)
                {
                    Append (' ', padding); // TODO Fill Method is too slow.
                }

                s.CopyTo (GetSpan (charsWritten));
                Advance (charsWritten);
            }
        }
    }

    /// <summary>
    /// Register custom formatter
    /// </summary>
    public static void RegisterTryFormat<T> (TryFormat<T> formatMethod)
    {
        FormatterCache<T>.TryFormatDelegate = formatMethod;
    }

    /// <summary>
    /// Supports the Nullable type for a given struct type.
    /// </summary>
    public static void EnableNullableFormat<T>() where T : struct
    {
        RegisterTryFormat (CreateNullableFormatter<T>());
    }
    
    #endregion
}
