// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* StreamMode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.Pdf
{
    /// <summary>
    /// Indicates whether we are within a BT/ET block.
    /// </summary>
    enum StreamMode
    {
        /// <summary>
        /// Graphic mode. This is default.
        /// </summary>
        Graphic,

        /// <summary>
        /// Text mode.
        /// </summary>
        Text,
    }
}
