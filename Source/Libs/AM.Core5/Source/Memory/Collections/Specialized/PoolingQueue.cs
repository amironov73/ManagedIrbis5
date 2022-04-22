// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PoolingQueue.cs -- очередь, хранящая свои элементы в пуле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Очередь, хранящая свои элементы в пуле.
/// </summary>
/// <typeparam name="T">Тип элементов очереди.</typeparam>
public abstract class PoolingQueue<T>
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Очередь пуста?
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Количество элементов в очереди.
    /// </summary>
    public int Count { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    protected PoolingQueue()
    {
        Count = 0;
        _enqueueIndex = 0;
        _dequeueIndex = 0;
        _enqueueTo = _dequeueFrom = default;
    }

    #endregion

    #region Private members

    private IPoolingNode<T>? _enqueueTo;
    private IPoolingNode<T>? _dequeueFrom;
    private int _enqueueIndex, _dequeueIndex;

    #endregion

    #region Public methods

    /// <inheritdoc cref="Queue{T}.Enqueue"/>
    public void Enqueue
        (
            T obj
        )
    {
        if (Count == 0 && _enqueueTo is null)
        {
            _enqueueTo = _dequeueFrom = CreateNodeHolder();
        }

        _enqueueTo![_enqueueIndex] = obj;
        _enqueueIndex++;
        Count++;

        if (_enqueueIndex == PoolsDefaults.DefaultPoolBucketSize)
        {
            var enqueue = _enqueueTo;
            _enqueueTo = CreateNodeHolder();
            enqueue.Next = _enqueueTo;
            _enqueueIndex = 0;
        }
    }

    /// <summary>
    /// Создание холдера для элемента.
    /// </summary>
    protected abstract IPoolingNode<T> CreateNodeHolder();

    /// <summary>
    ///     Tries to return queue element if any available via `val` parameter.
    /// </summary>
    /// <returns>
    ///     true if element found or false otherwise
    /// </returns>
    public bool TryDequeue
        (
            out T val
        )
    {
        if (IsEmpty)
        {
            val = default!;
            return false;
        }

        val = Dequeue();
        return true;
    }

    /// <summary>
    ///     Returns queue element
    /// </summary>
    /// <returns>
    ///     Returns element or throws IndexOutOfRangeException if no element found
    /// </returns>
    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new IndexOutOfRangeException();
        }

        var obj = _dequeueFrom![_dequeueIndex];
        _dequeueFrom[_dequeueIndex] = default!;

        _dequeueIndex++;
        Count--;

        if (_dequeueIndex == PoolsDefaults.DefaultPoolBucketSize)
        {
            var dequeue = _dequeueFrom;
            _dequeueFrom = _dequeueFrom.Next;
            _dequeueIndex = 0;
            dequeue.Dispose();
        }

        if (Count == 0)
        {
            // return back to pool
            if (_enqueueTo != _dequeueFrom)
            {
                var empty = _dequeueFrom;
                _dequeueFrom = _dequeueFrom.Next;
                _dequeueIndex = 0;
                empty.Dispose();
            }
            else

                // reset to pool start
            {
                _enqueueIndex = 0;
                _dequeueIndex = 0;
            }
        }

        return obj;
    }

    /// <inheritdoc cref="Queue{T}.Clear"/>
    public void Clear()
    {
        while (_enqueueTo is not null)
        {
            var next = _enqueueTo.Next;
            _enqueueTo.Dispose();
            _enqueueTo = next;
        }

        _dequeueFrom = null;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Clear();
    }

    #endregion
}
