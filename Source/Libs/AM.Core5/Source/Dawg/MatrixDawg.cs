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

/* MatrixDawg.cs --
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

internal sealed class MatrixDawg<TPayload>
    : IDawg<TPayload>
{
    public TPayload this [IEnumerable<char> word]
    {
        get
        {
            var node_i = rootNodeIndex;

            foreach (var c in word)
            {
                var childIndexPlusOne = GetChildIndexPlusOne (node_i, c);

                if (childIndexPlusOne == 0)
                {
                    return default!;
                }

                node_i = childIndexPlusOne - 1;
            }

            if (node_i == -1)
            {
                return default!;
            }

            return node_i < payloads.Length ? payloads[node_i] : default!;
        }
    }

    /// <summary>
    /// Returns a series of node indices
    /// </summary>
    IEnumerable<int> GetPath (IEnumerable<char> word)
    {
        var node_i = rootNodeIndex;

        yield return node_i;

        foreach (var c in word)
        {
            var childIndexPlusOne = GetChildIndexPlusOne (node_i, c);

            if (childIndexPlusOne == 0)
            {
                yield return -1;
                yield break;
            }

            node_i = childIndexPlusOne - 1;

            yield return node_i;
        }
    }

    private int GetChildIndexPlusOne (int node_i, char c)
    {
        var children = node_i < payloads.Length ? children1 : children0;

        if (node_i >= payloads.Length)
        {
            node_i -= payloads.Length;
        }

        if (node_i >= children.GetLength (0))
        {
            return 0; // node has no children
        }

        if (c < firstChar)
        {
            return 0;
        }

        if (c > lastChar)
        {
            return 0;
        }

        var charIndexPlusOne = charToIndexPlusOne[c - firstChar];

        if (charIndexPlusOne == 0)
        {
            return 0;
        }

        return children[node_i, charIndexPlusOne - 1];
    }

    public int GetLongestCommonPrefixLength (IEnumerable<char> word)
    {
        return GetPath (word).Count (i => i != -1) - 1;
    }

    struct StackItem
    {
        public int node_i, child_i;
    }

    public IEnumerable<KeyValuePair<string, TPayload>> MatchPrefix (IEnumerable<char> prefix)
    {
        var prefixStr = prefix.AsString();

        var node_i = prefixStr.Length == 0 ? rootNodeIndex : GetPath (prefixStr).Last();

        var stack = new Stack<StackItem>();

        if (node_i != -1)
        {
            if (node_i < payloads.Length)
            {
                var payload = payloads[node_i];

                if (!EqualityComparer<TPayload>.Default.Equals (payload, default))
                {
                    yield return new KeyValuePair<string, TPayload> (prefixStr, payload);
                }
            }

            var sb = new StringBuilder (prefixStr);

            var child_i = -1;

            for (;;)
            {
                var children = node_i < payloads.Length ? children1 : children0;

                var adj_node_i = (node_i >= payloads.Length)
                    ? node_i - payloads.Length
                    : node_i;

                if (adj_node_i < children.GetLength (0))
                {
                    var next_child_i = child_i + 1;

                    for (; next_child_i < indexToChar.Length; ++next_child_i)
                    {
                        if (children[adj_node_i, next_child_i] != 0)
                        {
                            break;
                        }
                    }

                    if (next_child_i < indexToChar.Length)
                    {
                        stack.Push (new StackItem { node_i = node_i, child_i = next_child_i });
                        sb.Append (indexToChar[next_child_i]);
                        node_i = children[adj_node_i, next_child_i] - 1;

                        if (node_i < payloads.Length)
                        {
                            var payload = payloads[node_i];

                            if (!EqualityComparer<TPayload>.Default.Equals (payload, default))
                            {
                                yield return new KeyValuePair<string, TPayload> (sb.ToString(), payload);
                            }
                        }

                        continue;
                    }
                }

                // No (more) children.

                if (stack.Count == 0)
                {
                    break;
                }

                --sb.Length;
                var item = stack.Pop();

                node_i = item.node_i;
                child_i = item.child_i;
            }
        }
    }

    IEnumerable<KeyValuePair<string, TPayload>> IDawg<TPayload>.GetPrefixes (IEnumerable<char> key)
    {
        throw new NotImplementedException();
    }

    public void SaveAsOldDawg (Stream stream, Action<BinaryWriter, TPayload> writePayload)
    {
        throw new NotImplementedException();
    }

    public int GetNodeCount()
    {
        return nodeCount;
    }

    public KeyValuePair<string, TPayload> GetRandomItem (Random random)
    {
        throw new NotImplementedException();
    }

    private readonly TPayload[] payloads;
    private readonly int[,] children1;
    private readonly int[,] children0;
    private readonly char[] indexToChar;
    private readonly ushort[] charToIndexPlusOne;
    private readonly int nodeCount, rootNodeIndex;
    private readonly char firstChar;
    private readonly char lastChar;

    /// <summary>
    ///
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="readPayload"></param>
    public MatrixDawg
        (
            BinaryReader reader,
            Func<BinaryReader,
                TPayload> readPayload
        )
    {
        charToIndexPlusOne = null!;


        // The nodes are grouped by (has payload, has children).
        nodeCount = reader.ReadInt32();

        rootNodeIndex = reader.ReadInt32();

        payloads = reader.ReadArray (readPayload);

        indexToChar = reader.ReadArray (r => r.ReadChar());

        charToIndexPlusOne = CharToIndexPlusOneMap.Get (indexToChar)!;

        children1 = ReadChildren (reader, indexToChar);
        children0 = ReadChildren (reader, indexToChar);

        firstChar = indexToChar.FirstOrDefault();
        lastChar = indexToChar.LastOrDefault();
    }

    private static int[,] ReadChildren (BinaryReader reader, char[] indexToChar)
    {
        var nodeCount = reader.ReadUInt32();

        var children = new int [nodeCount, indexToChar.Length];

        for (var node_i = 0; node_i < nodeCount; ++node_i)
        {
            var childCount = YaleReader.ReadInt (reader, indexToChar.Length + 1);

            for (ushort child_i = 0; child_i < childCount; ++child_i)
            {
                var charIndex = YaleReader.ReadInt (reader, indexToChar.Length);
                var childNodeIndex = reader.ReadInt32();

                children[node_i, charIndex] = childNodeIndex + 1;
            }
        }

        return children;
    }
}
