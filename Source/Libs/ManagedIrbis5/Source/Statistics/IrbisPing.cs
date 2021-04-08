﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisPing.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Threading;

using AM;
using AM.PlatformAbstraction;

#endregion

#nullable enable

namespace ManagedIrbis.Statistics
{
    /// <summary>
    ///
    /// </summary>
    public sealed class IrbisPing
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Raised when the <see cref="Statistics"/> is updated.
        /// </summary>
        public event EventHandler? StatisticsUpdated;

        #endregion

        #region Properties

        /// <summary>
        /// Whether the <see cref="IrbisPing"/> is active?
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Platform abstraction level.
        /// </summary>
        public PlatformAbstractionLayer PlatformAbstraction { get; set; }

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncConnection Connection { get; private set; }

        /// <summary>
        /// Statistics.
        /// </summary>
        public PingStatistics Statistics { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisPing
            (
                ISyncConnection connection
            )
        {
            PlatformAbstraction = new PlatformAbstractionLayer();
            Connection = connection;
            Statistics = new PingStatistics();
            _timer = new Timer(_timer_Elapsed, null, 1000, 1000);
        }

        #endregion

        #region Private members

        private readonly Timer _timer;

        private bool _busy;

        private void _timer_Elapsed
            (
                object state
            )
        {
            if (!Active
                || _busy
                || Connection.Busy)
            {
                return;
            }

            _busy = true;
            try
            {
                var ping = PingOnce();
                Statistics.Add(ping);
                StatisticsUpdated.Raise(this);
            }
            finally
            {
                _busy = false;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Ping once.
        /// </summary>
        public PingData PingOnce()
        {
            PingData result = new PingData
            {
                Moment = PlatformAbstraction.Now()
            };

            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                Connection.NoOperation();

                stopwatch.Stop();
                unchecked
                {
                    result.RoundTripTime = (int) stopwatch.ElapsedMilliseconds;
                }
                result.Success = true;
            }
            catch
            {
                result.Success = false;
            }

            return result;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            _timer.Dispose();
        }

        #endregion

    }
}
