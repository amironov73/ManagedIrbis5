// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XGraphicsPdfPageOptions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies how the content of an existing PDF page and new content is combined.
    /// </summary>
    public enum XGraphicsPdfPageOptions
    {
        /// <summary>
        /// The new content is inserted behind the old content and any subsequent drawing in done above the existing graphic.
        /// </summary>
        Append,

        /// <summary>
        /// The new content is inserted before the old content and any subsequent drawing in done beneath the existing graphic.
        /// </summary>
        Prepend,

        /// <summary>
        /// The new content entirely replaces the old content and any subsequent drawing in done on a blank page.
        /// </summary>
        Replace,
    }
}
