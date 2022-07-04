// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CoreGraphicsPath.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Represents a graphics path that uses the same notation as GDI+.
    /// </summary>
    internal class CoreGraphicsPath
    {
        // Same values as GDI+ uses.
        const byte PathPointTypeStart = 0;  // move
        const byte PathPointTypeLine = 1;  // line
        const byte PathPointTypeBezier = 3;  // default Bezier (= cubic Bezier)
        const byte PathPointTypePathTypeMask = 0x07;  // type mask (lowest 3 bits).
        const byte PathPointTypeCloseSubpath = 0x80;  // closed flag

        public CoreGraphicsPath()
        { }

        public CoreGraphicsPath(CoreGraphicsPath path)
        {
            _points = new List<XPoint>(path._points);
            _types = new List<byte>(path._types);
        }

        public void MoveOrLineTo(double x, double y)
        {
            // Make a MoveTo if there is no previous subpath or the previous subpath was closed.
            // Otherwise make a LineTo.
            if (_types.Count == 0 || (_types[_types.Count - 1] & PathPointTypeCloseSubpath) == PathPointTypeCloseSubpath)
                MoveTo(x, y);
            else
                LineTo(x, y, false);
        }

        public void MoveTo(double x, double y)
        {
            _points.Add(new XPoint(x, y));
            _types.Add(PathPointTypeStart);
        }

        public void LineTo(double x, double y, bool closeSubpath)
        {
            if (_points.Count > 0 && _points[_points.Count - 1].Equals(new XPoint(x, y)))
                return;

            _points.Add(new XPoint(x, y));
            _types.Add((byte)(PathPointTypeLine | (closeSubpath ? PathPointTypeCloseSubpath : 0)));
        }

        public void BezierTo(double x1, double y1, double x2, double y2, double x3, double y3, bool closeSubpath)
        {
            _points.Add(new XPoint(x1, y1));
            _types.Add(PathPointTypeBezier);
            _points.Add(new XPoint(x2, y2));
            _types.Add(PathPointTypeBezier);
            _points.Add(new XPoint(x3, y3));
            _types.Add((byte)(PathPointTypeBezier | (closeSubpath ? PathPointTypeCloseSubpath : 0)));
        }

        /// <summary>
        /// Adds an arc that fills exactly one quadrant (quarter) of an ellipse.
        /// Just a quick hack to draw rounded rectangles before AddArc is fully implemented.
        /// </summary>
        public void QuadrantArcTo(double x, double y, double width, double height, int quadrant, bool clockwise)
        {
            if (width < 0)
                throw new ArgumentOutOfRangeException("width");
            if (height < 0)
                throw new ArgumentOutOfRangeException("height");

            double w = Const.κ * width;
            double h = Const.κ * height;
            double x1, y1, x2, y2, x3, y3;
            switch (quadrant)
            {
                case 1:
                    if (clockwise)
                    {
                        x1 = x + w;
                        y1 = y - height;
                        x2 = x + width;
                        y2 = y - h;
                        x3 = x + width;
                        y3 = y;
                    }
                    else
                    {
                        x1 = x + width;
                        y1 = y - h;
                        x2 = x + w;
                        y2 = y - height;
                        x3 = x;
                        y3 = y - height;
                    }
                    break;

                case 2:
                    if (clockwise)
                    {
                        x1 = x - width;
                        y1 = y - h;
                        x2 = x - w;
                        y2 = y - height;
                        x3 = x;
                        y3 = y - height;
                    }
                    else
                    {
                        x1 = x - w;
                        y1 = y - height;
                        x2 = x - width;
                        y2 = y - h;
                        x3 = x - width;
                        y3 = y;
                    }
                    break;

                case 3:
                    if (clockwise)
                    {
                        x1 = x - w;
                        y1 = y + height;
                        x2 = x - width;
                        y2 = y + h;
                        x3 = x - width;
                        y3 = y;
                    }
                    else
                    {
                        x1 = x - width;
                        y1 = y + h;
                        x2 = x - w;
                        y2 = y + height;
                        x3 = x;
                        y3 = y + height;
                    }
                    break;

                case 4:
                    if (clockwise)
                    {
                        x1 = x + width;
                        y1 = y + h;
                        x2 = x + w;
                        y2 = y + height;
                        x3 = x;
                        y3 = y + height;
                    }
                    else
                    {
                        x1 = x + w;
                        y1 = y + height;
                        x2 = x + width;
                        y2 = y + h;
                        x3 = x + width;
                        y3 = y;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("quadrant");
            }
            BezierTo(x1, y1, x2, y2, x3, y3, false);
        }

        /// <summary>
        /// Closes the current subpath.
        /// </summary>
        public void CloseSubpath()
        {
            int count = _types.Count;
            if (count > 0)
                _types[count - 1] |= PathPointTypeCloseSubpath;
        }

        /// <summary>
        /// Gets or sets the current fill mode (alternate or winding).
        /// </summary>
        XFillMode FillMode
        {
            get { return _fillMode; }
            set { _fillMode = value; }
        }
        XFillMode _fillMode;

        public void AddArc(double x, double y, double width, double height, double startAngle, double sweepAngle)
        {
            XMatrix matrix = XMatrix.Identity;
            List<XPoint> points = GeometryHelper.BezierCurveFromArc(x, y, width, height, startAngle, sweepAngle, PathStart.MoveTo1st, ref matrix);
            int count = points.Count;
            Debug.Assert((count + 2) % 3 == 0);

            MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
                BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y, points[idx + 2].X, points[idx + 2].Y, false);
        }

        public void AddArc(XPoint point1, XPoint point2, XSize size, double rotationAngle, bool isLargeArg, XSweepDirection sweepDirection)
        {
            List<XPoint> points = GeometryHelper.BezierCurveFromArc(point1, point2, size, rotationAngle, isLargeArg,
                sweepDirection == XSweepDirection.Clockwise, PathStart.MoveTo1st);
            int count = points.Count;
            Debug.Assert((count + 2) % 3 == 0);

            MoveOrLineTo(points[0].X, points[0].Y);
            for (int idx = 1; idx < count; idx += 3)
                BezierTo(points[idx].X, points[idx].Y, points[idx + 1].X, points[idx + 1].Y, points[idx + 2].X, points[idx + 2].Y, false);
        }

        public void AddCurve(XPoint[] points, double tension)
        {
            int count = points.Length;
            if (count < 2)
                throw new ArgumentException("AddCurve requires two or more points.", "points");

            tension /= 3;
            MoveOrLineTo(points[0].X, points[0].Y);
            if (count == 2)
            {
                //figure.Segments.Add(GeometryHelper.CreateCurveSegment(points[0], points[0], points[1], points[1], tension));
                ToCurveSegment(points[0], points[0], points[1], points[1], tension);
            }
            else
            {
                //figure.Segments.Add(GeometryHelper.CreateCurveSegment(points[0], points[0], points[1], points[2], tension));
                ToCurveSegment(points[0], points[0], points[1], points[2], tension);
                for (int idx = 1; idx < count - 2; idx++)
                {
                    //figure.Segments.Add(GeometryHelper.CreateCurveSegment(points[idx - 1], points[idx], points[idx + 1], points[idx + 2], tension));
                    ToCurveSegment(points[idx - 1], points[idx], points[idx + 1], points[idx + 2], tension);
                }
                //figure.Segments.Add(GeometryHelper.CreateCurveSegment(points[count - 3], points[count - 2], points[count - 1], points[count - 1], tension));
                ToCurveSegment(points[count - 3], points[count - 2], points[count - 1], points[count - 1], tension);
            }
        }

        ///// <summary>
        ///// Appends a Bézier curve for a cardinal spline through pt1 and pt2.
        ///// </summary>
        //void ToCurveSegment(double x0, double y0, double x1, double y1, double x2, double y2, double x3, double y3, double tension3, bool closeSubpath)
        //{
        //    BezierTo(
        //        x1 + tension3 * (x2 - x0), y1 + tension3 * (y2 - y0),
        //        x2 - tension3 * (x3 - x1), y2 - tension3 * (y3 - y1),
        //        x2, y2, closeSubpath);
        //}

        void ToCurveSegment(XPoint pt0, XPoint pt1, XPoint pt2, XPoint pt3, double tension3)
        {
            BezierTo(
                pt1.X + tension3 * (pt2.X - pt0.X), pt1.Y + tension3 * (pt2.Y - pt0.Y),
                pt2.X - tension3 * (pt3.X - pt1.X), pt2.Y - tension3 * (pt3.Y - pt1.Y),
                pt2.X, pt2.Y,
                false);
        }

        /// <summary>
        /// Gets the path points in GDI+ style.
        /// </summary>
        public XPoint[] PathPoints { get { return _points.ToArray(); } }

        /// <summary>
        /// Gets the path types in GDI+ style.
        /// </summary>
        public byte[] PathTypes { get { return _types.ToArray(); } }

        readonly List<XPoint> _points = new List<XPoint>();
        readonly List<byte> _types = new List<byte>();
    }
}
