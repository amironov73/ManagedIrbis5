// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* StockPointList.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A collection class containing a list of <see cref="StockPoint"/> objects
/// that define the set of points to be displayed on the curve.
/// </summary>
[Serializable]
public class StockPointList
    : List<StockPoint>, IPointListEdit
{
    #region Properties

    /// <summary>
    /// Indexer to access the specified <see cref="StockPoint"/> object by
    /// its ordinal position in the list.
    /// </summary>
    /// <param name="index">The ordinal position (zero-based) of the
    /// <see cref="StockPoint"/> object to be accessed.</param>
    /// <value>A <see cref="StockPoint"/> object reference.</value>
    public new PointPair this [int index]
    {
        get { return base[index]; }
        set { base[index] = new StockPoint (value); }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor for the collection class
    /// </summary>
    public StockPointList()
    {
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The StockPointList from which to copy</param>
    public StockPointList (StockPointList rhs)
    {
        for (int i = 0; i < rhs.Count; i++)
        {
            StockPoint pt = new StockPoint (rhs[i]);
            this.Add (pt);
        }
    }

    /// <summary>
    /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
    /// calling the typed version of <see cref="Clone" />
    /// </summary>
    /// <returns>A deep copy of this object</returns>
    object ICloneable.Clone()
    {
        return this.Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public StockPointList Clone()
    {
        return new StockPointList (this);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Add a <see cref="StockPoint"/> object to the collection at the end of the list.
    /// </summary>
    /// <param name="point">The <see cref="StockPoint"/> object to
    /// be added</param>
    new public void Add (StockPoint point)
    {
        base.Add (new StockPoint (point));
    }

    /// <summary>
    /// Add a <see cref="PointPair"/> object to the collection at the end of the list.
    /// </summary>
    /// <param name="point">The <see cref="PointPair"/> object to be added</param>
    public void Add (PointPair point)
    {
//			throw new ArgumentException( "Error: Only the StockPt type can be added to StockPointList" +
//				".  An ordinary PointPair is not allowed" );
        base.Add (new StockPoint (point));
    }

    /// <summary>
    /// Add a <see cref="StockPoint"/> object to the collection at the end of the list using
    /// the specified values.  The unspecified values (low, open, close) are all set to
    /// <see cref="PointPairBase.Missing" />.
    /// </summary>
    /// <param name="date">An <see cref="XDate" /> value</param>
    /// <param name="high">The high value for the day</param>
    /// <returns>The zero-based ordinal index where the point was added in the list.</returns>
    public void Add (double date, double high)
    {
        Add (new StockPoint (date, high, PointPair.Missing, PointPair.Missing,
            PointPair.Missing, PointPair.Missing));
    }

    /// <summary>
    /// Add a single point to the <see cref="PointPairList"/> from values of type double.
    /// </summary>
    /// <param name="date">An <see cref="XDate" /> value</param>
    /// <param name="high">The high value for the day</param>
    /// <param name="low">The low value for the day</param>
    /// <param name="open">The opening value for the day</param>
    /// <param name="close">The closing value for the day</param>
    /// <param name="vol">The trading volume for the day</param>
    /// <returns>The zero-based ordinal index where the point was added in the list.</returns>
    public void Add (double date, double high, double low, double open, double close, double vol)
    {
        StockPoint point = new StockPoint (date, high, low, open, close, vol);
        Add (point);
    }

    /// <summary>
    /// Access the <see cref="StockPoint" /> at the specified ordinal index.
    /// </summary>
    /// <remarks>
    /// To be compatible with the <see cref="IPointList" /> interface, the
    /// <see cref="StockPointList" /> must implement an index that returns a
    /// <see cref="PointPair" /> rather than a <see cref="StockPoint" />.  This method
    /// will return the actual <see cref="StockPoint" /> at the specified position.
    /// </remarks>
    /// <param name="index">The ordinal position (zero-based) in the list</param>
    /// <returns>The specified <see cref="StockPoint" />.
    /// </returns>
    public StockPoint GetAt (int index)
    {
        return base[index];
    }

    #endregion
}
