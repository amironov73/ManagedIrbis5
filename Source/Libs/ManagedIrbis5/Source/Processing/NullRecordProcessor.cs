// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* NullRecordProcessor.cs -- пустой процессор записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using ManagedIrbis.Gbl;

#endregion

#nullable enable

namespace ManagedIrbis.Processing
{
    /// <summary>
    /// Пустой процессор записей.
    /// </summary>
    public sealed class NullRecordProcessor
        : ISyncRecordProcessor,
        IAsyncRecordProcessor
    {
        #region ISyncRecordProcessor members

        /// <inheritdoc cref="ISyncRecordProcessor.ProcessOneRecord"/>
        public GblProtocolLine ProcessOneRecord (Record record) => new ();

        /// <inheritdoc cref="ISyncRecordProcessor.ProcessRecords"/>
        public GblResult ProcessRecords (ISyncRecordSource source, ISyncRecordSink sync) => new ();

        #endregion

        #region IAsyncRecordProcessor members

        /// <inheritdoc cref="IAsyncRecordProcessor.ProcessOneRecordAsync"/>
        public Task<GblProtocolLine> ProcessOneRecordAsync (Record record) =>
            Task.FromResult (new GblProtocolLine());

        /// <inheritdoc cref="IAsyncRecordProcessor.ProcessRecordsAsync"/>
        public Task<GblResult> ProcessRecordsAsync (ISyncRecordSource source, ISyncRecordSink sync) =>
            Task.FromResult (new GblResult());

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        #endregion

        #region IDisposable

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
        } // method Dispose

        #endregion

    } // class NullRecordProcessor

} // namespace ManagedIrbis.Processing
