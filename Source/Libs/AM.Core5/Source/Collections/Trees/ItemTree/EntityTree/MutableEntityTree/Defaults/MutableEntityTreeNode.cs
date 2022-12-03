// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* MutableEntityTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Default tree node for participation in a mutable hierarchical structure
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public class MutableEntityTreeNode<TId, TItem>
    : MutableEntityTreeNode<MutableEntityTreeNode<TId, TItem>, TId, TItem>
{
    /// <summary>
    /// Root constructor
    /// </summary>
    /// <param name="definition">Entity definition that determines identity parameters</param>
    /// <param name="rootItem">Item contained by root</param>
    /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
    public MutableEntityTreeNode
        (
            IEntityDefinition<TId, TItem> definition,
            TItem rootItem,
            ErrorCheckOptions checkOptions = ErrorCheckOptions.Default
        )
        : base (definition, rootItem, checkOptions)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Root constructor
    /// </summary>
    /// <param name="getId">Entity Id selector</param>
    /// <param name="rootItem">Item contained by root</param>
    /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
    public MutableEntityTreeNode
        (
            Func<TItem, TId> getId,
            TItem rootItem,
            ErrorCheckOptions checkOptions = ErrorCheckOptions.Default
        )
        : this (new EntityDefinition<TId, TItem> (getId), rootItem, checkOptions)
    {
        // пустое тело конструктора
    }

    private MutableEntityTreeNode
        (
            TItem item,
            MutableEntityTreeNode<TId, TItem> parent
        )
        : base (item, parent)
    {
        // пустое тело конструктора
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    protected sealed override MutableEntityTreeNode<TId, TItem> Create
        (
            TItem item,
            MutableEntityTreeNode<TId, TItem> parent
        )
    {
        return new MutableEntityTreeNode<TId, TItem> (item, parent);
    }
}
