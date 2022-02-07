// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MemoryEx.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Buffers;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

public static class MemoryEx
{
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int Length<T> (this IMemoryOwner<T> that) =>
        that.Memory.Length;

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> AsCountdown<T> (this CountdownMemoryOwner<T> that,
        bool noDefaultOwner = false) =>
        Pool<CountdownMemoryOwner<T>>.Get().Init (that, 0, that.Memory.Length, noDefaultOwner);

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> AsCountdown<T> (this CountdownMemoryOwner<T> that, int offset,
        bool noDefaultOwner = false) =>
        Pool<CountdownMemoryOwner<T>>.Get().Init (that, offset, that.Memory.Length - offset, noDefaultOwner);

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> AsCountdown<T> (this CountdownMemoryOwner<T> that, int offset, int length,
        bool noDefaultOwner = false) =>
        Pool<CountdownMemoryOwner<T>>.Get().Init (that, offset, length, noDefaultOwner);

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static IMemoryOwner<T> Slice<T> (this CountdownMemoryOwner<T> that, int offset) =>
        Slice (that, offset, that.Memory.Length - offset);

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static IMemoryOwner<T> Slice<T> (this CountdownMemoryOwner<T> that, int offset, int length) =>
        that.AsCountdown (offset, length);
}
