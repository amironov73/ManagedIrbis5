// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisIdleManager.cs -- sends NOP command to the server
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Sends NOP command to the server.
    /// </summary>
    public sealed class IrbisIdleManager
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Raised when connection is idle.
        /// </summary>
        public event EventHandler? Idle;

        #endregion

        #region Properties

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncProvider Connection { get; private set; }

        /// <summary>
        /// Idle interval, milliseconds.
        /// </summary>
        public int Interval { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisIdleManager
            (
                ISyncProvider connection,
                int interval
            )
        {
            if (interval < 10)
            {
                throw new ArgumentOutOfRangeException(nameof(interval));
            }

            Connection = connection;
            Interval = interval;

            Connection.Disposing += Connection_Disposing;
            _timer = new Timer
            {
                Interval = interval,
                Enabled = true
            };
            _timer.Tick += _timer_Tick;
            _lock = new object();
        }

        #endregion

        #region Private members

        private readonly Timer _timer;
        private readonly object _lock;

        private void Connection_Disposing
            (
                object? sender,
                EventArgs e
            )
        {
            Dispose();
        }

        private void _timer_Tick
            (
                object? sender,
                EventArgs e
            )
        {
            lock (_lock)
            {
                if (Connection.Connected
                    && !Connection.Busy)
                {
                    Connection.NoOperation();

                    Idle?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Manually raise <see cref="Idle"/> event.
        /// </summary>
        public void Raise()
        {
            _timer_Tick(this, EventArgs.Empty);
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
