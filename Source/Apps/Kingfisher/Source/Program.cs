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

#endregion

#nullable enable

namespace Kingfisher;

internal static class Program
{
    private static int Main
        (
            string[] args
        )
    {
        if (args.Length != 1)
        {
            Console.WriteLine ("Kingfisher <connectionString>");
            return 1;
        }

        try
        {
            using var lister = new ZimaLister (args[0]);

            lister.RetrieveTree();
            lister.ListRecords();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
