// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* HiLowBarItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Encapsulates an "High-Low" Bar curve type that displays a bar in which both
/// the bottom and the top of the bar are set by data valuesfrom the
/// <see cref="PointPair"/> struct.
/// </summary>
/// <remarks>The <see cref="HiLowBarItem"/> type is intended for displaying
/// bars that cover a band of data, such as a confidence interval, "waterfall"
/// chart, etc.  The position of each bar is set
/// according to the <see cref="PointPair"/> values.  The independent axis
/// is assigned with <see cref="BarSettings.Base"/>, and is a
/// <see cref="BarBase"/> enum type.  If <see cref="BarSettings.Base"/>
/// is set to <see cref="BarBase.Y"/> or <see cref="BarBase.Y2"/>, then
/// the bars will actually be horizontal, since the X axis becomes the
/// value axis and the Y or Y2 axis becomes the independent axis.</remarks>
/// <author> John Champion </author>
/// <version> $Revision: 3.18 $ $Date: 2007-11-03 04:41:28 $ </version>
[Serializable]
public class HiLowBarItem
    : BarItem, ICloneable
{
    #region Constructors

    /// <summary>
    /// Create a new <see cref="HiLowBarItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="x">An array of double precision values that define
    /// the independent (X axis) values for this curve</param>
    /// <param name="y">An array of double precision values that define
    /// the dependent (Y axis) values for this curve</param>
    /// <param name="baseVal">An array of double precision values that define the
    /// base value (the bottom) of the bars for this curve.
    /// </param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Bar.Fill"/> and <see cref="Bar.Border"/> properties.
    /// </param>
    public HiLowBarItem (string label, double[] x, double[] y, double[] baseVal, Color color) :
        this (label, new PointPairList (x, y, baseVal), color)
    {
    }

    /// <summary>
    /// Create a new <see cref="HiLowBarItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The label that will appear in the legend.</param>
    /// <param name="points">A <see cref="IPointList"/> of double precision value trio's that define
    /// the X, Y, and lower dependent values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Bar.Fill"/> and <see cref="Bar.Border"/> properties.
    /// </param>
    public HiLowBarItem (string label, IPointList points, Color color)
        : base (label, points, color)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="HiLowBarItem"/> object from which to copy</param>
    public HiLowBarItem (HiLowBarItem rhs) : base (rhs)
    {
        _bar = rhs._bar.Clone(); // new HiLowBar( rhs.Bar );
    }

    /// <summary>
    /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
    /// calling the typed version of <see cref="Clone" />
    /// </summary>
    /// <returns>A deep copy of this object</returns>
    object ICloneable.Clone()
    {
        return this.Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    new public HiLowBarItem Clone()
    {
        return new HiLowBarItem (this);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema3 = 11;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected HiLowBarItem (SerializationInfo info, StreamingContext context) : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        int sch = info.GetInt32 ("schema3");
    }

    /// <summary>
    /// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
    [SecurityPermission (SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData (SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData (info, context);
    }

    #endregion
}
