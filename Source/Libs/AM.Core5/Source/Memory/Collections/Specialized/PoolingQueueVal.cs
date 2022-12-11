// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PoolingQueueVal.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Poolinq queue stores items in buckets of 256 size, who linked with linked list.
/// Nodes of this list and storage (array[256])
/// ** NOT THREAD SAFE **
/// Enqueue, dequeue: O(1).
/// </summary>
/// <typeparam name="T">Items should be classes because underlying collection stores object type</typeparam>
public sealed class PoolingQueueVal<T>
    : PoolingQueue<T>
    where T : struct
{
    /// <inheritdoc cref="PoolingQueue{T}.CreateNodeHolder"/>
    protected override IPoolingNode<T> CreateNodeHolder()
    {
        return Pool<PoolingNode<T>>.Get()
            .Init (PoolsDefaults.DefaultPoolBucketSize);
    }
}
