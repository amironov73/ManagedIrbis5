// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* AddOperations.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region TreeNodeCopyOperations

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace TreeCollections;

public abstract partial class TreeNode<TNode>
{
    /// <summary>
    /// Deep copy of this node's descendants to a given SerialTreeNode parent/root using a filtering predicate.
    /// </summary>
    /// <typeparam name="TSNode"></typeparam>
    /// <param name="parent">Destination parent/root</param>
    /// <param name="copy">Copy function</param>
    /// <param name="allowNext">Predicate determining eligibility of source node and its descendants</param>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
    public void CopyTo<TSNode>
        (
            TSNode parent,
            Action<TNode, TSNode> copy,
            Func<TNode, bool> allowNext,
            int? maxRelativeDepth = null
        )
        where TSNode: SerialTreeNode<TSNode>, new()
    {
        if (!allowNext (This) || maxRelativeDepth <= 0)
        {
            return;
        }

        BuildSerializable (This, parent, copy, allowNext, 0, maxRelativeDepth ?? int.MaxValue);
    }

    /// <summary>
    /// Deep copy of this node's descendants to a given SerialTreeNode parent/root.
    /// </summary>
    /// <typeparam name="TSNode"></typeparam>
    /// <param name="parent">Destination parent/root</param>
    /// <param name="copy">Copy function</param>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
    public void CopyTo<TSNode>
        (
            TSNode parent,
            Action<TNode, TSNode> copy,
            int? maxRelativeDepth = null
        )
        where TSNode: SerialTreeNode<TSNode>, new()
    {
        CopyTo (parent, copy, _ => true, maxRelativeDepth);
    }

    private static void BuildSerializable<TSNode>
        (
            TNode sourceNode,
            TSNode destParent,
            Action<TNode, TSNode> copy,
            Func<TNode, bool> allowNext,
            int curDepth,
            int maxRelativeDepth
        )
        where TSNode: SerialTreeNode<TSNode>, new()
    {
        if (curDepth++ == maxRelativeDepth)
        {
            return;
        }

        var children = new List<TSNode>();

        foreach (var child in sourceNode.Children.Where (allowNext))
        {
            var destNode = new TSNode();
            copy (child, destNode);

            children.Add (destNode);

            BuildSerializable (child, destNode, copy, allowNext, curDepth, maxRelativeDepth);
        }

        destParent.Children = children.ToArray();
    }
}
