// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CodeDirection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.BarCodes
{
    /// <summary>
    /// Specifies the drawing direction of the code.
    /// </summary>
    public enum CodeDirection
    {
        /// <summary>
        /// Does not rotate the code.
        /// </summary>
        LeftToRight,

        /// <summary>
        /// Rotates the code 180° at the anchor position.
        /// </summary>
        BottomToTop,

        /// <summary>
        /// Rotates the code 180° at the anchor position.
        /// </summary>
        RightToLeft,

        /// <summary>
        /// Rotates the code 180° at the anchor position.
        /// </summary>
        TopToBottom,
    }
}
