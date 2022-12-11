// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PoolingNodeCanon.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

internal sealed class PoolingNodeCanon<T>
    : PoolingNodeBase<object>, IPoolingNode<T>
    where T : class
{
    IPoolingNode<T>? IPoolingNode<T>.Next
    {
        get => (IPoolingNode<T>?) Next;
        set => Next = (IPoolingNode<object>?) value;
    }

    T IPoolingNode<T>.this [int index]
    {
        get => (T)_buf!.Memory.Span[index];
        set => _buf!.Memory.Span[index] = value;
    }

    IPoolingNode<T> IPoolingNode<T>.Init (int capacity)
    {
        this.Init (capacity);
        return this;
    }

    public override void Dispose()
    {
        base.Dispose();
        Pool<PoolingNodeCanon<T>>.Return (this);
    }

    public override IPoolingNode<object> Init (int capacity)
    {
        Next = null;
        _buf = Pool.GetBuffer<object> (capacity);
        return this;
    }
}
