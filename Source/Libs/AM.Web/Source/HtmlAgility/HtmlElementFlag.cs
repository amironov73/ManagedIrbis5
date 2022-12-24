// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* HtmlElementFlag.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Flags that describe the behavior of an Element node.
/// </summary>
[Flags]
public enum HtmlElementFlag
{
    /// <summary>
    /// The node is a CDATA node.
    /// </summary>
    CData = 1,

    /// <summary>
    /// The node is empty. META or IMG are example of such nodes.
    /// </summary>
    Empty = 2,

    /// <summary>
    /// The node will automatically be closed during parsing.
    /// </summary>
    Closed = 4,

    /// <summary>
    /// The node can overlap.
    /// </summary>
    CanOverlap = 8
}
