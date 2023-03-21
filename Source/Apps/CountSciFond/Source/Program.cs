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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

using AM;

using Istu.OldModel;
using Istu.OldModel.Implementation;

using LinqToDB.Data;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Records;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Spectre.Console;

using CM = System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace CountSciFond;

internal sealed class Program
{
    /// <summary>
    /// Конфигурация приложения.
    /// </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    /// <summary>
    /// Хост приложения
    /// </summary>
    public static IHost ApplicationHost { get; private set; } = null!;

    private static void Initialize
        (
            string[] args
        )
    {
        var hostBuilder = Host.CreateDefaultBuilder (args);
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", true, true)
            .AddEnvironmentVariables()
            .AddCommandLine (args);

        Configuration = configurationBuilder
            .Build();

        hostBuilder.ConfigureLogging (logging =>
        {
            logging.ClearProviders();
        });

        ApplicationHost = hostBuilder.Build();
        AnsiConsole.Write (new Rule ("Инициализация завершена")
        {
            Border = BoxBorder.Double,
            Justification = Justify.Left
        });
    }

    public static void Main (string[] args)
    {
        try
        {
            Initialize (args);

            using var connection = new SyncConnection
            {
                Host = "172.20.1.186",
                Port = 6666,
                Username = "miron",
                Password = "miron",
                Database = "ISTU",
                Workstation = Workstation.Cataloger.ToString()
            };
            if (!connection.Connect())
            {
                AnsiConsole.MarkupLine ("[red]Невозможно подключиться[/]");
                return;
            }

            var configuration = RecordConfiguration.GetDefault();
            var storehouse = Storehouse.GetInstance
                (
                    ApplicationHost.Services,
                    Configuration
                );
            var specialReaders = new HashSet<string>();
            var podsobFonds = new HashSet<string>();
            var maxMfn = 10_000; // connection.GetMaxMfn();
            var irbisExepmlars = new List<string>();

            AnsiConsole.Progress()
                .AutoClear (false)
                .HideCompleted (false)
                .Columns
                    (
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new RemainingTimeColumn { Style = new Style (Color.Yellow) }
                    )
                .Start (context =>
                {
                    //var batch = BatchRecordReader.WholeDatabase (connection, connection.EnsureDatabase(), 1_000);
                    var batch = BatchRecordReader.Interval (connection,
                        database: connection.EnsureDatabase(),
                        lastMfn: maxMfn,
                        batchSize:1_000);
                    var task = context.AddTask ($"Чтение {maxMfn} записей");
                    task.MaxValue = maxMfn;
                    foreach (var record in batch)
                    {
                        task.Value = record.Mfn;

                        var worklist = configuration.GetWorksheet (record);
                        if (!IrbisUtility.IsBook (worklist))
                        {
                            continue;
                        }

                        var exemplars = ExemplarInfo.ParseRecord (record);
                        foreach (var exemplar in exemplars)
                        {
                            var status = exemplar.Status.FirstChar();
                            if (status is not ('0' or '1' or '5' or '9'))
                            {
                                continue;
                            }

                            var number = exemplar.Number;
                            if (!string.IsNullOrEmpty (number))
                            {
                                irbisExepmlars.Add (number);
                            }
                        }
                    }
                });
            AnsiConsole.WriteLine ("Экземпляры загружены");

            AnsiConsole.Status()
                .Start ("Загрузка читателей", context =>
                {
                    var found = storehouse.GetKladovka().GetReaders().Where (it => it.Category == "служебная запись");
                    var filtered = found.Select (it => it.Ticket).Where (it => !string.IsNullOrEmpty (it));
                    foreach (var one in filtered)
                    {
                        specialReaders.Add (one!);
                    }

                    var readerList = string.Join (", ", specialReaders);
                    AnsiConsole.WriteLine ($"Спецчитатели: {readerList}");
                });
            AnsiConsole.Status()
                .Start ("Загрузка выдач", context =>
                {
                    var found = storehouse.GetKladovka().GetPodsob().ToArray();
                    var selected = found
                        .Where (it => !string.IsNullOrEmpty (it.Ticket) && specialReaders.Contains (it.Ticket))
                        .Select (it => it.Inventory.ToInvariantString());
                    foreach (var one in selected)
                    {
                        podsobFonds.Add (one);
                    }
                    AnsiConsole.WriteLine ("Выдачи загружены");
                });

            var counter = 0;
            AnsiConsole.Progress()
                .AutoClear (false)
                .HideCompleted (false)
                .Columns
                    (
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new RemainingTimeColumn { Style = new Style (Color.Yellow) }
                    )
                .Start (context =>
                {
                    var task = context.AddTask ("Отсеивание");
                    task.MaxValue = irbisExepmlars.Count;
                    for (var i = 0; i < irbisExepmlars.Count; i++)
                    {
                        task.Value = i;
                        var exemplar = irbisExepmlars[i];
                        if (!podsobFonds.Contains (exemplar))
                        {
                            counter++;
                        }
                    }

                    AnsiConsole.WriteLine ("Экземпляры отсеяны");
                });

            var table = new Table();
            table.AddColumn ("[blue]Показатель[/]");
            table.AddColumn (new TableColumn ("[blue]Количество[/]").RightAligned());
            table.AddRow ("В базе данных", $"[yellow]{irbisExepmlars.Count}[/]");
            table.AddRow ("Спецчитателей", $"[yellow]{specialReaders.Count}[/]");
            table.AddRow ("В подсобных фондах", $"[yellow]{podsobFonds.Count}[/]");

            AnsiConsole.WriteLine();
            AnsiConsole.Write (table);
            AnsiConsole.WriteLine();

            AnsiConsole.Write (new Rule ("Спасибо за внимание!")
            {
                Border = BoxBorder.Double,
                Justification = Justify.Left
            });
        }
        catch (Exception exception)
        {
            AnsiConsole.WriteException (exception, ExceptionFormats.ShortenEverything);
        }
    }
}
