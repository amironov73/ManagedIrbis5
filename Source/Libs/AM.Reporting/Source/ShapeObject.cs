// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Specifies a kind of the shape.
    /// </summary>
    public enum ShapeKind
    {
        /// <summary>
        /// Specifies a rectangle shape.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Specifies a round rectangle shape.
        /// </summary>
        RoundRectangle,

        /// <summary>
        /// Specifies an ellipse shape.
        /// </summary>
        Ellipse,

        /// <summary>
        /// Specifies a triangle shape.
        /// </summary>
        Triangle,

        /// <summary>
        /// Specifies a diamond shape.
        /// </summary>
        Diamond
    }

    /// <summary>
    /// Represents a shape object.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="ShapeKind"/> property to specify a shape. To set the width, style and color of the
    /// shape's border, use the <b>Border.Width</b>, <b>Border.Style</b> and <b>Border.Color</b> properties.
    /// </remarks>
    public partial class ShapeObject : ReportComponentBase
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a shape kind.
        /// </summary>
        [DefaultValue (ShapeKind.Rectangle)]
        [Category ("Appearance")]
        public ShapeKind Shape { get; set; }

        /// <summary>
        /// Gets or sets a shape curvature if <see cref="ShapeKind"/> is <b>RoundRectangle</b>.
        /// </summary>
        /// <remarks>
        /// 0 value means automatic curvature.
        /// </remarks>
        [DefaultValue (0f)]
        [Category ("Appearance")]
        public float Curve { get; set; }

        #endregion

        #region Private Methods

#if MONO
    private GraphicsPath GetRoundRectPath(float x, float y, float x1, float y1, float radius)
    {
      GraphicsPath gp = new GraphicsPath();
      if (radius < 1)
        radius = 1;
      gp.AddLine(x + radius, y, x1 - radius, y);
      gp.AddArc(x1 - radius - 1, y, radius + 1, radius + 1, 270, 90);
      gp.AddLine(x1, y + radius, x1, y1 - radius);
      gp.AddArc(x1 - radius - 1, y1 - radius - 1, radius + 1, radius + 1, 0, 90);
      gp.AddLine(x1 - radius, y1, x + radius, y1);
      gp.AddArc(x, y1 - radius - 1, radius + 1, radius + 1, 90, 90);
      gp.AddLine(x, y1 - radius, x, y + radius);
      gp.AddArc(x, y, radius, radius, 180, 90);
      gp.CloseFigure();
      return gp;
    }
#else
        private GraphicsPath GetRoundRectPath (float x, float y, float x1, float y1, float radius)
        {
            var gp = new GraphicsPath();
            if (radius < 1)
            {
                radius = 1;
            }

            gp.AddArc (x1 - radius - 1, y, radius + 1, radius + 1, 270, 90);
            gp.AddArc (x1 - radius - 1, y1 - radius - 1, radius + 1, radius + 1, 0, 90);
            gp.AddArc (x, y1 - radius - 1, radius + 1, radius + 1, 90, 90);
            gp.AddArc (x, y, radius, radius, 180, 90);
            gp.CloseFigure();
            return gp;
        }
#endif

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as ShapeObject;
            Shape = src.Shape;
            Curve = src.Curve;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs eventArgs)
        {
            if (Math.Abs (Width) < 1 || Math.Abs (Height) < 1)
            {
                return;
            }

            var g = eventArgs.Graphics;
            var x = (AbsLeft + Border.Width / 2) * eventArgs.ScaleX;
            var y = (AbsTop + Border.Width / 2) * eventArgs.ScaleY;
            var dx = (Width - Border.Width) * eventArgs.ScaleX - 1;
            var dy = (Height - Border.Width) * eventArgs.ScaleY - 1;
            var x1 = x + dx;
            var y1 = y + dy;

            var pen = eventArgs.Cache.GetPen (Border.Color, Border.Width * eventArgs.ScaleX, Border.DashStyle);
            Brush brush = null;
            if (Fill is SolidFill)
            {
                brush = eventArgs.Cache.GetBrush ((Fill as SolidFill).Color);
            }
            else
            {
                brush = Fill.CreateBrush (new RectangleF (x, y, dx, dy), eventArgs.ScaleX, eventArgs.ScaleY);
            }

            var report = Report;
            if (report is { SmoothGraphics: true } && Shape != ShapeKind.Rectangle)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }

            switch (Shape)
            {
                case ShapeKind.Rectangle:
                    g.FillAndDrawRectangle (pen, brush, x, y, dx, dy);
                    break;

                case ShapeKind.RoundRectangle:
                    var min = Math.Min (dx, dy);
                    if (Curve == 0)
                    {
                        min = min / 4;
                    }
                    else
                    {
                        min = Math.Min (min, Curve * eventArgs.ScaleX * 10);
                    }

                    var gp = GetRoundRectPath (x, y, x1, y1, min);
                    g.FillAndDrawPath (pen, brush, gp);
                    gp.Dispose();
                    break;

                case ShapeKind.Ellipse:
                    g.FillAndDrawEllipse (pen, brush, x, y, dx, dy);
                    break;

                case ShapeKind.Triangle:
                    PointF[] triPoints =
                    {
                        new PointF (x1, y1), new PointF (x, y1), new PointF (x + dx / 2, y), new PointF (x1, y1)
                    };
                    g.FillAndDrawPolygon (pen, brush, triPoints);
                    break;

                case ShapeKind.Diamond:
                    PointF[] diaPoints =
                    {
                        new PointF (x + dx / 2, y), new PointF (x1, y + dy / 2), new PointF (x + dx / 2, y1),
                        new PointF (x, y + dy / 2)
                    };
                    g.FillAndDrawPolygon (pen, brush, diaPoints);
                    break;
            }

            if (Fill is not SolidFill)
            {
                brush.Dispose();
            }

            if (report is { SmoothGraphics: true })
            {
                g.InterpolationMode = InterpolationMode.Default;
                g.SmoothingMode = SmoothingMode.Default;
            }
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            Border.SimpleBorder = true;
            base.Serialize (writer);
            var c = writer.DiffObject as ShapeObject;

            if (Shape != c.Shape)
            {
                writer.WriteValue ("Shape", Shape);
            }

            if (Curve != c.Curve)
            {
                writer.WriteFloat ("Curve", Curve);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapeObject"/> class with default settings.
        /// </summary>
        public ShapeObject()
        {
            Shape = ShapeKind.Rectangle;
            FlagSimpleBorder = true;
        }
    }
}
