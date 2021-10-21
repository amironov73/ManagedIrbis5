// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* NullRecordSource.cs -- пустой источник записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Processing
{
    /// <summary>
    /// Пустой источник записей для целей отладки.
    /// </summary>
    public sealed class NullRecordSource
        : ISyncRecordSource,
          IAsyncRecordSource
    {
        #region ISyncRecordSource members

        /// <inheritdoc cref="ISyncRecordSource.GetNextRecord"/>
        public Record? GetNextRecord() => null;

        /// <inheritdoc cref="ISyncRecordSource.GetRecordCount"/>
        public int GetRecordCount() => 0;

        #endregion

        #region IAsyncRecordSource members

        /// <inheritdoc cref="IAsyncRecordSource.GetNextRecordAsync"/>
        public Task<Record?> GetNextRecordAsync() => Task.FromResult<Record?> (null);

        /// <inheritdoc cref="IAsyncRecordSource.GetRecordCountAsync"/>
        /// <returns></returns>
        public Task<int> GetRecordCountAsync() => Task.FromResult (0);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() {}

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        #endregion

    } // class NullRecordSource

} // namespace ManagedIrbis.Processing
