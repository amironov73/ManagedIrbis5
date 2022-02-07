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

using System;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.ObjectPool;

#endregion

#nullable enable

namespace AM.Memory;

public static class Pool<T>
    where T : class, new()
{
    private static readonly DefaultObjectPool<T> _freeObjectsQueue = new (new DefaultPooledObjectPolicy<T>());

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static T Get()
    {
        return _freeObjectsQueue.Get();
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static void Return<T1> (T1 instance) where T1 : T
    {
        _freeObjectsQueue.Return (instance);
    }
}

public static class Pool
{
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static T Get<T>() where T : class, new()
    {
        return Pool<T>.Get();
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static void Return<T> (T instance) where T : class, new()
    {
        Pool<T>.Return (instance);
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> GetBuffer<T> (int size)
    {
        return InternalArraysPool.Rent<T> (size, false);
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static CountdownMemoryOwner<T> GetBufferFrom<T> (ReadOnlySpan<T> source)
    {
        return InternalArraysPool.RentFrom (source, false);
    }
}
