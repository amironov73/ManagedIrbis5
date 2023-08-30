// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* NullHost.cs -- пустой хост, не выполняющий никаких полезных действий
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;

using AM.ComponentModel;

using Microsoft.Extensions.Hosting;

#endregion

namespace AM.Hosting;

/// <summary>
/// Пустой хост, не выполняющий никаких полезных действий.
/// </summary>
public sealed class NullHost
    : IHost
{
    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода
    }

    #endregion

    #region IHost members

    /// <inheritdoc cref="IHost.StartAsync"/>
    public Task StartAsync
        (
            CancellationToken cancellationToken
        )
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IHost.StopAsync"/>
    public Task StopAsync
        (
            CancellationToken cancellationToken
        )
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IHost.Services"/>
    public IServiceProvider Services { get; } = new ServiceAggregator();

    #endregion
}
