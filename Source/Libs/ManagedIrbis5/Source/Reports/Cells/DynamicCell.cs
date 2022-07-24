// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* DynamicCell.cs -- динамическая ячейка
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
/// Динамическая ячейка отчета. Рендеринг происходит в обработчике
/// события.
/// </summary>
public class DynamicCell
    : ReportCell
{
    #region Events

    /// <summary>
    /// Raised on cell computation.
    /// </summary>
    public event EventHandler<ReportComputeEventArgs>? Computation;

    /// <summary>
    /// Raised on cell rendering.
    /// </summary>
    public event EventHandler<ReportRenderingEventArgs>? Rendering;

    #endregion

    #region ReportCell members

    /// <inheritdoc cref="ReportCell.Compute"/>
    public override string? Compute
        (
            ReportContext context
        )
    {
        Sure.NotNull (context);

        ReportComputeEventArgs? eventArgs = null;

        OnBeforeCompute (context);

        var computation = Computation;
        if (computation is not null)
        {
            try
            {
                eventArgs = new ReportComputeEventArgs (context);
                computation (this, eventArgs);
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (DynamicCell) + "::" + nameof (Compute)
                    );
            }
        }

        OnAfterCompute (context);

        return eventArgs?.Result;
    }

    /// <inheritdoc cref="ReportCell.Render" />
    public override void Render
        (
            ReportContext context
        )
    {
        var rendering = Rendering;
        if (rendering is not null)
        {
            try
            {
                var eventArgs = new ReportRenderingEventArgs (context);
                rendering (this, eventArgs);
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (DynamicCell) + "::" + nameof (Render)
                    );
            }
        }
    }

    #endregion
}
