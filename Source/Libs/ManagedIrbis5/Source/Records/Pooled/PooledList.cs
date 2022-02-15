// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PooledList.cs -- специальный вариант списка для подполей/полей записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Специальный вариант списка для подполей/полей библиографической записи
/// </summary>
public sealed class PooledList<T>
    : IReadOnlyList<T>
    where T: class
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PooledList()
    {
        _list = new List<T>();
    }

    #endregion

    #region Private members

    private readonly List<T> _list;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление элемента в список.
    /// </summary>
    internal void Add
        (
            T item
        )
    {
        Sure.NotNull (item);

        _list.Add (item);
    }

    /// <summary>
    /// Очистка списка.
    /// </summary>
    internal void Clear()
    {
        _list.Clear();
    }

    #endregion

    #region IReadOnlyList<T> members

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    /// <inheritdoc cref="IReadOnlyCollection{T}.Count"/>
    public int Count => _list.Count;

    /// <inheritdoc cref="IReadOnlyList{T}.this"/>
    public T this [int index] => _list[index];

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{nameof (Count)}: {Count}";
    }

    #endregion
}
