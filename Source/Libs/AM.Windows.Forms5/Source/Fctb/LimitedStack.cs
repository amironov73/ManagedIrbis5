// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LimitedStack.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Limited stack
/// </summary>
public class LimitedStack<T>
{
    T[] items;
    int count;
    int start;

    /// <summary>
    /// Max stack length
    /// </summary>
    public int MaxItemCount
    {
        get { return items.Length; }
    }

    /// <summary>
    /// Current length of stack
    /// </summary>
    public int Count
    {
        get { return count; }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="maxItemCount">Maximum length of stack</param>
    public LimitedStack (int maxItemCount)
    {
        items = new T[maxItemCount];
        count = 0;
        start = 0;
    }

    /// <summary>
    /// Pop item
    /// </summary>
    public T Pop()
    {
        if (count == 0)
            throw new Exception ("Stack is empty");

        var i = LastIndex;
        var item = items[i];
        items[i] = default (T);

        count--;

        return item;
    }

    int LastIndex
    {
        get { return (start + count - 1) % items.Length; }
    }

    /// <summary>
    /// Peek item
    /// </summary>
    public T Peek()
    {
        if (count == 0)
            return default (T);

        return items[LastIndex];
    }

    /// <summary>
    /// Push item
    /// </summary>
    public void Push (T item)
    {
        if (count == items.Length)
            start = (start + 1) % items.Length;
        else
            count++;

        items[LastIndex] = item;
    }

    /// <summary>
    /// Clear stack
    /// </summary>
    public void Clear()
    {
        items = new T[items.Length];
        count = 0;
        start = 0;
    }

    public T[] ToArray()
    {
        var result = new T[count];
        for (var i = 0; i < count; i++)
            result[i] = items[(start + i) % items.Length];
        return result;
    }
}
