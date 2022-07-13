// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* WslDistributionFlags.cs -- поведение размещения WSL
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Перечисление WSL_DISTRIBUTION_FLAGS задает поведение размещения
/// в подсистеме Windows Subsystem for Linux (WSL).
/// </summary>
[Flags]
public enum WslDistributionFlags
{
    /// <summary>
    /// Флаги не специфицированы.
    /// </summary>
    None = 0,

    /// <summary>
    /// Разрешение взаимодействия WSL с обычными процессами Windows
    /// (например, пользователь может запустить "cmd.exe" или
    /// "notepad.exe" из сессии WSL).
    /// </summary>
    EnableInterop = 1,

    /// <summary>
    /// Добавление переменной окружения %PATH% из среды Windows
    /// в сессию WSL.
    /// </summary>
    AppendNtPath = 2,

    /// <summary>
    /// Автоматическое монтирование дисков Windows в сессии WSL
    /// (например, диск C: будет смонтирован как "/mnt/c").
    /// </summary>
    EnableDriveMounting = 4
}
