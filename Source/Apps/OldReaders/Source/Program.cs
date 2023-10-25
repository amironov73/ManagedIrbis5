// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- program entry point
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;
using AM.Linq;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Readers;

using Microsoft.Extensions.Configuration;

#endregion

// Наименование библиотеки
const string libraryName = "иогунб";
const string litres = "литре";

List<ReaderInfo> OldReaders = new ();

DateTime threshold;
DateTime lowerBound;

try
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    var connectionString = configuration["connectionString"]
        .ThrowIfNullOrEmpty ("connectionString not set");
    threshold = IrbisDate.ConvertStringToDate
        (
            configuration["threshold"].ThrowIfNullOrEmpty ("threshold not set")
        );
    lowerBound = DateTime.MinValue;
    var lowerText = configuration["lowerBound"];
    if (!string.IsNullOrEmpty (lowerText))
    {
        lowerBound = IrbisDate.ConvertStringToDate (lowerText);
    }

    Console.WriteLine ("Loading readers");
    var allReaders = new List<ReaderInfo>();
    using (var connection = ConnectionFactory.Shared.CreateSyncConnection())
    {
        connection.ParseConnectionString (connectionString);
        connection.Connect();
        if (!connection.IsConnected)
        {
            Console.Error.WriteLine ("Can't connect");
            Console.Error.WriteLine (IrbisException.GetErrorDescription (connection.LastError));
            return 1;
        }

        var manager = new ReaderManager (connection)
        {
            OmitDeletedRecords = true
        };
        manager.BatchRead += (_, _) => Console.Write (".");

        var databases = configuration["databases"]
            .ThrowIfNull ("databases not specified")
            .Split
                (
                    new[] { ' ', ';', ',' },
                    StringSplitOptions.RemoveEmptyEntries
                );

        foreach (var database in databases)
        {
            var maxMfn = connection.GetMaxMfn (database) - 1;
            Console.WriteLine ($"Database: {database}, records: {maxMfn}");

            allReaders.AddRange (manager.GetAllReaders (database));

            Console.WriteLine();
        }
    }

    WriteDelimiter();

    Console.WriteLine ("Merging");
    Console.WriteLine ($"Records before merging: {allReaders.Count}");

    allReaders = ReaderManager.MergeReaders (allReaders);

    Console.WriteLine ($"Records after merging: {allReaders.Count}");
    WriteDelimiter();

    Console.WriteLine ("Filtering");
    foreach (var reader in allReaders)
    {
        ProcessReader (reader);
    }
    var oldReaders = OldReaders.ToArray();

    WriteDelimiter();
    Console.WriteLine ("Sorting");

    oldReaders = oldReaders.OrderBy (reader => reader.FullName)
        .ToArray();

    WriteDelimiter();
    Console.WriteLine
        (
            "Create table: {0} lines",
            oldReaders.Length
        );

    Console.WriteLine("ФИО\tБилет\tРегистрация\tКол-во\tПоследнее событие\tОтделы");
    for (var i = 0; i < oldReaders.Length; i++)
    {
        var reader = oldReaders[i];
        var lastDate = (string?) reader.UserData;
        if (string.IsNullOrEmpty (lastDate))
        {
            lastDate = "--";
        }

        var departments = string.Join
            (
                ", ",
                reader.Registrations.ThrowIfNull ()
                    .Select (reg => reg.Chair)
                    .Concat
                        (
                            reader.Visits.ThrowIfNull()
                                .Select (visit => visit.Department)
                        )
                    .NonEmptyLines()
                    .Distinct()
            );

        var regDate = reader.RegistrationDate.ToShortDateString();
        var visitCount = reader.Visits.ThrowIfNull().Length + reader.Registrations.SafeLength();
        Console.WriteLine ($"{reader.FullName}\t{reader.Ticket}\t{regDate}\t{visitCount}\t{lastDate}\t{departments}");
    }
}
catch (Exception exception)
{
    Console.WriteLine (exception);
}

return 0;

void WriteDelimiter()
{
    Console.WriteLine();
    Console.WriteLine (new string ('#', 70));
    Console.WriteLine();
}

void ProcessReader
    (
        ReaderInfo reader
    )
{
    var ticket = reader.Ticket;
    if (string.IsNullOrEmpty (ticket))
    {
        return;
    }

    ticket = ticket.ToLower();
    if (ticket.Contains (litres))
    {
        return;
    }

    var workplace = reader.WorkPlace;
    if (!string.IsNullOrEmpty (workplace))
    {
        workplace = workplace.ToLower();
        if (workplace.Contains (libraryName))
        {
            return;
        }
    }

    var visits = reader.Visits;
    if (visits.IsNullOrEmpty())
    {
        return;
    }

    var debt = visits.GetLoans().GetDebt();
    if (debt.Length != 0)
    {
        return;
    }

    VisitInfo? lastEvent = null;
    if (visits.Length != 0)
    {
        var maxDate = visits[0].DateGivenString;
        lastEvent = visits[0];

        for (var i = 1; i < visits.Length; i++)
        {
            var date = visits[i].DateGivenString;
            if (string.CompareOrdinal (date, maxDate) > 0)
            {
                maxDate = date;
                lastEvent = visits[i];
            }
        }
    }

    if (lastEvent is null)
    {
        var lastRegistration = GetLastRegistration (reader.Registrations);

        if (lastRegistration is not null)
        {
            if (lastRegistration.Date >= threshold)
            {
                return;
            }

            reader.UserData = lastRegistration.Date
                .ToShortDateString() + " перерегистрация";
        }
        else
        {
            lastRegistration = GetLastRegistration (reader.Enrollment);

            if (lastRegistration is not null)
            {
                if (lastRegistration.Date >= threshold)
                {
                    return;
                }

                reader.UserData = lastRegistration.Date
                    .ToShortDateString() + " регистрация";
            }
            else
            {
                return;
            }
        }
    }
    else
    {
        var dateGiven = lastEvent.DateGiven;
        if (dateGiven < lowerBound || dateGiven >= threshold)
        {
            return;
        }

        reader.UserData = dateGiven.ToShortDateString() + " посещение";
    }

    OldReaders.Add (reader);
}

ReaderRegistration? GetLastRegistration
    (
        ReaderRegistration[]? registrations
    )
{
    ReaderRegistration? result = null;

    if (registrations is { Length: not 0 })
    {
        var maxDate = registrations[0].DateString;
        result = registrations[0];

        for (var i = 1; i < registrations.Length; i++)
        {
            var date = registrations[i].DateString;
            if (string.CompareOrdinal (date, maxDate) > 0)
            {
                maxDate = date;
                result = registrations[i];
            }
        }
    }

    return result;
}
