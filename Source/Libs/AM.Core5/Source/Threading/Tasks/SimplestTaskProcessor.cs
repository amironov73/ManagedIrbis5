// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* SimplestTaskProcessor.cs -- простейший процессор задач
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM.Collections;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Threading.Tasks;

/// <summary>
/// Простейший процессор задач.
/// </summary>
public sealed class SimplestTaskProcessor
{
    #region Nested classes

    internal sealed class ActionWrapper
    {
        public Task? Task { get; set; }

        public Action? Action { get; init; }

        public SimplestTaskProcessor? Processor { get; init; }

        public void Worker()
        {
            if (Processor!._source.IsCancellationRequested)
            {
                return;
            }

            try
            {
                Action!.Invoke();
            }
            catch (Exception exception)
            {
                lock (Processor!.Exceptions)
                {
                    Processor.Exceptions.Add (exception);
                }
            }

            lock (Processor!._running)
            {
                Processor._running.Remove (this);
            }

            var semaphore = Processor._semaphore;
            semaphore.Release();
        }
    }

    #endregion

    #region Properties

    /// <summary>
    /// Исключения, возникшие при выполнении задач.
    /// </summary>
    public NonNullCollection<Exception> Exceptions { get; }

    /// <summary>
    /// Возникали ли исключения?
    /// </summary>
    public bool HaveErrors => Exceptions.Count != 0;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SimplestTaskProcessor
        (
            int parallelism
        )
    {
        Magna.Logger.LogTrace (nameof (SimplestTaskProcessor) + "::Constructor");

        if (parallelism <= 0)
        {
            Magna.Logger.LogError
                (
                    nameof (SimplestTaskProcessor) + "::Constructor"
                                                   + "parallelism={Parallelism}",
                    parallelism
                );

            throw new ArgumentOutOfRangeException (nameof (parallelism));
        }

        _queue = new BlockingCollection<Action>();
        _running = new NonNullCollection<ActionWrapper>();
        Exceptions = new NonNullCollection<Exception>();
        _semaphore = new SemaphoreSlim (parallelism, parallelism);
        _source = new CancellationTokenSource();

        Task.Factory.StartNew (_MainWorker);
    }

    #endregion

    #region Private members

    private readonly BlockingCollection<Action> _queue;
    private readonly NonNullCollection<ActionWrapper> _running;
    private readonly CancellationTokenSource _source;

    private readonly SemaphoreSlim _semaphore;

    private void _MainWorker()
    {
        while (!_queue.IsCompleted)
        {
            _semaphore.Wait (_source.Token);

            if (!_queue.TryTake (out var action))
            {
                continue;
            }

            var wrapper = new ActionWrapper
            {
                Action = action,
                Processor = this
            };
            var task = new Task (wrapper.Worker);
            wrapper.Task = task;
            lock (_running)
            {
                _running.Add (wrapper);
            }

            wrapper.Task.Start();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Прекращение выборки из очереди.
    /// </summary>
    public void Cancel()
    {
        _source.Cancel();
        _queue.CompleteAdding();
    }

    /// <summary>
    /// Задачи больше добавляться не будут.
    /// </summary>
    public void Complete()
    {
        Magna.Logger.LogTrace (nameof (SimplestTaskProcessor) + "::" + nameof (Complete));

        _queue.CompleteAdding();
    }

    /// <summary>
    /// Добавление задачи.
    /// </summary>
    public void Enqueue
        (
            Action action
        )
    {
        Magna.Logger.LogTrace (nameof (SimplestTaskProcessor) + "::" + nameof (Enqueue));

        _queue.Add (action);
    }

    /// <summary>
    /// Ожидание завершения всех задач.
    /// </summary>
    public void WaitForCompletion()
    {
        Magna.Logger.LogTrace (nameof (SimplestTaskProcessor) + "::" + nameof (WaitForCompletion) + ": begin");

        while (!_queue.IsCompleted)
        {
            Thread.SpinWait (100_000);
        }

        Task[] tasks;

        lock (_running)
        {
            tasks = _running
                .Select (r => r.Task!)
                .ToArray();
        }

        Task.WaitAll (tasks);

        Magna.Logger.LogTrace (nameof (SimplestTaskProcessor) + "::" + nameof (WaitForCompletion) + ": end");
    }

    #endregion
}
