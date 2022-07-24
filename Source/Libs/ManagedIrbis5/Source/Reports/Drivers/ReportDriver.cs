// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ReportDriver.cs -- abstract report driver
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Reports;

/// <summary>
/// Abstract report driver.
/// </summary>
public abstract class ReportDriver
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Driver capabilities.
    /// </summary>
    public virtual DriverCapability Capabilities => DriverCapability.None;

    /// <summary>
    /// Prologue.
    /// </summary>
    public string? Prologue { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected ReportDriver()
    {
        Magna.Logger.LogTrace (nameof (ReportDriver) + "::Constructor");
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    /// <summary>
    /// Begin cell.
    /// </summary>
    public virtual void BeginCell
        (
            ReportContext context,
            ReportCell cell
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (cell);
    }

    /// <summary>
    /// Begin document.
    /// </summary>
    public virtual void BeginDocument
        (
            ReportContext context,
            IrbisReport report
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (report);
    }

    /// <summary>
    /// Begin row.
    /// </summary>
    public virtual void BeginParagraph
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);
    }

    /// <summary>
    /// Begin row.
    /// </summary>
    public virtual void BeginRow
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);
    }

    /// <summary>
    /// Begin section.
    /// </summary>
    public virtual void BeginSection
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);
    }

    /// <summary>
    /// Begin table.
    /// </summary>
    public virtual void BeginTable
        (
            ReportContext context,
            IrbisReport report
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (report);
    }

    /// <summary>
    /// End cell.
    /// </summary>
    public virtual void EndCell
        (
            ReportContext context,
            ReportCell cell
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (cell);
    }

    /// <summary>
    /// End document.
    /// </summary>
    public virtual void EndDocument
        (
            ReportContext context,
            IrbisReport report
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (report);
    }

    /// <summary>
    /// End row.
    /// </summary>
    public virtual void EndParagraph
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);
    }

    /// <summary>
    /// End row.
    /// </summary>
    public virtual void EndRow
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);
    }

    /// <summary>
    /// End section.
    /// </summary>
    public virtual void EndSection
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);
    }

    /// <summary>
    /// End table.
    /// </summary>
    public virtual void EndTable
        (
            ReportContext context,
            IrbisReport report
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (report);
    }

    /// <summary>
    /// Start new page.
    /// </summary>
    public virtual void NewPage
        (
            ReportContext context,
            ReportBand band
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (band);
    }

    /// <summary>
    /// Set the document prologue.
    /// </summary>
    public virtual void SetPrologue
        (
            string? prologue
        )
    {
        Prologue = prologue;
    }

    /// <summary>
    /// Write the text.
    /// </summary>
    public virtual void Write
        (
            ReportContext context,
            string? text
        )
    {
        Sure.NotNull (context);
        text.NotUsed();
    }

    /// <summary>
    /// Write service (specific) text.
    /// </summary>
    public virtual void WriteServiceText
        (
            ReportContext context,
            string? text
        )
    {
        Sure.NotNull (context);
        text.NotUsed();
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public virtual void Dispose()
    {
        Magna.Logger.LogError (nameof (ReportDriver) + "::" + nameof (Dispose));
    }

    #endregion
}
