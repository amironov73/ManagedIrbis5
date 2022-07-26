// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable RedundantAssignment
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#pragma warning disable 649

/* BatchRecordFormatter.cs -- пакетное форматирование записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Batch;

/// <summary>
/// Пакетное форматирование записей <see cref="Record"/> на ИРБИС-сервере.
/// </summary>
public sealed class BatchRecordFormatter
    : IEnumerable<string>
{
    #region Events

    /// <summary>
    /// Raised on batch reading.
    /// </summary>
    public event EventHandler? BatchRead;

    /// <summary>
    /// Raised when exception occurs.
    /// </summary>
    public event EventHandler<ExceptionEventArgs<Exception>>? Exception;

    #endregion

    #region Properties

    /// <summary>
    /// Batch size.
    /// </summary>
    public int BatchSize { get; }

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider Connection { get; }

    /// <summary>
    /// Database name.
    /// </summary>
    public string Database { get; }

    /// <summary>
    /// Format.
    /// </summary>
    public string Format { get; }

    /// <summary>
    /// Total number of records formatted.
    /// </summary>
    public int RecordsFormatted { get; private set; }

    /// <summary>
    /// Number of records to format.
    /// </summary>
    public int TotalRecords { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BatchRecordFormatter
        (
            ISyncProvider connection,
            string database,
            string format,
            int batchSize,
            IEnumerable<int>? range
        )
    {
        if (batchSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchRecordFormatter) + "::Constructor"
                    + ": batchsize={BatchSize}",
                    batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        Connection = connection;
        Database = database;
        BatchSize = batchSize;
        Format = format;

        range ??= Array.Empty<int>();
        _packages = range.Chunk (batchSize).ToArray();
        TotalRecords = _packages.Sum (p => p.Length);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public BatchRecordFormatter
        (
            string connectionString,
            string database,
            string format,
            int batchSize,
            IEnumerable<int> range
        )
    {
        if (batchSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchRecordFormatter) + "::Constructor"
                    + ": batchSize={BatchSize}",
                    batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        Connection = ConnectionFactory.Shared.CreateSyncConnection();
        Connection.Configure (connectionString);
        _ownConnection = true;
        Database = database;
        BatchSize = batchSize;
        Format = format;

        _packages = range.Chunk (batchSize).ToArray();
        TotalRecords = _packages.Sum (p => p.Length);
    }

    #endregion

    #region Private members

    private readonly bool _ownConnection;

    private readonly int[][] _packages;

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
    /// Read interval of records
    /// </summary>
    public static IEnumerable<string> Interval
        (
            ISyncProvider connection,
            string database,
            string format,
            int firstMfn,
            int lastMfn,
            int batchSize
        )
    {
        if (batchSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchRecordFormatter) + "::" + nameof (Interval)
                    + ": batchSize={BatchSize}",
                    batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        int maxMfn = connection.GetMaxMfn (database) - 1;
        if (maxMfn == 0)
        {
            return Array.Empty<string>();
        }

        lastMfn = Math.Min (lastMfn, maxMfn);
        if (firstMfn > lastMfn)
        {
            return Array.Empty<string>();
        }

        var result = new BatchRecordFormatter
            (
                connection,
                database,
                format,
                batchSize,
                Enumerable.Range (firstMfn, lastMfn - firstMfn + 1)
            );

        return result;
    }

    /// <summary>
    /// Считывает все записи сразу.
    /// </summary>
    public List<string> FormatAll()
    {
        var result = new List<string> (TotalRecords);

        foreach (var record in this)
        {
            result.Add (record);
        }

        return result;
    }

    /// <summary>
    /// Search and format records.
    /// </summary>
    public static IEnumerable<string> Search
        (
            ISyncProvider connection,
            string database,
            string format,
            string searchExpression,
            int batchSize
        )
    {
        if (batchSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchRecordFormatter) + "::" + nameof (Search)
                    + ": batchSize={BatchSize}",
                    batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        var parameters = new SearchParameters
        {
            Database = database,
            Expression = searchExpression
        };
        var found = connection.Search (parameters);
        if (found?.Length == 0)
        {
            return Array.Empty<string>();
        }

        var range = FoundItem.ToMfn (found);
        var result = new BatchRecordFormatter
            (
                connection,
                database,
                format,
                batchSize,
                range
            );

        return result;
    }

    /// <summary>
    /// Format whole database
    /// </summary>
    public static IEnumerable<string> WholeDatabase
        (
            ISyncProvider connection,
            string database,
            string format,
            int batchSize
        )
    {
        if (batchSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchRecordFormatter) + "::" + nameof (WholeDatabase)
                    + ": batchSize={BatchSize}",
                    batchSize
                );

            throw new ArgumentOutOfRangeException (nameof (batchSize));
        }

        int maxMfn = connection.GetMaxMfn (database) - 1;
        if (maxMfn == 0)
        {
            return Array.Empty<string>();
        }

        var result = new BatchRecordFormatter
            (
                connection,
                database,
                format,
                batchSize,
                Enumerable.Range (1, maxMfn)
            );

        return result;
    }

    #endregion

    #region IEnumerable members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<string> GetEnumerator()
    {
        Magna.Logger.LogTrace (nameof (BatchRecordFormatter) + "::" + nameof (GetEnumerator) + ": start");

        foreach (var package in _packages)
        {
            var parameters = new FormatRecordParameters
            {
                Database = Database,
                Format = Format,
                Mfns = package
            };
            if (!Connection.FormatRecords (parameters))
            {
                yield break;
            }

            var records = parameters.Result.AsArray();
            RecordsFormatted += records.Length;
            BatchRead.Raise (this);
            foreach (var record in records)
            {
                yield return record;
            }
        }

        Magna.Logger.LogTrace (nameof (BatchRecordFormatter) + "::" + nameof (GetEnumerator) + ": end");

        if (_ownConnection)
        {
            Connection.Dispose();
        }
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
