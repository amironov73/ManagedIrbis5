// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* CubeSourceBase.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using AM.Reporting.CrossView;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Base class for all CubeSources such as <see cref="SliceCubeSource"/>.
/// </summary>
[TypeConverter (typeof (TypeConverters.CubeSourceConverter))]
public abstract class CubeSourceBase
    : DataComponentBase
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler? OnChanged;

    #endregion


    #region Properties

    /// <summary>
    ///
    /// </summary>
    public int XAxisFieldsCount => CubeLink?.XAxisFieldsCount ?? 0;

    /// <summary>
    ///
    /// </summary>
    public int YAxisFieldsCount => CubeLink?.YAxisFieldsCount ?? 0;

    /// <summary>
    ///
    /// </summary>
    public int MeasuresCount => CubeLink?.MeasuresCount ?? 0;

    /// <summary>
    ///
    /// </summary>
    public int MeasuresLevel => CubeLink?.MeasuresLevel ?? 0;

    /// <summary>
    ///
    /// </summary>
    public bool MeasuresInXAxis => CubeLink?.MeasuresInXAxis ?? false;

    /// <summary>
    ///
    /// </summary>
    public bool MeasuresInYAxis => CubeLink?.MeasuresInYAxis ?? false;

    /// <summary>
    ///
    /// </summary>
    public int DataColumnCount => CubeLink?.DataColumnCount ?? 0;

    /// <summary>
    ///
    /// </summary>
    public int DataRowCount => CubeLink?.DataRowCount ?? 0;

    /// <summary>
    ///
    /// </summary>
    public bool SourceAssigned => CubeLink is not null;

    /// <summary>
    ///
    /// </summary>
    public IBaseCubeLink? CubeLink => Reference as IBaseCubeLink;

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="CubeSourceBase"/>
    /// class with default settings.
    /// </summary>
    protected CubeSourceBase()
    {
        SetFlags (Flags.HasGlobalName, true);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public CrossViewMeasureCell GetMeasureCell
        (
            int colIndex,
            int rowIndex
        )
    {
        return CubeLink?.GetMeasureCell (colIndex, rowIndex)
               ?? new CrossViewMeasureCell();
    }

    /// <summary>
    ///
    /// </summary>
    public void TraverseXAxis
        (
            CrossViewAxisDrawCellHandler crossViewAxisDrawCellHandler
        )
    {
        CubeLink?.TraverseXAxis (crossViewAxisDrawCellHandler);
    }


    /// <summary>
    ///
    /// </summary>
    public void TraverseYAxis
        (
            CrossViewAxisDrawCellHandler crossViewAxisDrawCellHandler
        )
    {
        CubeLink?.TraverseYAxis (crossViewAxisDrawCellHandler);
    }

    /// <summary>
    ///
    /// </summary>
    public string GetXAxisFieldName
        (
            int fieldIndex
        )
    {
        return CubeLink != null
            ? CubeLink.GetXAxisFieldName (fieldIndex)
            : string.Empty;
    }

    /// <summary>
    ///
    /// </summary>
    public string GetYAxisFieldName
        (
            int fieldIndex
        )
    {
        return CubeLink != null
            ? CubeLink.GetYAxisFieldName (fieldIndex)
            : string.Empty;
    }

    /// <summary>
    ///
    /// </summary>
    public string GetMeasureName
        (
            int measureIndex
        )
    {
        return CubeLink != null
            ? CubeLink.GetMeasureName (measureIndex)
            : string.Empty;
    }

    /// <summary>
    ///
    /// </summary>
    public void Changed()
    {
        OnChanged?.Invoke (this, EventArgs.Empty);
    }

    #endregion
}
