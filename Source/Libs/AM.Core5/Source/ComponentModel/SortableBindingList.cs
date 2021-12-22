// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* SortableBindingList.cs -- BindingList<T>, поддерживающий сортировку
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#endregion

#nullable enable

namespace AM.ComponentModel;

/// <summary>
/// <see cref="BindingList{T}"/>, поддерживающий сортировку.
/// </summary>
public sealed class SortableBindingList<T>
    : BindingList<T>
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SortableBindingList()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SortableBindingList
        (
            IList<T> list
        )
        : base (list)
    {
    }

    #endregion

    #region Private members

    private bool _isSorted;
    private ListSortDirection _sortDirection = ListSortDirection.Ascending;
    private PropertyDescriptor? _sortProperty;

    #endregion

    #region BindingList<T> members

    /// <inheritdoc cref="BindingList{T}.ApplySortCore(PropertyDescriptor, ListSortDirection)"/>
    protected override void ApplySortCore
        (
            PropertyDescriptor property,
            ListSortDirection direction
        )
    {
        _sortProperty = property;
        _sortDirection = direction;

        IEnumerable<T> query = Items;

        query = direction == ListSortDirection.Ascending
            ? query.OrderBy (i => property.GetValue (i!))
            : query.OrderByDescending (i => property.GetValue (i!));

        var newIndex = 0;
        foreach (var item in query)
        {
            Items[newIndex] = item;
            newIndex++;
        }

        _isSorted = true;
        OnListChanged (new ListChangedEventArgs (ListChangedType.Reset, -1));
    }

    /// <inheritdoc cref="BindingList{T}.SortPropertyCore"/>
    protected override PropertyDescriptor? SortPropertyCore => _sortProperty;

    /// <inheritdoc cref="BindingList{T}.SortDirectionCore"/>
    protected override ListSortDirection SortDirectionCore => _sortDirection;

    /// <inheritdoc cref="BindingList{T}.IsSortedCore"/>
    protected override bool IsSortedCore => _isSorted;

    /// <inheritdoc cref="BindingList{T}.SupportsSortingCore"/>
    protected override bool SupportsSortingCore => true;

    #endregion
}
