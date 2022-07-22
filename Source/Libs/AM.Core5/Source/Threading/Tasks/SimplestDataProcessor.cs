// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* SimplestDataProcessor.cs -- простейший процессор данных
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Threading.Tasks;

/// <summary>
/// Простейший процессор данных.
/// </summary>
public sealed class SimplestDataProcessor<TInput, TOutput>
{
    #region Nested classes

    /// <summary>
    /// Один элемент данных, как входных, так и соответствующих выходных.
    /// </summary>
    public sealed class Item
    {
        #region Properties

        /// <summary>
        /// Входящие данные.
        /// </summary>
        public TInput? Input { get; init; }

        /// <summary>
        /// Результат их обработки.
        /// </summary>
        public TOutput? Output { get; set; }

        /// <summary>
        /// Исключение, которое возникло при обработке.
        /// </summary>
        public Exception? Exception { get; set; }

        #endregion

        #region Private members

        internal SimplestDataProcessor<TInput, TOutput>? _processor;
        internal Task? _task;

        internal void Process()
        {
            try
            {
                Output = _processor!._function (Input!);
            }
            catch (Exception exception)
            {
                Exception = exception;
            }

            var runningTasks = _processor!._running;
            lock (runningTasks)
            {
                runningTasks.Remove (_task!);
            }
        }

        #endregion
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SimplestDataProcessor
        (
            Func<TInput, TOutput> function,
            int parallelism = 0
        )
    {
        if (parallelism < 0)
        {
            throw new ArgumentOutOfRangeException (nameof (parallelism));
        }

        if (parallelism == 0)
        {
            parallelism = Environment.ProcessorCount;
        }

        _function = function;
        _queue = new BlockingCollection<Item>();
        _running = new List<Task> (parallelism);
        _semaphore = new SemaphoreSlim (parallelism, parallelism);

        Task.Factory.StartNew (_MainWorker);
    }

    #endregion

    #region Private members

    internal readonly Func<TInput, TOutput> _function;
    private readonly SemaphoreSlim _semaphore;
    private readonly BlockingCollection<Item> _queue;
    private readonly List<Task> _running;

    private void _MainWorker()
    {
        while (!_queue.IsCompleted)
        {
            _semaphore.Wait();

            if (!_queue.TryTake (out var item))
            {
                continue;
            }

            item._processor = this;
            var task = new Task (item.Process);
            item._task = task;
            lock (_running)
            {
                _running.Add (task);
            }

            task.Start();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Задачи больше добавляться не будут.
    /// </summary>
    public void Complete()
    {
        Magna.Logger.LogTrace (nameof (SimplestDataProcessor<TInput, TOutput>) + "::" + nameof (Complete));

        _queue.CompleteAdding();
    }

    /// <summary>
    /// Добавление задачи.
    /// </summary>
    public void Enqueue
        (
            TInput input
        )
    {
        Magna.Logger.LogTrace (nameof (SimplestDataProcessor<TInput, TOutput>) + "::" + nameof (Enqueue));

        var item = new Item { Input = input };
        _queue.Add (item);
    }

    /// <summary>
    /// Ожидание завершения всех задач.
    /// </summary>
    public void WaitForCompletion()
    {
        Magna.Logger.LogTrace (nameof (SimplestDataProcessor<TInput, TOutput>) + "::" + nameof (WaitForCompletion) + ": begin");

        while (!_queue.IsCompleted)
        {
            Thread.SpinWait (100000);
        }

        Task[] tasks;

        lock (_running)
        {
            tasks = _running.ToArray();
        }

        if (tasks.Length != 0)
        {
            Task.WaitAll (tasks);
        }

        Magna.Logger.LogTrace (nameof (SimplestDataProcessor<TInput, TOutput>) + "::" + nameof (WaitForCompletion) + ": end");
    }

    #endregion
}
