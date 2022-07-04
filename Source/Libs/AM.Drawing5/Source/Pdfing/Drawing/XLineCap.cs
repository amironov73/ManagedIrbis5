// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XLineCap.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies the available cap styles with which an XPen object can start and end a line.
    /// </summary>
    public enum XLineCap
    {
        /// <summary>
        /// Specifies a flat line cap.
        /// </summary>
        Flat = 0,

        /// <summary>
        /// Specifies a round line cap.
        /// </summary>
        Round = 1,

        /// <summary>
        /// Specifies a square line cap.
        /// </summary>
        Square = 2
    }
}
