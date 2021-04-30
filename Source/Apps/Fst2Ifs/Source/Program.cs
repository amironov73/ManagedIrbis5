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

/* Program.cs -- утилита для создания таблицы актуализации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace Fst2Ifs
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
                var inputName = parseResult.ValueForArgument<string>("fst-file")
                    .ThrowIfNullOrEmpty();
                var outputName = parseResult.ValueForArgument<string>("ifs-file")
                    .ThrowIfNullOrEmpty();
                var encoding = IrbisEncoding.Ansi;

                var reader = new StreamReader(inputName, encoding);
                var writer = new StreamWriter(outputName, false, encoding);

                try
                {
                    using var transformer = new FstTransformer(reader, writer);
                    transformer.TransformFile();
                }
                catch (Exception exception)
                {
                    Console.Error.WriteLine(exception.Message);
                    Environment.ExitCode = 1;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        static void Main(string[] args)
        {
            var inputArgument = new Argument<string>("fst-file")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "имя FST-файла, например, ibis.fst"
            };
            var outputArgument = new Argument<string>("ifs-file")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "имя IFS-файла (будет перезаписан!), например, ibis.ifs"
            };
            var rootCommand = new RootCommand("Fst2Ifs")
            {
                inputArgument,
                outputArgument
            };
            rootCommand.Description = "Создание таблицы актуализации по таблице инверсии";
            rootCommand.Handler = CommandHandler.Create<ParseResult>(Run);

            new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .Build()
                .Invoke(args);
        }
    }
}
