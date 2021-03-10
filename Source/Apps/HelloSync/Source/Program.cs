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

using ManagedIrbis;

using static System.Console;

#endregion

#nullable enable
class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    private static int Main(string[] args)
    {
        try
        {
            using var connection = ConnectionFactory.Shared
                .CreateConnection();

            connection.Host = args.Length == 0
                ? "127.0.0.1"
                : args[0];
            connection.Username = "librarian";
            connection.Password = "secret";

            var success = connection.Connect();
            if (!success)
            {
                Error.WriteLine("Can't connect");
                return 1;
            }

            WriteLine("Successfully connected");

            //var version = connection.GetServerVersion();
            //WriteLine(version);

            //var processes = connection.ListProcesses();
            //WriteLine("Processes: "
            //          + string.Join<ProcessInfo>(" | ", processes));

            var maxMfn = connection.GetMaxMfn();
            WriteLine($"Max MFN={maxMfn}");

            connection.Nop();
            WriteLine("NOP");

            var found = connection.Search(Search.Keyword("бетон$"));
            WriteLine("Found: " + string.Join(", ", found));

            var terms = connection.ReadTerms("K=БЕТОН", 10);
            WriteLine("Terms: " + string.Join<Term>(", ", terms));

            //if (terms.Length != 0)
            //{
            //    var postings = connection.ReadPostings(terms[0].Text!, 10);
            //    WriteLine("Postings: " + string.Join<TermPosting>(", ", postings));
            //}

            var record = connection.ReadRecord(1);
            WriteLine($"ReadRecord={record?.FM(200, 'a')}");

            var formatted = connection.FormatRecord("@brief", 1);
            WriteLine($"Formatted={formatted}");

            //var files = connection.ListFiles("2.IBIS.*.mnu");
            //WriteLine("Files: " + string.Join(",", files));

            var fileText = connection.ReadTextFile("2.IBIS.brief.pft");
            WriteLine($"BRIEF: {fileText}");
            WriteLine();

            connection.Dispose();
            WriteLine("Successfully disconnected");
        }
        catch (Exception exception)
        {
            WriteLine(exception);
            return 1;
        }

        return 0;
    } // method Main
} // class Program
