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

/* YaleGraph.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Dawg;

class YaleGraph
{
    private readonly int rootNodeIndex;
    private readonly char firstChar;
    private readonly char lastChar;
    private readonly ushort[] charToIndexPlusOne;
    private readonly int[] firstChildForNode;
    private readonly YaleChild[] children;
    private readonly char[] indexToChar;

    public YaleGraph (YaleChild[] children,
        int[] firstChildForNode,
        ushort[] charToIndexPlusOne,
        int rootNodeIndex,
        char[] indexToChar)
    {
        this.children = children;
        this.firstChildForNode = firstChildForNode;
        this.charToIndexPlusOne = charToIndexPlusOne;
        this.indexToChar = indexToChar;
        lastChar = this.indexToChar.LastOrDefault();
        firstChar = this.indexToChar.FirstOrDefault();
        this.rootNodeIndex = rootNodeIndex;
    }

    public int NodeCount => firstChildForNode.Length - 1;

    public IEnumerable<int> GetPath (IEnumerable<char> word)
    {
        var node_i = rootNodeIndex;

        yield return node_i;

        if (node_i == -1)
        {
            yield break;
        }

        foreach (var c in word)
        {
            var child_i = GetChildIndex (node_i, c);

            if (child_i >= 0)
            {
                node_i = children[child_i].Index;

                yield return node_i;
                continue;
            }

            yield return -1;
            yield break;
        }
    }

    private int GetChildIndex (int node_i, char c)
    {
        if (c >= firstChar && c <= lastChar)
        {
            var charIndexPlusOne = charToIndexPlusOne[c - firstChar];

            if (charIndexPlusOne != 0)
            {
                var firstChild_i = firstChildForNode[node_i];

                var lastChild_i = firstChildForNode[node_i + 1];

                var nChildren = lastChild_i - firstChild_i;

                var charIndex = (ushort)(charIndexPlusOne - 1);

                int child_i;

                if (nChildren == 1)
                {
                    child_i = children[firstChild_i].CharIndex == charIndex ? firstChild_i : -1;
                }
                else
                {
                    var searchValue = new YaleChild (-1, charIndex);

                    child_i = Array.BinarySearch (children, firstChild_i, nChildren, searchValue, childComparer);
                }

                return child_i;
            }
        }

        return -1;
    }

    public IEnumerable<KeyValuePair<string, int>> MatchTree (IEnumerable<IEnumerable<char>> tree)
    {
        var node_i = rootNodeIndex;

        var stack = new Stack<Frame>();

        var enums = tree.ToList();

        var sb = new StringBuilder (enums.Count);

        IEnumerator<char> enumerator = null;

        for (;;)
        {
            if (enumerator != null)
            {
                var childIndex = -1;
                while (enumerator.MoveNext())
                {
                    childIndex = GetChildIndex (node_i, enumerator.Current);

                    if (childIndex >= 0)
                    {
                        break;
                    }
                }

                if (childIndex >= 0)
                {
                    sb.Append (enumerator.Current);
                    stack.Push (new Frame (node_i, enumerator));
                    node_i = children[childIndex].Index;
                    enumerator = null;
                }
                else
                {
                    enumerator.Dispose();

                    if (stack.Count == 0)
                    {
                        yield break;
                    }

                    (node_i, enumerator) = stack.Pop();
                    --sb.Length;
                }
            }
            else if (stack.Count < enums.Count)
            {
                enumerator = enums[stack.Count].GetEnumerator();
            }
            else
            {
                yield return new KeyValuePair<string, int> (sb.ToString(), node_i);

                if (stack.Count == 0)
                {
                    yield break;
                }

                (node_i, enumerator) = stack.Pop();
                --sb.Length;
            }
        }
    }

    class Frame
    {
        public Frame (int nodeIndex, IEnumerator<char> enumerator)
        {
            NodeIndex = nodeIndex;
            Enumerator = enumerator;
        }

        public int NodeIndex { get; }
        public IEnumerator<char> Enumerator { get; }

        public void Deconstruct (out int nodeIndex, out IEnumerator<char> enumerator)
        {
            nodeIndex = NodeIndex;
            enumerator = Enumerator;
        }
    }

    private static readonly ChildComparer childComparer = new ();

    class ChildComparer : IComparer<YaleChild>
    {
        public int Compare (YaleChild x, YaleChild y)
        {
            return x.CharIndex.CompareTo (y.CharIndex);
        }
    }

    public bool IsLeaf (int node_i)
    {
        return firstChildForNode[node_i] == firstChildForNode[node_i + 1];
    }

    public IEnumerable<int> MatchPrefix (StringBuilder sb, int node_i)
    {
        if (node_i != -1)
        {
            yield return node_i;

            var firstChild_i = firstChildForNode[node_i];

            var lastChild_i = firstChildForNode[node_i + 1];

            for (var i = firstChild_i; i < lastChild_i; ++i)
            {
                var child = children[i];

                sb.Append (indexToChar[child.CharIndex]);

                foreach (var child_node_i in MatchPrefix (sb, child.Index))
                {
                    yield return child_node_i;
                }

                --sb.Length;
            }
        }
    }
}
