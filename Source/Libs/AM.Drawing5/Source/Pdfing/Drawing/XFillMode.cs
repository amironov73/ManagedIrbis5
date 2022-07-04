// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XFillMode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies how the interior of a closed path is filled.
    /// </summary>
    public enum XFillMode  // Same values as System.Drawing.FillMode.
    {
        /// <summary>
        /// Specifies the alternate fill mode. Called the 'odd-even rule' in PDF terminology.
        /// </summary>
        Alternate = 0,

        /// <summary>
        /// Specifies the winding fill mode. Called the 'nonzero winding number rule' in PDF terminology.
        /// </summary>
        Winding = 1,
    }
}
