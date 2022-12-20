// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EnumerableIList.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Generic;

/// <summary>
///
/// </summary>
public static class EnumerableIList
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static EnumerableIList<T> Create<T>(IList<T> list) => new (list);
}

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct EnumerableIList<T>
    : IEnumerableIList<T>, IList<T>
{
    private readonly IList<T> _list;

    /// <summary>
    ///
    /// </summary>
    /// <param name="list"></param>
    public EnumerableIList(IList<T> list)
    {
        _list = list;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public EnumeratorIList<T> GetEnumerator() => new (_list);

    /// <summary>
    ///
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static implicit operator EnumerableIList<T>(List<T> list) => new EnumerableIList<T>(list);

    /// <summary>
    ///
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static implicit operator EnumerableIList<T>(T[] array) => new (array);

    /// <summary>
    ///
    /// </summary>
    public static EnumerableIList<T> Empty = default;

    // IList pass through

    /// <inheritdoc />
    public T this[int index] { get => _list[index]; set => _list[index] = value; }

    /// <inheritdoc />
    public int Count => _list.Count;

    /// <inheritdoc />
    public bool IsReadOnly => _list.IsReadOnly;

    /// <inheritdoc />
    public void Add(T item) => _list.Add(item);

    /// <inheritdoc />
    public void Clear() => _list.Clear();

    /// <inheritdoc />
    public bool Contains(T item) => _list.Contains(item);

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    /// <inheritdoc />
    public int IndexOf(T item) => _list.IndexOf(item);

    /// <inheritdoc />
    public void Insert(int index, T item) => _list.Insert(index, item);

    /// <inheritdoc />
    public bool Remove(T item) => _list.Remove(item);

    /// <inheritdoc />
    public void RemoveAt(int index) => _list.RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
}
