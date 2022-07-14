// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SYSTEM_POWER_STATUS.cs -- статус электрического питания системы
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Структура содержит информацию об электрическом питании системы.
/// </summary>
[StructLayout (LayoutKind.Explicit, Size = 12)]
public struct SYSTEM_POWER_STATUS
{
    /// <summary>
    /// Состояние питания.
    /// </summary>
    [FieldOffset (0)]
    public ACPowerStatus ACLineStatus;

    /// <summary>
    /// Состояние батареи.
    /// </summary>
    [FieldOffset (1)]
    public BatteryFlags BatteryFlag;

    /// <summary>
    /// Процент заряда батареи (от 0 до 100, 255 означает "неизвестно").
    /// </summary>
    [FieldOffset (2)]
    public byte BatteryLifePercent;

    /// <summary>
    /// Зарезервировано, должно быть 0.
    /// </summary>
    [FieldOffset (3)]
    public byte Reserved1;

    /// <summary>
    /// Количество секунд работы системы на оставшемся заряде батареи,
    /// -1 означает "неизвестно".
    /// </summary>
    [FieldOffset (4)]
    public int BatteryLifeTime;

    /// <summary>
    /// Количество секунд работы при полной зарядке батареи,
    /// -1 означает "неизвестно".
    /// </summary>
    [FieldOffset (8)]
    public int BatteryFullLifeTime;
}
