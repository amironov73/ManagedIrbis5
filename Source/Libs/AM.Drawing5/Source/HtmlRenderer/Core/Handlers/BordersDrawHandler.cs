// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* BordersDrawHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Dom;
using AM.Drawing.HtmlRenderer.Core.Utils;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Handlers;

/// <summary>
/// Contains all the complex paint code to paint different style borders.
/// </summary>
internal static class BordersDrawHandler
{
    #region Fields and Consts

    /// <summary>
    /// used for all border paint to use the same points and not create new array each time.
    /// </summary>
    private static readonly RPoint[] _borderPts = new RPoint[4];

    #endregion

    /// <summary>
    /// Draws all the border of the box with respect to style, width, etc.
    /// </summary>
    /// <param name="graphics">the device to draw into</param>
    /// <param name="box">the box to draw borders for</param>
    /// <param name="rect">the bounding rectangle to draw in</param>
    /// <param name="isFirst">is it the first rectangle of the element</param>
    /// <param name="isLast">is it the last rectangle of the element</param>
    public static void DrawBoxBorders
        (
            RGraphics graphics,
            CssBox box,
            RRect rect,
            bool isFirst,
            bool isLast
        )
    {
        if (rect is { Width: > 0, Height: > 0 })
        {
            if (!(string.IsNullOrEmpty (box.BorderTopStyle) || box.BorderTopStyle == CssConstants.None ||
                  box.BorderTopStyle == CssConstants.Hidden) && box.ActualBorderTopWidth > 0)
            {
                DrawBorder (Border.Top, box, graphics, rect, isFirst, isLast);
            }

            if (isFirst && !(string.IsNullOrEmpty (box.BorderLeftStyle) || box.BorderLeftStyle == CssConstants.None ||
                             box.BorderLeftStyle == CssConstants.Hidden) && box.ActualBorderLeftWidth > 0)
            {
                DrawBorder (Border.Left, box, graphics, rect, true, isLast);
            }

            if (!(string.IsNullOrEmpty (box.BorderBottomStyle) || box.BorderBottomStyle == CssConstants.None ||
                  box.BorderBottomStyle == CssConstants.Hidden) && box.ActualBorderBottomWidth > 0)
            {
                DrawBorder (Border.Bottom, box, graphics, rect, isFirst, isLast);
            }

            if (isLast && !(string.IsNullOrEmpty (box.BorderRightStyle) || box.BorderRightStyle == CssConstants.None ||
                            box.BorderRightStyle == CssConstants.Hidden) && box.ActualBorderRightWidth > 0)
            {
                DrawBorder (Border.Right, box, graphics, rect, isFirst, true);
            }
        }
    }

    /// <summary>
    /// Draw simple border.
    /// </summary>
    /// <param name="border">Desired border</param>
    /// <param name="graphics">the device to draw to</param>
    /// <param name="box">Box which the border corresponds</param>
    /// <param name="brush">the brush to use</param>
    /// <param name="rectangle">the bounding rectangle to draw in</param>
    /// <returns>Beveled border path, null if there is no rounded corners</returns>
    public static void DrawBorder
        (
            Border border,
            RGraphics graphics,
            CssBox box,
            RBrush brush,
            RRect rectangle
        )
    {
        SetInOutsetRectanglePoints (border, box, rectangle, true, true);
        graphics.DrawPolygon (brush, _borderPts);
    }


    #region Private methods

    /// <summary>
    /// Draw specific border (top/bottom/left/right) with the box data (style/width/rounded).<br/>
    /// </summary>
    /// <param name="border">desired border to draw</param>
    /// <param name="box">the box to draw its borders, contain the borders data</param>
    /// <param name="graphics">the device to draw into</param>
    /// <param name="rect">the rectangle the border is enclosing</param>
    /// <param name="isLineStart">Specifies if the border is for a starting line (no bevel on left)</param>
    /// <param name="isLineEnd">Specifies if the border is for an ending line (no bevel on right)</param>
    private static void DrawBorder
        (
            Border border,
            CssBox box,
            RGraphics graphics,
            RRect rect,
            bool isLineStart,
            bool isLineEnd
        )
    {
        var style = GetStyle (border, box);
        var color = GetColor (border, box, style);

        var borderPath = GetRoundedBorderPath (graphics, border, box, rect);
        if (borderPath != null)
        {
            // rounded border need special path
            object? prevMode = null;
            if (box is { HtmlContainer: { AvoidGeometryAntialias: false }, IsRounded: true })
            {
                prevMode = graphics.SetAntiAliasSmoothingMode();
            }

            var pen = GetPen (graphics, style, color, GetWidth (border, box));
            using (borderPath)
                graphics.DrawPath (pen, borderPath);

            graphics.ReturnPreviousSmoothingMode (prevMode!);
        }
        else
        {
            // non rounded border
            if (style == CssConstants.Inset || style == CssConstants.Outset)
            {
                // inset/outset border needs special rectangle
                SetInOutsetRectanglePoints (border, box, rect, isLineStart, isLineEnd);
                graphics.DrawPolygon (graphics.GetSolidBrush (color), _borderPts);
            }
            else
            {
                // solid/dotted/dashed border draw as simple line
                var pen = GetPen (graphics, style, color, GetWidth (border, box));
                switch (border)
                {
                    case Border.Top:
                        graphics.DrawLine (pen, Math.Ceiling (rect.Left), rect.Top + box.ActualBorderTopWidth / 2,
                            rect.Right - 1, rect.Top + box.ActualBorderTopWidth / 2);
                        break;
                    case Border.Left:
                        graphics.DrawLine (pen, rect.Left + box.ActualBorderLeftWidth / 2, Math.Ceiling (rect.Top),
                            rect.Left + box.ActualBorderLeftWidth / 2, Math.Floor (rect.Bottom));
                        break;
                    case Border.Bottom:
                        graphics.DrawLine (pen, Math.Ceiling (rect.Left), rect.Bottom - box.ActualBorderBottomWidth / 2,
                            rect.Right - 1, rect.Bottom - box.ActualBorderBottomWidth / 2);
                        break;
                    case Border.Right:
                        graphics.DrawLine (pen, rect.Right - box.ActualBorderRightWidth / 2, Math.Ceiling (rect.Top),
                            rect.Right - box.ActualBorderRightWidth / 2, Math.Floor (rect.Bottom));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Set rectangle for inset/outset border as it need diagonal connection to other borders.
    /// </summary>
    /// <param name="border">Desired border</param>
    /// <param name="b">Box which the border corresponds</param>
    /// <param name="r">the rectangle the border is enclosing</param>
    /// <param name="isLineStart">Specifies if the border is for a starting line (no bevel on left)</param>
    /// <param name="isLineEnd">Specifies if the border is for an ending line (no bevel on right)</param>
    /// <returns>Beveled border path, null if there is no rounded corners</returns>
    private static void SetInOutsetRectanglePoints (Border border, CssBox b, RRect r, bool isLineStart, bool isLineEnd)
    {
        switch (border)
        {
            case Border.Top:
                _borderPts[0] = new RPoint (r.Left, r.Top);
                _borderPts[1] = new RPoint (r.Right, r.Top);
                _borderPts[2] = new RPoint (r.Right, r.Top + b.ActualBorderTopWidth);
                _borderPts[3] = new RPoint (r.Left, r.Top + b.ActualBorderTopWidth);
                if (isLineEnd)
                {
                    _borderPts[2].X -= b.ActualBorderRightWidth;
                }

                if (isLineStart)
                {
                    _borderPts[3].X += b.ActualBorderLeftWidth;
                }

                break;
            case Border.Right:
                _borderPts[0] = new RPoint (r.Right - b.ActualBorderRightWidth, r.Top + b.ActualBorderTopWidth);
                _borderPts[1] = new RPoint (r.Right, r.Top);
                _borderPts[2] = new RPoint (r.Right, r.Bottom);
                _borderPts[3] = new RPoint (r.Right - b.ActualBorderRightWidth, r.Bottom - b.ActualBorderBottomWidth);
                break;
            case Border.Bottom:
                _borderPts[0] = new RPoint (r.Left, r.Bottom - b.ActualBorderBottomWidth);
                _borderPts[1] = new RPoint (r.Right, r.Bottom - b.ActualBorderBottomWidth);
                _borderPts[2] = new RPoint (r.Right, r.Bottom);
                _borderPts[3] = new RPoint (r.Left, r.Bottom);
                if (isLineStart)
                {
                    _borderPts[0].X += b.ActualBorderLeftWidth;
                }

                if (isLineEnd)
                {
                    _borderPts[1].X -= b.ActualBorderRightWidth;
                }

                break;
            case Border.Left:
                _borderPts[0] = new RPoint (r.Left, r.Top);
                _borderPts[1] = new RPoint (r.Left + b.ActualBorderLeftWidth, r.Top + b.ActualBorderTopWidth);
                _borderPts[2] = new RPoint (r.Left + b.ActualBorderLeftWidth, r.Bottom - b.ActualBorderBottomWidth);
                _borderPts[3] = new RPoint (r.Left, r.Bottom);
                break;
        }
    }

    /// <summary>
    /// Makes a border path for rounded borders.<br/>
    /// To support rounded dotted/dashed borders we need to use arc in the border path.<br/>
    /// Return null if the border is not rounded.<br/>
    /// </summary>
    /// <param name="graphics">the device to draw into</param>
    /// <param name="border">Desired border</param>
    /// <param name="cssBox">Box which the border corresponds</param>
    /// <param name="rect">the rectangle the border is enclosing</param>
    /// <returns>Beveled border path, null if there is no rounded corners</returns>
    private static RGraphicsPath? GetRoundedBorderPath
        (
            RGraphics graphics,
            Border border,
            CssBox cssBox,
            RRect rect
        )
    {
        RGraphicsPath? path = null;
        switch (border)
        {
            case Border.Top:
                if (cssBox.ActualCornerNw > 0 || cssBox.ActualCornerNe > 0)
                {
                    path = graphics.GetGraphicsPath();
                    path.Start (rect.Left + cssBox.ActualBorderLeftWidth / 2,
                        rect.Top + cssBox.ActualBorderTopWidth / 2 + cssBox.ActualCornerNw);

                    if (cssBox.ActualCornerNw > 0)
                    {
                        path.ArcTo (rect.Left + cssBox.ActualBorderLeftWidth / 2 + cssBox.ActualCornerNw,
                            rect.Top + cssBox.ActualBorderTopWidth / 2, cssBox.ActualCornerNw, RGraphicsPath.Corner.TopLeft);
                    }

                    path.LineTo (rect.Right - cssBox.ActualBorderRightWidth / 2 - cssBox.ActualCornerNe,
                        rect.Top + cssBox.ActualBorderTopWidth / 2);

                    if (cssBox.ActualCornerNe > 0)
                    {
                        path.ArcTo (rect.Right - cssBox.ActualBorderRightWidth / 2,
                            rect.Top + cssBox.ActualBorderTopWidth / 2 + cssBox.ActualCornerNe, cssBox.ActualCornerNe,
                            RGraphicsPath.Corner.TopRight);
                    }
                }

                break;
            case Border.Bottom:
                if (cssBox.ActualCornerSw > 0 || cssBox.ActualCornerSe > 0)
                {
                    path = graphics.GetGraphicsPath();
                    path.Start (rect.Right - cssBox.ActualBorderRightWidth / 2,
                        rect.Bottom - cssBox.ActualBorderBottomWidth / 2 - cssBox.ActualCornerSe);

                    if (cssBox.ActualCornerSe > 0)
                    {
                        path.ArcTo (rect.Right - cssBox.ActualBorderRightWidth / 2 - cssBox.ActualCornerSe,
                            rect.Bottom - cssBox.ActualBorderBottomWidth / 2, cssBox.ActualCornerSe,
                            RGraphicsPath.Corner.BottomRight);
                    }

                    path.LineTo (rect.Left + cssBox.ActualBorderLeftWidth / 2 + cssBox.ActualCornerSw,
                        rect.Bottom - cssBox.ActualBorderBottomWidth / 2);

                    if (cssBox.ActualCornerSw > 0)
                    {
                        path.ArcTo (rect.Left + cssBox.ActualBorderLeftWidth / 2,
                            rect.Bottom - cssBox.ActualBorderBottomWidth / 2 - cssBox.ActualCornerSw, cssBox.ActualCornerSw,
                            RGraphicsPath.Corner.BottomLeft);
                    }
                }

                break;
            case Border.Right:
                if (cssBox.ActualCornerNe > 0 || cssBox.ActualCornerSe > 0)
                {
                    path = graphics.GetGraphicsPath();

                    var noTop = cssBox.BorderTopStyle == CssConstants.None || cssBox.BorderTopStyle == CssConstants.Hidden;
                    var noBottom = cssBox.BorderBottomStyle == CssConstants.None ||
                                   cssBox.BorderBottomStyle == CssConstants.Hidden;
                    path.Start (rect.Right - cssBox.ActualBorderRightWidth / 2 - (noTop ? cssBox.ActualCornerNe : 0),
                        rect.Top + cssBox.ActualBorderTopWidth / 2 + (noTop ? 0 : cssBox.ActualCornerNe));

                    if (cssBox.ActualCornerNe > 0 && noTop)
                    {
                        path.ArcTo (rect.Right - cssBox.ActualBorderLeftWidth / 2,
                            rect.Top + cssBox.ActualBorderTopWidth / 2 + cssBox.ActualCornerNe, cssBox.ActualCornerNe,
                            RGraphicsPath.Corner.TopRight);
                    }

                    path.LineTo (rect.Right - cssBox.ActualBorderRightWidth / 2,
                        rect.Bottom - cssBox.ActualBorderBottomWidth / 2 - cssBox.ActualCornerSe);

                    if (cssBox.ActualCornerSe > 0 && noBottom)
                    {
                        path.ArcTo (rect.Right - cssBox.ActualBorderRightWidth / 2 - cssBox.ActualCornerSe,
                            rect.Bottom - cssBox.ActualBorderBottomWidth / 2, cssBox.ActualCornerSe,
                            RGraphicsPath.Corner.BottomRight);
                    }
                }

                break;
            case Border.Left:
                if (cssBox.ActualCornerNw > 0 || cssBox.ActualCornerSw > 0)
                {
                    path = graphics.GetGraphicsPath();

                    var noTop = cssBox.BorderTopStyle == CssConstants.None || cssBox.BorderTopStyle == CssConstants.Hidden;
                    var noBottom = cssBox.BorderBottomStyle == CssConstants.None ||
                                   cssBox.BorderBottomStyle == CssConstants.Hidden;
                    path.Start (rect.Left + cssBox.ActualBorderLeftWidth / 2 + (noBottom ? cssBox.ActualCornerSw : 0),
                        rect.Bottom - cssBox.ActualBorderBottomWidth / 2 - (noBottom ? 0 : cssBox.ActualCornerSw));

                    if (cssBox.ActualCornerSw > 0 && noBottom)
                    {
                        path.ArcTo (rect.Left + cssBox.ActualBorderLeftWidth / 2,
                            rect.Bottom - cssBox.ActualBorderBottomWidth / 2 - cssBox.ActualCornerSw, cssBox.ActualCornerSw,
                            RGraphicsPath.Corner.BottomLeft);
                    }

                    path.LineTo (rect.Left + cssBox.ActualBorderLeftWidth / 2,
                        rect.Top + cssBox.ActualBorderTopWidth / 2 + cssBox.ActualCornerNw);

                    if (cssBox.ActualCornerNw > 0 && noTop)
                    {
                        path.ArcTo (rect.Left + cssBox.ActualBorderLeftWidth / 2 + cssBox.ActualCornerNw,
                            rect.Top + cssBox.ActualBorderTopWidth / 2, cssBox.ActualCornerNw, RGraphicsPath.Corner.TopLeft);
                    }
                }

                break;
        }

        return path;
    }

    /// <summary>
    /// Get pen to be used for border draw respecting its style.
    /// </summary>
    private static RPen GetPen
        (
            RGraphics graphics,
            string style,
            RColor color,
            double width
        )
    {
        var result = graphics.GetPen(color);
        result.Width = width;
        switch (style)
        {
            case "solid":
                result.DashStyle = RDashStyle.Solid;
                break;

            case "dotted":
                result.DashStyle = RDashStyle.Dot;
                break;

            case "dashed":
                result.DashStyle = RDashStyle.Dash;
                break;
        }

        return result;
    }

    /// <summary>
    /// Get the border color for the given box border.
    /// </summary>
    private static RColor GetColor
        (
            Border border,
            CssBoxProperties box,
            string style
        )
    {
        return border switch
        {
            Border.Top => style == CssConstants.Inset
                ? Darken (box.ActualBorderTopColor)
                : box.ActualBorderTopColor,

            Border.Right => style == CssConstants.Outset
                ? Darken (box.ActualBorderRightColor)
                : box.ActualBorderRightColor,

            Border.Bottom => style == CssConstants.Outset
                ? Darken (box.ActualBorderBottomColor)
                : box.ActualBorderBottomColor,

            Border.Left => style == CssConstants.Inset
                ? Darken (box.ActualBorderLeftColor)
                : box.ActualBorderLeftColor,

            _ => throw new ArgumentOutOfRangeException (nameof (border))
        };
    }

    /// <summary>
    /// Get the border width for the given box border.
    /// </summary>
    private static double GetWidth
        (
            Border border,
            CssBoxProperties box
        )
    {
        return border switch
        {
            Border.Top => box.ActualBorderTopWidth,
            Border.Right => box.ActualBorderRightWidth,
            Border.Bottom => box.ActualBorderBottomWidth,
            Border.Left => box.ActualBorderLeftWidth,
            _ => throw new ArgumentOutOfRangeException (nameof (border))
        };
    }

    /// <summary>
    /// Get the border style for the given box border.
    /// </summary>
    private static string GetStyle
        (
            Border border,
            CssBoxProperties box
        )
    {
        return border switch
        {
            Border.Top => box.BorderTopStyle,
            Border.Right => box.BorderRightStyle,
            Border.Bottom => box.BorderBottomStyle,
            Border.Left => box.BorderLeftStyle,
            _ => throw new ArgumentOutOfRangeException (nameof (border))
        };
    }

    /// <summary>
    /// Makes the specified color darker for inset/outset borders.
    /// </summary>
    private static RColor Darken
        (
            RColor color
        )
    {
        return RColor.FromArgb (color.R / 2, color.G / 2, color.B / 2);
    }

    #endregion
}
