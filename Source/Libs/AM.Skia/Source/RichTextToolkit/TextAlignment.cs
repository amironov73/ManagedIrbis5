// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Skia.RichTextKit
{
    /// <summary>
    /// Species the alignment of text within a text block
    /// </summary>
    public enum TextAlignment
    {
        /// <summary>
        /// Use base direction of the text block.
        /// </summary>
        Auto,

        /// <summary>
        /// Left-aligns text to a x-coord of 0.
        /// </summary>
        Left,

        /// <summary>
        /// Center aligns text between 0 and <see cref="TextBlock.MaxWidth"/> unless not
        /// specified in which case it uses the widest line in the text block.
        /// </summary>
        Center,

        /// <summary>
        /// Right aligns text <see cref="TextBlock.MaxWidth"/> unless not
        /// specified in which case it right aligns to the widest line in the text block.
        /// </summary>
        Right,
    }
}
