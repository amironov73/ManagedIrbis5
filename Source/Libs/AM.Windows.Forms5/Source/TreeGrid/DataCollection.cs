// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DataCollection.cs -- набор данных, хранящихся в строке грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Набор данных, хранящихся в строке грида.
/// При любом изменении данных грид перерисовывается.
/// </summary>
[Serializable]
public sealed class DataCollection
    : Collection<object?>
{
    #region Properties

    /// <summary>
    /// Нода, которой принадлежат данные.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TreeGridNode Node { get; }

    /// <summary>
    /// Gets the tree grid.
    /// </summary>
    /// <value>The tree grid.</value>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
    public TreeGrid Grid => Node.Grid.ThrowIfNull();

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="node">Нода, которой принадлежат данные.</param>
    public DataCollection
        (
            TreeGridNode node
        )
    {
        Sure.NotNull (node);

        Node = node;
    }

    #endregion

    #region Collection<T> members

    /// <inheritdoc cref="Collection{T}.ClearItems"/>
    protected override void ClearItems()
    {
        base.ClearItems();
        _UpdateNode();
    }

    /// <inheritdoc cref="Collection{T}.InsertItem"/>
    protected override void InsertItem
        (
            int index,
            object? item
        )
    {
        Sure.NonNegative (index);

        base.InsertItem (index, item);
        _UpdateNode();
    }

    /// <inheritdoc cref="Collection{T}.RemoveItem"/>
    protected override void RemoveItem
        (
            int index
        )
    {
        Sure.NonNegative (index);

        base.RemoveItem (index);
        _UpdateNode();
    }

    /// <inheritdoc cref="Collection{T}.SetItem"/>
    protected override void SetItem
        (
            int index,
            object? item
        )
    {
        Sure.NonNegative (index);

        base.SetItem (index, item);
        _UpdateNode();
    }

    #endregion

    #region Private members

    internal void _UpdateNode() => Node._UpdateGrid();

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление множества элементов.
    /// </summary>
    public void AddRange
        (
            params object[] range
        )
    {
        Sure.NotNull (range);

        foreach (var item in range)
        {
            Add (item);
        }
    }

    /// <summary>
    /// Добавление множества элементов.
    /// </summary>
    public void AddRange
        (
            IEnumerable range
        )
    {
        Sure.NotNull ((object?) range);

        foreach (var item in range)
        {
            Add (item);
        }
    }

    /// <summary>
    /// Безопасный доступ к элементу по индексу.
    /// </summary>
    public object? SafeGet
        (
            int index
        )
    {
        return index >= 0 && index < Count ? this[index] : null;
    }

    /// <summary>
    /// Безопасное задание значения элемента по индексу.
    /// </summary>
    public void SafeSet
        (
            int index,
            object? data
        )
    {
        while (Count <= index)
        {
            Add (null);
        }

        this[index] = data;
    }

    #endregion
}
