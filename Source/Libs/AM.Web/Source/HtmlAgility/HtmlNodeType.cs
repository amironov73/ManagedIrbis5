// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace HtmlAgilityPack;

/// <summary>
/// Represents the type of a node.
/// </summary>
public enum HtmlNodeType
{
    /// <summary>
    /// The root of a document.
    /// </summary>
    Document,

    /// <summary>
    /// An HTML element.
    /// </summary>
    Element,

    /// <summary>
    /// An HTML comment.
    /// </summary>
    Comment,

    /// <summary>
    /// A text node is always the child of an element or a document node.
    /// </summary>
    Text,
}
