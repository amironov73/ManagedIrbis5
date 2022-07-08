// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* PooledBuffer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;

#endregion

#nullable enable

namespace AM.Updating.Internal;

internal readonly struct PooledBuffer<T> : IDisposable
{
    public T[] Array { get; }

    public PooledBuffer (int minimumLength) =>
        Array = ArrayPool<T>.Shared.Rent (minimumLength);

    public void Dispose() =>
        ArrayPool<T>.Shared.Return (Array);
}

internal static class PooledBuffer
{
    public static PooledBuffer<byte> ForStream() => new (81920);
}
