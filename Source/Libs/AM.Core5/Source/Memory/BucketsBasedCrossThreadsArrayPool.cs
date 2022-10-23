// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* BucketsBasedCrossThreadsArrayPool.cs -- пул массивов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Пул массивов.
/// </summary>
public sealed class BucketsBasedCrossThreadsArrayPool<T>
{
    #region Properties

    /// <summary>
    /// Общий пул.
    /// </summary>
    public static BucketsBasedCrossThreadsArrayPool<T> Shared =>
        _shared ??= new BucketsBasedCrossThreadsArrayPool<T>();

    #endregion

    #region Construction

    static BucketsBasedCrossThreadsArrayPool()
    {
        _pool = new Queue<T[]>[24];
        for (var i = 0; i < _pool.Length; i++)
        {
            _pool[i] = new Queue<T[]>();
        }
    }

    #endregion

    #region Private members

    [ThreadStatic]
    private static BucketsBasedCrossThreadsArrayPool<T>? _shared;

    private static readonly Queue<T[]>[] _pool;

    #endregion

    /// <summary>
    /// Займ объекта.
    /// </summary>
    public T[] Rent
        (
            int minimumLength
        )
    {
        Sure.Positive (minimumLength);

        var queueIndex = MemoryUtility.GetBucket (minimumLength);
        var queue = _pool[queueIndex];
        T[] result;

        if (queue.Count == 0)
        {
            var length = MemoryUtility.GetMaxSizeForBucket (queueIndex);
            result = new T[length];
            return result;
        }

        result = queue.Dequeue();
        return result;
    }

    /// <summary>
    /// Возврат объекта.
    /// </summary>
    public void Return
        (
            T[] array
        )
    {
        Sure.NotNull (array);

        _pool[MemoryUtility.GetBucket (array.Length)].Enqueue (array);
    }
}
