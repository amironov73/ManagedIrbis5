// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XGraphicsPathInternals.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#if GDI
using System.Drawing;
using System.Drawing.Drawing2D;
#endif
#if WPF
using System.Windows;
using System.Windows.Media;
#endif
//#if NETFX_CORE
//using Windows.UI.Xaml.Media;
///#endif

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Provides access to the internal data structures of XGraphicsPath.
    /// This class prevents the public interface from pollution with internal functions.
    /// </summary>
    public sealed class XGraphicsPathInternals
    {
        internal XGraphicsPathInternals(XGraphicsPath path)
        {
            _path = path;
        }
        XGraphicsPath _path;
    }
}
