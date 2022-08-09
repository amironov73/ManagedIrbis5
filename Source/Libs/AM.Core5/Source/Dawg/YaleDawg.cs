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

class YaleDawg<TPayload> : IDawg<TPayload>
{
    private readonly TPayload[] payloads;
    private readonly char[] indexToChar;
    private readonly int nodeCount, rootNodeIndex;
    private readonly int[] firstChildForNode;
    private readonly YaleChild[] children;
    private readonly YaleGraph yaleGraph;

    public YaleDawg (BinaryReader reader, Func<BinaryReader, TPayload> readPayload)
    {
        nodeCount = reader.ReadInt32();

        rootNodeIndex = reader.ReadInt32();

        payloads = reader.ReadArray (readPayload);

        indexToChar = reader.ReadArray (r => r.ReadChar());

        var charToIndexPlusOne = CharToIndexPlusOneMap.Get (indexToChar);

        YaleReader.ReadChildren (indexToChar, nodeCount, reader, out firstChildForNode, out children);

        yaleGraph = new YaleGraph (children, firstChildForNode, charToIndexPlusOne, rootNodeIndex, indexToChar);
    }

    TPayload IDawg<TPayload>.this [IEnumerable<char> word]
    {
        get
        {
            var node_i = GetPath (word).Last();

            if (node_i == -1)
            {
                return default;
            }

            return GetPayload (node_i);
        }
    }

    private TPayload GetPayload (int node_i)
    {
        return node_i < payloads.Length ? payloads[node_i] : default;
    }

    IEnumerable<int> GetPath (IEnumerable<char> word)
    {
        return yaleGraph.GetPath (word);
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

        foreach (var node_i in yaleGraph.MatchPrefix (sb, GetPath (prefixStr).Last()))
        {
            var payload = GetPayload (node_i);

            if (!EqualityComparer<TPayload>.Default.Equals (payload, default))
            {
                yield return new KeyValuePair<string, TPayload> (sb.ToString(), payload);
            }
        }
    }

    public IEnumerable<KeyValuePair<string, TPayload>> GetPrefixes (IEnumerable<char> key)
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
        return nodeCount;
    }

    public KeyValuePair<string, TPayload> GetRandomItem (Random random)
    {
        var nodeIndex = random.Next (0, payloads.Length);

        var payload = payloads[nodeIndex];

        var sb = new StringBuilder();

        for (;;)
        {
            var childIndexes = children.Select ((c, i) => new { c, i })
                .Where (t => t.c.Index == nodeIndex)
                .Select (t => t.i)
                .ToList();

            var childIndex = childIndexes[random.Next (0, childIndexes.Count)];

            sb.Insert (0, indexToChar[children[childIndex].CharIndex]);

            var parentIndex = Array.BinarySearch (firstChildForNode, childIndex);

            if (parentIndex < 0)
            {
                parentIndex = ~parentIndex - 1;
            }
            else
            {
                while (parentIndex < firstChildForNode.Length - 1 && firstChildForNode[parentIndex + 1] == childIndex)
                    ++parentIndex;
            }

            if (parentIndex == rootNodeIndex)
            {
                return new KeyValuePair<string, TPayload> (sb.ToString(), payload);
            }

            nodeIndex = parentIndex;
        }
    }
}
