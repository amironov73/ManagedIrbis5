// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* LinearAsOrdinalScale.cs --
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
/// The LinearAsOrdinalScale class inherits from the <see cref="Scale" /> class, and implements
/// the features specific to <see cref="AxisType.LinearAsOrdinal" />.
/// </summary>
/// <remarks>
/// LinearAsOrdinal is an ordinal axis that will have labels formatted with values from the actual data
/// values of the first <see cref="CurveItem" /> in the <see cref="CurveList" />.
/// Although the tics are labeled with real data values, the actual points will be
/// evenly-spaced in spite of the data values.  For example, if the X values of the first curve
/// are 1, 5, and 100, then the tic labels will show 1, 5, and 100, but they will be equal
/// distance from each other.
/// </remarks>
[Serializable]
class LinearAsOrdinalScale
    : Scale
{
    #region constructors

    /// <summary>
    /// Default constructor that defines the owner <see cref="Axis" />
    /// (containing object) for this new object.
    /// </summary>
    /// <param name="owner">The owner, or containing object, of this instance</param>
    public LinearAsOrdinalScale (Axis owner)
        : base (owner)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="LinearAsOrdinalScale" /> object from which to copy</param>
    /// <param name="owner">The <see cref="Axis" /> object that will own the
    /// new instance of <see cref="LinearAsOrdinalScale" /></param>
    public LinearAsOrdinalScale (Scale rhs, Axis owner)
        : base (rhs, owner)
    {
    }


    /// <inheritdoc cref="Scale.Clone"/>
    public override Scale Clone (Axis owner)
    {
        return new LinearAsOrdinalScale (this, owner);
    }

    #endregion

    #region properties

    /// <inheritdoc cref="Scale.Type"/>
    public override AxisType Type
    {
        get { return AxisType.LinearAsOrdinal; }
    }

    #endregion

    #region methods

    /// <inheritdoc cref="Scale.PickScale"/>
    /// <seealso cref="PickScale"/>
    /// <seealso cref="AxisType.Ordinal"/>
    public override void PickScale
        (
            GraphPane pane,
            Graphics graphics,
            float scaleFactor
        )
    {
        // call the base class first
        base.PickScale (pane, graphics, scaleFactor);

        // First, get the date ranges from the first curve in the list
        double xMin; // = Double.MaxValue;
        double xMax; // = Double.MinValue;
        double yMin; // = Double.MaxValue;
        double yMax; // = Double.MinValue;
        double tMin = 0;
        double tMax = 1;

        foreach (var curve in pane.CurveList)
        {
            if ((_ownerAxis is Y2Axis && curve.IsY2Axis) ||
                (_ownerAxis is YAxis && !curve.IsY2Axis) ||
                (_ownerAxis is X2Axis && curve.IsX2Axis) ||
                (_ownerAxis is XAxis && !curve.IsX2Axis))
            {
                curve.GetRange (out xMin, out xMax, out yMin, out yMax, false, false, pane);
                if (_ownerAxis is XAxis || _ownerAxis is X2Axis)
                {
                    tMin = xMin;
                    tMax = xMax;
                }
                else
                {
                    tMin = yMin;
                    tMax = yMax;
                }
            }
        }

        var range = Math.Abs (tMax - tMin);

        // Now, set the axis range based on a ordinal scale
        base.PickScale (pane, graphics, scaleFactor);
        OrdinalScale.PickScale (pane, graphics, scaleFactor, this);

        SetScaleMag (tMin, tMax, range / Default.TargetXSteps);
    }

    /// <inheritdoc cref="Scale.MakeLabel"/>
    internal override string MakeLabel
        (
            GraphPane pane,
            int index,
            double dVal
        )
    {
        _format ??= Default.Format;

        var tmpIndex = (int)dVal - 1;

        if (pane.CurveList.Count > 0 && pane.CurveList[0].Points.Count > tmpIndex)
        {
            var val = pane.CurveList[0].Points[tmpIndex].X;
            var scaleMult = Math.Pow (10.0, _mag);
            return (val / scaleMult).ToString (_format);
        }

        return string.Empty;
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
    protected LinearAsOrdinalScale
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
}
