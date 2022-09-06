// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* XAxis.cs --
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
/// <see cref="XAxis"/> inherits from <see cref="Axis"/>, and defines the
/// special characteristics of a horizontal axis, specifically located at
/// the bottom of the <see cref="Chart.Rect"/> of the <see cref="GraphPane"/>
/// object
/// </summary>
[Serializable]
public class XAxis
    : Axis, ICloneable
{
    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="XAxis"/> class.
    /// </summary>
    public new struct Default
    {
        // Default X Axis properties
        /// <summary>
        /// The default display mode for the <see cref="XAxis"/>
        /// (<see cref="Axis.IsVisible"/> property). true to display the scale
        /// values, title, tic marks, false to hide the axis entirely.
        /// </summary>
        public static bool IsVisible = true;

        /// <summary>
        /// Determines if a line will be drawn at the zero value for the
        /// <see cref="XAxis"/>, that is, a line that
        /// divides the negative values from positive values.
        /// <seealso cref="MajorGrid.IsZeroLine"/>.
        /// </summary>
        public static bool IsZeroLine = false;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor that sets all <see cref="XAxis"/> properties to
    /// default values as defined in the <see cref="Default"/> class
    /// </summary>
    public XAxis()
        : this ("X Axis")
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Default constructor that sets all <see cref="XAxis"/> properties to
    /// default values as defined in the <see cref="Default"/> class, except
    /// for the axis title
    /// </summary>
    /// <param name="title">The <see cref="Axis.Title"/> for this axis</param>
    public XAxis (string title)
        : base (title)
    {
        IsVisible = Default.IsVisible;
        _majorGrid._isZeroLine = Default.IsZeroLine;
        if (Scale != null)
        {
            Scale._fontSpec.Angle = 0F;
        }
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The XAxis object from which to copy</param>
    public XAxis (XAxis rhs)
        : base (rhs)
    {
        // пустое тело конструктора
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
    public XAxis Clone()
    {
        return new XAxis (this);
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
    protected XAxis (SerializationInfo info, StreamingContext context)
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
        // Move the origin to the BottomLeft of the ChartRect, which is the left
        // side of the X axis (facing from the label side)
        graphics.TranslateTransform (pane.Chart._rect.Left, pane.Chart._rect.Bottom);
    }

    /// <inheritdoc cref="Axis.IsPrimary"/>
    internal override bool IsPrimary
        (
            GraphPane pane
        )
    {
        return this == pane.XAxis;
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

    /// <inheritdoc cref="Axis.GetCrossAxis"/>
    public override Axis GetCrossAxis
        (
            GraphPane pane
        )
    {
        return pane.YAxis;
    }

//		override internal float GetMinPix( GraphPane pane )
//		{
//			return pane.Chart._rect.Left;
//		}

    #endregion
}
