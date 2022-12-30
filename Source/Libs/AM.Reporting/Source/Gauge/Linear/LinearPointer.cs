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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Gauge.Linear
{
    /// <summary>
    /// Represents a linear pointer.
    /// </summary>
#if !DEBUG
    [DesignTimeVisible(false)]
#endif
    public class LinearPointer : GaugePointer
    {
        #region Fields

        private float left;
        private float top;

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets o sets the height of gauge pointer.
        /// </summary>
        [Browsable (false)]
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets the width of a pointer.
        /// </summary>
        [Browsable (false)]
        public float Width { get; set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearPointer"/>
        /// </summary>
        /// <param name="parent">The parent gauge object.</param>
        public LinearPointer (GaugeObject parent) : base (parent)
        {
            Height = 4.0f;
            Width = 8.0f;
        }

        #endregion // Constructors

        #region Private Methods

        private void DrawHorz (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (BorderColor, BorderWidth * e.ScaleX, DashStyle.Solid);

            left = (float)(Parent.AbsLeft + 0.5f * Units.Centimeters + (Parent.Width - 1.0f * Units.Centimeters) *
                (Parent.Value - Parent.Minimum) / (Parent.Maximum - Parent.Minimum)) * e.ScaleX;
            top = (Parent.AbsTop + Parent.Height / 2) * e.ScaleY;
            Height = Parent.Height * 0.4f * e.ScaleY;
            Width = Parent.Width * 0.036f * e.ScaleX;

            var dx = Width / 2;
            var dy = Height * 0.3f;
            var brush = Fill.CreateBrush (new RectangleF (left - dx, top, Width, Height), e.ScaleX, e.ScaleY);
            var p = new PointF[]
            {
                new PointF (left, top),
                new PointF (left + dx, top + dy),
                new PointF (left + dx, top + Height),
                new PointF (left - dx, top + Height),
                new PointF (left - dx, top + dy)
            };

            if ((Parent as LinearGauge).Inverted)
            {
                p[1].Y = top - dy;
                p[2].Y = top - Height;
                p[3].Y = top - Height;
                p[4].Y = top - dy;
            }

            var path = new GraphicsPath();
            path.AddLines (p);
            path.AddLine (p[4], p[0]);

            g.FillAndDrawPath (pen, brush, path);
        }

        private void DrawVert (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (BorderColor, BorderWidth * e.ScaleX, DashStyle.Solid);

            left = (Parent.AbsLeft + Parent.Width / 2) * e.ScaleX;
            top = (float)(Parent.AbsTop + Parent.Height - 0.5f * Units.Centimeters -
                          (Parent.Height - 1.0f * Units.Centimeters) * (Parent.Value - Parent.Minimum) /
                          (Parent.Maximum - Parent.Minimum)) * e.ScaleY;
            Height = Parent.Height * 0.036f * e.ScaleY;
            Width = Parent.Width * 0.4f * e.ScaleX;

            var dx = Width * 0.3f;
            var dy = Height / 2;
            var brush = Fill.CreateBrush (new RectangleF (left, top - dy, Width, Height), e.ScaleX, e.ScaleY);
            var p = new PointF[]
            {
                new PointF (left, top),
                new PointF (left + dx, top - dy),
                new PointF (left + Width, top - dy),
                new PointF (left + Width, top + dy),
                new PointF (left + dx, top + dy)
            };

            if ((Parent as LinearGauge).Inverted)
            {
                p[1].X = left - dx;
                p[2].X = left - Width;
                p[3].X = left - Width;
                p[4].X = left - dx;
            }

            var path = new GraphicsPath();
            path.AddLines (p);
            path.AddLine (p[4], p[0]);

            g.FillAndDrawPath (pen, brush, path);
        }

        #endregion // Private Methods

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (GaugePointer src)
        {
            base.Assign (src);

            var s = src as LinearPointer;
            Height = s.Height;
            Width = s.Width;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs e)
        {
            base.Draw (e);

            if (Parent.Vertical)
            {
                DrawVert (e);
            }
            else
            {
                DrawHorz (e);
            }
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer, string prefix, GaugePointer diff)
        {
            base.Serialize (writer, prefix, diff);

            var dc = diff as LinearPointer;
            if (Height != dc.Height)
            {
                writer.WriteFloat (prefix + ".Height", Height);
            }

            if (Width != dc.Width)
            {
                writer.WriteFloat (prefix + ".Width", Width);
            }
        }

        #endregion // Public Methods
    }
}
