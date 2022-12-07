// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using ManagedIrbis.Fixing;

#endregion

#nullable enable

namespace FixRoot;

internal static class Program
{
    public static int Main
        (
            string[] args
        )
    {
        try
        {
            if (args.Length is < 1 or > 2)
            {
                Console.Error.WriteLine ("FixRoot <ini-file> [new-root]");
                return 1;
            }

            var currentDirectory = Directory.GetCurrentDirectory();
            var iniFile = Path.GetFullPath
                (
                    args[0],
                    currentDirectory
                );
            var newRoot = args.Length > 1
                ? args[1]
                : Path.GetDirectoryName (iniFile)!;
            newRoot = Path.GetFullPath (newRoot, currentDirectory);

            new IniFilePathFixer().ChangeRootPath (iniFile, newRoot);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
