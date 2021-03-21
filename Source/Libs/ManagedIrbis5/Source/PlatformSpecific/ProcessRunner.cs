// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ProcessRunner.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    static class ProcessRunner
    {
        #region Public methods

        public static string GetShell()
        {
            // TODO support for Linux and OS X

            string result = Environment.GetEnvironmentVariable("COMSPEC")
                             ?? "cmd.exe";

            return result;
        }

        /// <summary>
        /// Run process and forget about it.
        /// </summary>
        public static void RunAndForget
            (
                string commandLine
            )
        {
            string comspec = GetShell();

            commandLine = "/c " + commandLine;
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    comspec,
                    commandLine
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            Process process = Process.Start(startInfo)
                .ThrowIfNull("process");
            process.Dispose();
        }

        /// <summary>
        /// Run process and get its output.
        /// </summary>
        public static string RunAndGetOutput
            (
                string commandLine
            )
        {
            string comspec = GetShell();

            commandLine = "/c " + commandLine;
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    comspec,
                    commandLine
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = IrbisEncoding.Oem
            };

            Process process = Process.Start(startInfo)
                .ThrowIfNull("process");
            process.WaitForExit();

            string result = process.StandardOutput.ReadToEnd();

            return result;
        }

        /// <summary>
        /// Run process and wait for it.
        /// </summary>
        public static void RunAndWait
            (
                string commandLine
            )
        {
            string comspec = GetShell();

            commandLine = "/c " + commandLine;
            ProcessStartInfo startInfo = new ProcessStartInfo
                (
                    comspec,
                    commandLine
                )
            {
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            Process process = Process.Start(startInfo)
                .ThrowIfNull("process");
            process.WaitForExit();
        }

        #endregion
    }
}
