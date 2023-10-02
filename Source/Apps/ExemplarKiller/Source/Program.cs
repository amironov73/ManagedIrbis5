// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Configuration;
using AM.Linq;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using CM = System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace ExemplarKiller;

internal static class Program
{
    /// <summary>
    /// Префикс для поиска по инвентарному номеру.
    /// По умолчанию "IN="
    /// </summary>
    private static string? prefix;

    /// <summary>
    /// Формат для краткого библиографического описания.
    /// По умолчанию "@brief".
    /// </summary>
    private static string? format;

    /// <summary>
    /// Имя базы данных, содержащей сведения о читателях.
    /// По умолчанию "RDR".
    /// </summary>
    private static string? readers;

    /// <summary>
    /// Базы данных, в которых должен происходить поиск экземпляров.
    /// Базы задаются через точку с запятой, запятую или пробел.
    /// Должна быть указана хотя бы одна база!
    /// </summary>
    private static string[]? databases;

    /// <summary>
    ///
    /// </summary>
    private static bool doDelete;

    private static string? actNumber;
    private static SyncConnection? connection;

    /// <summary>
    /// Возврат экземпляра, если он числится на руках у читателя.
    /// </summary>
    private static void ReturnExemplar
        (
            Record record,
            string number,
            string? barcode
        )
    {
        var modified = false;

        Console.Write ($" <ticket {record.FM (30)}>");

        var fields = record.Fields
            .GetField (40);
        foreach (var field in fields)
        {
            var ok = field.GetFirstSubFieldValue ('b')
                .SameString (number);
            if (!ok && !string.IsNullOrEmpty (barcode))
            {
                ok = field.GetFirstSubFieldValue ('h')
                    .SameString (barcode);
            }

            if (ok)
            {
                field.SetSubFieldValue ('f', IrbisDate.TodayText);
                modified = true;
            }
        }

        if (modified)
        {
            Console.Write (" <returned>");
            connection!.WriteRecord (record);
        }
    }

    /// <summary>
    /// Удаление указанного экземпляра.
    /// </summary>
    private static void DeleteExemplar
        (
            string database,
            int mfn,
            string number
        )
    {
        var parameters = new ReadRecordParameters
        {
            Database = database,
            Mfn = mfn,
            Lock = false,
            Format = format
        };
        var bookRecord = connection!.ReadRecord (parameters);
        if (bookRecord is null)
        {
            return;
        }

        Console.Write
            (
                " [{0}]",
                bookRecord.Description
            );

        var found = bookRecord.Fields
            .GetField (910)
            .GetField ('b', number)
            .FirstOrDefault();

        if (found is null)
        {
            Console.Write (" <missing>");
        }
        else
        {
            connection = connection.ThrowIfNull();

            found.SetSubFieldValue ('a', "6");
            if (!string.IsNullOrEmpty (actNumber))
            {
                found.SetSubFieldValue ('v', actNumber);
            }

            Console.Write (" <written off>");
            connection.WriteRecord (bookRecord);

            var barcode = found.GetFirstSubFieldValue ('h');
            var searcher = new BatchSearcher
                (
                    connection,
                    readers!,
                    "H="
                );
            var readerRecords = searcher.SearchRead
                (
                    new[] { number, barcode }.NonEmptyLines()
                );
            foreach (var readerRecord in readerRecords)
            {
                ReturnExemplar
                    (
                        readerRecord,
                        number,
                        barcode
                    );
            }
        }
    }

    /// <summary>
    /// Отработка одного заданного экземпляра.
    /// </summary>
    private static void ProcessExemplar
        (
            int index,
            string number,
            string? description
        )
    {
        if (string.IsNullOrEmpty (number))
        {
            return;
        }

        Console.Write ($"{index}: {number} [{description}]");

        foreach (var database in databases!)
        {
            connection!.Database = database;
            var found = connection.Search ($"\"{prefix}{number}\"");
            if (found.Length == 0)
            {
                Console.Write ($" {database} [not found]");
            }
            else if (found.Length > 1)
            {
                Console.Write ($" {database} [too many!]");
            }
            else
            {
                var mfn = found[0];
                Console.Write ($" {database} [MFN {mfn}]");
                if (doDelete)
                {
                    try
                    {
                        DeleteExemplar
                            (
                                database,
                                mfn,
                                number
                            );
                    }
                    catch (Exception exception)
                    {
                        Console.Write ($" <exception>: {exception.Message}");
                    }
                }
            }
        }

        Console.WriteLine (" done");
    }

    /*

    private static bool ExceptionResolver
        (
            Exception exception
        )
    {
        Console.WriteLine
            (
                " {0}",
                exception.Message
            );

        return true;
    }

    */

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static int Main
        (
            string[] args
        )
    {
        if (args.Length is < 1 or > 3)
        {
            Console.WriteLine
                (
                    "Usage: ExemplarKiller <bookList> [connectionString] [actNumber]"
                );

            return 1;
        }

        var fileName = args[0];
        doDelete = ConfigurationUtility.GetBoolean ("delete");
        databases = ConfigurationUtility.GetString ("databases")
            .ThrowIfNull()
            .Split
                (
                    new[] { ';', ',', ' ' },
                    StringSplitOptions.RemoveEmptyEntries
                );
        if (databases.Length == 0)
        {
            Console.Error.WriteLine ("Empty database list");
            return 1;
        }

        if (args.Length > 2)
        {
            actNumber = args[2];
        }

        prefix = ConfigurationUtility.GetString
            (
                "prefix",
                "IN="
            )!;
        format = ConfigurationUtility.GetString
            (
                "format",
                "@brief"
            )!;
        readers = ConfigurationUtility.GetString
            (
                "readers",
                "RDR"
            )!;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            var connectionString = args.Length > 1
                ? args[1]
                : CM.AppSettings["connectionString"]
                    .ThrowIfNullOrEmpty();

            var numbers = File.ReadAllLines
                (
                    fileName,
                    Encoding.UTF8
                );
            Console.WriteLine ($"Numbers loaded: {numbers.Length}");

            using (connection = ConnectionFactory.Shared.CreateSyncConnection())
            {
                // TODO: восстановить
                // connection.SetRetry(10, ExceptionResolver);

                connection.ParseConnectionString (connectionString);
                connection.Connect();

                Console.WriteLine ("Connected");

                for (var i = 0; i < numbers.Length; i++)
                {
                    var line = numbers[i];
                    var parts = line.Split
                        (
                            new[] { ';', ' ', '\t' },
                            2,
                            StringSplitOptions.RemoveEmptyEntries
                        );
                    var number = parts[0].Trim();
                    string? description = null;
                    if (parts.Length != 1)
                    {
                        description = parts[1].Trim();
                    }

                    ProcessExemplar
                        (
                            i,
                            number,
                            description
                        );
                }
            }

            Console.WriteLine ("Disconnected");

            stopwatch.Stop();
            Console.WriteLine
                (
                    "Time elapsed: {0}",
                    stopwatch.Elapsed.ToAutoString()
                );
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
