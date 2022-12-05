// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable InconsistentNaming

/* TextScale.cs --
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
/// The TextScale class inherits from the <see cref="Scale" /> class, and implements
/// the features specific to <see cref="AxisType.Text" />.
/// </summary>
/// <remarks>
/// TextScale is an ordinal axis with user-defined text labels.  An ordinal axis means that
/// all data points are evenly spaced at integral values, and the actual coordinate values
/// for points corresponding to that axis are ignored.  That is, if the X axis is an
/// ordinal type, then all X values associated with the curves are ignored.
/// </remarks>
[Serializable]
public class TextScale
    : Scale
{
    #region constructors

    /// <summary>
    ///
    /// </summary>
    /// <param name="owner"></param>
    public TextScale
        (
            Axis owner
        )
        : base (owner)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="TextScale" /> object from which to copy</param>
    /// <param name="owner">The <see cref="Axis" /> object that will own the
    /// new instance of <see cref="TextScale" /></param>
    public TextScale
        (
            Scale rhs,
            Axis owner
        )
        : base (rhs, owner)
    {
        // пустое тело конструктора
    }

    /// <inheritdoc cref="Scale.Clone"/>
    public override Scale Clone
        (
            Axis owner
        )
    {
        return new TextScale (this, owner);
    }

    #endregion

    #region properties

    /// <inheritdoc cref="Scale.Type"/>
    public override AxisType Type => AxisType.Text;

    #endregion

    #region Methods

    /// <inheritdoc cref="Scale.CalcMinorStart"/>
    internal override int CalcMinorStart
        (
            double baseVal
        )
    {
        // This should never happen (no minor tics for text labels)
        return 0;
    }

    /// <inheritdoc cref="Scale.CalcBaseTic"/>
    internal override double CalcBaseTic()
    {
        return _baseTic != PointPairBase.Missing ? _baseTic : 1.0;
    }

    /// <inheritdoc cref="Scale.CalcNumTics"/>
    internal override int CalcNumTics()
    {
        // If no array of labels is available, just assume 10 labels so we don't blow up.
        var nTics = Math.Clamp (_textLabels?.Length ?? 10, 1, 1000);

        return nTics;
    }

    /// <inheritdoc cref="Scale.PickScale"/>
    public override void PickScale
        (
            GraphPane pane,
            Graphics graphics,
            float scaleFactor
        )
    {
        // call the base class first
        base.PickScale (pane, graphics, scaleFactor);

        // if text labels are provided, then autorange to the number of labels
        if (_textLabels != null)
        {
            if (_minAuto)
            {
                _min = 0.5;
            }

            if (_maxAuto)
            {
                _max = _textLabels.Length + 0.5;
            }
        }
        else
        {
            if (_minAuto)
            {
                _min -= 0.5;
            }

            if (_maxAuto)
            {
                _max += 0.5;
            }
        }

        // Test for trivial condition of range = 0 and pick a suitable default
        if (_max - _min < .1)
        {
            if (_maxAuto)
            {
                _max = _min + 10.0;
            }
            else
            {
                _min = _max - 10.0;
            }
        }

        if (_majorStepAuto)
        {
            if (!_isPreventLabelOverlap)
            {
                _majorStep = 1;
            }
            else if (_textLabels != null)
            {
                // Calculate the maximum number of labels
                var maxLabels = (double)CalcMaxLabels (graphics, pane, scaleFactor);

                // Calculate a step size based on the width of the labels
                var tmpStep = Math.Ceiling ((_max - _min) / maxLabels);

                // Use the lesser of the two step sizes
                //if ( tmpStep < this.majorStep )
                _majorStep = tmpStep;
            }
            else
            {
                _majorStep = (int)((_max - _min - 1.0) / Default.MaxTextLabels) + 1.0;
            }
        }
        else
        {
            _majorStep = (int)_majorStep;
            if (_majorStep <= 0)
            {
                _majorStep = 1.0;
            }
        }

        if (_minorStepAuto)
        {
            _minorStep = _majorStep / 10;

            //_minorStep = CalcStepSize( _majorStep, 10 );
            if (_minorStep < 1)
            {
                _minorStep = 1;
            }
        }

        _mag = 0;
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

        index *= (int)_majorStep;
        return _textLabels is null || index < 0 || index >= _textLabels.Length
            ? string.Empty
            : _textLabels[index];
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
    protected TextScale
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        var sch = info.GetInt32 ("schema2");
        sch.NotUsed();
    }

    /// <inheritdoc cref="Scale.GetObjectData"/>
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
