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

namespace AM.Reporting.Gauge.Linear
{
    /// <summary>
    /// Represents a linear scale.
    /// </summary>
#if !DEBUG
    [DesignTimeVisible(false)]
#endif
    public class LinearScale : GaugeScale
    {
        #region Fields

        private float left;
        private float top;
        private float height;
        private float width;
        private int majorTicksNum;

        #endregion // Fields

        #region Properties

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearScale"/> class.
        /// </summary>
        /// <param name="parent">The parent gauge object.</param>
        public LinearScale (GaugeObject parent) : base (parent)
        {
            MajorTicks = new ScaleTicks (10, 2, Color.Black);
            MinorTicks = new ScaleTicks (6, 1, Color.Black);
            majorTicksNum = 6;
        }

        #endregion // Constructors

        #region Private Methods

        private void DrawMajorTicksHorz (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (MajorTicks.Color, MajorTicks.Width * e.ScaleX, DashStyle.Solid);
            var brush = TextFill.CreateBrush (new RectangleF (Parent.AbsLeft * e.ScaleX, Parent.AbsTop * e.ScaleY,
                Parent.Width * e.ScaleX, Parent.Height * e.ScaleY), e.ScaleX, e.ScaleY);
            var x = left;
            var y1 = top;
            var y2 = top + height;
            var step = width / (majorTicksNum - 1);
            var textStep = (int)((Parent.Maximum - Parent.Minimum) / (majorTicksNum - 1));
            var font = e.Cache.GetFont (Font.FontFamily,
                Parent.IsPrinting ? Font.Size : Font.Size * e.ScaleX * 96f / DrawUtils.ScreenDpi, Font.Style);
            var text = Parent.Minimum.ToString();
            var y3 = y1 - 0.4f * Units.Centimeters * e.ScaleY;
            if ((Parent as LinearGauge).Inverted)
            {
                y3 = y2 - g.MeasureString (text, Font).Height + 0.4f * Units.Centimeters * e.ScaleY;
            }

            for (var i = 0; i < majorTicksNum; i++)
            {
                g.DrawLine (pen, x, y1, x, y2);
                var strSize = g.MeasureString (text, Font);
                g.DrawString (text, font, brush, x - strSize.Width / 2 * e.ScaleX / (DrawUtils.ScreenDpi / 96f), y3);
                text = Convert.ToString (textStep * (i + 1) + Parent.Minimum);
                x += step;
            }

            brush.Dispose();
        }

        private void DrawMinorTicksHorz (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (MinorTicks.Color, MinorTicks.Width * e.ScaleX, DashStyle.Solid);
            var x = left;
            var y1 = top + height * 0.2f;
            var y2 = top + height - height * 0.2f;
            var step = width / (majorTicksNum - 1) / 4;
            for (var i = 0; i < majorTicksNum - 1; i++)
            {
                x += step;
                for (var j = 0; j < 3; j++)
                {
                    g.DrawLine (pen, x, y1, x, y2);
                    x += step;
                }
            }
        }

        private void DrawMajorTicksVert (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (MajorTicks.Color, MajorTicks.Width * e.ScaleX, DashStyle.Solid);
            var brush = TextFill.CreateBrush (new RectangleF (Parent.AbsLeft * e.ScaleX, Parent.AbsTop * e.ScaleY,
                Parent.Width * e.ScaleX, Parent.Height * e.ScaleY), e.ScaleX, e.ScaleY);
            var y = top + height;
            var x1 = left;
            var x2 = left + width;
            var step = height / (majorTicksNum - 1);
            var textStep = (int)((Parent.Maximum - Parent.Minimum) / (majorTicksNum - 1));
            var font = e.Cache.GetFont (Font.FontFamily,
                Parent.IsPrinting ? Font.Size : Font.Size * e.ScaleX * 96f / DrawUtils.ScreenDpi, Font.Style);
            var text = Parent.Minimum.ToString();
            for (var i = 0; i < majorTicksNum; i++)
            {
                g.DrawLine (pen, x1, y, x2, y);
                var strSize = g.MeasureString (text, Font);
                var x3 = x1 - strSize.Width * e.ScaleX / (DrawUtils.ScreenDpi / 96f) -
                         0.04f * Units.Centimeters * e.ScaleX;
                if ((Parent as LinearGauge).Inverted)
                {
                    x3 = x2 + 0.04f * Units.Centimeters * e.ScaleX;
                }

                g.DrawString (text, font, brush, x3, y - strSize.Height / 2 * e.ScaleY / (DrawUtils.ScreenDpi / 96f));
                text = Convert.ToString (textStep * (i + 1) + Parent.Minimum);
                y -= step;
            }

            brush.Dispose();
        }

        private void DrawMinorTicksVert (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            var pen = e.Cache.GetPen (MinorTicks.Color, MinorTicks.Width * e.ScaleX, DashStyle.Solid);
            var y = top + height;
            var x1 = left + width * 0.2f;
            var x2 = left + width - width * 0.2f;
            var step = height / (majorTicksNum - 1) / 4;
            for (var i = 0; i < majorTicksNum - 1; i++)
            {
                y -= step;
                for (var j = 0; j < 3; j++)
                {
                    g.DrawLine (pen, x1, y, x2, y);
                    y -= step;
                }
            }
        }

        #endregion // Private Methods

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (GaugeScale src)
        {
            base.Assign (src);

            var s = src as LinearScale;
            MajorTicks.Assign (s.MajorTicks);
            MinorTicks.Assign (s.MinorTicks);
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs e)
        {
            base.Draw (e);

            if (Parent.Vertical)
            {
                left = (Parent.AbsLeft + 0.7f * Units.Centimeters) * e.ScaleX;
                top = (Parent.AbsTop + 0.5f * Units.Centimeters) * e.ScaleY;
                height = (Parent.Height - 1.0f * Units.Centimeters) * e.ScaleY;
                width = (Parent.Width - 1.4f * Units.Centimeters) * e.ScaleX;

                DrawMajorTicksVert (e);
                DrawMinorTicksVert (e);
            }
            else
            {
                left = (Parent.AbsLeft + 0.5f * Units.Centimeters) * e.ScaleX;
                top = (Parent.AbsTop + 0.6f * Units.Centimeters) * e.ScaleY;
                height = (Parent.Height - 1.2f * Units.Centimeters) * e.ScaleY;
                width = (Parent.Width - 1.0f * Units.Centimeters) * e.ScaleX;

                DrawMajorTicksHorz (e);
                DrawMinorTicksHorz (e);
            }
        }

        /// <inheritdoc/>
        public override void Serialize (ReportWriter writer, string prefix, GaugeScale diff)
        {
            base.Serialize (writer, prefix, diff);

            var dc = diff as LinearScale;
            MajorTicks.Serialize (writer, prefix + ".MajorTicks", dc.MajorTicks);
            MinorTicks.Serialize (writer, prefix + ".MinorTicks", dc.MinorTicks);
        }

        #endregion // Public Methods
    }
}
