// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator

/* BandColumns.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// This class holds the band columns settings. It is used in the <see cref="DataBand.Columns"/> property.
/// </summary>
[TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
public class BandColumns
{
    /// <summary>
    /// Gets or sets the number of columns.
    /// </summary>
    /// <remarks>
    /// Set this property to 0 or 1 if you don't want to use columns.
    /// </remarks>
    [DefaultValue (0)]
    public int Count
    {
        get => _count;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException (nameof (Count), "Value must be >= 0");
            }

            _count = value;
        }
    }

    /// <summary>
    /// The column width, in pixels.
    /// </summary>
    [DefaultValue (0f)]
    [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
    public float Width { get; set; }

    /// <summary>
    /// Gets or sets the layout of the columns.
    /// </summary>
    [DefaultValue (ColumnLayout.AcrossThenDown)]
    public ColumnLayout Layout { get; set; }

    /// <summary>
    /// Gets or sets the minimum row count that must be printed.
    /// </summary>
    /// <remarks>
    /// This property is used if the <b>Layout</b> property is set to <b>DownThenAcross</b>. 0 means that
    /// AM.Reporting should calculate the optimal number of rows.
    /// </remarks>
    [DefaultValue (0)]
    public int MinRowCount { get; set; }

    internal float ActualWidth
    {
        get
        {
            if (Width == 0 && _dataBand.Page is ReportPage page)
            {
                return (page.PaperWidth - page.LeftMargin - page.RightMargin) * Units.Millimeters /
                       (Count == 0 ? 1 : Count);
            }

            return Width;
        }
    }

    internal FloatCollection Positions
    {
        get
        {
            var positions = new FloatCollection();
            var columnWidth = ActualWidth;
            for (var i = 0; i < Count; i++)
            {
                positions.Add (i * columnWidth);
            }

            return positions;
        }
    }

    #region Private members

    private int _count;
    private DataBand _dataBand;

    #endregion

    /// <summary>
    /// Assigns values from another source.
    /// </summary>
    /// <param name="source">Source to assign from.</param>
    public void Assign
        (
            BandColumns source
        )
    {
        Count = source.Count;
        Width = source.Width;
        Layout = source.Layout;
        MinRowCount = source.MinRowCount;
    }

    internal void Serialize
        (
            ReportWriter writer,
            BandColumns columns
        )
    {
        if (Count != columns.Count)
        {
            writer.WriteInt ("Columns.Count", Count);
        }

        if (Width != columns.Width)
        {
            writer.WriteFloat ("Columns.Width", Width);
        }

        if (Layout != columns.Layout)
        {
            writer.WriteValue ("Columns.Layout", Layout);
        }

        if (MinRowCount != columns.MinRowCount)
        {
            writer.WriteInt ("Columns.MinRowCount", MinRowCount);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <b>BandColumns</b> class with default settings.
    /// </summary>
    public BandColumns
        (
            DataBand dataBand
        )
    {
        Sure.NotNull (dataBand);

        _dataBand = dataBand;
    }
}
