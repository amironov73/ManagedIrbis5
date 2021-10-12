// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* NullRecordSink.cs -- пустой приемник записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure
{
    /// <summary>
    /// Пустой приемник записей для целей отладки.
    /// </summary>
    public sealed class NullRecordSink
        : ISyncRecordSink,
          IAsyncRecordSink
    {
        #region ISyncRecordSink members

        /// <inheritdoc cref="ISyncRecordSink.PostRecord"/>
        public void PostRecord (Record record, string? message = null) {}

        /// <inheritdoc cref="ISyncRecordSink.Complete"/>
        public void Complete() {}

        #endregion

        #region IAsyncRecordSink members

        /// <inheritdoc cref="IAsyncRecordSink.PostRecordAsync"/>
        public Task PostRecordAsync (Record record, string? message = null) => Task.CompletedTask;

        /// <inheritdoc cref="IAsyncRecordSink.CompleteAsync"/>
        public Task CompleteAsync() => Task.CompletedTask;

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() {}

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        #endregion

    } // class NullRecordSink

} // namespace ManagedIrbis.Gbl.Infrastructure
