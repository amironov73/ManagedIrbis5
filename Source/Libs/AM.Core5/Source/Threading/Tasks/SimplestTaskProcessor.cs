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

/* SimplestTaskProcessor.cs -- simplest task processor
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
    /// Simplest task processor.
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
                    lock (Processor!._exceptions)
                    {
                        Processor._exceptions.Add(ex);
                    }
                }

                lock (Processor!._running)
                {
                    Processor._running.Remove(this);
                }

                var semaphore = Processor._semaphore;
                semaphore.Release();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Exceptions.
        /// </summary>
        public NonNullCollection<Exception> Exceptions => _exceptions;

        /// <summary>
        /// Have errors?
        /// </summary>
        public bool HaveErrors => Exceptions.Count != 0;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimplestTaskProcessor
            (
                int parallelism
            )
        {
            Magna.Trace(nameof(SimplestTaskProcessor) + "::Constructor");

            if (parallelism < 0)
            {
                Magna.Error
                    (
                        nameof(SimplestTaskProcessor) + "::Constructor: "
                        + "parallelism="
                        + parallelism
                    );

                throw new ArgumentOutOfRangeException(nameof(parallelism));
            }

            _queue = new BlockingCollection<Action>();
            _running = new NonNullCollection<ActionWrapper>();
            _exceptions = new NonNullCollection<Exception>();
            _semaphore = new SemaphoreSlim(parallelism, parallelism);

            Task.Factory.StartNew(_MainWorker);
        }

        #endregion

        #region Private members

        private readonly BlockingCollection<Action> _queue;
        private readonly NonNullCollection<ActionWrapper> _running;
        private readonly NonNullCollection<Exception> _exceptions;

        private readonly SemaphoreSlim _semaphore;

        private void _MainWorker()
        {
            while (!_queue.IsCompleted)
            {
                _semaphore.Wait();

                if (!_queue.TryTake(out var action))
                {
                    continue;
                }

                var wrapper = new ActionWrapper
                {
                    Action = action,
                    Processor = this
                };
                var task = new Task(wrapper.Worker);
                wrapper.Task = task;
                lock (_running)
                {
                    _running.Add(wrapper);
                }
                wrapper.Task.Start();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        ///
        /// </summary>
        public void Complete()
        {
            Magna.Trace(nameof(SimplestTaskProcessor) + "::"
                + nameof(Complete));

            _queue.CompleteAdding();
        }

        /// <summary>
        ///
        /// </summary>
        public void Enqueue
            (
                Action action
            )
        {
            Magna.Trace(nameof(SimplestTaskProcessor) + "::"
                + nameof(Enqueue));

            _queue.Add(action);
        }

        /// <summary>
        /// Wait for completion.
        /// </summary>
        public void WaitForCompletion()
        {
            Magna.Trace(nameof(SimplestTaskProcessor) + "::"
                + nameof(WaitForCompletion) + ": begin");

            while (!_queue.IsCompleted)
            {
                Thread.SpinWait(100000);
            }

            Task[] tasks;

            lock (_running)
            {
                tasks = _running
                    .Select(r => r.Task!)
                    .ToArray();
            }

            Task.WaitAll(tasks);

            Magna.Trace(nameof(SimplestTaskProcessor) + "::" + nameof(WaitForCompletion) + ": end");
        }

        #endregion

    } // class SimplestTaskProcessor

} // namespace AM.Threading.Tasks
