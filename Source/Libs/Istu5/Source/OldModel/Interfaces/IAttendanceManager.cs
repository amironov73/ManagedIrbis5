// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IAttendanceManager.cs -- интерфейс менеджера регистрации событий книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace Istu.OldModel.Interfaces
{
    /// <summary>
    /// Интерфейс менеджера регистрации событий книговыдачи.
    /// </summary>
    public interface IAttendanceManager
        : IDisposable
    {
        /// <summary>
        /// Регистрация посещения.
        /// </summary>
        void RegisterAttendance (Attendance info);

        /// <summary>
        /// Регистрация нескольких посещений сразу.
        /// </summary>
        void RegisterAttendances (IEnumerable<Attendance> attendances);

        /// <summary>
        /// Получение всех посещений для указанного читательского билета.
        /// </summary>
        Attendance[] GetAttendances (string ticket);

        /// <summary>
        /// Получение последнего по времени посещения
        /// для указанного читательского билета.
        /// </summary>
        Attendance? GetLastAttendance (string ticket);

        /// <summary>
        /// Получение последних по времени читателей,
        /// посещавших библиотеку.
        /// </summary>
        Reader[] GetLastReaders (int howMany = 200);

    } // interface IAttendanceManager

} // namespace Istu.OldModel.Interfaces
