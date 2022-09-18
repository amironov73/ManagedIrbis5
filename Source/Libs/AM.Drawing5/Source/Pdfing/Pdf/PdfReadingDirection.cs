// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Pdf;

/// <summary>
/// Specifies how the document should be displayed by a viewer when opened.
/// </summary>
public enum PdfReadingDirection
{
    /// <summary>
    /// Left to right.
    /// </summary>
    LeftToRight,

    /// <summary>
    /// Right to left (including vertical writing systems, such as Chinese, Japanese, and Korean)
    /// </summary>
    RightToLeft
}
