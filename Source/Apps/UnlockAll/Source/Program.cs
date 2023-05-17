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
using System.Diagnostics;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Spectre.Console;
using Spectre.Console.Rendering;

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

        hostBuilder.ConfigureLogging (logging => { logging.ClearProviders(); });

        ApplicationHost = hostBuilder.Build();
        AnsiConsole.Write (new Rule ("Инициализация завершена")
        {
            Border = BoxBorder.Double,
            Justification = Justify.Left
        });
        AnsiConsole.WriteLine();
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

            var databases = Configuration["databases"]
                .ThrowIfNullOrEmpty()
                .Split
                    (
                        CommonSeparators.CommaAndSemicolon,
                        StringSplitOptions.TrimEntries
                        | StringSplitOptions.RemoveEmptyEntries
                    )
                .ThrowIfNullOrEmpty();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var table = new Table();
            table.AddColumn ("[blue]База[/]");
            table.AddColumn ("[blue]В целом[/]");
            table.AddColumn ("[blue]Записи[/]");
            AnsiConsole.Live (table).Start (ctx =>
            {
                foreach (var database in databases)
                {
                    var columns = new IRenderable[]
                    {
                        new Markup (database),
                        new Markup ("[green]OK[/]"),
                        new Markup ("[green]OK[/]")
                    };
                    table.AddRow (columns);
                    ctx.Refresh();

                    var info = connection.GetDatabaseInfo (database);
                    if (info is null)
                    {
                        columns[1] = new Markup ("[red]Ошибка[/]");
                        columns[2] = new Markup ("[red]Ошибка[/]");
                    }
                    else
                    {
                        if (info.DatabaseLocked)
                        {
                            connection.UnlockDatabase (database);
                            columns[1] = new Markup ("[green]Снята блокировка[/]");
                        }

                        var lockedRecords = info.LockedRecords;
                        if (!lockedRecords.IsNullOrEmpty())
                        {
                            var mfnList = string.Join (", ", lockedRecords);
                            connection.UnlockRecords (lockedRecords);
                            columns[2] = new Markup ($"[green]{mfnList}[/]");
                        }
                    }

                    ctx.Refresh();
                }
            });

            stopwatch.Stop();
            var elapsed = stopwatch.Elapsed;
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine ($"Затрачено времени: {elapsed.ToAutoString()}");

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
