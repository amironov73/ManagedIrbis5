// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* PageRange.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting;

/// <summary>
/// Specifies the page range to print/export.
/// </summary>
public enum PageRange
{
    /// <summary>
    /// Print all pages.
    /// </summary>
    All,

    /// <summary>
    /// Print current page.
    /// </summary>
    Current,

    /// <summary>
    /// Print pages specified in the <b>PageNumbers</b> property of the <b>PrintSettings</b>.
    /// </summary>
    PageNumbers
}
