// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FontRunKind.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit;

/// <summary>
/// Indicates the kind of font run.
/// </summary>
public enum FontRunKind
{
    /// <summary>
    /// This is a normal text font run.
    /// </summary>
    Normal,

    /// <summary>
    /// This font run covers the trailing white space on a line.
    /// </summary>
    TrailingWhitespace,

    /// <summary>
    /// This is a special font run created for the truncation ellipsis.
    /// </summary>
    Ellipsis
}
