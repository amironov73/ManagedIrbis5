// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* DateAndOrdinalScale.cs --
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
/// The DateAsOrdinalScale class inherits from the <see cref="Scale" /> class, and implements
/// the features specific to <see cref="AxisType.DateAsOrdinal" />.
/// </summary>
/// <remarks>DateAsOrdinalScale is an ordinal axis that will have labels formatted with dates from the
/// actual data values of the first <see cref="CurveItem" /> in the <see cref="CurveList" />.
/// Although the tics are labeled with real data values, the actual points will be
/// evenly-spaced in spite of the data values.  For example, if the X values of the first curve
/// are 1, 5, and 100, then the tic labels will show 1, 5, and 100, but they will be equal
/// distance from each other.
/// </remarks>
[Serializable]
class DateAsOrdinalScale
    : Scale
{
    #region constructors

    /// <summary>
    /// Default constructor that defines the owner <see cref="Axis" />
    /// (containing object) for this new object.
    /// </summary>
    /// <param name="owner">The owner, or containing object, of this instance</param>
    public DateAsOrdinalScale
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
    /// <param name="rhs">The <see cref="DateAsOrdinalScale" /> object from which to copy</param>
    /// <param name="owner">The <see cref="Axis" /> object that will own the
    /// new instance of <see cref="DateAsOrdinalScale" /></param>
    public DateAsOrdinalScale
        (
            Scale rhs,
            Axis owner
        )
        : base (rhs, owner)
    {
        // пустое тело конструктора
    }

    /// <inheritdoc cref="ICloneable.Clone"/>
    public override Scale Clone (Axis owner)
    {
        return new DateAsOrdinalScale (this, owner);
    }

    #endregion

    #region properties

    /// <inheritdoc cref="Scale.Type"/>
    public override AxisType Type => AxisType.DateAsOrdinal;

    /// <inheritdoc cref="Scale.Min"/>
    public override double Min
    {
        get => _min;
        set
        {
            _min = XDate.MakeValidDate (value);
            _minAuto = false;
        }
    }

    /// <inheritdoc cref="Scale.Max"/>
    public override double Max
    {
        get => _max;
        set
        {
            _max = XDate.MakeValidDate (value);
            _maxAuto = false;
        }
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

/*			// First, get the date ranges from the first curve in the list
			double xMin; // = Double.MaxValue;
			double xMax; // = Double.MinValue;
			double yMin; // = Double.MaxValue;
			double yMax; // = Double.MinValue;
			double range = 1;

			foreach ( CurveItem curve in pane.CurveList )
			{
				if ( ( _ownerAxis is Y2Axis && curve.IsY2Axis ) ||
						( _ownerAxis is YAxis && !curve.IsY2Axis ) ||
						( _ownerAxis is X2Axis && curve.IsX2Axis ) ||
						( _ownerAxis is XAxis && !curve.IsX2Axis ) )
				{
					curve.GetRange( out xMin, out xMax, out yMin, out yMax, false, pane.IsBoundedRanges, pane );
					if ( _ownerAxis is XAxis || _ownerAxis is X2Axis )
						range = xMax - xMin;
					else
						range = yMax - yMin;
				}
			}
*/
        // Set the DateFormat by calling CalcDateStepSize
        //			DateScale.CalcDateStepSize( range, Default.TargetXSteps, this );
        SetDateFormat (pane);

        // Now, set the axis range based on a ordinal scale
        base.PickScale (pane, graphics, scaleFactor);
        OrdinalScale.PickScale (pane, graphics, scaleFactor, this);
    }

    internal void SetDateFormat
        (
            GraphPane pane
        )
    {
        if (_formatAuto)
        {
            double range = 10;

            if (pane.CurveList.Count > 0 && pane.CurveList[0].Points.Count > 1)
            {
                double val1, val2;

                PointPair pt1 = pane.CurveList[0].Points[0];
                PointPair pt2 = pane.CurveList[0].Points[pane.CurveList[0].Points.Count - 1];
                int p1 = 1;
                int p2 = pane.CurveList[0].Points.Count;
                if (pane.IsBoundedRanges)
                {
                    p1 = (int)Math.Floor (_ownerAxis.Scale.Min);
                    p2 = (int)Math.Ceiling (_ownerAxis.Scale.Max);
                    p1 = Math.Min (Math.Max (p1, 1), pane.CurveList[0].Points.Count);
                    p2 = Math.Min (Math.Max (p2, 1), pane.CurveList[0].Points.Count);
                    if (p2 > p1)
                    {
                        pt1 = pane.CurveList[0].Points[p1 - 1];
                        pt2 = pane.CurveList[0].Points[p2 - 1];
                    }
                }

                if (_ownerAxis is XAxis || _ownerAxis is X2Axis)
                {
                    val1 = pt1.X;
                    val2 = pt2.X;
                }
                else
                {
                    val1 = pt1.Y;
                    val2 = pt2.Y;
                }

                if (val1 != PointPairBase.Missing &&
                    val2 != PointPairBase.Missing &&
                    !double.IsNaN (val1) &&
                    !double.IsNaN (val2) &&
                    !double.IsInfinity (val1) &&
                    !double.IsInfinity (val2) &&
                    Math.Abs (val2 - val1) > 1e-10)
                {
                    range = Math.Abs (val2 - val1);
                }
            }

            if (range > Default.RangeYearYear)
            {
                _format = Default.FormatYearYear;
            }
            else if (range > Default.RangeYearMonth)
            {
                _format = Default.FormatYearMonth;
            }
            else if (range > Default.RangeMonthMonth)
            {
                _format = Default.FormatMonthMonth;
            }
            else if (range > Default.RangeDayDay)
            {
                _format = Default.FormatDayDay;
            }
            else if (range > Default.RangeDayHour)
            {
                _format = Default.FormatDayHour;
            }
            else if (range > Default.RangeHourHour)
            {
                _format = Default.FormatHourHour;
            }
            else if (range > Default.RangeHourMinute)
            {
                _format = Default.FormatHourMinute;
            }
            else if (range > Default.RangeMinuteMinute)
            {
                _format = Default.FormatMinuteMinute;
            }
            else if (range > Default.RangeMinuteSecond)
            {
                _format = Default.FormatMinuteSecond;
            }
            else if (range > Default.RangeSecondSecond)
            {
                _format = Default.FormatSecondSecond;
            }
            else // MilliSecond
            {
                _format = Default.FormatMillisecond;
            }
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
        if (_format == null)
        {
            _format = Default.Format;
        }

        double val;

        int tmpIndex = (int)dVal - 1;

        if (pane.CurveList.Count > 0 && pane.CurveList[0].Points.Count > tmpIndex)
        {
            if (_ownerAxis is XAxis || _ownerAxis is X2Axis)
            {
                val = pane.CurveList[0].Points[tmpIndex].X;
            }
            else
            {
                val = pane.CurveList[0].Points[tmpIndex].Y;
            }

            return XDate.ToString (val, _format);
        }
        else
        {
            return string.Empty;
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
    protected DateAsOrdinalScale
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
