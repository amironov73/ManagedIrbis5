// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StringEx.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;

using AM.Text;

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

internal static class StringEx
{
    private static readonly char[] _spaceOrTab = { ' ', '\t' };

#if !NO_INLINE
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
    public static string[] SplitOnTabOrSpace
        (
            this string that
        )
    {
#if DEBUG
        if (that == null)
        {
            throw new ArgumentNullException (nameof (that));
        }
#endif
        return that.Split (_spaceOrTab, StringSplitOptions.RemoveEmptyEntries);
    }

#if !NO_INLINE
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
    public static bool IsTabOrSpace (this char c) => c is ' ' or '\t';

    public static string? GetReversed
        (
            this string? that
        )
    {
        if (that == null || that.Length <= 1)
        {
            return that;
        }

        using var owner = MemoryPool<char>.Shared.Rent (that.Length);
        var buffer = owner.Memory.Span.Slice (0, that.Length);
        var lastIndex = that.Length - 1;
        for (var i = 0; i < buffer.Length; i++)
        {
            buffer[i] = that[lastIndex - i];
        }

        return buffer.ToString();
    }

    public static string Replace
        (
            this string that,
            int index,
            int removeCount,
            string replacement
        )
    {
        var capacity = Math.Max (that.Length, that.Length + replacement.Length - removeCount);
        var builder = AM.Text.StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (capacity);
        builder.Replace (index, removeCount, replacement);
        return builder.ReturnShared();
    }

#if !NO_INLINE
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
    public static char GetCharOrTerminator
        (
            this string that,
            int index
        )
    {
#if DEBUG
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException (nameof (index));
        }
#endif
        return index < that.Length ? that[index] : '\0';
    }

    public static string ConcatString
        (
            string str0,
            int startIndex0,
            int count0,
            string str1
        )
    {
        return str0.AsSpan (startIndex0, count0).ConcatString (str1);
    }

    public static string ConcatString
        (
            string str0,
            string str1,
            int startIndex1,
            int count1
        )
    {
        return str0.ConcatString (str1.AsSpan (startIndex1, count1));
    }

    public static string ConcatString
        (
            string str0,
            int startIndex0,
            int count0,
            string str1,
            char char2,
            string str3,
            int startIndex3
        )
    {
        var count3 = str3.Length - startIndex3;
        var capacity = count0 + str1.Length + 1 + count3;
        var builder = AM.Text.StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (capacity);
        builder.Append (str0, startIndex0, count0);
        builder.Append (str1);
        builder.Append (char2);
        builder.Append (str3, startIndex3, count3);
        return builder.ReturnShared();
    }

    public static string ConcatString
        (
            string str0,
            int startIndex0,
            int count0,
            string str1,
            string str2,
            int startIndex2
        )
    {
        var count2 = str2.Length - startIndex2;
        var capacity = count0 + str1.Length + count2;
        var builder = AM.Text.StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (capacity);
        builder.Append (str0, startIndex0, count0);
        builder.Append (str1);
        builder.Append (str2, startIndex2, count2);
        return builder.ReturnShared();
    }

    public static string ConcatString
        (
            string str0,
            int startIndex0,
            int count0,
            char char1,
            string str2,
            int startIndex2
        )
    {
        return ConcatString (str0, startIndex0, count0, char1.ToString(), str2, startIndex2);
    }

    public static string ConcatString
        (
            this string that,
            ReadOnlySpan<char> value
        )
    {
#if DEBUG
        if (that == null)
        {
            throw new ArgumentNullException (nameof (that));
        }
#endif
        if (that.Length == 0)
        {
            return value.ToString();
        }

        if (value.IsEmpty)
        {
            return that;
        }

        var capacity = that.Length + value.Length;
        var builder = AM.Text.StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (capacity);
        builder.Append (that);
        builder.Append (value);
        return builder.ReturnShared();
    }

#if !NO_INLINE
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
    public static bool IsLineBreakChar (this char c) => c is '\n' or '\r';

    public static string WithoutIndex
        (
            this string that,
            int index
        )
    {
#if DEBUG
        if (that == null)
        {
            throw new ArgumentNullException (nameof (that));
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException (nameof (index));
        }

        if (index >= that.Length)
        {
            throw new ArgumentOutOfRangeException (nameof (index));
        }
#endif

        if (index == 0)
        {
            return that.Substring (1);
        }

        var lastIndex = that.Length - 1;
        if (index == lastIndex)
        {
            return that.Substring (0, lastIndex);
        }

        var builder = AM.Text.StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (lastIndex);
        builder.Append (that, 0, index);
        builder.Append (that, index + 1, lastIndex - index);
        return builder.ReturnShared();
    }
}
