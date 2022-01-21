// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertToLocalFunction
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

// get rid of "event never used" warning

#pragma warning disable 67

/* BatchRecordReader.cs -- пакетное чтение записей для ускорения процесса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AM;
using AM.Collections;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Batch;

/// <summary>
/// Пакетное чтение записей для ускорения процесса.
/// </summary>
public sealed class BatchRecordReader
    : IEnumerable<Record>
{
    #region Events

    /// <summary>
    /// Возбуждается при чтении очередной порции записей.
    /// </summary>
    public event EventHandler? BatchRead;

    /// <summary>
    /// Возбуждается при возникновении исключения.
    /// </summary>
    public event EventHandler<ExceptionEventArgs<Exception>>? Exception;

    /// <summary>
    /// Возбуждается по окончании чтения записей.
    /// </summary>
    public event EventHandler? ReadComplete;

    #endregion

    #region Properties

    /// <summary>
    /// Размер порции записей.
    /// </summary>
    public int BatchSize { get; }

    /// <summary>
    /// Синхронный провайдер.
    /// </summary>
    public ISyncProvider Connection { get; }

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    public string Database { get; }

    /// <summary>
    /// Пропускать удаленные записи?
    /// </summary>
    public bool OmitDeletedRecords { get; set; }

    /// <summary>
    /// Количество записей, выданных потребителю к данному моменту.
    /// </summary>
    public int RecordsRead { get; private set; }

    /// <summary>
    /// Количество записей, подлежащих чтению.
    /// </summary>
    public int TotalRecords { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BatchRecordReader
        (
            ISyncProvider connection,
            IEnumerable<int> range,
            int batchSize = 500,
            string? database = null,
            bool omitDeletedRecords = true
        )
    {
        if (batchSize < 1)
        {
            Magna.Error
                (
                    nameof (BatchRecordReader) + "::Constructor"
                                               + ": batchSize="
                                               + batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        Connection = connection;
        Database = connection.EnsureDatabase (database);
        BatchSize = batchSize;
        OmitDeletedRecords = omitDeletedRecords;

        _chunks = range.Chunk (batchSize).ToArray();
        TotalRecords = _chunks.Sum (p => p.Length);
    }

    #endregion

    #region Private members

    private readonly int[][] _chunks;

    private bool _HandleException
        (
            Exception exception
        )
    {
        var handler = Exception;

        if (handler is null)
        {
            return false;
        }

        var arguments = new ExceptionEventArgs<Exception> (exception);
        handler (this, arguments);

        return arguments.Handled;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Чтение интервала записей.
    /// </summary>
    public static IEnumerable<Record> Interval
        (
            ISyncProvider connection,
            int firstMfn = 1,
            int lastMfn = 0,
            string? database = null,
            int batchSize = 500,
            bool omitDeletedRecords = true,
            Action<BatchRecordReader>? action = null
        )
    {
        if (batchSize < 1)
        {
            Magna.Error
                (
                    nameof (BatchRecordReader) + "::" + nameof (Interval)
                    + ": batchSize="
                    + batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        database = connection.EnsureDatabase (database);

        var maxMfn = connection.GetMaxMfn (database) - 1;
        if (maxMfn == 0)
        {
            return Array.Empty<Record>();
        }

        lastMfn = Math.Min (lastMfn, maxMfn);
        if (firstMfn > lastMfn)
        {
            return Array.Empty<Record>();
        }

        var result = new BatchRecordReader
            (
                connection,
                Enumerable.Range (firstMfn, lastMfn - firstMfn + 1),
                batchSize,
                database,
                omitDeletedRecords
            );

        if (action is not null)
        {
            void BatchHandler (object? o, EventArgs eventArgs) => action (result);
            result.BatchRead += BatchHandler;

            void CompleteHandler (object? o, EventArgs eventArgs) => result.BatchRead -= BatchHandler;
            result.ReadComplete += CompleteHandler;
        }

        return result;
    }

    /// <summary>
    /// Считывает все записи сразу.
    /// </summary>
    public List<Record> ReadAll
        (
            bool omitDeletedRecords = true
        )
    {
        var result = new List<Record> (TotalRecords);

        foreach (var record in this)
        {
            if (omitDeletedRecords && record.Deleted)
            {
                continue;
            }

            result.Add (record);
        }

        return result;
    }

    /// <summary>
    /// Чтение записей, соответствующих запросу.
    /// </summary>
    public static IEnumerable<Record> Search
        (
            ISyncProvider connection,
            string searchExpression,
            string? database = null,
            int batchSize = 500,
            Action<BatchRecordReader>? action = null
        )
    {
        if (batchSize < 1)
        {
            Magna.Error
                (
                    nameof (BatchRecordReader) + "::" + nameof (Search)
                    + ": batchSize="
                    + batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        var parameters = new SearchParameters
        {
            Database = database,
            Expression = searchExpression
        };
        var found = connection.Search (parameters);
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<Record>();
        }

        var range = FoundItem.ToMfn (found);
        var result = new BatchRecordReader
            (
                connection,
                range,
                batchSize,
                database
            );

        if (action is not null)
        {
            void BatchHandler (object? o, EventArgs eventArgs) => action (result);
            result.BatchRead += BatchHandler;

            void CompleteHandler (object? o, EventArgs eventArgs) => result.BatchRead -= BatchHandler;
            result.ReadComplete += CompleteHandler;
        }

        return result;
    }

    /// <summary>
    /// Чтение всей базы данных.
    /// </summary>
    public static IEnumerable<Record> WholeDatabase
        (
            ISyncProvider connection,
            string? database = null,
            int batchSize = 500,
            bool omitDeletedRecords = true,
            Action<BatchRecordReader>? action = null
        )
    {
        if (batchSize < 1)
        {
            Magna.Error
                (
                    nameof (BatchRecordReader) + "::" + nameof (WholeDatabase)
                    + ": batchSize="
                    + batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        database = connection.EnsureDatabase (database);
        var maxMfn = connection.GetMaxMfn (database) - 1;
        if (maxMfn == 0)
        {
            return Array.Empty<Record>();
        }

        var result = new BatchRecordReader
            (
                connection,
                Enumerable.Range (1, maxMfn),
                batchSize,
                database,
                omitDeletedRecords
            );

        if (action is not null)
        {
            void BatchHandler (object? o, EventArgs eventArgs) => action (result);
            result.BatchRead += BatchHandler;

            void CompleteHandler (object? o, EventArgs eventArgs) => result.BatchRead -= BatchHandler;
            result.ReadComplete += CompleteHandler;
        }

        return result;
    } // method WholeDatabase

    /// <summary>
    /// Read whole database
    /// </summary>
    public static IEnumerable<Record> WholeDatabase
        (
            ISyncProvider connection,
            string database,
            int batchSize,
            Action<BatchRecordReader>? action
        )
    {
        var result = (BatchRecordReader)WholeDatabase
            (
                connection,
                database,
                batchSize
            );

        if (!ReferenceEquals (action, null))
        {
            EventHandler batchHandler = (_, _) => action (result);
            result.BatchRead += batchHandler;
        }

        return result;
    }

    #endregion

    #region IEnumerable members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<Record> GetEnumerator()
    {
        Magna.Trace (nameof (BatchRecordReader) + "::" + nameof (GetEnumerator) + ": start");

        foreach (var package in _chunks)
        {
            Record[]? records = null;
            try
            {
                records = Connection.ReadRecords
                    (
                        Database,
                        package
                    );
                RecordsRead += records?.Length ?? 0;
                BatchRead.Raise (this);
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        nameof (BatchRecordReader) + "::" + nameof (GetEnumerator),
                        exception
                    );

                if (!_HandleException (exception))
                {
                    throw;
                }
            }

            if (records is not null)
            {
                foreach (var record in records)
                {
                    if (OmitDeletedRecords && record.Deleted)
                    {
                        continue;
                    }

                    yield return record;
                }
            }
        }

        ReadComplete.Raise (this);

        Magna.Trace (nameof (BatchRecordReader) + "::" + nameof (GetEnumerator) + ": end");
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
