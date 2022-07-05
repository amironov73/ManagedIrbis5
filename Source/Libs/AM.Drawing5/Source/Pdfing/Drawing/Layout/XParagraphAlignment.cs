// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XParagraphAlignment.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.Layout
{
    /// <summary>
    /// Specifies the alignment of a paragraph.
    /// </summary>
    public enum XParagraphAlignment
    {
        /// <summary>
        /// Default alignment, typically left alignment.
        /// </summary>
        Default,

        /// <summary>
        /// The paragraph is rendered left aligned.
        /// </summary>
        Left,

        /// <summary>
        /// The paragraph is rendered centered.
        /// </summary>
        Center,

        /// <summary>
        /// The paragraph is rendered right aligned.
        /// </summary>
        Right,

        /// <summary>
        /// The paragraph is rendered justified.
        /// </summary>
        Justify,
    }
}
