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
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Processing;

/// <summary>
/// Пустой приемник записей для целей отладки.
/// </summary>
public sealed class NullRecordSink
    : ISyncRecordSink,
    IAsyncRecordSink
{
    #region ISyncRecordSink members

    /// <inheritdoc cref="ISyncRecordSink.PostRecord"/>
    public void PostRecord
        (
            Record record,
            string? message = null
        )
    {
        // nothing to do here
    }

    /// <inheritdoc cref="ISyncRecordSink.Complete"/>
    public void Complete()
    {
        // nothing to do here
    }

    /// <inheritdoc cref="ISyncRecordSink.GetProtocol"/>
    public IReadOnlyList<ProtocolLine> GetProtocol()
    {
        return Array.Empty<ProtocolLine>();
    }

    /// <inheritdoc cref="ISyncRecordSink.GetProtocol"/>
    ProtocolLine[] ISyncRecordSink.GetProtocol()
    {
        return Array.Empty<ProtocolLine>();
    }

    #endregion

    #region IAsyncRecordSink members

    /// <inheritdoc cref="IAsyncRecordSink.PostRecordAsync"/>
    public Task PostRecordAsync
        (
            Record record,
            string? message = null
        )
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IAsyncRecordSink.CompleteAsync"/>
    public Task CompleteAsync()
    {
        return Task.CompletedTask;
    }

    /// <inheritdoc cref="IAsyncRecordSink.GetProtocolAsync"/>
    public Task<ProtocolLine[]> GetProtocolAsync()
    {
        return Task.FromResult (Array.Empty<ProtocolLine>());
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // nothing to do here
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
