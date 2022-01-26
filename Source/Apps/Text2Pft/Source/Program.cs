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

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.IO;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;

#endregion

namespace Text2Pft;

internal static class Program
{
    private static readonly Argument<string> _inputArgument = new ("text-file")
    {
        Arity = ArgumentArity.ExactlyOne,
        Description = "имя текстового файла, например, ticket.html"
    };
    private static readonly Argument<string> _outputArgument = new ("pft-file")
    {
        Arity = ArgumentArity.ExactlyOne,
        Description = "имя PFT-файла (будет перезаписан!), например, ticket.pft"
    };

    static void Run
        (
            ParseResult parseResult
        )
    {
        try
        {
            var inputName = parseResult.GetValueForArgument (_inputArgument)
                .ThrowIfNullOrEmpty();
            var outputName = parseResult.GetValueForArgument (_outputArgument)
                .ThrowIfNullOrEmpty();
            var encoding = IrbisEncoding.Ansi;

            using var input = new StreamReader (inputName, encoding);
            using var output = new StreamWriter (outputName, false, encoding);

            try
            {
                PftUtility.TextToPft (input, output);
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

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    public static void Main
        (
            string[] args
        )
    {
        var rootCommand = new RootCommand ("Mnu2Tre")
        {
            _inputArgument,
            _outputArgument
        };
        rootCommand.Description = "Создание PFT по текстовому файлу";
        rootCommand.SetHandler ((Action<ParseResult>) Run);

        new CommandLineBuilder (rootCommand)
            .UseDefaults()
            .Build()
            .Invoke (args);
    }
}
