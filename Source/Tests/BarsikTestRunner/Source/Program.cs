// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;

using AM.Scripting.Barsik;

#endregion

#nullable enable

class Program
{
    private static string FindTestRoot()
    {
        var currentFolder = Directory.GetCurrentDirectory();

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
        var inputFolder = FindTestRoot();
        var outputFolder = Directory.GetCurrentDirectory();

        TestUtility.RunTests (inputFolder, outputFolder);
    }
}
