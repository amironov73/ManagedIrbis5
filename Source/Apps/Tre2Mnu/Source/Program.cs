// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Trees;

#endregion

#nullable enable

namespace Tre2Mnu
{
    class Program
    {
        static void ProcessMenu
            (
                string input,
                string output
            )
        {
            var reader = new StreamReader(File.OpenRead(input), IrbisEncoding.Ansi);
            var tree = TreeFile.ParseStream(reader);
            var menu = tree.ToMenu();
            File.WriteAllText
                (
                    output,
                    menu.ToText(),
                    IrbisEncoding.Ansi
                );
        }

        static int Main(string[] args)
        {
            var rootCommand = new RootCommand("Tre2Mnu")
            {
                new Argument<string>("input")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    Description = "Входной файл"
                },

                new Argument<string>("output")
                {
                    Arity = ArgumentArity.ExactlyOne,
                    Description = "Результирующий файл"
                }
            };
            rootCommand.Description = "Создание MNU-файла по TRE-файлу";
            rootCommand.Handler = CommandHandler.Create<string, string>(ProcessMenu);

            new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .Build()
                .Invoke(args);

            return 0;

        } // method Main

    } // class Program

} // namespace Tre2Mnu
