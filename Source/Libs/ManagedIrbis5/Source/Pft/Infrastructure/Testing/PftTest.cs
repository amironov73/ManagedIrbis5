// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;

using AM;
using AM.ConsoleIO;
using AM.Text;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Testing
{
    /// <summary>
    /// Single test for PFT formatting.
    /// </summary>
    public sealed class PftTest
    {
        #region Constants

        /// <summary>
        /// Если в папке с данными для теста находится
        /// файл с таким именем, тест вызовет отладчик
        /// перед началом выполнения.
        /// </summary>
        public const string DebugBreakFileName = "debug.break";

        /// <summary>
        /// Description file name.
        /// </summary>
        public const string DescriptionFileName = "description.txt";

        /// <summary>
        /// Expected result file name.
        /// </summary>
        public const string ExpectedFileName = "expected.txt";

        /// <summary>
        /// Если в папке с данными для теста находится
        /// файл с таким именем, тест пропускается.
        /// </summary>
        public const string IgnoreFileName = "test.ignore";

        /// <summary>
        /// Input file name.
        /// </summary>
        public const string InputFileName = "input.txt";

        /// <summary>
        /// Record file name.
        /// </summary>
        public const string RecordFileName = "record.txt";

        #endregion

        #region Properties

        /// <summary>
        /// Provider.
        /// </summary>
        public ISyncProvider? Provider { get; set; }

        /// <summary>
        /// Folder name.
        /// </summary>
        public string Folder { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTest
            (
                string folder
            )
        {
            Folder = Path.GetFullPath(folder);
        }

        #endregion

        #region Private members

        private string GetFullName
            (
                string shortName
            )
        {
            return Path.Combine
                (
                    Folder,
                    shortName
                );
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the directory contains test?
        /// </summary>
        public static bool IsDirectoryContainsTest
            (
                string directory
            )
        {
            var result =
                File.Exists(Path.Combine(directory, DescriptionFileName))
                && File.Exists(Path.Combine(directory, RecordFileName))
                && File.Exists(Path.Combine(directory, InputFileName));

            return result;
        }

        /// <summary>
        /// Run the test.
        /// </summary>
        public PftTestResult Run
            (
                string name
            )
        {
            var result = new PftTestResult
            {
                Name = name,
                StartTime = DateTime.Now
            };

            try
            {
                if (ReferenceEquals(Provider, null))
                {
                    throw new PftException("environment not set");
                }

                var descriptionFile = GetFullName(DescriptionFileName);
                if (File.Exists(descriptionFile))
                {
                    var description = File.ReadAllText
                        (
                            descriptionFile,
                            IrbisEncoding.Utf8
                        );
                    result.Description = description;
                    ConsoleInput.Write(description);
                }

                var ignoreFile = GetFullName(IgnoreFileName);
                if (File.Exists(ignoreFile))
                {
                    ConsoleInput.Write(" IGNORED");
                    goto DONE;
                }

                var recordFile = GetFullName(RecordFileName);

                if (ReferenceEquals(recordFile, null))
                {
                    throw new IrbisException
                        (
                            "GetFullName returns null"
                        );
                }

                var record = PlainText.ReadOneRecord
                    (
                        recordFile,
                        IrbisEncoding.Utf8
                    )
                    .ThrowIfNull("ReadOneRecord returns null");
                record.Mfn = 1; // TODO some other value?

                var pftFile = GetFullName(InputFileName);
                var input = File.ReadAllText
                    (
                        pftFile,
                        IrbisEncoding.Utf8
                    )
                    .DosToUnix()
                    .ThrowIfNull("input");
                result.Input = input;

                //ConsoleInput.WriteLine(input);
                //ConsoleInput.WriteLine();

                var lexer = new PftLexer();
                var tokenList = lexer.Tokenize(input);
                var writer = new StringWriter();
                tokenList.Dump(writer);
                result.Tokens = writer.ToString()
                    .DosToUnix()
                    .ThrowIfNull("tokens");

                //ConsoleInput.WriteLine(result.Tokens);
                //ConsoleInput.WriteLine();

                var parser = new PftParser(tokenList);
                var program = parser.Parse();

                //result.Ast = program.DumpToText().DosToUnix();
                //ConsoleInput.WriteLine(result.Ast);
                //ConsoleInput.WriteLine();

                var expectedFile = GetFullName(ExpectedFileName);
                string? expected = null;
                if (File.Exists(expectedFile))
                {
                    expected = File.ReadAllText
                        (
                            expectedFile,
                            IrbisEncoding.Utf8
                        )
                        .DosToUnix()
                        .ThrowIfNull("expected");
                    result.Expected = expected;
                }

                var provider = Provider.ThrowIfNull("Provider");

                string output;
                using (var formatter = new PftFormatter { Program = program })
                {
                    formatter.SetProvider(provider);

                    var breakFile = GetFullName(DebugBreakFileName);
                    if (File.Exists(breakFile))
                    {
                        Debugger.Break();
                    }

                    output = formatter.FormatRecord(record)
                        .DosToUnix()
                        .ThrowIfNull("output");
                }
                result.Output = output;

                //ConsoleInput.WriteLine(output);

                if (expected != null)
                {
                    if (output != expected)
                    {
                        result.Failed = true;

                        ConsoleInput.WriteLine();
                        ConsoleInput.WriteLine("!!! FAILED !!!");
                        ConsoleInput.WriteLine();
                        ConsoleInput.WriteLine("EXPECTED");
                        ConsoleInput.WriteLine(expected);
                        ConsoleInput.WriteLine();
                        ConsoleInput.WriteLine("ACTUAL");
                        ConsoleInput.WriteLine(output);
                        ConsoleInput.WriteLine();
                    }
                }
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "PftTest::Run",
                        exception
                    );

                result.Failed = true;
                result.Exception = exception.ToString();
            }

            DONE:

            result.FinishTime = DateTime.Now;
            result.Duration = result.FinishTime - result.StartTime;

            return result;
        }

        #endregion
    }
}
