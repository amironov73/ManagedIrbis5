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

/* YaleDawg.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Dawg;

internal sealed class YaleDawg<TPayload>
    : IDawg<TPayload>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public YaleDawg
        (
            BinaryReader reader,
            Func<BinaryReader, TPayload> readPayload
        )
    {
        Sure.NotNull (reader);
        Sure.NotNull (readPayload);

        _nodeCount = reader.ReadInt32();
        _rootNodeIndex = reader.ReadInt32();
        _payloads = reader.ReadArray (readPayload);
        _indexToChar = reader.ReadArray (r => r.ReadChar());

        var charToIndexPlusOne = CharToIndexPlusOneMap.Get (_indexToChar);
        YaleReader.ReadChildren (_indexToChar, _nodeCount, reader, out _firstChildForNode, out _children);

        _yaleGraph = new YaleGraph (_children, _firstChildForNode, charToIndexPlusOne!, _rootNodeIndex, _indexToChar);
    }

    #endregion

    #region Private members

    private readonly TPayload[] _payloads;
    private readonly char[] _indexToChar;
    private readonly int _nodeCount, _rootNodeIndex;
    private readonly int[] _firstChildForNode;
    private readonly YaleChild[] _children;
    private readonly YaleGraph _yaleGraph;

    TPayload IDawg<TPayload>.this [IEnumerable<char> word]
    {
        get
        {
            var node_i = GetPath (word).Last();

            if (node_i == -1)
            {
                return default!;
            }

            return GetPayload (node_i);
        }
    }

    private TPayload GetPayload (int node_i)
    {
        return node_i < _payloads.Length ? _payloads[node_i] : default!;
    }

    IEnumerable<int> GetPath (IEnumerable<char> word)
    {
        return _yaleGraph.GetPath (word);
    }

    int IDawg<TPayload>.GetLongestCommonPrefixLength (IEnumerable<char> word)
    {
        return GetPath (word).Count (i => i != -1) - 1; // -1 for root node
    }

    IEnumerable<KeyValuePair<string, TPayload>> IDawg<TPayload>.MatchPrefix (IEnumerable<char> prefix)
    {
        return MatchPrefix (prefix);
    }

    private IEnumerable<KeyValuePair<string, TPayload>> MatchPrefix (IEnumerable<char> prefix)
    {
        var prefixStr = prefix.AsString();

        var sb = new StringBuilder (prefixStr);

        foreach (var node_i in _yaleGraph.MatchPrefix (sb, GetPath (prefixStr).Last()))
        {
            var payload = GetPayload (node_i);

            if (!EqualityComparer<TPayload>.Default.Equals (payload, default))
            {
                yield return new KeyValuePair<string, TPayload> (sb.ToString(), payload);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<KeyValuePair<string, TPayload>> GetPrefixes
        (
            IEnumerable<char> key
        )
    {
        var sb = new StringBuilder();
        var keyStr = key.AsString();
        var strIndex = -1;

        foreach (var node_i in GetPath (keyStr))
        {
            if (node_i == -1)
            {
                break;
            }

            if (strIndex >= 0)
            {
                sb.Append (keyStr[strIndex]);
            }

            var payload = GetPayload (node_i);

            if (!EqualityComparer<TPayload>.Default.Equals (payload, default))
            {
                yield return new KeyValuePair<string, TPayload> (sb.ToString(), payload);
            }

            if (strIndex++ == keyStr.Length)
            {
                break;
            }
        }
    }

    int IDawg<TPayload>.GetNodeCount()
    {
        return _nodeCount;
    }

    public KeyValuePair<string, TPayload> GetRandomItem
        (
            Random random
        )
    {
        Sure.NotNull (random);

        var nodeIndex = random.Next (0, _payloads.Length);
        var payload = _payloads[nodeIndex];
        var sb = new StringBuilder();

        for (;;)
        {
            var childIndexes = _children.Select ((c, i) => new { c, i })
                .Where (t => t.c.Index == nodeIndex)
                .Select (t => t.i)
                .ToList();

            var childIndex = childIndexes[random.Next (0, childIndexes.Count)];

            sb.Insert (0, _indexToChar[_children[childIndex].CharIndex]);

            var parentIndex = Array.BinarySearch (_firstChildForNode, childIndex);

            if (parentIndex < 0)
            {
                parentIndex = ~parentIndex - 1;
            }
            else
            {
                while (parentIndex < _firstChildForNode.Length - 1 && _firstChildForNode[parentIndex + 1] == childIndex)
                    ++parentIndex;
            }

            if (parentIndex == _rootNodeIndex)
            {
                return new KeyValuePair<string, TPayload> (sb.ToString(), payload);
            }

            nodeIndex = parentIndex;
        }
    }

    #endregion
}
