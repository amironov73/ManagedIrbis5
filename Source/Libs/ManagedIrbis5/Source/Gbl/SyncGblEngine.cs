// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SyncGblEngine.cs -- синхронная реализация по умолчанию движка пакетной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Processing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl;

/// <summary>
/// Синхронная реализация по умолчанию движка пакетной
/// корректировки записей.
/// </summary>
public sealed class SyncGblEngine
    : ISyncGblEngine
{
    #region Properties

    /// <summary>
    /// Синхронный ИРБИС-провайдер.
    /// </summary>
    public ISyncProvider IrbisProvider { get; }

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
    public SyncGblEngine
        (
            ISyncProvider irbisProvider,
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
            ISyncRecordSource recordSource,
            ISyncRecordSink recordSink
        )
    {
        Sure.NotNull (recordSource);
        Sure.NotNull (recordSink);

        var result = new GblContext
        {
            SyncRecordSource = recordSource,
            SyncRecordSink = recordSink,
            SyncProvider = IrbisProvider,
            Logger = ServiceProvider.GetService<ILogger<SyncGblEngine>>()
        };

        return result;
    }

    #endregion

    #region ISyncGblEngine members

    /// <inheritdoc cref="ISyncGblEngine.CorrectRecords"/>
    public GblResult CorrectRecords
        (
            GblContext context,
            IReadOnlyList<GblNode> program
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (program);

        var result = new GblResult();

        var recordSource = context.SyncRecordSource;
        var recordSink = context.SyncRecordSink;
        if (recordSource is not null && recordSink is not null)
        {
            var index = 0;
            var progress = ServiceProvider.GetService<IProgress<int>>();

            while (true)
            {
                ++index;

                var record = recordSource.GetNextRecord();
                if (record is null)
                {
                    break;
                }

                result.RecordsSupposed++;
                context.CurrentRecord = record;

                foreach (var node in program)
                {
                    node.Execute (context);
                }

                result.RecordsProcessed++;
                result.RecordsSucceeded++;
                progress?.Report (index);
            }

            recordSink.Complete();
            result.Protocol = recordSink.GetProtocol();

            recordSource.Dispose();
            recordSink.Dispose();
        }

        return result;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода
    }

    #endregion
}

