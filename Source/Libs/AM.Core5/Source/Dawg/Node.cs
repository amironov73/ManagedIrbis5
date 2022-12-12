// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* Node.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal sealed class Node<TPayload>
{
    readonly Dictionary<char, Node<TPayload>> _children;

    /// <summary>
    ///
    /// </summary>
    public Node()
    {
        Payload = default!;
        _children = new Dictionary<char, Node<TPayload>>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="children"></param>
    internal Node (Dictionary<char, Node<TPayload>> children)
    {
        Payload = default!;
        _children = children;
    }

    public TPayload Payload { get; set; }

    public Node<TPayload> GetOrAddEdge (char c)
    {
        if (!_children.TryGetValue (c, out var newNode))
        {
            newNode = new Node<TPayload>();

            _children.Add (c, newNode);
        }

        return newNode;
    }

    public Node<TPayload>? GetChild (char c)
    {
        _children.TryGetValue (c, out var node);

        return node;
    }

    public bool HasChildren => _children.Count > 0;

    public Dictionary<char, Node<TPayload>> Children => _children;

    public IOrderedEnumerable<KeyValuePair<char, Node<TPayload>>> SortedChildren
        => _children.OrderBy (e => e.Key);

    public int GetRecursiveChildNodeCount()
        => GetAllDistinctNodes().Count();

    public IEnumerable<Node<TPayload>> GetAllDistinctNodes()
    {
        var visitedNodes = new HashSet<Node<TPayload>> { this };

        var stack = new Stack<IEnumerator<KeyValuePair<char, Node<TPayload>>>>();

        var enumerator = _children.GetEnumerator();

        stack.Push (enumerator);

        for (;;)
        {
            if (stack.Peek().MoveNext())
            {
                var node = stack.Peek().Current.Value;
                if (visitedNodes.Contains (node))
                {
                    continue;
                }

                visitedNodes.Add (node);
                stack.Push (node._children.GetEnumerator());
            }
            else
            {
                stack.Pop();
                if (stack.Count == 0)
                {
                    break;
                }
            }
        }

        return visitedNodes;
    }

    public bool HasPayload => !EqualityComparer<TPayload>.Default.Equals (Payload, default);
}
