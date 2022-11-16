// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ProcessUtility.cs -- работа с процессами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;

#endregion

#nullable enable

namespace AM.Diagnostics;

/// <summary>
/// Работа с процессами.
/// </summary>
public static class ProcessUtility
{
    #region Public methods

    /// <summary>
    /// Escape an argument for inclusion within a shell command.
    /// </summary>
    /// <param name="argument">The argument to escape.</param>
    /// <param name="quote"></param>
    /// <returns>An escapd string.</returns>
    public static string Escape
        (
            string argument,
            bool quote = false
        )
    {
        Sure.NotNull (argument);

        if (quote)
        {
            return "\""
                + argument
                    .Replace ("\\", "\\\\")
                    .Replace ("\"", "\\\"")
                    .Replace (";", "\\;")
                + "\"";
        }

        return argument
            .Replace ("\\", "\\\\")
            .Replace ("\"", "\\\"")
            .Replace (";", "\\;");
    }

    /// <summary>
    ///     Execute a shell command.
    /// </summary>
    /// <param name="filename">The path to the executable.</param>
    /// <param name="arguments">The arguments to add.</param>
    /// <param name="timeout">The timeout for the command.</param>
    /// <returns>The output of the shell command.</returns>
    public static string ExecuteShellCommand
        (
            string filename,
            string arguments,
            int timeout = 9000
        )
    {
        string result;
        using var process = new Process();
        process.StartInfo.FileName = filename;
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;

        //set it to english
        if (!process.StartInfo.EnvironmentVariables.ContainsKey ("LC_ALL"))
        {
            process.StartInfo.EnvironmentVariables.Add ("LC_ALL", "C");
        }

        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        var output = new StringBuilder();
        var error = new StringBuilder();

        using var outputWaitHandle = new AutoResetEvent (false);
        using var errorWaitHandle = new AutoResetEvent (false);
        process.OutputDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data is null)
            {
                outputWaitHandle.Set();
            }
            else
            {
                output.AppendLine (eventArgs.Data);
            }
        };
        process.ErrorDataReceived += (_, eventArgs) =>
        {
            if (eventArgs.Data == null)
            {
                errorWaitHandle.Set();
            }
            else
            {
                error.AppendLine (eventArgs.Data);
            }
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        if (process.WaitForExit (timeout)
            && outputWaitHandle.WaitOne (timeout)
            && errorWaitHandle.WaitOne (timeout))
        {
            if (process.ExitCode == 0)
            {
                result = output.ToString();
            }
            else
            {
                throw new ArsMagnaException (error.ToString());
            }
        }
        else
        {
            // Timed out.
            throw new ArsMagnaException ("Timed out");
        }

        return result;
    }

    /// <summary>
    /// Запуск процесса с перехватом всего консольного вывода
    /// в строковую переменную.
    /// </summary>
    /// <param name="processStartInfo">Настройки для запуска процесса.</param>
    /// <param name="standardInput">Если не <c>null</c>, задает
    /// содержимое стандартного ввода.</param>
    /// <returns>Содержимое стандартного вывода.</returns>
    [ExcludeFromCodeCoverage]
    public static string RunAndReadStandardOutput
        (
            ProcessStartInfo processStartInfo,
            string? standardInput = default
        )
    {
        Sure.NotNull (processStartInfo);

        processStartInfo.UseShellExecute = false;
        processStartInfo.RedirectStandardOutput = true;
        if (standardInput is not null)
        {
            processStartInfo.RedirectStandardInput = true;
        }

        using var process = new Process { StartInfo = processStartInfo };
        if (standardInput is not null)
        {
            process.StandardInput.Write (standardInput);
        }

        process.Start();
        process.WaitForExit();

        return process.StandardOutput.ReadToEnd();
    }

    /// <summary>
    /// Запуск процесса с перехватом всего консольного вывода
    /// в строковую переменную.
    /// </summary>
    /// <param name="fileName">Имя файла с программой.</param>
    /// <param name="arguments">Аргументы командной строки.</param>
    /// <param name="standardInput">Если не <c>null</c>, задает
    /// содержимого стандартного ввода.</param>
    /// <returns>Содержимое стандартного вывода.</returns>
    public static string RunAndReadStandardOutput
        (
            string fileName,
            string arguments,
            string? standardInput = null
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var processStartInfo = new ProcessStartInfo
            (
                fileName,
                arguments
            );

        return RunAndReadStandardOutput (processStartInfo, standardInput);
    }

    /// <summary>
    /// Запуск процесса с перехватом всего консольного вывода
    /// в строковую переменную.
    /// </summary>
    /// <param name="processStartInfo">Настройки для запуска процесса.</param>
    /// <param name="standardInput">Если не <c>null</c>, задает
    /// содержимое стандартного ввода.</param>
    /// <returns>Содержимого стандартных потоков вывода и ошибок.
    /// </returns>
    public static (string, string) RunAndReadStandardOutputAndError
        (
            ProcessStartInfo processStartInfo,
            string? standardInput = null
        )
    {
        Sure.NotNull (processStartInfo);

        processStartInfo.UseShellExecute = false;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
        if (standardInput is not null)
        {
            processStartInfo.RedirectStandardInput = true;
        }

        using var process = new Process { StartInfo = processStartInfo };
        if (standardInput is not null)
        {
            process.StandardInput.Write (standardInput);
        }

        process.Start();
        process.WaitForExit();
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();

        return (stdout, stderr);
    }

    /// <summary>
    /// Запуск процесса с перехватом всего консольного вывода
    /// в строковую переменную.
    /// </summary>
    /// <param name="fileName">Имя файла программы.</param>
    /// <param name="arguments">Аргументы командной строки.</param>
    /// <param name="standardInput">Если не <c>null</c>, задает
    /// содержимое потока стандартного ввода.</param>
    /// <returns>Содержимое стандартных потоков вывода и ошибок.
    /// </returns>
    public static (string, string) RunAndReadStandardOutputAndError
        (
            string fileName,
            string arguments,
            string? standardInput = default
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var psi = new ProcessStartInfo (fileName, arguments);

        return RunAndReadStandardOutputAndError (psi, standardInput);
    }

    /// <summary>
    /// Запускает процесс и ожидает его завершения.
    /// </summary>
    /// <param name="processStartInfo">Настройки запуска программы.</param>
    /// <param name="milliseconds">Сколько миллисекунд ожидать.
    /// Неположительные значения = бесконечно.</param>
    /// <returns>Код, вовращенный процессом.
    /// Если с процессом не сложилось, возвращается -1.</returns>
    public static int RunAndWait
        (
            ProcessStartInfo processStartInfo,
            int milliseconds = -1
        )
    {
        Sure.NotNull (processStartInfo);

        using var process = new Process { StartInfo = processStartInfo };
        process.Start();
        if (milliseconds >= 0)
        {
            if (!process.WaitForExit (milliseconds))
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
    /// <param name="fileName">Имя файла программы.</param>
    /// <param name="arguments">Аргументы командной строки.</param>
    /// <param name="milliseconds">Сколько миллисекунд ожидать.
    /// Неположительные значения = бесконечно.</param>
    /// <returns>Код, возвращенный процессом.
    /// Если с процессом как-то не сложилось, возвращается -1.</returns>
    public static int RunAndWait
        (
            string fileName,
            string arguments,
            int milliseconds = -1
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var processStartInfo = new ProcessStartInfo (fileName, arguments)
        {
            UseShellExecute = false
        };

        return RunAndWait (processStartInfo, milliseconds);
    }

    #endregion
}
