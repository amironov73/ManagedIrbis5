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

/* ServerWatchdog.cs -- серверный сторожевой таймер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Server.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Серверный сторожевой таймер.
    /// </summary>
    public sealed class ServerWatchdog
    {
        #region Constants

        /// <summary>
        /// Default timeout, seconds.
        /// </summary>
        public const int DefaultTimeout = 30;

        #endregion

        #region Properties

        /// <summary>
        /// Engine.
        /// </summary>
        public ServerEngine Engine { get; private set; }

        /// <summary>
        /// Cancellation token.
        /// </summary>
        public CancellationToken Token { get; private set; }

        /// <summary>
        /// Corresponding task.
        /// </summary>
        public Task Task { get; private set; }

        /// <summary>
        /// Timeout, seconds.
        /// </summary>
        public int Timeout { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ServerWatchdog
            (
                ServerEngine engine
            )
        {
            Engine = engine;
            Token = engine.GetCancellationToken();
            Task = MainLoop();
            Timeout = DefaultTimeout;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Главный цикл.
        /// </summary>
        public async Task MainLoop()
        {
            if (Timeout <= 0)
            {
                // TODO is it right decision?
                return;
            }

            while (!Token.IsCancellationRequested)
            {
                await Task.Yield();

                try
                {
                    ServerWorker[] workers;

                    lock (Engine.SyncRoot)
                    {
                        workers = Engine.Workers.ToArray();
                    }

                    var threshold = DateTime.Now.AddSeconds(-Timeout);
                    var longRunning = workers
                        .Where(w => w.Data.Started < threshold)
                        .ToArray();

                    foreach (var worker in longRunning)
                    {
                        var task = worker.Data.Task.ThrowIfNull(nameof(worker.Data.Task));
                        Magna.Warning($"Long running worker: {task.Id}");

                        // TODO kill long running task?
                    }
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            nameof(ServerWatchdog) + "::" + nameof(MainLoop),
                            exception
                        );
                }
            }

        } // method MainLoop

        #endregion

    } // class ServerWatchdog

} // namespace ManagedIrbis.Server
