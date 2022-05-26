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
using System.IO;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using static System.Console;

#endregion

#nullable enable

internal static class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    private static int Main (string[] args)
    {
        try
        {
            using var connection = ConnectionFactory.Shared
                .CreateSyncConnection();

            connection.Host = args.Length == 0
                ? "127.0.0.1"
                : args[0];
            connection.Username = "librarian";
            connection.Password = "secret";

            var success = connection.Connect();
            if (!success)
            {
                Error.WriteLine ("Can't connect");
                return 1;
            }

            WriteLine ("Successfully connected");

            var version = connection.GetServerVersion();
            WriteLine (version);

            var processes = connection.ListProcesses();
            if (processes is not null)
            {
                WriteLine ("Processes: "
                           + string.Join<ProcessInfo> (" | ", processes));
            }

            var stat = connection.GetServerStat();
            if (stat is not null)
            {
                WriteLine (stat);
            }

            var databases = connection.ListDatabases();
            foreach (var database in databases)
            {
                WriteLine ($"{database.Name} => {database.Description}");
            }

            WriteLine();

            var maxMfn = connection.GetMaxMfn();
            WriteLine ($"Database={connection.Database}, max MFN={maxMfn}");

            var dbInfo = connection.GetDatabaseInfo();
            if (dbInfo is not null)
            {
                dbInfo.Write (Out);
            }

            connection.NoOperation();
            WriteLine ("NOP");

            var found = connection.Search (Search.Keyword ("бетон$"));
            WriteLine ("Found: " + string.Join (", ", found));

            var terms = connection.ReadTerms ("K=БЕТОН", 10);
            if (terms is not null)
            {
                WriteLine ("Terms: " + string.Join<Term> (", ", terms));
            }

            //if (terms.Length != 0)
            //{
            //    var postings = connection.ReadPostings(terms[0].Text!, 10);
            //    WriteLine("Postings: " + string.Join<TermPosting>(", ", postings));
            //}

            var record = connection.ReadRecord (1);
            WriteLine ($"ReadRecord={record?.FM (200, 'a')}");

            var rawRecord = connection.ReadRawRecord (1);
            WriteLine ($"ReadRawRecord={rawRecord?.FM (200)}");

            var formatted = connection.FormatRecord ("@brief", 1);
            WriteLine ($"Formatted={formatted}");

            var files = connection.ListFiles ("2.IBIS.*.mnu");
            if (files is not null)
            {
                WriteLine ("Files: " + string.Join (",", files));
            }

            var users = connection.ListUsers();
            if (users is not null)
            {
                WriteLine ("Users: " + string.Join<UserInfo> (", ", users));
            }

            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = connection.Database,
                FileName = "brief.pft"
            };
            var fileText = connection.ReadTextFile (specification);
            WriteLine ($"BRIEF: {fileText}");
            WriteLine();

            specification = new FileSpecification
            {
                Path = IrbisPath.System,
                FileName = "logo.gif"
            };
            var binary = connection.ReadBinaryFile (specification);
            if (binary is not null)
            {
                File.WriteAllBytes ("logo.gif", binary);
            }

#pragma warning disable 162

            // ReSharper disable HeuristicUnreachableCode

            if (false)
            {
                // если нам не жалко каталог, можем проверить,
                // как сохраняются записи

                var records = new List<Record> (10);
                for (var i = 1; i <= 10; i++)
                {
                    var newRecord = new Record
                    {
                        { 700, 'a', "Миронов", 'b', "А. В.", 'g', "Алексей Владимирович" },
                        { 200, 'a', $"Новая запись N {i}" },
                        { 920, "PAZK" },
                        { 300, $"Примечание к новой записи {i}" }
                    };

                    records.Add (newRecord);
                }

                connection.WriteRecords (records);
            }

            // ReSharper restore HeuristicUnreachableCode
#pragma warning restore 162

            connection.Dispose();
            WriteLine ("Successfully disconnected");
        }
        catch (Exception exception)
        {
            WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
