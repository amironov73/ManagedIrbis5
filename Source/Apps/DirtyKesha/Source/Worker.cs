// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Worker.cs -- фоновая функциональность
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

#endregion

#nullable enable

namespace DirtyKesha;

/// <summary>
/// Фоновая функциональность.
/// </summary>
public sealed class Worker
    : BackgroundService
{
    #region BackgroundService members

    /// <inheritdoc cref="BackgroundService.ExecuteAsync"/>
    protected override async Task ExecuteAsync
        (
            CancellationToken stoppingToken
        )
    {
        Client.RunBot (stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay (1000, stoppingToken);
        }
    }

    #endregion
}
