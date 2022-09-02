// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* NoDupePointList.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// A simple storage struct to maintain an individual sampling of data.  This only
/// contains two data values in order to reduce to memory load for large datasets.
/// (e.g., no Tag or Z property)
/// </summary>
public struct DataPoint
{
    /// <summary>
    /// The X value for the point, stored as a double type.
    /// </summary>
    public double X;

    /// <summary>
    /// The Y value for the point, stored as a double type.
    /// </summary>
    public double Y;
}

/// <summary>
/// A collection class to maintain a set of samples.
/// </summary>
/// <remarks>This type, intended for very
/// large datasets, will reduce the number of points displayed by eliminating
/// individual points that overlay (at the same pixel location) on the graph.
/// Note that this type probably does not make sense for line plots, but is intended
/// primarily for scatter plots.
/// </remarks>
[Serializable]
public class NoDupePointList
    : List<DataPoint>, IPointListEdit
{
    /// <summary>
    /// Protected field that stores the number of data points after filtering (e.g.,
    /// <see cref="FilterData" /> has been called).  The <see cref="Count" /> property
    /// returns the total count for an unfiltered dataset, or <see cref="_filteredCount" />
    /// for a dataset that has been filtered.
    /// </summary>
    protected int _filteredCount;

    /// <summary>
    /// Protected array of indices for all the points that are currently visible.  This only
    /// applies if <see cref="IsFiltered" /> is true.
    /// </summary>
    protected int[]? _visibleIndices;


    /// <summary>
    /// Gets or sets a value that determines how close a point must be to a prior
    /// neighbor in order to be filtered out.
    /// </summary>
    /// <remarks>
    /// A value of 0 indicates that subsequent
    /// points must coincide exactly at the same pixel location.  A value of 1 or more
    /// indicates that number of pixels distance from a prior point that will cause
    /// a new point to be filtered out.  For example, a value of 2 means that, once
    /// a particular pixel location is taken, any subsequent point that lies within 2
    /// pixels of that location will be filtered out.
    /// </remarks>
    public int FilterMode { get; set; }

    /// <summary>
    /// Gets a value indicating whether or not the data have been filtered.  If the data
    /// have not been filtered, then <see cref="Count" /> will be equal to
    /// <see cref="TotalCount" />.
    /// </summary>
    public bool IsFiltered { get; protected set; }

    /// <summary>
    /// Indexer: get the DataPoint instance at the specified ordinal position in the list
    /// </summary>
    /// <remarks>
    /// This method will throw an exception if the index is out of range.  This can happen
    /// if the index is less than the number of filtered values, or if data points are
    /// removed from a filtered dataset with updating the filter (by calling
    /// <see cref="FilterData" />).
    /// </remarks>
    /// <param name="index">The ordinal position in the list of points</param>
    /// <returns>Returns a <see cref="PointPair" /> instance.  The <see cref="PointPair.Z" />
    /// and <see cref="PointPair.Tag" /> properties will be defaulted to
    /// <see cref="PointPairBase.Missing" /> and null, respectively.
    /// </returns>
    public new PointPair this [int index]
    {
        get
        {
            var indices = _visibleIndices.ThrowIfNull();
            var j = index;
            if (IsFiltered)
            {
                j = indices[index];
            }

            var dp = base[j];
            var pt = new PointPair (dp.X, dp.Y);
            return pt;
        }
        set
        {
            var indices = _visibleIndices.ThrowIfNull();
            var j = index;
            if (IsFiltered)
            {
                j = indices[index];
            }

            base[j] = new DataPoint { X = value.X, Y = value.Y };
        }
    }

    /// <summary>
    /// Gets the number of active samples in the collection.  This is the number of
    /// samples that are non-duplicates.  See the <see cref="TotalCount" /> property
    /// to get the total number of samples in the list.
    /// </summary>
    public new int Count => !IsFiltered ? base.Count : _filteredCount;

    /// <summary>
    /// Gets the total number of samples in the collection.  See the <see cref="Count" />
    /// property to get the number of active (non-duplicate) samples in the list.
    /// </summary>
    public int TotalCount => base.Count;

    /// <summary>
    /// Append a data point to the collection
    /// </summary>
    /// <param name="pt">The <see cref="PointPair" /> value to append</param>
    public void Add (PointPair pt)
    {
        var dp = new DataPoint
        {
            X = pt.X,
            Y = pt.Y
        };
        Add (dp);
    }


    /// <summary>
    /// Append a point to the collection
    /// </summary>
    /// <param name="x">The x value of the point to append</param>
    /// <param name="y">The y value of the point to append</param>
    public void Add (double x, double y)
    {
        var dp = new DataPoint
        {
            X = x,
            Y = y
        };
        Add (dp);
    }


   /// <inheritdoc cref="ICloneable.Clone"/>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// typesafe clone method
    /// </summary>
    /// <returns>A new cloned NoDupePointList.  This returns a copy of the structure,
    /// but it does not duplicate the data (it just keeps a reference to the original)
    /// </returns>
    public NoDupePointList Clone()
    {
        return new NoDupePointList (this);
    }

    /// <summary>
    /// default constructor
    /// </summary>
    public NoDupePointList()
    {
        IsFiltered = false;
        _filteredCount = 0;
        _visibleIndices = null;
        FilterMode = 0;
    }

    /// <summary>
    /// copy constructor -- this returns a copy of the structure,
    /// but it does not duplicate the data (it just keeps a reference to the original)
    /// </summary>
    /// <param name="rhs">The NoDupePointList to be copied</param>
    public NoDupePointList (NoDupePointList rhs)
    {
        var count = rhs.TotalCount;
        for (var i = 0; i < count; i++)
            Add (rhs.GetDataPointAt (i));

        _filteredCount = rhs._filteredCount;
        IsFiltered = rhs.IsFiltered;
        FilterMode = rhs.FilterMode;

        if (rhs._visibleIndices != null)
        {
            _visibleIndices = (int[])rhs._visibleIndices.Clone();
        }
        else
        {
            _visibleIndices = null;
        }
    }

    /// <summary>
    /// Protected method to access the internal DataPoint collection, without any
    /// translation to a PointPair.
    /// </summary>
    /// <param name="index">The ordinal position of the DataPoint of interest</param>
    protected DataPoint GetDataPointAt (int index)
    {
        return base[index];
    }

    /// <summary>
    /// Clears any filtering previously done by a call to <see cref="FilterData" />.
    /// After calling this method, all data points will be visible, and
    /// <see cref="Count" /> will be equal to <see cref="TotalCount" />.
    /// </summary>
    public void ClearFilter()
    {
        IsFiltered = false;
        _filteredCount = 0;
    }

    /// <summary>
    /// Go through the collection, and hide (filter out) any points that fall on the
    /// same pixel location as a previously included point.
    /// </summary>
    /// <remarks>
    /// This method does not delete any points, it just temporarily hides them until
    /// the next call to <see cref="FilterData" /> or <see cref="ClearFilter" />.
    /// You should call <see cref="FilterData" /> once your collection of points has
    /// been constructed.  You may need to call <see cref="FilterData" /> again if
    /// you add points, or if the chart rect changes size (by resizing, printing,
    /// image save, etc.), or if the scale range changes.
    /// You must call <see cref="GraphPane.AxisChange()" /> before calling
    /// this method so that the <see cref="Chart.Rect">GraphPane.Chart.Rect</see>
    /// and the scale ranges are valid.  This method is not valid for
    /// ordinal axes (but ordinal axes don't make sense for very large datasets
    /// anyway).
    /// </remarks>
    /// <param name="pane">The <see cref="GraphPane" /> into which the data
    /// will be plotted. </param>
    /// <param name="yAxis">The <see cref="Axis" /> class to be used in the Y direction
    /// for plotting these data.  This can be a <see cref="YAxis" /> or a
    /// <see cref="Y2Axis" />, and can be a primary or secondary axis (if multiple Y or Y2
    /// axes are being used).
    /// </param>
    /// <param name="xAxis">The <see cref="Axis" /> class to be used in the X direction
    /// for plotting these data.  This can be an <see cref="XAxis" /> or a
    /// <see cref="X2Axis" />.
    /// </param>
    public void FilterData (GraphPane pane, Axis xAxis, Axis yAxis)
    {
        if (_visibleIndices == null || _visibleIndices.Length < base.Count)
        {
            _visibleIndices = new int[base.Count];
        }

        _filteredCount = 0;
        IsFiltered = true;

        var width = (int)pane.Chart.Rect.Width;
        var height = (int)pane.Chart.Rect.Height;
        if (width <= 0 || height <= 0)
        {
            throw new IndexOutOfRangeException ("Error in FilterData: Chart rect not valid");
        }

        var usedArray = new bool[width, height];
        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
            usedArray[i, j] = false;

        var xScale = xAxis.Scale.ThrowIfNull();
        var yScale = yAxis.Scale.ThrowIfNull();
        xScale.SetupScaleData (pane, xAxis);
        yScale.SetupScaleData (pane, yAxis);

        var n = FilterMode < 0 ? 0 : FilterMode;
        var left = (int)pane.Chart.Rect.Left;
        var top = (int)pane.Chart.Rect.Top;

        for (var i = 0; i < base.Count; i++)
        {
            var dp = base[i];
            var x = (int)(xScale.Transform (dp.X) + 0.5) - left;
            var y = (int)(yScale.Transform (dp.Y) + 0.5) - top;

            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                var used = false;
                if (n <= 0)
                {
                    used = usedArray[x, y];
                }
                else
                {
                    for (var ix = x - n; ix <= x + n; ix++)
                    for (var iy = y - n; iy <= y + n; iy++)
                        used |= (ix >= 0 && ix < width && iy >= 0 && iy < height && usedArray[ix, iy]);
                }

                if (!used)
                {
                    usedArray[x, y] = true;
                    _visibleIndices[_filteredCount] = i;
                    _filteredCount++;
                }
            }
        }
    }
}
