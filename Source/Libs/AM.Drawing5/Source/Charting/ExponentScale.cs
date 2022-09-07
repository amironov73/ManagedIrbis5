// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* ExponentScale.cs --
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
/// The ExponentScale class inherits from the <see cref="Scale" /> class, and implements
/// the features specific to <see cref="AxisType.Exponent" />.
/// </summary>
/// <remarks>
/// ExponentScale is a non-linear axis in which the values are scaled using an exponential function
/// with the <see cref="Scale.Exponent" /> property.
/// </remarks>
[Serializable]
class ExponentScale
    : Scale
{
    #region constructors

    /// <summary>
    /// Default constructor that defines the owner <see cref="Axis" />
    /// (containing object) for this new object.
    /// </summary>
    /// <param name="owner">The owner, or containing object, of this instance</param>
    public ExponentScale (Axis owner)
        : base (owner)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="ExponentScale" /> object from which to copy</param>
    /// <param name="owner">The <see cref="Axis" /> object that will own the
    /// new instance of <see cref="ExponentScale" /></param>
    public ExponentScale (Scale rhs, Axis owner)
        : base (rhs, owner)
    {
    }

    /// <inheritdoc cref="Scale.Clone"/>
    public override Scale Clone (Axis owner)
    {
        return new ExponentScale (this, owner);
    }

    #endregion

    #region properties

    /// <inheritdoc cref="Scale.Type"/>
    public override AxisType Type => AxisType.Exponent;

    #endregion

    #region methods

    /// <inheritdoc cref="Scale.SetupScaleData"/>
    public override void SetupScaleData (GraphPane pane, Axis axis)
    {
        base.SetupScaleData (pane, axis);

        if (_exponent > 0)
        {
            _minLinTemp = Linearize (_min);
            _maxLinTemp = Linearize (_max);
        }
        else if (_exponent < 0)
        {
            _minLinTemp = Linearize (_max);
            _maxLinTemp = Linearize (_min);
        }
    }

    /// <inheritdoc cref="Scale.Linearize"/>
    public override double Linearize (double val)
    {
        return SafeExp (val, _exponent);
    }

    /// <inheritdoc cref="Scale.DeLinearize"/>
    public override double DeLinearize (double val)
    {
        return Math.Pow (val, 1 / _exponent);
    }

    /// <inheritdoc cref="Scale.CalcMajorTicValue"/>
    internal override double CalcMajorTicValue (double baseVal, double tic)
    {
        if (_exponent > 0.0)
        {
            //return baseVal + Math.Pow ( (double) this.majorStep * tic, exp );
            //baseVal is got from CalBase..., and it is exp..
            return Math.Pow (Math.Pow (baseVal, 1 / _exponent) + _majorStep * tic, _exponent);
        }

        if (_exponent < 0.0)
        {
            //baseVal is got from CalBase..., and it is exp..
            return Math.Pow (Math.Pow (baseVal, 1 / _exponent) + _majorStep * tic, _exponent);
        }

        return 1.0;
    }

    /// <inheritdoc cref="Scale.CalcMinorTicValue"/>
    internal override double CalcMinorTicValue (double baseVal, int iTic)
    {
        return baseVal + Math.Pow (_majorStep * iTic, _exponent);
    }

    /// <inheritdoc cref="Scale.CalcMinorStart"/>
    internal override int CalcMinorStart (double baseVal)
    {
        return (int)((Math.Pow (_min, _exponent) - baseVal) / Math.Pow (_minorStep, _exponent));
    }

    /// <inheritdoc cref="Scale.PickScale"/>
    /// <seealso cref="Scale.PickScale"/>
    /// <seealso cref="AxisType.Exponent"/>
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
        if (_max - _min < 1.0e-20)
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

        // set the scale magnitude if required
        if (_magAuto)
        {
            // Find the optimal scale display multiple
            double mag = 0;
            double mag2 = 0;

            if (Math.Abs (_min) > 1.0e-10)
            {
                mag = Math.Floor (Math.Log10 (Math.Abs (_min)));
            }

            if (Math.Abs (_max) > 1.0e-10)
            {
                mag2 = Math.Floor (Math.Log10 (Math.Abs (_max)));
            }

            if (Math.Abs (mag2) > Math.Abs (mag))
            {
                mag = mag2;
            }

            // Do not use scale multiples for magnitudes below 4
            if (Math.Abs (mag) <= 3)
            {
                mag = 0;
            }

            // Use a power of 10 that is a multiple of 3 (engineering scale)
            _mag = (int)(Math.Floor (mag / 3.0) * 3.0);
        }

        // Calculate the appropriate number of dec places to display if required
        if (_formatAuto)
        {
            var numDec = 0 - (int)(Math.Floor (Math.Log10 (_majorStep)) - _mag);
            if (numDec < 0)
            {
                numDec = 0;
            }

            _format = "f" + numDec;
        }
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

        var scaleMult = Math.Pow (10.0, _mag);
        var val = Math.Pow (dVal, 1 / _exponent) / scaleMult;
        return val.ToString (_format);
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
    protected ExponentScale
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
