// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* GeometryHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Collections.Generic;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Helper class for Geometry paths.
/// </summary>
static class GeometryHelper
{
    /// <summary>
    /// Creates between 1 and 5 Béziers curves from parameters specified like in GDI+.
    /// </summary>
    public static List<XPoint> BezierCurveFromArc
        (
            double x,
            double y,
            double width,
            double height,
            double startAngle,
            double sweepAngle,
            PathStart pathStart,
            ref XMatrix matrix
        )
    {
        var points = new List<XPoint>();

        // Normalize the angles.
        var alpha = startAngle;
        if (alpha < 0)
        {
            alpha += (1 + Math.Floor ((Math.Abs (alpha) / 360))) * 360;
        }
        else if (alpha > 360)
        {
            alpha -= Math.Floor (alpha / 360) * 360;
        }

        Debug.Assert (alpha is >= 0 and <= 360);

        var beta = sweepAngle;
        if (beta < -360)
        {
            beta = -360;
        }
        else if (beta > 360)
        {
            beta = 360;
        }

        if (alpha == 0 && beta < 0)
        {
            alpha = 360;
        }
        else if (alpha == 360 && beta > 0)
        {
            alpha = 0;
        }

        // Is it possible that the arc is small starts and ends in same quadrant?
        var smallAngle = Math.Abs (beta) <= 90;

        beta = alpha + beta;
        if (beta < 0)
        {
            beta += (1 + Math.Floor ((Math.Abs (beta) / 360))) * 360;
        }

        var clockwise = sweepAngle > 0;
        var startQuadrant = Quadrant (alpha, true, clockwise);
        var endQuadrant = Quadrant (beta, false, clockwise);

        if (startQuadrant == endQuadrant && smallAngle)
        {
            AppendPartialArcQuadrant (points, x, y, width, height, alpha, beta, pathStart, matrix);
        }
        else
        {
            var currentQuadrant = startQuadrant;
            var firstLoop = true;
            do
            {
                if (currentQuadrant == startQuadrant && firstLoop)
                {
                    double ξ = currentQuadrant * 90 + (clockwise ? 90 : 0);
                    AppendPartialArcQuadrant (points, x, y, width, height, alpha, ξ, pathStart, matrix);
                }
                else if (currentQuadrant == endQuadrant)
                {
                    double ξ = currentQuadrant * 90 + (clockwise ? 0 : 90);
                    AppendPartialArcQuadrant (points, x, y, width, height, ξ, beta, PathStart.Ignore1st, matrix);
                }
                else
                {
                    double ξ1 = currentQuadrant * 90 + (clockwise ? 0 : 90);
                    double ξ2 = currentQuadrant * 90 + (clockwise ? 90 : 0);
                    AppendPartialArcQuadrant (points, x, y, width, height, ξ1, ξ2, PathStart.Ignore1st, matrix);
                }

                // Don't stop immediately if arc is greater than 270 degrees.
                if (currentQuadrant == endQuadrant && smallAngle)
                {
                    break;
                }

                smallAngle = true;

                if (clockwise)
                {
                    currentQuadrant = currentQuadrant == 3 ? 0 : currentQuadrant + 1;
                }
                else
                {
                    currentQuadrant = currentQuadrant == 0 ? 3 : currentQuadrant - 1;
                }

                firstLoop = false;
            } while (true);
        }

        return points;
    }

    /// <summary>
    /// Calculates the quadrant (0 through 3) of the specified angle. If the angle lies on an edge
    /// (0, 90, 180, etc.) the result depends on the details how the angle is used.
    /// </summary>
    static int Quadrant (double φ, bool start, bool clockwise)
    {
        Debug.Assert (φ >= 0);
        if (φ > 360)
        {
            φ -= Math.Floor (φ / 360) * 360;
        }

        var quadrant = (int)(φ / 90);
        if (quadrant * 90 == φ)
        {
            if ((start && !clockwise) || (!start && clockwise))
            {
                quadrant = quadrant == 0 ? 3 : quadrant - 1;
            }
        }
        else
        {
            quadrant = clockwise ? ((int)Math.Floor (φ / 90)) % 4 : (int)Math.Floor (φ / 90);
        }

        return quadrant;
    }

    /// <summary>
    /// Appends a Bézier curve for an arc within a full quadrant.
    /// </summary>
    static void AppendPartialArcQuadrant (List<XPoint> points, double x, double y, double width, double height,
        double alpha, double beta, PathStart pathStart, XMatrix matrix)
    {
        Debug.Assert (alpha is >= 0 and <= 360);
        Debug.Assert (beta >= 0);
        if (beta > 360)
        {
            beta -= Math.Floor (beta / 360) * 360;
        }

        Debug.Assert (Math.Abs (alpha - beta) <= 90);

        // Scanling factor.
        var δx = width / 2;
        var δy = height / 2;

        // Center of ellipse.
        var x0 = x + δx;
        var y0 = y + δy;

        // We have the following quarters:
        //     |
        //   2 | 3
        // ----+-----
        //   1 | 0
        //     |
        // If the angles lie in quarter 2 or 3, their values are subtracted by 180 and the
        // resulting curve is reflected at the center. This algorithm works as expected (simply tried out).
        // There may be a mathematically more elegant solution...
        var reflect = false;
        if (alpha >= 180 && beta >= 180)
        {
            alpha -= 180;
            beta -= 180;
            reflect = true;
        }

        double sinAlpha, sinBeta;
        if (width == height)
        {
            // Circular arc needs no correction.
            alpha *= Calc.Deg2Rad;
            beta *= Calc.Deg2Rad;
        }
        else
        {
            // Elliptic arc needs the angles to be adjusted such that the scaling transformation is compensated.
            alpha *= Calc.Deg2Rad;
            sinAlpha = Math.Sin (alpha);
            if (Math.Abs (sinAlpha) > 1E-10)
            {
                alpha = Math.PI / 2 - Math.Atan (δy * Math.Cos (alpha) / (δx * sinAlpha));
            }

            beta *= Calc.Deg2Rad;
            sinBeta = Math.Sin (beta);
            if (Math.Abs (sinBeta) > 1E-10)
            {
                beta = Math.PI / 2 - Math.Atan (δy * Math.Cos (beta) / (δx * sinBeta));
            }
        }

        var κ = 4 * (1 - Math.Cos ((alpha - beta) / 2)) / (3 * Math.Sin ((beta - alpha) / 2));
        sinAlpha = Math.Sin (alpha);
        var cosAlpha = Math.Cos (alpha);
        sinBeta = Math.Sin (beta);
        var cosBeta = Math.Cos (beta);

        //XPoint pt1, pt2, pt3;
        if (!reflect)
        {
            // Calculation for quarter 0 and 1.
            switch (pathStart)
            {
                case PathStart.MoveTo1st:
                    points.Add (matrix.Transform (new XPoint (x0 + δx * cosAlpha, y0 + δy * sinAlpha)));
                    break;

                case PathStart.LineTo1st:
                    points.Add (matrix.Transform (new XPoint (x0 + δx * cosAlpha, y0 + δy * sinAlpha)));
                    break;

                case PathStart.Ignore1st:
                    break;
            }

            points.Add (matrix.Transform (new XPoint (x0 + δx * (cosAlpha - κ * sinAlpha), y0 + δy * (sinAlpha + κ * cosAlpha))));
            points.Add (matrix.Transform (new XPoint (x0 + δx * (cosBeta + κ * sinBeta), y0 + δy * (sinBeta - κ * cosBeta))));
            points.Add (matrix.Transform (new XPoint (x0 + δx * cosBeta, y0 + δy * sinBeta)));
        }
        else
        {
            // Calculation for quarter 2 and 3.
            switch (pathStart)
            {
                case PathStart.MoveTo1st:
                    points.Add (matrix.Transform (new XPoint (x0 - δx * cosAlpha, y0 - δy * sinAlpha)));
                    break;

                case PathStart.LineTo1st:
                    points.Add (matrix.Transform (new XPoint (x0 - δx * cosAlpha, y0 - δy * sinAlpha)));
                    break;

                case PathStart.Ignore1st:
                    break;
            }

            points.Add (matrix.Transform (new XPoint (x0 - δx * (cosAlpha - κ * sinAlpha), y0 - δy * (sinAlpha + κ * cosAlpha))));
            points.Add (matrix.Transform (new XPoint (x0 - δx * (cosBeta + κ * sinBeta), y0 - δy * (sinBeta - κ * cosBeta))));
            points.Add (matrix.Transform (new XPoint (x0 - δx * cosBeta, y0 - δy * sinBeta)));
        }
    }

    /// <summary>
    /// Creates between 1 and 5 Béziers curves from parameters specified like in WPF.
    /// </summary>
    public static List<XPoint> BezierCurveFromArc (XPoint point1, XPoint point2, XSize size,
        double rotationAngle, bool isLargeArc, bool clockwise, PathStart pathStart)
    {
        // See also http://www.charlespetzold.com/blog/blog.xml from January 2, 2008:
        // http://www.charlespetzold.com/blog/2008/01/Mathematics-of-ArcSegment.html
        var δx = size.Width;
        var δy = size.Height;
        Debug.Assert (δx * δy > 0);
        var factor = δy / δx;
        var isCounterclockwise = !clockwise;

        // Adjust for different radii and rotation angle.
        var matrix = new XMatrix();
        matrix.RotateAppend (-rotationAngle);
        matrix.ScaleAppend (δy / δx, 1);
        var pt1 = matrix.Transform (point1);
        var pt2 = matrix.Transform (point2);

        // Get info about chord that connects both points.
        var midPoint = new XPoint ((pt1.X + pt2.X) / 2, (pt1.Y + pt2.Y) / 2);
        var vect = pt2 - pt1;
        var halfChord = vect.Length / 2;

        // Get vector from chord to center.
        var vectRotated =
            // (comparing two Booleans here!)
            isLargeArc == isCounterclockwise
            ? new XVector (-vect.Y, vect.X)
            : new XVector (vect.Y, -vect.X);

        vectRotated.Normalize();

        // Distance from chord to center.
        var centerDistance = Math.Sqrt (δy * δy - halfChord * halfChord);
        if (double.IsNaN (centerDistance))
        {
            centerDistance = 0;
        }

        // Calculate center point.
        var center = midPoint + centerDistance * vectRotated;

        // Get angles from center to the two points.
        var α = Math.Atan2 (pt1.Y - center.Y, pt1.X - center.X);
        var β = Math.Atan2 (pt2.Y - center.Y, pt2.X - center.X);

        // (another comparison of two Booleans!)
        if (isLargeArc == (Math.Abs (β - α) < Math.PI))
        {
            if (α < β)
            {
                α += 2 * Math.PI;
            }
            else
            {
                β += 2 * Math.PI;
            }
        }

        // Invert matrix for final point calculation.
        matrix.Invert();
        var sweepAngle = β - α;

        // Let the algorithm of GDI+ DrawArc to Bézier curves do the rest of the job
        return BezierCurveFromArc (center.X - δx * factor, center.Y - δy, 2 * δx * factor, 2 * δy,
            α / Calc.Deg2Rad, sweepAngle / Calc.Deg2Rad, pathStart, ref matrix);
    }
}
