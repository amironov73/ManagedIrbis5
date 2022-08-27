// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* SYSTEM_INFO.cs -- информация о состоянии компьютерной системы
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

namespace AM.Win32;

/// <summary>
/// Структура <c>SYSTEM_INFO</c> содержит информацию о текущей
/// компьютерной системе. Это включает в себя архитектуру
/// и тип процессора, количество процессоров в системе,
/// размер страницы и другую подобную информацию.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Explicit, Size = 36)]
public struct SYSTEM_INFO
{
    /// <summary>
    /// <para>Устаревший элемент, оставленный для совместимости
    /// с Windows NT 3.5 и более ранними версиями. Новые приложения
    /// должны использовать ветвь объединения
    /// <c>wProcessorArchitecture.</c></para>
    /// <para>Windows Me/98/95: система всегда устанавливает для этого
    /// члена нулевое значение, определенное для
    /// <c>PROCESSOR_ARCHITECTURE_INTEL</c>.</para>
    /// </summary>
    [FieldOffset (0)]
    public uint dwOemId;

    /// <summary>
    /// Архитектура процессора.
    /// </summary>
    [FieldOffset (0)]
    public ushort wProcessorArchitecture;

    /// <summary>
    /// Зарезервировано для использования в будущем.
    /// </summary>
    [FieldOffset (2)]
    public ushort wReserved;

    /// <summary>
    /// Размер страницы и степень детализации защиты страницы
    /// и обязательства. Это размер страницы, используемый функцией
    /// <c>VirtualAlloc</c>.
    /// </summary>
    [FieldOffset (4)]
    public uint dwPageSize;

    /// <summary>
    /// Указатель на наименьший адрес памяти, доступный приложениям
    /// и библиотекам динамической компоновки (DLL).
    /// </summary>
    [FieldOffset (8)]
    public uint lpMinimumApplicationAddress;

    /// <summary>
    /// Указатель на самый высокий адрес памяти, доступный
    /// для приложений и библиотек DLL.
    /// </summary>
    [FieldOffset (12)]
    public uint lpMaximumApplicationAddress;

    /// <summary>
    /// Маска, представляющая набор процессоров, настроенных
    /// в системе. Бит 0 - процессор 0; бит 31 - процессор 31.
    /// </summary>
    [FieldOffset (16)]
    public uint dwActiveProcessorMask;

    /// <summary>
    /// Количество процессоров в системе.
    /// </summary>
    [FieldOffset (20)]
    public uint dwNumberOfProcessors;

    /// <summary>
    /// Устаревшее поле, оставленное для совместимости
    /// с Windows NT 3.5 и более ранними версиями. Используйте поля
    /// <see cref="wProcessorArchitecture" />,
    /// <see cref="wProcessorLevel" /> и
    /// <see cref="wProcessorRevision" />,
    /// чтобы определить тип процессора.
    /// </summary>
    [FieldOffset (24)]
    public uint dwProcessorType;

    /// <summary>
    /// Детализация, с которой выделяется виртуальная память.
    /// Например, запрос <c>VirtualAlloc</c> на выделение 1 байта
    /// зарезервирует адресное пространство в байтах
    /// <c>dwAllocationGranularity</c>. Раньше это значение было
    /// жестко закодировано как 64 КБ, но для других аппаратных
    /// архитектур могут потребоваться другие значения.
    /// </summary>
    [FieldOffset (28)]
    public uint dwAllocationGranularity;

    /// <summary>
    /// Уровень процессора, зависящий от архитектуры системы.
    /// Его следует использовать только для демонстрации.
    /// Чтобы определить набор функций процессора,
    /// используйте функцию <c>IsProcessorFeaturePresent</c>.
    /// </summary>
    [FieldOffset (32)]
    public ushort wProcessorLevel;

    /// <summary>
    /// Ревизия процессора, зависящая от архитектуры.
    /// </summary>
    [FieldOffset (34)]
    public ushort wProcessorRevision;
}
