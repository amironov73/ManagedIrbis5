// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* GasGaugeRegion.cs --
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
/// A class representing a region on the GasGuage chart
/// <see cref="GasGaugeRegion"/>s.
/// </summary>
[Serializable]
public class GasGaugeRegion
    : CurveItem, ICloneable
{
    #region Fields

    /// <summary>
    /// Defines the minimum value of this <see cref="GasGaugeRegion"/>
    /// </summary>
    private double _minValue;

    /// <summary>
    /// Defines the maximum value of this <see cref="GasGaugeRegion"/>
    /// </summary>
    private double _maxValue;

    /// <summary>
    /// Defines the Color of this <see cref="GasGaugeRegion"/>
    /// </summary>
    private Color _color;

    /// <summary>
    /// Internally calculated; Start angle of this pie that defines this <see cref="GasGaugeRegion"/>
    /// </summary>
    private float _startAngle;

    /// <summary>
    /// Internally calculated; Sweep angle of this pie that defines this <see cref="GasGaugeRegion"/>
    /// </summary>
    private float _sweepAngle;

    /// <summary>
    /// Private	field	that stores the	<see cref="Charting.Fill"/> data for this
    /// <see cref="GasGaugeRegion"/>.	 Use the public property <see	cref="Fill"/> to
    /// access this value.
    /// </summary>
    private Fill _fill;

    /// <summary>
    /// A <see cref="TextObj"/> which will customize the label display of this
    /// <see cref="GasGaugeRegion"/>
    /// </summary>
    private TextObj _labelDetail;

    /// <summary>
    /// Private	field	that stores the	<see cref="Border"/> class that defines	the
    /// properties of the	border around	this <see cref="GasGaugeRegion"/>. Use the public
    /// property	<see cref="Border"/> to access this value.
    /// </summary>
    private Border _border;

    /// <summary>
    /// The bounding rectangle for this <see cref="GasGaugeRegion"/>.
    /// </summary>
    private RectangleF _boundingRectangle;

    /// <summary>
    /// Private field to hold the GraphicsPath of this <see cref="GasGaugeRegion"/> to be
    /// used for 'hit testing'.
    /// </summary>
    private GraphicsPath? _slicePath;

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
    protected GasGaugeRegion (SerializationInfo info, StreamingContext context)
        : base (info, context)
    {
        // The schema value is just a file version parameter. You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        var sch = info.GetInt32 ("schema2");
        sch.NotUsed();

        _labelDetail = (TextObj)info.GetValue ("labelDetail", typeof (TextObj))!;
        _fill = (Fill)info.GetValue ("fill", typeof (Fill))!;
        _border = (Border)info.GetValue ("border", typeof (Border))!;
        _color = (Color)info.GetValue ("color", typeof (Color))!;
        _minValue = info.GetDouble ("minValue");
        _maxValue = info.GetDouble ("maxValue");
        _startAngle = (float)info.GetDouble ("startAngle");
        _sweepAngle = (float)info.GetDouble ("sweepAngle");
        _boundingRectangle = (RectangleF)info.GetValue ("boundingRectangle", typeof (RectangleF))!;
        _slicePath = (GraphicsPath)info.GetValue ("slicePath", typeof (GraphicsPath))!;
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
        info.AddValue ("color", _color);
        info.AddValue ("border", _border);
        info.AddValue ("minVal", _minValue);
        info.AddValue ("maxVal", _maxValue);
        info.AddValue ("startAngle", _startAngle);
        info.AddValue ("sweepAngle", _sweepAngle);
        info.AddValue ("boundingRectangle", _boundingRectangle);
        info.AddValue ("slicePath", _slicePath);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new <see cref="GasGaugeRegion"/>
    /// </summary>
    /// <param name="label">The value associated with this <see cref="GasGaugeRegion"/> instance.</param>
    /// <param name="color">The display color for this <see cref="GasGaugeRegion"/> instance.</param>
    /// <param name="minVal">The minimum value of this <see cref="GasGaugeNeedle"/>.</param>
    /// <param name="maxVal">The maximum value of this <see cref="GasGaugeNeedle"/>.</param>
    public GasGaugeRegion
        (
            string label,
            double minVal,
            double maxVal,
            Color color
        )
        : base (label)
    {
        _fill = null!;
        MinValue = minVal;
        MaxValue = maxVal;
        RegionColor = color;
        StartAngle = 0f;
        SweepAngle = 0f;
        _border = new Border (Default.BorderColor, Default.BorderWidth);
        _labelDetail = new TextObj();
        _labelDetail.FontSpec.Size = Default.FontSize;
        _slicePath = null;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="ggr">The <see cref="GasGaugeRegion"/> object from which to copy</param>
    public GasGaugeRegion (GasGaugeRegion ggr)
        : base (ggr)
    {
        _fill = null!;
        _minValue = ggr._minValue;
        _maxValue = ggr._maxValue;
        _color = ggr._color;
        _startAngle = ggr._startAngle;
        _sweepAngle = ggr._sweepAngle;
        _border = ggr._border.Clone();
        _labelDetail = ggr._labelDetail.Clone();
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
    public GasGaugeRegion Clone()
    {
        return new GasGaugeRegion (this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the SlicePath of this <see cref="GasGaugeRegion"/>
    /// </summary>
    public GraphicsPath SlicePath => _slicePath;

    /// <summary>
    /// Gets or sets the LabelDetail of this <see cref="GasGaugeRegion"/>
    /// </summary>
    public TextObj LabelDetail
    {
        get => _labelDetail;
        set => _labelDetail = value;
    }

    /// <summary>
    /// Gets or sets the Border of this <see cref="GasGaugeRegion"/>
    /// </summary>
    public Border Border
    {
        get => (_border);
        set => _border = value;
    }

    /// <summary>
    /// Gets or sets the RegionColor of this <see cref="GasGaugeRegion"/>
    /// </summary>
    public Color RegionColor
    {
        get => _color;
        set
        {
            _color = value;
            Fill = new Fill (_color);
        }
    }

    /// <summary>
    /// Gets or sets the Fill of this <see cref="GasGaugeRegion"/>
    /// </summary>
    public Fill Fill
    {
        get => _fill;
        set => _fill = value;
    }

    /// <summary>
    /// Gets or sets the SweepAngle of this <see cref="GasGaugeRegion"/>
    /// </summary>
    private float SweepAngle
    {
        get => _sweepAngle;
        set => _sweepAngle = value;
    }

    /// <summary>
    /// Gets or sets the StartAngle of this <see cref="GasGaugeRegion"/>
    /// </summary>
    private float StartAngle
    {
        get => (_startAngle);
        set => _startAngle = value;
    }

    /// <summary>
    /// Gets or sets the MinValue of this <see cref="GasGaugeRegion"/>
    /// </summary>
    public double MinValue
    {
        get => _minValue;
        set => _minValue = value > 0 ? value : 0;
    }

    /// <summary>
    /// Gets or sets the MaxValue of this <see cref="GasGaugeRegion"/>
    /// </summary>
    public double MaxValue
    {
        get => _maxValue;
        set => _maxValue = value > 0 ? value : 0;
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

    #region Defaults

    /// <summary>
    /// Specify the default property values for the <see cref="GasGaugeRegion"/> class.
    /// </summary>
    public struct Default
    {
        /// <summary>
        /// The default border pen width for the <see cref="GasGaugeRegion"/>
        /// </summary>
        public static float BorderWidth = 1.0F;

        /// <summary>
        /// The default fill type for the <see cref="GasGaugeRegion"/>
        /// </summary>
        public static FillType FillType = FillType.Brush;

        /// <summary>
        /// The default value for the visibility of the <see cref="GasGaugeRegion"/> border.
        /// </summary>
        public static bool IsBorderVisible = true;

        /// <summary>
        /// The default value for the color of the <see cref="GasGaugeRegion"/> border
        /// </summary>
        public static Color BorderColor = Color.Gray;

        /// <summary>
        /// The default value for the color of the <see cref="GasGaugeRegion"/> fill
        /// </summary>
        public static Color FillColor = Color.Empty;

        /// <summary>
        /// The default value for the fill brush of the <see cref="GasGaugeRegion"/>
        /// </summary>
        public static Brush FillBrush = null!;

        /// <summary>
        /// The default value for the visibility of the <see cref="GasGaugeRegion"/> fill.
        /// </summary>
        public static bool isVisible = true;

//			public static PieLabelType LabelType = PieLabelType.Name;

        /// <summary>
        /// The default value for the font size of the <see cref="GasGaugeRegion"/> labels.
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

                _slicePath.AddPie (tRect.X, tRect.Y, tRect.Width, tRect.Height,
                    -0.0f, -180.0f);

                graphics.FillPie (Fill.MakeBrush (_boundingRectangle), tRect.X, tRect.Y, tRect.Width, tRect.Height,
                    -StartAngle, -SweepAngle);

                if (Border.IsVisible)
                {
                    var borderPen = _border.GetPen (pane, scaleFactor);
                    graphics.DrawPie (borderPen, tRect.X, tRect.Y, tRect.Width, tRect.Height,
                        -0.0f, -180.0f);
                    borderPen.Dispose();
                }

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

        // Fill the slice
        if (_fill.IsVisible)
        {
            // just avoid height/width being less than 0.1 so GDI+ doesn't cry
            using (var brush = _fill.MakeBrush (rect))
            {
                graphics.FillRectangle (brush, rect);

                //brush.Dispose();
            }
        }

        // Border the bar
        if (!_border.Color.IsEmpty)
        {
            _border.Draw (graphics, pane, scaleFactor, rect);
        }
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
    /// Calculate the values needed to properly display this <see cref="GasGaugeRegion"/>.
    /// </summary>
    /// <param name="pane">
    /// A graphic device object to be drawn into. This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    public static void CalculateGasGuageParameters (GraphPane pane)
    {
        //loop thru slices and get total value and maxDisplacement
        var minVal = double.MaxValue;
        var maxVal = double.MinValue;
        foreach (var curve in pane.CurveList)
        {
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
        }

        //Calculate start and sweep angles for each of the GasGaugeRegion based on teh min and max value
        foreach (var curve in pane.CurveList)
        {
            if (curve is GasGaugeRegion region)
            {
                var start = ((float)region.MinValue - (float)minVal) / ((float)maxVal - (float)minVal) * 180.0f;
                var sweep = ((float)region.MaxValue - (float)minVal) / ((float)maxVal - (float)minVal) * 180.0f;
                sweep = sweep - start;

                var f = new Fill (Color.White, region.RegionColor, -(sweep / 2f));
                region.Fill = f;

                region.StartAngle = start;
                region.SweepAngle = sweep;
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

        //nonExpRect.Y += -(float)0.025F * nonExpRect.Height;
        //nonExpRect.Y += ((chartRect.Height) - (nonExpRect.Height / 2)) - 10.0f;

        nonExpRect.Inflate (-0.05F * nonExpRect.Height, -(float)0.05 * nonExpRect.Width);

        CalculateGasGuageParameters (pane);

        foreach (var curve in pane.CurveList)
        {
            if (curve is GasGaugeRegion region)
            {
                region._boundingRectangle = nonExpRect;
            }
        }

        return nonExpRect;
    }

    #endregion
}
