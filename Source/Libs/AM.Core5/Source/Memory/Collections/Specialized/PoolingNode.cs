// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PoolingNode.cs -- контейнер в коллекции, использующей пулинг
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Контейнер в коллекции, использующей пулинг.
/// </summary>
/// <typeparam name="T">Тип хранимого значения.</typeparam>
internal sealed class PoolingNode<T>
    : PoolingNodeBase<T>
{
    #region PoolingNodeBase members

    /// <inheritdoc cref="PoolingNodeBase{T}.Dispose"/>
    public override void Dispose()
    {
        base.Dispose();
        Pool<PoolingNode<T>>.Return (this);
    }

    /// <inheritdoc cref="PoolingNodeBase{T}.Init"/>
    public override IPoolingNode<T> Init
        (
            int capacity
        )
    {
        Sure.Positive (capacity);

        Next = null;
        _buf = Pool.GetBuffer<T> (capacity);
        return this;
    }

    #endregion
}
