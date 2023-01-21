// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM.Kotik.Barsik;

#endregion

#nullable enable

namespace Meow;

internal static class Program
{
    private static void ExecuteScript
        (
            string fileName
        )
    {
        var sourceText = File.ReadAllText (fileName);
        var program = Grammar.ParseProgram (sourceText);
        Console.WriteLine (new string ('=', 70));
        program.Dump();
        Console.WriteLine(new string('=', 70));
        var context = new Context (Console.In, Console.Out, Console.Error);
        program.Execute (context);
        Console.WriteLine (new string ('=', 70));
        context.DumpVariables();
    }

    public static int Main
        (
            string[] args
        )
    {
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        // foreach (var fileName in args)
        // {
        //     ExecuteScript (fileName);
        // }

        var result = KotikUtility.CreateAndRunInterpreter
            (
                args,
                interpreter =>
                {
                    interpreter.WithStdLib();
                },
                (_, exception) =>
                {
                    Console.Error.WriteLine (exception);
                },
                interpreter =>
                {
                    interpreter.Context.DumpVariables();
                }
            );

        return result;
    }
}
