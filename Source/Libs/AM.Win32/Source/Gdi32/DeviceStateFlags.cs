// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* DeviceStateFlags.cs -- флаги состояния устройства отображения
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Флаги состояни устройства отображения.
/// </summary>
[Flags]
public enum DeviceStateFlags
{
    /// <summary>
    /// Устройство является частью рабочего стола.
    /// </summary>
    DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001,

    /// <summary>
    /// Назначение флага неизвестно.
    /// </summary>
    DISPLAY_DEVICE_MULTI_DRIVER = 0x00000002,

    /// <summary>
    /// Основной рабочий стол находится на устройстве. Для системы
    /// с одной видеокартой этот флаг всегда установлен. В системе
    /// с несколькими видеокартами этот флаг может быть только
    /// у одного устройства.
    /// </summary>
    DISPLAY_DEVICE_PRIMARY_DEVICE = 0x00000004,

    /// <summary>
    /// Представляет псевдоустройство, используемое для зеркального
    /// отображения вывода приложения для удаленного взаимодействия
    /// или других целей. С этим устройством связан невидимый
    /// псевдомонитор. Например, NetMeeting использует его.
    /// Обратите внимание, что GetSystemMetrics(SM_MONITORS)
    /// учитывает только видимые мониторы.
    /// </summary>
    DISPLAY_DEVICE_MIRRORING_DRIVER = 0x00000008,

    /// <summary>
    /// Устройство совместимо с VGA.
    /// </summary>
    DISPLAY_DEVICE_VGA_COMPATIBLE = 0x00000010,

    /// <summary>
    /// Устройство съемное; он не может быть основным дисплеем.
    /// </summary>
    DISPLAY_DEVICE_REMOVABLE = 0x00000020,

    /// <summary>
    /// Устройство имеет больше режимов отображения, чем поддерживают
    /// его устройства вывода.
    /// </summary>
    DISPLAY_DEVICE_MODESPRUNED = 0x08000000,

    /// <summary>
    /// Назначение флага неизвестно.
    /// </summary>
    DISPLAY_DEVICE_REMOTE = 0x04000000,

    /// <summary>
    /// Назначение флага неизвестно.
    /// </summary>
    DISPLAY_DEVICE_DISCONNECT = 0x02000000
}
