// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ConsoleProcessRunner.cs -- запускает консольный процесс и перехватывает его выдачу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Diagnostics;

/// <summary>
/// Запускает консольный процесс и перехватывает его выдачу
/// для показа, например, в текстовом боксе.
/// </summary>
[ExcludeFromCodeCoverage]
public sealed class ConsoleProcessRunner
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Gets the receiver.
    /// </summary>
    public IConsoleOutputReceiver? Receiver { get; private set; }

    /// <summary>
    /// Gets the running process.
    /// </summary>
    public Process? RunningProcess { get; private set; }

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

    private void _OutputDataReceived
        (
            object? sender,
            DataReceivedEventArgs e
        )
    {
        if (Receiver != null && e.Data != null)
        {
            Receiver.ReceiveConsoleOutput (e.Data);
        }
    }

    private void _ProcessExited
        (
            object? sender,
            EventArgs e
        )
    {
        var process = RunningProcess;
        if (!ReferenceEquals (process, null))
        {
            process.OutputDataReceived -= _OutputDataReceived;
            process.Exited -= _ProcessExited;
            RunningProcess = null;
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
        if (RunningProcess is { HasExited: false })
        {
            Magna.Logger.LogError
                (
                    nameof (ConsoleProcessRunner) + "::" + nameof (Start)
                    + ": process already running"
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
            StandardErrorEncoding = null,

            // If the value of the StandardOutputEncoding property
            // is null, the process uses the default standard
            // output encoding for the standard output.
            StandardOutputEncoding = null,

            UseShellExecute = false
        };
        RunningProcess = new Process
        {
            StartInfo = startInfo

            //, SynchronizingObject = Receiver // Use this to event handler calls
            // that are issued as a result of an Exited event on the process
        };
        RunningProcess.OutputDataReceived += _OutputDataReceived;
        RunningProcess.ErrorDataReceived += _OutputDataReceived;
        RunningProcess.Exited += _ProcessExited;
        RunningProcess.Start();
        RunningProcess.BeginOutputReadLine();
        RunningProcess.BeginErrorReadLine();
    }

    /// <summary>
    /// Останавливает запущенный процесс, если это возможно.
    /// </summary>
    public void Stop()
    {
        // TODO: try to kill running process?

        if (RunningProcess is { HasExited: false })
        {
            RunningProcess.OutputDataReceived -= _OutputDataReceived;
            RunningProcess.Exited -= _ProcessExited;
        }
    }

    #endregion

    #region IDisposable

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        RunningProcess?.Dispose();
    }

    #endregion
}
