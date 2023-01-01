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

using System.Drawing;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

using System.ComponentModel;

#endregion

#nullable enable

namespace AM.Reporting.Gauge.Radial
{
    /// <summary>
    /// Represents a linear pointer.
    /// </summary>
#if !DEBUG
    [DesignTimeVisible(false)]
#endif
    public class RadialPointer : GaugePointer
    {
        #region Fields

        private RadialScale scale;

        #endregion // Fields

        /// <summary>
        /// Gets or sets the value, indicating that gradient should be rotated automatically
        /// </summary>
        [Browsable (true)]
        public bool GradientAutoRotate { get; set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialPointer"/>
        /// </summary>
        /// <param name="parent">The parent gauge object.</param>
        /// <param name="scale">The scale object.</param>
        public RadialPointer (GaugeObject parent, RadialScale scale) : base (parent)
        {
            this.scale = scale;
            GradientAutoRotate = true;
        }

        #endregion // Constructors

        #region Private Methods

        private void DrawHorz (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (BorderColor, BorderWidth * e.ScaleX, DashStyle.Solid);

            var center = (Parent as RadialGauge).Center;
            var circleWidth = Parent.Width / 16f;
            var circleHeight = Parent.Height / 16f;
            var pointerCircle = new RectangleF (center.X - circleWidth / 2 * e.ScaleX,
                center.Y - circleHeight / 2 * e.ScaleY, circleWidth * e.ScaleX, circleHeight * e.ScaleY);

            //double rotateTo = (scale.AverageValue - Parent.Minimum);
            var startAngle = -135 * RadialGauge.Radians;
            var angle = (Parent.Value - Parent.Minimum) / scale.StepValue * scale.MajorStep * RadialGauge.Radians;
            if ((Parent as RadialGauge).Type == RadialGaugeType.Semicircle)
            {
                if ((Parent as RadialGauge).Position is RadialGaugePosition.Bottom or RadialGaugePosition.Top)
                {
                    startAngle = -90 * RadialGauge.Radians;
                    if ((Parent as RadialGauge).Position == RadialGaugePosition.Bottom)
                    {
                        angle *= -1;
                    }
                }
                else if ((Parent as RadialGauge).Position == RadialGaugePosition.Left)
                {
                    startAngle = -180 * RadialGauge.Radians;
                }
                else if ((Parent as RadialGauge).Position == RadialGaugePosition.Right)
                {
                    startAngle = -180 * RadialGauge.Radians;
                    angle *= -1;
                }
            }
            else if (RadialUtils.IsQuadrant (Parent))
            {
                if (RadialUtils.IsLeft (Parent) && RadialUtils.IsTop (Parent))
                {
                    startAngle = -90 * RadialGauge.Radians;
                }
                else if (RadialUtils.IsLeft (Parent) && RadialUtils.IsBottom (Parent))
                {
                    startAngle = -180 * RadialGauge.Radians;
                }
                else if (RadialUtils.IsRight (Parent) && RadialUtils.IsTop (Parent))
                {
                    startAngle = 90 * RadialGauge.Radians;
                }
                else if (RadialUtils.IsRight (Parent) && RadialUtils.IsBottom (Parent))
                {
                    startAngle = 180 * RadialGauge.Radians;
                    angle *= -1;
                }
            }

            //double startAngle = rotateTo / scale.StepValue * -scale.MajorStep * RadialGauge.Radians;

            var ptrLineY = center.Y - pointerCircle.Width / 2 - pointerCircle.Width / 5;
            var ptrLineY1 = scale.AvrTick.Y + scale.MinorTicks.Length * 1.7f;
            var ptrLineWidth = circleWidth / 3 * e.ScaleX;
            var pointerPerpStrt = new PointF[2];
            pointerPerpStrt[0] = new PointF (center.X - ptrLineWidth, ptrLineY);
            pointerPerpStrt[1] = new PointF (center.X + ptrLineWidth, ptrLineY);

            var pointerPerpEnd = new PointF[2];
            pointerPerpEnd[0] = new PointF (center.X - ptrLineWidth / 3, ptrLineY1);
            pointerPerpEnd[1] = new PointF (center.X + ptrLineWidth / 3, ptrLineY1);


            pointerPerpStrt = RadialUtils.RotateVector (pointerPerpStrt, startAngle, center);
            pointerPerpEnd = RadialUtils.RotateVector (pointerPerpEnd, startAngle, center);

            var rotatedPointerPerpStrt = RadialUtils.RotateVector (pointerPerpStrt, angle, center);
            var rotatedPointerPerpEnd = RadialUtils.RotateVector (pointerPerpEnd, angle, center);

            //calc brush rect
            float x = 0, y = 0, dx = 0, dy = 0;
            if (angle / RadialGauge.Radians >= 0 && angle / RadialGauge.Radians < 45)
            {
                x = rotatedPointerPerpEnd[1].X;
                y = rotatedPointerPerpEnd[0].Y - (rotatedPointerPerpEnd[0].Y - pointerCircle.Y);
                dx = pointerCircle.X + pointerCircle.Width - rotatedPointerPerpEnd[0].X;
                dy = rotatedPointerPerpEnd[0].Y - pointerCircle.Y;
            }
            else if (angle / RadialGauge.Radians >= 45 && angle / RadialGauge.Radians < 90)
            {
                x = rotatedPointerPerpEnd[0].X;
                y = rotatedPointerPerpEnd[1].Y;
                dx = pointerCircle.X + pointerCircle.Width - rotatedPointerPerpEnd[0].X;
                dy = pointerCircle.Y + pointerCircle.Height - rotatedPointerPerpEnd[0].Y;
            }
            else if (angle / RadialGauge.Radians >= 90 && angle / RadialGauge.Radians < 135)
            {
                x = rotatedPointerPerpEnd[0].X;
                y = rotatedPointerPerpEnd[1].Y;
                dx = pointerCircle.X + pointerCircle.Width - rotatedPointerPerpEnd[0].X;
                dy = pointerCircle.Y + pointerCircle.Height - rotatedPointerPerpEnd[1].Y;
            }
            else if (angle / RadialGauge.Radians >= 135 && angle / RadialGauge.Radians < 225)
            {
                x = pointerCircle.X;
                y = rotatedPointerPerpEnd[0].Y;
                dx = rotatedPointerPerpEnd[1].X - pointerCircle.X;
                dy = pointerCircle.Y + pointerCircle.Height - rotatedPointerPerpEnd[0].Y;
            }
            else if (angle / RadialGauge.Radians >= 225)
            {
                x = pointerCircle.X;
                y = pointerCircle.Y;
                dx = rotatedPointerPerpEnd[0].X - pointerCircle.X;
                dy = rotatedPointerPerpEnd[1].Y - pointerCircle.Y;
            }

            var brushRect = new RectangleF (x, y, dx, dy);
            if (GradientAutoRotate && Fill is LinearGradientFill)
            {
                (Fill as LinearGradientFill).Angle =
                    (int)(startAngle / RadialGauge.Radians + angle / RadialGauge.Radians) + 90;
            }

            var brush = Fill.CreateBrush (brushRect, e.ScaleX, e.ScaleY);

            var p = new PointF[]
            {
                rotatedPointerPerpStrt[0],
                rotatedPointerPerpStrt[1],
                rotatedPointerPerpEnd[1],
                rotatedPointerPerpEnd[0],
            };
            var path = new GraphicsPath();
            path.AddLines (p);
            path.AddLine (p[3], p[0]);

            g.FillAndDrawEllipse (pen, brush, pointerCircle);

            g.FillAndDrawPath (pen, brush, path);
        }

        #endregion // Private Methods

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (GaugePointer src)
        {
            base.Assign (src);

            var s = src as RadialPointer;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs e)
        {
            base.Draw (e);
            DrawHorz (e);
        }

        /// <inheritdoc/>
        public override void Serialize (ReportWriter writer, string prefix, GaugePointer diff)
        {
            base.Serialize (writer, prefix, diff);
        }

        #endregion // Public Methods
    }
}
