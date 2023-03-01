// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Uft16ValueStringBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Buffers.Text;

/// <summary>
/// 
/// </summary>
[PublicAPI]
public partial struct Utf16ValueStringBuilder
    : IDisposable, IBufferWriter<char>, IResettableBufferWriter<char>
{
    #region Delegates
    
    /// <summary>
    /// 
    /// </summary>
    public delegate bool TryFormat<T> 
        (
            T value,
            Span<char> destination, 
            out int charsWritten,
            ReadOnlySpan<char> format
        );
    
    #endregion

    #region Constants
    
    private const int ThreadStaticBufferSize = 31111;
    private const int DefaultBufferSize = 32768; // use 32K default buffer.

    #endregion

    #region Nested classes

    static class ExceptionUtil
    {
        public static void ThrowArgumentOutOfRangeException 
            (
                string paramName
            )
        {
            throw new ArgumentOutOfRangeException (paramName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
                    formatter = EnumUtil<T>.TryFormatUtf16;
                }
                else if (typeof (T) == typeof (string))
                {
                    formatter = TryFormatString;
                }
                else
                {
                    formatter = TryFormatDefault;
                }
            }

            TryFormatDelegate = formatter;
        }

        static bool TryFormatString 
            (
                T value,
                Span<char> dest,
                out int written,
                ReadOnlySpan<char> format
            )
        {
            var s = value as string;

            if (s == null)
            {
                written = 0;
                return true;
            }

            // also use this length when result is false.
            written = s.Length;
            return s.AsSpan().TryCopyTo (dest);
        }

        static bool TryFormatDefault 
            (
                T value,
                Span<char> dest,
                out int written,
                ReadOnlySpan<char> format
            )
        {
            if (value == null)
            {
                written = 0;
                return true;
            }

            var s = (value is IFormattable formattable && format.Length != 0)
                ? formattable.ToString (format.ToString(), null)
                : value.ToString();

            // also use this length when result is false.
            written = s.Length;
            return s.AsSpan().TryCopyTo (dest);
        }
    }

    #endregion

    #region Properties

    /// <summary>Length of written buffer.</summary>
    public int Length => _index;

    #endregion

    #region Construction
    
    static Utf16ValueStringBuilder()
    {
        var newLine = Environment.NewLine.ToCharArray();
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

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="disposeImmediately">
    /// If true uses thread-static buffer that is faster but must return immediately.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// This exception is thrown when <c>new StringBuilder(disposeImmediately: true)
    /// </c> or <c>ZString.CreateStringBuilder(notNested: true)</c> is nested.
    /// </exception>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public Utf16ValueStringBuilder 
        (
            bool disposeImmediately
        )
    {
        if (disposeImmediately && scratchBufferUsed)
        {
            ThrowNestedException();
        }

        char[]? buf;
        if (disposeImmediately)
        {
            buf = _scratchBuffer;
            if (buf == null)
            {
                buf = _scratchBuffer = new char[ThreadStaticBufferSize];
            }

            scratchBufferUsed = true;
        }
        else
        {
            buf = ArrayPool<char>.Shared.Rent (DefaultBufferSize);
        }

        _buffer = buf;
        _index = 0;
        _disposeImmediately = disposeImmediately;
    }
    
    #endregion


    /// <summary>Get the written buffer data.</summary>
    public ReadOnlySpan<char> AsSpan() => _buffer.AsSpan (0, _index);

    /// <summary>Get the written buffer data.</summary>
    public ReadOnlyMemory<char> AsMemory() => _buffer.AsMemory (0, _index);

    /// <summary>Get the written buffer data.</summary>
    public ArraySegment<char> AsArraySegment() => new ArraySegment<char> (_buffer, 0, _index);

    #region Private members

    private static readonly char _newLine1;
    private static readonly char _newLine2;
    private static readonly bool _crlf;

    private char[]? _buffer;
    private int _index;
    private readonly bool _disposeImmediately;

    [ThreadStatic] 
    private static char[]? _scratchBuffer;

    [ThreadStatic] 
    internal static bool scratchBufferUsed;

    void IResettableBufferWriter<char>.Reset()
    {
        _index = 0;
    }

    private void ThrowArgumentException (string paramName)
    {
        throw new ArgumentException ("Can't format argument.", paramName);
    }

    private static void ThrowFormatException()
    {
        throw new FormatException (
            "Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
    }

    void AppendFormatInternal<T> (T arg, int width, ReadOnlySpan<char> format, string argName)
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
                var padding = width - s.Length;
                if (padding > 0)
                {
                    Append (' ', padding); // TODO Fill Method is too slow.
                }

                Append (s);
            }
            else
            {
                Span<char> s = stackalloc char[typeof (T).IsValueType ? Unsafe.SizeOf<T>() * 8 : 1024];

                if (!FormatterCache<T>.TryFormatDelegate (arg, s, out var charsWritten, format))
                {
                    s = stackalloc char[s.Length * 2];
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

                Append (s.Slice (0, charsWritten));
            }
        }
    }

    static void ThrowNestedException()
    {
        throw new NestedStringBuilderCreationException (nameof (Utf16ValueStringBuilder));
    }

    #endregion
    
    #region Public methods

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
                ArrayPool<char>.Shared.Return (_buffer);
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
    public void TryGrow
        (
            int sizeHint
        )
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
    public void Grow
        (
            int sizeHint
        )
    {
        var nextSize = _buffer!.Length * 2;
        if (sizeHint != 0)
        {
            nextSize = Math.Max (nextSize, _index + sizeHint);
        }

        var newBuffer = ArrayPool<char>.Shared.Rent (nextSize);

        _buffer.CopyTo (newBuffer, 0);
        if (_buffer.Length != ThreadStaticBufferSize)
        {
            ArrayPool<char>.Shared.Return (_buffer);
        }

        _buffer = newBuffer;
    }

    /// <summary>Appends the default line terminator to the end of this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine()
    {
        if (_crlf)
        {
            if (_buffer!.Length - _index < 2)
            {
                Grow (2);
            }

            _buffer[_index] = _newLine1;
            _buffer[_index + 1] = _newLine2;
            _index += 2;
        }
        else
        {
            if (_buffer!.Length - _index < 1)
            {
                Grow (1);
            }

            _buffer[_index] = _newLine1;
            _index += 1;
        }
    }

    /// <summary>Appends the string representation of a specified value to this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            char value
        )
    {
        if (_buffer!.Length - _index < 1)
        {
            Grow (1);
        }

        _buffer[_index++] = value;
    }

    /// <summary>
    /// 
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            char value,
            int repeatCount
        )
    {
        if (repeatCount < 0)
        {
            throw new ArgumentOutOfRangeException (nameof (repeatCount));
        }

        GetSpan (repeatCount).Fill (value);
        Advance (repeatCount);
    }

    /// <summary>Appends the string representation of a specified value followed by the default line terminator to the end of this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine
        (
            char value
        )
    {
        Append (value);
        AppendLine();
    }

    /// <summary>Appends the string representation of a specified value to this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            string value
        )
    {
        Append (value.AsSpan());
    }

    /// <summary>Appends the string representation of a specified value followed by the default line terminator to the end of this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine
        (
            string value
        )
    {
        Append (value);
        AppendLine();
    }

    /// <summary>
    /// 
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            string? value,
            int startIndex, 
            int count
        )
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

    /// <summary>
    /// 
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            char[] value,
            int startIndex,
            int charCount
        )
    {
        if (_buffer!.Length - _index < charCount)
        {
            Grow (charCount);
        }

        Array.Copy (value, startIndex, _buffer, _index, charCount);
        _index += charCount;
    }

    /// <summary>Appends a contiguous region of arbitrary memory to this instance.</summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Append
        (
            ReadOnlySpan<char> value
        )
    {
        if (_buffer!.Length - _index < value.Length)
        {
            Grow (value.Length);
        }

        value.CopyTo (_buffer.AsSpan (_index));
        _index += value.Length;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void AppendLine
        (
            ReadOnlySpan<char> value
        )
    {
        Append (value);
        AppendLine();
    }

    /// <summary>Appends the string representation of a specified value to this instance.</summary>
    public void Append<T>
        (
            T value
        )
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
    public void AppendLine<T>
        (
            T value
        )
    {
        Append (value);
        AppendLine();
    }

    /// <summary>
    /// Inserts a string 0 or more times into this builder at the specified position.
    /// </summary>
    /// <param name="index">The index to insert in this builder.</param>
    /// <param name="value">The string to insert.</param>
    /// <param name="count">The number of times to insert the string.</param>
    public void Insert
        (
            int index,
            string value,
            int count
        )
    {
        Insert (index, value.AsSpan(), count);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Insert
        (
            int index,
            string value
        )
    {
        Insert (index, value.AsSpan(), 1);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Insert
        (
            int index,
            ReadOnlySpan<char> value,
            int count
        )
    {
        if (count < 0)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (count));
        }

        var currentLength = Length;
        if ((uint)index > (uint)currentLength)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (index));
        }

        if (value.Length == 0 || count == 0)
        {
            return;
        }

        var newSize = index + value.Length * count;
        var newBuffer = ArrayPool<char>.Shared.Rent (Math.Max (DefaultBufferSize, newSize));

        _buffer.AsSpan (0, index).CopyTo (newBuffer);
        var newBufferIndex = index;

        for (var i = 0; i < count; i++)
        {
            value.CopyTo (newBuffer.AsSpan (newBufferIndex));
            newBufferIndex += value.Length;
        }

        var remainLnegth = _index - index;
        _buffer.AsSpan (index, remainLnegth).CopyTo (newBuffer.AsSpan (newBufferIndex));

        if (_buffer!.Length != ThreadStaticBufferSize)
        {
            if (_buffer != null)
            {
                ArrayPool<char>.Shared.Return (_buffer);
            }
        }

        _buffer = newBuffer;
        _index = newBufferIndex + remainLnegth;
    }

    /// <summary>
    /// Replaces all instances of one character with another in this builder.
    /// </summary>
    /// <param name="oldChar">The character to replace.</param>
    /// <param name="newChar">The character to replace <paramref name="oldChar"/> with.</param>
    public void Replace
        (
            char oldChar,
            char newChar
        )
    {
        Replace (oldChar, newChar, 0, Length);
    }

    /// <summary>
    /// Replaces all instances of one character with another in this builder.
    /// </summary>
    /// <param name="oldChar">The character to replace.</param>
    /// <param name="newChar">The character to replace <paramref name="oldChar"/> with.</param>
    /// <param name="startIndex">The index to start in this builder.</param>
    /// <param name="count">The number of characters to read in this builder.</param>
    public void Replace
        (
            char oldChar,
            char newChar,
            int startIndex,
            int count
        )
    {
        var currentLength = Length;
        if ((uint)startIndex > (uint)currentLength)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (startIndex));
        }

        if (count < 0 || startIndex > currentLength - count)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (count));
        }

        var endIndex = startIndex + count;

        for (var i = startIndex; i < endIndex; i++)
        {
            if (_buffer![i] == oldChar)
            {
                _buffer[i] = newChar;
            }
        }
    }

    /// <summary>
    /// Replaces all instances of one string with another in this builder.
    /// </summary>
    /// <param name="oldValue">The string to replace.</param>
    /// <param name="newValue">The string to replace <paramref name="oldValue"/> with.</param>
    /// <remarks>
    /// If <paramref name="newValue"/> is <c>null</c>, instances of <paramref name="oldValue"/>
    /// are removed from this builder.
    /// </remarks>
    public void Replace
        (
            string oldValue,
            string newValue
        )
    {
        Replace (oldValue, newValue, 0, Length);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    public void Replace
        (
            ReadOnlySpan<char> oldValue,
            ReadOnlySpan<char> newValue
        )
    {
        Replace (oldValue, newValue, 0, Length);
    }

    /// <summary>
    /// Replaces all instances of one string with another in part of this builder.
    /// </summary>
    /// <param name="oldValue">The string to replace.</param>
    /// <param name="newValue">The string to replace <paramref name="oldValue"/> with.</param>
    /// <param name="startIndex">The index to start in this builder.</param>
    /// <param name="count">The number of characters to read in this builder.</param>
    /// <remarks>
    /// If <paramref name="newValue"/> is <c>null</c>, instances of <paramref name="oldValue"/>
    /// are removed from this builder.
    /// </remarks>
    public void Replace
        (
            string oldValue,
            string newValue,
            int startIndex,
            int count
        )
    {
        if (oldValue == null)
        {
            throw new ArgumentNullException (nameof (oldValue));
        }

        Replace (oldValue.AsSpan(), newValue.AsSpan(), startIndex, count);
    }

    /// <summary>
    /// 
    /// </summary>
    public void Replace
        (
            ReadOnlySpan<char> oldValue,
            ReadOnlySpan<char> newValue,
            int startIndex,
            int count
        )
    {
        var currentLength = Length;

        if ((uint)startIndex > (uint)currentLength)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (startIndex));
        }

        if (count < 0 || startIndex > currentLength - count)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (count));
        }

        if (oldValue.Length == 0)
        {
            throw new ArgumentException ("oldValue.Length is 0", nameof (oldValue));
        }

        var readOnlySpan = AsSpan();
        var endIndex = startIndex + count;
        var matchCount = 0;

        for (var i = startIndex; i < endIndex; i += oldValue.Length)
        {
            var span = readOnlySpan.Slice (i, endIndex - i);
            var pos = span.IndexOf (oldValue, StringComparison.Ordinal);
            if (pos == -1)
            {
                break;
            }

            i += pos;
            matchCount++;
        }

        if (matchCount == 0)
        {
            return;
        }

        var newBuffer = ArrayPool<char>.Shared.Rent (Math.Max (DefaultBufferSize,
            Length + (newValue.Length - oldValue.Length) * matchCount));

        _buffer.AsSpan (0, startIndex).CopyTo (newBuffer);
        var newBufferIndex = startIndex;

        for (var i = startIndex; i < endIndex; i += oldValue.Length)
        {
            var span = readOnlySpan.Slice (i, endIndex - i);
            var pos = span.IndexOf (oldValue, StringComparison.Ordinal);
            if (pos == -1)
            {
                var remain = readOnlySpan.Slice (i);
                remain.CopyTo (newBuffer.AsSpan (newBufferIndex));
                newBufferIndex += remain.Length;
                break;
            }

            readOnlySpan.Slice (i, pos).CopyTo (newBuffer.AsSpan (newBufferIndex));
            newValue.CopyTo (newBuffer.AsSpan (newBufferIndex + pos));
            newBufferIndex += pos + newValue.Length;
            i += pos;
        }

        if (_buffer!.Length != ThreadStaticBufferSize)
        {
            ArrayPool<char>.Shared.Return (_buffer);
        }

        _buffer = newBuffer;
        _index = newBufferIndex;
    }

    /// <summary>
    /// Replaces the contents of a single position within the builder.
    /// </summary>
    /// <param name="newChar">The character to use at the position.</param>
    /// <param name="replaceIndex">The index to replace.</param>
    public void ReplaceAt
        (
            char newChar,
            int replaceIndex
        )
    {
        var currentLength = Length;
        if ((uint) replaceIndex > (uint) currentLength)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (replaceIndex));
        }

        _buffer![replaceIndex] = newChar;
    }

    /// <summary>
    /// Removes a range of characters from this builder.
    /// </summary>
    /// <remarks>
    /// This method does not reduce the capacity of this builder.
    /// </remarks>
    public void Remove 
        (
            int startIndex,
            int length
        )
    {
        if (length < 0)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (length));
        }

        if (startIndex < 0)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (startIndex));
        }

        if (length > Length - startIndex)
        {
            ExceptionUtil.ThrowArgumentOutOfRangeException (nameof (length));
        }

        if (Length == length && startIndex == 0)
        {
            _index = 0;
            return;
        }

        if (length == 0)
        {
            return;
        }

        var remain = startIndex + length;
        _buffer.AsSpan (remain, Length - remain).CopyTo (_buffer.AsSpan (startIndex));
        _index -= length;
    }

    /// <summary>Copy inner buffer to the destination span.</summary>
    public bool TryCopyTo
        (
            Span<char> destination, 
            out int charsWritten
        )
    {
        if (destination.Length < _index)
        {
            charsWritten = 0;
            return false;
        }

        charsWritten = _index;
        _buffer.AsSpan (0, _index).CopyTo (destination);
        return true;
    }

    /// <summary>Converts the value of this instance to a System.String.</summary>
    public override string ToString()
    {
        if (_index == 0)
        {
            return string.Empty;
        }

        return new string (_buffer, 0, _index);
    }

    // IBufferWriter

    /// <summary>IBufferWriter.GetMemory.</summary>
    public Memory<char> GetMemory
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
    public Span<char> GetSpan
        (
            int sizeHint
        )
    {
        if ((_buffer!.Length - _index) < sizeHint)
        {
            Grow (sizeHint);
        }

        return _buffer.AsSpan (_index);
    }

    /// <summary>IBufferWriter.Advance.</summary>
    public void Advance 
        (
            int count
        )
    {
        _index += count;
    }

    /// <summary>
    /// Register custom formatter
    /// </summary>
    public static void RegisterTryFormat<T>
        (
            TryFormat<T> formatMethod
        )
    {
        FormatterCache<T>.TryFormatDelegate = formatMethod;
    }

    static TryFormat<T?> CreateNullableFormatter<T>()
        where T: struct
    {
        return new TryFormat<T?> ((T? x, Span<char> dest, out int written, ReadOnlySpan<char> format) =>
        {
            if (x == null)
            {
                written = 0;
                return true;
            }

            return FormatterCache<T>.TryFormatDelegate (x.Value, dest, out written, format);
        });
    }

    /// <summary>
    /// Supports the Nullable type for a given struct type.
    /// </summary>
    public static void EnableNullableFormat<T>() 
        where T: struct
    {
        RegisterTryFormat<T?> (CreateNullableFormatter<T>());
    }

    #endregion
}
