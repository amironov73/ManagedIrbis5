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

using System;
using System.IO;
using System.Linq;

using ManagedIrbis.Direct;
using ManagedIrbis.Pft.Infrastructure.Testing;

#nullable enable

namespace PftTests
{
    static class Program
    {
        private static string? _testsRoot;

        private static string? FindTestsFolder
            (
                string startFrom
            )
        {
            var candidate = startFrom;

            while (true)
            {
                var guess = Path.Combine(candidate, "Tests");
                if (Directory.Exists(guess))
                {
                    return candidate;
                }

                var parent = Path.GetDirectoryName(candidate);
                if (string.IsNullOrEmpty(parent))
                {
                    return null;
                }

                candidate = parent;
            }
        }

        private static void DiscoverAndRunTests()
        {
            var provider = new DirectProvider(_testsRoot!);

            var tester = new PftTester(_testsRoot!);
            tester.SetEnvironment(provider);

            tester.DiscoverTests();

            tester.RunTests();

            var fileName = DateTime.Now.ToString
                           (
                               "yyyy-MM-dd-hh-mm-ss"
                           )
                           + ".json";
            tester.WriteResults(fileName);

            var total = tester.Results.Count;
            int failed = tester.Results.Count(t => t.Failed);

            ConsoleColor foreColor;
            //foreach (PftTestResult result in tester.Results)
            //{
            //    foreColor = Console.ForegroundColor;
            //    Console.ForegroundColor = ConsoleColor.White;
            //    Console.Write(result.Name);
            //    Console.Write('\t');
            //    Console.ForegroundColor = result.Failed
            //        ? ConsoleColor.Red
            //        : ConsoleColor.Green;
            //    Console.Write(result.Failed ? "FAILED" : "OK");
            //    Console.Write('\t');
            //    Console.ForegroundColor = ConsoleColor.Cyan;
            //    Console.Write
            //        (
            //            "{0:0},{1:000}",
            //            result.Duration.TotalSeconds,
            //            result.Duration.Milliseconds
            //        );
            //    Console.ForegroundColor = foreColor;
            //    Console.Write('\t');
            //    Console.WriteLine(result.Description);
            //}

            //Console.WriteLine();
            foreColor = Console.ForegroundColor;
            Console.ForegroundColor = failed == 0
                ? ConsoleColor.Green
                : ConsoleColor.Red;
            Console.WriteLine
                (
                    "Total tests: {0}, failed: {1}",
                    total,
                    failed
                );
            if (failed != 0)
            {
                Console.Write
                    (
                        "Failed tests: {0}",
                        string.Join
                            (
                                ", ",
                                tester.Results
                                    .Where(t => t.Failed)
                                    .Select(t => t.Name)
                            )
                    );
                Console.WriteLine();
            }

            Console.ForegroundColor = foreColor;
        }

        static int Main
            (
                string[] args
            )
        {
            var startAt = AppContext.BaseDirectory;
            if (args.Length != 0)
            {
                startAt = args[0];
            }

            _testsRoot = FindTestsFolder(startAt);
            if (_testsRoot is null)
            {
                Console.WriteLine("Can't locate tests");
                return 1;
            }

            try
            {
                DiscoverAndRunTests();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return 1;
            }

            return 0;
        }
    }
}
