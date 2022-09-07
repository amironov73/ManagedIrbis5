// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* LogScale.cs --
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
/// The LogScale class inherits from the <see cref="Scale" /> class, and implements
/// the features specific to <see cref="AxisType.Log" />.
/// </summary>
/// <remarks>
/// LogScale is a non-linear axis in which the values are scaled using the base 10
/// <see cref="Math.Log(double)" />
/// function.
/// </remarks>
[Serializable]
class LogScale
    : Scale
{
    #region constructors

    /// <summary>
    /// Default constructor that defines the owner <see cref="Axis" />
    /// (containing object) for this new object.
    /// </summary>
    /// <param name="owner">The owner, or containing object, of this instance</param>
    public LogScale (Axis owner)
        : base (owner)
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="LogScale" /> object from which to copy</param>
    /// <param name="owner">The <see cref="Axis" /> object that will own the
    /// new instance of <see cref="LogScale" /></param>
    public LogScale (Scale rhs, Axis owner)
        : base (rhs, owner)
    {
    }

    /// <inheritdoc cref="Scale.Clone"/>
    public override Scale Clone (Axis owner)
    {
        return new LogScale (this, owner);
    }

    #endregion

    #region properties

    /// <inheritdoc cref="Scale.Type"/>
    public override AxisType Type => AxisType.Log;

    /// <inheritdoc cref="Scale.Min"/>
    public override double Min
    {
        get { return _min; }
        set
        {
            if (value > 0)
            {
                _min = value;
            }
        }
    }

    /// <inheritdoc cref="Scale.Max"/>
    public override double Max
    {
        get { return _max; }
        set
        {
            if (value > 0)
            {
                _max = value;
            }
        }
    }

    #endregion

    #region methods

    /// <inheritdoc cref="Scale.SetupScaleData"/>
    public override void SetupScaleData
        (
            GraphPane pane,
            Axis axis
        )
    {
        base.SetupScaleData (pane, axis);

        _minLinTemp = Linearize (_min);
        _maxLinTemp = Linearize (_max);
    }

    /// <inheritdoc cref="Scale.Linearize"/>
    public override double Linearize (double val)
    {
        return SafeLog (val);
    }

    /// <inheritdoc cref="Scale.DeLinearize"/>
    public override double DeLinearize (double val)
    {
        return Math.Pow (10.0, val);
    }

    /// <inheritdoc cref="Scale.CalcMajorTicValue"/>
    internal override double CalcMajorTicValue (double baseVal, double tic)
    {
        return baseVal + tic * CyclesPerStep;

        //	double val = baseVal + (double)tic * CyclesPerStep;
        //	double frac = val - Math.Floor( val );
    }

    /// <inheritdoc cref="Scale.CalcMinorTicValue"/>
    internal override double CalcMinorTicValue (double baseVal, int iTic)
    {
        double[] dLogVal =
        {
            0, 0.301029995663981, 0.477121254719662, 0.602059991327962,
            0.698970004336019, 0.778151250383644, 0.845098040014257,
            0.903089986991944, 0.954242509439325, 1
        };

        return baseVal + Math.Floor (iTic / 9.0) + dLogVal[(iTic + 9) % 9];
    }

    /// <inheritdoc cref="Scale.CalcMinorStart"/>
    internal override int CalcMinorStart (double baseVal)
    {
        return -9;
    }

    /// <inheritdoc cref="Scale.CalcBaseTic"/>
    internal override double CalcBaseTic()
    {
        if (_baseTic != PointPairBase.Missing)
        {
            return _baseTic;
        }
        else
        {
            // go to the nearest even multiple of the step size
            return Math.Ceiling (SafeLog (_min) - 0.00000001);
        }
    }

    /// <inheritdoc cref="Scale.CalcNumTics"/>
    internal override int CalcNumTics()
    {
        var nTics = 1;

        //iStart = (int) ( Math.Ceiling( SafeLog( this.min ) - 1.0e-12 ) );
        //iEnd = (int) ( Math.Floor( SafeLog( this.max ) + 1.0e-12 ) );

        //nTics = (int)( ( Math.Floor( Scale.SafeLog( _max ) + 1.0e-12 ) ) -
        //		( Math.Ceiling( Scale.SafeLog( _min ) - 1.0e-12 ) ) + 1 ) / CyclesPerStep;
        nTics = (int)((SafeLog (_max) - SafeLog (_min)) / CyclesPerStep) + 1;

        if (nTics < 1)
        {
            nTics = 1;
        }
        else if (nTics > 1000)
        {
            nTics = 1000;
        }

        return nTics;
    }

    private double CyclesPerStep
    {
        //get { return (int)Math.Max( Math.Floor( Scale.SafeLog( _majorStep ) ), 1 ); }
        get { return _majorStep; }
    }

    /// <inheritdoc cref="Scale.PickScale"/>
    /// <seealso cref="PickScale"/>
    /// <seealso cref="AxisType.Log"/>
    public override void PickScale
        (
            GraphPane pane,
            Graphics graphics,
            float scaleFactor
        )
    {
        // call the base class first
        base.PickScale (pane, graphics, scaleFactor);

        // Majorstep is always 1 for log scales
        if (_majorStepAuto)
        {
            _majorStep = 1.0;
        }

        _mag = 0; // Never use a magnitude shift for log scales

        //this.numDec = 0;		// The number of decimal places to display is not used

        // Check for bad data range
        if (_min <= 0.0 && _max <= 0.0)
        {
            _min = 1.0;
            _max = 10.0;
        }
        else if (_min <= 0.0)
        {
            _min = _max / 10.0;
        }
        else if (_max <= 0.0)
        {
            _max = _min * 10.0;
        }

        // Test for trivial condition of range = 0 and pick a suitable default
        if (_max - _min < 1.0e-20)
        {
            if (_maxAuto)
            {
                _max = _max * 2.0;
            }

            if (_minAuto)
            {
                _min = _min / 2.0;
            }
        }

        // Get the nearest power of 10 (no partial log cycles allowed)
        if (_minAuto)
        {
            _min = Math.Pow (10.0,
                Math.Floor (Math.Log10 (_min)));
        }

        if (_maxAuto)
        {
            _max = Math.Pow (10.0,
                Math.Ceiling (Math.Log10 (_max)));
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

        return _isUseTenPower
            ? $"{dVal:F0}"
            : Math.Pow (10.0, dVal).ToString (_format);
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
    protected LogScale
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
