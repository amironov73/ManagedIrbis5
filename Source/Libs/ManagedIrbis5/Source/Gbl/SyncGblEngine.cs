// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* SyncGblEngine.cs -- синхронная реализация по умолчанию движка пакетной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Processing;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
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
                SyncProvider = IrbisProvider,
                Logger = ServiceProvider.GetService <ILogger<SyncGblEngine>> ()
            };

            return result;

        } // method CreateContext

        #endregion

        #region ISyncGblEngine members

        /// <inheritdoc cref="ISyncGblEngine.CorrectRecords"/>
        public GblResult CorrectRecords
            (
                GblContext context,
                IReadOnlyList<GblNode> program
            )
        {
            var result = new GblResult();

            var recordSource = context.SyncRecordSource;
            var recordSink = context.SyncRecordSink;
            if (recordSource is not null && recordSink is not null)
            {
                var index = 0;
                var progress = ServiceProvider.GetService <IProgress<int>>();

                while (true)
                {
                    ++index;

                    var record = recordSource.GetNextRecord();
                    if (record is null)
                    {
                        break;
                    }

                    context.CurrentRecord = record;

                    foreach (var node in program)
                    {
                        node.Execute (context);
                    }

                    progress?.Report (index);

                } // while

                recordSink.Complete();
                result.Protocol = recordSink.GetProtocol();

                recordSource.Dispose();
                recordSink.Dispose();

            } // if

            return result;

        } // method CorrectRecords

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() { }

        #endregion

    } // class SyncGblEngine

} // namespace ManagedIrbis.Gbl
