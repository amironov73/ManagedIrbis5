// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- утилита для создания таблицы актуализации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.IO;

using AM;

using ManagedIrbis.Fst;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace Fst2Ifs;

internal static class Program
{
    private static readonly Argument<string> _inputArgument = new ("fst-file")
    {
        Arity = ArgumentArity.ExactlyOne,
        Description = "имя FST-файла, например, ibis.fst"
    };

    private static readonly Argument<string> _outputArgument = new ("ifs-file")
    {
        Arity = ArgumentArity.ExactlyOne,
        Description = "имя IFS-файла (будет перезаписан!), например, ibis.ifs"
    };

    private static void Run
        (
            ParseResult parseResult
        )
    {
        Sure.NotNull (parseResult);

        try
        {
            var inputName = parseResult.GetValueForArgument (_inputArgument)
                .ThrowIfNullOrEmpty();

            var outputName = parseResult.GetValueForArgument (_outputArgument)
                .ThrowIfNullOrEmpty();

            var encoding = IrbisEncoding.Ansi;

            var reader = new StreamReader (inputName, encoding);
            var writer = new StreamWriter (outputName, false, encoding);

            try
            {
                using var transformer = new FstTransformer (reader, writer);

                transformer.TransformFile();
            }
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
    }

    internal static void Main
        (
            string[] args
        )
    {
        var rootCommand = new RootCommand ("Fst2Ifs")
        {
            _inputArgument,
            _outputArgument
        };
        rootCommand.Description = "Создание таблицы актуализации по таблице инверсии";
        rootCommand.SetHandler ((Action<ParseResult>) Run);

        new CommandLineBuilder (rootCommand)
            .UseDefaults()
            .Build()
            .Invoke (args);
    }
}
