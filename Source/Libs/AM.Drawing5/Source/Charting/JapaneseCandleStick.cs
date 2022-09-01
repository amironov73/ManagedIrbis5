// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* JapanceseCandleStick.cs --
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
/// This class handles the drawing of the curve
/// <see cref="JapaneseCandleStick"/> objects.
/// </summary>
[Serializable]
public class JapaneseCandleStick
    : OHLCBar, ICloneable
{
    #region Fields

    /// <summary>
    /// Private field to store the <see cref="Fill" /> class to be used for filling the
    /// candlestick "bars" when the <see cref="StockPoint.Close" /> value is greater than
    /// the <see cref="StockPoint.Open" /> value.  See the public property
    /// <see cref="RisingFill" /> to access this value.
    /// </summary>
    private Fill _risingFill;

    /// <summary>
    /// Private field to store the <see cref="Fill" /> class to be used for filling the
    /// candlestick "bars" when the <see cref="StockPoint.Close" /> value is less than
    /// the <see cref="StockPoint.Open" /> value.  See the public property
    /// <see cref="FallingFill" /> to access this value.
    /// </summary>
    private Fill _fallingFill;

    /// <summary>
    /// Private field to store the <see cref="Border" /> class to be used for drawing the
    /// candlestick "bars" when the <see cref="StockPoint.Close" /> value is greater than
    /// the <see cref="StockPoint.Open" /> value.  See the public property
    /// <see cref="RisingBorder" /> to access this value.
    /// </summary>
    private Border _risingBorder;

    /// <summary>
    /// Private field to store the <see cref="Border" /> class to be used for drawing the
    /// candlestick "bars" when the <see cref="StockPoint.Close" /> value is less than
    /// the <see cref="StockPoint.Open" /> value.  See the public property
    /// <see cref="FallingBorder" /> to access this value.
    /// </summary>
    private Border _fallingBorder;

    /// <summary>
    /// Private field that stores the CandleStick color when the <see cref="StockPoint.Close" />
    /// value is less than the <see cref="StockPoint.Open" /> value.  Use the public
    /// property <see cref="FallingColor"/> to access this value.
    /// </summary>
    protected Color _fallingColor;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="JapaneseCandleStick"/> class.
    /// </summary>
    public new struct Default
    {
        /// <summary>
        /// The default fillcolor for drawing the rising case CandleSticks
        /// (<see cref="JapaneseCandleStick.RisingFill"/> property).
        /// </summary>
        public static Color RisingColor = Color.White;

        /// <summary>
        /// The default fillcolor for drawing the falling case CandleSticks
        /// (<see cref="JapaneseCandleStick.FallingFill"/> property).
        /// </summary>
        public static Color FallingColor = Color.Black;

        /// <summary>
        /// The default color for the border of the rising CandleSticks
        /// (<see cref="JapaneseCandleStick.RisingBorder" /> property).
        /// </summary>
        public static Color RisingBorder = Color.Black;

        /// <summary>
        /// The default color for the border of the falling CandleSticks
        /// (<see cref="JapaneseCandleStick.FallingBorder" /> property).
        /// </summary>
        public static Color FallingBorder = Color.Black;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="Fill" /> class that is used to fill the candlestick
    /// "bars" when the <see cref="StockPoint.Close" /> value is greater than the
    /// <see cref="StockPoint.Open" /> value.
    /// </summary>
    public Fill RisingFill
    {
        get => _risingFill;
        set => _risingFill = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="Fill" /> class that is used to fill the candlestick
    /// "bars" when the <see cref="StockPoint.Close" /> value is less than the
    /// <see cref="StockPoint.Open" /> value.
    /// </summary>
    public Fill FallingFill
    {
        get => _fallingFill;
        set => _fallingFill = value;
    }

    /// <summary>
    /// The <see cref="Border" /> instance to be used for drawing the border frame of
    /// the candlestick "bars" when the <see cref="StockPoint.Close" /> value is greater than the
    /// <see cref="StockPoint.Open" /> value.
    /// </summary>
    public Border RisingBorder
    {
        get => _risingBorder;
        set => _risingBorder = value;
    }

    /// <summary>
    /// The <see cref="Border" /> instance to be used for drawing the border frame of
    /// the candlestick "bars" when the <see cref="StockPoint.Close" /> value is less than the
    /// <see cref="StockPoint.Open" /> value.
    /// </summary>
    public Border FallingBorder
    {
        get => _fallingBorder;
        set => _fallingBorder = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="System.Drawing.Color"/> data for this
    /// <see cref="JapaneseCandleStick"/> when the value of the candlestick is
    /// falling.
    /// </summary>
    /// <remarks>This property only controls the color of
    /// the vertical line when the value is falling.  The rising color is controlled
    /// by the <see cref="LineBase.Color" /> property.
    /// </remarks>
    public Color FallingColor
    {
        get => _fallingColor;
        set => _fallingColor = value;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor that sets all <see cref="JapaneseCandleStick"/> properties to
    /// default values as defined in the <see cref="Default"/> class.
    /// </summary>
    public JapaneseCandleStick() : base()
    {
        _risingFill = new Fill (Default.RisingColor);
        _fallingFill = new Fill (Default.FallingColor);

        _risingBorder = new Border (Default.RisingBorder, LineBase.Default.Width);
        _fallingBorder = new Border (Default.FallingBorder, LineBase.Default.Width);

        _fallingColor = Default.FallingColor;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="JapaneseCandleStick"/> object from which to copy</param>
    public JapaneseCandleStick (JapaneseCandleStick rhs) : base (rhs)
    {
        _risingFill = rhs._risingFill.Clone();
        _fallingFill = rhs._fallingFill.Clone();

        _risingBorder = rhs._risingBorder.Clone();
        _fallingBorder = rhs._fallingBorder.Clone();

        _fallingColor = rhs._fallingColor;
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
    public new JapaneseCandleStick Clone()
    {
        return new JapaneseCandleStick (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema2 = 11;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected JapaneseCandleStick
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        int sch = info.GetInt32 ("schema2");

        _risingFill = (Fill)info.GetValue ("risingFill", typeof (Fill));
        _fallingFill = (Fill)info.GetValue ("fallingFill", typeof (Fill));
        _risingBorder = (Border)info.GetValue ("risingBorder", typeof (Border));
        _fallingBorder = (Border)info.GetValue ("fallingBorder", typeof (Border));

        if (schema2 >= 11)
        {
            _fallingColor = (Color)info.GetValue ("fallingColor", typeof (Color));
        }
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
        info.AddValue ("risingFill", _risingFill);
        info.AddValue ("fallingFill", _fallingFill);
        info.AddValue ("risingBorder", _risingBorder);
        info.AddValue ("fallingBorder", _fallingBorder);
        info.AddValue ("fallingColor", _fallingColor);
    }

    #endregion

    #region Rendering Methods

    /// <summary>
    /// Draw the <see cref="JapaneseCandleStick"/> to the specified <see cref="Graphics"/>
    /// device at the specified location.
    /// </summary>
    /// <param name="g">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="isXBase">boolean value that indicates if the "base" axis for this
    /// <see cref="JapaneseCandleStick"/> is the X axis.  True for an <see cref="XAxis"/> base,
    /// false for a <see cref="YAxis"/> or <see cref="Y2Axis"/> base.</param>
    /// <param name="pixBase">The independent axis position of the center of the candlestick in
    /// pixel units</param>
    /// <param name="pixHigh">The high value position of the candlestick in
    /// pixel units</param>
    /// <param name="pixLow">The low value position of the candlestick in
    /// pixel units</param>
    /// <param name="pixOpen">The opening value position of the candlestick in
    /// pixel units</param>
    /// <param name="pixClose">The closing value position of the candlestick in
    /// pixel units</param>
    /// <param name="halfSize">The scaled width of one-half of a bar, in pixels</param>
    /// <param name="scaleFactor">
    /// The scaling factor for the features of the graph based on the <see cref="PaneBase.BaseDimension"/>.  This
    /// scaling factor is calculated by the <see cref="PaneBase.CalcScaleFactor"/> method.  The scale factor
    /// represents a linear multiple to be applied to font sizes, symbol sizes, etc.</param>
    /// <param name="pen">A pen with the <see cref="Color"/> attribute for this
    /// <see cref="JapaneseCandleStick"/></param>
    /// <param name="fill">
    /// The <see cref="Fill" /> instance to be used for filling this
    /// <see cref="JapaneseCandleStick" />
    /// </param>
    /// <param name="border">The <see cref="Border" /> instance to be used for drawing the
    /// border around the <see cref="JapaneseCandleStick" /> filled box</param>
    /// <param name="pt">The <see cref="PointPair" /> to be used for determining the
    /// <see cref="Fill" />, just in case it's a <see cref="FillType.GradientByX" />,
    /// <see cref="FillType.GradientByY" />, or
    /// <see cref="FillType.GradientByZ" /> <see cref="FillType" /></param>
    public void Draw (Graphics g, GraphPane pane, bool isXBase,
        float pixBase, float pixHigh, float pixLow,
        float pixOpen, float pixClose, float halfSize,
        float scaleFactor, Pen pen, Fill fill, Border border, PointPair pt)
    {
        //float halfSize = (int) ( _size * scaleFactor / 2.0f + 0.5f );

        if (pixBase != PointPairBase.Missing && Math.Abs (pixLow) < 1000000 &&
            Math.Abs (pixHigh) < 1000000)
        {
            RectangleF rect;
            if (isXBase)
            {
                rect = new RectangleF (pixBase - halfSize, Math.Min (pixOpen, pixClose),
                    halfSize * 2.0f, Math.Abs (pixOpen - pixClose));

                g.DrawLine (pen, pixBase, pixHigh, pixBase, pixLow);
            }
            else
            {
                rect = new RectangleF (Math.Min (pixOpen, pixClose), pixBase - halfSize,
                    Math.Abs (pixOpen - pixClose), halfSize * 2.0f);

                g.DrawLine (pen, pixHigh, pixBase, pixLow, pixBase);
            }

            if (_isOpenCloseVisible && Math.Abs (pixOpen) < 1000000 &&
                Math.Abs (pixClose) < 1000000)
            {
                if (rect.Width == 0)
                {
                    rect.Width = 1;
                }

                if (rect.Height == 0)
                {
                    rect.Height = 1;
                }

                fill.Draw (g, rect, pt);
                border.Draw (g, pane, scaleFactor, rect);
            }
        }
    }


    /// <summary>
    /// Draw all the <see cref="JapaneseCandleStick"/>'s to the specified <see cref="Graphics"/>
    /// device as a candlestick at each defined point.
    /// </summary>
    /// <param name="g">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="curve">A <see cref="JapaneseCandleStickItem"/> object representing the
    /// <see cref="JapaneseCandleStick"/>'s to be drawn.</param>
    /// <param name="baseAxis">The <see cref="Axis"/> class instance that defines the base (independent)
    /// axis for the <see cref="JapaneseCandleStick"/></param>
    /// <param name="valueAxis">The <see cref="Axis"/> class instance that defines the value (dependent)
    /// axis for the <see cref="JapaneseCandleStick"/></param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    public void Draw (Graphics g, GraphPane pane, JapaneseCandleStickItem curve,
        Axis baseAxis, Axis valueAxis, float scaleFactor)
    {
        //ValueHandler valueHandler = new ValueHandler( pane, false );

        float pixBase, pixHigh, pixLow, pixOpen, pixClose;

        if (curve.Points != null)
        {
            //float halfSize = _size * scaleFactor;
            float halfSize = GetBarWidth (pane, baseAxis, scaleFactor);

            Color tColor = _color;
            Color tFallingColor = _fallingColor;
            float tPenWidth = _width;
            Fill tRisingFill = _risingFill;
            Fill tFallingFill = _fallingFill;
            Border tRisingBorder = _risingBorder;
            Border tFallingBorder = _fallingBorder;
            if (curve.IsSelected)
            {
                tColor = Selection.Border.Color;
                tFallingColor = Selection.Border.Color;
                tPenWidth = Selection.Border.Width;
                tRisingFill = Selection.Fill;
                tFallingFill = Selection.Fill;
                tRisingBorder = Selection.Border;
                tFallingBorder = Selection.Border;
            }

            using (Pen risingPen = new Pen (tColor, tPenWidth))
            using (Pen fallingPen = new Pen (tFallingColor, tPenWidth))
            {
                // Loop over each defined point
                for (int i = 0; i < curve.Points.Count; i++)
                {
                    PointPair pt = curve.Points[i];
                    double date = pt.X;
                    double high = pt.Y;
                    double low = pt.Z;
                    double open = PointPairBase.Missing;
                    double close = PointPairBase.Missing;
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
                        ((high > 0 && low > 0) || !valueAxis.Scale.IsLog))
                    {
                        pixBase = (int)(baseAxis.Scale.Transform (curve.IsOverrideOrdinal, i, date) + 0.5);

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
                            using (Pen tPen = GetPen (pane, scaleFactor, pt))
                                Draw (g, pane, baseAxis is XAxis || baseAxis is X2Axis,
                                    pixBase, pixHigh, pixLow, pixOpen,
                                    pixClose, halfSize, scaleFactor,
                                    (tPen),
                                    (close > open ? tRisingFill : tFallingFill),
                                    (close > open ? tRisingBorder : tFallingBorder), pt);
                        }
                        else
                        {
                            Draw (g, pane, baseAxis is XAxis || baseAxis is X2Axis,
                                pixBase, pixHigh, pixLow, pixOpen,
                                pixClose, halfSize, scaleFactor,
                                (close > open ? risingPen : fallingPen),
                                (close > open ? tRisingFill : tFallingFill),
                                (close > open ? tRisingBorder : tFallingBorder), pt);
                        }
                    }
                }
            }
        }
    }

    #endregion
}
