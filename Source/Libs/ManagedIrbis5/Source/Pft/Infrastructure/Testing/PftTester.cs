// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftTester.cs -- автоматический тестировщик PFT-форматтера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;
using AM.Collections;
using AM.ConsoleIO;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Testing
{
    /// <summary>
    /// Автоматический тестировщик PFT-форматтера.
    /// </summary>
    public sealed class PftTester
    {
        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        public ISyncProvider Provider { get; private set; }

        /// <summary>
        /// Folder name.
        /// </summary>
        public string Folder { get; private set; }

        /// <summary>
        /// Tests.
        /// </summary>
        public NonNullCollection<PftTest> Tests { get; private set; }

        /// <summary>
        /// Results.
        /// </summary>
        public NonNullCollection<PftTestResult> Results { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTester
            (
                string folder
            )
        {
            Provider = new NullProvider();
            Folder = folder;
            Tests = new NonNullCollection<PftTest>();
            Results = new NonNullCollection<PftTestResult>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Discover tests.
        /// </summary>
        public void DiscoverTests()
        {
            var directories = Directory.GetDirectories
                (
                    Folder,
                    "*",
                    SearchOption.AllDirectories
                );

            foreach (var subDir in directories)
            {
                if (PftTest.IsDirectoryContainsTest(subDir))
                {
                    var test = new PftTest(subDir);
                    Tests.Add(test);
                }
            }
        }

        /// <summary>
        /// Run the test.
        /// </summary>
        public PftTestResult? RunTest
            (
                PftTest test
            )
        {
            PftTestResult? result = null;

            var name = Path.GetFileName(test.Folder);

            var foreColor = ConsoleInput.ForegroundColor;
            ConsoleInput.ForegroundColor = ConsoleColor.Cyan;

            ConsoleInput.Write(name + ": ");

            ConsoleInput.ForegroundColor = foreColor;

            try
            {
                result = test.Run(name);

                ConsoleInput.ForegroundColor = result.Failed
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;

                //ConsoleInput.WriteLine();
                ConsoleInput.Write(" ");
                ConsoleInput.WriteLine
                    (
                        result.Failed
                        ? "FAIL"
                        : "OK"
                    );

                ConsoleInput.ForegroundColor = foreColor;

                //ConsoleInput.WriteLine(new string('=', 70));
                //ConsoleInput.WriteLine();
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftTester::RunTest",
                        exception
                    );

                Debug.WriteLine(exception);

                ConsoleInput.WriteLine(exception.ToString());
            }

            //ConsoleInput.WriteLine();

            return result;
        }

        /// <summary>
        /// Run the tests.
        /// </summary>
        public void RunTests()
        {
            foreach (var test in Tests)
            {
                test.Provider = Provider;
                var result = RunTest(test);
                if (result is not null)
                {
                    Results.Add(result);
                }
            }
        }

        /// <summary>
        /// Set environment.
        /// </summary>
        public void SetEnvironment
            (
                ISyncProvider provider
            )
        {
            Provider = provider;
        }

        /// <summary>
        /// Write test results to the file.
        /// </summary>
        public void WriteResults
            (
                string fileName
            )
        {
            /*

            using (StreamWriter writer = new StreamWriter
                (
                    new FileStream
                        (
                            fileName,
                            FileMode.Create,
                            FileAccess.Write
                        )
                ))
            {
                JArray array = JArray.FromObject(Results);
                string text = array.ToString(Formatting.Indented);
                writer.Write(text);
            }

            */
        }

        #endregion
    }
}
