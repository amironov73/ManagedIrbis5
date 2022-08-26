// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ConsoleModeFlags.cs -- опции режимов консоли
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Опции режимов консоли.
/// </summary>
[Flags]
public enum ConsoleModeFlags
{
    /// <summary>
    /// CTRL+C обрабатывается системой и не помещается в буфер ввода.
    /// Если входной буфер считывается с помощью ReadFile
    /// или ReadConsole, другие управляющие ключи обрабатываются
    /// системой и не возвращаются в буфер ReadFile или ReadConsole.
    /// Если также включен режим ENABLE_LINE_INPUT, символы возврата
    /// каретки, возврата каретки и перевода строки обрабатываются
    /// системой.
    /// </summary>
    ENABLE_PROCESSED_INPUT = 0x0001,

    /// <summary>
    /// Функция ReadFile или ReadConsole возвращает значение
    /// только при чтении символа возврата каретки. Если
    /// этот режим отключен, функции возвращаются, когда
    /// доступны один или несколько символов.
    /// </summary>
    ENABLE_LINE_INPUT = 0x0002,

    /// <summary>
    /// Символы, прочитанные функцией ReadFile или ReadConsole,
    /// записываются в активный экранный буфер по мере их чтения.
    /// Этот режим можно использовать, только если также включен
    /// режим ENABLE_LINE_INPUT.
    /// </summary>
    ENABLE_ECHO_INPUT = 0x0004,

    /// <summary>
    /// Действия пользователя, которые изменяют размер экранного
    /// буфера консоли, отображаются в буфере ввода консоли.
    /// Информация об этих событиях может быть прочитана
    /// из входного буфера приложениями, использующими функцию
    /// ReadConsoleInput, но не теми, которые используют
    /// ReadFile или ReadConsole.
    /// </summary>
    ENABLE_WINDOW_INPUT = 0x0008,

    /// <summary>
    /// Если указатель мыши находится в пределах границ окна консоли
    /// и окно имеет фокус клавиатуры, события мыши, генерируемые
    /// движением мыши и нажатием кнопок, помещаются в буфер ввода.
    /// Эти события отбрасываются ReadFile или ReadConsole,
    /// даже если этот режим включен.
    /// </summary>
    ENABLE_MOUSE_INPUT = 0x0010,

    /// <summary>
    /// Символы, записанные функцией WriteFile или WriteConsole
    /// или отраженные функцией ReadFile или ReadConsole,
    /// анализируются на наличие управляющих последовательностей
    /// ASCII, и выполняется правильное действие. Обрабатываются
    /// символы Backspace, Tab, Bell, возврата каретки
    /// и перевода строки.
    /// </summary>
    ENABLE_PROCESSED_OUTPUT = 0x0001,

    /// <summary>
    /// При записи с помощью WriteFile или WriteConsole
    /// или отображении с помощью ReadFile или ReadConsole курсор
    /// перемещается в начало следующей строки, когда он достигает
    /// конца текущей строки. Это приводит к тому, что строки,
    /// отображаемые в окне консоли, автоматически прокручиваются
    /// вверх, когда курсор выходит за пределы последней строки
    /// в окне. Это также заставляет содержимое экранного буфера
    /// консоли прокручиваться вверх (отбрасывая верхнюю строку
    /// экранного буфера консоли), когда курсор перемещается
    /// за пределы последней строки в экранном буфере консоли.
    /// Если этот режим отключен, последний символ в строке
    /// перезаписывается любыми последующими символами.
    /// </summary>
    ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002
}
