// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using Istu.OldModel;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Spectre.Console;

#endregion

namespace CountSciFond;

internal sealed class Program
{
    /// <summary>
    /// Конфигурация приложения.
    /// </summary>
    public static IConfiguration Configuration { get; private set; } = null!;

    /// <summary>
    /// Хост приложения.
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

        hostBuilder.ConfigureLogging (static logging =>
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

            using var connection = ConnectionFactory.Shared.CreateSyncConnection();
            var connectionString = Configuration["irbis-connection"].ThrowIfNullOrEmpty();
            connection.ParseConnectionString (connectionString);
            if (!connection.Connect())
            {
                AnsiConsole.MarkupLine ("[red]Невозможно подключиться[/]");
                return;
            }

            var storehouse = Storehouse.GetInstance
                (
                    ApplicationHost.Services,
                    Configuration
                );
            var specialReaders = new HashSet<string>();
            var podsobFonds = new HashSet<string>();
            var maxMfn = connection.GetMaxMfn();
            var irbisExepmlars = new HashSet<string>();

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
                .Start (/* capturing */ context =>
                {
                    var batch = BatchRecordReader.WholeDatabase
                        (
                            connection,
                            database: connection.EnsureDatabase(),
                            batchSize: 1_000
                        );
                    var task = context.AddTask ($"Чтение {maxMfn} записей");
                    task.MaxValue = maxMfn;
                    foreach (var record in batch)
                    {
                        task.Value = record.Mfn;

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
            AnsiConsole.WriteLine ("Книги загружены");

            var translatorCount = 0;
            AnsiConsole.Status()
                .Start ("Загрузка транслятора", /* capturing */ _ =>
                {
                    var found = storehouse.GetKladovka().GetTranslator()
                        .Select (static it => it.Inventory.ToInvariantString()).ToArray();
                    foreach (var one in found)
                    {
                        if (!irbisExepmlars.Contains (one))
                        {
                            irbisExepmlars.Add (one);
                            translatorCount++;
                        }
                    }

                    AnsiConsole.WriteLine ("Транслятор обработан");
                });

            AnsiConsole.Status()
                .Start ("Загрузка читателей", /* capturing */ _ =>
                {
                    var found = storehouse.GetKladovka().GetReaders()
                        .Where (static it => it.Category == "служебная запись")
                        .Select (static it => it.Ticket)
                        .ToArray()
                        .Where (static it => !string.IsNullOrEmpty (it))
                        .ToArray();
                    foreach (var one in found)
                    {
                        specialReaders.Add (one!);
                    }

                    var readerList = string.Join (", ", specialReaders);
                    AnsiConsole.WriteLine ($"Спецчитатели: {readerList}");
                });

            AnsiConsole.Status()
                .Start ("Загрузка выдач", /* capturing */ _ =>
                {
                    var found = storehouse.GetKladovka().GetPodsob()
                        .ToArray()
                        .Where (/* capturing */ it => !string.IsNullOrEmpty (it.Ticket)
                                      && specialReaders.Contains (it.Ticket))
                        .Select (static it => it.Inventory.ToInvariantString())
                        .ToArray();
                    foreach (var one in found)
                    {
                        podsobFonds.Add (one);
                    }
                    AnsiConsole.WriteLine ("Выдачи загружены");
                });

            var sifted = 0; // отсеянные экземпляры
            var screenedOut = 0; // экземпляры, успешно прошедшие отсеивание
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
                .Start (/* capturing */ context =>
                {
                    var task = context.AddTask ("Отсев");
                    task.MaxValue = irbisExepmlars.Count;
                    var exemplars = irbisExepmlars.ToArray();
                    for (var i = 0; i < exemplars.Length; i++)
                    {
                        task.Value = i;
                        var exemplar = exemplars[i];
                        if (!podsobFonds.Contains (exemplar))
                        {
                            screenedOut++;
                        }
                        else
                        {
                            sifted++;
                        }
                    }

                    task.Value = task.MaxValue;
                    AnsiConsole.WriteLine ("Отсев завершен");
                });

            var table = new Table();
            table.AddColumn ("[blue]Показатель[/]");
            table.AddColumn (new TableColumn ("[blue]Количество[/]").RightAligned());
            table.AddRow ("В базе данных", $"[yellow]{irbisExepmlars.Count}[/]");
            table.AddRow ("Из транслятора", $"[yellow]{translatorCount}[/]");
            table.AddRow ("Спецчитателей", $"[yellow]{specialReaders.Count}[/]");
            table.AddRow ("В подсобных фондах", $"[yellow]{podsobFonds.Count}[/]");
            table.AddRow ("Отсеянных", $"[yellow]{sifted}[/]");
            table.AddRow ("Прошедших отсеивание", $"[yellow]{screenedOut}[/]");

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
