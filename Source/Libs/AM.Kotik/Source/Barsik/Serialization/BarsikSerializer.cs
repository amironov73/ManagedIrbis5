// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BarsikSerializer.cs -- сериализатор Барсик-дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Kotik.Ast;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Serialization;

/// <summary>
/// Сериализатор Барсик-дерева.
/// </summary>
public static class BarsikSerializer
{
    #region Private members

    // !BAST
    private static readonly byte[] _signature =
    {
        0x21, 0x42, 0x41, 0x53, 0x54
    };

    private static readonly int _version = 1;

    #endregion

    #region Public methods

    /// <summary>
    /// Сохранение дерева в поток.
    /// </summary>
    public static void Save
        (
            AstNode rootNode,
            BinaryWriter writer
        )
    {
        Sure.NotNull (rootNode);
        Sure.NotNull (writer);

        writer.Write (_signature);
        writer.Write (_version);
        writer.Write (12);
        SerializeNode (writer, rootNode);
    }

    /// <summary>
    /// Serialize the node.
    /// </summary>
    public static void SerializeNode
        (
            BinaryWriter writer,
            AstNode node
        )
    {
        Sure.NotNull (writer);
        Sure.NotNull (node);

        var nodeType = node.GetType();
        var mapping = TypeMap.FindType (nodeType);

        if (ReferenceEquals(mapping, null))
        {
            Magna.Logger.LogError
                (
                    nameof (BarsikSerializer) + "::" + nameof (SerializeNode)
                    + ": unknown node type {Type}",
                    nodeType.AssemblyQualifiedName
                );

            throw new BarsikException
                (
                    "Unknown node type="
                    + nodeType.AssemblyQualifiedName
                );
        }

        writer.Write (mapping.Code);
        node.MereSerialize (writer);
        writer.Flush();
    }

    #endregion
}
