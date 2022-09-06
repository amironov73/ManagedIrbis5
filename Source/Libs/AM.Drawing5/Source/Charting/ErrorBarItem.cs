// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ErrorBarItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Encapsulates an "Error Bar" curve type that displays a vertical or horizontal
/// line with a symbol at each end.
/// </summary>
/// <remarks>The <see cref="ErrorBarItem"/> type is intended for displaying
/// confidence intervals, candlesticks, stock High-Low charts, etc.  It is
/// technically not a bar, since it is drawn as a vertical or horizontal line.
/// The default symbol at each end of the "bar" is <see cref="SymbolType.HDash"/>,
/// which creates an "I-Beam".  For horizontal bars
/// (<see cref="BarBase.Y"/> or
/// <see cref="BarBase.Y2"/>), you will need to change the symbol to
/// <see cref="SymbolType.VDash"/> to get horizontal "I-Beams".
/// Since the horizontal segments are actually symbols, their widths are
/// controlled by the symbol size in <see cref="ErrorBar.Symbol"/>,
/// specified in points (1/72nd inch).  The position of each "I-Beam" is set
/// according to the <see cref="PointPair"/> values.  The independent axis
/// is assigned with <see cref="BarSettings.Base"/>, and is a
/// <see cref="BarBase"/> enum type.</remarks>
[Serializable]
public class ErrorBarItem
    : CurveItem, ICloneable
{
    #region Fields

    #endregion

    #region Properties

    /// <summary>
    /// Gets a reference to the <see cref="ErrorBar"/> class defined
    /// for this <see cref="ErrorBarItem"/>.
    /// </summary>
    public ErrorBar Bar { get; }

    /// <inheritdoc cref="CurveItem.IsZIncluded"/>
    internal override bool IsZIncluded
        (
            GraphPane pane
        )
    {
        return true;
    }

    /// <inheritdoc cref="CurveItem.IsXIndependent"/>
    internal override bool IsXIndependent
        (
            GraphPane pane
        )
    {
        return pane._barSettings.Base == BarBase.X;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new <see cref="ErrorBarItem"/>, specifying only the legend label.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    public ErrorBarItem
        (
            string label
        )
        : base (label)
    {
        Bar = new ErrorBar();
    }

    /// <summary>
    /// Create a new <see cref="ErrorBarItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="x">An array of double precision values that define
    /// the X axis values for this curve</param>
    /// <param name="y">An array of double precision values that define
    /// the Y axis values for this curve</param>
    /// <param name="lowValue">An array of double precision values that define
    /// the lower dependent values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> properties.
    /// </param>
    public ErrorBarItem
        (
            string label,
            double[] x,
            double[] y,
            double[] lowValue,
            Color color
        )
        : this (label, new PointPairList (x, y, lowValue), color)
    {
    }

    /// <summary>
    /// Create a new <see cref="ErrorBarItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="points">A <see cref="IPointList"/> of double precision values that define
    /// the X, Y and lower dependent values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> properties.
    /// </param>
    public ErrorBarItem
        (
            string label,
            IPointList points,
            Color color
        )
        : base (label, points)
    {
        Bar = new ErrorBar (color);
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="ErrorBarItem"/> object from which to copy</param>
    public ErrorBarItem
        (
            ErrorBarItem rhs
        )
        : base (rhs)
    {
        Bar = new ErrorBar (rhs.Bar);
    }

    /// <inheritdoc cref="ICloneable.Clone"/>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public ErrorBarItem Clone()
    {
        return new ErrorBarItem (this);
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
    protected ErrorBarItem
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        Bar = (ErrorBar) info.GetValue ("bar", typeof (ErrorBar)).ThrowIfNull();

        // This is now just a dummy variable, since barBase was removed
        info.GetValue ("barBase", typeof (BarBase)).NotUsed ();
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

        // BarBase is now just a dummy value, since the GraphPane.BarBase is used exclusively
        info.AddValue ("barBase", BarBase.X);
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
        if (IsVisible)
        {
            Bar.Draw
                (
                    graphics,
                    pane,
                    this,
                    BaseAxis (pane),
                    ValueAxis (pane),
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
        float pixBase, pixValue, pixLowValue;

        if (pane._barSettings.Base == BarBase.X)
        {
            pixBase = rect.Left + rect.Width / 2.0F;
            pixValue = rect.Top;
            pixLowValue = rect.Bottom;
        }
        else
        {
            pixBase = rect.Top + rect.Height / 2.0F;
            pixValue = rect.Right;
            pixLowValue = rect.Left;
        }

        using var pen = new Pen (Bar.Color, Bar.PenWidth);
        Bar.Draw
            (
                graphics,
                pane,
                pane._barSettings.Base == BarBase.X,
                pixBase,
                pixValue,
                pixLowValue,
                scaleFactor,
                pen,
                false,
                null
            );
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

        if (i < 0 || Points is null || i >= Points.Count)
        {
            return false;
        }

        var valueAxis = ValueAxis (pane);
        var baseAxis = BaseAxis (pane);

        var scaledSize = Bar.Symbol.Size * pane.CalcScaleFactor();

        // pixBase = pixel value for the bar center on the base axis
        // pixHiVal = pixel value for the bar top on the value axis
        // pixLowVal = pixel value for the bar bottom on the value axis

        var clusterWidth = pane.BarSettings.GetClusterWidth();
        var barWidth = GetBarWidth (pane);
        var clusterGap = pane._barSettings.MinClusterGap * barWidth;
        var barGap = barWidth * pane._barSettings.MinBarGap;

        // curBase = the scale value on the base axis of the current bar
        // curHiVal = the scale value on the value axis of the current bar
        // curLowVal = the scale value of the bottom of the bar
        var valueHandler = new ValueHandler (pane, false);
        valueHandler.GetValues (this, i, out var curBase, out var curLowVal, out var curHiVal);

        // Any value set to double max is invalid and should be skipped
        // This is used for calculated values that are out of range, divide
        // by zero, etc.
        // Also, any value <= zero on a log scale is invalid

        if (!Points[i].IsInvalid3D)
        {
            // calculate a pixel value for the top of the bar on value axis
            var pixLowVal = valueAxis.Scale.Transform (IsOverrideOrdinal, i, curLowVal);
            var pixHiVal = valueAxis.Scale.Transform (IsOverrideOrdinal, i, curHiVal);

            // calculate a pixel value for the center of the bar on the base axis
            var pixBase = baseAxis.Scale.Transform (IsOverrideOrdinal, i, curBase);

            // Calculate the pixel location for the side of the bar (on the base axis)
            var pixSide = pixBase - scaledSize / 2.0F;

            // Draw the bar
            if (baseAxis is XAxis or X2Axis)
            {
                coords = string.Create
                    (
                        CultureInfo.InvariantCulture,
                        $"{pixSide:f0},{pixLowVal:f0},{pixSide + scaledSize:f0},{pixHiVal:f0}"
                    );
            }
            else
            {
                coords = string.Create
                    (
                        CultureInfo.InvariantCulture,
                        $"{pixLowVal:f0},{pixSide:f0},{pixHiVal:f0},{pixSide + scaledSize:f0}"
                    );
            }

            return true;
        }

        return false;
    }

    #endregion
}
