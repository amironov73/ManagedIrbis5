// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* DatasetDriver.cs -- драйвер-обертка над датасетами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Data;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Reports;

/// <summary>
/// Драйвер-обертка над датасетами.
/// </summary>
public sealed class DatasetDriver
    : ReportDriver
{
    #region Properties

    /// <summary>
    /// Dataset.
    /// </summary>
    public DataSet? DataSet { get; internal set; }

    #endregion

    #region Private members

    private List<string>? _currentLine;

    #endregion

    #region ReportDriver members

    /// <inheritdoc cref="ReportDriver.BeginDocument"/>
    public override void BeginDocument
        (
            ReportContext context,
            IrbisReport report
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (report);

        DataSet = new DataSet();
        DataSet.Tables.Add (new DataTable());
    }

    /// <inheritdoc cref="ReportDriver.BeginRow"/>
    public override void BeginRow
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);

        Sure.NotNull (context);
        Sure.NotNull (band);

        _currentLine = new List<string>();
    }

    /// <inheritdoc cref="ReportDriver.BeginCell"/>
    public override void BeginCell
        (
            ReportContext context,
            ReportCell cell
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (cell);

        _currentLine.ThrowIfNull().Add (null!);
    }

    /// <inheritdoc cref="ReportDriver.EndRow"/>
    public override void EndRow
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);

        var table = DataSet.ThrowIfNull().Tables[0];
        var row = table.NewRow();
        row.ItemArray = _currentLine.ThrowIfNull ().ToArray();
        table.Rows.Add (row);
    }

    /// <inheritdoc cref="ReportDriver.Write"/>
    public override void Write
        (
            ReportContext context,
            string? text
        )
    {
        Sure.NotNull (context);

        var currentLine = _currentLine.ThrowIfNull();
        currentLine.ThrowIfNull() [currentLine.Count - 1] = text!;
    }

    #endregion
}
