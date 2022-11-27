// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontWeight.cs -- вес шрифта
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Вес (толщина) шрифта в диапазоне от 0 до 1000. Например, 400 — обычный шрифт,
/// а 700 — полужирный. Если это значение равно нулю, используется вес по умолчанию.
/// </summary>
[Flags]
public enum FontWeight
{
    /// <summary>
    /// Используется вес по умолчанию.
    /// </summary>
    FW_DONTCARE = 0,

    /// <summary>
    /// Тонкий.
    /// </summary>
    FW_THIN = 100,

    /// <summary>
    /// Сверхлегкий.
    /// </summary>
    FW_EXTRALIGHT = 200,

    /// <summary>
    /// Легкий.
    /// </summary>
    FW_LIGHT = 300,

    /// <summary>
    /// Нормальный.
    /// </summary>
    FW_NORMAL = 400,

    /// <summary>
    /// Средний.
    /// </summary>
    FW_MEDIUM = 500,

    /// <summary>
    /// Полужирный.
    /// </summary>
    FW_SEMIBOLD = 600,

    /// <summary>
    /// Жирный.
    /// </summary>
    FW_BOLD = 700,

    /// <summary>
    /// Сверхжирный.
    /// </summary>
    FW_EXTRABOLD = 800,

    /// <summary>
    /// Тяжелый.
    /// </summary>
    FW_HEAVY = 900,

    /// <summary>
    /// Сверхлегкий.
    /// </summary>
    FW_ULTRALIGHT = FW_EXTRALIGHT,

    /// <summary>
    /// Обычный.
    /// </summary>
    FW_REGULAR = FW_NORMAL,

    /// <summary>
    /// Полуж ирный.
    /// </summary>
    FW_DEMIBOLD = FW_SEMIBOLD,

    /// <summary>
    /// Сверхтяжелый.
    /// </summary>
    FW_ULTRABOLD = FW_EXTRABOLD,

    /// <summary>
    /// Черный.
    /// </summary>
    FW_BLACK = FW_HEAVY
}
