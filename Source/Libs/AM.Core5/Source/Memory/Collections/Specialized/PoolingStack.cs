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

/* PoolingStack.cs -- стек, хранящий свои элементы в пуле
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Стек, хранящий свои элементы в пуле.
/// </summary>
/// <typeparam name="T">Тип элементов стека.</typeparam>
public class PoolingStack<T>
    : PoolingStackBase<T>
{
    #region PoolingStackBase members

    /// <inheritdoc cref="PoolingStackBase{T}.CreateNodeHolder"/>
    protected override IPoolingNode<T> CreateNodeHolder()
    {
        return Pool<PoolingNode<T>>.Get().Init (PoolsDefaults.DefaultPoolBucketSize);
    }

    #endregion
}
