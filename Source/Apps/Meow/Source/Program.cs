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
using System.Text;

using AM.Kotik.Barsik;

#endregion

#nullable enable

namespace Meow;

internal static class Program
{
    public static int Main
        (
            string[] args
        )
    {
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        var result = Interpreter.CreateAndRunInterpreter
            (
                args,
                interpreter =>
                {
                    interpreter.WithStdLib();
                },
                (_, exception) =>
                {
                    Console.Error.WriteLine (exception);
                }
            );

        return result;
    }
}
