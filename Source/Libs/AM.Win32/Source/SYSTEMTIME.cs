// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* SYSTEMTIME.cs -- системная дата и время
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Структура SYSTEMTIME представляет дату и время, используя
/// отдельные элементы для месяца, дня, года, дня недели, часа,
/// минуты, секунды и миллисекунды.
/// </summary>
[Serializable]
[StructLayout (LayoutKind.Sequential, Pack = 2)]
public struct SYSTEMTIME
{
    /// <summary>
    /// <para>Текущий год. Должен быть больше 1601.
    /// </para>
    /// <para>Windows Server 2003, Windows XP:
    /// год не должен быть больше 30827.</para>
    /// </summary>
    public ushort wYear;

    /// <summary>
    /// Текущий месяц; Январь = 1, Февраль = 2 и т. д.
    /// </summary>
    public ushort wMonth;

    /// <summary>
    /// Текущий день недели; воскресенье = 0, понедельник = 1 и т. д.
    /// </summary>
    public ushort wDayOfWeek;

    /// <summary>
    /// Текущий день месяца, нумерация с 1.
    /// </summary>
    public ushort wDay;

    /// <summary>
    /// Часы.
    /// </summary>
    public ushort wHour;

    /// <summary>
    /// Минуты.
    /// </summary>
    public ushort wMinute;

    /// <summary>
    /// Секунды.
    /// </summary>
    public ushort wSecond;

    /// <summary>
    /// Миллисекунды.
    /// </summary>
    public ushort wMilliseconds;
}
