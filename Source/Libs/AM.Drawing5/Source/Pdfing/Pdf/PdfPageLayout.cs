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
/// Specifies the page layout to be used by a viewer when the document is opened.
/// </summary>
public enum PdfPageLayout
{
    /// <summary>
    /// Display one page at a time.
    /// </summary>
    SinglePage,

    /// <summary>
    /// Display the pages in one column.
    /// </summary>
    OneColumn,

    /// <summary>
    /// Display the pages in two columns, with oddnumbered pages on the left.
    /// </summary>
    TwoColumnLeft,

    /// <summary>
    /// Display the pages in two columns, with oddnumbered pages on the right.
    /// </summary>
    TwoColumnRight,

    /// <summary>
    /// (PDF 1.5) Display the pages two at a time, with odd-numbered pages on the left.
    /// </summary>
    TwoPageLeft,

    /// <summary>
    /// (PDF 1.5) Display the pages two at a time, with odd-numbered pages on the right.
    /// </summary>
    TwoPageRight
}
