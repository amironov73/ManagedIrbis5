// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* GasGaugeNeedle.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.Serialization;
using System.Drawing.Drawing2D;
using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A class representing a needle on the GasGuage chart
/// <see cref="GasGaugeNeedle"/>s.
/// </summary>
[Serializable]
public class GasGaugeNeedle
    : CurveItem, ICloneable
{
    #region Fields

    /// <summary>
    /// Value of this needle
    /// </summary>
    private double _needleValue;

    /// <summary>
    /// Width of the line being drawn
    /// </summary>
    private float _needleWidth;

    /// <summary>
    /// Color of the needle line
    /// </summary>
    private Color _color;

    /// <summary>
    /// Internally calculated angle that places this needle relative to the MinValue and
    /// MaxValue of 180 degree GasGuage
    /// </summary>
    private float _sweepAngle;

    /// <summary>
    /// Private field that stores the <see cref="Charting.Fill"/> data for this
    /// <see cref="GasGaugeNeedle"/>. Use the public property <see cref="Fill"/> to
    /// access this value.
    /// </summary>
    private Fill _fill;

    /// <summary>
    /// A <see cref="TextObj"/> which will customize the label display of this
    /// <see cref="GasGaugeNeedle"/>
    /// </summary>
    private TextObj _labelDetail;

    /// <summary>
    /// Private field that stores the <see cref="Border"/> class that defines the
    /// properties of the border around this <see cref="GasGaugeNeedle"/>. Use the public
    /// property <see cref="Border"/> to access this value.
    /// </summary>
    private Border _border;

    /// <summary>
    /// The bounding rectangle for this <see cref="GasGaugeNeedle"/>.
    /// </summary>
    private RectangleF _boundingRectangle;

    /// <summary>
    /// Private field to hold the GraphicsPath of this <see cref="GasGaugeNeedle"/> to be
    /// used for 'hit testing'.
    /// </summary>
    private GraphicsPath? _slicePath;

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new <see cref="GasGaugeNeedle"/>
    /// </summary>
    /// <param name="label">The value associated with this <see cref="GasGaugeNeedle"/>
    /// instance.</param>
    /// <param name="color">The display color for this <see cref="GasGaugeNeedle"/>
    /// instance.</param>
    /// <param name="val">The value of this <see cref="GasGaugeNeedle"/>.</param>
    public GasGaugeNeedle
        (
            string label,
            double val,
            Color color
        )
        : base (label)
    {
        _fill = null!;

        NeedleValue = val;
        NeedleColor = color;
        NeedleWidth = Default.NeedleWidth;
        SweepAngle = 0f;
        _border = new Border (Default.BorderColor, Default.BorderWidth);
        _labelDetail = new TextObj();
        _labelDetail.FontSpec.Size = Default.FontSize;
        _slicePath = null;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="ggn">The <see cref="GasGaugeNeedle"/> object from which to copy</param>
    public GasGaugeNeedle (GasGaugeNeedle ggn)
        : base (ggn)
    {
        _fill = null!;

        NeedleValue = ggn.NeedleValue;
        NeedleColor = ggn.NeedleColor;
        NeedleWidth = ggn.NeedleWidth;
        SweepAngle = ggn.SweepAngle;
        _border = ggn.Border.Clone();
        _labelDetail = ggn.LabelDetail.Clone();
        _labelDetail.FontSpec.Size = ggn.LabelDetail.FontSpec.Size;
    }

    /// <summary>
    /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
    /// calling the typed version of <see cref="Clone" />
    /// </summary>
    /// <returns>A deep copy of this object</returns>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public GasGaugeNeedle Clone()
    {
        return new GasGaugeNeedle (this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or Sets the NeedleWidth of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    public float NeedleWidth
    {
        get => _needleWidth;
        set => _needleWidth = value;
    }

    /// <summary>
    /// Gets or Sets the Border of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    public Border Border
    {
        get => (_border);
        set => _border = value;
    }

    /// <summary>
    /// Gets or Sets the SlicePath of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    public GraphicsPath SlicePath => _slicePath!;

    /// <summary>
    /// Gets or Sets the LableDetail of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    public TextObj LabelDetail
    {
        get => _labelDetail;
        set => _labelDetail = value;
    }


    /// <summary>
    /// Gets or Sets the NeedelColor of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    public Color NeedleColor
    {
        get => _color;
        set
        {
            _color = value;
            Fill = new Fill (_color);
        }
    }

    /// <summary>
    /// Gets or Sets the Fill of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    public Fill Fill
    {
        get => _fill;
        set => _fill = value;
    }

    /// <summary>
    /// Private property that Gets or Sets the SweepAngle of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    private float SweepAngle
    {
        get => _sweepAngle;
        set => _sweepAngle = value;
    }

    /// <summary>
    /// Gets or Sets the NeedleValue of this <see cref="GasGaugeNeedle"/>
    /// </summary>
    public double NeedleValue
    {
        get => (_needleValue);
        set => _needleValue = value > 0 ? value : 0;
    }

    /// <inheritdoc cref="CurveItem.IsZIncluded"/>
    internal override bool IsZIncluded (GraphPane pane)
    {
        return false;
    }

    /// <inheritdoc cref="CurveItem.IsXIndependent"/>
    internal override bool IsXIndependent (GraphPane pane)
    {
        return true;
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema2 = 10;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected GasGaugeNeedle (SerializationInfo info, StreamingContext context)
        : base (info, context)
    {
        // The schema value is just a file version parameter. You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        var sch = info.GetInt32 ("schema2");
        sch.NotUsed();

        _labelDetail = (TextObj)info.GetValue ("labelDetail", typeof (TextObj))!;
        _fill = (Fill)info.GetValue ("fill", typeof (Fill))!;
        _border = (Border)info.GetValue ("border", typeof (Border))!;
        _needleValue = info.GetDouble ("needleValue");
        _boundingRectangle = (RectangleF)info.GetValue ("boundingRectangle", typeof (RectangleF))!;
        _slicePath = (GraphicsPath)info.GetValue ("slicePath", typeof (GraphicsPath))!;
        _sweepAngle = (float)info.GetDouble ("sweepAngle");
        _color = (Color)info.GetValue ("color", typeof (Color))!;
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);
        info.AddValue ("schema2", schema2);
        info.AddValue ("labelDetail", _labelDetail);
        info.AddValue ("fill", _fill);
        info.AddValue ("border", _border);
        info.AddValue ("needleValue", _needleValue);
        info.AddValue ("boundingRectangle", _boundingRectangle);
        info.AddValue ("slicePath", _slicePath);
        info.AddValue ("sweepAngle", _sweepAngle);
    }

    #endregion

    #region Default

    /// <summary>
    /// Specify the default property values for the <see cref="GasGaugeNeedle"/> class.
    /// </summary>
    public struct Default
    {
        /// <summary>
        /// The default width of the gas gauge needle.  Units are points, scaled according
        /// to <see cref="PaneBase.CalcScaleFactor" />
        /// </summary>
        public static float NeedleWidth = 10.0F;

        /// <summary>
        /// The default pen width to be used for drawing the border around the GasGaugeNeedle
        /// (<see cref="LineBase.Width"/> property). Units are points.
        /// </summary>
        public static float BorderWidth = 1.0F;

        /// <summary>
        /// The default border mode for GasGaugeNeedle (<see cref="LineBase.IsVisible"/>
        /// property).
        /// true to display frame around GasGaugeNeedle, false otherwise
        /// </summary>
        public static bool IsBorderVisible = true;

        /// <summary>
        /// The default color for drawing frames around GasGaugeNeedle
        /// (<see cref="LineBase.Color"/> property).
        /// </summary>
        public static Color BorderColor = Color.Gray;

        /// <summary>
        /// The default fill type for filling the GasGaugeNeedle.
        /// </summary>
        public static FillType FillType = FillType.Brush;

        /// <summary>
        /// The default color for filling in the GasGaugeNeedle
        /// (<see cref="Fill.Color"/> property).
        /// </summary>
        public static Color FillColor = Color.Empty;

        /// <summary>
        /// The default custom brush for filling in the GasGaugeNeedle.
        /// (<see cref="Fill.Brush"/> property).
        /// </summary>
        public static Brush FillBrush = null!;

        /// <summary>
        ///Default value for controlling <see cref="GasGaugeNeedle"/> display.
        /// </summary>
        public static bool isVisible = true;

//			/// <summary>
//			/// Default value for <see cref="GasGaugeNeedle.LabelType"/>.
//			/// </summary>
//			public static PieLabelType LabelType = PieLabelType.Name;

        /// <summary>
        /// The default font size for <see cref="GasGaugeNeedle.LabelDetail"/> entries
        /// (<see cref="FontSpec.Size"/> property). Units are
        /// in points (1/72 inch).
        /// </summary>
        public static float FontSize = 10;
    }

    #endregion Defaults

    #region Methods

    /// <inheritdoc cref="CurveItem.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            GraphPane pane,
            int pos,
            float scaleFactor
        )
    {
        if (pane.Chart._rect is { Width: <= 0, Height: <= 0 })
        {
            _slicePath = null;
        }
        else
        {
            CalcRectangle (graphics, pane, scaleFactor, pane.Chart._rect);

            _slicePath = new GraphicsPath();

            if (!IsVisible)
            {
                return;
            }

            var tRect = _boundingRectangle;

            if (tRect is { Width: >= 1, Height: >= 1 })
            {
                var sMode = graphics.SmoothingMode;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var matrix = new Matrix();

                matrix.Translate (tRect.X + (tRect.Width / 2), tRect.Y + (tRect.Height / 2), MatrixOrder.Prepend);

                var pts = new PointF[2];
                pts[0] = new PointF (
                    ((tRect.Height * .10f) / 2.0f) * (float)Math.Cos (-SweepAngle * Math.PI / 180.0f),
                    ((tRect.Height * .10f) / 2.0f) * (float)Math.Sin (-SweepAngle * Math.PI / 180.0f));
                pts[1] = new PointF ((tRect.Width / 2.0f) * (float)Math.Cos (-SweepAngle * Math.PI / 180.0f),
                    (tRect.Width / 2.0f) * (float)Math.Sin (-SweepAngle * Math.PI / 180.0f));

                matrix.TransformPoints (pts);

                var p = new Pen (NeedleColor, ((tRect.Height * .10f) / 2.0f));
                p.EndCap = LineCap.ArrowAnchor;
                graphics.DrawLine (p, pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);

                //Fill center 10% with Black dot;
                var f = new Fill (Color.Black);
                var r = new RectangleF ((tRect.X + (tRect.Width / 2)) - 1.0f,
                    (tRect.Y + (tRect.Height / 2)) - 1.0f, 1.0f, 1.0f);
                r.Inflate ((tRect.Height * .10f), (tRect.Height * .10f));
                var b = f.MakeBrush (r);
                graphics.FillPie (b, r.X, r.Y, r.Width, r.Height, 0.0f, -180.0f);

                var borderPen = new Pen (Color.White, 2.0f);
                graphics.DrawPie (borderPen, r.X, r.Y, r.Width, r.Height, 0.0f, -180.0f);

                graphics.SmoothingMode = sMode;
            }
        }
    }

    /// <inheritdoc cref="CurveItem.DrawLegendKey"/>
    public override void DrawLegendKey
        (
            Graphics graphics,
            GraphPane pane,
            RectangleF rect,
            float scaleFactor
        )
    {
        if (!IsVisible)
        {
            return;
        }

        var yMid = rect.Top + rect.Height / 2.0F;

        var pen = new Pen (NeedleColor, pane.ScaledPenWidth (NeedleWidth / 2, scaleFactor));
        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.ArrowAnchor;
        pen.DashStyle = DashStyle.Solid;
        graphics.DrawLine (pen, rect.Left, yMid, rect.Right, yMid);
    }

    /// <inheritdoc cref="CurveItem.GetCoords"/>
    public override bool GetCoords
        (
            GraphPane pane,
            int i,
            out string coords
        )
    {
        coords = string.Empty;

        return false;
    }

    /// <summary>
    /// Calculate the values needed to properly display this <see cref="GasGaugeNeedle"/>.
    /// </summary>
    /// <param name="pane">
    /// A graphic device object to be drawn into. This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    public static void CalculateGasGaugeParameters (GraphPane pane)
    {
        //loop thru slices and get total value and maxDisplacement
        var minVal = double.MaxValue;
        var maxVal = double.MinValue;
        foreach (var curve in pane.CurveList)
            if (curve is GasGaugeRegion region)
            {
                if (maxVal < region.MaxValue)
                {
                    maxVal = region.MaxValue;
                }

                if (minVal > region.MinValue)
                {
                    minVal = region.MinValue;
                }
            }

        //Set Needle Sweep angle values here based on the min and max values of the GasGuage
        foreach (var curve in pane.CurveList)
        {
            if (curve is GasGaugeNeedle ggn)
            {
                var sweep = ((float)ggn.NeedleValue - (float)minVal) /
                    ((float)maxVal - (float)minVal) * 180.0f;
                ggn.SweepAngle = sweep;
            }
        }
    }

    /// <summary>
    /// Calculate the <see cref="RectangleF"/> that will be used to define the bounding rectangle of
    /// the GasGaugeNeedle.
    /// </summary>
    /// <remarks>This rectangle always lies inside of the <see cref="Chart.Rect"/>, and it is
    /// normally a square so that the pie itself is not oval-shaped.</remarks>
    /// <param name="g">
    /// A graphic device object to be drawn into. This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects. This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <param name="chartRect">The <see cref="RectangleF"/> (normally the <see cref="Chart.Rect"/>)
    /// that bounds this pie.</param>
    /// <returns></returns>
    public static RectangleF CalcRectangle (Graphics g, GraphPane pane, float scaleFactor, RectangleF chartRect)
    {
        var nonExpRect = chartRect;

        if ((2 * nonExpRect.Height) > nonExpRect.Width)
        {
            //Scale based on width
            var percentS = ((nonExpRect.Height * 2) - nonExpRect.Width) / (nonExpRect.Height * 2);
            nonExpRect.Height = ((nonExpRect.Height * 2) - ((nonExpRect.Height * 2) * percentS));
        }
        else
        {
            nonExpRect.Height = nonExpRect.Height * 2;
        }

        nonExpRect.Width = nonExpRect.Height;

        var xDelta = (chartRect.Width / 2) - (nonExpRect.Width / 2);

        //Align Horizontally
        nonExpRect.X += xDelta;

        nonExpRect.Inflate (-0.05f * nonExpRect.Height, -(float)0.05 * nonExpRect.Width);

        CalculateGasGaugeParameters (pane);

        foreach (var curve in pane.CurveList)
        {
            if (curve is GasGaugeNeedle ggn)
            {
                ggn._boundingRectangle = nonExpRect;
            }
        }

        return nonExpRect;
    }

    #endregion
}
