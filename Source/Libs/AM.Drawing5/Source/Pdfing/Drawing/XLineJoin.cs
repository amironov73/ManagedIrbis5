// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XLineJoin.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies how to join consecutive line or curve segments in a figure or subpath.
    /// </summary>
    public enum XLineJoin
    {
        /// <summary>
        /// Specifies a mitered join. This produces a sharp corner or a clipped corner,
        /// depending on whether the length of the miter exceeds the miter limit
        /// </summary>
        Miter = 0,

        /// <summary>
        /// Specifies a circular join. This produces a smooth, circular arc between the lines.
        /// </summary>
        Round = 1,

        /// <summary>
        /// Specifies a beveled join. This produces a diagonal corner.
        /// </summary>
        Bevel = 2,
    }
}
