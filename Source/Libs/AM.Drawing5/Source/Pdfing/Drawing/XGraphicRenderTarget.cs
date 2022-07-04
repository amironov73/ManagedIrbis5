// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XGraphicsRenderTarget.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Drawing
{
    ///<summary>
    /// Determines whether rendering based on GDI+ or WPF.
    /// For internal use in hybrid build only only.
    /// </summary>
    enum XGraphicTargetContext
    {
        // NETFX_CORE_TODO
        NONE = 0,

        /// <summary>
        /// Rendering does not depent on a particular technology.
        /// </summary>
        CORE = 1,

        /// <summary>
        /// Renders using GDI+.
        /// </summary>
        GDI = 2,

        /// <summary>
        /// Renders using WPF (including Silverlight).
        /// </summary>
        WPF = 3,

        /// <summary>
        /// Universal Windows Platform.
        /// </summary>
        UWP = 10,
    }
}
