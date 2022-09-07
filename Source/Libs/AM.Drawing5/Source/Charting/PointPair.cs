// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable InconsistentNaming

/* PointPair.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A simple point represented by an (X,Y,Z) group of double values.
/// </summary>
[Serializable]
public class PointPair
    : PointPairBase, ISerializable, ICloneable
{
    #region Member variables

    /// <summary>
    /// This PointPair's Z coordinate.  Also used for the lower value (dependent axis)
    /// for <see cref="HiLowBarItem"/> and <see cref="ErrorBarItem" /> charts.
    /// </summary>
    public double Z;

    /// <summary>
    /// A tag object for use by the user.  This can be used to store additional
    /// information associated with the <see cref="PointPair"/>.  ZedGraph never
    /// modifies this value, but if it is a <see cref="String"/> type, it
    /// may be displayed in a <see cref="System.Windows.Forms.ToolTip"/>
    /// within the <see cref="ZedGraphControl"/> object.
    /// </summary>
    /// <remarks>
    /// Note that, if you are going to Serialize ZedGraph data, then any type
    /// that you store in <see cref="Tag"/> must be a serializable type (or
    /// it will cause an exception).
    /// </remarks>
    public object? Tag;

    #endregion

    #region Construction

    /// <summary>
    /// Default Constructor
    /// </summary>
    public PointPair() : this (0, 0, 0, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Creates a point pair with the specified X and Y.
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    public PointPair (double x, double y)
        : this (x, y, 0, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Creates a point pair with the specified X, Y, and
    /// label (<see cref="Tag"/>).
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    /// <param name="label">This pair's string label (<see cref="Tag"/>)</param>
    public PointPair (double x, double y, string label)
        : this (x, y, 0, label as object)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Creates a point pair with the specified X, Y, and base value.
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    /// <param name="z">This pair's z or lower dependent coordinate.</param>
    public PointPair (double x, double y, double z)
        : this (x, y, z, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Creates a point pair with the specified X, Y, base value, and
    /// string label (<see cref="Tag"/>).
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    /// <param name="z">This pair's z or lower dependent coordinate.</param>
    /// <param name="label">This pair's string label (<see cref="Tag"/>)</param>
    public PointPair (double x, double y, double z, string? label)
        : this (x, y, z, label as object)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Creates a point pair with the specified X, Y, base value, and
    /// (<see cref="Tag"/>).
    /// </summary>
    /// <param name="x">This pair's x coordinate.</param>
    /// <param name="y">This pair's y coordinate.</param>
    /// <param name="z">This pair's z or lower dependent coordinate.</param>
    /// <param name="tag">This pair's <see cref="Tag"/> property</param>
    public PointPair (double x, double y, double z, object tag)
        : base (x, y)
    {
        Z = z;
        Tag = tag;
    }

    /// <summary>
    /// Creates a point pair from the specified <see cref="PointF"/> struct.
    /// </summary>
    /// <param name="pt">The <see cref="PointF"/> struct from which to get the
    /// new <see cref="PointPair"/> values.</param>
    public PointPair (PointF pt)
        : this (pt.X, pt.Y, 0, null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// The PointPair copy constructor.
    /// </summary>
    /// <param name="rhs">The basis for the copy.</param>
    public PointPair (PointPair rhs) : base (rhs)
    {
        Z = rhs.Z;

        if (rhs.Tag is ICloneable)
        {
            Tag = ((ICloneable)rhs.Tag).Clone();
        }
        else
        {
            Tag = rhs.Tag;
        }
    }

    #endregion

    /// <inheritdoc cref="ICloneable.Clone"/>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public PointPair Clone()
    {
        return new PointPair (this);
    }

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema2 = 11;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected PointPair
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        Z = info.GetDouble ("Z");
        Tag = info.GetValue ("Tag", typeof (object));
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
        info.AddValue ("Z", Z);
        info.AddValue ("Tag", Tag);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Readonly value that determines if either the X, Y, or Z
    /// coordinate in this PointPair is an invalid (not plotable) value.
    /// It is considered invalid if it is missing (equal to System.Double.Max),
    /// Infinity, or NaN.
    /// </summary>
    /// <returns>true if any value is invalid</returns>
    public bool IsInvalid3D =>
        X == Missing ||
        Y == Missing ||
        Z == Missing ||
        double.IsInfinity (X) ||
        double.IsInfinity (Y) ||
        double.IsInfinity (Z) ||
        double.IsNaN (X) ||
        double.IsNaN (Y) ||
        double.IsNaN (Z);

    /// <summary>
    /// The "low" value for this point (lower dependent-axis value).
    /// This is really just an alias for <see cref="PointPair.Z"/>.
    /// </summary>
    /// <value>The lower dependent value for this <see cref="PointPair"/>.</value>
    public double LowValue
    {
        get => Z;
        set => Z = value;
    }

    /// <summary>
    /// The ColorValue property is just an alias for the <see cref="PointPair.Z" />
    /// property.
    /// </summary>
    /// <remarks>
    /// For other types, such as the <see cref="StockPoint"/>, the <see cref="StockPoint" />
    /// can be mapped to a unique value.  This is used with the
    /// <see cref="FillType.GradientByColorValue" /> option.
    /// </remarks>
    public virtual double ColorValue
    {
        get => Z;
        set => Z = value;
    }

    #endregion

    #region Inner classes

    /// <summary>
    /// Compares points based on their y values.  Is setup to be used in an
    /// ascending order sort.
    /// <seealso cref="System.Collections.ArrayList.Sort()"/>
    /// </summary>
    public class PointPairComparerY
        : IComparer<PointPair>
    {
        /// <summary>
        /// Compares two <see cref="PointPair"/>s.
        /// </summary>
        /// <param name="left">Point to the left.</param>
        /// <param name="right">Point to the right.</param>
        /// <returns>-1, 0, or 1 depending on l.Y's relation to r.Y</returns>
        public int Compare (PointPair? left, PointPair? right)
        {
            if (left == null && right == null)
            {
                return 0;
            }

            if (left == null && right != null)
            {
                return -1;
            }
            if (left != null && right == null)
            {
                return 1;
            }

            var lY = left!.Y;
            var rY = right!.Y;

            if (Math.Abs (lY - rY) < .000000001)
            {
                return 0;
            }

            return lY < rY ? -1 : 1;
        }
    }

    /// <summary>
    /// Compares points based on their x values.  Is setup to be used in an
    /// ascending order sort.
    /// <seealso cref="System.Collections.ArrayList.Sort()"/>
    /// </summary>
    public class PointPairComparer : IComparer<PointPair>
    {
        private SortType sortType;

        /// <summary>
        /// Constructor for PointPairComparer.
        /// </summary>
        /// <param name="type">The axis type on which to sort.</param>
        public PointPairComparer (SortType type)
        {
            sortType = type;
        }

        /// <summary>
        /// Compares two <see cref="PointPair"/>s.
        /// </summary>
        /// <param name="left">Point to the left.</param>
        /// <param name="right">Point to the right.</param>
        /// <returns>-1, 0, or 1 depending on l.X's relation to r.X</returns>
        public int Compare (PointPair? left, PointPair? right)
        {
            if (left == null && right == null)
            {
                return 0;
            }

            if (left == null && right != null)
            {
                return -1;
            }
            if (left != null && right == null)
            {
                return 1;
            }

            double lVal, rVal;

            if (sortType == SortType.XValues)
            {
                lVal = left.X;
                rVal = right.X;
            }
            else
            {
                lVal = left.Y;
                rVal = right.Y;
            }

            if (lVal == Missing || double.IsInfinity (lVal) || double.IsNaN (lVal))
            {
                left = null;
            }

            if (rVal == Missing || double.IsInfinity (rVal) || double.IsNaN (rVal))
            {
                right = null;
            }

            if ((left == null && right == null) || (Math.Abs (lVal - rVal) < 1e-100))
            {
                return 0;
            }

            if (left == null && right != null)
            {
                return -1;
            }
            if (left != null && right == null)
            {
                return 1;
            }
            return lVal < rVal ? -1 : 1;
        }
    }

    #endregion

    #region Methods

    /// <inheritdoc cref="Object.Equals(object)"/>
    public override bool Equals (object? obj)
    {
        var rhs = (PointPair) obj!;
        return X == rhs.X && Y == rhs.Y && Z == rhs.Z;
    }

    /// <inheritdoc cref="Object.GetHashCode"/>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    /// <summary>
    /// Format this PointPair value using the default format.  Example:  "( 12.345, -16.876 )".
    /// The two double values are formatted with the "g" format type.
    /// </summary>
    /// <param name="isShowZ">true to show the third "Z" or low dependent value coordinate</param>
    /// <returns>A string representation of the PointPair</returns>
    public virtual string ToString (bool isShowZ)
    {
        return ToString (DefaultFormat, isShowZ);
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
    /// <param name="isShowZ">true to show the third "Z" or low dependent value coordinate</param>
    public virtual string ToString (string format, bool isShowZ)
    {
        return "( " + X.ToString (format) +
               ", " + Y.ToString (format) +
               (isShowZ ? (", " + Z.ToString (format)) : "")
               + " )";
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
    /// <returns>A string representation of the PointPair</returns>
    public string ToString (string formatX, string formatY, string formatZ)
    {
        return "( " + X.ToString (formatX) +
               ", " + Y.ToString (formatY) +
               ", " + Z.ToString (formatZ) +
               " )";
    }

    #endregion
}
