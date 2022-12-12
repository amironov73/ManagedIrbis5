// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Border.cs -- свойства границы (фрейма) для объекта
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
/// Класс, который инкапсулирует свойства границы (фрейма) для объекта.
/// Класс <see cref="Border"/> используется в различных объектах ZedGraph
/// для обработки рисования границы вокруг объекта.
/// </summary>
[Serializable]
public class Border
    : LineBase, ISerializable, ICloneable
{
    #region Fields

    /// <summary>
    /// Private field that stores the amount of inflation to be done on the rectangle
    /// before rendering.  This allows the border to be inset or outset relative to
    /// the actual rectangle area.  Use the public property <see cref="InflateFactor"/>
    /// to access this value.
    /// </summary>
    private float _inflateFactor;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="Fill"/> class.
    /// </summary>
    public new struct Default
    {
        /// <summary>
        /// The default value for <see cref="Border.InflateFactor"/>, in units of points (1/72 inch).
        /// </summary>
        /// <seealso cref="Border.InflateFactor"/>
        public static float InflateFactor = 0.0F;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// The default constructor.  Initialized to default values.
    /// </summary>
    public Border()
    {
        _inflateFactor = Default.InflateFactor;
    }

    /// <summary>
    /// Constructor that specifies the visibility, color and penWidth of the Border.
    /// </summary>
    /// <param name="isVisible">Determines whether or not the Border will be drawn.</param>
    /// <param name="color">The color of the Border</param>
    /// <param name="width">The width, in points (1/72 inch), for the Border.</param>
    public Border
        (
            bool isVisible,
            Color color,
            float width
        )
        : base (color)
    {
        Width = width;
        IsVisible = isVisible;
    }

    /// <summary>
    /// Constructor that specifies the color and penWidth of the Border.
    /// </summary>
    /// <param name="color">The color of the Border</param>
    /// <param name="width">The width, in points (1/72 inch), for the Border.</param>
    public Border
        (
            Color color,
            float width
        )
        : this (!color.IsEmpty, color, width)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The Border object from which to copy</param>
    public Border (Border rhs) : base (rhs)
    {
        _inflateFactor = rhs._inflateFactor;
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
    public Border Clone()
    {
        return new Border (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema = 11;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected Border
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema").NotUsed();

        _inflateFactor = info.GetSingle ("inflateFactor");
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);

        info.AddValue ("schema", schema);
        info.AddValue ("inflateFactor", _inflateFactor);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the amount of inflation to be done on the rectangle
    /// before rendering.
    /// </summary>
    /// <remarks>This allows the border to be inset or outset relative to
    /// the actual rectangle area.
    /// </remarks>
    public float InflateFactor
    {
        get => _inflateFactor;
        set => _inflateFactor = value;
    }

    #endregion

    #region Methods

    /*
    /// <summary>
    /// Create a new <see cref="Pen"/> object from the properties of this
    /// <see cref="Border"/> object.
    /// </summary>
    /// <param name="isPenWidthScaled">
    /// Set to true to have the <see cref="Border"/> pen width scaled with the
    /// scaleFactor.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor for the features of the graph based on the <see cref="PaneBase.BaseDimension"/>.  This
    /// scaling factor is calculated by the <see cref="PaneBase.CalcScaleFactor"/> method.  The scale factor
    /// represents a linear multiple to be applied to font sizes, symbol sizes, etc.
    /// </param>
    /// <returns>A <see cref="Pen"/> object with the proper color and pen width.</returns>
    public Pen MakePen( bool isPenWidthScaled, float scaleFactor )
    {
        float scaledPenWidth = _width;
        if ( isPenWidthScaled )
            scaledPenWidth = (float)(_width * scaleFactor);

        return new Pen( _color, scaledPenWidth );
    }
    */

    /// <summary>
    /// Draw the specified Border (<see cref="RectangleF"/>) using the properties of
    /// this <see cref="Border"/> object.
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="PaneBase"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor for the features of the graph based on the <see cref="PaneBase.BaseDimension"/>.  This
    /// scaling factor is calculated by the <see cref="PaneBase.CalcScaleFactor"/> method.  The scale factor
    /// represents a linear multiple to be applied to font sizes, symbol sizes, etc.
    /// </param>
    /// <param name="rect">A <see cref="RectangleF"/> struct to be drawn.</param>
    public void Draw
        (
            Graphics graphics,
            PaneBase pane,
            float scaleFactor,
            RectangleF rect
        )
    {
        // Need to use the RectangleF props since rounding it can cause the axisFrame to
        // not line up properly with the last tic mark
        if (IsVisible)
        {
            var smode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.None;

            RectangleF tRect = rect;

            float scaledInflate = (float)(_inflateFactor * scaleFactor);
            tRect.Inflate (scaledInflate, scaledInflate);

            using (Pen pen = GetPen (pane, scaleFactor))
                graphics.DrawRectangle (pen, tRect.X, tRect.Y, tRect.Width, tRect.Height);

            graphics.SmoothingMode = smode;
        }
    }

    #endregion
}
