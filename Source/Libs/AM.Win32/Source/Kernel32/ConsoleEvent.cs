// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ConsoleEvent.cs -- константы для событий консоли
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Константы для событий консоли.
/// </summary>
[Flags]
public enum ConsoleEvent
{
    /// <summary>
    /// Был получен сигнал CTRL+C либо от ввода с клавиатуры,
    /// либо от сигнала, сгенерированного функцией
    /// GenerateConsoleCtrlEvent.
    /// </summary>
    CTRL_C_EVENT = 0,

    /// <summary>
    /// Был получен сигнал CTRL+BREAK либо от ввода с клавиатуры,
    /// либо от сигнала, сгенерированного GenerateConsoleCtrlEvent.
    /// </summary>
    CTRL_BREAK_EVENT = 1,

    /// <summary>
    /// Сигнал, который система отправляет всем процессам,
    /// подключенным к консоли, когда пользователь закрывает
    /// консоль (либо нажав кнопку «Закрыть» в меню окна консоли,
    /// либо нажав кнопку «Завершить задачу» в диспетчере задач).
    /// </summary>
    CTRL_CLOSE_EVENT = 2,

    /// <summary>
    /// Сигнал, который система отправляет всем консольным процессам,
    /// когда пользователь выходит из системы. Этот сигнал
    /// не указывает, какой пользователь выходит из системы,
    /// поэтому никаких предположений делать нельзя.
    /// </summary>
    CTRL_LOGOFF_EVENT = 5,

    /// <summary>
    /// <para>Сигнал, который система отправляет всем консольным
    /// процессам при завершении работы системы.</para>
    /// <para>Обратите внимание, что этот сигнал принимается только
    /// службами. Интерактивные приложения завершаются при выходе
    /// из системы, поэтому они отсутствуют, когда система отправляет
    /// этот сигнал. У служб также есть собственный механизм
    /// уведомления о событиях завершения работы.</para>
    /// </summary>
    CTRL_SHUTDOWN_EVENT = 6
}
