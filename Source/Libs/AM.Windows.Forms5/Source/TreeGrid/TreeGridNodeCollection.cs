// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* TreeGridNodeCollection.cs -- коллекция узлов грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.ObjectModel;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Коллекция узлов грида.
/// </summary>
public sealed class TreeGridNodeCollection
    : Collection<TreeGridNode>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TreeGridNodeCollection
        (
            TreeGrid? grid,
            TreeGridNode? parent
        )
    {
        _grid = grid;
        _parent = parent;
    }

    #endregion

    #region Private members

    internal TreeGrid? _grid;
    internal TreeGridNode? _parent;


    internal void _UpdateGrid()
    {
        if (_grid != null)
        {
            _grid.UpdateState();
        }
    }

    /// <inheritdoc cref="Collection{T}.ClearItems"/>
    protected override void ClearItems()
    {
        base.ClearItems();
        _UpdateGrid();
    }

    /// <inheritdoc cref="Collection{T}.InsertItem"/>
    protected override void InsertItem
        (
            int index,
            TreeGridNode item
        )
    {
        Sure.NonNegative (index);
        Sure.NotNull (item);

        item.Parent = _parent;
        item._SetTreeGrid (_grid);
        base.InsertItem (index, item);
        _UpdateGrid();
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem"/>
    protected override void RemoveItem
        (
            int index
        )
    {
        Sure.InRange (index, this);

        base.RemoveItem (index);
        _UpdateGrid();
    }

    /// <inheritdoc cref="Collection{T}.SetItem"/>
    protected override void SetItem
        (
            int index,
            TreeGridNode item
        )
    {
        Sure.InRange (index, this);
        Sure.NotNull (item);

        item.Parent = _parent;
        item._SetTreeGrid (_grid);
        base.SetItem (index, item);
        _UpdateGrid();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление дочернего узла.
    /// </summary>
    public TreeGridNode Add
        (
            string? text,
            params object[]? data
        )
    {
        var result = new TreeGridNode { Title = text };
        if (data is not null)
        {
            result.Data.AddRange (data);
        }

        Add (result);

        return result;
    }

    #endregion
}
