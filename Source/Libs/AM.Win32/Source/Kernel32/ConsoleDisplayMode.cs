// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ConsoleDisplayMode.cs -- флаги режима отображения консоли
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги режима отображения консоли.
/// </summary>
[Flags]
public enum ConsoleDisplayMode
{
    /// <summary>
    /// Режим по умолчанию.
    /// </summary>
    DEFAULT = 0,

    /// <summary>
    /// Полноэкранная консоль. Консоль находится в этом режиме,
    /// как только окно разворачивается. В этот момент переход
    /// в полноэкранный режим еще может не получиться.
    /// </summary>
    CONSOLE_FULLSCREEN = 1,

    /// <summary>
    /// Полноэкранная консоль, напрямую взаимодействующая
    /// с видеооборудованием. Этот режим устанавливается после
    /// перехода консоли в режим CONSOLE_FULLSCREEN, чтобы указать,
    /// что переход в полноэкранный режим завершен.
    /// </summary>
    CONSOLE_FULLSCREEN_HARDWARE = 2
}
