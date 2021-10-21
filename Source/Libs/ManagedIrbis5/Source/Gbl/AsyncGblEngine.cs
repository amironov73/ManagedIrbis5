// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* AsyncGblEngine.cs -- асинхронная реализация по умолчанию движка пакетной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Processing;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Асинхронная реализация по умолчанию движка пакетной
    /// корректировки записей.
    /// </summary>
    public sealed class AsyncGblEngine
        : IAsyncGblEngine
    {
        #region Properties

        /// <summary>
        /// Синхронный ИРБИС-провайдер.
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
            IrbisProvider = irbisProvider;
            ServiceProvider = serviceProvider;

        } // constructor

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
            var result = new GblContext
            {
                SyncRecordSource = recordSource,
                SyncRecordSink = recordSink,
                AsyncProvider = IrbisProvider,
                Logger = ServiceProvider.GetService <ILogger<SyncGblEngine>> ()
            };

            return result;

        } // method CreateContext

        #endregion

        #region IAsyncGblEngine members

        /// <inheritdoc cref="IAsyncGblEngine.CorrectRecordsAsync"/>
        public async Task<GblResult> CorrectRecordsAsync
            (
                GblContext context,
                IReadOnlyList<GblNode> program
            )
        {
            var result = new GblResult();

            var recordSource = context.AsyncRecordSource;
            var recordSink = context.AsyncRecordSink;
            if (recordSource is not null && recordSink is not null)
            {
                var index = 0;
                var progress = ServiceProvider.GetService <IProgress<int>>();

                while (true)
                {
                    ++index;

                    var record = await recordSource.GetNextRecordAsync();
                    if (record is null)
                    {
                        break;
                    }

                    context.CurrentRecord = record;

                    foreach (var node in program)
                    {
                        await node.ExecuteAsync (context);
                    }

                    progress?.Report (index);

                } // while

                await recordSink.CompleteAsync();
                result.Protocol = await recordSink.GetProtocolAsync();

                await recordSource.DisposeAsync();
                await recordSink.DisposeAsync();
            }

            return result;

        } // method CorrectRecords

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        #endregion

    } // class AsyncGblEngine

} // namespace ManagedIrbis.Gbl
