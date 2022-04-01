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

namespace ManagedIrbis.Processing;

/// <summary>
/// Пустой процессор записей.
/// </summary>
public sealed class NullRecordProcessor
    : ISyncRecordProcessor,
        IAsyncRecordProcessor
{
    #region ISyncRecordProcessor members

    /// <inheritdoc cref="ISyncRecordProcessor.ProcessOneRecord"/>
    public ProtocolLine ProcessOneRecord (Record record)
    {
        return new ();
    }

    /// <inheritdoc cref="ISyncRecordProcessor.ProcessRecords"/>
    public GblResult ProcessRecords (ISyncRecordSource source, ISyncRecordSink sync)
    {
        return new ();
    }

    #endregion

    #region IAsyncRecordProcessor members

    /// <inheritdoc cref="IAsyncRecordProcessor.ProcessOneRecordAsync"/>
    public Task<ProtocolLine> ProcessOneRecordAsync (Record record)
    {
        return Task.FromResult (new ProtocolLine());
    }

    /// <inheritdoc cref="IAsyncRecordProcessor.ProcessRecordsAsync"/>
    public Task<GblResult> ProcessRecordsAsync (ISyncRecordSource source, ISyncRecordSink sync)
    {
        return Task.FromResult (new GblResult());
    }

    #endregion

    #region IAsyncDisposable members

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    #endregion

    #region IDisposable

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // nothing to do here
    }

    #endregion
}
