// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TextKind.cs -- kind of the text
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Text;

/// <summary>
/// Kind of the text.
/// </summary>
public enum TextKind
{
    /// <summary>
    /// Plain text.
    /// </summary>
    PlainText,

    /// <summary>
    /// Rich text (RTF).
    /// </summary>
    RichText,

    /// <summary>
    /// HTML text.
    /// </summary>
    Html
}
