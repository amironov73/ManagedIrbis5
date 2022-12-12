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

/// <summary>
///
/// </summary>
internal sealed class YaleGraph
{
    #region Nested classes

    class ChildComparer : IComparer<YaleChild>
    {
        public int Compare (YaleChild x, YaleChild y)
        {
            return x.CharIndex.CompareTo (y.CharIndex);
        }
    }

    class Frame
    {
        public int NodeIndex { get; }
        public IEnumerator<char> Enumerator { get; }

        public Frame
            (
                int nodeIndex,
                IEnumerator<char> enumerator
            )
        {
            NodeIndex = nodeIndex;
            Enumerator = enumerator;
        }

        public void Deconstruct
            (
                out int nodeIndex,
                out IEnumerator<char> enumerator
            )
        {
            nodeIndex = NodeIndex;
            enumerator = Enumerator;
        }
    }

    #endregion

    #region Construction

    public YaleGraph
        (
            YaleChild[] children,
            int[] firstChildForNode,
            ushort[] charToIndexPlusOne,
            int rootNodeIndex,
            char[] indexToChar
        )
    {
        _children = children;
        _firstChildForNode = firstChildForNode;
        _charToIndexPlusOne = charToIndexPlusOne;
        _indexToChar = indexToChar;
        _lastChar = _indexToChar.LastOrDefault();
        _firstChar = _indexToChar.FirstOrDefault();
        _rootNodeIndex = rootNodeIndex;
    }

    #endregion

    #region Private members

    private readonly int _rootNodeIndex;
    private readonly char _firstChar;
    private readonly char _lastChar;
    private readonly ushort[] _charToIndexPlusOne;
    private readonly int[] _firstChildForNode;
    private readonly YaleChild[] _children;
    private readonly char[] _indexToChar;

    private static readonly ChildComparer childComparer = new ();

    private int GetChildIndex (int node_i, char c)
    {
        if (c >= _firstChar && c <= _lastChar)
        {
            var charIndexPlusOne = _charToIndexPlusOne[c - _firstChar];

            if (charIndexPlusOne != 0)
            {
                var firstChild_i = _firstChildForNode[node_i];
                var lastChild_i = _firstChildForNode[node_i + 1];
                var nChildren = lastChild_i - firstChild_i;
                var charIndex = (ushort)(charIndexPlusOne - 1);
                int child_i;

                if (nChildren == 1)
                {
                    child_i = _children[firstChild_i].CharIndex == charIndex ? firstChild_i : -1;
                }
                else
                {
                    var searchValue = new YaleChild (-1, charIndex);

                    child_i = Array.BinarySearch (_children, firstChild_i, nChildren, searchValue, childComparer);
                }

                return child_i;
            }
        }

        return -1;
    }

    #endregion

    #region Public methods

    public int NodeCount => _firstChildForNode.Length - 1;

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<int> GetPath
        (
            IEnumerable<char> word
        )
    {
        Sure.NotNull (word);

        var node_i = _rootNodeIndex;

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
                node_i = _children[child_i].Index;

                yield return node_i;
                continue;
            }

            yield return -1;
            yield break;
        }
    }

    public IEnumerable<KeyValuePair<string, int>> MatchTree
        (
            IEnumerable<IEnumerable<char>> tree
        )
    {
        Sure.NotNull (tree);

        var node_i = _rootNodeIndex;
        var stack = new Stack<Frame>();
        var enums = tree.ToList();
        var sb = new StringBuilder (enums.Count);
        IEnumerator<char>? enumerator = null;

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
                    node_i = _children[childIndex].Index;
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

    public bool IsLeaf (int node_i)
    {
        return _firstChildForNode[node_i] == _firstChildForNode[node_i + 1];
    }

    public IEnumerable<int> MatchPrefix (StringBuilder sb, int node_i)
    {
        if (node_i != -1)
        {
            yield return node_i;

            var firstChild_i = _firstChildForNode[node_i];

            var lastChild_i = _firstChildForNode[node_i + 1];

            for (var i = firstChild_i; i < lastChild_i; ++i)
            {
                var child = _children[i];

                sb.Append (_indexToChar[child.CharIndex]);

                foreach (var child_node_i in MatchPrefix (sb, child.Index))
                {
                    yield return child_node_i;
                }

                --sb.Length;
            }
        }
    }

    #endregion
}
