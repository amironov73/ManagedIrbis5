// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* ReadOnlyEntityTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace TreeCollections;

/// <summary>
/// Abstract entity tree node participating in a read-only hierarchical structure.
/// A tree can be built from the root node in a single operation, but the hierarchical structure cannot be modified after that.
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TItem"></typeparam>
public abstract class ReadOnlyEntityTreeNode<TNode, TId, TItem>
    : EntityTreeNode<TNode, TId, TItem>
    where TNode: ReadOnlyEntityTreeNode<TNode, TId, TItem>
{
    /// <summary>
    /// Root constructor
    /// </summary>
    /// <param name="definition">Entity definition that determines identity parameters</param>
    /// <param name="rootItem">Item contained by root</param>
    /// <param name="checkOptions">Options governing how uniqueness is enforced</param>
    protected ReadOnlyEntityTreeNode
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
    protected ReadOnlyEntityTreeNode
        (
            TItem item,
            TNode parent
        )
        : base (item, parent)
    {
        // пустое тело конструктора
    }

    /// <inheritdoc cref="TreeNode{TNode}.IsReadOnly"/>
    public sealed override bool IsReadOnly => true;
}
