// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* CrossViewCellDescriptor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.CrossView;

/// <summary>
/// The descriptor that is used to describe one CrossView data cell.
/// </summary>
/// <remarks>
/// The <see cref="CrossViewCellDescriptor"/> class is used to define one data cell of the CrossView.
/// To set visual appearance of the data cell, use the <see cref="CrossViewDescriptor.TemplateCell"/>
/// property.
/// <para/>The collection of descriptors used to represent the CrossView data cells is stored
/// in the <b>CrossViewObject.Data.Cells</b> property.
/// </remarks>
public class CrossViewCellDescriptor
    : CrossViewDescriptor
{
    #region Properties

    /// <summary>
    /// Gets a value indicating that this is the "GrandTotal" element on X axis.
    /// </summary>
    public bool IsXGrandTotal { set; get; }

    /// <summary>
    /// Gets a value indicating that this is the "GrandTotal" element on Y axis.
    /// </summary>
    public bool IsYGrandTotal { set; get; }

    /// <summary>
    /// Gets a value indicating that this is the "Total" element on X axis.
    /// </summary>
    public bool IsXTotal { set; get; }

    /// <summary>
    /// Gets a value indicating that this is the "Total" element on Y axis.
    /// </summary>
    public bool IsYTotal { set; get; }

    /// <summary>
    /// Gets the name of field in X axis.
    /// </summary>
    public string XFieldName { set; get; }

    /// <summary>
    /// Gets the name of field in Y axis.
    /// </summary>
    public string YFieldName { set; get; }

    /// <summary>
    /// Gets the name of measure in cube.
    /// </summary>
    public string MeasureName { set; get; }

    /// <summary>
    /// Gets the x coordinate.
    /// </summary>
    public int X { set; get; }

    /// <summary>
    /// Gets the y coordinate.
    /// </summary>
    public int Y { set; get; }

    #endregion

    #region Public Methods

    /// <inheritdoc/>
    public override void Assign
        (
            CrossViewDescriptor source
        )
    {
        Sure.NotNull (source);

        base.Assign (source);
        if (source is CrossViewCellDescriptor src)
        {
            IsXTotal = src.IsXTotal;
            IsYTotal = src.IsYTotal;
            IsXGrandTotal = src.IsXGrandTotal;
            IsYGrandTotal = src.IsYGrandTotal;
            XFieldName = src.XFieldName;
            YFieldName = src.YFieldName;
            MeasureName = src.MeasureName;
            X = src.X;
            Y = src.Y;
        }
    }

    /// <inheritdoc/>
    public override void Serialize
        (
            ReportWriter writer
        )
    {
        Sure.NotNull (writer);

        var c = (CrossViewCellDescriptor) writer.DiffObject!;
        base.Serialize (writer);
        writer.ItemName = "Cell";
        if (IsXTotal != c.IsXTotal)
        {
            writer.WriteBool ("IsXTotal", IsXTotal);
        }

        if (IsYTotal != c.IsYTotal)
        {
            writer.WriteBool ("IsYTotal", IsYTotal);
        }

        if (IsXGrandTotal != c.IsXGrandTotal)
        {
            writer.WriteBool ("IsXGrandTotal", IsXGrandTotal);
        }

        if (IsYGrandTotal != c.IsYGrandTotal)
        {
            writer.WriteBool ("IsYGrandTotal", IsYGrandTotal);
        }

        if (XFieldName != c.XFieldName)
        {
            writer.WriteStr ("XFieldName", XFieldName);
        }

        if (YFieldName != c.YFieldName)
        {
            writer.WriteStr ("YFieldName", YFieldName);
        }

        if (MeasureName != c.MeasureName)
        {
            writer.WriteStr ("MeasureName", MeasureName);
        }

        if (X != c.X)
        {
            writer.WriteInt ("X", X);
        }

        if (Y != c.Y)
        {
            writer.WriteInt ("Y", Y);
        }
    }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="CrossViewCellDescriptor"/> class
    /// </summary>
    /// <param name="xFieldName">The Field Name in X axis.</param>
    /// <param name="yFieldName">The Field Name in Y axis.</param>
    /// <param name="measureName">The Measure Name.</param>
    /// <param name="isXTotal">Indicates the "XTotal" element.</param>
    /// <param name="isYTotal">Indicates the "YTotal" element.</param>
    /// <param name="isXGrandTotal">Indicates the "XGrandTotal" element.</param>
    /// <param name="isYGrandTotal">Indicates the "YGrandTotal" element.</param>
    public CrossViewCellDescriptor
        (
            string xFieldName,
            string yFieldName,
            string measureName,
            bool isXTotal,
            bool isYTotal,
            bool isXGrandTotal,
            bool isYGrandTotal
        )
    {
        IsXGrandTotal = isXGrandTotal;
        IsYGrandTotal = isYGrandTotal;
        MeasureName = measureName;
        if (isXGrandTotal)
        {
            XFieldName = "";
            IsXTotal = false;
        }
        else
        {
            XFieldName = xFieldName;
            IsXTotal = isXTotal;
        }

        if (isYGrandTotal)
        {
            YFieldName = "";
            IsYTotal = false;
        }
        else
        {
            YFieldName = yFieldName;
            IsYTotal = isYTotal;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CrossViewCellDescriptor"/> class
    /// </summary>
    public CrossViewCellDescriptor()
        : this ("", "", "", false, false, false, false)
    {
    }
}
