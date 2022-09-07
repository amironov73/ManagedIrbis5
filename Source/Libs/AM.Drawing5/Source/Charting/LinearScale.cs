// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* LinearScale.cs --
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
/// The LinearScale class inherits from the <see cref="Scale" /> class, and implements
/// the features specific to <see cref="AxisType.Linear" />.
/// </summary>
/// <remarks>
/// LinearScale is the normal, default cartesian axis.
/// </remarks>
[Serializable]
class LinearScale
    : Scale
{
    #region constructors

    /// <summary>
    /// Default constructor that defines the owner <see cref="Axis" />
    /// (containing object) for this new object.
    /// </summary>
    /// <param name="owner">The owner, or containing object, of this instance</param>
    public LinearScale (Axis owner)
        : base (owner)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="LinearScale" /> object from which to copy</param>
    /// <param name="owner">The <see cref="Axis" /> object that will own the
    /// new instance of <see cref="LinearScale" /></param>
    public LinearScale (Scale rhs, Axis owner)
        : base (rhs, owner)
    {
    }


    /// <inheritdoc cref="Scale.Clone"/>
    public override Scale Clone (Axis owner)
    {
        return new LinearScale (this, owner);
    }

    #endregion

    #region properties

    /// <inheritdoc cref="Scale.Type"/>
    public override AxisType Type => AxisType.Linear;

    #endregion

    #region methods

    /// <inheritdoc cref="Scale.PickScale"/>
    /// <seealso cref="PickScale"/>
    /// <seealso cref="AxisType.Linear"/>
    public override void PickScale
        (
            GraphPane pane,
            Graphics graphics,
            float scaleFactor
        )
    {
        // call the base class first
        base.PickScale (pane, graphics, scaleFactor);

        // Test for trivial condition of range = 0 and pick a suitable default
        if (_max - _min < 1.0e-30)
        {
            if (_maxAuto)
            {
                _max += 0.2 * (_max == 0 ? 1.0 : Math.Abs (_max));
            }

            if (_minAuto)
            {
                _min -= 0.2 * (_min == 0 ? 1.0 : Math.Abs (_min));
            }
        }

        // This is the zero-lever test.  If minVal is within the zero lever fraction
        // of the data range, then use zero.

        if (_minAuto && _min > 0 &&
            _min / (_max - _min) < Default.ZeroLever)
        {
            _min = 0;
        }

        // Repeat the zero-lever test for cases where the maxVal is less than zero
        if (_maxAuto && _max < 0 &&
            Math.Abs (_max / (_max - _min)) <
            Default.ZeroLever)
        {
            _max = 0;
        }

        // Calculate the new step size
        if (_majorStepAuto)
        {
            var targetSteps = (_ownerAxis is XAxis || _ownerAxis is X2Axis)
                ? Default.TargetXSteps
                : Default.TargetYSteps;

            // Calculate the step size based on target steps
            _majorStep = CalcStepSize (_max - _min, targetSteps);

            if (_isPreventLabelOverlap)
            {
                // Calculate the maximum number of labels
                double maxLabels = CalcMaxLabels (graphics, pane, scaleFactor);

                if (maxLabels < (_max - _min) / _majorStep)
                {
                    _majorStep = CalcBoundedStepSize (_max - _min, maxLabels);
                }
            }
        }

        // Calculate the new step size
        if (_minorStepAuto)
        {
            _minorStep = CalcStepSize (_majorStep,
                (_ownerAxis is XAxis || _ownerAxis is X2Axis)
                    ? Default.TargetMinorXSteps
                    : Default.TargetMinorYSteps);
        }

        // Calculate the scale minimum
        if (_minAuto)
        {
            _min = _min - MyMod (_min, _majorStep);
        }

        // Calculate the scale maximum
        if (_maxAuto)
        {
            _max = MyMod (_max, _majorStep) == 0.0 ? _max : _max + _majorStep - MyMod (_max, _majorStep);
        }

        SetScaleMag (_min, _max, _majorStep);
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
    protected LinearScale
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
