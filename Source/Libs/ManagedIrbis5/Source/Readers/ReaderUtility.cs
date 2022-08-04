// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* ReaderUtility.cs -- методы для работы с БД читателей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Net;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Методы для работы с БД читателей.
/// </summary>
public static class ReaderUtility
{
    #region Constants

    /// <summary>
    /// Имя по умолчанию для базы данных читателей.
    /// </summary>
    public const string DefaultDatabaseName = "RDR";

    /// <summary>
    /// Префикс по умолчанию для поиска читателя по идентификатору.
    /// </summary>
    public const string DefaultIdentifierPrefix = "RI=";

    #endregion

    #region Properties

    /// <summary>
    /// Database name.
    /// </summary>
    public static string DatabaseName { get; set; } = DefaultDatabaseName;

    /// <summary>
    /// Reader identifier search prefix.
    /// </summary>
    public static string IdentifierPrefix { get; set; } = DefaultIdentifierPrefix;

    #endregion

    #region Public methods

    /// <summary>
    /// Fix the reader email.
    /// </summary>
    /// <returns></returns>
    public static string? FixEmail
        (
            string? email
        )
    {
        if (string.IsNullOrEmpty (email))
        {
            return email;
        }

        email = MailUtility.CleanupEmail (email);

        return email;
    }

    /// <summary>
    /// Fix the reader name: remove extra spaces.
    /// </summary>
    public static string? FixName
        (
            string? name
        )
    {
        if (string.IsNullOrEmpty (name))
        {
            return name;
        }

        name = name.Trim();
        name = name.Replace (',', ' ');
        name = name.Replace ('.', ' ');
        while (name.Contains ("  "))
        {
            name = name.Replace ("  ", " ");
        }

        return name;
    }

    /// <summary>
    /// Fix the phone number: remove spaces
    /// and bad characters.
    /// </summary>
    public static string? FixPhone
        (
            string? phone
        )
    {
        if (string.IsNullOrEmpty (phone))
        {
            return phone;
        }

        phone = phone.Trim();
        if (phone.StartsWith ("+7"))
        {
            phone = "8" + phone.Substring (2);
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (phone.Length);
        foreach (var c in phone)
        {
            if (c.IsArabicDigit())
            {
                builder.Append (c);
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Fix the ticket number: remove spaces,
    /// convert cyrillic characters to latin equivalents.
    /// </summary>
    public static string? FixTicket
        (
            string? ticket
        )
    {
        if (string.IsNullOrEmpty (ticket))
        {
            return ticket;
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (ticket.Length);
        foreach (var c in ticket)
        {
            if (c <= ' ')
            {
                continue;
            }

            switch (c)
            {
                case 'А':
                case 'а':
                    builder.Append ('A');
                    break;

                case 'В':
                case 'в':
                    builder.Append ('B');
                    break;

                case 'Е':
                case 'е':
                    builder.Append ('E');
                    break;

                case 'О':
                case 'о':
                    builder.Append ('0');
                    break;

                case 'С':
                case 'с':
                    builder.Append ('C');
                    break;

                default:
                    if (c < 256)
                    {
                        builder.Append (char.ToUpper (c));
                    }

                    break;
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Слияние записей о читателях из разных баз.
    /// </summary>
    /// <remarks>
    /// Слияние происходит на основе номера читательского билета.
    /// </remarks>
    public static List<ReaderInfo> MergeReaders
        (
            List<ReaderInfo> readers
        )
    {
        Sure.NotNull (readers);

        var grouped = readers
            .Where (r => !string.IsNullOrEmpty (r.Ticket))
            .GroupBy (r => r.Ticket);

        var result = new List<ReaderInfo> (readers.Count);

        foreach (var grp in grouped)
        {
            var first = grp.First();
            first.Visits = grp
                .SelectMany (r => r.Visits ?? Array.Empty<VisitInfo>())
                .ToArray();
            result.Add (first);
        }

        return result;
    }

    /// <summary>
    /// Подсчёт количества событий.
    /// </summary>
    public static int CountEvents
        (
            List<ReaderInfo> readers,
            DateTime fromDay,
            DateTime toDay,
            bool visit
        )
    {
        Sure.NotNull (readers);

        var fromDayString = IrbisDate.ConvertDateToString (fromDay);
        var toDayString = IrbisDate.ConvertDateToString (toDay);
        var result = readers
            .SelectMany (r => r.Visits ?? Array.Empty<VisitInfo>())
            .Count (v =>
                string.CompareOrdinal (v.DateGivenString, fromDayString) >= 0
                && string.CompareOrdinal (v.DateGivenString, toDayString) <= 0
                && v.IsVisit == visit);

        return result;
    }

    /// <summary>
    /// Подсчёт количества событий
    /// </summary>
    public static int CountEvents
        (
            List<ReaderInfo> readers,
            DateTime fromDay,
            DateTime toDay,
            string department,
            bool visit
        )
    {
        Sure.NotNull (readers);

        var fromDayString = IrbisDate.ConvertDateToString (fromDay);
        var toDayString = IrbisDate.ConvertDateToString (toDay);

        var result = readers
            .SelectMany (r => r.Visits ?? Array.Empty<VisitInfo>())
            .Count (v =>
                string.CompareOrdinal (v.DateGivenString, fromDayString) >= 0
                && string.CompareOrdinal (v.DateGivenString, toDayString) <= 0
                && string.Compare (v.Department, department, StringComparison.OrdinalIgnoreCase) == 0
                && v.IsVisit == visit);

        return result;
    }

    /// <summary>
    /// Отбор событий.
    /// </summary>
    public static VisitInfo[] GetEvents
        (
            this List<ReaderInfo> readers
        )
    {
        Sure.NotNull (readers);

        return readers
            .SelectMany (r => r.Visits ?? Array.Empty<VisitInfo>())
            .ToArray();
    }

    /// <summary>
    /// Отбор событий.
    /// </summary>
    public static VisitInfo[] GetEvents
        (
            this VisitInfo[] events,
            string department
        )
    {
        Sure.NotNull (events);

        // department может быть пустой, это нормально

        return events
            .AsParallel()
            .Where (v => v.Department.SameString (department))
            .ToArray();
    }

    /// <summary>
    /// Отбор событий.
    /// </summary>
    public static VisitInfo[] GetEvents
        (
            this VisitInfo[] events,
            bool visit
        )
    {
        Sure.NotNull (events);

        return events
            .AsParallel()
            .Where (v => v.IsVisit == visit)
            .ToArray();
    }

    /// <summary>
    /// Отбор событий.
    /// </summary>
    public static VisitInfo[] GetEvents
        (
            this VisitInfo[] events,
            DateTime day
        )
    {
        Sure.NotNull (events);

        var dayString = IrbisDate.ConvertDateToString (day);
        var result = events
            .AsParallel()
            .Where (v => v.DateGivenString.SameString (dayString))
            .ToArray();

        return result;
    }

    /// <summary>
    /// Отбор событий.
    /// </summary>
    public static VisitInfo[] GetEvents
        (
            this VisitInfo[] events,
            DateTime fromDay,
            DateTime toDay
        )
    {
        Sure.NotNull (events);

        var fromDayString = IrbisDate.ConvertDateToString (fromDay);
        var toDayString = IrbisDate.ConvertDateToString (toDay);
        var result = events
            .AsParallel()
            .Where (v =>
                string.CompareOrdinal (v.DateGivenString, fromDayString) >= 0
                && string.CompareOrdinal (v.DateGivenString, toDayString) <= 0)
            .ToArray();

        return result;
    }

    #endregion
}
