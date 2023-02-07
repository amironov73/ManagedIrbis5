// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* Clocks.cs -- simple analog clock control
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Простое управление аналоговыми часами.
/// </summary>
[ToolboxBitmap (typeof (Clocks), "Images.Clocks.bmp")]
[System.ComponentModel.DesignerCategory ("Code")]
public sealed class Clocks
    : Control
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Clocks()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        Enabled = false;

        _timer = new Timer
        {
            Interval = 500,
            Enabled = true
        };
        _timer.Tick += _timer_Tick;
    }

    #endregion

    #region Private members

    private readonly Timer _timer;

    void _timer_Tick
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        Invalidate();
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="Control.DefaultSize"/>
    protected override Size DefaultSize => new (90, 90);

    /// <inheritdoc cref="Control.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        base.Dispose (disposing);
        _timer.Dispose();
    }

    /// <inheritdoc cref="Control.SetBoundsCore"/>
    protected override void SetBoundsCore
        (
            int x,
            int y,
            int width,
            int height,
            BoundsSpecified specified
        )
    {
        var min = (width < height) ? width : height;
        base.SetBoundsCore (x, y, min, min, specified);
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs e
        )
    {
        var g = e.Graphics;
        var r = ClientRectangle;
        r.Width--;
        r.Height--;
        var xx0 = r.Width / 2;
        var xy0 = r.Height / 2;
        g.TranslateTransform (xx0, xy0);
        var radius = (double)r.Height / 2 - 4;
        r = new Rectangle (-xx0, -xy0, r.Width, r.Height);
        var r2 = r;
        var dr = -r.Width / 30;
        if (dr > -3)
        {
            dr = -3;
        }

        r2.Inflate (dr, dr);

        using (Brush brush = new LinearGradientBrush (r,
                   Color.LightGray, Color.White, 45))
        {
            g.FillEllipse (brush, r);
        }

        using (var path = new GraphicsPath())
        {
            path.AddEllipse (r2);
            using (var pgb = new PathGradientBrush (path))
            {
                pgb.CenterPoint = new PointF (r.Left + r.Width / 4f,
                    r.Top + r.Height / 4f);
                pgb.CenterColor = Color.White;
                pgb.SurroundColors = new[] { Color.LightSkyBlue };
                g.FillEllipse (pgb, r2);
            }
        }

        g.DrawEllipse (Pens.Gray, r);
        if (r.Height > 100)
        {
            g.DrawEllipse (Pens.Gray, r2);
        }

        var fontHeight = (double)r.Height / 14;
        if (fontHeight >= 5)
        {
            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            var fontFamily = new FontFamily (GenericFontFamilies.SansSerif);
            var fontStyle = fontHeight >= 12 ? FontStyle.Bold : FontStyle.Regular;

            using var font = new Font (fontFamily, (float)fontHeight, fontStyle);

            for (double h = 1; h < 13; h++)
            {
                var hAngle = h * 30 * Math.PI / 180;
                var dx = Math.Sin (hAngle) * radius;
                var dy = Math.Cos (hAngle) * radius;
                var x0 = +dx;
                var y0 = -dy;
                var x1 = +dx * 0.80;
                var y1 = -dy * 0.80;
                var x2 = +dx * 0.95;
                var y2 = -dy * 0.95;
                g.DrawLine (Pens.Black, (float)x0, (float)y0,
                    (float)x2, (float)y2);
                g.DrawString (h.ToString ("#"), font, Brushes.Blue,
                    (float)x1, (float)y1, format);
            }
        }

        // Кружок в центре
        var dotRadius = r.Width / 8;
        var centerDot = new Rectangle
            (
                -dotRadius / 2,
                -dotRadius / 2,
                dotRadius,
                dotRadius
            );
        var grayDot = centerDot;
        grayDot.X += 2;
        grayDot.Y += 2;
        g.FillEllipse (Brushes.LightGray, grayDot);

        var t = DateTime.Now;

        // Минуты
        {
            var minAngle = t.Minute * 6 * Math.PI / 180;
            var dx = Math.Sin (minAngle) * radius;
            var dy = Math.Cos (minAngle) * radius;
            var x1 = +dx * 0.7;
            var y1 = -dy * 0.7;
            var x2 = -dx * 0.05;
            var y2 = +dy * 0.05;
            using (var p = new Pen (Brushes.LightGray, 2f))
            {
                g.DrawLine (p, (float)x1 + 2, (float)y1 + 2,
                    (float)x2 + 2, (float)y2 + 2);
            }

            using (var p = new Pen (Brushes.Black, 2f))
            {
                g.DrawLine (p, (float)x1, (float)y1,
                    (float)x2, (float)y2);
            }
        }

        // Секунды
        {
            var secAngle = t.Second * 6 * Math.PI / 180;
            var dx = Math.Sin (secAngle) * radius;
            var dy = Math.Cos (secAngle) * radius;
            var x1 = +dx * 0.85;
            var y1 = -dy * 0.85;
            var x2 = -dx * 0.10;
            var y2 = +dy * 0.10;
            g.DrawLine (Pens.LightGray, (float)x1 + 2, (float)y1 + 2,
                (float)x2 + 2, (float)y2 + 2);
            g.DrawLine (Pens.Black, (float)x1, (float)y1,
                (float)x2, (float)y2);
        }

        // Часы
        {
            var hourAngle = (t.Hour + t.Minute / 60.0) * 30 *
                Math.PI / 180;
            var dx = Math.Sin (hourAngle) * radius;
            var dy = Math.Cos (hourAngle) * radius;
            var x1 = +dx * 0.6;
            var y1 = -dy * 0.6;
            var x2 = -dx * 0.10;
            var y2 = +dy * 0.10;
            var x3 = +dy * 0.06;
            var y3 = +dx * 0.06;
            var x4 = -dy * 0.06;
            var y4 = -dx * 0.06;

            using var path = new GraphicsPath();
            path.AddEllipse (r2);

            using var pgb = new PathGradientBrush (path)
            {
                CenterPoint = new PointF (r2.Left, r2.Top),
                CenterColor = Color.LightBlue,
                SurroundColors = new[] { Color.Blue }
            };
            var points = new PointF[5];
            points[0] = new PointF ((float)x1, (float)y1);
            points[1] = new PointF ((float)x3, (float)y3);
            points[2] = new PointF ((float)x2, (float)y2);
            points[3] = new PointF ((float)x4, (float)y4);
            points[4] = new PointF ((float)x1, (float)y1);
            var grayPoints = new PointF[points.Length];
            for (var i = 0; i < points.Length; i++)
            {
                grayPoints[i].X = points[i].X + 2;
                grayPoints[i].Y = points[i].Y + 2;
            }

            g.FillPolygon (Brushes.LightGray, grayPoints, FillMode.Winding);
            g.FillPolygon (pgb, points, FillMode.Winding);
        }

        // Снова кружок в центре
        using (var path = new GraphicsPath())
        {
            path.AddEllipse (centerDot);
            using (var pgb = new PathGradientBrush (path))
            {
                pgb.CenterPoint = new PointF (-dotRadius / 4f,
                    -dotRadius / 4f);
                pgb.CenterColor = Color.White;
                pgb.SurroundColors = new[] { Color.Blue };
                g.FillEllipse (pgb, centerDot);
            }
        }
    }

    #endregion
}
