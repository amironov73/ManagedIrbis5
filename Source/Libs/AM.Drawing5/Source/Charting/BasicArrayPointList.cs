// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* BasicArrayPointList
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A data collection class for ZedGraph, provided as an alternative to <see cref="PointPairList" />.
/// </summary>
/// <remarks>
/// The data storage class for ZedGraph can be any type, so long as it uses the <see cref="IPointList" />
/// interface.  This class, albeit simple, is a demonstration of implementing the <see cref="IPointList" />
/// interface to provide a simple data collection using only two arrays.  The <see cref="IPointList" />
/// interface can also be used as a layer between ZedGraph and a database, for example.
/// </remarks>
/// <seealso cref="PointPairList" />
/// <seealso cref="IPointList" />
[Serializable]
public class BasicArrayPointList : IPointList
{
    #region Fields

    /// <summary>
    /// Instance of an array of x values
    /// </summary>
    public double[] x;

    /// <summary>
    /// Instance of an array of x values
    /// </summary>
    public double[] y;

    #endregion

    #region Properties

    /// <summary>
    /// Indexer to access the specified <see cref="PointPair"/> object by
    /// its ordinal position in the list.
    /// </summary>
    /// <remarks>
    /// Returns <see cref="PointPairBase.Missing" /> for any value of <see paramref="index" />
    /// that is outside of its corresponding array bounds.
    /// </remarks>
    /// <param name="index">The ordinal position (zero-based) of the
    /// <see cref="PointPair"/> object to be accessed.</param>
    /// <value>A <see cref="PointPair"/> object reference.</value>
    public PointPair this [int index]
    {
        get
        {
            double xVal, yVal;
            if (index >= 0 && index < x.Length)
                xVal = x[index];
            else
                xVal = PointPairBase.Missing;

            if (index >= 0 && index < y.Length)
                yVal = y[index];
            else
                yVal = PointPairBase.Missing;
            return new PointPair (xVal, yVal, PointPairBase.Missing, null);
        }
        set
        {
            if (index >= 0 && index < x.Length)
                x[index] = value.X;
            if (index >= 0 && index < y.Length)
                y[index] = value.Y;
        }
    }

    /// <summary>
    /// Returns the number of points available in the arrays.  Count will be the greater
    /// of the lengths of the X and Y arrays.
    /// </summary>
    public int Count
    {
        get { return x.Length > y.Length ? x.Length : y.Length; }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor to initialize the PointPairList from two arrays of
    /// type double.
    /// </summary>
    public BasicArrayPointList (double[] x, double[] y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The PointPairList from which to copy</param>
    public BasicArrayPointList (BasicArrayPointList rhs)
    {
        x = (double[])rhs.x.Clone();
        y = (double[])rhs.y.Clone();
    }

    /// <summary>
    /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
    /// calling the typed version of <see cref="Clone" />
    /// </summary>
    /// <returns>A deep copy of this object</returns>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public BasicArrayPointList Clone()
    {
        return new BasicArrayPointList (this);
    }

    #endregion
}
