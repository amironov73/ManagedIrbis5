// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftSerializer.cs -- сериализатор AST-дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Runtime.Mere;

using MicroPft.Ast;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace MicroPft;

/// <summary>
/// Сериализатор AST-дерева.
/// </summary>
internal static class PftSerializer
{
    #region Nested classes

    internal class TypeMap
    {
        /// <summary>
        /// Код типа.
        /// </summary>
        public byte code;

        /// <summary>
        /// Собственно тип.
        /// </summary>
        public Type? type;
    }

    #endregion

    #region Private members

    private static readonly byte[] _signature = "!PFT"u8.ToArray();

    private static readonly TypeMap[] _map =
    {
        new() { code = 1, type = typeof (CNode) },
        new() { code = 2, type = typeof (CommaNode) },
        new() { code = 3, type = typeof (ConditionalNode) },
        new() { code = 4, type = typeof (FieldNode) },
        new() { code = 5, type = typeof (GroupNode) },
        new() { code = 6, type = typeof (ModeNode) },
        new() { code = 7, type = typeof (NewLineNode) },
        new() { code = 8, type = typeof (RepeatingNode) },
        new() { code = 9, type = typeof (UnconditionalNode) },
        new() { code = 10, type = typeof (XNode) },
    };

    #endregion

    #region Public methods

    public static PftNode Deserialize
        (
            BinaryReader reader
        )
    {
        var code = reader.ReadByte();
        var type = FindType (code).ThrowIfNull();
        var result = (PftNode) Activator.CreateInstance (type.type.ThrowIfNull())
            .ThrowIfNull();

        result.MereDeserialize (reader);

        return result;
    }

    public static TypeMap FindCode
        (
            Type type
        )
    {
        foreach (var item in _map)
        {
            if (ReferenceEquals (item.type, type))
            {
                return item;
            }
        }

        Magna.Logger.LogError ("Unknown type {Type}", type);
        throw new ArgumentOutOfRangeException ($"Unknown type {type}");
    }

    public static TypeMap FindType
        (
            byte code
        )
    {
        foreach (var item in _map)
        {
            if (item.code == code)
            {
                return item;
            }
        }

        Magna.Logger.LogError ("Unknown code {Code}", code);
        throw new ArgumentOutOfRangeException ($"Unknown code {code}");
    }

    public static void Serialize
        (
            BinaryWriter writer,
            PftNode node
        )
    {
        var nodeType = node.GetType();
        var mapping = FindCode (nodeType);

        writer.Write (mapping.code);
        node.MereSerialize (writer);
    }

    public static byte[] Serialize
        (
            IEnumerable<PftNode> nodes
        )
    {
        Sure.NotNull (nodes);

        var stream = new MemoryStream();
        var writer = new BinaryWriter (stream);

        writer.Write (_signature);
        foreach (var node in nodes)
        {
            MereSerializer.Serialize (writer, node);
        }

        writer.Flush();
        stream.Flush();

        return stream.ToArray();
    }

    #endregion
}
