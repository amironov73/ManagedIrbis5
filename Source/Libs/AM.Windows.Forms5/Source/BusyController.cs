// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* BusyController.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM.Collections;
using AM.Text.Output;
using AM.Threading;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class BusyController
        : Component
    {
        #region Events

        /// <summary>
        /// Raised when exception occur.
        /// </summary>
        public event EventHandler<ExceptionEventArgs<Exception>>? ExceptionOccur;

        /// <summary>
        /// Raised when state changed.
        /// </summary>
        public event EventHandler<BusyStateEventArgs>? StateChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Control collection.
        /// </summary>
        public NonNullCollection<Control> Controls { get; private set; }

        /// <summary>
        /// For error messages.
        /// </summary>
        public AbstractOutput? Output { get; set; }

        /// <summary>
        /// State.
        /// </summary>
        public BusyState? State
        {
            get => _state;
            set => SetupState(value);
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyController()
        {
            Controls = new NonNullCollection<Control>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyController
            (
                BusyState? state
            )
        {
            State = state;
            Controls = new NonNullCollection<Control>();
        }

        #endregion

        #region Private members

        private BusyState? _state;

        /// <summary>
        /// Initialize <see cref="State"/> with specified value.
        /// </summary>
        protected void SetupState
            (
                BusyState? state
            )
        {
            if (!ReferenceEquals(_state, null))
            {
                _state.StateChanged -= _StateChanged;
            }

            _state = state;
            if (!ReferenceEquals(state, null))
            {
                state.StateChanged += _StateChanged;
            }
        }

        private void _StateChanged
            (
                object? sender,
                EventArgs e
            )
        {
            OnStateChanged();
        }

        /// <summary>
        /// Raises <see cref="ExceptionOccur"/> event.
        /// </summary>
        protected virtual void OnExceptionOccur
            (
                Exception exception
            )
        {
            var eventArgs = new ExceptionEventArgs<Exception>(exception);
            ExceptionOccur.Raise(this, eventArgs);
        }

        /// <summary>
        /// Raises <see cref="StateChanged"/> event.
        /// </summary>
        protected virtual void OnStateChanged()
        {
            var state = State;
            if (!ReferenceEquals(state, null))
            {
                Magna.Trace
                    (
                        "BusyController::OnStateChanged: "
                        + "busy="
                        + state.Busy
                    );

                var eventArgs
                    = new BusyStateEventArgs(state);
                StateChanged.Raise(this, eventArgs);
            }
        }

        /// <summary>
        /// Update control state.
        /// </summary>
        protected void UpdateControlState
            (
                bool enabled
            )
        {
            var state = State;
            if (!ReferenceEquals(state, null))
            {
                Magna.Trace
                    (
                        "BusyController::UpdateControlState: "
                        + "enabled="
                        + enabled
                    );

                foreach (var control in Controls)
                {
                    control.Enabled = enabled;
                }

                Application.DoEvents();
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Disable associated controls.
        /// </summary>
        public void DisableControls()
        {
            UpdateControlState(false);
        }

        /// <summary>
        /// Enable associated controls.
        /// </summary>
        public void EnableControls()
        {
            UpdateControlState(true);
        }

        /// <summary>
        /// Run the specified action.
        /// </summary>
        public bool Run
            (
                Action action
            )
        {
            var result = false;

            var state = State;
            if (!ReferenceEquals(state, null))
            {
                try
                {
                    Magna.Trace
                        (
                            "BusyController::Run: "
                            + "before"
                        );

                    UpdateControlState(false);
                    ApplicationUtility.Run
                        (
                            () => state.Run(action)
                        );

                    Magna.Trace
                        (
                            "BusyController::Run: "
                            + "normal after"
                        );

                    result = true;
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "BusyController::Run",
                            exception
                        );
                    WriteLine
                        (
                            "{0}: {1}",
                            exception.GetType().Name,
                            exception.Message
                        );

                    result = false;

                    OnExceptionOccur(exception);
                }
                finally
                {
                    UpdateControlState(true);
                }
            }

            return result;
        }

        /// <summary>
        /// Run the specified action.
        /// </summary>
        public async Task<bool> RunAsync
            (
                Action action
            )
        {
            var result = false;
            var state = State;
            if (!ReferenceEquals(state, null))
            {
                try
                {
                    UpdateControlState(false);

                    #if FW35 || FW40

                    PseudoAsync.Run(action);

                    #else

                    await state.RunAsync(action);

                    #endif

                    result = true;
                }
                catch (Exception exception)
                {
                    var unwrapped = Utility.Unwrap(exception);

                    Magna.TraceException
                        (
                            "BusyController::RunAsync",
                            unwrapped
                        );
                    WriteLine
                        (
                            "{0}: {1}",
                            exception.GetType().Name,
                            exception.Message
                        );

                    OnExceptionOccur(unwrapped);
                }
                finally
                {
                    UpdateControlState(true);
                }
            }

            return result;
        }

        /// <summary>
        /// Write message to <see cref="Output"/> if present.
        /// </summary>
        public void WriteLine
            (
                string format,
                params object[] args
            )
        {
            Output?.WriteLine(format, args);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        protected override void Dispose
            (
                bool disposing
            )
        {
            base.Dispose(disposing);

            var state = State;
            if (!ReferenceEquals(state, null))
            {
                state.StateChanged -= _StateChanged;
            }
        }

        #endregion

        #region Object members

        #endregion
    }
}
