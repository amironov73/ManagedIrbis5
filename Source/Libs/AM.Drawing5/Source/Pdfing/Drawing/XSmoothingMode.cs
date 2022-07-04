// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XSmoothingMode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies whether smoothing (or antialiasing) is applied to lines and curves
    /// and the edges of filled areas.
    /// </summary>
    [Flags]
    public enum XSmoothingMode  // same values as System.Drawing.Drawing2D.SmoothingMode
    {
        // Not used in PDF rendering process.

        /// <summary>
        /// Specifies an invalid mode.
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// Specifies the default mode.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Specifies high speed, low quality rendering.
        /// </summary>
        HighSpeed = 1,

        /// <summary>
        /// Specifies high quality, low speed rendering.
        /// </summary>
        HighQuality = 2,

        /// <summary>
        /// Specifies no antialiasing.
        /// </summary>
        None = 3,

        /// <summary>
        /// Specifies antialiased rendering.
        /// </summary>
        AntiAlias = 4,
    }
}
