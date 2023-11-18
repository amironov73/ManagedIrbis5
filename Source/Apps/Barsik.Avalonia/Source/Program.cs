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

using AM.Scripting.Barsik;

#endregion

namespace Barsik;

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

        var result = BarsikUtility.CreateAndRunInterpreter
            (
                args,
                static interpreter =>
                {
                    interpreter.WithStdLib();
                },
                static (_, exception) =>
                {
                    Console.WriteLine (exception);
                },
                static interpreter =>
                {
                    interpreter.Context.DumpVariables();
                }
            );

        return result;
    }
}
