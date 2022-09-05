// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* YAxis.cs --
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
/// <see cref="YAxis"/> inherits from <see cref="Axis"/>, and defines the
/// special characteristics of a vertical axis, specifically located on
/// the right side of the <see cref="Chart.Rect"/> of the <see cref="GraphPane"/>
/// object
/// </summary>
[Serializable]
public class YAxis
    : Axis, ICloneable
{
    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="YAxis"/> class.
    /// </summary>
    public new struct Default
    {
        // Default Y Axis properties
        /// <summary>
        /// The default display mode for the <see cref="YAxis"/>
        /// (<see cref="Axis.IsVisible"/> property). true to display the scale
        /// values, title, tic marks, false to hide the axis entirely.
        /// </summary>
        public static bool IsVisible = true;

        /// <summary>
        /// Determines if a line will be drawn at the zero value for the
        /// <see cref="YAxis"/>, that is, a line that
        /// divides the negative values from positive values.
        /// <seealso cref="MajorGrid.IsZeroLine"/>.
        /// </summary>
        public static bool IsZeroLine = true;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor that sets all <see cref="YAxis"/> properties to
    /// default values as defined in the <see cref="Default"/> class
    /// </summary>
    public YAxis()
        : this ("Y Axis")
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Default constructor that sets all <see cref="YAxis"/> properties to
    /// default values as defined in the <see cref="Default"/> class, except
    /// for the axis title
    /// </summary>
    /// <param name="title">The <see cref="Axis.Title"/> for this axis</param>
    public YAxis (string title)
        : base (title)
    {
        IsVisible = Default.IsVisible;
        _majorGrid._isZeroLine = Default.IsZeroLine;
        Scale._fontSpec.Angle = 90.0F;
        Title.FontSpec.Angle = -180F;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The YAxis object from which to copy</param>
    public YAxis (YAxis rhs)
        : base (rhs)
    {
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
    public YAxis Clone()
    {
        return new YAxis (this);
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
    protected YAxis
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

    /// <inheritdoc cref="Axis.SetTransformMatrix"/>
    public override void SetTransformMatrix
        (
            Graphics graphics,
            GraphPane pane,
            float scaleFactor
        )
    {
        // Move the origin to the TopLeft of the ChartRect, which is the left
        // side of the axis (facing from the label side)
        graphics.TranslateTransform (pane.Chart._rect.Left, pane.Chart._rect.Top);

        // rotate so this axis is in the left-right direction
        graphics.RotateTransform (90);
    }

    /// <inheritdoc cref="Axis.IsPrimary"/>
    internal override bool IsPrimary
        (
            GraphPane pane
        )
    {
        return this == pane.YAxis;
    }

    /// <inheritdoc cref="Axis.CalcCrossShift"/>
    internal override float CalcCrossShift
        (
            GraphPane pane
        )
    {
        var effCross = EffectiveCrossValue (pane);

        if (!CrossAuto)
        {
            return pane.XAxis.Scale._minPix - pane.XAxis.Scale.Transform (effCross);
        }
        else
        {
            return 0;
        }
    }

    /*
            override internal bool IsCrossed( GraphPane pane )
            {
                return !this.crossAuto && this.cross > pane.XAxis.Min && this.cross < pane.XAxis.Max;
            }
    */

    /// <inheritdoc cref="Axis.GetCrossAxis"/>
    public override Axis GetCrossAxis
        (
            GraphPane pane
        )
    {
        return pane.XAxis;
    }

    //		override internal float GetMinPix( GraphPane pane )
    //		{
    //			return pane.Chart._rect.Top;
    //		}

    #endregion
}
