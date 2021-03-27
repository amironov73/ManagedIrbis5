// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftSerializer.cs --
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

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    /// <summary>
    ///
    /// </summary>
    public static class PftSerializer
    {
        #region Private members

        private static readonly byte[] _signature =
        {
            0x21, 0x41, 0x53, 0x54
        };

        private static int _CurrentVersion() =>
            throw new NotImplementedException();
            //Connection.ClientVersion.Revision;

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
            var code = reader.ReadByte();
            var mapping = TypeMap.FindCode(code);

            if (ReferenceEquals(mapping, null))
            {
                Magna.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "unknown code="
                        + code
                    );
                Magna.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "offset="
                        + reader.BaseStream.Position.ToString("X")
                    );

                throw new PftSerializationException
                    (
                        "Unknown code="
                        + code
                    );
            }

            PftNode result;

            try
            {
                result = mapping.Create();
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftSerializer::Deserialize",
                        exception
                    );
                Magna.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "can't create instance of "
                        + mapping.Type.AssemblyQualifiedName
                    );
                Magna.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "problem with code="
                        + code
                    );
                Magna.Error
                    (
                        "PftSerializer::Deserialize: "
                        + "offset="
                        + reader.BaseStream.Position.ToString("X")
                    );

                throw new PftSerializationException
                    (
                        "AST deserialization error",
                        exception
                    );
            }

            result.Deserialize(reader);

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
            var count = reader.ReadPackedInt32();
            for (var i = 0; i < count; i++)
            {
                var node = Deserialize(reader);
                nodes.Add(node);
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
            var flag = reader.ReadBoolean();
            var result = flag
                ? Deserialize(reader)
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
            var memory = new MemoryStream(bytes);

            using var compressor
                = new DeflateStream(memory, CompressionMode.Decompress);
            using var reader
                = new BinaryReader(compressor, IrbisEncoding.Utf8);
                var result = Read(reader);

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
            var signature = new byte[4];
            reader.Read(signature, 0, 4);
            if (ArrayUtility.Compare(signature, _signature) != 0)
            {
                throw new IrbisException();
            }

            var actualVersion = reader.ReadInt32();
            var expectedVersion = _CurrentVersion();
            if (actualVersion != expectedVersion)
            {
                throw new IrbisException();
            }

            /*int offset = */ reader.ReadInt32();
            //reader.BaseStream.Position = offset;
            var result = Deserialize(reader);

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
            using var stream = File.OpenRead(fileName);
            using var compressor
                = new DeflateStream(stream, CompressionMode.Decompress);
            using var reader
                = new BinaryReader(compressor, IrbisEncoding.Utf8);
            var result = Read(reader);

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
            writer.Write(_signature);
            var version = _CurrentVersion();
            writer.Write(version);
            writer.Write(12);
            Serialize(writer, rootNode);
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
            using (Stream stream = File.Create(fileName))

            using (var compressor
                = new DeflateStream(stream, CompressionMode.Compress))
            using (var writer
                = new BinaryWriter(compressor, IrbisEncoding.Utf8))
            {
                Save(rootNode, writer);
            }
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
            var nodeType = node.GetType();
            var mapping = TypeMap.FindType(nodeType);

            if (ReferenceEquals(mapping, null))
            {
                Magna.Error
                    (
                        "PftSerializer::Serialize: "
                        + "unknown node type="
                        + nodeType.AssemblyQualifiedName
                    );
                var nodeInfo = node.GetNodeInfo();
                Magna.Error
                    (
                        nodeInfo.ToString()
                    );

                throw new IrbisException
                    (
                        "Unknown node type="
                        + nodeType.AssemblyQualifiedName
                    );
            }

            writer.Write(mapping.Code);
            node.Serialize(writer);
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
            writer.WritePackedInt32(nodes.Count);
            foreach (var node in nodes)
            {
                Serialize(writer, node);
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
            if (ReferenceEquals(node, null))
            {
                writer.Write(false);
            }
            else
            {
                writer.Write(true);
                Serialize(writer, node);
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
            // TODO Think about MemoryManager.GetMemoryStream
            var memory = new MemoryStream();

            using (var compressor
                = new DeflateStream(memory, CompressionMode.Compress))
            using (var writer
                = new BinaryWriter(compressor, IrbisEncoding.Utf8))
            {
                Save(rootNode, writer);
            }

            return memory.ToArray();
        }

        #endregion
    }
}
