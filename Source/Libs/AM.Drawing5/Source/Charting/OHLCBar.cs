// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* OHLCBar.cs -- рисование объектов-кривых
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Этот класс управляет рисованием объектов-кривых <see cref="OHLCBar"/>.
/// </summary>
[Serializable]
public class OHLCBar
    : LineBase, ICloneable, ISerializable
{
    #region Fields

    /// <summary>
    /// Private field that stores the total width for the Opening/Closing line
    /// segments.  Use the public property <see cref="Size"/> to access this value.
    /// </summary>
    protected float _size;

    /// <summary>
    /// The result of the autosize calculation, which is the size of the bars in
    /// user scale units.  This is converted to pixels at draw time.
    /// </summary>
    internal double _userScaleSize = 1.0;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="OHLCBar"/> class.
    /// </summary>
    public new struct Default
    {
        // Default Symbol properties
        /// <summary>
        /// The default width for the candlesticks (see <see cref="OHLCBar.Size" />),
        /// in units of points.
        /// </summary>
        public static float Size = 12;

        /// <summary>
        /// The default display mode for symbols (<see cref="OHLCBar.IsOpenCloseVisible"/> property).
        /// true to display symbols, false to hide them.
        /// </summary>
        public static bool IsOpenCloseVisible = true;

        /// <summary>
        /// The default value for the <see cref="OHLCBar.IsAutoSize" /> property.
        /// </summary>
        public static bool IsAutoSize = true;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a property that shows or hides the <see cref="OHLCBar"/> open/close "wings".
    /// </summary>
    /// <value>true to show the CandleStick wings, false to hide them</value>
    /// <seealso cref="Default.IsOpenCloseVisible"/>
    public bool IsOpenCloseVisible { get; set; }

    /// <summary>
    /// Gets or sets the total width to be used for drawing the opening/closing line
    /// segments ("wings") of the <see cref="OHLCBar" /> items. Units are points.
    /// </summary>
    /// <remarks>The size of the candlesticks can be set by this value, which
    /// is then scaled according to the scaleFactor (see
    /// <see cref="PaneBase.CalcScaleFactor"/>).  Alternatively,
    /// if <see cref="IsAutoSize"/> is true, the bar width will
    /// be set according to the maximum available cluster width less
    /// the cluster gap (see <see cref="BarSettings.GetClusterWidth"/>
    /// and <see cref="BarSettings.MinClusterGap"/>).  That is, if
    /// <see cref="IsAutoSize"/> is true, then the value of
    /// <see cref="Size"/> will be ignored.  If you modify the value of Size,
    /// then <see cref="IsAutoSize" /> will be automatically set to false.
    /// </remarks>
    /// <value>Size in points (1/72 inch)</value>
    /// <seealso cref="Default.Size"/>
    public float Size
    {
        get => _size;
        set
        {
            _size = value;
            IsAutoSize = false;
        }
    }

    /// <summary>
    /// Gets or sets a value that determines if the <see cref="Size" /> property will be
    /// calculated automatically based on the minimum axis scale step size between
    /// bars.
    /// </summary>
    public bool IsAutoSize { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor that sets all <see cref="OHLCBar"/> properties to
    /// default values as defined in the <see cref="Default"/> class.
    /// </summary>
    public OHLCBar()
        : this (LineBase.Default.Color)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Default constructor that sets the
    /// <see cref="Color"/> as specified, and the remaining
    /// <see cref="OHLCBar"/> properties to default
    /// values as defined in the <see cref="Default"/> class.
    /// </summary>
    /// <param name="color">A <see cref="Color"/> value indicating
    /// the color of the symbol
    /// </param>
    public OHLCBar (Color color) : base (color)
    {
        _size = Default.Size;
        IsAutoSize = Default.IsAutoSize;
        IsOpenCloseVisible = Default.IsOpenCloseVisible;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="OHLCBar"/> object from which to copy</param>
    public OHLCBar (OHLCBar rhs) : base (rhs)
    {
        IsOpenCloseVisible = rhs.IsOpenCloseVisible;
        _size = rhs._size;
        IsAutoSize = rhs.IsAutoSize;
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
    public OHLCBar Clone()
    {
        return new OHLCBar (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema = 10;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected OHLCBar
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema").NotUsed();

        IsOpenCloseVisible = info.GetBoolean ("isOpenCloseVisible");
        _size = info.GetSingle ("size");
        IsAutoSize = info.GetBoolean ("isAutoSize");
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);
        info.AddValue ("schema", schema);
        info.AddValue ("isOpenCloseVisible", IsOpenCloseVisible);
        info.AddValue ("size", _size);
        info.AddValue ("isAutoSize", IsAutoSize);
    }

    #endregion

    #region Rendering Methods

    /// <summary>
    /// Draw the <see cref="OHLCBar"/> to the specified <see cref="Graphics"/>
    /// device at the specified location.
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="isXBase">boolean value that indicates if the "base" axis for this
    /// <see cref="OHLCBar"/> is the X axis.  True for an <see cref="XAxis"/> base,
    /// false for a <see cref="YAxis"/> or <see cref="Y2Axis"/> base.</param>
    /// <param name="pixBase">The independent axis position of the center of the candlestick in
    /// pixel units</param>
    /// <param name="pixHigh">The dependent axis position of the top of the candlestick in
    /// pixel units</param>
    /// <param name="pixLow">The dependent axis position of the bottom of the candlestick in
    /// pixel units</param>
    /// <param name="pixOpen">The dependent axis position of the opening value of the candlestick in
    /// pixel units</param>
    /// <param name="pixClose">The dependent axis position of the closing value of the candlestick in
    /// pixel units</param>
    /// <param name="halfSize">
    /// The scaled width of the candlesticks, pixels</param>
    /// <param name="pen">A pen with attributes of <see cref="Color"/> and
    /// <see cref="LineBase.Width"/> for this <see cref="OHLCBar"/></param>
    public void Draw (Graphics graphics, GraphPane pane, bool isXBase,
        float pixBase, float pixHigh, float pixLow,
        float pixOpen, float pixClose,
        float halfSize, Pen pen)
    {
        if (pixBase != PointPairBase.Missing)
        {
            if (isXBase)
            {
                if (Math.Abs (pixLow) < 1_000_000 && Math.Abs (pixHigh) < 1_000_000)
                {
                    graphics.DrawLine (pen, pixBase, pixHigh, pixBase, pixLow);
                }

                if (IsOpenCloseVisible && Math.Abs (pixOpen) < 1_000_000)
                {
                    graphics.DrawLine (pen, pixBase - halfSize, pixOpen, pixBase, pixOpen);
                }

                if (IsOpenCloseVisible && Math.Abs (pixClose) < 1_000_000)
                {
                    graphics.DrawLine (pen, pixBase, pixClose, pixBase + halfSize, pixClose);
                }
            }
            else
            {
                if (Math.Abs (pixLow) < 1_000_000 && Math.Abs (pixHigh) < 1_000_000)
                {
                    graphics.DrawLine (pen, pixHigh, pixBase, pixLow, pixBase);
                }

                if (IsOpenCloseVisible && Math.Abs (pixOpen) < 1_000_000)
                {
                    graphics.DrawLine (pen, pixOpen, pixBase - halfSize, pixOpen, pixBase);
                }

                if (IsOpenCloseVisible && Math.Abs (pixClose) < 1_000_000)
                {
                    graphics.DrawLine (pen, pixClose, pixBase, pixClose, pixBase + halfSize);
                }
            }
        }
    }


    /// <summary>
    /// Draw all the <see cref="OHLCBar"/>'s to the specified <see cref="Graphics"/>
    /// device as a candlestick at each defined point.
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="curve">A <see cref="OHLCBarItem"/> object representing the
    /// <see cref="OHLCBar"/>'s to be drawn.</param>
    /// <param name="baseAxis">The <see cref="Axis"/> class instance that defines the base (independent)
    /// axis for the <see cref="OHLCBar"/></param>
    /// <param name="valueAxis">The <see cref="Axis"/> class instance that defines the value (dependent)
    /// axis for the <see cref="OHLCBar"/></param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    public void Draw
        (
            Graphics graphics,
            GraphPane pane,
            OHLCBarItem curve,
            Axis baseAxis,
            Axis valueAxis,
            float scaleFactor
        )
    {
        //ValueHandler valueHandler = new ValueHandler( pane, false );

        float pixHigh, pixLow, pixOpen, pixClose;

        if (curve.Points != null)
        {
            //float halfSize = _size * scaleFactor;
            var halfSize = GetBarWidth (pane, baseAxis, scaleFactor);

            using var pen = !curve.IsSelected
                ? new Pen (_color, _width)
                : new Pen (Selection.Border.Color, Selection.Border.Width);

            // Loop over each defined point
            for (var i = 0; i < curve.Points.Count; i++)
            {
                var pt = curve.Points[i];
                var date = pt.X;
                var high = pt.Y;
                var low = pt.Z;
                var open = PointPairBase.Missing;
                var close = PointPairBase.Missing;
                if (pt is StockPoint)
                {
                    open = (pt as StockPoint).Open;
                    close = (pt as StockPoint).Close;
                }

                // Any value set to double max is invalid and should be skipped
                // This is used for calculated values that are out of range, divide
                //   by zero, etc.
                // Also, any value <= zero on a log scale is invalid

                if (!curve.Points[i].IsInvalid3D &&
                    (date > 0 || !baseAxis.Scale.IsLog) &&
                    (high > 0 && low > 0 || !valueAxis.Scale.IsLog))
                {
                    float pixBase = (int)(baseAxis.Scale.Transform (curve.IsOverrideOrdinal, i, date) + 0.5);

                    //pixBase = baseAxis.Scale.Transform( curve.IsOverrideOrdinal, i, date );
                    pixHigh = valueAxis.Scale.Transform (curve.IsOverrideOrdinal, i, high);
                    pixLow = valueAxis.Scale.Transform (curve.IsOverrideOrdinal, i, low);
                    if (PointPairBase.IsValueInvalid (open))
                    {
                        pixOpen = float.MaxValue;
                    }
                    else
                    {
                        pixOpen = valueAxis.Scale.Transform (curve.IsOverrideOrdinal, i, open);
                    }

                    if (PointPairBase.IsValueInvalid (close))
                    {
                        pixClose = float.MaxValue;
                    }
                    else
                    {
                        pixClose = valueAxis.Scale.Transform (curve.IsOverrideOrdinal, i, close);
                    }

                    if (!curve.IsSelected && _gradientFill.IsGradientValueType)
                    {
                        using var tPen = GetPen (pane, scaleFactor, pt);
                        Draw
                            (
                                graphics,
                                pane,
                                baseAxis is XAxis or X2Axis,
                                pixBase,
                                pixHigh, pixLow, pixOpen,
                                pixClose,
                                halfSize,
                                tPen
                            );
                    }
                    else
                    {
                        Draw
                            (
                                graphics,
                                pane,
                                baseAxis is XAxis or X2Axis,
                                pixBase,
                                pixHigh,
                                pixLow,
                                pixOpen,
                                pixClose,
                                halfSize,
                                pen
                            );
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns the width of the candleStick, in pixels, based on the settings for
    /// <see cref="Size"/> and <see cref="IsAutoSize"/>.
    /// </summary>
    /// <param name="pane">The parent <see cref="GraphPane"/> object.</param>
    /// <param name="baseAxis">The <see cref="Axis"/> object that
    /// represents the bar base (independent axis).</param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <returns>The width of each bar, in pixel units</returns>
    public float GetBarWidth
        (
            GraphPane pane,
            Axis baseAxis,
            float scaleFactor
        )
    {
        float width;
        if (IsAutoSize)
        {
            width = baseAxis.Scale.GetClusterWidth (_userScaleSize) /
                    (1.0F + pane._barSettings.MinClusterGap) / 2.0f;
        }
        else
        {
            width = _size * scaleFactor / 2.0f;
        }

        // use integral size
        return (int)(width + 0.5f);
    }

    #endregion
}
