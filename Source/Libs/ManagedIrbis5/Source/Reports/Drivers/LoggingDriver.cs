// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* LoggingDriver.cs -- драйвер логирования для отладки отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Reports;

/// <summary>
/// Драйвер логирования для отладки отчета.
/// </summary>
public sealed class LoggingDriver
    : ReportDriver
{
    #region Properties

    /// <summary>
    /// Внутренний драйвер.
    /// </summary>
    public ReportDriver InnerDriver { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public LoggingDriver
        (
            ReportDriver innerDriver
        )
    {
        Sure.NotNull (innerDriver);

        InnerDriver = innerDriver;
    }

    #endregion

    #region ReportDriver members

    /// <inheritdoc cref="ReportDriver.BeginCell"/>
    public override void BeginCell
        (
            ReportContext context,
            ReportCell cell
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (cell);

        Magna.Logger.LogTrace
            (
                nameof (LoggingDriver) + "::" + nameof (BeginCell)
                + ": {Cell}",
                cell
            );

        InnerDriver.BeginCell (context, cell);
    }

    /// <inheritdoc cref="ReportDriver.BeginDocument"/>
    public override void BeginDocument
        (
            ReportContext context,
            IrbisReport report
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (report);

        Magna.Logger.LogTrace
            (
                nameof (LoggingDriver) + "::" + nameof (BeginDocument)
                + ": {Report}",
                report
            );

        InnerDriver.BeginDocument (context, report);
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

        Magna.Logger.LogTrace
            (
                nameof (LoggingDriver) + "::" + nameof (BeginRow)
                + ": {Band}",
                band
            );

        InnerDriver.BeginRow (context, band);
    }

    /// <inheritdoc cref="ReportDriver.EndCell"/>
    public override void EndCell
        (
            ReportContext context,
            ReportCell cell
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (cell);

        Magna.Logger.LogTrace
            (
                nameof (LoggingDriver) + "::" + nameof (EndCell)
                + ": {Cell}",
                cell
            );

        InnerDriver.EndCell (context, cell);
    }

    /// <inheritdoc cref="ReportDriver.EndDocument"/>
    public override void EndDocument
        (
            ReportContext context,
            IrbisReport report
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (report);

        Magna.Logger.LogTrace
            (
                nameof (LoggingDriver) + "::" + nameof (EndDocument)
                + ": {Report}",
                report
            );

        InnerDriver.EndDocument (context, report);
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

        Magna.Logger.LogTrace
            (
                nameof (LoggingDriver) + "::" + nameof (EndRow)
                + ": {Band}",
                band
            );

        InnerDriver.EndRow (context, band);
    }

    /// <inheritdoc cref="ReportDriver.Write"/>
    public override void Write
        (
            ReportContext context,
            string? text
        )
    {
        Sure.NotNull (context);

        Magna.Logger.LogTrace
            (
                nameof (LoggingDriver) + "::" + nameof (Write)
                + ": {Text}",
                text
            );

        InnerDriver.Write (context, text);
    }

    #endregion
}
