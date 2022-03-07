// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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

using AM.Text.Output;

#endregion

#nullable enable

namespace Sigler;

class Program
{
    static void Main
        (
            string[] args
        )
    {
        if (args.Length != 2)
        {
            Console.WriteLine("SIGLER <sigla.txt> <connectionString>");
            return;
        }

        var fileName = args[0];
        var connectionString = args[1];

        var output = AbstractOutput.Console;
        using var stamper = new SiglaStamper
            (
                connectionString,
                output
            );
        stamper.ProcessFile(fileName);
    }
}
