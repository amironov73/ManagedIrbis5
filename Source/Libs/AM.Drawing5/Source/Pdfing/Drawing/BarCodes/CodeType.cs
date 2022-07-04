// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CodeType.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing.BarCodes
{
    /// <summary>
    /// Specifies the type of the bar code.
    /// </summary>
    public enum CodeType
    {
        /// <summary>
        /// The standard 2 of 5 interleaved bar code.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Code2of5Interleaved,

        /// <summary>
        /// The standard 3 of 9 bar code.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        Code3of9Standard,

        /// <summary>
        /// The OMR code.
        /// </summary>
        Omr,

        /// <summary>
        /// The data matrix code.
        /// </summary>
        DataMatrix,
    }
}
