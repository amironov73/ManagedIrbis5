﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PoolingStackBase.cs -- базовый класс для стека, хранящего свои элементы в пуле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory.Collections.Specialized;

/// <summary>
/// Базовый класс для стека, хранящего свои элементы в пуле.
/// </summary>
public abstract class PoolingStackBase<T>
    : IDisposable
{
    private IPoolingNode<T>? _top;
    private int _topIndex;

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected PoolingStackBase()
    {
        Count = 0;
        _topIndex = 0;
        _top = null;
    }

    /// <summary>
    /// Стек пуст?
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Количество элементов в стеке.
    /// </summary>
    public int Count { get; private set; }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        Clear();
    }

    /// <summary>
    /// Создание холдера для ноды.
    /// </summary>
    protected abstract IPoolingNode<T> CreateNodeHolder();

    /// <summary>
    /// Помещение элемента на вершину стека.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Push (T obj)
    {
        _top ??= CreateNodeHolder();

        _top[_topIndex] = obj;
        _topIndex++;
        Count++;

        if (_topIndex == PoolsDefaults.DefaultPoolBucketSize)
        {
            var top = _top;
            _top = CreateNodeHolder();
            _top.Next = top;
            _topIndex = 0;
        }
    }

    /// <summary>
    ///     Tries to return queue element if any available via `val` parameter.
    /// </summary>
    /// <returns>
    ///     true if element found or false otherwise
    /// </returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public bool TryPop (out T val)
    {
        if (IsEmpty)
        {
            val = default!;
            return false;
        }

        val = Pop();
        return true;
    }

    /// <summary>
    ///     Returns queue element
    /// </summary>
    /// <returns>
    ///     Returns element or throws IndexOutOfRangeException if no element found
    /// </returns>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public T Pop()
    {
        if (IsEmpty)
        {
            throw new IndexOutOfRangeException();
        }

        _topIndex--;

        if (_topIndex < 0)
        {
            _topIndex = PoolsDefaults.DefaultPoolBucketSize - 1;
            var oldTop = _top!;
            _top = _top!.Next;
            oldTop.Dispose();
        }

        var obj = _top![_topIndex];
        _top[_topIndex] = default!;

        Count--;

        return obj;
    }

    /// <summary>
    /// Очистка стека.
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        while (_top != null)
        {
            var next = _top.Next;
            _top.Dispose();
            _top = next;
        }
    }
}
