// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* LineObj.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A class that represents a line segment object on the graph.  A list of
/// GraphObj objects is maintained by the <see cref="GraphObjList"/> collection class.
/// </summary>
/// <remarks>
/// This should not be confused with the <see cref="LineItem" /> class, which represents
/// a set of points plotted together as a "curve".  The <see cref="LineObj" /> class is
/// a single line segment, drawn as a "decoration" on the chart.</remarks>
[Serializable]
public class LineObj
    : GraphObj, ICloneable
{
    #region Fields

    #endregion

    #region Properties

    /// <summary>
    /// A <see cref="LineBase" /> class that contains the attributes for drawing this
    /// <see cref="LineObj" />.
    /// </summary>
    public LineBase Line { get; set; }

    #endregion


    #region Constructors

    /// <overloads>Constructors for the <see cref="LineObj"/> object</overloads>
    /// <summary>
    /// A constructor that allows the position, color, and size of the
    /// <see cref="LineObj"/> to be pre-specified.
    /// </summary>
    /// <param name="color">An arbitrary <see cref="System.Drawing.Color"/> specification
    /// for the arrow</param>
    /// <param name="x1">The x position of the starting point that defines the
    /// line.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y1">The y position of the starting point that defines the
    /// line.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="x2">The x position of the ending point that defines the
    /// line.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y2">The y position of the ending point that defines the
    /// line.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    public LineObj (Color color, double x1, double y1, double x2, double y2)
        : base (x1, y1, x2 - x1, y2 - y1)
    {
        Line = new LineBase (color);
        Location.AlignH = AlignH.Left;
        Location.AlignV = AlignV.Top;
    }

    /// <summary>
    /// A constructor that allows only the position of the
    /// line to be pre-specified.  All other properties are set to
    /// default values
    /// </summary>
    /// <param name="x1">The x position of the starting point that defines the
    /// <see cref="LineObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y1">The y position of the starting point that defines the
    /// <see cref="LineObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="x2">The x position of the ending point that defines the
    /// <see cref="LineObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y2">The y position of the ending point that defines the
    /// <see cref="LineObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    public LineObj (double x1, double y1, double x2, double y2)
        : this (LineBase.Default.Color, x1, y1, x2, y2)
    {
    }

    /// <summary>
    /// Default constructor -- places the <see cref="LineObj"/> at location
    /// (0,0) to (1,1).  All other values are defaulted.
    /// </summary>
    public LineObj() : this (LineBase.Default.Color, 0, 0, 1, 1)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="LineObj"/> object from which to copy</param>
    public LineObj (LineObj rhs) : base (rhs)
    {
        Line = new LineBase (rhs.Line);
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
    public LineObj Clone()
    {
        return new LineObj (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>

    // changed to 2 with addition of Style property
    public const int schema2 = 11;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected LineObj
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        Line = (LineBase) info.GetValue ("line", typeof (LineBase))!;
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
        info.AddValue ("line", Line);
    }

    #endregion

    #region Rendering Methods

    /// <inheritdoc cref="GraphObj.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            PaneBase pane,
            float scaleFactor
        )
    {
        // Convert the arrow coordinates from the user coordinate system
        // to the screen coordinate system
        var pix1 = Location.TransformTopLeft (pane);
        var pix2 = Location.TransformBottomRight (pane);

        if (pix1.X > -10000 && pix1 is { X: < 100000, Y: > -100000 and < 100000 } &&
            pix2.X > -10000 && pix2 is { X: < 100000, Y: > -100000 and < 100000 })
        {
            // calculate the length and the angle of the arrow "vector"
            double dy = pix2.Y - pix1.Y;
            double dx = pix2.X - pix1.X;
            var angle = (float)Math.Atan2 (dy, dx) * 180.0F / (float)Math.PI;
            var length = (float)Math.Sqrt (dx * dx + dy * dy);

            // Save the old transform matrix
            var transform = graphics.Transform;

            // Move the coordinate system so it is located at the starting point
            // of this arrow
            graphics.TranslateTransform (pix1.X, pix1.Y);

            // Rotate the coordinate system according to the angle of this arrow
            // about the starting point
            graphics.RotateTransform (angle);

            // get a pen according to this arrow properties
            using (var pen = Line.GetPen (pane, scaleFactor))

                //new Pen( _line._color, pane.ScaledPenWidth( _line._width, scaleFactor ) ) )
            {
                //pen.DashStyle = _style;

                graphics.DrawLine (pen, 0, 0, length, 0);
            }

            // Restore the transform matrix back to its original state
            graphics.Transform = transform;
        }
    }

    /// <inheritdoc cref="GraphObj.PointInBox"/>
    public override bool PointInBox
        (
            PointF point,
            PaneBase pane,
            Graphics graphics,
            float scaleFactor
        )
    {
        if (!base.PointInBox (point, pane, graphics, scaleFactor))
        {
            return false;
        }

        // transform the x,y location from the user-defined
        // coordinate frame to the screen pixel location
        var pix = _location.TransformTopLeft (pane);
        var pix2 = _location.TransformBottomRight (pane);

        using var pen = new Pen (Color.Black, (float)GraphPane.Default.NearestTol * 2.0F);
        using var path = new GraphicsPath();
        path.AddLine (pix, pix2);

        return path.IsOutlineVisible (point, pen);
    }

    /// <inheritdoc cref="GraphObj.GetCoords"/>
    public override void GetCoords
        (
            PaneBase pane,
            Graphics graphics,
            float scaleFactor,
            out string shape,
            out string coords
        )
    {
        // transform the x,y location from the user-defined
        // coordinate frame to the screen pixel location
        var pixRect = _location.TransformRect (pane);

        var matrix = new Matrix();
        if (pixRect.Right == 0)
        {
            pixRect.Width = 1;
        }

        var angle = (float)Math.Atan ((pixRect.Top - pixRect.Bottom) /
                                      (pixRect.Left - pixRect.Right));
        matrix.Rotate (angle, MatrixOrder.Prepend);

        // Move the coordinate system to local coordinates
        // of this text object (that is, at the specified
        // x,y location)
        matrix.Translate (-pixRect.Left, -pixRect.Top, MatrixOrder.Prepend);

        var pts = new PointF[4];
        pts[0] = new PointF (0, 3);
        pts[1] = new PointF (pixRect.Width, 3);
        pts[2] = new PointF (pixRect.Width, -3);
        pts[3] = new PointF (0, -3);
        matrix.TransformPoints (pts);

        shape = "poly";
        coords = string.Format ("{0:f0},{1:f0},{2:f0},{3:f0},{4:f0},{5:f0},{6:f0},{7:f0},",
            pts[0].X, pts[0].Y, pts[1].X, pts[1].Y,
            pts[2].X, pts[2].Y, pts[3].X, pts[3].Y);
    }

    #endregion
}
