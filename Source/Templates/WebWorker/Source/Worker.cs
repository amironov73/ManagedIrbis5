// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#nullable enable

namespace WebWorker;

internal sealed class Worker
    : BackgroundService
{
    #region Construction

    public Worker
        (
            ILogger<Worker> logger
        )
    {
        _logger = logger;
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    #endregion

    #region BackgroundService members

    /// <inheritdoc cref="BackgroundService.ExecuteAsync"/>
    protected override async Task ExecuteAsync
        (
            CancellationToken stoppingToken
        )
    {
        _logger.LogInformation ("Worker is starting");

        stoppingToken.Register (() => _logger.LogInformation ("Worker is stopping"));

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation ("Worker is doing background work");

            await Task.Delay (TimeSpan.FromSeconds (5), stoppingToken);
        }

        _logger.LogInformation ("Worker has stopped");
    }

    #endregion
}
