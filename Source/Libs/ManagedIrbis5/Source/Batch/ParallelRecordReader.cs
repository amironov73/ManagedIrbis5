// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* ParallelRecordReader.cs -- reads records from the server in parallel threads
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
    /// Reads records from the server in parallel threads.
    /// </summary>
    public sealed class ParallelRecordReader
        : IEnumerable<Record>,
            IDisposable
    {
        #region Properties

        /// <summary>
        /// Степень параллелизма.
        /// </summary>
        public int Parallelism { get; private set; }

        /// <summary>
        /// Строка подключения.
        /// </summary>
        public string? ConnectionString { get; private set; }

        /// <summary>
        /// Признак окончания.
        /// </summary>
        public bool Stop { get { return _AllDone(); } }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParallelRecordReader()
            : this(-1)
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
                    -1,
                    ConnectionUtility.GetStandardConnectionString()
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
                    -1,
                    connectionString,
                    _GetMfnList(connectionString)
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
            Parallelism = Math.Min(mfnList.Length / 1000, parallelism);
            if (Parallelism < 2)
            {
                Parallelism = 2;
            }

            _Run(mfnList);
        }

        #endregion

        #region Private members

        private Task[]? _tasks;

        private ConcurrentQueue<Record>? _queue;

        private AutoResetEvent? _event;

        private object? _lock;

        private static int[] _GetMfnList
            (
                string connectionString
            )
        {
            using (var connection = ConnectionFactory.Shared.CreateConnection())
            {
                connection.ParseConnectionString(connectionString);
                connection.Connect();

                int maxMfn = connection.GetMaxMfn() - 1;
                if (maxMfn <= 0)
                {
                    throw new IrbisException("MaxMFN=0");
                }
                int[] result = Enumerable.Range(1, maxMfn).ToArray();

                return result;
            }
        }

        private void _Run
            (
                int[] mfnList
            )
        {
            _queue = new ConcurrentQueue<Record>();
            _event = new AutoResetEvent(false);
            _lock = new object();

            _tasks = new Task[Parallelism];
            int[][] chunks = ArrayUtility.SplitArray
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

            foreach (Task task in _tasks)
            {
                Thread.Sleep(50);
                task.Start();
            }
        }

        private void _Worker
            (
                object state
            )
        {
            int[] chunk = (int[])state;
            int first = chunk.SafeAt(0, -1);
            int threadId = Thread.CurrentThread.ManagedThreadId;

            Magna.Trace
                (
                    "ParallelRecordReader::_Worker: begin: "
                    + "first="
                    + first
                    + ", length="
                    + chunk.Length
                    + ", thread="
                    + threadId
                );

            string connectionString = ConnectionString.ThrowIfNull();
            using (var connection = ConnectionFactory.Shared.CreateConnection())
            {
                connection.ParseConnectionString(connectionString);
                connection.Connect();

                BatchRecordReader batch = new BatchRecordReader
                    (
                        connection,
                        connection.Database,
                        1000,
                        chunk
                    );
                foreach (var record in batch)
                {
                    _PutRecord(record);
                }
            }

            _event?.Set();

            Magna.Trace
                (
                    "ParallelRecordReader::_Worker: end: "
                    + "first="
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
            _queue?.Enqueue(record);
            _event?.Set();
        }

        private bool _AllDone()
        {
            return _queue.IsEmpty
                   && _tasks.All(t => t.IsCompleted);
        }

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Record> GetEnumerator()
        {
            while (true)
            {
                if (Stop)
                {
                    yield break;
                }

                if (_queue is not null)
                {
                    while (_queue.TryDequeue(out var record))
                    {
                        yield return record;
                    }
                }

                _event?.Reset();

                _event?.WaitOne(10);
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
            List<Record> result = new List<Record>();

            foreach (var record in this)
            {
                result.Add(record);
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
                foreach (Task task in _tasks)
                {
                    task.Dispose();
                }
            }
        }

        #endregion

    } // class ParallelRecordReader

} // namespace ManagedIrbis.Batch
