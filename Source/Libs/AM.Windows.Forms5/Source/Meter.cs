// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Meter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using AM.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategoryAttribute("Code")]
    // ReSharper restore RedundantNameQualifier
    public sealed class Meter
        : Control
    {
        /// <summary>
        /// Raised when <see cref="Value"/> changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Raised when <see cref="MinimalValue"/> changed.
        /// </summary>
        public event EventHandler? MinValueChanged;

        /// <summary>
        /// Raised when <see cref="MaximalValue"/> changed.
        /// </summary>
        public event EventHandler? MaxValueChanged;

        private float _value = DefaultMinimalValue;

        ///<summary>
        /// Current value.
        ///</summary>
        [DefaultValue(DefaultMinimalValue)]
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                Invalidate();
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private const float DefaultMinimalValue = 0.0f;
        private float _minimalValue = DefaultMinimalValue;

        ///<summary>
        /// Minimal value.
        ///</summary>
        [DefaultValue(DefaultMinimalValue)]
        public float MinimalValue
        {
            get => _minimalValue;
            set
            {
                _minimalValue = value;
                Invalidate();
                MinValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private const float DefaultMaximalValue = 100.0f;
        private float _maximalValue = DefaultMaximalValue;

        ///<summary>
        /// Maximal value.
        ///</summary>
        [DefaultValue(DefaultMaximalValue)]
        public float MaximalValue
        {
            get => _maximalValue;
            set
            {
                _maximalValue = value;
                Invalidate();
                MaxValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private const bool DefaultEnableTracking = false;

        ///<summary>
        /// Enable tracking?
        ///</summary>
        [DefaultValue(DefaultEnableTracking)]
        public bool EnableTracking { get; set; } = DefaultEnableTracking;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Meter()
        {
            DoubleBuffered = true;
            ResizeRedraw = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            using (new GraphicsStateSaver(g))
            using (var pen = new Pen(ForeColor, 0))
            using (var grayPen = new Pen(Color.Gray, 0))
            using (var redPen = new Pen(Color.Red, 0))
            using (Brush lightBrush = new SolidBrush(Color.AntiqueWhite))
            using (Brush grayBrush = new SolidBrush(Color.Gray))
            {
                var size = ClientSize;
                g.TranslateTransform((float)size.Width / 2, size.Height);
                g.ScaleTransform(-size.Width / 2000f, -size.Height / 1000f);
                g.FillEllipse(lightBrush, -900, -900, 1800, 1800);
                g.DrawLine(grayPen, -1000, 20, 1000, 20);
                using (new GraphicsStateSaver(g))
                {
                    var delta = 5;

                    g.RotateTransform(-90f);
                    for (var angle = 0; angle < 180; angle += delta)
                    {
                        if ((angle % 30) == 0)
                        {
                            g.DrawLine(pen, 0, 1000, 0, 700);
                        }
                        else
                        {
                            g.DrawLine(pen, 0, 900, 0, 800);
                        }
                        g.RotateTransform(delta);
                    }
                }
                using (new GraphicsStateSaver(g))
                {
                    var angle = (Value - MinimalValue)
                        / (MaximalValue - MinimalValue) * 180f;
                    g.RotateTransform(angle - 90f);
                    g.DrawLine(redPen, 0, 0, 0, 750);
                }
                g.FillEllipse(grayBrush, -200, -200, 400, 400);
                g.DrawEllipse(grayPen, -250, -250, 500, 500);
            }

            base.OnPaint(e);
        }

        private void _Set(MouseEventArgs e)
        {
            var x = (float)Width / 2 - e.X;
            float y = Height - e.Y;
            var angle = (float)(Math.Atan2(y, x) * 180f / Math.PI);
            if ((angle >= 0f) && (angle <= 180f))
            {
                Value = MinimalValue
                    + (MaximalValue - MinimalValue) * angle / 180f;
            }
        }

        /// <inheritdoc />
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (EnableTracking)
            {
                Capture = true;
                _Set(e);
            }
            base.OnMouseDown(e);
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Capture = false;
            base.OnMouseUp(e);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (EnableTracking && Capture)
            {
                _Set(e);
            }
            base.OnMouseMove(e);
        }
    }
}
