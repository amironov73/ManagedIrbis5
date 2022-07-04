// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XPageDirection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies the direction of the y-axis.
    /// </summary>
    public enum XPageDirection
    {
        /// <summary>
        /// Increasing Y values go downwards. This is the default.
        /// </summary>
        Downwards = 0,

        /// <summary>
        /// Increasing Y values go upwards. This is only possible when drawing on a PDF page.
        /// It is not implemented when drawing on a System.Drawing.Graphics object.
        /// </summary>
        [Obsolete("Not implemeted - yagni")]
        Upwards = 1, // Possible, but needs a lot of case differentiation - postponed.
    }
}
