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

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Trees;

#endregion

#nullable enable

namespace Mnu2Tre;

class Program
{
    private static readonly Argument<string> _inputArgument = new ("mnu-file")
    {
        Arity = ArgumentArity.ExactlyOne,
        Description = "имя MNU-файла, например, ii.mnu"
    };

    private static readonly Argument<string> _outputArgument = new ("tre-file")
    {
        Arity = ArgumentArity.ExactlyOne,
        Description = "имя TRE-файла (будет перезаписан!), например, ii.tre"
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
        rootCommand.Description = "Создание дерева по файлу меню";
        rootCommand.SetHandler ((Action<ParseResult>)Run);

        new CommandLineBuilder (rootCommand)
            .UseDefaults()
            .Build()
            .Invoke (args);
    }
}
