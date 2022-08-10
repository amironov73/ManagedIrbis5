// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* VisitInfoUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Utility routines for <see cref="VisitInfo"/>.
    /// </summary>
    public static class VisitInfoUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Flatten visits from given readers.
        /// </summary>
        public static VisitInfo[] AllVisits
            (
                this IEnumerable<ReaderInfo> readers
            )
        {
            var result = readers.SelectMany
                (
                    reader => reader.Visits ?? Array.Empty<VisitInfo>()
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for all dates.
        /// </summary>
        public static VisitInfo[] GetDebt
            (
                this IEnumerable<VisitInfo> visits
            )
        {
            var result = visits.Where
                (
                    loan => !loan.IsReturned
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for given date.
        /// </summary>
        public static VisitInfo[] GetDebt
            (
                this IEnumerable<VisitInfo> visits,
                DateTime deadline
            )
        {
            var date = IrbisDate.ConvertDateToString(deadline);

            var result = visits.Where
                (
                    loan => !loan.IsVisit
                            && !loan.IsReturned
                            && string.CompareOrdinal(date, loan.DateExpectedString) >= 0
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for given date.
        /// </summary>
        public static VisitInfo[] GetDebt
            (
                this IEnumerable<VisitInfo> visits,
                string deadline
            )
        {
            var result = visits.Where
                (
                    loan => !loan.IsVisit
                            && !loan.IsReturned
                            && string.CompareOrdinal(deadline, loan.DateExpectedString) >= 0
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get debt loans for given date.
        /// </summary>
        public static VisitInfo[] GetDebt
            (
                this IEnumerable<VisitInfo> visits,
                string fromDeadline,
                string toDeadline
            )
        {
            var result = visits.Where
                (
                    loan =>
                    {
                        var date = loan.DateExpectedString;

                        return !loan.IsVisit
                               && !loan.IsReturned
                               && string.CompareOrdinal(date, fromDeadline) >= 0
                               && string.CompareOrdinal(date, toDeadline) <= 0;
                    }
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get loans (not pure visits).
        /// </summary>
        public static VisitInfo[] GetLoans
            (
                this IEnumerable<VisitInfo> visits
            )
        {
            var result = visits.Where
                (
                    visit => visit.IsVisit
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get pure visits (not loans).
        /// </summary>
        public static VisitInfo[] GetPureVisits
            (
                this IEnumerable<VisitInfo> visits
            )
        {
            var result = visits.Where
                (
                    visit => !visit.IsVisit
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get visits for given chair.
        /// </summary>
        public static VisitInfo[] GetVisits
            (
                this IEnumerable<VisitInfo> visits,
                ChairInfo chair
            )
        {
            var code = chair.Code;
            var result = visits.Where
                (
                    visit => visit.Department.SameString(code)
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get visits between given dates.
        /// </summary>
        public static VisitInfo[] GetVisits
            (
                this IEnumerable<VisitInfo> visits,
                DateTime dateFrom,
                DateTime dateTo
            )
        {
            var date1 = IrbisDate.ConvertDateToString(dateFrom);
            var date2 = IrbisDate.ConvertDateToString(dateTo);
            var result = visits.Where
                (
                    visit =>
                    {
                        var given = visit.DateGivenString;
                        return string.CompareOrdinal(given, date1) >= 0
                               && string.CompareOrdinal(given, date2) <= 0;
                    }
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Get visits for given date.
        /// </summary>
        public static VisitInfo[] GetVisits
            (
                this IEnumerable<VisitInfo> visits,
                DateTime date
            )
        {
            var date1 = IrbisDate.ConvertDateToString(date);
            var result = visits.Where
                (
                    visit => string.CompareOrdinal(visit.DateGivenString, date1) == 0
                )
                .ToArray();

            return result;
        }

        /// <summary>
        /// Один и тот же визит?
        /// </summary>
        public static bool SameVisit
            (
                this VisitInfo first,
                VisitInfo second
            )
        {
            var result = string.CompareOrdinal(first.Database, second.Database) == 0
                          && string.CompareOrdinal(first.Index, second.InventoryNumber) == 0
                          && string.CompareOrdinal(first.InventoryNumber, second.InventoryNumber) == 0
                          && string.CompareOrdinal(first.Barcode, second.Barcode) == 0
                          && string.CompareOrdinal(first.DateGivenString, second.DateGivenString) == 0;

            return result;
        }

        #endregion
    }
}
