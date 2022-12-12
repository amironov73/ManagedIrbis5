// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable InconsistentNaming

/* Line.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A class representing all the characteristics of the Line
/// segments that make up a curve on the graph.
/// </summary>
[Serializable]
public class Line
    : LineBase, ICloneable
{
    #region Fields

    /// <summary>
    /// Private field that stores the smoothing flag for this
    /// <see cref="Line"/>.  Use the public
    /// property <see cref="IsSmooth"/> to access this value.
    /// </summary>
    private bool _isSmooth;

    /// <summary>
    /// Private field that stores the smoothing tension
    /// for this <see cref="Line"/>.  Use the public property
    /// <see cref="SmoothTension"/> to access this value.
    /// </summary>
    /// <value>A floating point value indicating the level of smoothing.
    /// 0.0F for no smoothing, 1.0F for lots of smoothing, >1.0 for odd
    /// smoothing.</value>
    /// <seealso cref="IsSmooth"/>
    /// <seealso cref="Default.IsSmooth"/>
    /// <seealso cref="Default.SmoothTension"/>
    private float _smoothTension;

    /// <summary>
    /// Private field that stores the <see cref="Charting.StepType"/> for this
    /// <see cref="CurveItem"/>.  Use the public
    /// property <see cref="StepType"/> to access this value.
    /// </summary>
    private StepType _stepType;

    /// <summary>
    /// Private field that stores the <see cref="Charting.Fill"/> data for this
    /// <see cref="Line"/>.  Use the public property <see cref="Fill"/> to
    /// access this value.
    /// </summary>
    private Fill _fill;

    /// <summary>
    /// Private field that determines if this <see cref="Line"/> will be drawn with
    /// optimizations enabled.  Use the public
    /// property <see cref="IsOptimizedDraw"/> to access this value.
    /// </summary>
    private bool _isOptimizedDraw;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="Line"/> class.
    /// </summary>
    public new struct Default
    {
        // Default Line properties
        /// <summary>
        /// The default color for curves (line segments connecting the points).
        /// This is the default value for the <see cref="LineBase.Color"/> property.
        /// </summary>
        public static Color Color = Color.Red;

        /// <summary>
        /// The default color for filling in the area under the curve
        /// (<see cref="Fill.Color"/> property).
        /// </summary>
        public static Color FillColor = Color.Red;

        /// <summary>
        /// The default custom brush for filling in the area under the curve
        /// (<see cref="Fill.Brush"/> property).
        /// </summary>
        public static Brush? FillBrush = null;

        /// <summary>
        /// The default fill mode for the curve (<see cref="Fill.Type"/> property).
        /// </summary>
        public static FillType FillType = FillType.None;

        /// <summary>
        /// The default value for the <see cref="Line.IsSmooth"/>
        /// property.
        /// </summary>
        public static bool IsSmooth = false;

        /// <summary>
        /// The default value for the <see cref="Line.SmoothTension"/> property.
        /// </summary>
        public static float SmoothTension = 0.5F;

        /// <summary>
        /// The default value for the <see cref="Line.IsOptimizedDraw"/> property.
        /// </summary>
        public static bool IsOptimizedDraw = false;

        /// <summary>
        /// Default value for the curve type property
        /// (<see cref="Line.StepType"/>).  This determines if the curve
        /// will be drawn by directly connecting the points from the
        /// <see cref="CurveItem.Points"/> data collection,
        /// or if the curve will be a "stair-step" in which the points are
        /// connected by a series of horizontal and vertical lines that
        /// represent discrete, staticant values.  Note that the values can
        /// be forward oriented <code>ForwardStep</code> (<see cref="StepType"/>) or
        /// rearward oriented <code>RearwardStep</code>.
        /// That is, the points are defined at the beginning or end
        /// of the staticant value for which they apply, respectively.
        /// </summary>
        /// <value><see cref="StepType"/> enum value</value>
        public static StepType StepType = StepType.NonStep;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a property that determines if this <see cref="Line"/>
    /// will be drawn smooth.  The "smoothness" is controlled by
    /// the <see cref="SmoothTension"/> property.
    /// </summary>
    /// <value>true to smooth the line, false to just connect the dots
    /// with linear segments</value>
    /// <seealso cref="SmoothTension"/>
    /// <seealso cref="Default.IsSmooth"/>
    /// <seealso cref="Default.SmoothTension"/>
    public bool IsSmooth
    {
        get => _isSmooth;
        set => _isSmooth = value;
    }

    /// <summary>
    /// Gets or sets a property that determines the smoothing tension
    /// for this <see cref="Line"/>.  This property is only used if
    /// <see cref="IsSmooth"/> is true.  A tension value 0.0 will just
    /// draw ordinary line segments like an unsmoothed line.  A tension
    /// value of 1.0 will be smooth.  Values greater than 1.0 will generally
    /// give odd results.
    /// </summary>
    /// <value>A floating point value indicating the level of smoothing.
    /// 0.0F for no smoothing, 1.0F for lots of smoothing, >1.0 for odd
    /// smoothing.</value>
    /// <seealso cref="IsSmooth"/>
    /// <seealso cref="Default.IsSmooth"/>
    /// <seealso cref="Default.SmoothTension"/>
    public float SmoothTension
    {
        get => _smoothTension;
        set => _smoothTension = value;
    }

    /// <summary>
    /// Determines if the <see cref="CurveItem"/> will be drawn by directly connecting the
    /// points from the <see cref="CurveItem.Points"/> data collection,
    /// or if the curve will be a "stair-step" in which the points are
    /// connected by a series of horizontal and vertical lines that
    /// represent discrete, constant values.  Note that the values can
    /// be forward oriented <c>ForwardStep</c> (<see cref="Charting.StepType"/>) or
    /// rearward oriented <c>RearwardStep</c>.
    /// That is, the points are defined at the beginning or end
    /// of the constant value for which they apply, respectively.
    /// The <see cref="StepType"/> property is ignored for lines
    /// that have <see cref="IsSmooth"/> set to true.
    /// </summary>
    /// <value><see cref="Charting.StepType"/> enum value</value>
    /// <seealso cref="Default.StepType"/>
    public StepType StepType
    {
        get => _stepType;
        set => _stepType = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="Charting.Fill"/> data for this
    /// <see cref="Line"/>.
    /// </summary>
    public Fill Fill
    {
        get { return _fill; }
        set { _fill = value; }
    }

    /// <summary>
    /// Gets or sets a boolean value that determines if this <see cref="Line"/> will be drawn with
    /// optimizations enabled.
    /// </summary>
    /// <remarks>
    /// Normally, the optimizations can be used without a problem, especially if the data
    /// are sorted.  The optimizations are particularly helpful with very large datasets.
    /// However, if the data are very discontinuous (for example, a curve that doubles back
    /// on itself), then the optimizations can cause drawing artifacts in the form of
    /// missing line segments.  The default option for this mode is false, so you must
    /// explicitly enable it for each <see cref="LineItem.Line">LineItem.Line</see>.
    /// Also note that, even if the optimizations are enabled explicitly, no actual
    /// optimization will be done for datasets of less than 1000 points.
    /// </remarks>
    public bool IsOptimizedDraw
    {
        get { return _isOptimizedDraw; }
        set { _isOptimizedDraw = value; }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor that sets all <see cref="Line"/> properties to default
    /// values as defined in the <see cref="Default"/> class.
    /// </summary>
    public Line()
        : this (Color.Empty)
    {
    }

    /// <summary>
    /// Constructor that sets the color property to the specified value, and sets
    /// the remaining <see cref="Line"/> properties to default
    /// values as defined in the <see cref="Default"/> class.
    /// </summary>
    /// <param name="color">The color to assign to this new Line object</param>
    public Line (Color color)
    {
        _color = color.IsEmpty ? Default.Color : color;
        _stepType = Default.StepType;
        _isSmooth = Default.IsSmooth;
        _smoothTension = Default.SmoothTension;
        _fill = new Fill (Default.FillColor, Default.FillBrush, Default.FillType);
        _isOptimizedDraw = Default.IsOptimizedDraw;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The Line object from which to copy</param>
    public Line (Line rhs) : base (rhs)
    {
        _color = rhs._color;
        _stepType = rhs._stepType;
        _isSmooth = rhs._isSmooth;
        _smoothTension = rhs._smoothTension;
        _fill = rhs._fill.Clone();
        _isOptimizedDraw = rhs._isOptimizedDraw;
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
    public Line Clone()
    {
        return new Line (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema = 14;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected Line (SerializationInfo info, StreamingContext context)
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        var sch = info.GetInt32 ("schema");

        //if ( sch >= 14 )
        //	_color = (Color) info.GetValue( "color", typeof( Color ) );
        _stepType = (StepType)info.GetValue ("stepType", typeof (StepType))!;
        _isSmooth = info.GetBoolean ("isSmooth");
        _smoothTension = info.GetSingle ("smoothTension");
        _fill = (Fill)info.GetValue ("fill", typeof (Fill))!;

        if (sch >= 13)
        {
            _isOptimizedDraw = info.GetBoolean ("isOptimizedDraw");
        }
    }

    /// <summary>
    /// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);

        info.AddValue ("schema", schema);

        //info.AddValue( "color", _color );
        info.AddValue ("stepType", _stepType);
        info.AddValue ("isSmooth", _isSmooth);
        info.AddValue ("smoothTension", _smoothTension);
        info.AddValue ("fill", _fill);

        info.AddValue ("isOptimizedDraw", _isOptimizedDraw);
    }

    #endregion

    #region Rendering Methods

    /// <summary>
    /// Do all rendering associated with this <see cref="Line"/> to the specified
    /// <see cref="Graphics"/> device.  This method is normally only
    /// called by the Draw method of the parent <see cref="LineItem"/> object.
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="curve">A <see cref="LineItem"/> representing this
    /// curve.</param>
    public void Draw
        (
            Graphics graphics,
            GraphPane pane,
            CurveItem curve,
            float scaleFactor
        )
    {
        // If the line is being shown, draw it
        if (IsVisible)
        {
            //How to handle fill vs nofill?
            //if ( isSelected )
            //	GraphPane.Default.SelectedLine.

            var sModeSave = graphics.SmoothingMode;
            if (IsAntiAlias)
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
            }

            if (curve is StickItem)
            {
                DrawSticks (graphics, pane, curve, scaleFactor);
            }
            else if (IsSmooth || Fill.IsVisible)
            {
                DrawSmoothFilledCurve (graphics, pane, curve, scaleFactor);
            }
            else
            {
                DrawCurve (graphics, pane, curve, scaleFactor);
            }

            graphics.SmoothingMode = sModeSave;
        }
    }

    /// <summary>
    /// Render a single <see cref="Line"/> segment to the specified
    /// <see cref="Graphics"/> device.
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <param name="x1">The x position of the starting point that defines the
    /// line segment in screen pixel units</param>
    /// <param name="y1">The y position of the starting point that defines the
    /// line segment in screen pixel units</param>
    /// <param name="x2">The x position of the ending point that defines the
    /// line segment in screen pixel units</param>
    /// <param name="y2">The y position of the ending point that defines the
    /// line segment in screen pixel units</param>
    public void DrawSegment
        (
            Graphics graphics,
            GraphPane pane,
            float x1,
            float y1,
            float x2,
            float y2,
            float scaleFactor
        )
    {
        if (IsVisible && !Color.IsEmpty)
        {
            using var pen = GetPen (pane, scaleFactor);
            graphics.DrawLine (pen, x1, y1, x2, y2);
        }
    }

    /// <summary>
    /// Render the <see cref="Line"/>'s as vertical sticks (from a <see cref="StickItem" />) to
    /// the specified <see cref="Graphics"/> device.
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="curve">A <see cref="CurveItem"/> representing this
    /// curve.</param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    public void DrawSticks
        (
            Graphics graphics,
            GraphPane pane,
            CurveItem curve,
            float scaleFactor
        )
    {
        var source = this;
        if (curve.IsSelected)
        {
            source = Selection.Line;
        }

        var yAxis = curve.GetYAxis (pane);
        var xAxis = curve.GetXAxis (pane);

        var basePix = yAxis.Scale!.Transform (0.0);
        using (var pen = source.GetPen (pane, scaleFactor))
        {
            for (var i = 0; i < curve.Points!.Count; i++)
            {
                var pt = curve.Points[i];

                if (pt.X != PointPairBase.Missing &&
                    pt.Y != PointPairBase.Missing &&
                    !double.IsNaN (pt.X) &&
                    !double.IsNaN (pt.Y) &&
                    !double.IsInfinity (pt.X) &&
                    !double.IsInfinity (pt.Y) &&
                    (!xAxis.Scale!.IsLog || pt.X > 0.0) &&
                    (!yAxis.Scale.IsLog || pt.Y > 0.0))
                {
                    var pixY = yAxis.Scale.Transform (curve.IsOverrideOrdinal, i, pt.Y);
                    var pixX = xAxis.Scale.Transform (curve.IsOverrideOrdinal, i, pt.X);

                    if (pixX >= pane.Chart._rect.Left && pixX <= pane.Chart._rect.Right)
                    {
                        if (pixY > pane.Chart._rect.Bottom)
                        {
                            pixY = pane.Chart._rect.Bottom;
                        }

                        if (pixY < pane.Chart._rect.Top)
                        {
                            pixY = pane.Chart._rect.Top;
                        }

                        if (!curve.IsSelected && GradientFill.IsGradientValueType)
                        {
                            using (var tPen = GetPen (pane, scaleFactor, pt))
                            {
                                graphics.DrawLine (tPen, pixX, pixY, pixX, basePix);
                            }
                        }
                        else
                        {
                            graphics.DrawLine (pen, pixX, pixY, pixX, basePix);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draw the this <see cref="CurveItem"/> to the specified <see cref="Graphics"/>
    /// device using the specified smoothing property (<see cref="Line.SmoothTension"/>).
    /// The routine draws the line segments and the area fill (if any, see <see cref="FillType"/>;
    /// the symbols are drawn by the <see cref="Symbol.Draw"/> method.  This method
    /// is normally only called by the Draw method of the
    /// <see cref="CurveItem"/> object.  Note that the <see cref="StepType"/> property
    /// is ignored for smooth lines (e.g., when <see cref="Line.IsSmooth"/> is true).
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="curve">A <see cref="LineItem"/> representing this
    /// curve.</param>
    public void DrawSmoothFilledCurve
        (
            Graphics graphics,
            GraphPane pane,
            CurveItem curve,
            float scaleFactor
        )
    {
        var source = this;
        if (curve.IsSelected)
        {
            source = Selection.Line;
        }

        var points = curve.Points;
        if (IsVisible && !Color.IsEmpty && points != null &&
            BuildPointsArray (pane, curve, out var arrPoints, out var count) &&
            count > 2)
        {
            var tension = _isSmooth ? _smoothTension : 0f;

            // Fill the curve if needed
            if (Fill.IsVisible)
            {
                var yAxis = curve.GetYAxis (pane);

                using (var path = new GraphicsPath (FillMode.Winding))
                {
                    path.AddCurve (arrPoints!, 0, count - 2, tension);

                    var yMin = yAxis.Scale!._min < 0 ? 0.0 : yAxis.Scale._min;
                    CloseCurve (pane, curve, arrPoints!, count, yMin, path);

                    var rect = path.GetBounds();
                    using (var brush = source._fill.MakeBrush (rect))
                    {
                        if (pane.LineType == LineType.Stack && yAxis.Scale._min < 0 &&
                            IsFirstLine (pane, curve))
                        {
                            var zeroPix = yAxis.Scale.Transform (0);
                            var tRect = pane.Chart._rect;
                            tRect.Height = zeroPix - tRect.Top;
                            if (tRect.Height > 0)
                            {
                                var reg = graphics.Clip;
                                graphics.SetClip (tRect);
                                graphics.FillPath (brush, path);
                                graphics.SetClip (pane.Chart._rect);
                            }
                        }
                        else
                        {
                            graphics.FillPath (brush, path);
                        }

                        //brush.Dispose();
                    }

                    // restore the zero line if needed (since the fill tends to cover it up)
                    yAxis.FixZeroLine (graphics, pane, scaleFactor, rect.Left, rect.Right);
                }
            }

            // If it's a smooth curve, go ahead and render the path.  Otherwise, use the
            // standard drawcurve method just in case there are missing values.
            if (_isSmooth)
            {
                using (var pen = GetPen (pane, scaleFactor))
                {
                    // Stroke the curve
                    graphics.DrawCurve (pen, arrPoints!, 0, count - 2, tension);

                    //pen.Dispose();
                }
            }
            else
            {
                DrawCurve (graphics, pane, curve, scaleFactor);
            }
        }
    }

    private bool IsFirstLine
        (
            GraphPane pane,
            CurveItem curve
        )
    {
        var curveList = pane.CurveList;

        for (var j = 0; j < curveList.Count; j++)
        {
            var tCurve = curveList[j];

            if (tCurve is LineItem && tCurve.IsY2Axis == curve.IsY2Axis &&
                tCurve.YAxisIndex == curve.YAxisIndex)
            {
                return tCurve == curve;
            }
        }

        return false;
    }

    /// <summary>
    /// Draw the this <see cref="CurveItem"/> to the specified <see cref="Graphics"/>
    /// device.  The format (stair-step or line) of the curve is
    /// defined by the <see cref="StepType"/> property.  The routine
    /// only draws the line segments; the symbols are drawn by the
    /// <see cref="Symbol.Draw"/> method.  This method
    /// is normally only called by the Draw method of the
    /// <see cref="CurveItem"/> object
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="curve">A <see cref="LineItem"/> representing this
    /// curve.</param>
    public void DrawCurve
        (
            Graphics graphics,
            GraphPane pane,
            CurveItem curve,
            float scaleFactor
        )
    {
        var source = this;
        if (curve.IsSelected)
        {
            source = Selection.Line;
        }

        // switch to int to optimize drawing speed (per Dale-a-b)
        int tmpX,
            tmpY,
            lastX = int.MaxValue,
            lastY = int.MaxValue;

        double curX, curY, lowVal;
        PointPair curPt, lastPt = new PointPair();

        var lastBad = true;
        var points = curve.Points;
        var valueHandler = new ValueHandler (pane, false);
        var yAxis = curve.GetYAxis (pane);
        var xAxis = curve.GetXAxis (pane);

        var xIsLog = xAxis.Scale!.IsLog;
        var yIsLog = yAxis.Scale!.IsLog;

        // switch to int to optimize drawing speed (per Dale-a-b)
        var minX = (int)pane.Chart.Rect.Left;
        var maxX = (int)pane.Chart.Rect.Right;
        var minY = (int)pane.Chart.Rect.Top;
        var maxY = (int)pane.Chart.Rect.Bottom;

        using (var pen = source.GetPen (pane, scaleFactor))
        {
            if (points != null && !_color.IsEmpty && IsVisible)
            {
                //bool lastOut = false;
                bool isOut;

                var isOptDraw = _isOptimizedDraw && points.Count > 1000;

                // (Dale-a-b) we'll set an element to true when it has been drawn
                bool[,] isPixelDrawn = null!;

                if (isOptDraw)
                {
                    isPixelDrawn = new bool[maxX + 1, maxY + 1];
                }

                // Loop over each point in the curve
                for (var i = 0; i < points.Count; i++)
                {
                    curPt = points[i];
                    if (pane.LineType == LineType.Stack)
                    {
                        if (!valueHandler.GetValues (curve, i, out curX, out lowVal, out curY))
                        {
                            curX = PointPairBase.Missing;
                            curY = PointPairBase.Missing;
                        }
                    }
                    else
                    {
                        curX = curPt.X;
                        curY = curPt.Y;
                    }

                    // Any value set to double max is invalid and should be skipped
                    // This is used for calculated values that are out of range, divide
                    //   by zero, etc.
                    // Also, any value <= zero on a log scale is invalid
                    if (curX == PointPairBase.Missing ||
                        curY == PointPairBase.Missing ||
                        double.IsNaN (curX) ||
                        double.IsNaN (curY) ||
                        double.IsInfinity (curX) ||
                        double.IsInfinity (curY) ||
                        (xIsLog && curX <= 0.0) ||
                        (yIsLog && curY <= 0.0))
                    {
                        // If the point is invalid, then make a linebreak only if IsIgnoreMissing is false
                        // LastX and LastY are always the last valid point, so this works out
                        lastBad = lastBad || !pane.IsIgnoreMissing;
                        isOut = true;
                    }
                    else
                    {
                        // Transform the current point from user scale units to
                        // screen coordinates
                        tmpX = (int)xAxis.Scale.Transform (curve.IsOverrideOrdinal, i, curX);
                        tmpY = (int)yAxis.Scale.Transform (curve.IsOverrideOrdinal, i, curY);

                        // Maintain an array of "used" pixel locations to avoid duplicate drawing operations
                        // contributed by Dale-a-b
                        if (isOptDraw && tmpX >= minX && tmpX <= maxX &&
                            tmpY >= minY && tmpY <= maxY) // guard against the zoom-in case
                        {
                            if (isPixelDrawn[tmpX, tmpY])
                            {
                                continue;
                            }

                            isPixelDrawn[tmpX, tmpY] = true;
                        }

                        isOut = (tmpX < minX && lastX < minX) || (tmpX > maxX && lastX > maxX) ||
                                (tmpY < minY && lastY < minY) || (tmpY > maxY && lastY > maxY);

                        if (!lastBad)
                        {
                            try
                            {
                                // GDI+ plots the data wrong and/or throws an exception for
                                // outrageous coordinates, so we do a sanity check here
                                if (lastX > 5000000 || lastX < -5000000 ||
                                    lastY > 5000000 || lastY < -5000000 ||
                                    tmpX > 5000000 || tmpX < -5000000 ||
                                    tmpY > 5000000 || tmpY < -5000000)
                                {
                                    InterpolatePoint (graphics, pane, curve, lastPt, scaleFactor, pen,
                                        lastX, lastY, tmpX, tmpY);
                                }
                                else if (!isOut)
                                {
                                    if (!curve.IsSelected && GradientFill.IsGradientValueType)
                                    {
                                        using (var tPen = GetPen (pane, scaleFactor, lastPt))
                                        {
                                            if (StepType == StepType.NonStep)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, tmpX, tmpY);
                                            }
                                            else if (StepType == StepType.ForwardStep)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, tmpX, lastY);
                                                graphics.DrawLine (tPen, tmpX, lastY, tmpX, tmpY);
                                            }
                                            else if (StepType == StepType.RearwardStep)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, lastX, tmpY);
                                                graphics.DrawLine (tPen, lastX, tmpY, tmpX, tmpY);
                                            }
                                            else if (StepType == StepType.ForwardSegment)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, tmpX, lastY);
                                            }
                                            else
                                            {
                                                graphics.DrawLine (tPen, lastX, tmpY, tmpX, tmpY);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (StepType == StepType.NonStep)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, tmpX, tmpY);
                                        }
                                        else if (StepType == StepType.ForwardStep)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, tmpX, lastY);
                                            graphics.DrawLine (pen, tmpX, lastY, tmpX, tmpY);
                                        }
                                        else if (StepType == StepType.RearwardStep)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, lastX, tmpY);
                                            graphics.DrawLine (pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                        else if (StepType == StepType.ForwardSegment)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, tmpX, lastY);
                                        }
                                        else if (StepType == StepType.RearwardSegment)
                                        {
                                            graphics.DrawLine (pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                InterpolatePoint (graphics, pane, curve, lastPt, scaleFactor, pen,
                                    lastX, lastY, tmpX, tmpY);
                            }
                        }

                        lastPt = curPt;
                        lastX = tmpX;
                        lastY = tmpY;
                        lastBad = false;

                        //lastOut = isOut;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draw the this <see cref="CurveItem"/> to the specified <see cref="Graphics"/>
    /// device.  The format (stair-step or line) of the curve is
    /// defined by the <see cref="StepType"/> property.  The routine
    /// only draws the line segments; the symbols are drawn by the
    /// <see cref="Symbol.Draw"/> method.  This method
    /// is normally only called by the Draw method of the
    /// <see cref="CurveItem"/> object
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="curve">A <see cref="LineItem"/> representing this
    /// curve.</param>
    public void DrawCurveOriginal
        (
            Graphics graphics,
            GraphPane pane,
            CurveItem curve,
            float scaleFactor
        )
    {
        var source = this;
        if (curve.IsSelected)
        {
            source = Selection.Line;
        }

        float tmpX,
            tmpY,
            lastX = float.MaxValue,
            lastY = float.MaxValue;
        double curX, curY, lowVal;
        PointPair curPt, lastPt = new PointPair();

        var lastBad = true;
        var points = curve.Points;
        var valueHandler = new ValueHandler (pane, false);
        var yAxis = curve.GetYAxis (pane);
        var xAxis = curve.GetXAxis (pane);

        var xIsLog = xAxis.Scale!.IsLog;
        var yIsLog = yAxis.Scale!.IsLog;

        var minX = pane.Chart.Rect.Left;
        var maxX = pane.Chart.Rect.Right;
        var minY = pane.Chart.Rect.Top;
        var maxY = pane.Chart.Rect.Bottom;

        using (var pen = source.GetPen (pane, scaleFactor))
        {
            if (points != null && !_color.IsEmpty && IsVisible)
            {
                //bool lastOut = false;
                bool isOut;

                // Loop over each point in the curve
                for (var i = 0; i < points.Count; i++)
                {
                    curPt = points[i];
                    if (pane.LineType == LineType.Stack)
                    {
                        if (!valueHandler.GetValues (curve, i, out curX, out lowVal, out curY))
                        {
                            curX = PointPairBase.Missing;
                            curY = PointPairBase.Missing;
                        }
                    }
                    else
                    {
                        curX = curPt.X;
                        curY = curPt.Y;
                    }

                    // Any value set to double max is invalid and should be skipped
                    // This is used for calculated values that are out of range, divide
                    //   by zero, etc.
                    // Also, any value <= zero on a log scale is invalid
                    if (curX == PointPairBase.Missing ||
                        curY == PointPairBase.Missing ||
                        double.IsNaN (curX) ||
                        double.IsNaN (curY) ||
                        double.IsInfinity (curX) ||
                        double.IsInfinity (curY) ||
                        (xIsLog && curX <= 0.0) ||
                        (yIsLog && curY <= 0.0))
                    {
                        // If the point is invalid, then make a linebreak only if IsIgnoreMissing is false
                        // LastX and LastY are always the last valid point, so this works out
                        lastBad = lastBad || !pane.IsIgnoreMissing;
                        isOut = true;
                    }
                    else
                    {
                        // Transform the current point from user scale units to
                        // screen coordinates
                        tmpX = xAxis.Scale.Transform (curve.IsOverrideOrdinal, i, curX);
                        tmpY = yAxis.Scale.Transform (curve.IsOverrideOrdinal, i, curY);
                        isOut = (tmpX < minX && lastX < minX) || (tmpX > maxX && lastX > maxX) ||
                                (tmpY < minY && lastY < minY) || (tmpY > maxY && lastY > maxY);

                        if (!lastBad)
                        {
                            try
                            {
                                // GDI+ plots the data wrong and/or throws an exception for
                                // outrageous coordinates, so we do a sanity check here
                                if (lastX > 5000000 || lastX < -5000000 ||
                                    lastY > 5000000 || lastY < -5000000 ||
                                    tmpX > 5000000 || tmpX < -5000000 ||
                                    tmpY > 5000000 || tmpY < -5000000)
                                {
                                    InterpolatePoint (graphics, pane, curve, lastPt, scaleFactor, pen,
                                        lastX, lastY, tmpX, tmpY);
                                }
                                else if (!isOut)
                                {
                                    if (!curve.IsSelected && GradientFill.IsGradientValueType)
                                    {
                                        using (var tPen = GetPen (pane, scaleFactor, lastPt))
                                        {
                                            if (StepType == StepType.NonStep)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, tmpX, tmpY);
                                            }
                                            else if (StepType == StepType.ForwardStep)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, tmpX, lastY);
                                                graphics.DrawLine (tPen, tmpX, lastY, tmpX, tmpY);
                                            }
                                            else if (StepType == StepType.RearwardStep)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, lastX, tmpY);
                                                graphics.DrawLine (tPen, lastX, tmpY, tmpX, tmpY);
                                            }
                                            else if (StepType == StepType.ForwardSegment)
                                            {
                                                graphics.DrawLine (tPen, lastX, lastY, tmpX, lastY);
                                            }
                                            else
                                            {
                                                graphics.DrawLine (tPen, lastX, tmpY, tmpX, tmpY);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (StepType == StepType.NonStep)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, tmpX, tmpY);
                                        }
                                        else if (StepType == StepType.ForwardStep)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, tmpX, lastY);
                                            graphics.DrawLine (pen, tmpX, lastY, tmpX, tmpY);
                                        }
                                        else if (StepType == StepType.RearwardStep)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, lastX, tmpY);
                                            graphics.DrawLine (pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                        else if (StepType == StepType.ForwardSegment)
                                        {
                                            graphics.DrawLine (pen, lastX, lastY, tmpX, lastY);
                                        }
                                        else if (StepType == StepType.RearwardSegment)
                                        {
                                            graphics.DrawLine (pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                InterpolatePoint (graphics, pane, curve, lastPt, scaleFactor, pen,
                                    lastX, lastY, tmpX, tmpY);
                            }
                        }

                        lastPt = curPt;
                        lastX = tmpX;
                        lastY = tmpY;
                        lastBad = false;

                        //lastOut = isOut;
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method just handles the case where one or more of the coordinates are outrageous,
    /// or GDI+ threw an exception.  This method attempts to correct the outrageous coordinates by
    /// interpolating them to a point (along the original line) that lies at the edge of the ChartRect
    /// so that GDI+ will handle it properly.  GDI+ will throw an exception, or just plot the data
    /// incorrectly if the coordinates are too large (empirically, this appears to be when the
    /// coordinate value is greater than 5,000,000 or less than -5,000,000).  Although you typically
    /// would not see coordinates like this, if you repeatedly zoom in on a ZedGraphControl, eventually
    /// all your points will be way outside the bounds of the plot.
    /// </summary>
    private void InterpolatePoint
        (
            Graphics graphics,
            GraphPane pane,
            CurveItem curve,
            PointPair lastPt,
            float scaleFactor,
            Pen pen,
            float lastX,
            float lastY,
            float tmpX,
            float tmpY
        )
    {
        try
        {
            var chartRect = pane.Chart._rect;

            // try to interpolate values
            var lastIn = chartRect.Contains (lastX, lastY);
            var curIn = chartRect.Contains (tmpX, tmpY);

            // If both points are outside the ChartRect, make a new point that is on the LastX/Y
            // side of the ChartRect, and fall through to the code that handles lastIn == true
            if (!lastIn)
            {
                float newX, newY;

                if (Math.Abs (lastX) > Math.Abs (lastY))
                {
                    newX = lastX < 0 ? chartRect.Left : chartRect.Right;
                    newY = lastY + (tmpY - lastY) * (newX - lastX) / (tmpX - lastX);
                }
                else
                {
                    newY = lastY < 0 ? chartRect.Top : chartRect.Bottom;
                    newX = lastX + (tmpX - lastX) * (newY - lastY) / (tmpY - lastY);
                }

                lastX = newX;
                lastY = newY;
            }

            if (!curIn)
            {
                float newX, newY;

                if (Math.Abs (tmpX) > Math.Abs (tmpY))
                {
                    newX = tmpX < 0 ? chartRect.Left : chartRect.Right;
                    newY = tmpY + (lastY - tmpY) * (newX - tmpX) / (lastX - tmpX);
                }
                else
                {
                    newY = tmpY < 0 ? chartRect.Top : chartRect.Bottom;
                    newX = tmpX + (lastX - tmpX) * (newY - tmpY) / (lastY - tmpY);
                }

                tmpX = newX;
                tmpY = newY;
            }

            /*
            if ( this.StepType == StepType.ForwardStep )
            {
                g.DrawLine( pen, lastX, lastY, tmpX, lastY );
                g.DrawLine( pen, tmpX, lastY, tmpX, tmpY );
            }
            else if ( this.StepType == StepType.RearwardStep )
            {
                g.DrawLine( pen, lastX, lastY, lastX, tmpY );
                g.DrawLine( pen, lastX, tmpY, tmpX, tmpY );
            }
            else 		// non-step
                g.DrawLine( pen, lastX, lastY, tmpX, tmpY );
            */
            if (!curve.IsSelected && GradientFill.IsGradientValueType)
            {
                using (var tPen = GetPen (pane, scaleFactor, lastPt))
                {
                    if (StepType == StepType.NonStep)
                    {
                        graphics.DrawLine (tPen, lastX, lastY, tmpX, tmpY);
                    }
                    else if (StepType == StepType.ForwardStep)
                    {
                        graphics.DrawLine (tPen, lastX, lastY, tmpX, lastY);
                        graphics.DrawLine (tPen, tmpX, lastY, tmpX, tmpY);
                    }
                    else if (StepType == StepType.RearwardStep)
                    {
                        graphics.DrawLine (tPen, lastX, lastY, lastX, tmpY);
                        graphics.DrawLine (tPen, lastX, tmpY, tmpX, tmpY);
                    }
                    else if (StepType == StepType.ForwardSegment)
                    {
                        graphics.DrawLine (tPen, lastX, lastY, tmpX, lastY);
                    }
                    else
                    {
                        graphics.DrawLine (tPen, lastX, tmpY, tmpX, tmpY);
                    }
                }
            }
            else
            {
                if (StepType == StepType.NonStep)
                {
                    graphics.DrawLine (pen, lastX, lastY, tmpX, tmpY);
                }
                else if (StepType == StepType.ForwardStep)
                {
                    graphics.DrawLine (pen, lastX, lastY, tmpX, lastY);
                    graphics.DrawLine (pen, tmpX, lastY, tmpX, tmpY);
                }
                else if (StepType == StepType.RearwardStep)
                {
                    graphics.DrawLine (pen, lastX, lastY, lastX, tmpY);
                    graphics.DrawLine (pen, lastX, tmpY, tmpX, tmpY);
                }
                else if (StepType == StepType.ForwardSegment)
                {
                    graphics.DrawLine (pen, lastX, lastY, tmpX, lastY);
                }
                else if (StepType == StepType.RearwardSegment)
                {
                    graphics.DrawLine (pen, lastX, tmpY, tmpX, tmpY);
                }
            }
        }

        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }
    }

    /// <summary>
    /// Build an array of <see cref="PointF"/> values (pixel coordinates) that represents
    /// the current curve.  Note that this drawing routine ignores <see cref="PointPairBase.Missing"/>
    /// values, but it does not "break" the line to indicate values are missing.
    /// </summary>
    /// <param name="pane">A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.</param>
    /// <param name="curve">A <see cref="LineItem"/> representing this
    /// curve.</param>
    /// <param name="arrPoints">An array of <see cref="PointF"/> values in pixel
    /// coordinates representing the current curve.</param>
    /// <param name="count">The number of points contained in the "arrPoints"
    /// parameter.</param>
    /// <returns>true for a successful points array build, false for data problems</returns>
    public bool BuildPointsArray
        (
            GraphPane pane,
            CurveItem curve,
            out PointF[]? arrPoints,
            out int count
        )
    {
        arrPoints = null;
        count = 0;
        var points = curve.Points;

        if (IsVisible && !Color.IsEmpty && points != null)
        {
            var index = 0;
            float curX,
                curY,
                lastX = 0,
                lastY = 0;
            double x, y, lowVal;
            var valueHandler = new ValueHandler (pane, false);

            // Step type plots get twice as many points.  Always add three points so there is
            // room to close out the curve for area fills.
            arrPoints = new PointF[(_stepType == StepType.NonStep ? 1 : 2) *
                points.Count + 1];

            // Loop over all points in the curve
            for (var i = 0; i < points.Count; i++)
            {
                // make sure that the current point is valid
                if (!points[i].IsInvalid)
                {
                    // Get the user scale values for the current point
                    // use the valueHandler only for stacked types
                    if (pane.LineType == LineType.Stack)
                    {
                        valueHandler.GetValues (curve, i, out x, out lowVal, out y);
                    }

                    // otherwise, just access the values directly.  Avoiding the valueHandler for
                    // non-stacked types is an optimization to minimize overhead in case there are
                    // a large number of points.
                    else
                    {
                        x = points[i].X;
                        y = points[i].Y;
                    }

                    if (x == PointPairBase.Missing || y == PointPairBase.Missing)
                    {
                        continue;
                    }

                    // Transform the user scale values to pixel locations
                    var xAxis = curve.GetXAxis (pane);
                    curX = xAxis.Scale!.Transform (curve.IsOverrideOrdinal, i, x);
                    var yAxis = curve.GetYAxis (pane);
                    curY = yAxis.Scale!.Transform (curve.IsOverrideOrdinal, i, y);

                    if (curX < -1000000 || curY < -1000000 || curX > 1000000 || curY > 1000000)
                    {
                        continue;
                    }

                    // Add the pixel value pair into the points array
                    // Two points are added for step type curves
                    // ignore step-type setting for smooth curves
                    if (_isSmooth || index == 0 || StepType == StepType.NonStep)
                    {
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = curY;
                    }
                    else if (StepType == StepType.ForwardStep ||
                             StepType == StepType.ForwardSegment)
                    {
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = lastY;
                        index++;
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = curY;
                    }
                    else if (StepType == StepType.RearwardStep ||
                             StepType == StepType.RearwardSegment)
                    {
                        arrPoints[index].X = lastX;
                        arrPoints[index].Y = curY;
                        index++;
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = curY;
                    }

                    lastX = curX;
                    lastY = curY;
                    index++;
                }
            }

            // Make sure there is at least one valid point
            if (index == 0)
            {
                return false;
            }

            // Add an extra point at the end, since the smoothing algorithm requires it
            arrPoints[index] = arrPoints[index - 1];
            index++;

            count = index;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Build an array of <see cref="PointF"/> values (pixel coordinates) that represents
    /// the low values for the current curve.
    /// </summary>
    /// <remarks>Note that this drawing routine ignores <see cref="PointPairBase.Missing"/>
    /// values, but it does not "break" the line to indicate values are missing.
    /// </remarks>
    /// <param name="pane">A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.</param>
    /// <param name="curve">A <see cref="LineItem"/> representing this
    /// curve.</param>
    /// <param name="arrPoints">An array of <see cref="PointF"/> values in pixel
    /// coordinates representing the current curve.</param>
    /// <param name="count">The number of points contained in the "arrPoints"
    /// parameter.</param>
    /// <returns>true for a successful points array build, false for data problems</returns>
    public bool BuildLowPointsArray
        (
            GraphPane pane,
            CurveItem curve,
            out PointF[]? arrPoints,
            out int count
        )
    {
        arrPoints = null;
        count = 0;
        var points = curve.Points;

        if (IsVisible && !Color.IsEmpty && points != null)
        {
            var index = 0;
            float curX,
                curY,
                lastX = 0,
                lastY = 0;
            double x, y, hiVal;
            var valueHandler = new ValueHandler (pane, false);

            // Step type plots get twice as many points.  Always add three points so there is
            // room to close out the curve for area fills.
            arrPoints = new PointF[(_stepType == StepType.NonStep ? 1 : 2) *
                (pane.LineType == LineType.Stack ? 2 : 1) *
                points.Count + 1];

            // Loop backwards over all points in the curve
            // In this case an array of points was already built forward by BuildPointsArray().
            // This time we build backwards to complete a loop around the area between two curves.
            for (var i = points.Count - 1; i >= 0; i--)
            {
                // Make sure the current point is valid
                if (!points[i].IsInvalid)
                {
                    // Get the user scale values for the current point
                    valueHandler.GetValues (curve, i, out x, out y, out hiVal);

                    if (x == PointPairBase.Missing || y == PointPairBase.Missing)
                    {
                        continue;
                    }

                    // Transform the user scale values to pixel locations
                    var xAxis = curve.GetXAxis (pane);
                    curX = xAxis.Scale!.Transform (curve.IsOverrideOrdinal, i, x);
                    var yAxis = curve.GetYAxis (pane);
                    curY = yAxis.Scale!.Transform (curve.IsOverrideOrdinal, i, y);

                    // Add the pixel value pair into the points array
                    // Two points are added for step type curves
                    // ignore step-type setting for smooth curves
                    if (_isSmooth || index == 0 || StepType == StepType.NonStep)
                    {
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = curY;
                    }
                    else if (StepType == StepType.ForwardStep)
                    {
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = lastY;
                        index++;
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = curY;
                    }
                    else if (StepType == StepType.RearwardStep)
                    {
                        arrPoints[index].X = lastX;
                        arrPoints[index].Y = curY;
                        index++;
                        arrPoints[index].X = curX;
                        arrPoints[index].Y = curY;
                    }

                    lastX = curX;
                    lastY = curY;
                    index++;
                }
            }

            // Make sure there is at least one valid point
            if (index == 0)
            {
                return false;
            }

            // Add an extra point at the end, since the smoothing algorithm requires it
            arrPoints[index] = arrPoints[index - 1];
            index++;

            count = index;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Close off a <see cref="GraphicsPath"/> that defines a curve
    /// </summary>
    /// <param name="pane">A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.</param>
    /// <param name="curve">A <see cref="LineItem"/> representing this
    /// curve.</param>
    /// <param name="arrPoints">An array of <see cref="PointF"/> values in screen pixel
    /// coordinates representing the current curve.</param>
    /// <param name="count">The number of points contained in the "arrPoints"
    /// parameter.</param>
    /// <param name="yMin">The Y axis value location where the X axis crosses.</param>
    /// <param name="path">The <see cref="GraphicsPath"/> class that represents the curve.</param>
    public void CloseCurve
        (
            GraphPane pane,
            CurveItem curve,
            PointF[] arrPoints,
            int count,
            double yMin,
            GraphicsPath path
        )
    {
        // For non-stacked lines, the fill area is just the area between the curve and the X axis
        if (pane.LineType != LineType.Stack)
        {
            // Determine the current value for the bottom of the curve (usually the Y value where
            // the X axis crosses)
            float yBase;
            var yAxis = curve.GetYAxis (pane);
            yBase = yAxis.Scale!.Transform (yMin);

            // Add three points to the path to move from the end of the curve (as defined by
            // arrPoints) to the X axis, from there to the start of the curve at the X axis,
            // and from there back up to the beginning of the curve.
            path.AddLine (arrPoints[count - 1].X, arrPoints[count - 1].Y, arrPoints[count - 1].X, yBase);
            path.AddLine (arrPoints[count - 1].X, yBase, arrPoints[0].X, yBase);
            path.AddLine (arrPoints[0].X, yBase, arrPoints[0].X, arrPoints[0].Y);
        }

        // For stacked line types, the fill area is the area between this curve and the curve below it
        else
        {
            PointF[] arrPoints2;
            int count2;

            var tension = _isSmooth ? _smoothTension : 0f;

            // Find the next lower curve in the curveList that is also a LineItem type, and use
            // its smoothing properties for the lower side of the filled area.
            var index = pane.CurveList.IndexOf (curve);
            if (index > 0)
            {
                CurveItem tmpCurve;
                for (var i = index - 1; i >= 0; i--)
                {
                    tmpCurve = pane.CurveList[i];
                    if (tmpCurve is LineItem item)
                    {
                        tension = item.Line.IsSmooth ? item.Line.SmoothTension : 0f;
                        break;
                    }
                }
            }

            // Build another points array consisting of the low points (which are actually the points for
            // the curve below the current curve)
            BuildLowPointsArray (pane, curve, out arrPoints2!, out count2);

            // Add the new points to the GraphicsPath
            path.AddCurve (arrPoints2, 0, count2 - 2, tension);
        }
    }

    #endregion
}
