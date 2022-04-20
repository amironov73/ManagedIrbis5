// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PoolingNodeBase.cs -- базовый класс для контейнера в коллекции, использующей пулинг
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Buffers;

#endregion

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Базовый класс для контейнера в коллекции, использующей пулинг.
/// </summary>
/// <typeparam name="T">Тип хранимого значения.</typeparam>
internal abstract class PoolingNodeBase<T>
    : IPoolingNode<T>
{
    #region Private members

    /// <summary>
    /// Буфер, в котором хранится значение.
    /// </summary>
    protected IMemoryOwner<T>? _buf;

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка буфера.
    /// </summary>
    public void Clear()
    {
        _buf!.Memory.Span.Clear();
    }

    /// <summary>
    /// Длина буфера.
    /// </summary>
    public int Length => _buf!.Memory.Length;

    /// <summary>
    /// Индексатор.
    /// </summary>
    /// <param name="index"></param>
    public virtual T this [int index]
    {
        get => _buf!.Memory.Span[index];
        set => _buf!.Memory.Span[index] = value;
    }

    /// <summary>
    /// Очистка.
    /// </summary>
    public virtual void Dispose()
    {
        if (_buf is not null)
        {
            _buf.Dispose();
            _buf = null;
        }

        Next = null;
    }

    /// <summary>
    /// Ссылка на следующий элемент коллекции.
    /// </summary>
    public IPoolingNode<T>? Next { get; set; }

    /// <summary>
    /// Инициализация.
    /// </summary>
    public abstract IPoolingNode<T> Init (int capacity);

    #endregion
}
