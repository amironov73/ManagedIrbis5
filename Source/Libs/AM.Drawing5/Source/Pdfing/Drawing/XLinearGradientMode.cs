// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XLinearGradientMode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies the direction of a linear gradient.
    /// </summary>
    public enum XLinearGradientMode  // same values as System.Drawing.LinearGradientMode
    {
        /// <summary>
        /// Specifies a gradient from left to right.
        /// </summary>
        Horizontal = 0,

        /// <summary>
        /// Specifies a gradient from top to bottom.
        /// </summary>
        Vertical = 1,

        /// <summary>
        /// Specifies a gradient from upper left to lower right.
        /// </summary>
        ForwardDiagonal = 2,

        /// <summary>
        /// Specifies a gradient from upper right to lower left.
        /// </summary>
        BackwardDiagonal = 3,
    }
}
