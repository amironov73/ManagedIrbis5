// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* ConsoleInput.cs -- консольный ввод-вывод
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.ConsoleIO;

/// <summary>
/// Консольный ввод-вывод.
/// </summary>
public static class ConsoleInput
{
    #region Properties

    /// <summary>
    /// Цвет фона.
    /// </summary>
    public static ConsoleColor BackgroundColor
    {
        get => Driver.BackgroundColor;
        set => Driver.BackgroundColor = value;
    }

    /// <summary>
    /// Драйвер консоли.
    /// </summary>
    public static IConsoleDriver Driver => _driver;

    /// <summary>
    /// Цвет текста.
    /// </summary>
    public static ConsoleColor ForegroundColor
    {
        get => Driver.ForegroundColor;
        set => Driver.ForegroundColor = value;
    }

    /// <summary>
    /// Пользователь нажимал клавиши?
    /// </summary>
    public static bool KeyAvailable => Driver.KeyAvailable;

    /// <summary>
    /// Заголовок консоли.
    /// </summary>
    public static string Title
    {
        get => Driver.Title;
        set => Driver.Title = value;
    }

    #endregion

    #region Private members

    private static IConsoleDriver _driver = new SystemConsole();

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка консоли.
    /// </summary>
    public static void Clear()
    {
        Driver.Clear();
    }

    /// <summary>
    /// Считывание одного символа.
    /// </summary>
    public static int Read()
    {
        return Driver.Read();
    }

    /// <summary>
    /// Считывание одного нажатия клавиши.
    /// </summary>
    public static ConsoleKeyInfo ReadKey
        (
            bool intercept
        )
    {
        return Driver.ReadKey (intercept);
    }

    /// <summary>
    /// Считывание строки.
    /// </summary>
    public static string? ReadLine()
    {
        return Driver.ReadLine();
    }

    /// <summary>
    /// Переключение на другой драйвер клавиатуры.
    /// </summary>
    public static IConsoleDriver SetDriver
        (
            IConsoleDriver driver
        )
    {
        var previousDriver = _driver;

        _driver = driver;

        return previousDriver;
    }

    /// <summary>
    /// Вывод текста.
    /// </summary>
    public static void Write
        (
            string text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            Driver.Write (text);
        }
    }

    /// <summary>
    /// Переход на новую строку.
    /// </summary>
    public static void WriteLine()
    {
        Driver.WriteLine();
    }

    /// <summary>
    /// Write text and goto next line.
    /// </summary>
    public static void WriteLine
        (
            string text
        )
    {
        Write (text);
        WriteLine();
    }

    #endregion
}
