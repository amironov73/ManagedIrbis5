// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* NamedMutableEntityTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Default tree node for participation in a mutable hierarchical structure.
/// The contained entity is assumed to have a string name (alias).
/// It can be renamed through the node, potentially resulting in an alias error being raised.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public class NamedMutableEntityTreeNode<TId, TItem>
    : MutableEntityTreeNode<NamedMutableEntityTreeNode<TId, TItem>, TId, string, TItem>
{
    private readonly Action<TItem, string> _setItemName;

    /// <summary>
    /// Root constructor
    /// </summary>
    /// <param name="definition">Entity definition that determines identity parameters</param>
    /// <param name="setItemName">Function specifying how entity name is set</param>
    /// <param name="rootItem">Item contained by root</param>
    /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
    public NamedMutableEntityTreeNode
        (
            IEntityDefinition<TId, string, TItem> definition,
            Action<TItem, string> setItemName,
            TItem rootItem,
            ErrorCheckOptions checkOptions = ErrorCheckOptions.Default
        )
        : base (definition, rootItem, checkOptions)
    {
        _setItemName = setItemName;
    }

    /// <summary>
    /// Root constructor
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="getItemName">Entity name (alias) selector</param>
    /// <param name="setItemName">Function specifying how entity name (alias) is set</param>
    /// <param name="rootItem">Item contained by root</param>
    /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
    public NamedMutableEntityTreeNode
        (
            Func<TItem, TId> getId,
            Func<TItem, string> getItemName,
            Action<TItem, string> setItemName,
            TItem rootItem,
            ErrorCheckOptions checkOptions = ErrorCheckOptions.Default
        )
        : this (new NamedEntityDefinition<TId, TItem> (getId, getItemName), setItemName, rootItem, checkOptions)
    {
        // пустое тело конструктора
    }

    private NamedMutableEntityTreeNode
        (
            TItem item,
            NamedMutableEntityTreeNode<TId, TItem> parent,
            Action<TItem, string> setItemName
        )
        : base (item, parent)
    {
        _setItemName = setItemName;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    protected sealed override NamedMutableEntityTreeNode<TId, TItem> Create
        (
            TItem item,
            NamedMutableEntityTreeNode<TId, TItem> parent
        )
    {
        return new NamedMutableEntityTreeNode<TId, TItem> (item, parent, _setItemName);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="name"></param>
    protected sealed override void SetItemName
        (
            string name
        )
    {
        _setItemName (Item, name);
    }
}
