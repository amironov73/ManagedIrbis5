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

/* BatchRecordFormatter.cs -- batch record formatter
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
    /// Batch formatter for <see cref="Record"/>.
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
        public IIrbisConnection Connection { get; }

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
        /// Constructor.
        /// </summary>
        public BatchRecordFormatter
            (
                IIrbisConnection connection,
                string database,
                string format,
                int batchSize,
                IEnumerable<int> range
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        nameof(BatchRecordFormatter) + "::Constructor: "
                        + nameof(batchSize) + "="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            Connection = connection;
            Database = database;
            BatchSize = batchSize;
            Format = format;

            _packages = range.Slice(batchSize).ToArray();
            TotalRecords = _packages.Sum(p => p.Length);
        } // constructor

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
                Magna.Error
                    (
                        nameof(BatchRecordFormatter) + "::Constructor: "
                        + "batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            Connection = ConnectionFactory.Default.CreateConnection();
            Connection.ParseConnectionString(connectionString);
            _ownConnection = true;
            Database = database;
            BatchSize = batchSize;
            Format = format;

            _packages = range.Slice(batchSize).ToArray();
            TotalRecords = _packages.Sum(p => p.Length);
        } // constructor

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
        } // method _HandleException

        #endregion

        #region Public methods

        /// <summary>
        /// Read interval of records
        /// </summary>
        public static IEnumerable<string> Interval
            (
                IIrbisConnection connection,
                string database,
                string format,
                int firstMfn,
                int lastMfn,
                int batchSize
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        nameof(BatchRecordFormatter) + "::" + nameof(Interval)
                        + ": batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            int maxMfn = connection.GetMaxMfnAsync(database).Result - 1;
            if (maxMfn == 0)
            {
                return Array.Empty<string>();
            }

            lastMfn = Math.Min(lastMfn, maxMfn);
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
                    Enumerable.Range(firstMfn, lastMfn - firstMfn + 1)
                );

            return result;
        } // method Interval

        /// <summary>
        /// Считывает все записи сразу.
        /// </summary>
        public List<string> FormatAll()
        {
            var result = new List<string>(TotalRecords);

            foreach (var record in this)
            {
                result.Add(record);
            }

            return result;
        } // method FormatAll

        /// <summary>
        /// Search and format records.
        /// </summary>
        public static IEnumerable<string> Search
            (
                IIrbisConnection connection,
                string database,
                string format,
                string searchExpression,
                int batchSize
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        nameof(BatchRecordFormatter) + "::" + nameof(Search)
                        + ": batchSize="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            int[] found = connection.SearchAsync(searchExpression).Result;
            if (found.Length == 0)
            {
                return new string[0];
            }

            var result = new BatchRecordFormatter
                (
                    connection,
                    database,
                    format,
                    batchSize,
                    found
                );

            return result;
        } // method Search

        /// <summary>
        /// Format whole database
        /// </summary>
        public static IEnumerable<string> WholeDatabase
            (
                IIrbisConnection connection,
                string database,
                string format,
                int batchSize
            )
        {
            if (batchSize < 1)
            {
                Magna.Error
                    (
                        nameof(BatchRecordFormatter) + "::" + nameof(WholeDatabase)
                        + ": " + nameof(batchSize) + "="
                        + batchSize
                    );

                throw new ArgumentOutOfRangeException(nameof(batchSize));
            }

            int maxMfn = connection.GetMaxMfnAsync(database).Result - 1;
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
                    Enumerable.Range(1, maxMfn)
                );

            return result;
        } // method WholeDatabase

        #endregion

        #region IEnumerable members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<string> GetEnumerator()
        {
            Magna.Trace
                (
                    nameof(BatchRecordFormatter) + "::" + nameof(GetEnumerator)
                    + ": start"
                );

            foreach (int[] package in _packages)
            {
                /*
                string[] records = Connection.FormatRecords
                    (
                        Database,
                        Format,
                        package
                    );
                */

                var records = new string[package.Length];

                RecordsFormatted += records.Length;
                BatchRead.Raise(this);
                foreach (string record in records)
                {
                    yield return record;
                }
            }

            Magna.Trace
                (
                    nameof(BatchRecordFormatter) + "::" + nameof(GetEnumerator)
                    + ": end"
                );

            if (_ownConnection)
            {
                Connection.Dispose();
            }
        }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } // method GetEnumerator

        #endregion

    } // class BatchRecordFormatter

} // namespace ManagedIrbis.Batch
