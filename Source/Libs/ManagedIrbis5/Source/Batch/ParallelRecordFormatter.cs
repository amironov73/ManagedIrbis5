// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* ParallelRecordFormatter.cs -- formats records from the server in parallel threads
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

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Batch;

/// <summary>
/// Formats records from the server in parallel threads.
/// </summary>
public sealed class ParallelRecordFormatter
    : IEnumerable<string>,
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
    public string ConnectionString { get; private set; }

    /// <summary>
    /// Признак окончания.
    /// </summary>
    public bool IsStop => _AllDone();

    /// <summary>
    /// Используемый формат.
    /// </summary>
    public string Format { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ParallelRecordFormatter
        (
            int parallelism,
            string connectionString,
            int[] mfnList,
            string format
        )
    {
        if (parallelism <= 0)
        {
            parallelism = Utility.OptimalParallelism;
        }

        ConnectionString = connectionString;
        Format = format;
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

    private ConcurrentQueue<string>? _queue;

    private AutoResetEvent? _event;

    // private object? _lock;

    private void _Run
        (
            int[] mfnList
        )
    {
        _queue = new ConcurrentQueue<string>();
        _event = new AutoResetEvent (false);

        // _lock = new object();

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
                    _Worker,
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
            object? state
        )
    {
        var chunk = (int[]?)state;
        var first = chunk?.SafeAt (0, -1);
        var threadId = Thread.CurrentThread.ManagedThreadId;

        Magna.Logger.LogTrace
            (
                nameof (ParallelRecordFormatter) + "::" + nameof (_Worker)
                + ": begin: first={First}, length={Length}, thread={Thread}",
                first,
                chunk?.Length,
                threadId
            );

        var connectionString = ConnectionString.ThrowIfNull();
        using (var connection = ConnectionFactory.Shared.CreateSyncConnection())
        {
            connection.ParseConnectionString (connectionString);
            connection.Connect();

            var batch = new BatchRecordFormatter
                (
                    connection,
                    connection.Database.ThrowIfNull(),
                    Format,
                    1000,
                    chunk
                );
            foreach (var line in batch)
            {
                _PutLine (line);
            }
        }

        _event?.Set();

        Magna.Logger.LogTrace
            (
                nameof (ParallelRecordFormatter) + "::" + nameof (_Worker)
                + ": end: first={First}, length={Length}, thread={Thread}",
                first,
                chunk?.Length,
                threadId
            );
    }

    private void _PutLine
        (
            string line
        )
    {
        _queue?.Enqueue (line);
        _event?.Set();
    }

    private bool _AllDone()
    {
        return _queue.ThrowIfNull().IsEmpty
               && _tasks.ThrowIfNull().All (t => t.IsCompleted);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Форматирование всех записей.
    /// </summary>
    public string[] FormatAll()
    {
        var result = new List<string>();

        foreach (var line in this)
        {
            result.Add (line);
        }

        return result.ToArray();
    }

    #endregion

    #region IEnumerable<T> members

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<string> GetEnumerator()
    {
        while (true)
        {
            if (IsStop)
            {
                yield break;
            }

            if (_queue is not null)
            {
                while (_queue.TryDequeue (out var line))
                {
                    yield return line;
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
