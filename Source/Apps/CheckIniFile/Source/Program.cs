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

/* Program.cs -- утилита для проверки лицензии в клиентском INI-файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

using AM;
using AM.IO;

using ManagedIrbis.Client;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace CheckIniFile
{
    class Program
    {
        static void Run
            (
                ParseResult parseResult
            )
        {
            try
            {
                var fileName = parseResult.ValueForArgument<string>("ini-file")
                    .ThrowIfNullOrEmpty();
                var encoding = IrbisEncoding.Ansi;

                using var iniFile = new IniFile(fileName, encoding);
                var lm = new ClientLM();
                var result = lm.CheckHash(iniFile);

                Console.WriteLine($"{fileName}: {result}");

                Environment.ExitCode = result ? 0 : 1;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Environment.ExitCode = 2;
            }
        }

        static void Main
            (
                string[] args
            )
        {
            var fileArgument = new Argument<string>("ini-file")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "имя клиентского INI-файла, например, cirbisc.ini"
            };
            var rootCommand = new RootCommand("CheckIniFile")
            {
                fileArgument
            };
            rootCommand.Description = "Проверка в клиентском INI-файле АБИС ИРБИС64";
            rootCommand.Handler = CommandHandler.Create<ParseResult>(Run);

            new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .Build()
                .Invoke(args);
        }
    }
}
