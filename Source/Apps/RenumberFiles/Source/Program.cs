// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- утилита для перенумерации файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

using AM.Collections;
using AM.IO;
using AM.Text.Output;

#endregion

#nullable enable

namespace RenumberFiles
{
    class Program
    {
        static List<string> GatherFiles
            (
                IEnumerable<string> wildcards
            )
        {
            var result = new List<string>();
            foreach (var wildcard in wildcards)
            {
                if (Directory.Exists (wildcard))
                {
                    result.AddRange (Directory.GetFiles (wildcard, "*.*"));
                }
                else
                {
                    var found = DirectoryUtility.Glob (wildcard);
                    if (found.IsNullOrEmpty())
                    {
                        Console.Error.WriteLine ($"File {wildcard} not found");
                        continue;
                    }

                    result.AddRange (found);
                }

            } // foreach

            return result;

        } // method GatherFiles

        static void Run
            (
                ParseResult parseResult
            )
        {
            try
            {
                var wildcards = parseResult.ValueForArgument<string[]> (inputArgument);
                var groupNumber = parseResult.ValueForOption (numberOption);
                var groupWidth = parseResult.ValueForOption (widthOption);
                var dryRun = parseResult.ValueForOption (dryOption);

                if (wildcards.IsNullOrEmpty())
                {
                    Console.Error.WriteLine ("No input files specified");
                    Environment.Exit (1);
                }

                // отладочная печать -- какие спецификации файлов мы получили
                // Console.WriteLine (string.Join (", ", wildcards));

                try
                {
                    var existingFiles = GatherFiles (wildcards);
                    if (existingFiles.IsNullOrEmpty())
                    {
                        Console.Error.WriteLine ("No input files found");
                        Environment.Exit (1);
                    }

                    var renumber = new FileRenumber()
                    {
                        GroupWidth = groupWidth,
                        GroupNumber = groupNumber,
                        DryRun = dryRun
                    };

                    var output = AbstractOutput.Console;

                    var bunches = renumber.GenerateNames (existingFiles);
                    if (!renumber.CheckNames (bunches, output))
                    {
                        Console.Error.WriteLine ("Can't rename files");
                        Environment.Exit (1);
                    }

                    renumber.Rename (bunches, output);

                } // try

                catch (Exception exception)
                {
                    Console.Error.WriteLine (exception.Message);
                    Environment.ExitCode = 1;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine (exception);
            }

        } // method Run

        private static readonly Argument<string> inputArgument = new ("wildcard")
        {
            Arity = ArgumentArity.ZeroOrMore,
            Description = "маска файлов, подлежащих обработке"
        };
        private static readonly Option<int> numberOption = new ("-n", () => 0)
        {
            IsRequired = false,
            Description = "номер цифровой группы"
        };
        private static readonly Option<int> widthOption = new ("-w", () => 0)
        {
            IsRequired = false,
            Description = "ширина цифровой группы"
        };
        private static readonly Option<bool> dryOption = new ("-d", () => false)
        {
            IsRequired = false,
            Description = "холостой прогон (репетиция)"
        };

        static void Main (string[] args)
        {
            var rootCommand = new RootCommand("FileRenumber")
            {
                inputArgument,
                numberOption,
                widthOption,
                dryOption
            };
            rootCommand.Description = "Перенумерация файлов";
            rootCommand.Handler = CommandHandler.Create<ParseResult> (Run);

            new CommandLineBuilder (rootCommand)
                .UseDefaults()
                .Build()
                .Invoke(args);

        } // method Main

    } // class Progra,

} // namespace FileRenumber
