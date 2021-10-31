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

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Trees;

#endregion

namespace Mnu2Tre
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
                var inputName = parseResult.ValueForArgument<string> ("mnu-file")
                    .ThrowIfNullOrEmpty ();
                var outputName = parseResult.ValueForArgument<string> ("tre-file")
                    .ThrowIfNullOrEmpty ();

                try
                {
                    var menu = MenuFile.ParseLocalFile
                        (
                            inputName,
                            IrbisEncoding.Ansi
                        );
                    var tree = menu.ToTree();
                    tree.SaveToLocalFile
                        (
                            outputName,
                            IrbisEncoding.Ansi
                        );
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
            var inputArgument = new Argument<string> ("mnu-file")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "имя MNU-файла, например, ii.mnu"
            };
            var outputArgument = new Argument<string> ("tre-file")
            {
                Arity = ArgumentArity.ExactlyOne,
                Description = "имя TRE-файла (будет перезаписан!), например, ii.tre"
            };
            var rootCommand = new RootCommand ("Mnu2Tre")
            {
                inputArgument,
                outputArgument
            };
            rootCommand.Description = "Создание дерева по файлу меню";
            rootCommand.Handler = CommandHandler.Create<ParseResult> (Run);

            new CommandLineBuilder (rootCommand)
                .UseDefaults()
                .Build()
                .Invoke (args);

        } // method Main

    } // class Program

} // namespace Mnu2Tre
