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

/* JetStack.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

internal class JetStack<T>
{
    #region Nested structs

    // PERF: the struct wrapper avoids array-covariance-checks
    // from the runtime when assigning to elements of the array.
    private struct ObjectWrapper
    {
        public T Element;
    }

    #endregion

    #region Constants

    private const int DefaultCapacity = 4;

    #endregion

    #region Properties

    public int Count => _size;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public JetStack()
    {
        _array = new ObjectWrapper[DefaultCapacity];
        _firstItem = default!;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    // Create a stack with a specific initial capacity.  The initial capacity
    // must be a non-negative number.
    public JetStack
        (
            int capacity
        )
    {
        _array = new ObjectWrapper[capacity];
        _firstItem = default!;
    }

    #endregion

    #region Private members

    private ObjectWrapper[] _array;

    private int _size;
    private T _firstItem;

    // Non-inline from Stack.Push to improve its code quality as uncommon path
    [MethodImpl (MethodImplOptions.NoInlining)]
    private void PushWithResize (T item)
    {
        Array.Resize (ref _array, _array.Length << 1);
        _array[_size].Element = item;
        _size++;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    // Removes all Objects from the Stack.
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
        // Don't need to doc this but we clear the elements so that the gc can reclaim the references
        Array.Clear
            (
                _array,
                0,
                _size
            );
        _size = 0;
    }

    /// <summary>
    ///
    /// </summary>
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public T Peek()
    {
        return _array[_size - 1].Element;
    }

    /// <summary>
    ///
    /// </summary>
    // Pops an item from the top of the stack.  If the stack is empty, Pop
    // throws an InvalidOperationException.
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public T Pop()
    {
        var item = _firstItem;
        if (_firstItem != null)
        {
            _firstItem = default!;
            return item;
        }

        return _array[--_size].Element;
    }

    /// <summary>
    ///
    /// </summary>
    // Pushes an item to the top of the stack.
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public void Push
        (
            T item
        )
    {
        if (_firstItem == null)
        {
            _firstItem = item;
            return;
        }

        if (_size >= _array.Length)
        {
            PushWithResize (item);
        }
        else
        {
            _array[_size++].Element = item;
        }
    }
}
