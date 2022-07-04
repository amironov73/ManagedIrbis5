// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XColorSpace.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    ///<summary>
    /// Currently not used. Only DeviceRGB is rendered in PDF.
    /// </summary>
    public enum XColorSpace
    {
        /// <summary>
        /// Identifies the RGB color space.
        /// </summary>
        Rgb,

        /// <summary>
        /// Identifies the CMYK color space.
        /// </summary>
        Cmyk,

        /// <summary>
        /// Identifies the gray scale color space.
        /// </summary>
        GrayScale,
    }
}
