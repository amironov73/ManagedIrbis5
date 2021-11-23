// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* ParallelRecordReader.cs -- чтение записей с сервера в параллельных потоках
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Чтение записей с сервера в параллельных потоках.
    /// </summary>
    public sealed class ParallelRecordReader
        : IEnumerable<Record>,
            IDisposable
    {
        #region Properties

        /// <summary>
        /// Степень параллелизма.
        /// </summary>
        public int Parallelism { get; }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        public string? ConnectionString { get; }

        /// <summary>
        /// Признак окончания.
        /// </summary>
        public bool IsStop => _AllDone();

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader()
            : this (-1)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism
            )
            : this
                (
                    parallelism,
                    ConnectionUtility.GetStandardConnectionString()
                        .ThrowIfNull()
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism,
                string connectionString
            )
            : this
                (
                    parallelism,
                    connectionString,
                    _GetMfnList (connectionString)
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader
            (
                int parallelism,
                string connectionString,
                int[] mfnList
            )
        {
            if (parallelism <= 0)
            {
                parallelism = Utility.OptimalParallelism;
            }

            ConnectionString = connectionString;
            Parallelism = Math.Min (mfnList.Length / 1000, parallelism);
            if (Parallelism < 2)
            {
                Parallelism = 2;
            }

            _Run (mfnList);
        }

        #endregion

        #region Private members

        private Task[]? _tasks;

        private ConcurrentQueue<Record>? _queue;

        private AutoResetEvent? _event;

        private static int[] _GetMfnList
            (
                string connectionString
            )
        {
            using var connection = ConnectionFactory.Shared.CreateSyncConnection();
            connection.ParseConnectionString (connectionString);
            connection.Connect();

            var maxMfn = connection.GetMaxMfn() - 1;
            if (maxMfn <= 0)
            {
                throw new IrbisException ("MaxMFN=0");
            }

            var result = Enumerable.Range (1, maxMfn).ToArray();

            return result;
        }

        private void _Run
            (
                int[] mfnList
            )
        {
            _queue = new ConcurrentQueue<Record>();
            _event = new AutoResetEvent (false);

            _tasks = new Task[Parallelism];
            var chunks = ArrayUtility.SplitArray
                (
                    mfnList,
                    Parallelism
                );

            for (var i = 0; i < Parallelism; i++)
            {
                var task = new Task
                    (
                        _Worker!,
                        chunks[i]
                    );
                _tasks[i] = task;
            }

            foreach (var task in _tasks)
            {
                Thread.Sleep (50);
                task.Start();
            }
        }

        private void _Worker
            (
                object state
            )
        {
            var chunk = (int[])state;
            var first = chunk.SafeAt (0, -1);
            var threadId = Environment.CurrentManagedThreadId;

            Magna.Trace
                (
                    nameof (ParallelRecordReader) + "::" + nameof (_Worker)
                    + ": first="
                    + first
                    + ", length="
                    + chunk.Length
                    + ", thread="
                    + threadId
                );

            var connectionString = ConnectionString.ThrowIfNull();
            using (var connection = ConnectionFactory.Shared.CreateSyncConnection())
            {
                connection.ParseConnectionString (connectionString);
                connection.Connect();

                var database = connection.EnsureDatabase();
                var records = connection.ReadRecords (database, chunk);
                if (records is not null)
                {
                    foreach (var record in records)
                    {
                        _PutRecord (record);
                    }
                }
            }

            _event?.Set();

            Magna.Trace
                (
                    nameof (ParallelRecordReader) + "::" + nameof (_Worker)
                    + ": first="
                    + first
                    + ", length="
                    + chunk.Length
                    + ", thread="
                    + threadId
                );
        }

        private void _PutRecord
            (
                Record record
            )
        {
            _queue?.Enqueue (record);
            _event?.Set();
        }

        private bool _AllDone()
        {
            return _queue.ThrowIfNull().IsEmpty
                   && _tasks.ThrowIfNull().All (t => t.IsCompleted);
        }

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Record> GetEnumerator()
        {
            while (true)
            {
                if (IsStop)
                {
                    yield break;
                }

                if (_queue is not null)
                {
                    while (_queue.TryDequeue (out var record))
                    {
                        yield return record;
                    }
                }

                _event?.Reset();

                _event?.WaitOne (10);
            }
        }

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Read all records.
        /// </summary>
        public Record[] ReadAll()
        {
            var result = new List<Record>();

            foreach (var record in this)
            {
                result.Add (record);
            }

            return result.ToArray();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _event?.Dispose();
            if (_tasks is not null)
            {
                foreach (var task in _tasks)
                {
                    task.Dispose();
                }
            }
        }

        #endregion
    }
}
