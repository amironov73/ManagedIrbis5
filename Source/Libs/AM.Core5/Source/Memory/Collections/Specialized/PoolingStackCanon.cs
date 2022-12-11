// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* PoolingStackCannon.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Collection, which is working on shared btw all Pooling* collections buckets
/// </summary>
public class PoolingStackCanon<T>
    : PoolingStackBase<T>
    where T : class
{
    /// <inheritdoc cref="PoolingStackBase{T}.CreateNodeHolder"/>
    protected override IPoolingNode<T> CreateNodeHolder()
    {
        return (IPoolingNode<T>)Pool<PoolingNodeCanon<T>>.Get()
            .Init (PoolsDefaults.DefaultPoolBucketSize);
    }
}
