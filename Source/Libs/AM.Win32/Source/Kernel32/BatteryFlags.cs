// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* BatteryFlags.cs -- статус заряда батареи
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Статус заряда батареи.
/// </summary>
[Flags]
public enum BatteryFlags
    : byte
{
    /// <summary>
    /// Высокий заряд.
    /// </summary>
    High = 1,

    /// <summary>
    /// Низкий заряд.
    /// </summary>
    Low = 2,

    /// <summary>
    /// Критически низкий заряд.
    /// </summary>
    Critical = 4,

    /// <summary>
    /// Происходит зарядка от сети.
    /// </summary>
    Charging = 8,

    /// <summary>
    /// Системная батарея отсутствует.
    /// </summary>
    NoSystemBattery = 128,

    /// <summary>
    /// Статус неизвестен.
    /// </summary>
    Unknown = 255
}
