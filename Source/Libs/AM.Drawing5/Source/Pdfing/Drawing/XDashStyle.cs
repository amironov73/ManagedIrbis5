// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XDashStyle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies the style of dashed lines drawn with an XPen object.
    /// </summary>
    public enum XDashStyle  // Same values as System.Drawing.Drawing2D.DashStyle.
    {
        /// <summary>
        /// Specifies a solid line.
        /// </summary>
        Solid = 0,

        /// <summary>
        /// Specifies a line consisting of dashes.
        /// </summary>
        Dash = 1,

        /// <summary>
        /// Specifies a line consisting of dots.
        /// </summary>
        Dot = 2,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot.
        /// </summary>
        DashDot = 3,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot-dot.
        /// </summary>
        DashDotDot = 4,

        /// <summary>
        /// Specifies a user-defined custom dash style.
        /// </summary>
        Custom = 5,
    }
}
