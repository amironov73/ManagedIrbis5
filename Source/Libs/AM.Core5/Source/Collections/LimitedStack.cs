// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LimitedStack.cs -- стек ограниченной емкости
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Стек ограниченной емкости.
/// При превышении емкости старые элементы затираются новыми.
/// </summary>
public sealed class LimitedStack<T>
{
    #region Properties

    /// <summary>
    /// Количество элементов в стеке в текущий момент.
    /// </summary>
    public int Count => _count;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="capacity">Емкость стека</param>
    public LimitedStack
        (
            int capacity
        )
    {
        Sure.Positive (capacity);

        _items = new T[capacity];
        _count = 0;
        _start = 0;
    }

    #endregion

    #region Private members

    private T?[] _items;
    private int _count;
    private int _start;

    /// <summary>
    /// Последний индекс.
    /// </summary>
    private int LastIndex => (_start + _count - 1) % _items.Length;

    #endregion

    #region Public methods

    /// <summary>
    /// Извлечение элемента.
    /// </summary>
    public T? Pop()
    {
        if (_count == 0)
        {
            throw new Exception ("Stack is empty");
        }

        var i = LastIndex;
        var item = _items[i];
        _items[i] = default (T);

        _count--;

        return item;
    }

    /// <summary>
    /// Подглядывание.
    /// </summary>
    public T? Peek()
    {
        return _count == 0 ? default : _items[LastIndex];
    }

    /// <summary>
    /// Помещение элемента в стек.
    /// </summary>
    public void Push (T item)
    {
        if (_count == _items.Length)
        {
            _start = (_start + 1) % _items.Length;
        }
        else
        {
            _count++;
        }

        _items[LastIndex] = item;
    }

    /// <summary>
    /// Очистка.
    /// </summary>
    public void Clear()
    {
        _items = new T[_items.Length];
        _count = 0;
        _start = 0;
    }

    /// <summary>
    /// Преобразование в массив.
    /// </summary>
    public T?[] ToArray()
    {
        var result = new T?[_count];
        for (var i = 0; i < _count; i++)
        {
            result[i] = _items[(_start + i) % _items.Length];
        }

        return result;
    }

    #endregion
}
