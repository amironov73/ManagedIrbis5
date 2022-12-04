// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* NodeOrderItemComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

namespace TreeCollections;

internal class NodeOrderItemComparer<TNode, TItem>
    : IComparer<TNode>
    where TNode: ItemTreeNode<TNode, TItem>
{
    #region Construction

    public NodeOrderItemComparer
        (
            IComparer<TItem> itemComparer
        )
    {
        _itemComparer = itemComparer;
    }

    #endregion

    #region Private members

    private readonly IComparer<TItem> _itemComparer;

    #endregion

    #region IComparer members

    /// <inheritdoc cref="IComparer{T}.Compare"/>
    public int Compare
        (
            TNode? x,
            TNode? y
        )
    {
        return _itemComparer.Compare (x!.Item, y!.Item);
    }

    #endregion
}
