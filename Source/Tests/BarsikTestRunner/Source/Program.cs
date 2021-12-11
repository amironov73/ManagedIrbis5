// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Scripting.Barsik;

#endregion

#nullable enable

class Program
{
    private static string FindTestRoot
        (
            string startAt
        )
    {
        var currentFolder = startAt;

        while (true)
        {
            var candidateFolder = Path.Combine (currentFolder, "Tests");
            var signatureFile = Path.Combine (candidateFolder, "root.here");
            if (File.Exists (signatureFile))
            {
                return candidateFolder;
            }

            currentFolder = Path.GetDirectoryName (currentFolder);
            if (string.IsNullOrEmpty (currentFolder))
            {
                throw new Exception ("Can't find tests");
            }
        }
    }

    public static void Main (string[] args)
    {
        var startAt = AppContext.BaseDirectory;
        if (args.Length != 0)
        {
            startAt = args[0];
        }

        var inputFolder = FindTestRoot (startAt);
        var outputFolder = Directory.GetCurrentDirectory();

        TestUtility.RunTests (inputFolder, outputFolder);
    }
}
