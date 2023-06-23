// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM;
using AM.Scripting.Barsik;
using AM.StableDiffusion.Scripting;

#endregion

#nullable enable

namespace StableBarsik;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static int Main
        (
            string[] args
        )
    {
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        Magna.Initialize (args);

        var result = BarsikUtility.CreateAndRunInterpreter
            (
                args,
                interpreter =>
                {
                    interpreter.WithStdLib();
                    interpreter.Context.AttachModule (new StableModule());
                },
                (_, exception) =>
                {
                    Console.WriteLine (exception);
                },
                interpreter =>
                {
                    interpreter.Context.DumpVariables();
                }
            );

        return result;
    }
}
