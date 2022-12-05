// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable InconsistentNaming

/* ValueHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A class designed to simplify the process of getting the actual value for
/// the various stacked and regular curve types
/// </summary>
public class ValueHandler
{
    private GraphPane _pane;

    /// <summary>
    /// Basic constructor that saves a reference to the parent
    /// <see cref="GraphPane"/> object.
    /// </summary>
    /// <param name="pane">The parent <see cref="GraphPane"/> object.</param>
    /// <param name="initialize">A <see cref="bool"/> flag to indicate whether or
    /// not the drawing variables should be initialized.  Initialization is not
    /// required if this is part of a ZedGraph internal draw operation (i.e., its in
    /// the middle of a call to <see cref="GraphPane.Draw"/>).  Otherwise, you should
    /// initialize to make sure the drawing variables are configured.  true to do
    /// an initialization, false otherwise.</param>
    public ValueHandler (GraphPane pane, bool initialize)
    {
        _pane = pane;
        if (initialize)
        {
            // just create a dummy image, which results in a full draw operation
            using (Image image = pane.GetImage())
            {
            }
        }
    }

    /// <summary>
    /// Get the user scale values associate with a particular point of a
    /// particular curve.</summary>
    /// <remarks>The main purpose of this method is to handle
    /// stacked bars, in which case the stacked values are returned rather
    /// than the individual data values.
    /// </remarks>
    /// <param name="curve">A <see cref="CurveItem"/> object of interest.</param>
    /// <param name="iPt">The zero-based point index for the point of interest.</param>
    /// <param name="baseVal">A <see cref="Double"/> value representing the value
    /// for the independent axis.</param>
    /// <param name="lowVal">A <see cref="Double"/> value representing the lower
    /// value for the dependent axis.</param>
    /// <param name="hiVal">A <see cref="Double"/> value representing the upper
    /// value for the dependent axis.</param>
    /// <returns>true if the data point is value, false for
    /// <see cref="PointPairBase.Missing"/>, invalid, etc. data.</returns>
    public bool GetValues (CurveItem curve, int iPt, out double baseVal,
        out double lowVal, out double hiVal)
    {
        return GetValues (_pane, curve, iPt, out baseVal,
            out lowVal, out hiVal);
    }

    /// <summary>
    /// Get the user scale values associate with a particular point of a
    /// particular curve.</summary>
    /// <remarks>The main purpose of this method is to handle
    /// stacked bars and lines, in which case the stacked values are returned rather
    /// than the individual data values.  However, this method works generically for any
    /// curve type.
    /// </remarks>
    /// <param name="pane">The parent <see cref="GraphPane"/> object.</param>
    /// <param name="curve">A <see cref="CurveItem"/> object of interest.</param>
    /// <param name="iPt">The zero-based point index for the point of interest.</param>
    /// <param name="baseVal">A <see cref="Double"/> value representing the value
    /// for the independent axis.</param>
    /// <param name="lowVal">A <see cref="Double"/> value representing the lower
    /// value for the dependent axis.</param>
    /// <param name="hiVal">A <see cref="Double"/> value representing the upper
    /// value for the dependent axis.</param>
    /// <returns>true if the data point is value, false for
    /// <see cref="PointPairBase.Missing"/>, invalid, etc. data.</returns>
    public static bool GetValues
        (
            GraphPane pane,
            CurveItem? curve,
            int iPt,
            out double baseVal,
            out double lowVal,
            out double hiVal
        )
    {
        hiVal = PointPairBase.Missing;
        lowVal = PointPairBase.Missing;
        baseVal = PointPairBase.Missing;

        if (curve?.Points is null || curve.Points.Count <= iPt || !curve.IsVisible)
        {
            return false;
        }

        var baseAxis = curve.BaseAxis (pane);
        var valueAxis = curve.ValueAxis (pane);

        if (baseAxis is XAxis or X2Axis)
        {
            baseVal = curve.Points[iPt].X;
        }
        else
        {
            baseVal = curve.Points[iPt].Y;
        }

        // is it a stacked bar type?
        if (curve is BarItem && pane._barSettings.Type is BarType.Stack or BarType.PercentStack)
        {
            double positiveStack = 0;
            double negativeStack = 0;
            double curVal;

            // loop through all the curves, summing up the values to get a total (only
            // for the current ordinal position iPt)
            foreach (var tmpCurve in pane.CurveList)
            {
                // Sum the value for the current curve only if it is a bar
                if (tmpCurve is { IsBar: true, IsVisible: true })
                {
                    curVal = PointPairBase.Missing;

                    // For non-ordinal curves, find a matching base value (must match exactly)
                    if (curve.IsOverrideOrdinal || !baseAxis.Scale!.IsAnyOrdinal)
                    {
                        var points = tmpCurve.Points;

                        for (var i = 0; i < points!.Count; i++)
                        {
                            if (baseAxis is XAxis or X2Axis && points[i].X == baseVal)
                            {
                                curVal = points[i].Y;
                                break;
                            }
                            else if (!(baseAxis is XAxis or X2Axis) && points[i].Y == baseVal)
                            {
                                curVal = points[i].X;
                                break;
                            }
                        }
                    }

                    // otherwise, it's an ordinal type so use the value at the same ordinal position
                    else if (iPt < tmpCurve.Points!.Count)
                    {
                        // Get the value for the appropriate value axis
                        if (baseAxis is XAxis or X2Axis)
                        {
                            curVal = tmpCurve.Points[iPt].Y;
                        }
                        else
                        {
                            curVal = tmpCurve.Points[iPt].X;
                        }
                    }

                    // If it's a missing value, skip it
                    if (curVal == PointPairBase.Missing)
                    {
                        positiveStack = PointPairBase.Missing;
                        negativeStack = PointPairBase.Missing;
                    }

                    // the current curve is the target curve, save the summed values for later
                    if (tmpCurve == curve)
                    {
                        // if the value is positive, use the positive stack
                        if (curVal >= 0)
                        {
                            lowVal = positiveStack;
                            hiVal = (curVal == PointPairBase.Missing || positiveStack == PointPairBase.Missing)
                                ? PointPairBase.Missing
                                : positiveStack + curVal;
                        }

                        // otherwise, use the negative stack
                        else
                        {
                            hiVal = negativeStack;
                            lowVal = (curVal == PointPairBase.Missing || negativeStack == PointPairBase.Missing)
                                ? PointPairBase.Missing
                                : negativeStack + curVal;
                        }
                    }

                    // Add all positive values to the positive stack, and negative values to the
                    // negative stack
                    if (curVal >= 0)
                    {
                        positiveStack = (curVal == PointPairBase.Missing || positiveStack == PointPairBase.Missing)
                            ? PointPairBase.Missing
                            : positiveStack + curVal;
                    }
                    else
                    {
                        negativeStack = (curVal == PointPairBase.Missing || negativeStack == PointPairBase.Missing)
                            ? PointPairBase.Missing
                            : negativeStack + curVal;
                    }
                }
            }

            // if the curve is a PercentStack type, then calculate the percent for this bar
            // based on the total height of the stack
            if (pane._barSettings.Type == BarType.PercentStack &&
                hiVal != PointPairBase.Missing && lowVal != PointPairBase.Missing)
            {
                // Use the total magnitude of the positive plus negative bar stacks to determine
                // the percentage value
                positiveStack += Math.Abs (negativeStack);

                // just to avoid dividing by zero...
                if (positiveStack != 0)
                {
                    // calculate the percentage values
                    lowVal = lowVal / positiveStack * 100.0;
                    hiVal = hiVal / positiveStack * 100.0;
                }
                else
                {
                    lowVal = 0;
                    hiVal = 0;
                }
            }

            if (baseVal == PointPairBase.Missing || lowVal == PointPairBase.Missing ||
                hiVal == PointPairBase.Missing)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // If the curve is a stacked line type, then sum up the values similar to the stacked bar type
        else if (curve is LineItem && pane.LineType == LineType.Stack)
        {
            double stack = 0;
            double curVal;

            // loop through all the curves, summing up the values to get a total (only
            // for the current ordinal position iPt)
            foreach (var tmpCurve in pane.CurveList)
            {
                // make sure the curve is a Line type
                if (tmpCurve is LineItem && tmpCurve.IsVisible)
                {
                    curVal = PointPairBase.Missing;

                    // For non-ordinal curves, find a matching base value (must match exactly)
                    if (curve.IsOverrideOrdinal || !baseAxis.Scale!.IsAnyOrdinal)
                    {
                        var points = tmpCurve.Points;

                        for (var i = 0; i < points!.Count; i++)
                        {
                            if (points[i].X == baseVal)
                            {
                                curVal = points[i].Y;
                                break;
                            }
                        }
                    }

                    // otherwise, it's an ordinal type so use the value at the same ordinal position
                    else if (iPt < tmpCurve.Points!.Count)
                    {
                        // For line types, the Y axis is always the value axis
                        curVal = tmpCurve.Points[iPt].Y;
                    }

                    // if the current value is missing, then the rest of the stack is missing
                    if (curVal == PointPairBase.Missing)
                    {
                        stack = PointPairBase.Missing;
                    }

                    // if the current curve is the target curve, save the values
                    if (tmpCurve == curve)
                    {
                        lowVal = stack;

//							if ( curVal < 0 && stack == 0 )
//							{
//								stack = curVal;
//								lowVal = curVal;
//								hiVal = curVal;
//							}
//							else
                        hiVal = (curVal == PointPairBase.Missing || stack == PointPairBase.Missing)
                            ? PointPairBase.Missing
                            : stack + curVal;
                    }

                    // sum all the curves to a single total.  This includes both positive and
                    // negative values (unlike the bar stack type).
                    stack = (curVal == PointPairBase.Missing || stack == PointPairBase.Missing)
                        ? PointPairBase.Missing
                        : stack + curVal;
                }
            }

            if (baseVal == PointPairBase.Missing || lowVal == PointPairBase.Missing ||
                hiVal == PointPairBase.Missing)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // otherwise, the curve is not a stacked type (not a stacked bar or stacked line)
        else
        {
            if ((!(curve is HiLowBarItem)) && (!(curve is ErrorBarItem)))
            {
                lowVal = 0;
            }
            else
            {
                lowVal = curve.Points[iPt].LowValue;
            }

            if (baseAxis is XAxis or X2Axis)
            {
                hiVal = curve.Points[iPt].Y;
            }
            else
            {
                hiVal = curve.Points[iPt].X;
            }
        }

        // Special Exception: Bars on log scales should always plot from the Min value upwards,
        // since they can never be zero
        if (curve is BarItem && valueAxis.Scale!.IsLog && lowVal == 0)
        {
            lowVal = valueAxis.Scale._min;
        }

        if (baseVal == PointPairBase.Missing || hiVal == PointPairBase.Missing ||
            (lowVal == PointPairBase.Missing && curve is ErrorBarItem or HiLowBarItem))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Calculate the user scale position of the center of the specified bar, using the
    /// <see cref="Axis"/> as specified by <see cref="BarSettings.Base"/>.  This method is
    /// used primarily by the
    /// <see cref="GraphPane.FindNearestPoint(PointF,out CurveItem,out int)"/> method in order to
    /// determine the bar "location," which is defined as the center of the top of the individual bar.
    /// </summary>
    /// <param name="curve">The <see cref="CurveItem"/> representing the
    /// bar of interest.</param>
    /// <param name="barWidth">The width of each individual bar. This can be calculated using
    /// the <see cref="CurveItem.GetBarWidth"/> method.</param>
    /// <param name="iCluster">The cluster number for the bar of interest.  This is the ordinal
    /// position of the current point.  That is, if a particular <see cref="CurveItem"/> has
    /// 10 points, then a value of 3 would indicate the 4th point in the data array.</param>
    /// <param name="val">The actual independent axis value for the bar of interest.</param>
    /// <param name="iOrdinal">The ordinal position of the <see cref="CurveItem"/> of interest.
    /// That is, the first bar series is 0, the second is 1, etc.  Note that this applies only
    /// to the bars.  If a graph includes both bars and lines, then count only the bars.</param>
    /// <returns>A user scale value position of the center of the bar of interest.</returns>
    public double BarCenterValue (CurveItem curve, float barWidth, int iCluster,
        double val, int iOrdinal)
    {
        var baseAxis = curve.BaseAxis (_pane);
        if (curve is ErrorBarItem or HiLowBarItem or OHLCBarItem or JapaneseCandleStickItem)
        {
            if (baseAxis.Scale!.IsAnyOrdinal && iCluster >= 0 && !curve.IsOverrideOrdinal)
            {
                return (double)iCluster + 1.0;
            }
            else
            {
                return val;
            }
        }
        else
        {
            var clusterWidth = _pane._barSettings.GetClusterWidth();
            var clusterGap = _pane._barSettings.MinClusterGap * barWidth;
            var barGap = barWidth * _pane._barSettings.MinBarGap;

            if (curve.IsBar && _pane._barSettings.Type != BarType.Cluster)
            {
                iOrdinal = 0;
            }

            var centerPix = baseAxis.Scale!.Transform (curve.IsOverrideOrdinal, iCluster, val)
                            - clusterWidth / 2.0F + clusterGap / 2.0F +
                            iOrdinal * (barWidth + barGap) + 0.5F * barWidth;
            return baseAxis.Scale.ReverseTransform (centerPix);
        }
    }
}
