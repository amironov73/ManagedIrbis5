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
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;

#endregion

namespace Text2Pft
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
                var inputName = parseResult.ValueForArgument<string> ("text-file")
                    .ThrowIfNullOrEmpty ();
                var outputName = parseResult.ValueForArgument<string> ("pft-file")
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

        } // method Run

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        public static void Main
            (
                string[] args
            )
        {
            var inputArgument = new Argument<string> ("text-file")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "имя текстового файла, например, ticket.html"
            };
            var outputArgument = new Argument<string> ("pft-file")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "имя PFT-файла (будет перезаписан!), например, ticket.pft"
            };
            var rootCommand = new RootCommand ("Mnu2Tre")
            {
                inputArgument,
                outputArgument
            };
            rootCommand.Description = "Создание PFT по текстовому файлу";
            rootCommand.Handler = CommandHandler.Create<ParseResult> (Run);

            new CommandLineBuilder (rootCommand)
                .UseDefaults()
                .Build()
                .Invoke (args);

        } // method Main

    } // class Program

} // namespace Text2Pft
