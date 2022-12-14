// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DrawingRoutines.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public static class DrawingRoutines
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="rectangle"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="mode"></param>
    /// <param name="graphics"></param>
    /// <param name="blend"></param>
    public static void SafelyDrawLinearGradient
        (
            this Rectangle rectangle,
            Color startColor,
            Color endColor,
            LinearGradientMode mode,
            Graphics graphics,
            Blend? blend = null
        )
    {
        if (rectangle is { Width: > 0, Height: > 0 })
        {
            using (var brush = new LinearGradientBrush (rectangle, startColor, endColor, mode))
            {
                if (blend != null)
                {
                    brush.Blend = blend;
                }

                graphics.FillRectangle (brush, rectangle);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="rectangle"></param>
    /// <param name="startColor"></param>
    /// <param name="endColor"></param>
    /// <param name="mode"></param>
    /// <param name="graphics"></param>
    public static void SafelyDrawLinearGradientF
        (
            this RectangleF rectangle,
            Color startColor,
            Color endColor,
            LinearGradientMode mode,
            Graphics graphics
        )
    {
        if (rectangle is { Width: > 0, Height: > 0 })
        {
            using (var brush = new LinearGradientBrush (rectangle, startColor, endColor, mode))
            {
                graphics.FillRectangle (brush, rectangle);
            }
        }
    }
}
