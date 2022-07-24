// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* DynamicBand.cs -- динамическая полоса отчета
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
/// Динамическая полоса отчета. Рендеринг происходит в обработчике
/// события.
/// </summary>
public class DynamicBand
    : ReportBand
{
    #region Events

    /// <summary>
    /// Raised on band rendering.
    /// </summary>
    public event EventHandler<ReportRenderingEventArgs>? Rendering;

    #endregion

    #region ReportBand members

    /// <inheritdoc cref="ReportBand.Render" />
    public override void Render
        (
            ReportContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeRendering (context);

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
                        nameof (DynamicBand) + "::" + nameof (Render)
                    );
            }
        }

        OnAfterRendering (context);
    }

    #endregion
}
