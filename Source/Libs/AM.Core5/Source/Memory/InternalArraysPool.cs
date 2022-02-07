// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* InternalArraysPool.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

internal sealed class InternalArraysPool
{
    private const int MinBufferSize = 128;

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<byte> Rent (int length)
    {
        return Rent<byte> (length);
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> Rent<T> (int length, bool noDefaultOwner = false)
    {
        var realLength = length;
        var allocLength = length > MinBufferSize ? length : MinBufferSize;
        var owner = BucketsBasedCrossThreadsMemoryPool<T>.Shared.Rent (allocLength);
        return owner.AsCountdown (0, realLength, noDefaultOwner);
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> RentFrom<T> (ReadOnlySpan<T> source, bool noDefaultOwner = false)
    {
        var mem = Rent<T> (source.Length, noDefaultOwner);
        source.CopyTo (mem.Memory.Span);
        return mem;
    }
}
