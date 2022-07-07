// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontTechnology.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Fonts.OpenType
{
    /// <summary>
    /// Identifies the technology of an OpenType font file.
    /// </summary>
    enum FontTechnology
    {
        /// <summary>
        /// Font is Adobe Postscript font in CFF.
        /// </summary>
        PostscriptOutlines,

        /// <summary>
        /// Font is a TrueType font.
        /// </summary>
        TrueTypeOutlines,

        /// <summary>
        /// Font is a TrueType font collection.
        /// </summary>
        TrueTypeCollection
    }
}
