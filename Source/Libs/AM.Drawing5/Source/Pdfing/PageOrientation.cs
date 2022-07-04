// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PageOrientation.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore
{
    /// <summary>
    /// Specifies the orientation of a page.
    /// </summary>
    public enum PageOrientation
    {
        /// <summary>
        /// The default page orientation.
        /// </summary>
        Portrait,

        /// <summary>
        /// The width and height of the page are reversed.
        /// </summary>
        Landscape,
    }
}
