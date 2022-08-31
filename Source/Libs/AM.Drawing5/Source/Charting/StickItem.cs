// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* StickItem.cs --
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
/// Encapsulates a curve type that is displayed as a series of vertical "sticks",
/// one at each defined point.
/// </summary>
/// <remarks>
/// The sticks run from the zero value of the Y axis, to the Y point defined in each
/// <see cref="PointPair"/> of the <see cref="IPointList" /> (see <see cref="CurveItem.Points"/>).
/// The properties of the sticks are defined in the <see cref="Line"/> property.
/// Normally, the <see cref="Symbol"/> is not visible.  However, if you manually enable the
/// <see cref="Symbol"/> using the <see cref="Symbol.IsVisible"/> property, the
/// symbols will be drawn at the "Z" value from each <see cref="PointPair" /> (see
/// <see cref="PointPair.Z" />).
/// </remarks>
[Serializable]
public class StickItem
    : LineItem, ICloneable
{
    #region Properties

    /// <inheritdoc cref="LineItem.IsZIncluded"/>
    internal override bool IsZIncluded
        (
            GraphPane pane
        )
    {
        pane.NotUsed();

        return _symbol.IsVisible;
    }

    /// <inheritdoc cref="LineItem.IsXIndependent"/>
    internal override bool IsXIndependent
        (
            GraphPane pane
        )
    {
        pane.NotUsed();

        return true;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new <see cref="StickItem"/>, specifying only the legend <see cref="CurveItem.Label" />.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    public StickItem (string label) : base (label)
    {
        _symbol.IsVisible = false;
    }

    /// <summary>
    /// Create a new <see cref="StickItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="x">An array of double precision values that define
    /// the independent (X axis) values for this curve</param>
    /// <param name="y">An array of double precision values that define
    /// the dependent (Y axis) values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    /// <param name="lineWidth">The width (in points) to be used for the <see cref="Line"/>.  This
    /// width is scaled based on <see cref="PaneBase.CalcScaleFactor"/>.  Use a value of zero to
    /// hide the line (see <see cref="LineBase.IsVisible"/>).</param>
    public StickItem (string label, double[] x, double[] y, Color color, float lineWidth)
        : this (label, new PointPairList (x, y), color, lineWidth)
    {
    }

    /// <summary>
    /// Create a new <see cref="StickItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="x">An array of double precision values that define
    /// the independent (X axis) values for this curve</param>
    /// <param name="y">An array of double precision values that define
    /// the dependent (Y axis) values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    public StickItem (string label, double[] x, double[] y, Color color)
        : this (label, new PointPairList (x, y), color)
    {
    }

    /// <summary>
    /// Create a new <see cref="StickItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
    /// the X and Y values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    public StickItem (string label, IPointList points, Color color)
        : this (label, points, color, LineBase.Default.Width)
    {
    }

    /// <summary>
    /// Create a new <see cref="StickItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
    /// the X and Y values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    /// <param name="lineWidth">The width (in points) to be used for the <see cref="Line"/>.  This
    /// width is scaled based on <see cref="PaneBase.CalcScaleFactor"/>.  Use a value of zero to
    /// hide the line (see <see cref="LineBase.IsVisible"/>).</param>
    public StickItem (string label, IPointList points, Color color, float lineWidth)
        : base (label, points, color, Symbol.Default.Type, lineWidth)
    {
        _symbol.IsVisible = false;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="StickItem"/> object from which to copy</param>
    public StickItem (StickItem rhs) : base (rhs)
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
    public new StickItem Clone()
    {
        return new StickItem (this);
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
    protected StickItem
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema3").NotUsed();
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);
        info.AddValue ("schema3", schema3);
    }

    #endregion
}
