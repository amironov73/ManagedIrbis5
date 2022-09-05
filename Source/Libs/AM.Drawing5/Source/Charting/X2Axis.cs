// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* X2Axis.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// <see cref="X2Axis"/> inherits from <see cref="Axis"/>, and defines the
/// special characteristics of a horizontal axis, specifically located at
/// the top of the <see cref="Chart.Rect"/> of the <see cref="GraphPane"/>
/// object
/// </summary>
[Serializable]
public class X2Axis
    : Axis, ICloneable
{
    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="X2Axis"/> class.
    /// </summary>
    public new struct Default
    {
        // Default X2 Axis properties
        /// <summary>
        /// The default display mode for the <see cref="X2Axis"/>
        /// (<see cref="Axis.IsVisible"/> property). true to display the scale
        /// values, title, tic marks, false to hide the axis entirely.
        /// </summary>
        public static bool IsVisible = false;

        /// <summary>
        /// Determines if a line will be drawn at the zero value for the
        /// <see cref="X2Axis"/>, that is, a line that
        /// divides the negative values from positive values.
        /// <seealso cref="MajorGrid.IsZeroLine"/>.
        /// </summary>
        public static bool IsZeroLine = false;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor that sets all <see cref="X2Axis"/> properties to
    /// default values as defined in the <see cref="Default"/> class
    /// </summary>
    public X2Axis()
        : this ("X2 Axis")
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Default constructor that sets all <see cref="X2Axis"/> properties to
    /// default values as defined in the <see cref="Default"/> class, except
    /// for the axis title
    /// </summary>
    /// <param name="title">The <see cref="Axis.Title"/> for this axis</param>
    public X2Axis (string title)
        : base (title)
    {
        IsVisible = Default.IsVisible;
        _majorGrid._isZeroLine = Default.IsZeroLine;
        Scale._fontSpec.Angle = 180F;
        Title.FontSpec.Angle = 180F;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The X2Axis object from which to copy</param>
    public X2Axis (X2Axis rhs)
        : base (rhs)
    {
        // пустое тело конструктора
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
    public X2Axis Clone()
    {
        return new X2Axis (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema2 = 11;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected X2Axis
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();
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
    }

    #endregion

    #region Methods

    /// <summary>
    /// Setup the Transform Matrix to handle drawing of this <see cref="X2Axis"/>
    /// </summary>
    /// <param name="graphics">
    /// A graphic device object to be drawn into.  This is normally e.Graphics from the
    /// PaintEventArgs argument to the Paint() method.
    /// </param>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <param name="scaleFactor">
    /// The scaling factor to be used for rendering objects.  This is calculated and
    /// passed down by the parent <see cref="GraphPane"/> object using the
    /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
    /// font sizes, etc. according to the actual size of the graph.
    /// </param>
    public override void SetTransformMatrix
        (
            Graphics graphics,
            GraphPane pane,
            float scaleFactor
        )
    {
        // Move the origin to the TopLeft of the ChartRect, which is the left
        // side of the X2 axis (facing from the label side)
        graphics.TranslateTransform (pane.Chart._rect.Right, pane.Chart._rect.Top);

        //g.ScaleTransform( 1.0f, -1.0f );
        // rotate so this axis is in the right-left direction
        graphics.RotateTransform (180);
    }

    /// <summary>
    /// Determines if this <see cref="Axis" /> object is a "primary" one.
    /// </summary>
    /// <remarks>
    /// The primary axes are the <see cref="XAxis" /> (always),
    /// the <see cref="X2Axis" /> (always), the first
    /// <see cref="YAxis" /> in the <see cref="GraphPane.YAxisList" />
    /// (<see cref="CurveItem.YAxisIndex" /> = 0),  and the first
    /// <see cref="Y2Axis" /> in the <see cref="GraphPane.Y2AxisList" />
    /// (<see cref="CurveItem.YAxisIndex" /> = 0).  Note that
    /// <see cref="GraphPane.YAxis" /> and <see cref="GraphPane.Y2Axis" />
    /// always reference the primary axes.
    /// </remarks>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <returns>true for a primary <see cref="Axis" /> (for the <see cref="X2Axis" />,
    /// this is always true), false otherwise</returns>
    internal override bool IsPrimary (GraphPane pane)
    {
        return this == pane.X2Axis;
    }

    /// <summary>
    /// Calculate the "shift" size, in pixels, in order to shift the axis from its default
    /// location to the value specified by <see cref="Axis.Cross"/>.
    /// </summary>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    /// <returns>The shift amount measured in pixels</returns>
    internal override float CalcCrossShift (GraphPane pane)
    {
        var effCross = EffectiveCrossValue (pane);

        if (!CrossAuto)
        {
            return pane.YAxis.Scale.Transform (effCross) - pane.YAxis.Scale._maxPix;
        }
        else
        {
            return 0;
        }
    }

    /*
            override internal bool IsCrossed( GraphPane pane )
            {
                return !this.crossAuto && this.cross > pane.YAxis.Min && this.cross < pane.YAxis.Max;
            }
    */
    /// <summary>
    /// Gets the "Cross" axis that corresponds to this axis.
    /// </summary>
    /// <remarks>
    /// The cross axis is the axis which determines the of this Axis when the
    /// <see cref="Axis.Cross" >Axis.Cross</see> property is used.  The
    /// cross axis for any <see cref="XAxis" /> or <see cref="X2Axis" />
    /// is always the primary <see cref="YAxis" />, and
    /// the cross axis for any <see cref="YAxis" /> or <see cref="Y2Axis" /> is
    /// always the primary <see cref="XAxis" />.
    /// </remarks>
    /// <param name="pane">
    /// A reference to the <see cref="GraphPane"/> object that is the parent or
    /// owner of this object.
    /// </param>
    public override Axis GetCrossAxis (GraphPane pane)
    {
        return pane.YAxis;
    }

    //		override internal float GetMinPix( GraphPane pane )
    //		{
    //			return pane.Chart._rect.Left;
    //		}

    #endregion
}
