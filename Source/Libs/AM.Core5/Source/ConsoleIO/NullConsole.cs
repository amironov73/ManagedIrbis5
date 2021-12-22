// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* NullConsole.cs -- нулевая консоль
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.ConsoleIO;

/// <summary>
/// Нулевая консоль. Не выполняет никаких реальных действий.
/// </summary>
public sealed class NullConsole
    : IConsoleDriver
{
    #region IConsoleDriver members

    /// <inheritdoc />
    public ConsoleColor BackgroundColor { get; set; }

    /// <inheritdoc />
    public ConsoleColor ForegroundColor { get; set; }

    /// <inheritdoc />
    public bool KeyAvailable => false;

    /// <inheritdoc />
    public string Title { get; set; } = string.Empty;

    /// <inheritdoc />
    public void Clear()
    {
    }

    /// <inheritdoc />
    public ConsoleKeyInfo ReadKey
        (
            bool intercept
        )
    {
        return new ConsoleKeyInfo();
    }

    /// <inheritdoc />
    public int Read()
    {
        return -1;
    }

    /// <inheritdoc />
    public string? ReadLine()
    {
        return null;
    }

    /// <inheritdoc />
    public void Write
        (
            string? text
        )
    {
    }

    /// <inheritdoc />
    public void WriteLine()
    {
    }

    #endregion
}
