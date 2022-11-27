// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* BoundsRectFlags.cs -- флаги для ограничивающего прямоугольника
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги для ограничивающего прямоугольника.
/// </summary>
[Flags]
public enum BoundsRectFlags
{
    /// <summary>
    /// Ошибка.
    /// </summary>
    ERROR = 0,

    /// <summary>
    /// Очищает ограничивающий прямоугольник после его возврата.
    /// Если этот флаг не установлен, ограничивающий прямоугольник
    /// не будет очищен.
    /// </summary>
    DCB_RESET = 0x0001,

    /// <summary>
    /// Добавляет прямоугольник, заданный параметром lprcBounds,
    /// к ограничивающему прямоугольнику (используя операцию
    /// объединения прямоугольников). Использование как DCB_RESET,
    /// так и DCB_ACCUMULATE устанавливает ограничивающий прямоугольник
    /// в прямоугольник, заданный параметром lprcBounds.
    /// </summary>
    DCB_ACCUMULATE = 0x0002,

    /// <summary>
    /// То же, что и DCB_ACCUMULATE.
    /// </summary>
    DCB_DIRTY = DCB_ACCUMULATE,

    /// <summary>
    /// Объединение флагов DCB_RESET и DCB_ACCUMULATE.
    /// </summary>
    DCB_SET = DCB_RESET | DCB_ACCUMULATE,

    /// <summary>
    /// Включает накопление границ, которое по умолчанию отключено.
    /// </summary>
    DCB_ENABLE = 0x0004,

    /// <summary>
    /// Отключает накопление границ.
    /// </summary>
    DCB_DISABLE = 0x0008
}
