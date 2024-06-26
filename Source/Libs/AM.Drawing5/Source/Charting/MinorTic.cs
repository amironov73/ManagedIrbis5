// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* MinorTic.cs --
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
/// Class that holds the specific properties for the minor tics.
/// </summary>
[Serializable]
public class MinorTic
    : ICloneable, ISerializable
{
    internal bool _isOutside,
        _isInside,
        _isOpposite,
        _isCrossOutside,
        _isCrossInside;

    internal float _penWidth,
        _size;

    internal Color _color;

    #region Constructors

    /// <summary>
    /// Default Constructor
    /// </summary>
    public MinorTic()
    {
        _size = Default.Size;
        _color = Default.Color;
        _penWidth = Default.PenWidth;

        IsOutside = Default.IsOutside;
        IsInside = Default.IsInside;
        IsOpposite = Default.IsOpposite;
        _isCrossOutside = Default.IsCrossOutside;
        _isCrossInside = Default.IsCrossInside;
    }

    /// <summary>
    /// Copy constructor.
    /// </summary>
    /// <param name="rhs">The <see cref="MinorTic" /> that is to be copied.</param>
    public MinorTic (MinorTic rhs)
    {
        _size = rhs._size;
        _color = rhs._color;
        _penWidth = rhs._penWidth;

        IsOutside = rhs.IsOutside;
        IsInside = rhs.IsInside;
        IsOpposite = rhs.IsOpposite;
        _isCrossOutside = rhs._isCrossOutside;
        _isCrossInside = rhs._isCrossInside;
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
    public MinorTic Clone()
    {
        return new MinorTic (this);
    }

    #endregion

    #region Properties

    /// <summary>
    /// The color to use for drawing the tics of this class instance
    /// </summary>
    /// <value> The color is defined using the
    /// <see cref="System.Drawing.Color"/> class</value>
    /// <seealso cref="Default.Color"/>.
    /// <seealso cref="IsOutside"/>
    /// <seealso cref="Axis.IsVisible"/>
    public Color Color
    {
        get => _color;
        set => _color = value;
    }

    /// <summary>
    /// The length of the major tic marks.
    /// </summary>
    /// <remarks>
    /// This length will be scaled
    /// according to the <see cref="PaneBase.CalcScaleFactor"/> for the
    /// <see cref="GraphPane"/>
    /// </remarks>
    /// <value>The tic size is measured in points (1/72 inch)</value>
    /// <seealso cref="Default.Size"/>.
    /// <seealso cref="IsOutside"/>
    /// <seealso cref="Axis.IsVisible"/>
    /// <seealso cref="Color"/>
    public float Size
    {
        get => _size;
        set => _size = value;
    }

    /// <summary>
    /// Calculate the scaled tic size for this <see cref="Axis"/>
    /// </summary>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    /// <returns>The scaled tic size, in points (1/72 inch)</returns>
    /// <seealso cref="Size"/>
    /// <seealso cref="Scale.FontSpec"/>
    /// <seealso cref="PaneBase.CalcScaleFactor"/>
    public float ScaledTic (float scaleFactor)
    {
        return (float)(_size * scaleFactor);
    }

    /// <summary>
    /// This is convenience property sets the status of all the different
    /// tic properties in this instance to the same value.  true to activate all tics,
    /// false to clear all tics.
    /// </summary>
    /// <remarks>
    /// This setting does not persist.  That is, you can clear all the tics with
    /// <see cref="IsAllTics" /> = false, then activate them individually (example:
    /// <see cref="IsOutside" /> = true).
    /// </remarks>
    /// <seealso cref="IsOutside"/>
    /// <seealso cref="IsInside"/>
    /// <seealso cref="IsOpposite"/>
    /// <seealso cref="IsCrossInside"/>
    /// <seealso cref="IsCrossOutside"/>
    public bool IsAllTics
    {
        set
        {
            IsOutside = value;
            IsInside = value;
            IsOpposite = value;
            _isCrossOutside = value;
            _isCrossInside = value;
        }
    }

    /// <summary>
    /// Gets or sets a property that determines whether or not the minor outside tic marks
    /// are shown.
    /// </summary>
    /// <remarks>
    /// These are the tic marks on the outside of the <see cref="Axis"/> border.
    /// The minor tic spacing is controlled by <see cref="Scale.MinorStep"/>.
    /// </remarks>
    /// <value>true to show the minor outside tic marks, false otherwise</value>
    /// <seealso cref="Default.IsOutside"/>.
    /// <seealso cref="IsOutside"/>
    /// <seealso cref="IsInside"/>
    /// <seealso cref="IsOpposite"/>
    /// <seealso cref="IsCrossInside"/>
    /// <seealso cref="IsCrossOutside"/>
    public bool IsOutside
    {
        get => _isOutside;
        set => _isOutside = value;
    }

    /// <summary>
    /// Gets or sets a property that determines whether or not the major inside tic marks
    /// are shown.
    /// </summary>
    /// <remarks>
    /// These are the tic marks on the inside of the <see cref="Axis"/> border.
    /// The major tic spacing is controlled by <see cref="Scale.MajorStep"/>.
    /// </remarks>
    /// <value>true to show the major inside tic marks, false otherwise</value>
    /// <seealso cref="Default.IsInside"/>.
    /// <seealso cref="IsOutside"/>
    /// <seealso cref="IsInside"/>
    /// <seealso cref="IsOpposite"/>
    /// <seealso cref="IsCrossInside"/>
    /// <seealso cref="IsCrossOutside"/>
    public bool IsInside
    {
        get => _isInside;
        set => _isInside = value;
    }

    /// <summary>
    /// Gets or sets a property that determines whether or not the major opposite tic marks
    /// are shown.
    /// </summary>
    /// <remarks>
    /// These are the tic marks on the inside of the <see cref="Axis"/> border on
    /// the opposite side from the axis.
    /// The major tic spacing is controlled by <see cref="Scale.MajorStep"/>.
    /// </remarks>
    /// <value>true to show the major opposite tic marks, false otherwise</value>
    /// <seealso cref="Default.IsOpposite"/>.
    /// <seealso cref="IsOutside"/>
    /// <seealso cref="IsInside"/>
    /// <seealso cref="IsOpposite"/>
    /// <seealso cref="IsCrossInside"/>
    /// <seealso cref="IsCrossOutside"/>
    public bool IsOpposite
    {
        get => _isOpposite;
        set => _isOpposite = value;
    }

    /// <summary>
    /// Gets or sets the display mode for the <see cref="Axis"/> major outside
    /// "cross" tic marks.
    /// </summary>
    /// <remarks>
    /// The "cross" tics are a special, additional set of tic marks that
    /// always appear on the actual axis, even if it has been shifted due
    /// to the <see cref="Axis.Cross" /> setting.  The other tic marks are always
    /// fixed to the edges of the <see cref="Chart.Rect"/>.  The cross tics
    /// are normally not displayed, since, if <see cref="Axis.CrossAuto" /> is true,
    /// they will exactly overlay the "normal" and "inside" tics.  If
    /// <see cref="Axis.CrossAuto"/> is false, then you will most likely want to
    /// enable the cross tics.
    /// The major tic spacing is controlled by <see cref="Scale.MajorStep"/>.
    /// </remarks>
    /// <value>true to show the major cross tic marks, false otherwise</value>
    public bool IsCrossOutside
    {
        get => _isCrossOutside;
        set => _isCrossOutside = value;
    }

    /// <summary>
    /// Gets or sets the display mode for the <see cref="Axis"/> major inside
    /// "cross" tic marks.
    /// </summary>
    /// <remarks>
    /// The "cross" tics are a special, additional set of tic marks that
    /// always appear on the actual axis, even if it has been shifted due
    /// to the <see cref="Axis.Cross" /> setting.  The other tic marks are always
    /// fixed to the edges of the <see cref="Chart.Rect"/>.  The cross tics
    /// are normally not displayed, since, if <see cref="Axis.CrossAuto" /> is true,
    /// they will exactly overlay the "normal" and "inside" tics.  If
    /// <see cref="Axis.CrossAuto"/> is false, then you will most likely want to
    /// enable the cross tics.
    /// The major tic spacing is controlled by <see cref="Scale.MajorStep"/>.
    /// </remarks>
    /// <value>true to show the major cross tic marks, false otherwise</value>
    public bool IsCrossInside
    {
        get => _isCrossInside;
        set => _isCrossInside = value;
    }

    /// <summary>
    /// Gets or sets the pen width to be used when drawing the tic marks for
    /// this <see cref="Axis"/>
    /// </summary>
    /// <value>The pen width is defined in points (1/72 inch)</value>
    /// <seealso cref="Default.PenWidth"/>.
    /// <seealso cref="IsOutside"/>
    /// <seealso cref="Color"/>
    public float PenWidth
    {
        get => _penWidth;
        set => _penWidth = value;
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema = 10;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected MinorTic
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema").NotUsed();

        _color = (Color)info.GetValue ("color", typeof (Color))!;
        _size = info.GetSingle ("size");
        _penWidth = info.GetSingle ("penWidth");

        IsOutside = info.GetBoolean ("IsOutside");
        IsInside = info.GetBoolean ("IsInside");
        IsOpposite = info.GetBoolean ("IsOpposite");
        _isCrossOutside = info.GetBoolean ("isCrossOutside");
        _isCrossInside = info.GetBoolean ("isCrossInside");
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public virtual void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        info.AddValue ("schema", schema);

        info.AddValue ("color", _color);
        info.AddValue ("size", _size);
        info.AddValue ("penWidth", _penWidth);

        info.AddValue ("IsOutside", IsOutside);
        info.AddValue ("IsInside", IsInside);
        info.AddValue ("IsOpposite", IsOpposite);
        info.AddValue ("isCrossOutside", _isCrossOutside);
        info.AddValue ("isCrossInside", _isCrossInside);
    }

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="MinorTic"/> class.
    /// </summary>
    public struct Default
    {
        /// <summary>
        /// The default size for the <see cref="Axis"/> minor tic marks.
        /// (<see cref="MinorTic.Size"/> property). Units are in points (1/72 inch).
        /// </summary>
        public static float Size = 2.5F;

        /// <summary>
        /// The default pen width for drawing the <see cref="Axis"/> tic marks.
        /// (<see cref="MinorTic.PenWidth"/> property). Units are in points (1/72 inch).
        /// </summary>
        public static float PenWidth = 1.0F;

        /// <summary>
        /// The display mode for the <see cref="Axis"/> minor outside tic marks
        /// (<see cref="MinorTic.IsOutside"/> property).
        /// The minor tic spacing is controlled by <see cref="Scale.MinorStep"/>.
        /// </summary>
        /// <value>true to show the minor tic marks (outside the axis),
        /// false otherwise</value>
        public static bool IsOutside = true;

        /// <summary>
        /// The display mode for the <see cref="Axis"/> minor inside tic marks
        /// (<see cref="MinorTic.IsInside"/> property).
        /// The minor tic spacing is controlled by <see cref="Scale.MinorStep"/>.
        /// </summary>
        /// <value>true to show the minor tic marks (inside the axis),
        /// false otherwise</value>
        public static bool IsInside = true;

        /// <summary>
        /// The display mode for the <see cref="Axis"/> minor opposite tic marks
        /// (<see cref="MinorTic.IsOpposite"/> property).
        /// The minor tic spacing is controlled by <see cref="Scale.MinorStep"/>.
        /// </summary>
        /// <value>true to show the minor tic marks
        /// (inside the axis on the opposite side),
        /// false otherwise</value>
        public static bool IsOpposite = true;

        /// <summary>
        /// The default display mode for the <see cref="Axis"/> minor outside
        /// "cross" tic marks (<see cref="MinorTic.IsCrossOutside"/> property).
        /// </summary>
        /// <remarks>
        /// The "cross" tics are a special, additional set of tic marks that
        /// always appear on the actual axis, even if it has been shifted due
        /// to the <see cref="Axis.Cross" /> setting.  The other tic marks are always
        /// fixed to the edges of the <see cref="Chart.Rect"/>.  The cross tics
        /// are normally not displayed, since, if <see cref="Axis.CrossAuto" /> is true,
        /// they will exactly overlay the "normal" and "inside" tics.  If
        /// <see cref="Axis.CrossAuto"/> is false, then you will most likely want to
        /// enable the cross tics.
        /// The minor tic spacing is controlled by <see cref="Scale.MinorStep"/>.
        /// </remarks>
        /// <value>true to show the major cross tic marks, false otherwise</value>
        public static bool IsCrossOutside = false;

        /// <summary>
        /// The default display mode for the <see cref="Axis"/> minor inside
        /// "cross" tic marks (<see cref="MinorTic.IsCrossInside"/> property).
        /// </summary>
        /// <remarks>
        /// The "cross" tics are a special, additional set of tic marks that
        /// always appear on the actual axis, even if it has been shifted due
        /// to the <see cref="Axis.Cross" /> setting.  The other tic marks are always
        /// fixed to the edges of the <see cref="Chart.Rect"/>.  The cross tics
        /// are normally not displayed, since, if <see cref="Axis.CrossAuto" /> is true,
        /// they will exactly overlay the "normal" and "inside" tics.  If
        /// <see cref="Axis.CrossAuto"/> is false, then you will most likely want to
        /// enable the cross tics.
        /// The major tic spacing is controlled by <see cref="Scale.MinorStep"/>.
        /// </remarks>
        /// <value>true to show the major cross tic marks, false otherwise</value>
        public static bool IsCrossInside = false;

        /// <summary>
        /// The default color for minor tics (<see cref="MinorTic.Color"/> property).
        /// </summary>
        public static Color Color = Color.Black;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Draw a tic mark at the specified single position.  This includes the inner, outer,
    /// cross and opposite tic marks as required.
    /// </summary>
    /// <param name="g">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="pen">Graphic <see cref="Pen"/> with which to draw the tic mark.</param>
    /// <param name="pixVal">The pixel location of the tic mark on this
    /// <see cref="Axis"/></param>
    /// <param name="topPix">The pixel value of the top of the axis border</param>
    /// <param name="shift">The number of pixels to shift this axis, based on the
    /// value of <see cref="Axis.Cross"/>.  A positive value is into the ChartRect relative to
    /// the default axis position.</param>
    /// <param name="scaledTic">The scaled size of a minor tic, in pixel units</param>
    internal void Draw (Graphics g, GraphPane pane, Pen pen, float pixVal, float topPix,
        float shift, float scaledTic)
    {
        // draw the outside tic
        SmoothingMode smode = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.None;
        if (IsOutside)
        {
            g.DrawLine (pen, pixVal, shift, pixVal, shift + scaledTic);
        }

        // draw the cross tic
        if (_isCrossOutside)
        {
            g.DrawLine (pen, pixVal, 0.0f, pixVal, scaledTic);
        }

        // draw the inside tic
        if (IsInside)
        {
            g.DrawLine (pen, pixVal, shift, pixVal, shift - scaledTic);
        }

        // draw the inside cross tic
        if (_isCrossInside)
        {
            g.DrawLine (pen, pixVal, 0.0f, pixVal, -scaledTic);
        }

        // draw the opposite tic
        if (IsOpposite)
        {
            g.DrawLine (pen, pixVal, topPix, pixVal, topPix + scaledTic);
        }

        g.SmoothingMode = smode;
    }

    internal Pen GetPen (GraphPane pane, float scaleFactor)
    {
        return new Pen (_color, pane.ScaledPenWidth (_penWidth, scaleFactor));
    }

    #endregion
}
