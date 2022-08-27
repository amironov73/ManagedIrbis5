// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PipeWaitFlags.cs -- интервалы ожидания для именованных каналов
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Интервалы ожидания для именованных каналов.
/// </summary>
[Flags]
public enum PipeWaitFlags
{
    /// <summary>
    /// Бесконечное ожидание.
    /// </summary>
    NMPWAIT_WAIT_FOREVER = unchecked ((int) 0xffffffff),

    /// <summary>
    /// Не ждет именованный канал. Если именованный канал недоступен,
    /// функция возвращает ошибку.
    /// </summary>
    NMPWAIT_NOWAIT = 0x00000001,

    /// <summary>
    /// Использует время ожидания по умолчанию, указанное при вызове
    /// функции <c>CreateNamedPipe</c>.
    /// </summary>
    NMPWAIT_USE_DEFAULT_WAIT = 0x00000000
}
