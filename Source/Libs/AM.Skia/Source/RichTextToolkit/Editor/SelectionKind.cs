// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SelectionKind.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit.Editor;

/// <summary>
/// Defines a kind of selection range
/// </summary>
public enum SelectionKind
{
    /// <summary>
    /// No range
    /// </summary>
    None,

    /// <summary>
    /// Select a word
    /// </summary>
    Word,

    /// <summary>
    /// Select a line
    /// </summary>
    Line,

    /// <summary>
    /// Select a paragraph
    /// </summary>
    Paragraph,

    /// <summary>
    /// Select the entire document (ie: select all)
    /// </summary>
    Document
}
