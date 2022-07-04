// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XCombineMode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies how different clipping regions can be combined.
    /// </summary>
    public enum XCombineMode  // Same values as System.Drawing.Drawing2D.CombineMode.
    {
        /// <summary>
        /// One clipping region is replaced by another.
        /// </summary>
        Replace = 0,

        /// <summary>
        /// Two clipping regions are combined by taking their intersection.
        /// </summary>
        Intersect = 1,

        /// <summary>
        /// Not yet implemented in PdfSharpCore.
        /// </summary>
        Union = 2,

        /// <summary>
        /// Not yet implemented in PdfSharpCore.
        /// </summary>
        Xor = 3,

        /// <summary>
        /// Not yet implemented in PdfSharpCore.
        /// </summary>
        Exclude = 4,

        /// <summary>
        /// Not yet implemented in PdfSharpCore.
        /// </summary>
        Complement = 5,
    }
}
