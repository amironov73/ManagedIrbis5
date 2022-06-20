// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AsyncGblEngine.cs -- асинхронная реализация по умолчанию движка пакетной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Processing;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl;

/// <summary>
/// Асинхронная реализация по умолчанию движка пакетной
/// корректировки записей.
/// </summary>
public sealed class AsyncGblEngine
    : IAsyncGblEngine
{
    #region Properties

    /// <summary>
    /// Асинхронный ИРБИС-провайдер.
    /// </summary>
    public IAsyncProvider IrbisProvider { get; }

    /// <summary>
    /// Провайдер различных сервисов,
    /// которые могут понадобиться в процессе корректировки.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AsyncGblEngine
        (
            IAsyncProvider irbisProvider,
            IServiceProvider serviceProvider
        )
    {
        Sure.NotNull (irbisProvider);
        Sure.NotNull (serviceProvider);

        IrbisProvider = irbisProvider;
        ServiceProvider = serviceProvider;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание контекста.
    /// </summary>
    public GblContext CreateContext
        (
            IAsyncRecordSource recordSource,
            IAsyncRecordSink recordSink
        )
    {
        Sure.NotNull (recordSource);
        Sure.NotNull (recordSink);

        var result = new GblContext
        {
            AsyncRecordSource = recordSource,
            AsyncRecordSink = recordSink,
            AsyncProvider = IrbisProvider,
            Logger = ServiceProvider.GetService<ILogger<SyncGblEngine>>()
        };

        return result;
    }

    #endregion

    #region IAsyncGblEngine members

    /// <inheritdoc cref="IAsyncGblEngine.CorrectRecordsAsync"/>
    public async Task<GblResult> CorrectRecordsAsync
        (
            GblContext context,
            IReadOnlyList<GblNode> program
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (program);

        var result = new GblResult();

        var recordSource = context.AsyncRecordSource;
        var recordSink = context.AsyncRecordSink;
        if (recordSource is not null && recordSink is not null)
        {
            var index = 0;
            var progress = ServiceProvider.GetService<IProgress<int>>();

            while (true)
            {
                ++index;

                var record = await recordSource.GetNextRecordAsync();
                if (record is null)
                {
                    break;
                }

                result.RecordsSupposed++;
                context.CurrentRecord = record;

                foreach (var node in program)
                {
                    await node.ExecuteAsync (context);
                }

                result.RecordsProcessed++;
                result.RecordsSucceeded++;
                progress?.Report (index);
            }

            await recordSink.CompleteAsync();
            result.Protocol = await recordSink.GetProtocolAsync();

            await recordSource.DisposeAsync();
            await recordSink.DisposeAsync();
        }

        return result;
    }

    #endregion

    #region IAsyncDisposable members

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    #endregion
}
