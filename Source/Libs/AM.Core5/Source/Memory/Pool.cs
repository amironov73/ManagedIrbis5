// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Pool.cs -- утилиты для пулинга объектов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Microsoft.Extensions.ObjectPool;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Утилиты для пулинга объектов.
/// </summary>
public static class Pool<T>
    where T : class, new()
{
    #region Private members

    private static readonly DefaultObjectPool<T> _freeObjectsQueue
        = new (new DefaultPooledObjectPolicy<T>());

    #endregion

    #region Public methods

    /// <summary>
    /// Получение объекта из пула.
    /// </summary>
    public static T Get()
    {
        return _freeObjectsQueue.Get();
    }

    /// <summary>
    /// Возврат объекта в пул.
    /// </summary>
    public static void Return<T1>
        (
            T1 instance
        )
        where T1 : T
    {
        _freeObjectsQueue.Return (instance);
    }

    #endregion
}

/// <summary>
/// Утилиты для пулинга объектов
/// </summary>
public static class Pool
{
    #region Public methods

    /// <summary>
    /// Получение объекта из пула.
    /// </summary>
    public static T Get<T>()
        where T : class, new()
    {
        return Pool<T>.Get();
    }

    /// <summary>
    /// Возврат объекта в пул.
    /// </summary>
    public static void Return<T>
        (
            T instance
        )
        where T : class, new()
    {
        Pool<T>.Return (instance);
    }

    /// <summary>
    /// Получение буфера указанного размера.
    /// </summary>
    public static CountdownMemoryOwner<T> GetBuffer<T>
        (
            int size
        )
    {
        Sure.Positive (size);

        return InternalArraysPool.Rent<T> (size);
    }

    /// <summary>
    /// Получение буфера из указанного диапазона.
    /// </summary>
    public static CountdownMemoryOwner<T> GetBufferFrom<T>
        (
            ReadOnlySpan<T> source
        )
    {
        return InternalArraysPool.RentFrom (source);
    }

    #endregion
}
