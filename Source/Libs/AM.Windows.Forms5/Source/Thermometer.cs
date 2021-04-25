// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Thermometer.cs -- простой "градусник"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Простой "градусник".
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategoryAttribute("Code")]
    // ReSharper restore RedundantNameQualifier
    public sealed class Thermometer
        : Control
    {
        #region Events

        /// <summary>
        /// Raised when current temperature changed.
        /// </summary>
        public event EventHandler? CurrentTemperatureChanged;

        /// <summary>
        /// Raised when minimal temperature changed.
        /// </summary>
        public event EventHandler? MinimalTemperatureChanged;

        /// <summary>
        /// Raised when maximal temperature changed.
        /// </summary>
        public event EventHandler? MaximalTemperatureChanged;

        #endregion

        #region Properties

        private const double DefaultCurrentTemperature = 0.0;

        private double _currentTemperature = DefaultMinimalTemperature;

        ///<summary>
        ///
        ///</summary>
        [DefaultValue(DefaultCurrentTemperature)]
        public double CurrentTemperature
        {
            get => _currentTemperature;
            set
            {
                _currentTemperature = value;
                _SetTemperatures();
                CurrentTemperatureChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private const double DefaultMinimalTemperature = 0.0;

        private double _minimalTemperature = DefaultMinimalTemperature;

        ///<summary>
        ///
        ///</summary>
        [DefaultValue(DefaultMinimalTemperature)]
        public double MinimalTemperature
        {
            get => _minimalTemperature;
            set
            {
                _minimalTemperature = value;
                _SetTemperatures();
                MinimalTemperatureChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private const double DefaultMaximalTemperature = 100.0;

        private double _maximalTemperature = DefaultMaximalTemperature;

        ///<summary>
        ///
        ///</summary>
        [DefaultValue(DefaultMaximalTemperature)]
        public double MaximalTemperature
        {
            get => _maximalTemperature;
            set
            {
                _maximalTemperature = value;
                _SetTemperatures();
                MaximalTemperatureChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Thermometer()
        {
            ResizeRedraw = true;
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        #endregion

        #region Private members

        private void _SetTemperatures()
        {
            _maximalTemperature = Math.Max(_maximalTemperature,
                _minimalTemperature);
            _currentTemperature = Math.Max(Math.Min(
                _currentTemperature, _maximalTemperature),
                _minimalTemperature);
            Invalidate();
        }

        #endregion

        #region Control members

        /// <inheritdoc />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            var graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            var globuleDiameter = 16;
            var columnWidth = 8;
            var columnPosition = (globuleDiameter - columnWidth) / 2;
            using (var blackPen = new Pen(Color.DarkGray))
            using (Brush whiteBrush = new SolidBrush(Color.White))
            using (Brush redBrush = new SolidBrush(Color.Red))
            {
                var globule = new Rectangle(0,
                    Height - globuleDiameter, globuleDiameter,
                    globuleDiameter);
                var column = new Rectangle(columnPosition,
                    columnWidth / 2, columnWidth, Height - globuleDiameter);
                var innerColumn = column;
                var gap = new Rectangle(columnPosition, 0, columnWidth, columnWidth);
                innerColumn.Inflate(-1, -1);
                innerColumn.Y -= 2;
                innerColumn.Height += 6;
                graphics.FillPie(whiteBrush, gap, 180f, 180f);
                graphics.DrawArc(blackPen, gap, 180f, 180f);
                graphics.FillEllipse(whiteBrush, globule);
                graphics.DrawEllipse(blackPen, globule);
                graphics.DrawRectangle(blackPen, column);

                using (var glassBrush = new LinearGradientBrush(innerColumn, Color.White, Color.Black, 0f))
                {
                    var blend = new ColorBlend
                    {
                        Colors = new[]
                        {
                            Color.FromArgb(230, 230, 255),
                            Color.White,
                            Color.FromArgb(220, 220, 255),
                        },
                        Positions = new[] { 0f, 0.3f, 1f }
                    };
                    glassBrush.InterpolationColors = blend;
                    graphics.FillRectangle(glassBrush, innerColumn);
                }

                using (var globulePath = new GraphicsPath())
                {
                    globulePath.AddEllipse(globule);
                    using (var globuleBrush = new PathGradientBrush(globulePath))
                    {
                        globuleBrush.CenterPoint = new PointF(
                            globule.Left + globuleDiameter / 4f,
                            globule.Top + globuleDiameter / 4f);
                        var blend = new ColorBlend
                        {
                            Colors = new[] { Color.Red, Color.Red, Color.White },
                            Positions = new[] { 0f, 0.5f, 1f }
                        };
                        globuleBrush.InterpolationColors = blend;
                        graphics.FillEllipse(globuleBrush, globule);
                    }
                }

                var innerColumnHeight = (int)
                    ((_currentTemperature - _minimalTemperature)
                    / (_maximalTemperature - _minimalTemperature)
                    * column.Height);
                if (innerColumnHeight > 0)
                {
                    var redColumn = new Rectangle(columnPosition + 1,
                        Height - globuleDiameter - innerColumnHeight + 2,
                        columnWidth - 2, innerColumnHeight);

                    using (var columnBrush = new LinearGradientBrush (redColumn, Color.White, Color.Red, 0f))
                    {
                        var blend = new ColorBlend
                        {
                            Colors = new[]
                            {
                                Color.Red,
                                Color.FromArgb(255, 200, 200),
                                Color.Red
                            },
                            Positions = new[] { 0f, 0.3f, 1f }
                        };
                        columnBrush.InterpolationColors = blend;
                        graphics.FillRectangle(columnBrush, redColumn);
                    }

                    var columnGap = new Rectangle(columnPosition + 1,
                        redColumn.Top - columnWidth / 2 + 1, columnWidth - 2,
                        columnWidth - 2);
                    graphics.FillPie(redBrush, columnGap, 180f, 180f);
                }
            }
            base.OnPaint(e);
        }

        #endregion
    }
}
