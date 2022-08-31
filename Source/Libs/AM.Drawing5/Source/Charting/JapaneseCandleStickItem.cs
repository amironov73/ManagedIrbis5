// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* JapaneseCandleStickItem.cs --
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
/// Encapsulates a Japanese CandleStick curve type that displays a vertical (or horizontal)
/// line displaying the range of data values at each sample point, plus a filled bar
/// signifying the opening and closing value for the sample.
/// </summary>
/// <remarks>For this type to work properly, your <see cref="IPointList" /> must contain
/// <see cref="StockPoint" /> objects, rather than ordinary <see cref="PointPair" /> types.
/// This is because the <see cref="OHLCBarItem"/> type actually displays 5 data values
/// but the <see cref="PointPair" /> only stores 3 data values.  The <see cref="StockPoint" />
/// stores <see cref="StockPoint.Date" />, <see cref="StockPoint.Close" />,
/// <see cref="StockPoint.Open" />, <see cref="StockPoint.High" />, and
/// <see cref="StockPoint.Low" /> members.
/// For a JapaneseCandleStick chart, the range between opening and closing values
/// is drawn as a filled bar, with the filled color different
/// (<see cref="JapaneseCandleStick.RisingFill" />) for the case of
/// <see cref="StockPoint.Close" />
/// higher than <see cref="StockPoint.Open" />, and
/// <see cref="JapaneseCandleStick.FallingFill" />
/// for the reverse.  The width of the bar is controlled
/// by the <see cref="OHLCBar.Size" /> property, which is specified in
/// points (1/72nd inch), and scaled according to <see cref="PaneBase.CalcScaleFactor" />.
/// The candlesticks are drawn horizontally or vertically depending on the
/// value of <see cref="BarSettings.Base"/>, which is a
/// <see cref="BarBase"/> enum type.</remarks>
[Serializable]
public class JapaneseCandleStickItem
    : CurveItem, ICloneable
{
    #region Fields

    #endregion

    #region Properties

    /// <summary>
    /// Gets a reference to the <see cref="JapaneseCandleStick"/> class defined
    /// for this <see cref="JapaneseCandleStickItem"/>.
    /// </summary>
    public JapaneseCandleStick Stick { get; }

    /// <summary>
    /// Gets a flag indicating if the X axis is the independent axis for this <see cref="CurveItem" />
    /// </summary>
    /// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
    /// </param>
    /// <value>true if the X axis is independent, false otherwise</value>
    internal override bool IsXIndependent (GraphPane pane)
    {
        return pane._barSettings.Base == BarBase.X;
    }

    /// <summary>
    /// Gets a flag indicating if the Z data range should be included in the axis scaling calculations.
    /// </summary>
    /// <remarks>
    /// IsZIncluded is true for <see cref="JapaneseCandleStickItem" /> objects, since the Y and Z
    /// values are defined as the High and Low values for the day.</remarks>
    /// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
    /// </param>
    /// <value>true if the Z data are included, false otherwise</value>
    internal override bool IsZIncluded (GraphPane pane)
    {
        return true;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new <see cref="OHLCBarItem"/>, specifying only the legend label.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    public JapaneseCandleStickItem (string label)
        : base (label)
    {
        Stick = new JapaneseCandleStick();
    }

    /// <summary>
    /// Create a new <see cref="JapaneseCandleStickItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="points">An <see cref="IPointList"/> of double precision values that define
    /// the Date, Close, Open, High, and Low values for the curve.  Note that this
    /// <see cref="IPointList" /> should contain <see cref="StockPoint" /> items rather
    /// than <see cref="PointPair" /> items.
    /// </param>
    public JapaneseCandleStickItem (string label, IPointList points)
        : base (label, points)
    {
        Stick = new JapaneseCandleStick();
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="JapaneseCandleStickItem"/> object from which to copy</param>
    public JapaneseCandleStickItem (JapaneseCandleStickItem rhs)
        : base (rhs)
    {
        Stick = rhs.Stick.Clone();
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
    public JapaneseCandleStickItem Clone()
    {
        return new JapaneseCandleStickItem (this);
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
    protected JapaneseCandleStickItem
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        Stick = (JapaneseCandleStick)info.GetValue ("stick",
            typeof (JapaneseCandleStick));
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
        info.AddValue ("stick", Stick);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Do all rendering associated with this <see cref="OHLCBarItem"/> to the specified
    /// <see cref="Graphics"/> device.  This method is normally only
    /// called by the Draw method of the parent <see cref="CurveList"/>
    /// collection object.
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="pos">The ordinal position of the current <see cref="OHLCBarItem"/>
    /// curve.</param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    public override void Draw (Graphics graphics, GraphPane pane, int pos, float scaleFactor)
    {
        if (_isVisible)
        {
            Stick.Draw (graphics, pane, this, BaseAxis (pane),
                ValueAxis (pane), scaleFactor);
        }
    }

    /// <summary>
    /// Draw a legend key entry for this <see cref="OHLCBarItem"/> at the specified location
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="rect">The <see cref="RectangleF"/> struct that specifies the
    /// location for the legend key</param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    public override void DrawLegendKey (Graphics graphics, GraphPane pane, RectangleF rect,
        float scaleFactor)
    {
        float pixBase, pixHigh, pixLow, pixOpen, pixClose;

        if (pane._barSettings.Base == BarBase.X)
        {
            pixBase = rect.Left + rect.Width / 2.0F;
            pixHigh = rect.Top;
            pixLow = rect.Bottom;
            pixOpen = pixHigh + rect.Height / 3;
            pixClose = pixLow - rect.Height / 3;
        }
        else
        {
            pixBase = rect.Top + rect.Height / 2.0F;
            pixHigh = rect.Right;
            pixLow = rect.Left;
            pixOpen = pixHigh - rect.Width / 3;
            pixClose = pixLow + rect.Width / 3;
        }

        Axis baseAxis = BaseAxis (pane);

        //float halfSize = _stick.GetBarWidth( pane, baseAxis, scaleFactor );
        float halfSize = 2 * scaleFactor;

        using (Pen pen = new Pen (Stick.Color, Stick._width))
        {
            Stick.Draw (graphics, pane, pane._barSettings.Base == BarBase.X, pixBase, pixHigh,
                pixLow, pixOpen, pixClose, halfSize, scaleFactor, pen,
                Stick.RisingFill,
                Stick.RisingBorder, null);
        }
    }

    /// <summary>
    /// Determine the coords for the rectangle associated with a specified point for
    /// this <see cref="CurveItem" />
    /// </summary>
    /// <param name="pane">The <see cref="GraphPane" /> to which this curve belongs</param>
    /// <param name="i">The index of the point of interest</param>
    /// <param name="coords">A list of coordinates that represents the "rect" for
    /// this point (used in an html AREA tag)</param>
    /// <returns>true if it's a valid point, false otherwise</returns>
    public override bool GetCoords (GraphPane pane, int i, out string coords)
    {
        coords = string.Empty;

        if (i < 0 || i >= _points.Count)
        {
            return false;
        }

        Axis valueAxis = ValueAxis (pane);
        Axis baseAxis = BaseAxis (pane);

        float halfSize = Stick.Size * pane.CalcScaleFactor();

        PointPair pt = _points[i];
        double date = pt.X;
        double high = pt.Y;
        double low = pt.Z;

        if (!pt.IsInvalid3D &&
            (date > 0 || !baseAxis.Scale.IsLog) &&
            ((high > 0 && low > 0) || !valueAxis.Scale.IsLog))
        {
            float pixBase, pixHigh, pixLow;
            pixBase = baseAxis.Scale.Transform (_isOverrideOrdinal, i, date);
            pixHigh = valueAxis.Scale.Transform (_isOverrideOrdinal, i, high);
            pixLow = valueAxis.Scale.Transform (_isOverrideOrdinal, i, low);

            // Calculate the pixel location for the side of the bar (on the base axis)
            float pixSide = pixBase - halfSize;

            // Draw the bar
            if (baseAxis is XAxis || baseAxis is X2Axis)
            {
                coords = string.Format ("{0:f0},{1:f0},{2:f0},{3:f0}",
                    pixSide, pixLow,
                    pixSide + halfSize * 2, pixHigh);
            }
            else
            {
                coords = string.Format ("{0:f0},{1:f0},{2:f0},{3:f0}",
                    pixLow, pixSide,
                    pixHigh, pixSide + halfSize * 2);
            }

            return true;
        }

        return false;
    }

    #endregion
}
