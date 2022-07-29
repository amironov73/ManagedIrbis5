// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftSerializer.cs -- умеет сериализовать дерево PFT
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Serialization;

/// <summary>
/// Умеет сериализовать дерево PFT
/// </summary>
public static class PftSerializer
{
    #region Private members

    // !AST
    private static readonly byte[] _signature =
    {
        0x21, 0x41, 0x53, 0x54
    };

    private static int _CurrentVersion() =>
        ClientVersion.Version.Build;

    #endregion

    #region Public method

    /// <summary>
    /// Deserialize the node.
    /// </summary>
    public static PftNode Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        const string methodName = nameof (PftSerializer) + "::" + nameof (Deserialize);
        var code = reader.ReadByte();
        var mapping = TypeMap.FindCode (code);

        if (ReferenceEquals (mapping, null))
        {
            Magna.Logger.LogError
                (
                    methodName + ": unknown code {Code}",
                    code
                );
            Magna.Logger.LogError
                (
                    methodName + ": offset={Offset}",
                    reader.BaseStream.Position.ToString ("X")
                );

            throw new PftSerializationException ("Unknown code=" + code);
        }

        PftNode result;

        try
        {
            result = mapping.Create.ThrowIfNull()();
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    methodName
                );
            Magna.Logger.LogError
                (
                    methodName + ": can't create instance of {Type}",
                    mapping.Type!.AssemblyQualifiedName
                );
            Magna.Logger.LogError
                (
                    methodName + ": problem with code {Code}",
                    code
                );
            Magna.Logger.LogError
                (
                    methodName + "offset={Offset}",
                    reader.BaseStream.Position.ToString ("X")
                );

            throw new PftSerializationException
                (
                    "AST deserialization error",
                    exception
                );
        }

        result.Deserialize (reader);

        return result;
    }

    /// <summary>
    /// Deserialize the node collection.
    /// </summary>
    public static void Deserialize
        (
            BinaryReader reader,
            ICollection<PftNode> nodes
        )
    {
        Sure.NotNull (reader);
        Sure.NotNull (nodes);

        var count = reader.ReadPackedInt32();
        for (var i = 0; i < count; i++)
        {
            var node = Deserialize (reader);
            nodes.Add (node);
        }
    }

    /// <summary>
    /// Deserialize nullable node.
    /// </summary>
    public static PftNode? DeserializeNullable
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        var flag = reader.ReadBoolean();
        var result = flag
            ? Deserialize (reader)
            : null;

        return result;
    }

    /// <summary>
    /// Restore the program from the byte array.
    /// </summary>
    public static PftNode FromMemory
        (
            byte[] bytes
        )
    {
        Sure.NotNull (bytes);

        var memory = new MemoryStream (bytes);

        using var compressor = new DeflateStream (memory, CompressionMode.Decompress);
        using var reader = new BinaryReader (compressor, IrbisEncoding.Utf8);
        var result = Read (reader);

        return result;
    }

    /// <summary>
    /// Read the AST from the stream.
    /// </summary>
    public static PftNode Read
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        var signature = new byte[4];
        var read = reader.Read (signature, 0, 4);
        if (read != 4)
        {
            Magna.Logger.LogError
                (
                    nameof (PftSerializer) + "::" + nameof (Read)
                    + ": read too little {Read}",
                    read
                );
        }

        if (ArrayUtility.Compare (signature, _signature) != 0)
        {
            throw new IrbisException();
        }

        var actualVersion = reader.ReadInt32();
        var expectedVersion = _CurrentVersion();
        if (actualVersion != expectedVersion)
        {
            throw new IrbisException();
        }

        /*int offset = */
        reader.ReadInt32();

        //reader.BaseStream.Position = offset;
        var result = Deserialize (reader);

        return result;
    }

    /// <summary>
    /// Read the AST from the file.
    /// </summary>
    public static PftNode Read
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var stream = File.OpenRead (fileName);
        using var compressor = new DeflateStream (stream, CompressionMode.Decompress);
        using var reader = new BinaryReader (compressor, IrbisEncoding.Utf8);
        var result = Read (reader);

        return result;
    }

    /// <summary>
    /// Save the program to the stream.
    /// </summary>
    public static void Save
        (
            PftNode rootNode,
            BinaryWriter writer
        )
    {
        Sure.NotNull (rootNode);
        Sure.NotNull (writer);

        writer.Write (_signature);
        var version = _CurrentVersion();
        writer.Write (version);
        writer.Write (12);
        Serialize (writer, rootNode);
    }

    /// <summary>
    /// Save the program to the file.
    /// </summary>
    public static void Save
        (
            PftNode rootNode,
            string fileName
        )
    {
        Sure.NotNull (rootNode);
        Sure.NotNullNorEmpty (fileName);

        using var stream = File.Create (fileName);
        using var compressor = new DeflateStream (stream, CompressionMode.Compress);
        using var writer = new BinaryWriter (compressor, IrbisEncoding.Utf8);
        Save (rootNode, writer);
    }

    /// <summary>
    /// Serialize the node.
    /// </summary>
    public static void Serialize
        (
            BinaryWriter writer,
            PftNode node
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull (node);

        var nodeType = node.GetType();
        var mapping = TypeMap.FindType (nodeType);

        if (mapping is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftSerializer) + "::" + nameof (Serialize)
                    + ": unknown node type {Type}",
                    nodeType.AssemblyQualifiedName
                );

            var nodeInfo = node.GetNodeInfo();
            Magna.Logger.LogError
                (
                    nameof (PftSerializer) + "::" + nameof (Serialize)
                    + ": node {Info}",
                    nodeInfo.ToString()
                );

            throw new IrbisException ("Unknown node type=" + nodeType.AssemblyQualifiedName);
        }

        writer.Write (mapping.Code);
        node.Serialize (writer);
        writer.Flush();
    }

    /// <summary>
    /// Serialize the collection of <see cref="PftNode"/>s.
    /// </summary>
    public static void Serialize
        (
            BinaryWriter writer,
            ICollection<PftNode> nodes
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull (nodes);

        writer.WritePackedInt32 (nodes.Count);
        foreach (var node in nodes)
        {
            Serialize (writer, node);
        }
    }

    /// <summary>
    /// Serialize nullable node.
    /// </summary>
    public static void SerializeNullable
        (
            BinaryWriter writer,
            PftNode? node
        )
    {
        Sure.NotNull (writer);

        if (node is null)
        {
            writer.Write (false);
        }
        else
        {
            writer.Write (true);
            Serialize (writer, node);
        }
    }

    /// <summary>
    /// Save the program to byte array.
    /// </summary>
    public static byte[] ToMemory
        (
            PftNode rootNode
        )
    {
        Sure.NotNull (rootNode);

        // TODO Think about MemoryCenter.GetMemoryStream
        var memory = new MemoryStream();

        using (var compressor = new DeflateStream (memory, CompressionMode.Compress))
        using (var writer = new BinaryWriter (compressor, IrbisEncoding.Utf8))
        {
            Save (rootNode, writer);
        }

        return memory.ToArray();
    }

    #endregion
}
