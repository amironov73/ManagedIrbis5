// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseNullableAnnotationInsteadOfAttribute

/* ItemTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Abstract tree node that refines TreeNode by including a payload item
/// </summary>
/// <typeparam name="TNode"></typeparam>
/// <typeparam name="TItem"></typeparam>
public abstract partial class ItemTreeNode<TNode, TItem>
    : TreeNode<TNode>, IItemTreeNode<TItem>
    where TNode: ItemTreeNode<TNode, TItem>
{
    private bool _isBuilt;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    protected ItemTreeNode
        (
            TItem item,
            TNode? parent
        )
        : base (parent, new List<TNode>())
    {
        Item = item;
    }

    /// <summary>
    ///
    /// </summary>
    public TItem Item { get; }

    /// <summary>
    /// Abstract factory method for generating a descendant
    /// </summary>
    /// <param name="item"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    protected abstract TNode Create
        (
            TItem item,
            TNode parent
        );

    internal void Build
        (
            IReadOnlyList<TItem> childItems
        )
    {
        if (!IsReadOnly)
        {
            InnerBuild (childItems);
            return;
        }

        if (_isBuilt)
        {
            throw new InvalidOperationException ("Cannot add children to a read-only tree that has been built");
        }

        _isBuilt = true;
        InnerBuild (childItems);
    }

    private void InnerBuild
        (
            IReadOnlyCollection<TItem> childItems
        )
    {
        if (childItems.Count == 0 || !OnAddCanProceed())
        {
            return;
        }

        var newNodes =
            childItems
                .Select (item => Create (item, This))
                .ToArray();

        AppendChildren (newNodes);

        SetChildrenSiblingReferences();
        SetChildErrorsOnAttachment();

        newNodes.ForEach (n => n.OnNodeAttached());
    }
}
