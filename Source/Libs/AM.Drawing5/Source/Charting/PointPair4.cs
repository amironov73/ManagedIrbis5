// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* PointPair4.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// The basic <see cref="PointPair" /> class holds three data values (X, Y, Z).  This
/// class extends the basic PointPair to contain four data values (X, Y, Z, T).
/// </summary>
[Serializable]
public class PointPair4
    : PointPair
{
    #region Member variables

    /// <summary>
    /// This PointPair4's T coordinate.
    /// </summary>
    public double T;

    #endregion

    #region Constructors

    /// <summary>
    /// Default Constructor
    /// </summary>
    public PointPair4() : base()
    {
        T = 0;
    }

    /// <summary>
    /// Creates a point pair with the specified X, Y, Z, and T value.
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    /// <param name="z">This pair's z coordinate.</param>
    /// <param name="t">This pair's t coordinate.</param>
    public PointPair4 (double x, double y, double z, double t) : base (x, y, z)
    {
        T = t;
    }

    /// <summary>
    /// Creates a point pair with the specified X, Y, base value, and
    /// label (<see cref="PointPair.Tag"/>).
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    /// <param name="z">This pair's z coordinate.</param>
    /// <param name="t">This pair's t coordinate.</param>
    /// <param name="label">This pair's string label (<see cref="PointPair.Tag"/>)</param>
    public PointPair4 (double x, double y, double z, double t, string label) :
        base (x, y, z, label)
    {
        T = t;
    }

    /// <summary>
    /// The PointPair4 copy constructor.
    /// </summary>
    /// <param name="rhs">The basis for the copy.</param>
    public PointPair4 (PointPair4 rhs) : base (rhs)
    {
        T = rhs.T;
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
    protected PointPair4
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema3").NotUsed();

        T = info.GetDouble ("T");
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);
        info.AddValue ("schema2", schema3);
        info.AddValue ("T", T);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Readonly value that determines if either the X, Y, Z, or T
    /// coordinate in this PointPair4 is an invalid (not plotable) value.
    /// It is considered invalid if it is missing (equal to System.Double.Max),
    /// Infinity, or NaN.
    /// </summary>
    /// <returns>true if any value is invalid</returns>
    public bool IsInvalid4D
    {
        get
        {
            return X == Missing ||
                   Y == Missing ||
                   Z == Missing ||
                   T == Missing ||
                   double.IsInfinity (X) ||
                   double.IsInfinity (Y) ||
                   double.IsInfinity (Z) ||
                   double.IsInfinity (T) ||
                   double.IsNaN (X) ||
                   double.IsNaN (Y) ||
                   double.IsNaN (Z) ||
                   double.IsNaN (T);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Format this PointPair4 value using the default format.  Example:  "( 12.345, -16.876 )".
    /// The two double values are formatted with the "g" format type.
    /// </summary>
    /// <param name="isShowZT">true to show the third "Z" and fourth "T" value coordinates</param>
    /// <returns>A string representation of the PointPair4</returns>
    public new string ToString (bool isShowZT)
    {
        return ToString (DefaultFormat, isShowZT);
    }

    /// <summary>
    /// Format this PointPair value using a general format string.
    /// Example:  a format string of "e2" would give "( 1.23e+001, -1.69e+001 )".
    /// If <see paramref="isShowZ"/>
    /// is true, then the third "Z" coordinate is also shown.
    /// </summary>
    /// <param name="format">A format string that will be used to format each of
    /// the two double type values (see <see cref="System.Double.ToString()"/>).</param>
    /// <returns>A string representation of the PointPair</returns>
    /// <param name="isShowZT">true to show the third "Z" or low dependent value coordinate</param>
    public new string ToString (string format, bool isShowZT)
    {
        return "( " + X.ToString (format) +
               ", " + Y.ToString (format) +
               (isShowZT
                   ? (", " + Z.ToString (format) +
                      ", " + T.ToString (format))
                   : "") + " )";
    }

    /// <summary>
    /// Format this PointPair value using different general format strings for the X, Y, and Z values.
    /// Example:  a format string of "e2" would give "( 1.23e+001, -1.69e+001 )".
    /// </summary>
    /// <param name="formatX">A format string that will be used to format the X
    /// double type value (see <see cref="System.Double.ToString()"/>).</param>
    /// <param name="formatY">A format string that will be used to format the Y
    /// double type value (see <see cref="System.Double.ToString()"/>).</param>
    /// <param name="formatZ">A format string that will be used to format the Z
    /// double type value (see <see cref="System.Double.ToString()"/>).</param>
    /// <param name="formatT">A format string that will be used to format the T
    /// double type value (see <see cref="System.Double.ToString()"/>).</param>
    /// <returns>A string representation of the PointPair</returns>
    public string ToString (string formatX, string formatY, string formatZ, string formatT)
    {
        return "( " + X.ToString (formatX) +
               ", " + Y.ToString (formatY) +
               ", " + Z.ToString (formatZ) +
               ", " + T.ToString (formatT) +
               " )";
    }

    #endregion
}
