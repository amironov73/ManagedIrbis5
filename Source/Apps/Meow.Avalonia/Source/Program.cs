// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM.Kotik.Avalonia;
using AM.Kotik.Barsik;

#endregion

#nullable enable

namespace Meow.Avalonia;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static int Main (string[] args)
    {
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        BarsikApplication.Arguments = args;
        var result = Interpreter.CreateAndRunInterpreter
            (
                args,
                interpreter =>
                {
                    interpreter
                        .WithStdLib()
                        .WithAvalonia();
                    interpreter.Context.Commmon.Settings.ReplMode = false;
                },
                (_, exception) =>
                {
                    // TODO: показать окно с текстом исключения
                    Console.WriteLine (exception);
                }
            );

        return result;
    }
}
