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

/* MultiDawgBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
public sealed class MultiDawgBuilder<TPayload>
    : DawgBuilder<IList<TPayload>>
{
    private const int Version = 0;
    private static readonly int Signature = BitConverter.ToInt32 (Encoding.UTF8.GetBytes ("MDWG"), 0);

    public MultiDawgBuilder()
    {
    }

    internal MultiDawgBuilder (Node<IList<TPayload>> root) : base (root)
    {
    }

    public MultiDawg<TPayload> BuildMultiDawg()
    {
        FuseEnds();

        var stream = new MemoryStream();
        SaveTo (stream);
        stream.Position = 0;

        return LoadFrom (stream);
    }

    public void FuseEnds()
    {
        new LevelBuilder<IList<TPayload>> (new SequenceEqualityComparer<TPayload>()).MergeEnds (_root);
    }

    public static MultiDawg<TPayload> LoadFrom (Stream stream)
    {
        var reader = BuiltinTypeIO.TryGetReader<TPayload>()
                     ?? throw new Exception ($"No built in reader found for type {nameof (TPayload)}.");

        return LoadFrom (stream, reader);
    }

    public static MultiDawg<TPayload> LoadFrom (Stream stream, Func<BinaryReader, TPayload> readPayload)
    {
        var reader = new BinaryReader (stream);
        if (reader.ReadInt32() != Signature)
        {
            throw new Exception ("Invalid signature.");
        }

        if (reader.ReadInt32() != Version)
        {
            throw new Exception ("Invalid version.");
        }

        var nodeCount = reader.ReadInt32();
        var rootNodeIndex = reader.ReadInt32();

        var payloads = reader.ReadArray (r => r.ReadArray (readPayload));

        var indexToChar = reader.ReadArray (r => r.ReadChar());

        var charToIndexPlusOne = CharToIndexPlusOneMap.Get (indexToChar);

        YaleReader.ReadChildren (indexToChar, nodeCount, reader, out var firstChildForNode, out var children);
        var yaleGraph = new YaleGraph (children, firstChildForNode, charToIndexPlusOne, rootNodeIndex, indexToChar);
        return new MultiDawg<TPayload> (yaleGraph, payloads);
    }

    public void SaveTo (Stream stream)
    {
        var writer = new BinaryWriter (stream);
        writer.Write (Signature);
        writer.Write (Version);
        Serializer.SaveAsMultiDawg (writer, _root, BuiltinTypeIO.GetWriter<TPayload>());
        writer.Flush(); // do not close the stream
    }

    internal static MultiDawgBuilder<TPayload> MergeMulti (Dictionary<char, Node<IList<TPayload>>> children)
    {
        var root = new Node<IList<TPayload>> (children);
        return new MultiDawgBuilder<TPayload> (root);
    }
}
