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
using System.Linq;

using AM;

using Istu.OldModel.Interfaces;

using LinqToDB;
using LinqToDB.Data;

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
        #region Properties

        /// <summary>
        /// Кладовка.
        /// </summary>
        public Storehouse Storehouse { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AttendanceManager
            (
                Storehouse storehouse
            )
        {
            Storehouse = storehouse;
        } // constructor

        #endregion

        #region Private members

        private DataConnection? _dataConnection;

        private DataConnection _GetDb() => _dataConnection ??= Storehouse.GetKladovka();

        #endregion

        #region IAttendanceManager members

        /// <inheritdoc cref="IAttendanceManager.RegisterAttendance"/>
        public void RegisterAttendance
            (
                Attendance info
            )
        {
            var db = _GetDb();
            db.Insert (info);
        } // method RegisterAttendance

        /// <inheritdoc cref="IAttendanceManager.RegisterAttendances"/>
        public void RegisterAttendances
            (
                IEnumerable<Attendance> attendances
            )
        {
            var database = _GetDb();
            database.BulkCopy (attendances);
        } // method RegisterAttendances

        /// <inheritdoc cref="IAttendanceManager.GetAttendances"/>
        public Attendance[] GetAttendances
            (
                string ticket
            )
        {
            var database = _GetDb();
            var attendances = database.GetAttendances();
            var result = attendances
                .Where (attendance => attendance.Ticket == ticket)
                .ToArray();

            return result;
        } // method GetAttendances

        /// <inheritdoc cref="IAttendanceManager.GetLastAttendance"/>
        public Attendance? GetLastAttendance
            (
                string ticket
            )
        {
            var db = _GetDb();
            var attendances = db.GetAttendances();
            var result = attendances
                .Where (attendance => attendance.Ticket == ticket)
                .OrderByDescending (attendance => attendance.Moment)
                .FirstOrDefault();

            return result;
        } // method GetLastAttendance

        /// <inheritdoc cref="IAttendanceManager.GetLastReaders"/>
        public Reader[] GetLastReaders
            (
                int howMany = 200
            )
        {
            Sure.Positive (howMany, nameof (howMany));

            var db = _GetDb();
            var attendances = db.GetAttendances();
            var readers = db.GetReaders();
            var result = readers.Join
                    (
                        attendances.Where (attendance => attendance.Type == "a")
                            .OrderByDescending (attendance => attendance.Moment),
                        reader => reader.Ticket,
                        attendance => attendance.Ticket,
                        (reader, attendance) => reader
                    )
                .Take (howMany)
                .ToArray()
                .Distinct (new ReaderComparer())
                .ToArray();

            return result;

        } // method GetLatestReaders

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_dataConnection is not null)
            {
                _dataConnection.Dispose();
                _dataConnection = null;
            }

        } // method Dispose

        #endregion

    } // class AttendanceManager

} // namespace Istu.OldModel.Implementation
