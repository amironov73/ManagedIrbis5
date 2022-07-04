// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XLineAlignment.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies the alignment of a text string relative to its layout rectangle
    /// </summary>
    public enum XLineAlignment  // same values as System.Drawing.StringAlignment (except BaseLine)
    {
        /// <summary>
        /// Specifies the text be aligned near the layout.
        /// In a left-to-right layout, the near position is left. In a right-to-left layout, the near
        /// position is right.
        /// </summary>
        Near = 0,

        /// <summary>
        /// Specifies that text is aligned in the center of the layout rectangle.
        /// </summary>
        Center = 1,

        /// <summary>
        /// Specifies that text is aligned far from the origin position of the layout rectangle.
        /// In a left-to-right layout, the far position is right. In a right-to-left layout, the far
        /// position is left. 
        /// </summary>
        Far = 2,

        /// <summary>
        /// Specifies that text is aligned relative to its base line.
        /// With this option the layout rectangle must have a height of 0.
        /// </summary>
        BaseLine = 3,
    }
}
