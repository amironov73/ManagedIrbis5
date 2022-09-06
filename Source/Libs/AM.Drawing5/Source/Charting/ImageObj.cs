// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ImageObj.cs --
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
/// A class that represents an image object on the graph. A list of
/// <see cref="GraphObj"/> objects is maintained by the
/// <see cref="GraphObjList"/>
/// collection class.
/// </summary>
[Serializable]
public class ImageObj
    : GraphObj, ICloneable
{
    #region Fields

    /// <summary>
    /// Private field that stores the image.  Use the public property <see cref="Image"/>
    /// to access this value.
    /// </summary>
    private Image _image;

    /// <summary>
    /// Private field that determines if the image will be scaled to the output rectangle.
    /// </summary>
    /// <value>true to scale the image, false to draw the image unscaled, but clipped
    /// to the destination rectangle</value>
    private bool _isScaled;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="ImageObj"/> class.
    /// </summary>
    public new struct Default
    {
        // Default text item properties
        /// <summary>
        /// Default value for the <see cref="ImageObj"/>
        /// <see cref="ImageObj.IsScaled"/> property.
        /// </summary>
        public static bool IsScaled = true;
    }

    #endregion

    #region Properties

    /// <summary>
    /// The <see cref="System.Drawing.Image"/> object.
    /// </summary>
    /// <value> A <see cref="System.Drawing.Image"/> class reference. </value>
    public Image Image
    {
        get { return _image; }
        set { _image = value; }
    }

    /// <summary>
    /// Gets or sets a property that determines if the image will be scaled to the
    /// output rectangle (see <see cref="Location"/>).
    /// </summary>
    /// <value>true to scale the image, false to draw the image unscaled, but clipped
    /// to the destination rectangle</value>
    public bool IsScaled
    {
        get { return _isScaled; }
        set { _isScaled = value; }
    }

    #endregion

    #region Constructors

    /// <overloads>Constructors for the <see cref="ImageObj"/> object</overloads>
    /// <summary>
    /// A default constructor that places a null <see cref="System.Drawing.Image"/> at a
    /// default <see cref="RectangleF"/> of (0,0,1,1)
    /// </summary>
    public ImageObj() :
        this (null, 0, 0, 1, 1)
    {
    }

    /// <summary>
    /// A constructor that allows the <see cref="System.Drawing.Image"/> and
    /// <see cref="RectangleF"/> location for the
    /// <see cref="ImageObj"/> to be pre-specified.
    /// </summary>
    /// <param name="image">A <see cref="System.Drawing.Image"/> class that defines
    /// the image</param>
    /// <param name="rect">A <see cref="RectangleF"/> struct that defines the
    /// image location, specifed in units based on the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    public ImageObj (Image image, RectangleF rect) :
        this (image, rect.X, rect.Y, rect.Width, rect.Height)
    {
    }

    /// <overloads>Constructors for the <see cref="ImageObj"/> object</overloads>
    /// <summary>
    /// A constructor that allows the <see cref="System.Drawing.Image"/> and
    /// <see cref="RectangleF"/> location for the
    /// <see cref="ImageObj"/> to be pre-specified.
    /// </summary>
    /// <param name="image">A <see cref="System.Drawing.Image"/> class that defines
    /// the image</param>
    /// <param name="rect">A <see cref="RectangleF"/> struct that defines the
    /// image location, specifed in units based on the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="coordType">The <see cref="CoordType"/> enum value that
    /// indicates what type of coordinate system the x and y parameters are
    /// referenced to.</param>
    /// <param name="alignH">The <see cref="AlignH"/> enum that specifies
    /// the horizontal alignment of the object with respect to the (x,y) location</param>
    /// <param name="alignV">The <see cref="AlignV"/> enum that specifies
    /// the vertical alignment of the object with respect to the (x,y) location</param>
    public ImageObj (Image image, RectangleF rect, CoordType coordType,
        AlignH alignH, AlignV alignV) :
        base (rect.X, rect.Y, rect.Width, rect.Height, coordType,
            alignH, alignV)
    {
        _image = image;
        _isScaled = Default.IsScaled;
    }

    /// <overloads>Constructors for the <see cref="ImageObj"/> object</overloads>
    /// <summary>
    /// A constructor that allows the <see cref="System.Drawing.Image"/> and
    /// individual <see cref="System.Single"/> coordinate locations for the
    /// <see cref="ImageObj"/> to be pre-specified.
    /// </summary>
    /// <param name="image">A <see cref="System.Drawing.Image"/> class that defines
    /// the image</param>
    /// <param name="left">The position of the left side of the rectangle that defines the
    /// <see cref="ImageObj"/> location.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="top">The position of the top side of the rectangle that defines the
    /// <see cref="ImageObj"/> location.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="width">The width of the rectangle that defines the
    /// <see cref="ImageObj"/> location.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    /// <param name="height">The height of the rectangle that defines the
    /// <see cref="ImageObj"/> location.  The units of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.</param>
    public ImageObj (Image image, double left, double top,
        double width, double height) :
        base (left, top, width, height)
    {
        _image = image;
        _isScaled = Default.IsScaled;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="ImageObj"/> object from which to copy</param>
    public ImageObj (ImageObj rhs) : base (rhs)
    {
        _image = rhs._image;
        _isScaled = rhs.IsScaled;
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
    public ImageObj Clone()
    {
        return new ImageObj (this);
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
    protected ImageObj (SerializationInfo info, StreamingContext context) : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        int sch = info.GetInt32 ("schema2");

        _image = (Image)info.GetValue ("image", typeof (Image));
        _isScaled = info.GetBoolean ("isScaled");
    }

    /// <inheritdoc cref="GraphObj.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);
        info.AddValue ("schema2", schema2);
        info.AddValue ("image", _image);
        info.AddValue ("isScaled", _isScaled);
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
        if (_image != null)
        {
            // Convert the rectangle coordinates from the user coordinate system
            // to the screen coordinate system
            RectangleF tmpRect = _location.TransformRect (pane);

            if (_isScaled)
            {
                graphics.DrawImage (_image, tmpRect);
            }
            else
            {
                Region clip = graphics.Clip;
                graphics.SetClip (tmpRect);
                graphics.DrawImageUnscaled (_image, Rectangle.Round (tmpRect));
                graphics.SetClip (clip, CombineMode.Replace);

                //g.DrawImageUnscaledAndClipped( image, Rectangle.Round( tmpRect ) );
            }
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
        if (_image != null)
        {
            if (!base.PointInBox (point, pane, graphics, scaleFactor))
            {
                return false;
            }

            // transform the x,y location from the user-defined
            // coordinate frame to the screen pixel location
            RectangleF tmpRect = _location.TransformRect (pane);

            return tmpRect.Contains (point);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Determines the shape type and Coords values for this GraphObj
    /// </summary>
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
        RectangleF pixRect = _location.TransformRect (pane);

        shape = "rect";
        coords = string.Format ("{0:f0},{1:f0},{2:f0},{3:f0}",
            pixRect.Left, pixRect.Top, pixRect.Right, pixRect.Bottom);
    }

    #endregion
}
