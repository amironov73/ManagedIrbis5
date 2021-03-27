// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BatchRecordReader.cs -- batch record reader
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Batch reader for <see cref="Record"/>.
    /// </summary>
    public sealed class BatchRecordReader
        : IEnumerable<Record>
    {
        #region Events

        /// <summary>
        /// Raised on batch reading.
        /// </summary>
#pragma warning disable 67
        public event EventHandler? BatchRead;
#pragma warning restore 67

        /// <summary>
        /// Raised when exception occurs.
        /// </summary>
        public event EventHandler<ExceptionEventArgs<Exception>>? Exception;

        /// <summary>
        /// Raised when all data read.
        /// </summary>
        public event EventHandler? ReadComplete;

        #endregion

        #region Properties

        /// <summary>
        /// Batch size.
        /// </summary>
        public int BatchSize { get; private set; }

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncIrbisProvider Connection { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// Omit deleted records?
        /// </summary>
        public bool OmitDeletedRecords { get; set; }

        /// <summary>
        /// Total number of records read.
        /// </summary>
        public int RecordsRead { get; private set; }

        /// <summary>
        /// Number of records to read.
        /// </summary>
        public int TotalRecords { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchRecordReader
            (
                ISyncIrbisProvider connection,
                string database,
                int batchSize,
                IEnumerable<int> range
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        "BatchRecordReader::Constructor: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            Connection = connection;
            Database = database;
            BatchSize = batchSize;

            _packages = range.Slice(batchSize).ToArray();
            TotalRecords = _packages.Sum(p => p.Length);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchRecordReader
            (
                ISyncIrbisProvider connection,
                string database,
                int batchSize,
                bool omitDeletedRecords,
                IEnumerable<int> range
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        "BatchRecordReader::Constructor: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            Connection = connection;
            Database = database;
            BatchSize = batchSize;
            OmitDeletedRecords = omitDeletedRecords;

            _packages = range.Slice(batchSize).ToArray();
            TotalRecords = _packages.Sum(p => p.Length);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchRecordReader
            (
                string connectionString,
                string database,
                int batchSize,
                bool omitDeletedRecords,
                IEnumerable<int> range
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        "BatchRecordReader::Constructor: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            Connection = ConnectionFactory.Shared.CreateSyncConnection();
            Connection.Configure(connectionString);
            _ownConnection = true;
            Database = database;
            BatchSize = batchSize;
            OmitDeletedRecords = omitDeletedRecords;

            _packages = range.Slice(batchSize).ToArray();
            TotalRecords = _packages.Sum(p => p.Length);
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

            if (ReferenceEquals(handler, null))
            {
                return false;
            }

            var arguments = new ExceptionEventArgs<Exception>(exception);
            handler(this, arguments);

            return arguments.Handled;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Read interval of records
        /// </summary>
        public static IEnumerable<Record> Interval
            (
                ISyncIrbisProvider connection,
                string database,
                int firstMfn,
                int lastMfn,
                int batchSize
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        "BatchRecordReader::Interval: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            var maxMfn = connection.GetMaxMfn(database) - 1;
            if (maxMfn == 0)
            {
                return Array.Empty<Record>();
            }

            lastMfn = Math.Min(lastMfn, maxMfn);
            if (firstMfn > lastMfn)
            {
                return Array.Empty<Record>();
            }

            var result = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    Enumerable.Range(firstMfn, lastMfn - firstMfn + 1)
                );

            return result;
        }

        /// <summary>
        /// Read interval of records
        /// </summary>
        public static IEnumerable<Record> Interval
            (
                ISyncIrbisProvider connection,
                string database,
                int firstMfn,
                int lastMfn,
                int batchSize,
                bool omitDeletedRecords,
                Action<BatchRecordReader>? action
            )
        {
            BatchRecordReader result = (BatchRecordReader)Interval
                (
                    connection,
                    database,
                    firstMfn,
                    lastMfn,
                    batchSize
                );
            result.OmitDeletedRecords = omitDeletedRecords;

            if (!ReferenceEquals(action, null))
            {
                EventHandler batchHandler
                    = (sender, args) => action(result);
                result.BatchRead += batchHandler;

                EventHandler completeHandler = (sender, args) =>
                {
                    result.BatchRead -= batchHandler;
                };
                result.ReadComplete += completeHandler;
            }

            return result;
        }

        /// <summary>
        /// Считывает все записи сразу.
        /// </summary>
        public List<Record> ReadAll()
        {
            var result = new List<Record>(TotalRecords);

            foreach (var record in this)
            {
                result.Add(record);
            }

            return result;
        }

        /// <summary>
        /// Считывает все записи сразу.
        /// </summary>
        public List<Record> ReadAll
            (
                bool omitDeletedRecords
            )
        {
            var result = new List<Record>(TotalRecords);

            foreach (var record in this)
            {
                if (omitDeletedRecords && record.Deleted)
                {
                    continue;
                }

                result.Add(record);
            }

            return result;
        }

        /// <summary>
        /// Search and read records.
        /// </summary>
        public static IEnumerable<Record> Search
            (
                ISyncIrbisProvider connection,
                string database,
                string searchExpression,
                int batchSize
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        "BatchRecordReader::Search: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            var found = connection.Search(searchExpression);
            if (found.Length == 0)
            {
                return Array.Empty<Record>();
            }

            var reader = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    found
                );

            return reader;
        }

        /// <summary>
        /// Search and read records.
        /// </summary>
        public static IEnumerable<Record> Search
            (
                ISyncIrbisProvider connection,
                string database,
                string searchExpression,
                int batchSize,
                Action<BatchRecordReader>? action
            )
        {
            var result = (BatchRecordReader)Search
                (
                    connection,
                    database,
                    searchExpression,
                    batchSize
                );

            if (!ReferenceEquals(action, null))
            {
                EventHandler batchHandler = (sender, args) => action(result);
                result.BatchRead += batchHandler;
            }

            return result;
        }

        /// <summary>
        /// Read whole database
        /// </summary>
        public static IEnumerable<Record> WholeDatabase
            (
                ISyncIrbisProvider connection,
                string database,
                int batchSize
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        "BatchRecordReader::WholeDatabase: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            int maxMfn = connection.GetMaxMfn(database) - 1;
            if (maxMfn == 0)
            {
                return Array.Empty<Record>();
            }

            var result = new BatchRecordReader
                (
                    connection,
                    database,
                    batchSize,
                    Enumerable.Range(1, maxMfn)
                );

            return result;
        }

        /// <summary>
        /// Read whole database
        /// </summary>
        public static IEnumerable<Record> WholeDatabase
            (
                ISyncIrbisProvider connection,
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

            if (!ReferenceEquals(action, null))
            {
                EventHandler batchHandler = (sender, args) => action(result);
                result.BatchRead += batchHandler;
            }

            return result;
        }

        /// <summary>
        /// Read whole database
        /// </summary>
        public static IEnumerable<Record> WholeDatabase
            (
                ISyncIrbisProvider connection,
                string database,
                int batchSize,
                bool omitDeletedRecords,
                Action<BatchRecordReader>? action
            )
        {
            var result = (BatchRecordReader)WholeDatabase
                (
                    connection,
                    database,
                    batchSize
                );
            result.OmitDeletedRecords = omitDeletedRecords;

            if (!ReferenceEquals(action, null))
            {
                EventHandler batchHandler
                    = (sender, args) => action(result);
                result.BatchRead += batchHandler;
            }

            return result;
        }

        #endregion

        #region IEnumerable members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Record> GetEnumerator()
        {
            Magna.Trace("BatchRecordReader::GetEnumerator: start");

            /*
            foreach (int[] package in _packages)
            {
                Record[]? records = null;
                try
                {
                    records = Connection.ReadRecords
                        (
                            Database,
                            package
                        );
                    RecordsRead += records.Length;
                    BatchRead.Raise(this);
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "BatchRecordReader::GetEnumerator",
                            exception
                        );

                    if (!_HandleException(exception))
                    {
                        throw;
                    }
                }

                if (!ReferenceEquals(records, null))
                {
                    foreach (var record in records)
                    {
                        if (OmitDeletedRecords
                            && record.Deleted)
                        {
                            continue;
                        }

                        yield return record;
                    }
                }
                */

            Magna.Trace("BatchRecordReader::GetEnumerator: end");

            ReadComplete.Raise(this);

            if (_ownConnection)
            {
                Connection.Dispose();
            }

            yield break;
        }


        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    } // class BatchRecordReader

} // namespace ManagedIrbis.Batch
