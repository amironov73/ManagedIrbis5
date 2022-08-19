// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Win32Helper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public static class Win32Helper
{
    #region Private members

    internal static Control? ControlAtPoint (Point pt)
    {
        return Control.FromChildHandle (Win32.NativeMethods.WindowFromPoint (pt));
    }

    internal static uint MakeLong (int low, int high)
    {
        return (uint)((high << 16) + low);
    }

    internal static uint HitTestCaption (Control control)
    {
        var captionRectangle = new Rectangle
            (
                0,
                0,
                control.Width,
                control.ClientRectangle.Top - control.PointToClient (control.Location).X
            );

        return captionRectangle.Contains (Control.MousePosition) ? (uint)2 : 0;
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static bool IsRunningOnMono { get; } = Type.GetType ("Mono.Runtime") != null;

    #endregion
}
