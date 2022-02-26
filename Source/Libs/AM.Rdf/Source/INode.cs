// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* INode.cs -- интерфейс узла
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Rdf;

/// <summary>
/// Интерфейс узла.
/// </summary>
public interface INode
{
    /// <summary>
    ///
    /// </summary>
    NodeType NodeType { get; }

    /// <summary>
    ///
    /// </summary>
    IGraph Graph { get; }
}
