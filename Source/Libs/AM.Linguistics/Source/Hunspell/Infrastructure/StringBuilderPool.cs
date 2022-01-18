// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;
using System.Threading;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

internal static class StringBuilderPool
{
    private const int MaxCachedBuilderCapacity = WordList.MaxWordLen;

    private static StringBuilder Cache;

    public static StringBuilder Get()
    {
        return GetClearedBuilder();
    }

    public static StringBuilder Get (string value)
    {
        return GetClearedBuilderWithCapacity (value.Length).Append (value);
    }

    public static StringBuilder Get (string value, int capacity)
    {
        return GetClearedBuilderWithCapacity (capacity).Append (value);
    }

    public static StringBuilder Get (int capacity)
    {
        return GetClearedBuilderWithCapacity (capacity);
    }

    public static StringBuilder Get (string value, int valueStartIndex, int valueLength)
    {
        return Get (value, valueStartIndex, valueLength, valueLength);
    }

    public static StringBuilder Get (string value, int valueStartIndex, int valueLength, int capacity)
    {
        return GetClearedBuilderWithCapacity (capacity).Append (value, valueStartIndex, valueLength);
    }

    public static StringBuilder Get (ReadOnlySpan<char> value)
    {
        return GetClearedBuilderWithCapacity (value.Length).Append (value);
    }

    public static void Return (StringBuilder builder)
    {
#if DEBUG
        if (builder == null) throw new ArgumentNullException (nameof (builder));
#endif

        if (builder.Capacity <= MaxCachedBuilderCapacity) Volatile.Write (ref Cache, builder);
    }

    public static string GetStringAndReturn (StringBuilder builder)
    {
        var value = builder.ToString();
        Return (builder);
        return value;
    }

    private static StringBuilder GetClearedBuilder()
    {
        var taken = Interlocked.Exchange (ref Cache, null);
        return taken != null
            ? taken.Clear()
            : new StringBuilder();
    }

    private static StringBuilder GetClearedBuilderWithCapacity (int minimumCapacity)
    {
        var taken = Interlocked.Exchange (ref Cache, null);
        return taken != null && taken.Capacity >= minimumCapacity
            ? taken.Clear()
            : new StringBuilder (minimumCapacity);
    }
}
