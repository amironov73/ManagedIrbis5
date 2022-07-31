// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* SerialTreeNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace TreeCollections;

/// <summary>
/// Abstract tree node supporting one way (parent to child) hierarchical relationships
/// </summary>
/// <typeparam name="TNode"></typeparam>
public abstract class SerialTreeNode<TNode>
    where TNode: SerialTreeNode<TNode>, new()
{
    private readonly TNode _this;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="children"></param>
    protected SerialTreeNode
        (
            params TNode[]? children
        )
    {
        Children = children ?? Array.Empty<TNode>();
        _this = (TNode)this;
    }

    /// <summary>
    ///
    /// </summary>
    public TNode[] Children { get; set; }

    /// <summary>
    /// Filters all nodes matching a predicate starting from this node with pre-order traversal
    /// </summary>
    /// <param name="isMatch"></param>
    /// <returns></returns>
    public IEnumerable<TNode> Where
        (
            Func<TNode, bool> isMatch
        )
    {
        Sure.NotNull (isMatch);

        return FindRecursive (_this, isMatch).Where (_ => true);
    }

    /// <summary>
    /// Returns sequence of all nodes starting from this node with pre-order traversal
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TNode> SelectAll()
    {
        return Where (_ => true);
    }

    /// <summary>
    /// Returns first element of all nodes matching a predicate starting from this node with pre-order traversal
    /// </summary>
    /// <param name="isMatch"></param>
    /// <returns></returns>
    public TNode? FirstOrDefault
        (
            Func<TNode, bool> isMatch
        )
    {
        Sure.NotNull (isMatch);

        return Where (isMatch).FirstOrDefault();
    }

    /// <summary>
    /// Perform an action on each node starting from this node with pre-order traversal
    /// </summary>
    /// <param name="doIt"></param>
    public void ForEach
        (
            Action<TNode, int> doIt
        )
    {
        Sure.NotNull (doIt);

        ForEach (_this, 0, doIt);
    }

    /// <summary>
    /// Convert this node to a string representation
    /// </summary>
    /// <param name="toLineOfText"></param>
    /// <param name="indentation"></param>
    /// <returns></returns>
    public string ToString
        (
            Func<TNode, string> toLineOfText,
            int indentation = 5
        )
    {
        Sure.NotNull (toLineOfText);
        Sure.NonNegative (indentation);

        var builder = StringBuilderPool.Shared.Get();
        ForEach
            (
                (n, l) =>
                    builder.AppendLine ($"{string.Empty.PadLeft (l * indentation)}{toLineOfText (n)}")
            );

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    private static IEnumerable<TNode> FindRecursive
        (
            TNode node,
            Func<TNode, bool> isMatch
        )
    {
        if (isMatch (node)) yield return node;

        foreach (var descendant in node.Children.SelectMany (child => FindRecursive (child, isMatch)))
        {
            yield return descendant;
        }
    }

    private static void ForEach
        (
            TNode cur,
            int relativeLevel,
            Action<TNode, int> doIt
        )
    {
        doIt (cur, relativeLevel);

        relativeLevel++;

        foreach (var child in cur.Children)
        {
            ForEach (child, relativeLevel, doIt);
        }
    }
}
