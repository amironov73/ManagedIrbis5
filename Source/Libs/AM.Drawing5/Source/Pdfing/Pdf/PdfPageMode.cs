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
public enum PdfPageMode
{
    /// <summary>
    /// Neither document outline nor thumbnail images visible.
    /// </summary>
    UseNone,

    /// <summary>
    /// Document outline visible.
    /// </summary>
    UseOutlines,

    /// <summary>
    /// Thumbnail images visible.
    /// </summary>
    UseThumbs,

    /// <summary>
    /// Full-screen mode, with no menu bar, windowcontrols, or any other window visible.
    /// </summary>
    FullScreen,

    /// <summary>
    /// (PDF 1.5) Optional content group panel visible.
    /// </summary>
    UseOC,

    /// <summary>
    /// (PDF 1.6) Attachments panel visible.
    /// </summary>
    UseAttachments
}
