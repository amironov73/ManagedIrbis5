// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* AttendanceManager.cs -- менеджер регистрации событий книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Istu.OldModel.Interfaces;

#endregion

#nullable enable

namespace Istu.OldModel.Implementation
{
    /// <summary>
    /// Менеджер регистрации событий книговыдачи.
    /// </summary>
    public sealed class AttendanceManager
        : IAttendanceManager
    {
        #region IAttendanceManager members

        /// <inheritdoc cref="IAttendanceManager.RegisterAttendance"/>
        public void RegisterAttendance(Attendance info)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IAttendanceManager.RegisterAttendances"/>
        public void RegisterAttendances(IEnumerable<Attendance> attendances)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IAttendanceManager.GetAttendances"/>
        public Attendance[] GetAttendances(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IAttendanceManager.GetLastAttendance"/>
        public Attendance GetLastAttendance(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="GetLatestReadersWithCellphone"/>
        public Reader[] GetLatestReadersWithCellphone()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

    } // class AttendanceManager

} // namespace Istu.OldModel.Implementation
