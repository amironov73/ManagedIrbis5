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

#endregion

#nullable enable

namespace AM.Threading.Tasks
{
    /// <summary>
    /// Простейший процессор задач.
    /// </summary>
    public sealed class SimplestTaskProcessor
    {
        #region Nested classes

        class ActionWrapper
        {
            public Task? Task { get; set; }

            public Action? Action { get; init; }

            public SimplestTaskProcessor? Processor { get; init; }

            public void Worker()
            {
                try
                {
                    Action!.Invoke();
                }
                catch (Exception ex)
                {
                    lock (Processor!.Exceptions)
                    {
                        Processor.Exceptions.Add (ex);
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
            Magna.Trace (nameof (SimplestTaskProcessor) + "::Constructor");

            if (parallelism <= 0)
            {
                Magna.Error
                    (
                        nameof (SimplestTaskProcessor) + "::Constructor: "
                        + "parallelism="
                        + parallelism
                    );

                throw new ArgumentOutOfRangeException (nameof (parallelism));
            }

            _queue = new BlockingCollection<Action>();
            _running = new NonNullCollection<ActionWrapper>();
            Exceptions = new NonNullCollection<Exception>();
            _semaphore = new SemaphoreSlim (parallelism, parallelism);

            Task.Factory.StartNew (_MainWorker);

        } // constructor

        #endregion

        #region Private members

        private readonly BlockingCollection<Action> _queue;
        private readonly NonNullCollection<ActionWrapper> _running;

        private readonly SemaphoreSlim _semaphore;

        private void _MainWorker()
        {
            while (!_queue.IsCompleted)
            {
                _semaphore.Wait();

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

        } // method _MainWorker

        #endregion

        #region Public methods

        /// <summary>
        /// Задачи больше добавляться не будут.
        /// </summary>
        public void Complete()
        {
            Magna.Trace (nameof (SimplestTaskProcessor) + "::"
                + nameof (Complete));

            _queue.CompleteAdding();

        } // method Complete

        /// <summary>
        /// Добавление задачи.
        /// </summary>
        public void Enqueue
            (
                Action action
            )
        {
            Magna.Trace (nameof (SimplestTaskProcessor) + "::"
                + nameof (Enqueue));

            _queue.Add(action);

        } // method Enqueue

        /// <summary>
        /// Ожидание завершения всех задач.
        /// </summary>
        public void WaitForCompletion()
        {
            Magna.Trace (nameof (SimplestTaskProcessor) + "::"
                + nameof (WaitForCompletion) + ": begin");

            while (!_queue.IsCompleted)
            {
                Thread.SpinWait (100000);
            }

            Task[] tasks;

            lock (_running)
            {
                tasks = _running
                    .Select(r => r.Task!)
                    .ToArray();
            }

            Task.WaitAll (tasks);

            Magna.Trace (nameof (SimplestTaskProcessor) + "::"
                + nameof (WaitForCompletion) + ": end");

        } // method WaitForCompletion

        #endregion

    } // class SimplestTaskProcessor

} // namespace AM.Threading.Tasks
