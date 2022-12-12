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

#nullable enable

namespace TreeCollections;

/// <summary>
/// Abstract entity tree node participating in a mutable hierarchical structure.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
    : EntityTreeNode<TNode, TId, TItem>
    where TNode: MutableEntityTreeNode<TNode, TId, TItem>
{
    /// <summary>
    /// Root constructor
    /// </summary>
    /// <param name="definition">Entity definition that determines identity parameters</param>
    /// <param name="rootItem">Item contained by root</param>
    /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
    protected MutableEntityTreeNode
        (
            IEntityDefinition<TId, TItem> definition,
            TItem rootItem,
            ErrorCheckOptions checkOptions = ErrorCheckOptions.Default
        )
        : base (definition, checkOptions, rootItem)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Descendant constructor
    /// </summary>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    protected MutableEntityTreeNode
        (
            TItem item,
            TNode parent
        )
        : base (item, parent)
    {
        // пустое тело конструктора
    }

    /// <inheritdoc cref="TreeNode{TNode}.IsReadOnly"/>
    public sealed override bool IsReadOnly => false;

    /// <summary>
    /// Called before this node is detached from parent
    /// </summary>
    protected virtual void OnNodeDetaching()
    {
        // пустое тело метода
    }

    /// <summary>
    /// Called after this node is detached from parent
    /// </summary>
    /// <param name="formerParent"></param>
    protected virtual void OnNodeDetached
        (
            TNode formerParent
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Called before this node is assigned to a new parent
    /// </summary>
    /// <param name="targetParent"></param>
    protected virtual void OnNodeReparenting
        (
            TNode targetParent
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Called after this node is assigned to a new parent
    /// </summary>
    protected virtual void OnNodeReparented()
    {
        // пустое тело метода
    }

    /// <summary>
    /// Called when this node's children have been re-ordered
    /// </summary>
    protected virtual void OnChildrenReordered()
    {
        // пустое тело метода
    }
}
