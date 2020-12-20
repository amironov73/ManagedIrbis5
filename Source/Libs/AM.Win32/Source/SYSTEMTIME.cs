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

namespace AM.Win32
{
    /// <summary>
    /// The SYSTEMTIME structure represents a date and time using
    /// individual members for the month, day, year, weekday, hour,
    /// minute, second, and millisecond.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct SYSTEMTIME
    {
        /// <summary>
        /// <para>Current year. The year must be greater than 1601.
        /// </para>
        /// <para>Windows Server 2003, Windows XP: The year cannot
        /// be greater than 30827.</para>
        /// </summary>
        public ushort wYear;

        /// <summary>
        /// Current month; January = 1, February = 2, and so on.
        /// </summary>
        public ushort wMonth;

        /// <summary>
        /// Current day of the week; Sunday = 0, Monday = 1, and so on.
        /// </summary>
        public ushort wDayOfWeek;

        /// <summary>
        /// Current day of the month.
        /// </summary>
        public ushort wDay;

        /// <summary>
        /// Current hour.
        /// </summary>
        public ushort wHour;

        /// <summary>
        /// Current minute.
        /// </summary>
        public ushort wMinute;

        /// <summary>
        /// Current second.
        /// </summary>
        public ushort wSecond;

        /// <summary>
        /// Current millisecond.
        /// </summary>
        public ushort wMilliseconds;

    } // struct SYSTEMTIME

} // namespace AM.Win32
