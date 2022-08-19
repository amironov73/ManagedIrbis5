// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* DrawHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using static System.Math;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public static class DrawHelper
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static Point RtlTransform
        (
            Control control,
            Point point
        )
    {
        return control.RightToLeft != RightToLeft.Yes
            ? point
            : point with { X = control.Right - point.X };
    }

    /// <summary>
    ///
    /// </summary>
    public static Rectangle RtlTransform
        (
            Control control,
            Rectangle rectangle
        )
    {
        if (control.RightToLeft != RightToLeft.Yes)
        {
            return rectangle;
        }

        return rectangle with { X = control.ClientRectangle.Right - rectangle.Right };
    }

    /// <summary>
    ///
    /// </summary>
    public static GraphicsPath GetRoundedCornerTab
        (
            GraphicsPath? graphicsPath,
            Rectangle rect,
            bool upCorner
        )
    {
        if (graphicsPath is null)
        {
            graphicsPath = new GraphicsPath();
        }
        else
        {
            graphicsPath.Reset();
        }

        var curveSize = 6;
        if (upCorner)
        {
            graphicsPath.AddLine (rect.Left, rect.Bottom, rect.Left, rect.Top + curveSize / 2);
            graphicsPath.AddArc (new Rectangle (rect.Left, rect.Top, curveSize, curveSize), 180, 90);
            graphicsPath.AddLine (rect.Left + curveSize / 2, rect.Top, rect.Right - curveSize / 2, rect.Top);
            graphicsPath.AddArc (new Rectangle (rect.Right - curveSize, rect.Top, curveSize, curveSize), -90, 90);
            graphicsPath.AddLine (rect.Right, rect.Top + curveSize / 2, rect.Right, rect.Bottom);
        }
        else
        {
            graphicsPath.AddLine (rect.Right, rect.Top, rect.Right, rect.Bottom - curveSize / 2);
            graphicsPath.AddArc (new Rectangle (rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize), 0, 90);
            graphicsPath.AddLine (rect.Right - curveSize / 2, rect.Bottom, rect.Left + curveSize / 2, rect.Bottom);
            graphicsPath.AddArc (new Rectangle (rect.Left, rect.Bottom - curveSize, curveSize, curveSize), 90, 90);
            graphicsPath.AddLine (rect.Left, rect.Bottom - curveSize / 2, rect.Left, rect.Top);
        }

        return graphicsPath;
    }

    /// <summary>
    ///
    /// </summary>
    public static GraphicsPath CalculateGraphicsPathFromBitmap
        (
            Bitmap bitmap
        )
    {
        return CalculateGraphicsPathFromBitmap (bitmap, Color.Empty);
    }

    // From http://edu.cnzz.cn/show_3281.html
    /// <summary>
    ///
    /// </summary>
    public static GraphicsPath CalculateGraphicsPathFromBitmap
        (
            Bitmap bitmap,
            Color colorTransparent
        )
    {
        var graphicsPath = new GraphicsPath();
        if (colorTransparent == Color.Empty)
        {
            colorTransparent = bitmap.GetPixel (0, 0);
        }

        for (var row = 0; row < bitmap.Height; row++)
        {
            for (var col = 0; col < bitmap.Width; col++)
            {
                if (bitmap.GetPixel (col, row) != colorTransparent)
                {
                    var colOpaquePixel = col;
                    var colNext = col;
                    for (colNext = colOpaquePixel; colNext < bitmap.Width; colNext++)
                        if (bitmap.GetPixel (colNext, row) == colorTransparent)
                        {
                            break;
                        }

                    graphicsPath.AddRectangle (new Rectangle (colOpaquePixel, row, colNext - colOpaquePixel, 1));
                    col = colNext;
                }
            }
        }

        return graphicsPath;
    }

    /// <summary>
    ///
    /// </summary>
    public static int Balance
        (
            int length,
            int margin,
            int input,
            int lower,
            int upper
        )
    {
        return Max (Min (input, upper - length - margin), lower + margin);
    }

    #endregion
}
