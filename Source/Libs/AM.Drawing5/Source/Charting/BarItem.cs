// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* BarItem.cs --
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
/// Encapsulates a bar type that displays vertical or horizontal bars
/// </summary>
/// <remarks>
/// The orientation of the bars depends on the state of
/// <see cref="BarSettings.Base"/>, and the bars can be stacked or
/// clustered, depending on the state of <see cref="BarSettings.Type"/>
/// </remarks>
[Serializable]
public class BarItem
    : CurveItem, ICloneable, ISerializable
{
    #region Fields

    #endregion

    #region Properties

    /// <summary>
    /// Gets a reference to the <see cref="Charting.Bar"/> class defined
    /// for this <see cref="BarItem"/>.
    /// </summary>
    public Bar? Bar { get; protected set; }

    /// <summary>
    /// Gets a flag indicating if the Z data range should be included in the axis scaling calculations.
    /// </summary>
    /// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
    /// </param>
    /// <value>true if the Z data are included, false otherwise</value>
    internal override bool IsZIncluded
        (
            GraphPane pane
        )
    {
        return this is HiLowBarItem;
    }

    /// <summary>
    /// Gets a flag indicating if the X axis is the independent axis for this <see cref="CurveItem" />
    /// </summary>
    /// <param name="pane">The parent <see cref="GraphPane" /> of this <see cref="CurveItem" />.
    /// </param>
    /// <value>true if the X axis is independent, false otherwise</value>
    internal override bool IsXIndependent (GraphPane pane)
    {
        return pane._barSettings.Base == BarBase.X || pane._barSettings.Base == BarBase.X2;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new <see cref="BarItem"/>, specifying only the legend label for the bar.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    public BarItem
        (
            string label
        )
        : base (label)
    {
        Bar = new Bar();
    }

    /// <summary>
    /// Create a new <see cref="BarItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="x">An array of double precision values that define
    /// the independent (X axis) values for this curve</param>
    /// <param name="y">An array of double precision values that define
    /// the dependent (Y axis) values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Charting.Bar.Fill"/> and <see cref="Charting.Bar.Border"/> properties.
    /// </param>
    public BarItem
        (
            string label,
            double[] x,
            double[] y,
            Color color
        )
        : this (label, new PointPairList (x, y), color)
    {
    }

    /// <summary>
    /// Create a new <see cref="BarItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
    /// the X and Y values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Charting.Bar.Fill"/> and <see cref="Charting.Bar.Border"/> properties.
    /// </param>
    public BarItem
        (
            string label,
            IPointList points,
            Color color
        )
        : base (label, points)
    {
        Bar = new Bar (color);
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="BarItem"/> object from which to copy</param>
    public BarItem
        (
            BarItem rhs
        )
        : base (rhs)
    {
        //bar = new Bar( rhs.Bar );
        Bar = rhs.Bar.Clone();
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
    public BarItem Clone()
    {
        return new BarItem (this);
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
    protected BarItem
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        Bar = (Bar?) info.GetValue ("bar", typeof (Bar));
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
        info.AddValue ("bar", Bar);
    }

    #endregion

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
        // Pass the drawing onto the bar class
        if (_isVisible)
        {
            Bar?.DrawBars
                (
                    graphics,
                    pane,
                    this,
                    BaseAxis (pane),
                    ValueAxis (pane),
                    GetBarWidth (pane),
                    pos,
                    scaleFactor
                );
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
        Bar?.Draw (graphics, pane, rect, scaleFactor, true, false, null);
    }

    /// <summary>
    /// Create a <see cref="TextObj" /> for each bar in the <see cref="GraphPane" />.
    /// </summary>
    /// <remarks>
    /// This method will go through the bars, create a label that corresponds to the bar value,
    /// and place it on the graph depending on user preferences.  This works for horizontal or
    /// vertical bars in clusters or stacks, but only for <see cref="BarItem" /> types.  This method
    /// does not apply to <see cref="ErrorBarItem" /> or <see cref="HiLowBarItem" /> objects.
    /// Call this method only after calling <see cref="GraphPane.AxisChange()" />.
    /// </remarks>
    /// <param name="pane">The GraphPane in which to place the text labels.</param>
    /// <param name="isBarCenter">true to center the labels inside the bars, false to
    /// place the labels just above the top of the bar.</param>
    /// <param name="valueFormat">The double.ToString string format to use for creating
    /// the labels.
    /// </param>
    public static void CreateBarLabels
        (
            GraphPane pane,
            bool isBarCenter,
            string valueFormat
        )
    {
        CreateBarLabels
            (
                pane,
                isBarCenter,
                valueFormat,
                TextObj.Default.FontFamily,
                TextObj.Default.FontSize,
                TextObj.Default.FontColor,
                TextObj.Default.FontBold,
                TextObj.Default.FontItalic,
                TextObj.Default.FontUnderline
            );
    }

    /// <summary>
    /// Create a <see cref="TextObj" /> for each bar in the <see cref="GraphPane" />.
    /// </summary>
    /// <remarks>
    /// This method will go through the bars, create a label that corresponds to the bar value,
    /// and place it on the graph depending on user preferences.  This works for horizontal or
    /// vertical bars in clusters or stacks, but only for <see cref="BarItem" /> types.  This method
    /// does not apply to <see cref="ErrorBarItem" /> or <see cref="HiLowBarItem" /> objects.
    /// Call this method only after calling <see cref="GraphPane.AxisChange()" />.
    /// </remarks>
    /// <param name="pane">The GraphPane in which to place the text labels.</param>
    /// <param name="isBarCenter">true to center the labels inside the bars, false to
    /// place the labels just above the top of the bar.</param>
    /// <param name="valueFormat">The double.ToString string format to use for creating
    /// the labels.
    /// </param>
    /// <param name="fontColor">The color in which to draw the labels</param>
    /// <param name="fontFamily">The string name of the font family to use for the labels</param>
    /// <param name="fontSize">The floating point size of the font, in scaled points</param>
    /// <param name="isBold">true for a bold font type, false otherwise</param>
    /// <param name="isItalic">true for an italic font type, false otherwise</param>
    /// <param name="isUnderline">true for an underline font type, false otherwise</param>
    public static void CreateBarLabels
        (
            GraphPane pane,
            bool isBarCenter,
            string valueFormat,
            string fontFamily,
            float fontSize,
            Color fontColor,
            bool isBold,
            bool isItalic,
            bool isUnderline
        )
    {
        var isVertical = pane.BarSettings.Base == BarBase.X;

        // keep a count of the number of BarItems
        var curveIndex = 0;

        // Get a valuehandler to do some calculations for us
        var valueHandler = new ValueHandler (pane, true);

        // Loop through each curve in the list
        foreach (var curve in pane.CurveList)
        {
            // work with BarItems only
            if (curve is BarItem bar)
            {
                var points = curve.Points;

                // ADD JKB 9/21/07
                // The labelOffset should depend on whether the curve is YAxis or Y2Axis.
                // JHC - Generalize to any value axis
                // Make the gap between the bars and the labels = 1.5% of the axis range

                var scale = curve.ValueAxis (pane).Scale;
                var labelOffset = (float)(scale._max - scale._min) * 0.015f;

                // Loop through each point in the BarItem
                for (var i = 0; i < points.Count; i++)
                {
                    // Get the high, low and base values for the current bar
                    // note that this method will automatically calculate the "effective"
                    // values if the bar is stacked
                    valueHandler.GetValues (curve, i, out var baseVal, out var lowVal, out var hiVal);

                    // Get the value that corresponds to the center of the bar base
                    // This method figures out how the bars are positioned within a cluster
                    var centerVal = (float)valueHandler.BarCenterValue (bar,
                        bar.GetBarWidth (pane), i, baseVal, curveIndex);

                    // Create a text label -- note that we have to go back to the original point
                    // data for this, since hiVal and lowVal could be "effective" values from a bar stack
                    var barLabelText = (isVertical ? points[i].Y : points[i].X).ToString (valueFormat);

                    // Calculate the position of the label -- this is either the X or the Y coordinate
                    // depending on whether they are horizontal or vertical bars, respectively
                    float position;
                    if (isBarCenter)
                    {
                        position = (float)(hiVal + lowVal) / 2.0f;
                    }
                    else if (hiVal >= 0)
                    {
                        position = (float)hiVal + labelOffset;
                    }
                    else
                    {
                        position = (float)hiVal - labelOffset;
                    }

                    // Create the new TextObj
                    TextObj label;
                    label = isVertical
                        ? new TextObj (barLabelText, centerVal, position)
                        : new TextObj (barLabelText, position, centerVal);

                    label.FontSpec.Family = fontFamily;

                    // Configure the TextObj

                    // CHANGE JKB 9/21/07
                    // CoordinateFrame should depend on whether curve is YAxis or Y2Axis.
                    label.Location.CoordinateFrame =
                        (isVertical && curve.IsY2Axis) ? CoordType.AxisXY2Scale : CoordType.AxisXYScale;

                    label.FontSpec.Size = fontSize;
                    label.FontSpec.FontColor = fontColor;
                    label.FontSpec.IsItalic = isItalic;
                    label.FontSpec.IsBold = isBold;
                    label.FontSpec.IsUnderline = isUnderline;

                    label.FontSpec.Angle = isVertical ? 90 : 0;
                    label.Location.AlignH = isBarCenter ? AlignH.Center : (hiVal >= 0 ? AlignH.Left : AlignH.Right);
                    label.Location.AlignV = AlignV.Center;
                    label.FontSpec.Border.IsVisible = false;
                    label.FontSpec.Fill.IsVisible = false;

                    // Add the TextObj to the GraphPane
                    pane.GraphObjList.Add (label);
                }

                curveIndex++;
            }
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

        if (i < 0 || i >= _points.Count)
        {
            return false;
        }

        var valueAxis = ValueAxis (pane);
        var baseAxis = BaseAxis (pane);

        // pixBase = pixel value for the bar center on the base axis
        // pixHiVal = pixel value for the bar top on the value axis
        // pixLowVal = pixel value for the bar bottom on the value axis
        float pixBase, pixHiVal, pixLowVal;

        var clusterWidth = pane.BarSettings.GetClusterWidth();
        var barWidth = GetBarWidth (pane);
        var clusterGap = pane._barSettings.MinClusterGap * barWidth;
        var barGap = barWidth * pane._barSettings.MinBarGap;

        // curBase = the scale value on the base axis of the current bar
        // curHiVal = the scale value on the value axis of the current bar
        // curLowVal = the scale value of the bottom of the bar
        double curBase, curLowVal, curHiVal;
        var valueHandler = new ValueHandler (pane, false);
        valueHandler.GetValues (this, i, out curBase, out curLowVal, out curHiVal);

        // Any value set to double max is invalid and should be skipped
        // This is used for calculated values that are out of range, divide
        //   by zero, etc.
        // Also, any value <= zero on a log scale is invalid

        if (!_points[i].IsInvalid3D)
        {
            // calculate a pixel value for the top of the bar on value axis
            pixLowVal = valueAxis.Scale.Transform (_isOverrideOrdinal, i, curLowVal);
            pixHiVal = valueAxis.Scale.Transform (_isOverrideOrdinal, i, curHiVal);

            // calculate a pixel value for the center of the bar on the base axis
            pixBase = baseAxis.Scale.Transform (_isOverrideOrdinal, i, curBase);

            // Calculate the pixel location for the side of the bar (on the base axis)
            var pixSide = pixBase - clusterWidth / 2.0F + clusterGap / 2.0F +
                          pane.CurveList.GetBarItemPos (pane, this) * (barWidth + barGap);

            // Draw the bar
            if (baseAxis is XAxis or X2Axis)
            {
                coords = $"{pixSide:f0},{pixLowVal:f0},{pixSide + barWidth:f0},{pixHiVal:f0}";
            }
            else
            {
                coords = $"{pixLowVal:f0},{pixSide:f0},{pixHiVal:f0},{pixSide + barWidth:f0}";
            }

            return true;
        }

        return false;
    }

    #endregion
}
