// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DelegatingList.cs -- список, делегирующий свои функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Список, делегирующий свои функции.
/// </summary>
public sealed class DelegatingList<TItem>
    : IList<TItem>
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DelegatingList()
    {
        GetCount = () => 0;
        GetItem = _ => default!;
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Получение количества элементов в списке.
    /// </summary>
    public Func<int> GetCount { get; set; }

    /// <summary>
    /// Доступ к элементу по индексу.
    /// </summary>
    public Func<int, TItem> GetItem { get; set; }

    #endregion

    #region IList members

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() => throw new NotImplementedException();

    /// <inheritdoc cref="ICollection{T}.Add"/>
    void ICollection<TItem>.Add (TItem item) => throw new NotImplementedException();

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    void ICollection<TItem>.Clear() => throw new NotImplementedException();

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    bool ICollection<TItem>.Contains (TItem item) => throw new NotImplementedException();

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    void ICollection<TItem>.CopyTo (TItem[] array, int arrayIndex) => throw new NotImplementedException();

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    bool ICollection<TItem>.Remove (TItem item) => throw new NotImplementedException();

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count => GetCount();

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool IsReadOnly => true;

    /// <inheritdoc cref="IList{T}.IndexOf"/>
    int IList<TItem>.IndexOf (TItem item) => throw new NotImplementedException();

    /// <inheritdoc cref="IList{T}.Insert"/>
    void IList<TItem>.Insert (int index, TItem item) => throw new NotImplementedException();

    /// <inheritdoc cref="IList{T}.RemoveAt"/>
    void IList<TItem>.RemoveAt (int index) => throw new NotImplementedException();

    /// <inheritdoc cref="IList{T}.this"/>
    public TItem this [int index]
    {
        get => GetItem (index);
        set => throw new NotImplementedException();
    }

    #endregion
}
