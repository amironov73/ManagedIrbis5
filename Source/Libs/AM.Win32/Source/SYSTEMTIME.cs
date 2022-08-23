// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

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
    #region Properties

    /// <summary>
    /// <para>Текущий год. Должен быть больше 1601.
    /// </para>
    /// <para>Windows Server 2003, Windows XP:
    /// год не должен быть больше 30827.</para>
    /// </summary>
    public ushort Year;

    /// <summary>
    /// Текущий месяц; Январь = 1, Февраль = 2 и т. д.
    /// </summary>
    public ushort Month;

    /// <summary>
    /// Текущий день недели; воскресенье = 0, понедельник = 1 и т. д.
    /// </summary>
    public ushort DayOfWeek;

    /// <summary>
    /// Текущий день месяца, нумерация с 1.
    /// </summary>
    public ushort Day;

    /// <summary>
    /// Часы.
    /// </summary>
    public ushort Hour;

    /// <summary>
    /// Минуты.
    /// </summary>
    public ushort Minute;

    /// <summary>
    /// Секунды.
    /// </summary>
    public ushort Second;

    /// <summary>
    /// Миллисекунды.
    /// </summary>
    public ushort Milliseconds;

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"{Year}.{Month}.{Day} ({DayOfWeek}) {Hour}:{Minute}:{Second}:{Milliseconds}";
    }

    #endregion
}
