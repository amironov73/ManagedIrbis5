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

namespace AM.Reporting.Gauge.Simple
{
    /// <summary>
    /// Represents a simple pointer.
    /// </summary>
#if !DEBUG
    [DesignTimeVisible(false)]
#endif
    public class SimplePointer : GaugePointer
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets o sets the Left offset of gauge pointer.
        /// </summary>
        [Browsable (false)]
        internal float Left { get; set; }

        /// <summary>
        /// Gets o sets the Top offset of gauge pointer.
        /// </summary>
        [Browsable (false)]
        internal float Top { get; set; }

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

        /// <summary>
        /// Gets or sets the pointer ratio.
        /// </summary>
        [Browsable (false)]
        public float PointerRatio { get; set; } = 0.08f;

        /// <summary>
        /// Gets or sets the pointer horizontal offset (cm).
        /// </summary>
        [Browsable (false)]
        public float HorizontalOffset { get; set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimplePointer"/> class.
        /// </summary>
        /// <param name="parent">The parent gauge object.</param>
        public SimplePointer (GaugeObject parent) : base (parent)
        {
            Height = Parent.Height * PointerRatio;
            Width = Parent.Width * PointerRatio;
            HorizontalOffset = 0.5f * Units.Centimeters;
        }

        #endregion // Constructors

        #region Internal Methods

        internal virtual void DrawHorz (PaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (BorderColor, BorderWidth * e.ScaleX, DashStyle.Solid);

            Left = (Parent.AbsLeft + Parent.Border.Width / 2 + HorizontalOffset) * e.ScaleX;
            Top = (Parent.AbsTop + Parent.Border.Width / 2 + (Parent.Height - Parent.Border.Width) / 2 -
                   (Parent.Height - Parent.Border.Width) * PointerRatio / 2) * e.ScaleY;
            Height = ((Parent.Height - Parent.Border.Width) * PointerRatio) * e.ScaleY;
            Width = (float)((Parent.Width - Parent.Border.Width - HorizontalOffset * 2) *
                (Parent.Value - Parent.Minimum) / (Parent.Maximum - Parent.Minimum) * e.ScaleX);

            var brush = Fill.CreateBrush (new RectangleF (Left, Top, Width, Height), e.ScaleX, e.ScaleY);
            g.FillAndDrawRectangle (pen, brush, Left, Top, Width, Height);
        }

        internal virtual void DrawVert (PaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (BorderColor, BorderWidth * e.ScaleY, DashStyle.Solid);

            Width = ((Parent.Width - Parent.Border.Width) * PointerRatio) * e.ScaleX;
            Height = (float)((Parent.Height - Parent.Border.Width - HorizontalOffset * 2) *
                (Parent.Value - Parent.Minimum) / (Parent.Maximum - Parent.Minimum) * e.ScaleY);
            Left = (Parent.AbsLeft + Parent.Border.Width / 2 + (Parent.Width - Parent.Border.Width) / 2 -
                    (Parent.Width - Parent.Border.Width) * PointerRatio / 2) * e.ScaleX;
            Top = (Parent.AbsTop + Parent.Border.Width / 2 + Parent.Height - Parent.Border.Width - HorizontalOffset) *
                e.ScaleY - Height;

            var brush = Fill.CreateBrush (new RectangleF (Left, Top, Width, Height), e.ScaleX, e.ScaleY);
            g.FillAndDrawRectangle (pen, brush, Left, Top, Width, Height);
        }

        #endregion // Internal Methods

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (GaugePointer src)
        {
            base.Assign (src);

            var s = src as SimplePointer;
            Height = s.Height;
            Width = s.Width;
        }

        /// <inheritdoc/>
        public override void Draw (PaintEventArgs e)
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
        public override void Serialize (ReportWriter writer, string prefix, GaugePointer diff)
        {
            base.Serialize (writer, prefix, diff);

            var dc = diff as SimplePointer;
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
