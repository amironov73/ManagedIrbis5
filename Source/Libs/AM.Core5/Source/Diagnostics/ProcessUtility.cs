// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ProcessUtility.cs -- работа с процессами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Diagnostics
{
    /// <summary>
    /// Работа с процессами.
    /// </summary>
    public static class ProcessUtility
    {
        #region Public methods

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="standardInput">Если не null, задает
        /// содержимое стандартного ввода.</param>
        public static string RunAndReadStandardOutput
            (
                ProcessStartInfo info,
                string? standardInput
            )
        {
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            if (!ReferenceEquals(standardInput, null))
            {
                info.RedirectStandardInput = true;
            }

            using var process = new Process
            {
                StartInfo = info
            };
            if (!ReferenceEquals(standardInput, null))
            {
                process.StandardInput.Write(standardInput);
            }
            process.Start();
            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        public static string RunAndReadStandardOutput
            (
                ProcessStartInfo info
            )
        {
            return RunAndReadStandardOutput
                (
                    info,
                    null
                );
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        public static string RunAndReadStandardOutput
            (
                string fileName,
                string arguments,
                string? standardInput
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var psi = new ProcessStartInfo
                (
                    fileName,
                    arguments
                );

            return RunAndReadStandardOutput(psi, standardInput);
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        public static string RunAndReadStandardOutput
            (
                string fileName,
                string arguments
            )
        {
            return RunAndReadStandardOutput
                (
                    fileName,
                    arguments,
                    null
                );
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="standardInput">Если не null, задает
        /// содержимое стандартного ввода.</param>
        public static string[] RunAndReadStandardOutputAndError
            (
                ProcessStartInfo info,
                string? standardInput
            )
        {
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            if (!ReferenceEquals(standardInput, null))
            {
                info.RedirectStandardInput = true;
            }

            using var process = new Process
            {
                StartInfo = info
            };
            if (!ReferenceEquals(standardInput, null))
            {
                process.StandardInput.Write(standardInput);
            }
            process.Start();
            process.WaitForExit();
            var stdout = process.StandardOutput.ReadToEnd();
            var stderr = process.StandardError.ReadToEnd();

            return new []
            {
                stdout, stderr
            };
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        public static string[] RunAndReadStandardOutputAndError
            (
                ProcessStartInfo info
            )
        {
            return RunAndReadStandardOutputAndError(info, null);
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        public static string[] RunAndReadStandardOutputAndError
            (
                string fileName,
                string arguments,
                string? standardInput
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var psi = new ProcessStartInfo (fileName, arguments);

            return RunAndReadStandardOutputAndError(psi, standardInput);
        }

        /// <summary>
        /// Запуск процесса с перехватом всего консольного вывода
        /// в строковую переменную.
        /// </summary>
        public static string[] RunAndReadStandardOutputAndError
            (
                string fileName,
                string arguments
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            return RunAndReadStandardOutputAndError(fileName, arguments, null);
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="milliseconds">Сколько ожидать.
        /// Неположительные значения = бесконечно.</param>
        /// <returns>Код, вовращенный процессом.
        /// Если с процессом не сложилось, возвращается -1.</returns>
        public static int RunAndWait
            (
                ProcessStartInfo info,
                int milliseconds
            )
        {
            using var process = new Process
            {
                StartInfo = info
            };
            process.Start();
            if (milliseconds >= 0)
            {
                if (!process.WaitForExit(milliseconds))
                {
                    return -1;
                }
            }
            else
            {
                process.WaitForExit();
            }

            return process.ExitCode;
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        /// <param name="milliseconds">Сколько ожидать.
        /// Неположительные значения = бесконечно.</param>
        /// <returns>Код, возвращенный процессом.
        /// Если с процессом как-то не сложилось, возвращается -1.</returns>
        public static int RunAndWait
            (
                string fileName,
                string arguments,
                int milliseconds
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var psi = new ProcessStartInfo
                (
                    fileName,
                    arguments
                )
            {
                UseShellExecute = false
            };

            return RunAndWait(psi, milliseconds);
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        public static int RunAndWait
            (
                string fileName,
                string arguments
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            return RunAndWait(fileName, arguments, -1);
        }

        /// <summary>
        /// Запускает процесс и ожидает его завершения.
        /// </summary>
        public static int RunAndWait
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var psi = new ProcessStartInfo(fileName);

            return RunAndWait(psi, -1);
        }

        #endregion
    }
}
