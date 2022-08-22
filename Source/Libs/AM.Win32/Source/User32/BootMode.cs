// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* BootMode.cs -- режим загрузки операционной системы
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Режим загрузки операционной системы.
/// </summary>
public enum BootMode
{
    /// <summary>
    /// Неизвестно.
    /// </summary>
    UnknownBootMode = -1,

    /// <summary>
    /// Обычный режим.
    /// </summary>
    NormalBoot = 0,

    /// <summary>
    /// Безопасный режим.
    /// </summary>
    FailSafeBoot = 1,

    /// <summary>
    /// Безопасный режим с поддержкой сети.
    /// </summary>
    FailSafeWithNetworkSupportBoot = 2
}
