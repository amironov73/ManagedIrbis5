// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisSystemEvent.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    //
    // При старте сервера
    //
    // var systemEvent = new IrbisSystemEvent();
    // if (!systemEvent.CheckOtherServerRunning())
    // {
    //   Облом
    // }
    // systemEvent.SayIamRunning();
    //
    // В конце
    //
    // systemEvent.Dispose();
    //

    /// <summary>
    ///
    /// </summary>
    public sealed class IrbisSystemEvent
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// Name of the event.
        /// </summary>
        public const string StartedName = "IRBIS64_STARTED";

        /// <summary>
        /// Name of the event.
        /// </summary>
        public const string StopName = "IRBIS64_STOP_";

        #endregion

        #region Properties

        /// <summary>
        /// IRBIS64 server started.
        /// </summary>
        public EventWaitHandle? StartedEvent { get; private set; }

        /// <summary>
        /// Stop the IRBIS64 server.
        /// </summary>
        public EventWaitHandle? StopEvent { get; private set; }

        /// <summary>
        /// New event created.
        /// </summary>
        public bool Created { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisSystemEvent()
        {
            StartedEvent = new EventWaitHandle
                (
                    false,
                    EventResetMode.ManualReset,
                    StartedName,
                    out var created
                );
            Created = created;

            Magna.Trace
                (
                    "IrbisSystemEvent::Constructor: "
                    + StartedName
                    + ": created="
                    + created
                );

            StopEvent = new EventWaitHandle
                (
                    false,
                    EventResetMode.ManualReset,
                    StopName
                );

        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~IrbisSystemEvent()
        {
            Magna.Trace
                (
                    "IrbisSystemEvent::Destructor"
                );

            Dispose();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Check if other IRBIS64 server is running.
        /// </summary>
        public bool CheckOtherServerRunning()
        {
#if WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            return !StartedEvent.WaitOne(1);

#endif
        }

        /// <summary>
        /// Whether the IRBIS64 needs to stop.
        /// </summary>
        public bool CheckStopRequested()
        {
#if WINMOBILE || PocketPC

            throw new NotImplementedException();

#else

            return StopEvent.WaitOne(1);

#endif
        }

        /// <summary>
        /// Request stop.
        /// </summary>
        public void RequestStop()
        {
            StopEvent.Set();
        }

        /// <summary>
        /// Say "I am running" to other servers (if any).
        /// </summary>
        public void SayIamRunning()
        {
            StartedEvent.Set();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Magna.Trace
                (
                    "IrbisSystemEvent::Dispose"
                );

            GC.SuppressFinalize(this);

            if (!ReferenceEquals(StartedEvent, null))
            {
                StartedEvent.Dispose();
            }

            if (!ReferenceEquals(StopEvent, null))
            {
                StopEvent.Dispose();
            }

        }

        #endregion

    }
}
