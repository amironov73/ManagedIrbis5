// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberNeverOverridden.Global

/* ReportBand.cs -- базовый тип для полос отчета
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;

using ManagedIrbis.Pft;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Reports;

/// <summary>
/// Базовый тип для полос отчета. Простая не повторяющаяся полоса.
/// </summary>
public class ReportBand
    : IAttributable,
    IVerifiable,
    IDisposable
{
    #region Events

    /// <summary>
    /// Raised after rendering.
    /// </summary>
    public event EventHandler<ReportRenderingEventArgs>? AfterRendering;

    /// <summary>
    /// Raised before rendering.
    /// </summary>
    public event EventHandler<ReportRenderingEventArgs>? BeforeRendering;

    #endregion

    #region Properties

    /// <summary>
    /// Attributes.
    /// </summary>
    [XmlArray ("attr")]
    [JsonPropertyName ("attr")]
    public ReportAttributes Attributes { get; }

    /// <summary>
    /// Cells.
    /// </summary>
    [XmlArray ("cells")]
    [JsonPropertyName ("cells")]
    public CellCollection Cells { get; }

    /// <summary>
    /// Report.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public virtual IrbisReport? Report
    {
        get => _report;
        internal set
        {
            _report = value;
            Cells.SetReport (value);
        }
    }

    /// <summary>
    /// Parent band.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public ReportBand? Parent { get; internal set; }

    /// <summary>
    /// Arbitrary user data.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    public object? UserData { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ReportBand()
    {
        Magna.Logger.LogTrace (nameof (ReportBand) + "::Constructor");

        Attributes = new ReportAttributes();
        Cells = new CellCollection
        {
            Band = this
        };
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReportBand
        (
            params ReportCell[] cells
        )
        : this()
    {
        Sure.NotNull (cells);

        foreach (var cell in cells)
        {
            Cells.Add (cell);
        }
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReportBand
        (
            params ReportAttribute[] attributes
        )
        : this()
    {
        Sure.NotNull (attributes);

        foreach (var attribute in attributes)
        {
            Attributes.Add
                (
                    attribute.Name.ThrowIfNull(),
                    attribute.Value.ThrowIfNull()
                );
        }
    }

    #endregion

    #region Private members

    private IrbisReport? _report;

    private void _Render
        (
            ReportContext context
        )
    {
        var driver = context.Driver;
        driver.BeginRow (context, this);
        foreach (var cell in Cells)
        {
            cell.Render (context);
        }

        driver.EndRow (context, this);
    }

    /// <summary>
    /// Called after <see cref="Render"/>.
    /// </summary>
    protected void OnAfterRendering
        (
            ReportContext context
        )
    {
        var afterRendering = AfterRendering;
        if (afterRendering is not null)
        {
            var eventArgs = new ReportRenderingEventArgs (context);
            afterRendering (this, eventArgs);
        }
    }

    /// <summary>
    /// Called before <see cref="Render"/>.
    /// </summary>
    protected void OnBeforeRendering
        (
            ReportContext context
        )
    {
        Sure.NotNull (context);

        if (BeforeRendering is { } beforeRendering)
        {
            var eventArgs = new ReportRenderingEventArgs (context);
            beforeRendering (this, eventArgs);

            context.OnRendering();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Clone the band.
    /// </summary>
    public virtual ReportBand Clone()
    {
        return (ReportBand) MemberwiseClone();
    }

    /// <summary>
    /// Render the band.
    /// </summary>
    public virtual void Render
        (
            ReportContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeRendering (context);

        RenderOnce (context);

        OnAfterRendering (context);
    }

    /// <summary>
    /// Render the band once (ignore records).
    /// </summary>
    public virtual void RenderOnce
        (
            ReportContext context,
            IPftFormatter? formatter = default
        )
    {
        Sure.NotNull (context);

        context.SetVariables (formatter);

        context.Index = -1;
        context.CurrentRecord = null;

        _Render (context);
    }

    /// <summary>
    /// Последовательный рендеринг всех записей,
    /// доступных в текущем контексте.
    /// </summary>
    public virtual void RenderAllRecords
        (
            ReportContext context,
            IPftFormatter? formatter = default
        )
    {
        Sure.NotNull (context);

        context.SetVariables (formatter);

        var index = 0;
        foreach (var record in context.Records)
        {
            context.CurrentRecord = record;
            context.Index = index;

            _Render (context);

            index++;
        }

        context.Index = -1;
        context.CurrentRecord = null;
    }

    /// <summary>
    /// Render given index.
    /// </summary>
    public void RenderRecord
        (
            ReportContext context,
            PftFormatter? formatter,
            int index
        )
    {
        context.Index = index;
        context.CurrentRecord = context.Records!.SafeAt (index);
        context.SetVariables (formatter);

        _Render (context);
    }

    /// <summary>
    /// Render given index.
    /// </summary>
    public void RenderRecord (ReportContext context, int index) =>
        RenderRecord (context, null, index);

    /// <summary>
    /// Should serialize <see cref="Attributes"/>?
    /// </summary>
    public bool ShouldSerializeAttributes() => Attributes.Count != 0;

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public virtual bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ReportBand> (this, throwOnError);

        verifier
            .VerifySubObject (Attributes, "attributes")
            .VerifySubObject (Cells, "cells");

        foreach (var cell in Cells)
        {
            verifier
                .ReferenceEquals
                    (
                        cell.Band,
                        this,
                        "cell.Band != this"
                    )
                .ReferenceEquals
                    (
                        cell.Report,
                        Report,
                        "cell.Report != this.Report"
                    );
        }

        // TODO Add some verification

        return verifier.Result;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public virtual void Dispose()
    {
        Magna.Logger.LogTrace (nameof (ReportBand) + "::" + nameof (Dispose));

        Cells.Dispose();
    }

    #endregion
}
