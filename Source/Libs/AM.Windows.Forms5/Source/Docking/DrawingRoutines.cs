// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

public static class DrawingRoutines
{
    public static void SafelyDrawLinearGradient (this Rectangle rectangle, Color startColor, Color endColor,
        LinearGradientMode mode, Graphics graphics, Blend blend = null)
    {
        if (rectangle.Width > 0 && rectangle.Height > 0)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush (rectangle, startColor, endColor, mode))
            {
                if (blend != null)
                {
                    brush.Blend = blend;
                }

                graphics.FillRectangle (brush, rectangle);
            }
        }
    }

    public static void SafelyDrawLinearGradientF (this RectangleF rectangle, Color startColor, Color endColor,
        LinearGradientMode mode, Graphics graphics)
    {
        if (rectangle.Width > 0 && rectangle.Height > 0)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush (rectangle, startColor, endColor, mode))
            {
                graphics.FillRectangle (brush, rectangle);
            }
        }
    }
}