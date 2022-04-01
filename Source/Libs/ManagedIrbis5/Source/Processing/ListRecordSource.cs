// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ListRecordSource.cs -- синхронный источник записей в оперативной памяти
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
/// Источник записей в оперативной памяти.
/// </summary>
public sealed class ListRecordSource
    : ISyncRecordSource,
        IAsyncRecordSource
{
    #region Properties

    /// <summary>
    /// Список записей.
    /// </summary>
    public IReadOnlyList<Record> RecordList { get; }

    /// <summary>
    /// Текущий индекс.
    /// </summary>
    public int Index { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ListRecordSource
        (
            IReadOnlyList<Record> recordList
        )
    {
        _syncRoot = new object();
        RecordList = recordList;
        Index = -1;
    }

    #endregion

    #region Private members

    private readonly object _syncRoot;

    #endregion

    #region ISyncRecordSource members

    /// <inheritdoc cref="ISyncRecordSource.GetNextRecord"/>
    public Record? GetNextRecord()
    {
        lock (_syncRoot)
        {
            if (++Index >= RecordList.Count)
            {
                return null;
            }

            var result = RecordList[Index];

            return result;
        }
    }

    /// <inheritdoc cref="ISyncRecordSource.GetRecordCount"/>
    public int GetRecordCount()
    {
        lock (_syncRoot)
        {
            return RecordList.Count;
        }
    }

    #endregion

    #region IAsyncRecordSource members

    /// <inheritdoc cref="IAsyncRecordSource.GetNextRecordAsync"/>
    public Task<Record?> GetNextRecordAsync()
    {
        lock (_syncRoot)
        {
            if (++Index >= RecordList.Count)
            {
                return Task.FromResult<Record?> (null);
            }

            var result = RecordList[Index];

            return Task.FromResult<Record?> (result);
        }
    }

    /// <inheritdoc cref="IAsyncRecordSource.GetRecordCountAsync"/>
    public Task<int> GetRecordCountAsync()
    {
        lock (_syncRoot)
        {
            return Task.FromResult (RecordList.Count);
        }
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
