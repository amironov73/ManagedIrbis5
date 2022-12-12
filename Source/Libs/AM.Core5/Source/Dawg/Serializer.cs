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

/* Serializer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Dawg;

static class Serializer
{
    public static void SaveAsYaleDawg<TPayload> (this Node<TPayload> root, BinaryWriter writer,
        Action<BinaryWriter, TPayload> writePayload)
    {
        const int version = 2;
        writer.Write (version);

        var allNodes = root.GetAllDistinctNodes().ToArray();

        Array.Sort (allNodes, new NodeByPayloadComparer<TPayload>());
        var totalChildCount = allNodes.Sum (n => n.Children.Count);

        writer.Write (allNodes.Length);

        var nodeIndex = allNodes
            .Select ((node, i) => new { node, i })
            .ToDictionary (t => t.node, t => t.i);

        if (!nodeIndex.TryGetValue (root, out var rootNodeIndex))
        {
            rootNodeIndex = -1;
        }

        writer.Write (rootNodeIndex);

        var nodesWithPayloads = allNodes.TakeWhile (n => n.HasPayload).ToArray();

        writer.WriteArray (nodesWithPayloads, (binaryWriter, node) => writePayload (binaryWriter, node.Payload));

        WriteCharsAndChildren (writer, allNodes, totalChildCount, nodeIndex);
    }

    private static void WriteCharsAndChildren<T> (BinaryWriter writer, Node<T>[] allNodes, int totalChildCount,
        Dictionary<Node<T>, int> nodeIndex)
    {
        var allChars = allNodes.SelectMany (node => node.Children.Keys).Distinct().OrderBy (c => c).ToArray();

        writer.WriteArray (allChars);

        writer.Write (totalChildCount);

        WriteChildrenNoLength (writer, allNodes, nodeIndex, allChars);
    }

    public static void SaveAsMultiDawg<TPayload> (BinaryWriter writer, Node<IList<TPayload>> root,
        Action<BinaryWriter, TPayload> writePayload)
    {
        var groups = root
            .GetAllDistinctNodes()
            .GroupBy (n => n.Payload.Count)
            .OrderByDescending (g => g.Key)
            .Select (g => new { PayloadCount = g.Key, Nodes = g.ToList() })
            .ToList();

        var allNodes = groups
            .SelectMany (g => g.Nodes)
            .ToArray();

        var totalChildCount = allNodes.Sum (n => n.Children.Count);

        writer.Write (allNodes.Length);

        var nodeIndex = allNodes
            .Select ((node, i) => new { node, i })
            .ToDictionary (t => t.node, t => t.i);

        if (!nodeIndex.TryGetValue (root, out var rootNodeIndex))
        {
            rootNodeIndex = -1;
        }

        writer.Write (rootNodeIndex);

        var maxPayloadCount = groups.FirstOrDefault()?.PayloadCount ?? 0;

        writer.Write (maxPayloadCount);

        for (var payload_i = 0; payload_i < maxPayloadCount; ++payload_i)
        {
            var ggs = groups.TakeWhile (g => g.PayloadCount > payload_i).ToList();

            writer.Write (ggs.Sum (g => g.Nodes.Count));

            foreach (var n in ggs.SelectMany (g => g.Nodes))
            {
                writePayload (writer, n.Payload[payload_i]);
            }
        }

        WriteCharsAndChildren (writer, allNodes, totalChildCount, nodeIndex);
    }

    public static void SaveAsMatrixDawg<TPayload> (this Node<TPayload> root, BinaryWriter writer,
        Action<BinaryWriter, TPayload> writePayload)
    {
        const int version = 1;
        writer.Write (version);

        var allNodes = root.GetAllDistinctNodes()
            .ToArray();

        writer.Write (allNodes.Length);

        var cube = new Node<TPayload> [2, 2][];

        var nodeGroups = allNodes.GroupBy (node => new { node.HasPayload, node.HasChildren })
            .ToDictionary (g => g.Key, g => g.ToArray());

        for (var p = 0; p < 2; ++p)
        for (var c = 0; c < 2; ++c)
        {
            var key = new { HasPayload = p != 0, HasChildren = c != 0 };
            cube[p, c] = nodeGroups.TryGetValue (key, out var arr) ? arr : Array.Empty<Node<TPayload>>();
        }

        var nodesWithPayloads = cube[1, 1].Concat (cube[1, 0]).ToArray();

        var nodeIndex = nodesWithPayloads.Concat (cube[0, 1].Concat (cube[0, 0]))
            .Select ((node, i) => new { node, i })
            .ToDictionary (t => t.node, t => t.i);

        var rootNodeIndex = nodeIndex[root];

        writer.Write (rootNodeIndex);

        writer.Write (nodesWithPayloads.Length);

        foreach (var node in nodesWithPayloads)
        {
            writePayload (writer, node.Payload);
        }

        var allChars = allNodes.SelectMany (node => node.Children.Keys).Distinct().OrderBy (c => c).ToArray();

        writer.Write (allChars.Length);

        foreach (var c in allChars)
        {
            writer.Write (c);
        }

        WriteChildren (writer, nodeIndex, cube[1, 1], allChars);
        WriteChildren (writer, nodeIndex, cube[0, 1], allChars);
    }

    private static void WriteChildren<TPayload> (BinaryWriter writer, Dictionary<Node<TPayload>, int> nodeIndex,
        Node<TPayload>[] nodes, char[] allChars)
    {
        writer.Write (nodes.Length);

        WriteChildrenNoLength (writer, nodes, nodeIndex, allChars);
    }

    private static void WriteChildrenNoLength<T> (BinaryWriter writer, IEnumerable<Node<T>> nodes,
        Dictionary<Node<T>, int> nodeIndex, char[] allChars)
    {
        var charToIndexPlusOne = CharToIndexPlusOneMap.Get (allChars);

        var firstChar = allChars.FirstOrDefault();

        foreach (var node in nodes)
        {
            WriteInt (writer, node.Children.Count, allChars.Length + 1);

            foreach (var child in node.Children.OrderBy (c => c.Key))
            {
                var charIndex = charToIndexPlusOne![child.Key - firstChar] - 1;

                WriteInt (writer, charIndex, allChars.Length);

                writer.Write (nodeIndex[child.Value]);
            }
        }
    }

    private static void WriteInt (BinaryWriter writer, int charIndex, int numPossibleValues)
    {
        if (numPossibleValues > 256)
        {
            writer.Write ((ushort)charIndex);
        }
        else
        {
            writer.Write ((byte)charIndex);
        }
    }
}
