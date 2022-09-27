// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using Istu.OldModel;
using Istu.OldModel.Implementation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace IstuTests;

internal static class Program
{
    private static int Main
        (
            string[] args
        )
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json")
            .Build();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices (services =>
            {
                services.AddLogging (logging => { logging.AddConsole(); });
            })
            .Build();

        var storehouse = new Storehouse (host.Services, configuration);
        using var kladovka = storehouse.GetKladovka();
        var readers = kladovka.GetReaders();
        var readerCount = readers.Count();
        Console.WriteLine ($"Total readers = {readerCount}");
        var attendanceManager = new AttendanceManager (storehouse);
        var lastAttendance = attendanceManager.GetLastAttendance ("р-1");
        Console.WriteLine (lastAttendance);
        var lastReaders = attendanceManager.GetLastReaders();
        foreach (var reader in lastReaders)
        {
            Console.WriteLine ($"{reader.Ticket}: {reader.Name}");
        }

        var readerManager = new ReaderManager (storehouse);
        var oneReader = readerManager.GetReaderByTicket ("р-1");
        Console.WriteLine (oneReader);

        var foundReaders = readerManager.FindReaders
            (
                ReaderSearchCriteria.Ticket,
                "р-1",
                10
            );
        foreach (var foundReader in foundReaders)
        {
            Console.WriteLine (foundReader);
        }

        return 0;
    }
}
