// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* BoxObj.cs -- объект прямоугольника с рамкой и/или заливкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Класс, представляющий объект прямоугольника с рамкой и/или заливкой
/// на графике. Список объектов <code>BoxObj</code> поддерживается классом
/// коллекции <see cref="GraphObjList"/>.
/// </summary>
[Serializable]
public class BoxObj
    : GraphObj, ICloneable, ISerializable
{
    #region Fields

    /// <summary>
    /// Private field that stores the <see cref="Charting.Fill"/> data for this
    /// <see cref="BoxObj"/>.  Use the public property <see cref="Fill"/> to
    /// access this value.
    /// </summary>
    protected Fill _fill;

    /// <summary>
    /// Private field that determines the properties of the border around this
    /// <see cref="BoxObj"/>
    /// Use the public property <see cref="Border"/> to access this value.
    /// </summary>
    protected Border _border;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="ArrowObj"/> class.
    /// </summary>
    public new struct Default
    {
        /// <summary>
        /// The default pen width used for the <see cref="BoxObj"/> border
        /// (<see cref="LineBase.Width"/> property).  Units are points (1/72 inch).
        /// </summary>
        public static float PenWidth = 1.0F;

        /// <summary>
        /// The default color used for the <see cref="BoxObj"/> border
        /// (<see cref="LineBase.Color"/> property).
        /// </summary>
        public static Color BorderColor = Color.Black;

        /// <summary>
        /// The default color used for the <see cref="BoxObj"/> fill
        /// (<see cref="Fill.Color"/> property).
        /// </summary>
        public static Color FillColor = Color.White;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="Charting.Fill"/> data for this
    /// <see cref="BoxObj"/>.
    /// </summary>
    public Fill Fill
    {
        get { return _fill; }
        set { _fill = value; }
    }

    /// <summary>
    /// Gets or sets the <see cref="Charting.Border"/> object, which
    /// determines the properties of the border around this
    /// <see cref="BoxObj"/>
    /// </summary>
    public Border Border
    {
        get { return _border; }
        set { _border = value; }
    }

    #endregion

    #region Constructors

    /// <overloads>Constructors for the <see cref="BoxObj"/> object</overloads>
    /// <summary>
    /// A constructor that allows the position, border color, and solid fill color
    /// of the <see cref="BoxObj"/> to be pre-specified.
    /// </summary>
    /// <param name="borderColor">An arbitrary <see cref="System.Drawing.Color"/> specification
    /// for the box border</param>
    /// <param name="fillColor">An arbitrary <see cref="System.Drawing.Color"/> specification
    /// for the box fill (will be a solid color fill)</param>
    /// <param name="x">The x location for this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="y">The y location for this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="width">The width of this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="height">The height of this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    public BoxObj (double x, double y, double width, double height, Color borderColor, Color fillColor)
        : base (x, y, width, height)
    {
        Border = new Border (borderColor, Default.PenWidth);
        Fill = new Fill (fillColor);
    }

    /// <summary>
    /// A constructor that allows the position
    /// of the <see cref="BoxObj"/> to be pre-specified.  Other properties are defaulted.
    /// </summary>
    /// <param name="x">The x location for this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="y">The y location for this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="width">The width of this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="height">The height of this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    public BoxObj (double x, double y, double width, double height)
        :
        base (x, y, width, height)
    {
        Border = new Border (Default.BorderColor, Default.PenWidth);
        Fill = new Fill (Default.FillColor);
    }

    /// <summary>
    /// A default constructor that creates a <see cref="BoxObj"/> using a location of (0,0),
    /// and a width,height of (1,1).  Other properties are defaulted.
    /// </summary>
    public BoxObj() : this (0, 0, 1, 1)
    {
    }

    /// <summary>
    /// A constructor that allows the position, border color, and two-color
    /// gradient fill colors
    /// of the <see cref="BoxObj"/> to be pre-specified.
    /// </summary>
    /// <param name="borderColor">An arbitrary <see cref="System.Drawing.Color"/> specification
    /// for the box border</param>
    /// <param name="fillColor1">An arbitrary <see cref="System.Drawing.Color"/> specification
    /// for the start of the box gradient fill</param>
    /// <param name="fillColor2">An arbitrary <see cref="System.Drawing.Color"/> specification
    /// for the end of the box gradient fill</param>
    /// <param name="x">The x location for this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="y">The y location for this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="width">The width of this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    /// <param name="height">The height of this <see cref="BoxObj" />.  This will be in units determined by
    /// <see cref="Location.CoordinateFrame" />.</param>
    public BoxObj (double x, double y, double width, double height, Color borderColor,
        Color fillColor1, Color fillColor2) :
        base (x, y, width, height)
    {
        Border = new Border (borderColor, Default.PenWidth);
        Fill = new Fill (fillColor1, fillColor2);
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="BoxObj"/> object from which to copy</param>
    public BoxObj (BoxObj rhs) : base (rhs)
    {
        Border = rhs.Border.Clone();
        Fill = rhs.Fill.Clone();
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
    public BoxObj Clone()
    {
        return new BoxObj (this);
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
    protected BoxObj
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        _fill = (Fill) info.GetValue ("fill", typeof (Fill)).ThrowIfNull ();
        _border = (Border) info.GetValue ("border", typeof (Border)).ThrowIfNull ();
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
        info.AddValue ("fill", _fill);
        info.AddValue ("border", _border);
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
        var pixRect = Location.TransformRect (pane);

        // Clip the rect to just outside the PaneRect so we don't end up with wild coordinates.
        var tmpRect = pane.Rect;
        tmpRect.Inflate (20, 20);
        pixRect.Intersect (tmpRect);

        if (Math.Abs (pixRect.Left) < 100000 &&
            Math.Abs (pixRect.Top) < 100000 &&
            Math.Abs (pixRect.Right) < 100000 &&
            Math.Abs (pixRect.Bottom) < 100000)
        {
            // If the box is to be filled, fill it
            _fill.Draw (graphics, pixRect);

            // Draw the border around the box if required
            _border.Draw (graphics, pane, scaleFactor, pixRect);
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
        var pixRect = _location.TransformRect (pane);

        return pixRect.Contains (point);
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
        var pixRect = _location.TransformRect (pane);

        shape = "rect";
        coords = string.Create
            (
                CultureInfo.InvariantCulture,
                $"{pixRect.Left:f0},{pixRect.Top:f0},{pixRect.Right:f0},{pixRect.Bottom:f0}"
            );
    }

    #endregion
}
