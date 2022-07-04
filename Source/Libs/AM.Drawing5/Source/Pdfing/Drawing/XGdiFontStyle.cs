// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XGdiFontStyle
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Backward compatibility.
    /// </summary>
    [Flags]
    internal enum XGdiFontStyle  // Same values as System.Drawing.FontStyle.
    {
        // Must be identical to both:
        // System.Drawing.FontStyle and
        // PdfSharpCore.Drawing.FontStyle

        /// <summary>
        /// Normal text.
        /// </summary>
        Regular = 0,

        /// <summary>
        /// Bold text.
        /// </summary>
        Bold = 1,

        /// <summary>
        /// Italic text.
        /// </summary>
        Italic = 2,

        /// <summary>
        /// Bold and italic text. 
        /// </summary>
        BoldItalic = 3,

        /// <summary>
        /// Underlined text.
        /// </summary>
        Underline = 4,

        /// <summary>
        /// Text with a line through the middle.
        /// </summary>
        Strikeout = 8,
    }
}
