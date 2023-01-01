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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Gauge.Radial
{
    #region Enums

    /// <summary>
    /// Radial Gauge types
    /// </summary>
    [Flags]
    public enum RadialGaugeType
    {
        /// <summary>
        /// Full sized gauge
        /// </summary>
        Circle = 1,

        /// <summary>
        /// Half of the radial gauge
        /// </summary>
        Semicircle = 2,

        /// <summary>
        /// Quarter of the radial gauge
        /// </summary>
        Quadrant = 4
    }

    /// <summary>
    /// Radial Gauge position types
    /// </summary>
    [Flags]
    public enum RadialGaugePosition
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Top
        /// </summary>
        Top = 1,

        /// <summary>
        /// Bottom
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// Left
        /// </summary>
        Left = 4,

        /// <summary>
        /// Right
        /// </summary>
        Right = 8
    }

    #endregion // Enums

    /// <summary>
    /// Represents a linear gauge.
    /// </summary>
    public partial class RadialGauge : GaugeObject
    {
        private const double RAD = Math.PI / 180.0;
        private RadialGaugeType type;
        private RadialGaugePosition position;

        #region Properties

        /// <inheritdoc/>
        public override float Width
        {
            get => base.Width;
            set
            {
                base.Width = value;
                if (base.Height != base.Width)
                {
                    base.Height = Width;
                }
            }
        }

        /// <inheritdoc/>
        public override float Height
        {
            get => base.Height;
            set
            {
                base.Height = value;
                if (base.Width != base.Height)
                {
                    base.Width = Height;
                }
            }
        }

        /// <summary>
        /// Returns centr of the gauge
        /// </summary>
        [Browsable (false)]
        public PointF Center { get; set; }

        /// <summary>
        /// The number of radians in one degree
        /// </summary>
        public static double Radians => RAD;

        /// <summary>
        /// Gets or sets the Radial Gauge type
        /// </summary>
        [Browsable (true)]
        [Category ("Appearance")]
        public RadialGaugeType Type
        {
            get => type;
            set
            {
                if (value == RadialGaugeType.Circle)
                {
                    position = RadialGaugePosition.None;
                    type = value;
                }

                if (value == RadialGaugeType.Semicircle &&
                    !(Position is RadialGaugePosition.Bottom or RadialGaugePosition.Left or RadialGaugePosition.Right or RadialGaugePosition.Top))
                {
                    position = RadialGaugePosition.Top;
                    type = value;
                }
                else if (value == RadialGaugeType.Quadrant &&
                         !(
                             ((Position & RadialGaugePosition.Left) != 0 && (Position & RadialGaugePosition.Top) != 0 &&
                              (Position & RadialGaugePosition.Right) == 0 &&
                              (Position & RadialGaugePosition.Bottom) == 0) ||
                             ((Position & RadialGaugePosition.Right) != 0 &&
                              (Position & RadialGaugePosition.Top) != 0 &&
                              (Position & RadialGaugePosition.Left) == 0 &&
                              (Position & RadialGaugePosition.Bottom) == 0) ||
                             ((Position & RadialGaugePosition.Left) != 0 &&
                              (Position & RadialGaugePosition.Bottom) != 0 &&
                              (Position & RadialGaugePosition.Right) == 0 &&
                              (Position & RadialGaugePosition.Top) == 0) ||
                             ((Position & RadialGaugePosition.Right) != 0 &&
                              (Position & RadialGaugePosition.Bottom) != 0 &&
                              (Position & RadialGaugePosition.Left) == 0 && (Position & RadialGaugePosition.Top) == 0)
                         ))
                {
                    position = RadialGaugePosition.Top | RadialGaugePosition.Left;
                    type = value;
                }
            }
        }

        /// <summary>
        /// Gats or sets the Radial Gauge position. Doesn't work for Full Radial Gauge.
        /// </summary>
        [Category ("Appearance")]
        [Editor ("AM.Reporting.TypeEditors.FlagsEditor, AM.Reporting", typeof (UITypeEditor))]
        public RadialGaugePosition Position
        {
            get => position;
            set
            {
                if (Type == RadialGaugeType.Semicircle &&
                    value is RadialGaugePosition.Bottom or RadialGaugePosition.Left or RadialGaugePosition.Right or RadialGaugePosition.Top)
                {
                    position = value;
                }
                else if (Type == RadialGaugeType.Quadrant &&
                         (
                             ((value & RadialGaugePosition.Left) != 0 && (value & RadialGaugePosition.Top) != 0 &&
                              (value & RadialGaugePosition.Right) == 0 && (value & RadialGaugePosition.Bottom) == 0) ||
                             ((value & RadialGaugePosition.Right) != 0 && (value & RadialGaugePosition.Top) != 0 &&
                              (value & RadialGaugePosition.Left) == 0 && (value & RadialGaugePosition.Bottom) == 0) ||
                             ((value & RadialGaugePosition.Left) != 0 && (value & RadialGaugePosition.Bottom) != 0 &&
                              (value & RadialGaugePosition.Right) == 0 && (value & RadialGaugePosition.Top) == 0) ||
                             ((value & RadialGaugePosition.Right) != 0 && (value & RadialGaugePosition.Bottom) != 0 &&
                              (value & RadialGaugePosition.Left) == 0 && (value & RadialGaugePosition.Top) == 0)
                         ))
                {
                    position = value;
                }
                else if (Type == RadialGaugeType.Circle)
                {
                    position = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets the semicircles offset
        /// </summary>
        [Category ("Appearance")]
        public float SemicircleOffsetRatio { get; set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialGauge"/> class.
        /// </summary>
        public RadialGauge() : base()
        {
            InitializeComponent();
            Scale = new RadialScale (this);
            Pointer = new RadialPointer (this, Scale as RadialScale);
            Label = new RadialLabel (this);
            Height = 4.0f * Units.Centimeters;
            Width = 4.0f * Units.Centimeters;
            SemicircleOffsetRatio = type == RadialGaugeType.Semicircle &&
                                    position is RadialGaugePosition.Left or RadialGaugePosition.Right
                ? 1.5f
                : 1;
            Type = RadialGaugeType.Circle;
            Border.Lines = BorderLines.None;
        }

        #endregion // Constructor

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);
            var src = source as RadialGauge;
            Type = src.Type;
            Position = src.Position;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs eventArgs)
        {
            var g = eventArgs.Graphics;

            var x = (AbsLeft + Border.Width / 2) * eventArgs.ScaleX;
            var y = (AbsTop + Border.Width / 2) * eventArgs.ScaleY;
            var dx = (Width - Border.Width) * eventArgs.ScaleX - 1;
            var dy = (Height - Border.Width) * eventArgs.ScaleY - 1;
            var x1 = x + dx;
            var y1 = y + dy;

            var pen = eventArgs.Cache.GetPen (Border.Color, Border.Width * eventArgs.ScaleX, Border.DashStyle);
            Brush brush;
            if (Fill is SolidFill)
            {
                brush = eventArgs.Cache.GetBrush ((Fill as SolidFill).Color);
            }
            else
            {
                brush = Fill.CreateBrush (new RectangleF (x, y, dx, dy), eventArgs.ScaleX, eventArgs.ScaleY);
            }

            Center = new PointF (x + dx / 2, y + dy / 2);

            if (type == RadialGaugeType.Circle)
            {
                g.FillAndDrawEllipse (pen, brush, x, y, dx, dy);
            }
            else if (type == RadialGaugeType.Semicircle)
            {
                var semiOffset = (Width / 16f / 2f + 2f) * SemicircleOffsetRatio * eventArgs.ScaleY;
                var points = new PointF[4];
                if (position == RadialGaugePosition.Top)
                {
                    g.FillPie (brush, x, y, dx, dy, -180, 180);
                    g.DrawArc (pen, x, y, dx, dy, -180, 180);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        -90 * RAD, Center)[0];

                    points[0] = new PointF (startPoint.X, startPoint.Y - 1 * eventArgs.ScaleY);
                    points[1] = new PointF (startPoint.X, startPoint.Y + semiOffset);
                    points[2] = new PointF (startPoint.X + dx, startPoint.Y + semiOffset);
                    points[3] = new PointF (startPoint.X + dx, startPoint.Y - 1 * eventArgs.ScaleY);
                }
                else if (position == RadialGaugePosition.Bottom)
                {
                    g.FillPie (brush, x, y, dx, dy, 0, 180);
                    g.DrawArc (pen, x, y, dx, dy, 0, 180);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        90 * RAD, Center)[0];

                    points[0] = new PointF (startPoint.X, startPoint.Y + 1 * eventArgs.ScaleY);
                    points[1] = new PointF (startPoint.X, startPoint.Y - semiOffset);
                    points[2] = new PointF (startPoint.X - dx, startPoint.Y - semiOffset);
                    points[3] = new PointF (startPoint.X - dx, startPoint.Y + 1 * eventArgs.ScaleY);
                }
                else if (position == RadialGaugePosition.Left)
                {
                    g.FillPie (brush, x, y, dx, dy, 90, 180);
                    g.DrawArc (pen, x, y, dx, dy, 90, 180);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        180 * RAD, Center)[0];

                    points[0] = new PointF (startPoint.X - 1 * eventArgs.ScaleX, startPoint.Y);
                    points[1] = new PointF (startPoint.X + semiOffset, startPoint.Y);
                    points[2] = new PointF (startPoint.X + semiOffset, startPoint.Y - dy);
                    points[3] = new PointF (startPoint.X - 1 * eventArgs.ScaleX, startPoint.Y - dy);
                }
                else if (position == RadialGaugePosition.Right)
                {
                    g.FillPie (brush, x, y, dx, dy, -90, 180);
                    g.DrawArc (pen, x, y, dx, dy, -90, 180);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        -180 * RAD, Center)[0];

                    points[0] = new PointF (startPoint.X + 1 * eventArgs.ScaleX, startPoint.Y);
                    points[1] = new PointF (startPoint.X - semiOffset, startPoint.Y);
                    points[2] = new PointF (startPoint.X - semiOffset, startPoint.Y - dy);
                    points[3] = new PointF (startPoint.X + 1 * eventArgs.ScaleX, startPoint.Y - dy);
                }

                if (position != RadialGaugePosition.None)
                {
                    var path = new GraphicsPath();
                    path.AddLines (points);
                    g.FillAndDrawPath (pen, brush, path);
                }
            }
            else if (type == RadialGaugeType.Quadrant)
            {
                var semiOffset = (Width / 16f / 2f + 2f) * SemicircleOffsetRatio * eventArgs.ScaleY;
                if (RadialUtils.IsTop (this) && RadialUtils.IsLeft (this))
                {
                    g.FillPie (brush, x, y, dx, dy, -180, 90);
                    g.DrawArc (pen, x, y, dx, dy, -180, 90);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        -90 * RAD, Center)[0];

                    var points = new PointF[5];
                    points[0] = new PointF (startPoint.X, startPoint.Y - 1 * eventArgs.ScaleY);
                    points[1] = new PointF (startPoint.X, startPoint.Y + semiOffset);
                    points[2] = new PointF (startPoint.X + dx / 2 + semiOffset, startPoint.Y + semiOffset);
                    points[3] = new PointF (startPoint.X + dx / 2 + semiOffset, y);
                    points[4] = new PointF (startPoint.X + dx / 2 - 1 * eventArgs.ScaleX, y);
                    var path = new GraphicsPath();
                    path.AddLines (points);
                    g.FillAndDrawPath (pen, brush, path);
                }
                else if (RadialUtils.IsBottom (this) && RadialUtils.IsLeft (this))
                {
                    g.FillPie (brush, x, y, dx, dy, -270, 90);
                    g.DrawArc (pen, x, y, dx, dy, -270, 90);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        -90 * RAD, Center)[0];
                    var points = new PointF[5];
                    points[0] = new PointF (startPoint.X, startPoint.Y + 1 * eventArgs.ScaleY);
                    points[1] = new PointF (startPoint.X, startPoint.Y - semiOffset);
                    points[2] = new PointF (startPoint.X + dx / 2 + semiOffset, startPoint.Y - semiOffset);
                    points[3] = new PointF (startPoint.X + dx / 2 + semiOffset, y + dy);
                    points[4] = new PointF (x + dx / 2 - 1 * eventArgs.ScaleX, y + dy);
                    var path = new GraphicsPath();
                    path.AddLines (points);
                    g.FillAndDrawPath (pen, brush, path);
                }
                else if (RadialUtils.IsTop (this) && RadialUtils.IsRight (this))
                {
                    g.FillPie (brush, x, y, dx, dy, -90, 90);
                    g.DrawArc (pen, x, y, dx, dy, -90, 90);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        90 * RAD, Center)[0];

                    var points = new PointF[5];
                    points[0] = new PointF (startPoint.X, startPoint.Y - 1 * eventArgs.ScaleY);
                    points[1] = new PointF (startPoint.X, startPoint.Y + semiOffset);
                    points[2] = new PointF (startPoint.X - dx / 2 - semiOffset, startPoint.Y + semiOffset);
                    points[3] = new PointF (x + dx / 2 - semiOffset, y);
                    points[4] = new PointF (x + dx / 2 + 1 * eventArgs.ScaleX, y);
                    var path = new GraphicsPath();
                    path.AddLines (points);
                    g.FillAndDrawPath (pen, brush, path);
                }
                else if (RadialUtils.IsBottom (this) && RadialUtils.IsRight (this))
                {
                    g.FillPie (brush, x, y, dx, dy, 0, 90);
                    g.DrawArc (pen, x, y, dx, dy, 0, 90);

                    var startPoint = RadialUtils.RotateVector (new PointF[] { new PointF (x + dx / 2, y), Center },
                        90 * RAD, Center)[0];

                    var points = new PointF[5];
                    points[0] = new PointF (startPoint.X, startPoint.Y + 1 * eventArgs.ScaleY);
                    points[1] = new PointF (startPoint.X, startPoint.Y - semiOffset);
                    points[2] = new PointF (x + dx / 2 - semiOffset, startPoint.Y - semiOffset);
                    points[3] = new PointF (x + dx / 2 - semiOffset, y + dy);
                    points[4] = new PointF (x + dx / 2 + 1 * eventArgs.ScaleX, y + dy);
                    var path = new GraphicsPath();
                    path.AddLines (points);
                    g.FillAndDrawPath (pen, brush, path);
                }
            }

            Scale.Draw (eventArgs);
            Pointer.Draw (eventArgs);
            Label.Draw (eventArgs);
            DrawMarkers (eventArgs);
            if (Fill is not SolidFill)
            {
                brush.Dispose();
            }

            if (Report is { SmoothGraphics: true })
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }
        }

        /// <inheritdoc/>
        public override void Serialize (ReportWriter writer)
        {
            var c = writer.DiffObject as RadialGauge;
            base.Serialize (writer);
            if (Type != c.Type)
            {
                writer.WriteValue ("Type", Type);
            }

            if (Position != c.Position)
            {
                writer.WriteValue ("Position", Position);
            }
        }

        #endregion // Public Methods
    }
}
