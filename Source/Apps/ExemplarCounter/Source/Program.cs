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
using System.Globalization;
using System.Threading;

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Records;

using Spectre.Console;

using CM = System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace ExemplarCounter;

internal sealed class Program
{
    public static void Main (string[] args)
    {
        try
        {
            using var connection = new SyncConnection
            {
                Host = "127.0.0.1",
                Port = 6666,
                Username = "librarian",
                Password = "secret",
                Database = "IBIS",
                Workstation = Workstation.Cataloger.ToString()
            };
            if (!connection.Connect())
            {
                AnsiConsole.MarkupLine ("[red]Can't connect[/]");
                return;
            }

            // var configuration = RecordConfiguration.GetDefault();
            var totalExemplars = 0; // всего экземпляров
            var lessMillion = 0; // номера до 1.000.000
            var millionAndHalf = 0; // номера до 1.600.000
            var bigger = 0; // номера после 1.600.000
            var foreign = 0; // иностранщина
            var other = 0; // прочие (не иностранщина)
            var maxMfn = connection.GetMaxMfn();

            var batch = BatchRecordReader.WholeDatabase (connection, connection.EnsureDatabase(), 1_000);
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
                    var task = context.AddTask ($"Reading {maxMfn} records");
                    task.MaxValue = maxMfn;

                    foreach (var record in batch)
                    {
                        task.Value = record.Mfn;

                        // var worklist = configuration.GetWorksheet (record);
                        // if (!IrbisUtility.IsBook (worklist))
                        // {
                        //     continue;
                        // }

                        var exemplars = ExemplarInfo.ParseRecord (record);
                        foreach (var exemplar in exemplars)
                        {
                            var inventory = exemplar.Number;
                            var barcode = exemplar.Barcode;
                            var place = exemplar.Place;
                            if (string.IsNullOrEmpty (inventory)
                                || string.IsNullOrEmpty (barcode)
                                || !barcode.StartsWith ("E0")
                                || string.IsNullOrEmpty (place)
                                || !(place.SameString ("ФКХ") || place.SameString ("ФДХ")))
                            {
                                continue;
                            }

                            if (!int.TryParse (inventory, CultureInfo.InvariantCulture, out var number))
                            {
                                if (inventory[0].SameChar ('И'))
                                {
                                    foreign++;
                                }
                                else
                                {
                                    Interlocked.Increment (ref other);
                                }
                            }
                            else
                            {
                                if (number < 1_000_000)
                                {
                                    lessMillion++;
                                }
                                else if (number < 1_600_000)
                                {
                                    millionAndHalf++;
                                }
                                else
                                {
                                    bigger++;
                                }
                            }

                            totalExemplars++;
                        }
                    }
                });

            var table = new Table();
            table.AddColumn ("[blue]Категория[/]");
            table.AddColumn (new TableColumn ("[blue]Количество[/]").RightAligned());

            table.AddRow ("До 1 млн", $"[yellow]{lessMillion}[/]");
            table.AddRow ("1-1.6 млн", $"[yellow]{millionAndHalf}[/]");
            table.AddRow ("После 1.6 млн", $"[yellow]{bigger}[/]");
            table.AddRow ("Иностранщина", $"[yellow]{foreign}[/]");
            table.AddRow ("Прочих", $"[yellow]{other}[/]");
            table.AddEmptyRow();
            table.AddRow ("[bold]Итого[/]", $"[bold yellow]{totalExemplars}[/]");
            
            AnsiConsole.WriteLine();
            AnsiConsole.Write (table);
            AnsiConsole.WriteLine();
        }
        catch (Exception exception)
        {
            AnsiConsole.WriteException (exception, ExceptionFormats.ShortenEverything);
        }
    }
}
