// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConsoleProcessRunner.cs -- запускает консольный процесс и перехватывает его выдачу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Diagnostics
{
    /// <summary>
    /// Runs console process and intercepts its output
    /// redirecting to text box.
    /// </summary>
    public sealed class ConsoleProcessRunner
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets the receiver.
        /// </summary>
        public IConsoleOutputReceiver? Receiver
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the running process.
        /// </summary>
        public Process? RunningProcess => _runningProcess;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConsoleProcessRunner
            (
                IConsoleOutputReceiver? receiver
            )
        {
            Receiver = receiver;
        }

        #endregion

        #region Private members

        private Process? _runningProcess;

        private void _OutputDataReceived
            (
                object sender,
                DataReceivedEventArgs e
            )
        {
            if (Receiver != null && e?.Data != null)
            {
                Receiver.ReceiveConsoleOutput(e.Data);
            }
        }

        private void _ProcessExited
            (
                object sender,
                EventArgs e
            )
        {
            var process = RunningProcess;
            if (!ReferenceEquals(process, null))
            {
#nullable disable
                process.OutputDataReceived -= _OutputDataReceived;
                process.Exited -= _ProcessExited;
#nullable restore
                _runningProcess = null;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts new process with the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="arguments">The arguments.</param>
        public void Start
            (
                string fileName,
                string arguments
            )
        {
            if (!ReferenceEquals(RunningProcess, null)
                 && !RunningProcess.HasExited)
            {
                Magna.Error
                    (
                        "ConsoleProcessRunner::Start: "
                        + "process already running"
                    );

                throw new ArsMagnaException();
            }

            var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,

                    // ReSharper disable AssignNullToNotNullAttribute
                    StandardErrorEncoding = null,

                    // If the value of the StandardOutputEncoding property
                    // is null, the process uses the default standard
                    // output encoding for the standard output.
                    StandardOutputEncoding = null,

                    // ReSharper restore AssignNullToNotNullAttribute

                    UseShellExecute = false
                };
            _runningProcess = new Process
                {
                    StartInfo = startInfo
                    //, SynchronizingObject = Receiver // Use this to event handler calls
                    // that are issued as a result of an Exited event on the process
                };
           //ISynchronizeInvoke synchronizingObject =
           //         Receiver as ISynchronizeInvoke;
           //if (synchronizingObject != null)
           //{
           //     _runningProcess.SynchronizingObject = synchronizingObject;
           //}
#nullable disable
            _runningProcess.OutputDataReceived += _OutputDataReceived;
            _runningProcess.ErrorDataReceived += _OutputDataReceived;
            _runningProcess.Exited += _ProcessExited;
#nullable restore
            _runningProcess.Start();
            _runningProcess.BeginOutputReadLine();
            _runningProcess.BeginErrorReadLine();
        }

        /// <summary>
        /// Stops running process if any.
        /// </summary>
        public void Stop()
        {
            // TODO: try to kill running process?

            if (!ReferenceEquals(RunningProcess, null)
                 && !RunningProcess.HasExited)
            {
#nullable disable
                RunningProcess.OutputDataReceived -= _OutputDataReceived;
                RunningProcess.Exited -= _ProcessExited;
#nullable restore
            }
        }

        #endregion

        #region IDisposable

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _runningProcess?.Dispose();
        }

        #endregion
    }
}
