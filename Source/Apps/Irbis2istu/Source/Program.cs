// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- основная логика утилиты
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.Linq;

using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider.SqlServer;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace Irbis2istu
{
    /// <summary>
    /// Основная логика утилиты.
    /// </summary>
    static class Program
    {
        private static DataConnection? _database; // подключение к базе ИРНИТУ
        private static Stopwatch? _stopwatch; // для подсчета, сколько времени заняла операция
        private static StreamReader? _reader; // чтение текстового файла с записями
        private static CaseInsensitiveDictionary<bool>? _indexes; // индексы для отслеживания
        private static string? _sourceFile;
        private static string? _sqlConnectionString;
        private static PftProgram? _titleProgram;
        private static PftProgram? _headingProgram;
        private static PftProgram? _authorsProgram;
        private static PftProgram? _exemplarsProgram;
        private static PftProgram? _linkProgram;
        private static PftProgram? _typeProgram;
        private static PftProgram? _briefProgram;

        /// <summary>
        /// Ограничение строки до указанной длины.
        /// </summary>
        private static string Limit(this string text, int length) => text.Length < length
            ? text
            : text.Substring(0, length);

        private static PftFormatter _GetFormatter
            (
                PftProgram program
            )
        {
            var result = new PftFormatter
            {
                Program = program
            };
            var provider = (DirectProvider) result.Context.Provider;
            provider.FallForwardPath = ".";

            return result;
        }
        private static string? _GetPlace
            (
                Record record
            )
        {
            var places = record.FMA(2003)
                .Where(s => s.SameString("ГРТ")
                            || s.SameString("МСК")
                            || s.SameString("УСО")
                            || s.SameString("УХТТ"))
                .ToArray();

            string? result = null;

            if (places.Length != 0)
            {
                Array.Sort(places);
                result = string.Join(", ", places);
            }

            return result;
        }

        private static string _GetTitle
            (
                Record record
            )
        {
            if (ReferenceEquals(_titleProgram, null))
            {
                var source = File.ReadAllText("title.pft", IrbisEncoding.Ansi);
                _titleProgram = PftUtility.CompileProgram(source);
            }

            var formatter = _GetFormatter(_titleProgram);
            var result = formatter.FormatRecord(record);


            return result.Limit(250);
        }

        private static string _GetDescription
            (
                Record record
            )
        {
            if (ReferenceEquals(_briefProgram, null))
            {
                var source = File.ReadAllText("sbrief_istu.pft", IrbisEncoding.Ansi);
                _briefProgram = PftUtility.CompileProgram(source);
            }

            var formatter = _GetFormatter(_briefProgram);
            var result = formatter.FormatRecord(record);

            return result.Limit(500);
        }

        private static string? _GetHeading
            (
                Record record
            )
        {
            if (ReferenceEquals(_headingProgram, null))
            {
                var source = File.ReadAllText("heading.pft", IrbisEncoding.Ansi);
                _headingProgram = PftUtility.CompileProgram(source);
            }

            var formatter = _GetFormatter(_headingProgram);
            var result = formatter.FormatRecord(record).Limit(128);

            return result.EmptyToNull();
        }

        private static string? _GetAuthors
            (
                Record record
            )
        {
            if (ReferenceEquals(_authorsProgram, null))
            {
                var source = File.ReadAllText("authors.pft", IrbisEncoding.Ansi);
                _authorsProgram = PftUtility.CompileProgram(source);
            }

            var formatter = _GetFormatter(_authorsProgram);
            var merged = formatter.FormatRecord(record);
            string[] lines = merged.SplitLines().NonEmptyLines().ToArray();
            var result = string.Join("; ", lines).Limit(200);

            return result.EmptyToNull();
        }

        private static int _GetYear
            (
                Record record
            )
        {
            var result = record.FM (210, 'd');
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM (461, 'h');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM (461, 'z');
            }
            if (string.IsNullOrEmpty(result))
            {
                result = record.FM (934);
            }
            if (string.IsNullOrEmpty (result))
            {
                return 0;
            }

            var match = Regex.Match(result, @"\d{4}");
            if (match.Success)
            {
                result = match.Value;
            }

            return result.SafeToInt32();
        }

        private static int _GetExemplars
            (
                Record record
            )
        {
            if (ReferenceEquals(_exemplarsProgram, null))
            {
                var source = File.ReadAllText("exemplars.pft", IrbisEncoding.Ansi);
                _exemplarsProgram = PftUtility.CompileProgram(source);
            }

            var formatter = _GetFormatter(_exemplarsProgram);
            var result = formatter.FormatRecord(record);

            return result.SafeToInt32();
        }

        private static string? _GetLink
            (
                Record record
            )
        {
            if (ReferenceEquals(_linkProgram, null))
            {
                var source = File.ReadAllText("link.pft", IrbisEncoding.Ansi);
                _linkProgram = PftUtility.CompileProgram(source);
            }

            var formatter = _GetFormatter(_linkProgram);
            var result = formatter.FormatRecord(record).Limit(200);

            return result.EmptyToNull();
        }

        private static string _GetType
            (
                Record record
            )
        {
            if (ReferenceEquals(_typeProgram, null))
            {
                var source = File.ReadAllText("type.pft", IrbisEncoding.Ansi);
                _typeProgram = PftUtility.CompileProgram(source);
            }

            var formatter = _GetFormatter(_typeProgram);
            var formatted = formatter.FormatRecord(record);
            var result = formatted.Contains("1")
                ? "электронный"
                : "традиционный";

            return result;
        }

        public static Record? ReadRecord
            (
                int mfn
            )
        {
            const string recordSeparator = "*****";

            var result = new Record
            {
                Mfn = mfn,
                Database = "ISTU"
            };
            while (true)
            {
                var line = _reader!.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }

                if (line == recordSeparator)
                {
                    break;
                }

                if (line[0] != '#')
                {
                    return null;
                }

                var pos = line.IndexOf(':') + 1;
                if (pos <= 1 || line[pos] != ' ')
                {
                    return null;
                }

                var tag = FastNumber.ParseInt32(line, 1, pos - 2);
                var field = new Field(tag);
                result.Fields.Add(field);
                int start = ++pos, length = line.Length;
                while (pos < length)
                {
                    if (line[pos] == '^')
                    {
                        break;
                    }
                    pos++;
                }

                if (pos != start)
                {
                    field.Value = line.Substring(start, pos - start);
                    start = pos;
                }

                while (start < length - 1)
                {
                    var code = line[++start];
                    pos = ++start;
                    while (pos < length)
                    {
                        if (line[pos] == '^')
                        {
                            break;
                        }
                        pos++;
                    }

                    var sub = new SubField
                        (
                            code,
                            line.Substring(start, pos - start)
                        );
                    field.Subfields.Add(sub);
                    start = pos;
                }
            }

            //result.Modified = false;
            return result;
        }

        static void ProcessRecord
            (
                Record record
            )
        {
            var worklist = record.FM(920);
            if (string.IsNullOrEmpty(worklist))
            {
                return;
            }

            worklist = worklist.ToUpperInvariant();
            if (worklist != "PAZK" && worklist != "SPEC" && worklist != "PVK")
            {
                return;
            }

            var index = record.FM(903);
            if (string.IsNullOrEmpty(index))
            {
                return;
            }

            if (_indexes!.ContainsKey(index))
            {
                Console.WriteLine("Repeating index: {0}", index);
                return;
            }

            _indexes.Add(index, false);

            var description = _GetDescription(record);

            if (string.IsNullOrEmpty(description))
            {
                return;
            }

            description = description.Limit(500);

            var data = new IrbisData
            {
                Index = index.Limit(32),
                Description = description,
                Heading = _GetHeading(record),
                Title = _GetTitle(record),
                Author = _GetAuthors(record),
                Count = _GetExemplars(record),
                Year = _GetYear(record),
                Link = _GetLink(record),
                Type = _GetType(record),
                Place = _GetPlace(record)
            };

            _database!.Insert(data);
            Console.WriteLine("[{0}] {1}", record.Mfn, data.Description);

        } // method ProcessRecord

        static void Main
            (
                string[] args
            )
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need 2 arguments");
                return;
            }

            _indexes = new CaseInsensitiveDictionary<bool>();
            _sourceFile = args[0];
            _sqlConnectionString = args[1];

            try
            {
                _stopwatch = new Stopwatch();
                _stopwatch.Start();

                using (_reader = new StreamReader(_sourceFile))
                using (_database = SqlServerTools.CreateDataConnection(_sqlConnectionString))
                {
                    Console.WriteLine
                    (
                        "Started at: {0}",
                        DateTime.Now.ToLongUniformString()
                    );

                    _database
                        .SetCommand("delete from [dbo].[irbisdata]")
                        .Execute();
                    Console.WriteLine("table truncated");

                    var mfn = 1;
                    Record? record;
                    while ((record = ReadRecord(mfn)) != null)
                    {
                        try
                        {
                            ProcessRecord(record);
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine("[{0}] {1}", mfn, exception.Message);
                        }

                        mfn++;
                    }

                    _database
                        .SetCommand("EXECUTE [upload_done]")
                        .Execute();
                    Console.WriteLine("[upload_done]");

                    _database
                        .SetCommand("insert into [FlagTable] default values")
                        .Execute();
                    Console.WriteLine("[FlagTable]");
                }

                _stopwatch.Stop();
                var elapsed = _stopwatch.Elapsed;
                Console.WriteLine
                    (
                        "Elapsed: {0}",
                        elapsed.ToAutoString()
                    );

            } // try

            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        } // method Main

    } // class Program

} // namespace Irbis2istu
