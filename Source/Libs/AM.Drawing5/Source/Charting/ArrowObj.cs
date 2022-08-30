// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ArrowObj.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A class that represents a graphic arrow or line object on the graph.  A list of
/// ArrowObj objects is maintained by the <see cref="GraphObjList"/> collection class.
/// </summary>
[Serializable]
public class ArrowObj
    : LineObj, ICloneable, ISerializable
{
    #region Fields

    /// <summary>
    /// Private field that stores the arrowhead size, measured in points.
    /// Use the public property <see cref="Size"/> to access this value.
    /// </summary>
    private float _size;

    /// <summary>
    /// Private boolean field that stores the arrowhead state.
    /// Use the public property <see cref="IsArrowHead"/> to access this value.
    /// </summary>
    /// <value> true if an arrowhead is to be drawn, false otherwise </value>
    private bool _isArrowHead;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="ArrowObj"/> class.
    /// </summary>
    public new struct Default
    {
        /// <summary>
        /// The default size for the <see cref="ArrowObj"/> item arrowhead
        /// (<see cref="ArrowObj.Size"/> property).  Units are in points (1/72 inch).
        /// </summary>
        public static float Size = 12.0F;

        /// <summary>
        /// The default display mode for the <see cref="ArrowObj"/> item arrowhead
        /// (<see cref="ArrowObj.IsArrowHead"/> property).  true to show the
        /// arrowhead, false to hide it.
        /// </summary>
        public static bool IsArrowHead = true;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The size of the arrowhead.
    /// </summary>
    /// <remarks>The display of the arrowhead can be
    /// enabled or disabled with the <see cref="IsArrowHead"/> property.
    /// </remarks>
    /// <value> The size is defined in points (1/72 inch) </value>
    /// <seealso cref="Default.Size"/>
    public float Size
    {
        get => _size;
        set => _size = value;
    }

    /// <summary>
    /// Determines whether or not to draw an arrowhead
    /// </summary>
    /// <value> true to show the arrowhead, false to show the line segment
    /// only</value>
    /// <seealso cref="Default.IsArrowHead"/>
    public bool IsArrowHead
    {
        get => _isArrowHead;
        set => _isArrowHead = value;
    }

    #endregion

    #region Constructors

    /// <overloads>Constructors for the <see cref="ArrowObj"/> object</overloads>
    /// <summary>
    /// A constructor that allows the position, color, and size of the
    /// <see cref="ArrowObj"/> to be pre-specified.
    /// </summary>
    /// <param name="color">An arbitrary <see cref="System.Drawing.Color"/> specification
    /// for the arrow</param>
    /// <param name="size">The size of the arrowhead, measured in points.</param>
    /// <param name="x1">The x position of the starting point that defines the
    /// arrow.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y1">The y position of the starting point that defines the
    /// arrow.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="x2">The x position of the ending point that defines the
    /// arrow.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y2">The y position of the ending point that defines the
    /// arrow.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    public ArrowObj (Color color, float size, double x1, double y1,
        double x2, double y2)
        : base (color, x1, y1, x2, y2)
    {
        _isArrowHead = Default.IsArrowHead;
        _size = size;
    }

    /// <summary>
    /// A constructor that allows only the position of the
    /// arrow to be pre-specified.  All other properties are set to
    /// default values
    /// </summary>
    /// <param name="x1">The x position of the starting point that defines the
    /// <see cref="ArrowObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y1">The y position of the starting point that defines the
    /// <see cref="ArrowObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="x2">The x position of the ending point that defines the
    /// <see cref="ArrowObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="y2">The y position of the ending point that defines the
    /// <see cref="ArrowObj"/>.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    public ArrowObj (double x1, double y1, double x2, double y2)
        : this (LineBase.Default.Color, Default.Size, x1, y1, x2, y2)
    {
    }

    /// <summary>
    /// Default constructor -- places the <see cref="ArrowObj"/> at location
    /// (0,0) to (1,1).  All other values are defaulted.
    /// </summary>
    public ArrowObj()
        :
        this (LineBase.Default.Color, Default.Size, 0, 0, 1, 1)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="ArrowObj"/> object from which to copy</param>
    public ArrowObj (ArrowObj rhs)
        : base (rhs)
    {
        _size = rhs.Size;
        _isArrowHead = rhs.IsArrowHead;
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
    public new ArrowObj Clone()
    {
        return new ArrowObj (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema3 = 10;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected ArrowObj
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        var sch = info.GetInt32 ("schema3");

        _size = info.GetSingle ("size");
        _isArrowHead = info.GetBoolean ("isArrowHead");
    }

    /// <inheritdoc cref="LineObj.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);
        info.AddValue ("schema3", schema2);
        info.AddValue ("size", _size);
        info.AddValue ("isArrowHead", _isArrowHead);
    }

    #endregion

    #region Rendering methods

    /// <inheritdoc cref="LineObj.Draw"/>
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

        if (pix1.X > -10000 && pix1.X < 100000 && pix1.Y > -100000 && pix1.Y < 100000 &&
            pix2.X > -10000 && pix2.X < 100000 && pix2.Y > -100000 && pix2.Y < 100000)
        {
            // get a scaled size for the arrowhead
            var scaledSize = (float)(_size * scaleFactor);

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
            using (var pen = _line.GetPen (pane, scaleFactor))

                //new Pen( _color, pane.ScaledPenWidth( _penWidth, scaleFactor ) ) )
            {
                //pen.DashStyle = _style;

                // Only show the arrowhead if required
                if (_isArrowHead)
                {
                    // Draw the line segment for this arrow
                    graphics.DrawLine (pen, 0, 0, length - scaledSize + 1, 0);

                    // Create a polygon representing the arrowhead based on the scaled
                    // size
                    var polyPt = new PointF[4];
                    var hsize = scaledSize / 3.0F;
                    polyPt[0].X = length;
                    polyPt[0].Y = 0;
                    polyPt[1].X = length - scaledSize;
                    polyPt[1].Y = hsize;
                    polyPt[2].X = length - scaledSize;
                    polyPt[2].Y = -hsize;
                    polyPt[3] = polyPt[0];

                    using (var brush = new SolidBrush (_line._color))

                        // render the arrowhead
                        graphics.FillPolygon (brush, polyPt);
                }
                else
                {
                    graphics.DrawLine (pen, 0, 0, length, 0);
                }
            }

            // Restore the transform matrix back to its original state
            graphics.Transform = transform;
        }
    }

    #endregion
}
