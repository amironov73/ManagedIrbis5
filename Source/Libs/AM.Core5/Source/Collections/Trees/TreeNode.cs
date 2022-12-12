// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* TreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Abstract tree node supporting two-way (parent to child and child to parent) hierarchical relationships.
/// Can enumerate with a variety of traversal strategies; defaults to pre-order.
/// </summary>
/// <typeparam name="TNode"></typeparam>
public abstract partial class TreeNode<TNode>
    : IEnumerable<TNode>, ITreeNode
    where TNode: TreeNode<TNode>
{
    private protected readonly List<TNode> ChildrenList;
    private readonly Lazy<HierarchyPosition> _hierarchyId;

    /// <summary>
    ///
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="children"></param>
    protected TreeNode
        (
            TNode? parent,
            List<TNode> children
        )
    {
        This = (TNode)this;
        Parent = parent;
        Root = parent?.Root ?? This;
        Level = parent?.Level + 1 ?? 0;

        ChildrenList = children;

        _hierarchyId = new Lazy<HierarchyPosition> (GetHierarchyId, LazyThreadSafetyMode.PublicationOnly);
    }

    /// <summary>
    ///
    /// </summary>
    public TNode Root { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public int Level { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public TNode? Parent { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public TNode? NextSibling { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public TNode? PreviousSibling { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public IReadOnlyList<TNode> Children => ChildrenList;

    /// <summary>
    ///
    /// </summary>
    public int OrderIndex => Parent?.ChildrenList.IndexOf (This) ?? -1;

    /// <summary>
    /// Hierarchical identity of this node
    /// </summary>
    public HierarchyPosition HierarchyId => IsReadOnly ? _hierarchyId.Value : GetHierarchyId();

    /// <summary>
    ///
    /// </summary>
    public abstract bool IsReadOnly { get; }

    /// <summary>
    /// Exposes enumeration with pre-order traversal and optional max depth of traversal (relative to this node)
    /// </summary>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
    /// <returns></returns>
    public IEnumerable<TNode> PreOrder (int? maxRelativeDepth = null)
    {
        return new EnumeratorHost<TNode> (new PreOrderEnumerator<TNode> (This, maxRelativeDepth)!);
    }

    /// <summary>
    /// Exposes enumeration with pre-order traversal, filtering predicate, and optional max depth of traversal (relative to this node).
    /// The filtering predicate will terminate traversing a branch if no children satisfy the predicate, even if deeper descendants do.
    /// </summary>
    /// <param name="allowNext">Predicate determining eligibility of node and its descendants</param>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
    /// <returns></returns>
    public IEnumerable<TNode> PreOrder (Func<TNode, bool> allowNext, int? maxRelativeDepth = null)
    {
        return new EnumeratorHost<TNode>
            (
                new PreOrderFilteringEnumerator<TNode> (This, allowNext, maxRelativeDepth)!
            );
    }

    /// <summary>
    /// Exposes enumeration with level-order (breadth-first) traversal and optional max depth of traversal (relative to this node)
    /// </summary>
    /// <param name="maxRelativeDepth">Max depth of traversal (relative to this node)</param>
    /// <returns></returns>
    public IEnumerable<TNode> LevelOrder (int? maxRelativeDepth = null)
    {
        return new EnumeratorHost<TNode>
            (
                new LevelOrderEnumerator<TNode> (This, maxRelativeDepth)!
            );
    }

    /// <summary>
    /// Exposes enumeration with supplied enumerator
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public IEnumerable<TNode> EnumerateWith (IEnumerator<TNode> enumerator)
    {
        return new EnumeratorHost<TNode> (enumerator);
    }

    /// <summary>
    /// Exposes enumeration with (default) pre-order traversal
    /// </summary>
    /// <returns></returns>
    public IEnumerator<TNode> GetEnumerator()
    {
        return new PreOrderEnumerator<TNode> (This)!;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///
    /// </summary>
    protected TNode This { get; }

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnNodeAttached()
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void SetChildErrorsOnAttachment()
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected virtual bool OnAddCanProceed() => true;

    private protected void AppendChildren (IEnumerable<TNode> children)
    {
        ChildrenList.AddRange (children);
    }

    private protected void SetChildrenSiblingReferences()
    {
        for (var i = 0; i != Children.Count; i++)
        {
            var cur = Children[i];
            cur.PreviousSibling = i != 0 ? Children[i - 1] : null;
            cur.NextSibling = i != Children.Count - 1 ? Children[i + 1] : null;
        }
    }

    private HierarchyPosition GetHierarchyId()
    {
        var ordinals =
            SelectPathDownward()
                .Select (n => n.Parent?.Children
                    .Select ((child, i) => new { child, index = i + 1 })
                    .First (ni => ni.child.Equals (n)).index ?? 0);

        return new HierarchyPosition (ordinals.ToArray());
    }
}
