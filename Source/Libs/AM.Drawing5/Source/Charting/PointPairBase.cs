// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* PointPairBase.cs --
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
/// This is a base class that provides base-level functionality for a data point consisting
/// of an (X,Y) pair of double values.
/// </summary>
/// <remarks>
/// This class is typically a base class for actual <see cref="PointPair" /> type implementations.
/// </remarks>
[Serializable]
public class PointPairBase
    : ISerializable
{
    #region Member variables

    /// <summary>
    /// Missing values are represented internally using <see cref="System.Double.MaxValue"/>.
    /// </summary>
    public const double Missing = double.MaxValue;

    /// <summary>
    /// The default format to be used for displaying point values via the
    /// <see cref="ToString()"/> method.
    /// </summary>
    public const string DefaultFormat = "G";

    /// <summary>
    /// This PointPair's X coordinate
    /// </summary>
    public double X;

    /// <summary>
    /// This PointPair's Y coordinate
    /// </summary>
    public double Y;

    #endregion

    #region Constructors

    /// <summary>
    /// Default Constructor
    /// </summary>
    public PointPairBase()
        : this (0, 0)
    {
    }

    /// <summary>
    /// Creates a point pair with the specified X and Y.
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    public PointPairBase (double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Creates a point pair from the specified <see cref="PointF"/> struct.
    /// </summary>
    /// <param name="pt">The <see cref="PointF"/> struct from which to get the
    /// new <see cref="PointPair"/> values.</param>
    public PointPairBase (PointF pt)
        : this (pt.X, pt.Y)
    {
    }

    /// <summary>
    /// The PointPairBase copy constructor.
    /// </summary>
    /// <param name="rhs">The basis for the copy.</param>
    public PointPairBase (PointPairBase rhs)
    {
        X = rhs.X;
        Y = rhs.Y;
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema = 11;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected PointPairBase
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema").NotUsed();

        X = info.GetDouble ("X");
        Y = info.GetDouble ("Y");
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public virtual void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        info.AddValue ("schema", schema);
        info.AddValue ("X", X);
        info.AddValue ("Y", Y);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Readonly value that determines if either the X or the Y
    /// coordinate in this PointPair is a missing value.
    /// </summary>
    /// <returns>true if either value is missing</returns>
    public bool IsMissing
    {
        get { return X == Missing || Y == Missing; }
    }

    /// <summary>
    /// Readonly value that determines if either the X or the Y
    /// coordinate in this PointPair is an invalid (not plotable) value.
    /// It is considered invalid if it is missing (equal to System.Double.Max),
    /// Infinity, or NaN.
    /// </summary>
    /// <returns>true if either value is invalid</returns>
    public bool IsInvalid
    {
        get
        {
            return X == Missing ||
                   Y == Missing ||
                   double.IsInfinity (X) ||
                   double.IsInfinity (Y) ||
                   double.IsNaN (X) ||
                   double.IsNaN (Y);
        }
    }

    /// <summary>
    /// static method to determine if the specified point value is invalid.
    /// </summary>
    /// <remarks>The value is considered invalid if it is <see cref="PointPairBase.Missing"/>,
    /// <see cref="Double.PositiveInfinity"/>, <see cref="Double.NegativeInfinity"/>
    /// or <see cref="Double.NaN"/>.</remarks>
    /// <param name="value">The value to be checked for validity.</param>
    /// <returns>true if the value is invalid, false otherwise</returns>
    public static bool IsValueInvalid (double value)
    {
        return (value == Missing ||
                double.IsInfinity (value) ||
                double.IsNaN (value));
    }

    #endregion

    #region Operator Overloads

    /// <summary>
    /// Implicit conversion from PointPair to PointF.  Note that this conversion
    /// can result in data loss, since the data are being cast from a type
    /// double (64 bit) to a float (32 bit).
    /// </summary>
    /// <param name="pair">The PointPair struct on which to operate</param>
    /// <returns>A PointF struct equivalent to the PointPair</returns>
    public static implicit operator PointF (PointPairBase pair)
    {
        return new PointF ((float)pair.X, (float)pair.Y);
    }

    #endregion

    #region Methods

    ///<inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        var rhs = (PointPairBase) obj.ThrowIfNull();
        return X == rhs.X && Y == rhs.Y;
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        // ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
        return base.GetHashCode();
        // ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return ToString (DefaultFormat);
    }

    /// <summary>
    /// Format this PointPair value using a general format string.
    /// Example:  a format string of "e2" would give "( 1.23e+001, -1.69e+001 )".
    /// </summary>
    /// <param name="format">A format string that will be used to format each of
    /// the two double type values (see <see cref="System.Double.ToString()"/>).</param>
    /// <returns>A string representation of the PointPair</returns>
    public string ToString (string format)
    {
        return "( " + X.ToString (format) +
               ", " + Y.ToString (format) +
               " )";
    }

    /// <summary>
    /// Format this PointPair value using different general format strings for the X and Y values.
    /// Example:  a format string of "e2" would give "( 1.23e+001, -1.69e+001 )".
    /// The Z value is not displayed (see <see cref="PointPair.ToString( string, string, string )"/>).
    /// </summary>
    /// <param name="formatX">A format string that will be used to format the X
    /// double type value (see <see cref="System.Double.ToString()"/>).</param>
    /// <param name="formatY">A format string that will be used to format the Y
    /// double type value (see <see cref="System.Double.ToString()"/>).</param>
    /// <returns>A string representation of the PointPair</returns>
    public string ToString (string formatX, string formatY)
    {
        return "( " + X.ToString (formatX) +
               ", " + Y.ToString (formatY) +
               " )";
    }

    #endregion
}
