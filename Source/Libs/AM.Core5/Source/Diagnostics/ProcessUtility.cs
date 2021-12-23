// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
    /// Запуск процесса с перехватом всего консольного вывода
    /// в строковую переменную.
    /// </summary>
    /// <param name="info">Настройки для запуска процесса.</param>
    /// <param name="standardInput">Если не <c>null</c>, задает
    /// содержимое стандартного ввода.</param>
    /// <returns>Содержимое стандартного вывода.</returns>
    [ExcludeFromCodeCoverage]
    public static string RunAndReadStandardOutput
        (
            ProcessStartInfo info,
            string? standardInput = default
        )
    {
        Sure.NotNull (info);

        info.UseShellExecute = false;
        info.RedirectStandardOutput = true;
        if (standardInput is not null)
        {
            info.RedirectStandardInput = true;
        }

        using var process = new Process { StartInfo = info };
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

        var psi = new ProcessStartInfo
            (
                fileName,
                arguments
            );

        return RunAndReadStandardOutput (psi, standardInput);
    }

    /// <summary>
    /// Запуск процесса с перехватом всего консольного вывода
    /// в строковую переменную.
    /// </summary>
    /// <param name="info">Настройки для запуска процесса.</param>
    /// <param name="standardInput">Если не <c>null</c>, задает
    /// содержимое стандартного ввода.</param>
    /// <returns>Содержимого стандартных потоков вывода и ошибок.
    /// </returns>
    public static (string, string) RunAndReadStandardOutputAndError
        (
            ProcessStartInfo info,
            string? standardInput = null
        )
    {
        info.UseShellExecute = false;
        info.RedirectStandardOutput = true;
        info.RedirectStandardError = true;
        if (!ReferenceEquals (standardInput, null))
        {
            info.RedirectStandardInput = true;
        }

        using var process = new Process { StartInfo = info };
        if (!ReferenceEquals (standardInput, null))
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
    /// <param name="info">Настройки запуска программы.</param>
    /// <param name="milliseconds">Сколько миллисекунд ожидать.
    /// Неположительные значения = бесконечно.</param>
    /// <returns>Код, вовращенный процессом.
    /// Если с процессом не сложилось, возвращается -1.</returns>
    public static int RunAndWait
        (
            ProcessStartInfo info,
            int milliseconds = -1
        )
    {
        Sure.NotNull (info);

        using var process = new Process { StartInfo = info };
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

        var psi = new ProcessStartInfo (fileName, arguments)
        {
            UseShellExecute = false
        };

        return RunAndWait (psi, milliseconds);
    }

    #endregion
}
