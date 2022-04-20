// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PoolingList.cs -- список, хранящий свои элементы в пуле
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Список, хранящий свои элементы в массиве, занимаемом из системного пула.
/// </summary>
/// <typeparam name="T">Тип элементов списка.</typeparam>
public sealed class PoolingList<T>
    : PoolingListBase<T>
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PoolingList()
    {
        Init();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Инициализация.
    /// </summary>
    public PoolingList<T> Init()
    {
        _root = Pool.GetBuffer<IPoolingNode<T>> (PoolsDefaults.DefaultPoolBucketSize);
        _ver = 0;

        return this;
    }

    #endregion

    /// <inheritdoc cref="PoolingListBase{T}.CreateNodeHolder"/>
    protected override IPoolingNode<T> CreateNodeHolder()
    {
        return Pool<PoolingNode<T>>.Get().Init (PoolsDefaults.DefaultPoolBucketSize);
    }
}
