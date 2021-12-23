// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IConsoleOutputReceiver.cs -- принимает вывод консольного процесса
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Diagnostics;

/// <summary>
/// Принимает вывод консольного процесса.
/// </summary>
public interface IConsoleOutputReceiver
{
    /// <summary>
    /// Прием очередной порции данных.
    /// </summary>
    void ReceiveConsoleOutput
        (
            string text
        );
}
