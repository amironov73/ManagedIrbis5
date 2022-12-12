// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* OperationHelpers.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace TreeCollections;

public abstract partial class MutableEntityTreeNode<TNode, TId, TItem>
{
    private void MoveToNonSiblingAdjacentPosition (TNode targetNode, Adjacency adjacency)
    {
        OnNodeReparenting (targetNode);

        var targetIndex = targetNode.OrderIndex + (adjacency == Adjacency.Before ? 0 : 1);

        var newParent = targetNode.Parent;

        Detach();
        newParent!.AttachChildOnMove (This, targetIndex);
    }

    private void MoveToSiblingAdjacentPosition (TNode targetNode, Adjacency adjacency)
    {
        var curIndex = OrderIndex;
        var targetIndex = targetNode.OrderIndex;

        var diff = targetIndex - curIndex;
        if (diff == 0) return;

        targetIndex += (diff < 0 ? 0 : -1) + (adjacency == Adjacency.Before ? 0 : 1);

        Parent!.ChildrenList.Remove (This);
        Parent.ChildrenList.Insert (targetIndex, This);

        Parent.SetChildrenSiblingReferences();

        Parent.OnChildrenReordered();
    }

    private void AttachChildOnAdd (TNode node, int? insertIndex = null)
    {
        InnerAddChild (node, insertIndex);

        SetChildrenSiblingReferences();

        node.SetErrorsAfterAddingThis();
        node.OnNodeAttached();
    }

    private void AttachChildOnMove (TNode node, int? insertIndex = null)
    {
        node.Parent = This;

        InnerAddChild (node, insertIndex);

        SetChildrenSiblingReferences();

        foreach (var n in node)
        {
            n.Level = n.Parent!.Level + 1;
            n.Root = Root;
            n.TreeIdMap = TreeIdMap;
        }

        node.SetErrorsAfterMovingThis();
        node.OnNodeAttached();
        node.OnNodeReparented();
    }

    private void InnerAddChild (TNode node, int? insertIndex)
    {
        if (insertIndex < 0)
        {
            throw new ArgumentOutOfRangeException (nameof (insertIndex));
        }

        if (!insertIndex.HasValue || insertIndex.Value >= ChildrenList.Count)
        {
            ChildrenList.Add (node);
        }
        else
        {
            ChildrenList.Insert (insertIndex.Value, node);
        }
    }

    private bool HasSameIdentityAs (TNode other) => HasEquivalentId (other.Id);
    private bool HasSameAliasAs (TNode other) => Definition.AliasEqualityComparer.Equals (Item, other.Item);

    private bool IsCompatible (TNode externalNode)
    {
        return externalNode.Definition.Equals (Definition) &&
               externalNode.CheckOptions == CheckOptions;
    }
}
