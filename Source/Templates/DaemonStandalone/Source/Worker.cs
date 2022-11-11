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

using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace DaemonStandalone;

/// <summary>
/// Фоновая функциональность
/// </summary>
public sealed class Worker
    : BackgroundService
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Worker
        (
            ILogger<Worker> logger
        )
    {
        _logger = logger;
    }

    #endregion

    #region Private members

    private readonly ILogger<Worker> _logger;

    #endregion

    #region BackgroundService members

    /// <inheritdoc cref="BackgroundService.ExecuteAsync"/>
    protected override async Task ExecuteAsync
        (
            CancellationToken stoppingToken
        )
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation ("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay (1000, stoppingToken);
        }
    }

    #endregion
}
