// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BindingListSource.cs -- combination of BindingSource and BindingList
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Combination of <see cref="BindingSource"/>
/// and <see cref="System.ComponentModel.BindingList{T}"/>.
/// </summary>
/// <remarks>Values added through <see cref="Add"/>
/// or <see cref="Insert"/> methods can't be <c>null</c>.
/// </remarks>
[System.ComponentModel.DesignerCategory ("Code")]
public class BindingListSource<T>
    : BindingSource
{
    #region Properties

    /// <summary>
    /// Gets the inner list.
    /// </summary>
    public System.ComponentModel.BindingList<T> InnerList { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="BindingListSource{T}"/> class.
    /// </summary>
    public BindingListSource()
    {
        InnerList = new ();
        DataSource = InnerList;
    }

    #endregion

    #region InnerList delegating members

    /// <inheritdoc cref="System.ComponentModel.BindingList{T}.ResetBindings"/>
    public void ResetBindings() => InnerList.ResetBindings();

    /// <inheritdoc cref="System.ComponentModel.BindingList{T}.CancelNew"/>
    public void CancelNew (int itemIndex) => InnerList.CancelNew (itemIndex);

    /// <inheritdoc cref="System.ComponentModel.BindingList{T}.EndNew"/>
    public void EndNew (int itemIndex) => InnerList.EndNew (itemIndex);

    /// <inheritdoc cref="Collection{T}.Add"/>
    public void Add (T item)
    {
        object? value = item;
        if (value == null)
        {
            throw new ArgumentNullException (nameof (item));
        }

        InnerList.Add (item);
    }

    /// <inheritdoc cref="Collection{T}.CopyTo"/>
    public void CopyTo (T[] array, int index) => InnerList.CopyTo (array, index);

    /// <inheritdoc cref="Collection{T}.Contains"/>
    public bool Contains (T item) => InnerList.Contains (item);

    /// <inheritdoc cref="Collection{T}.IndexOf"/>
    public int IndexOf (T item) => InnerList.IndexOf (item);

    /// <inheritdoc cref="Collection{T}.Insert"/>
    public void Insert (int index, T item) => InnerList.Insert (index, item);

    /// <inheritdoc cref="Collection{T}.Remove"/>
    public bool Remove (T item) => InnerList.Remove (item);

    #endregion
}
