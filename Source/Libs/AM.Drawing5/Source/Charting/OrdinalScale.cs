// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* OrdinalScale.cs --
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
/// The OrdinalScale class inherits from the <see cref="Scale" />
/// class, and implements
/// the features specific to <see cref="AxisType.Ordinal" />.
/// </summary>
/// <remarks>
/// OrdinalScale is an ordinal axis with tic labels generated at
/// integral values.  An ordinal axis means that
/// all data points are evenly spaced at integral values, and the
/// actual coordinate values
/// for points corresponding to that axis are ignored.  That is,
/// if the X axis is an
/// ordinal type, then all X values associated with the curves
/// are ignored.
/// </remarks>
[Serializable]
internal class OrdinalScale
    : Scale
{
    #region Construction

    /// <summary>
    /// Default constructor that defines the owner <see cref="Axis" />
    /// (containing object) for this new object.
    /// </summary>
    /// <param name="owner">The owner, or containing object, of this instance</param>
    public OrdinalScale (Axis owner)
        : base (owner)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="OrdinalScale" /> object from which to copy</param>
    /// <param name="owner">The <see cref="Axis" /> object that will own the
    /// new instance of <see cref="OrdinalScale" /></param>
    public OrdinalScale (Scale rhs, Axis owner)
        : base (rhs, owner)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Create a new clone of the current item, with a new owner assignment
    /// </summary>
    /// <param name="owner">The new <see cref="Axis" /> instance that will be
    /// the owner of the new Scale</param>
    /// <returns>A new <see cref="Scale" /> clone.</returns>
    public override Scale Clone (Axis owner)
    {
        return new OrdinalScale (this, owner);
    }

    #region Properties

    /// <inheritdoc cref="Scale.Type"/>
    public override AxisType Type => AxisType.Ordinal;

    #endregion

    #region methods

    /// <inheritdoc cref="Scale.PickScale"/>
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

        PickScale (pane, graphics, scaleFactor, this);
    }

    internal static void PickScale
        (
            GraphPane pane,
            Graphics graphics,
            float scaleFactor,
            Scale scale
        )
    {
        // Test for trivial condition of range = 0 and pick a suitable default
        if (scale._max - scale._min < 1.0)
        {
            if (scale._maxAuto)
            {
                scale._max = scale._min + 0.5;
            }
            else
            {
                scale._min = scale._max - 0.5;
            }
        }
        else
        {
            // Calculate the new step size
            if (scale._majorStepAuto)
            {
                // Calculate the step size based on targetSteps
                scale._majorStep = CalcStepSize (scale._max - scale._min,
                    (scale._ownerAxis is XAxis || scale._ownerAxis is X2Axis)
                        ? Default.TargetXSteps
                        : Default.TargetYSteps);

                if (scale.IsPreventLabelOverlap)
                {
                    // Calculate the maximum number of labels
                    var maxLabels = (double)scale.CalcMaxLabels (graphics, pane, scaleFactor);

                    // Calculate a step size based on the width of the labels
                    var tmpStep = Math.Ceiling ((scale._max - scale._min) / maxLabels);

                    // Use the greater of the two step sizes
                    if (tmpStep > scale._majorStep)
                    {
                        scale._majorStep = tmpStep;
                    }
                }
            }

            scale._majorStep = (int)scale._majorStep;
            if (scale._majorStep < 1.0)
            {
                scale._majorStep = 1.0;
            }

            // Calculate the new minor step size
            if (scale._minorStepAuto)
            {
                scale._minorStep = CalcStepSize (scale._majorStep,
                    (scale._ownerAxis is XAxis || scale._ownerAxis is X2Axis)
                        ? Default.TargetMinorXSteps
                        : Default.TargetMinorYSteps);
            }

            if (scale._minAuto)
            {
                scale._min -= 0.5;
            }

            if (scale._maxAuto)
            {
                scale._max += 0.5;
            }
        }
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
    protected OrdinalScale
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
