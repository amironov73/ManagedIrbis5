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

using NamerCommon;

#endregion

#nullable enable

namespace NamerConsole;

/// <summary>
/// Точка входа в программу.
/// </summary>
internal static class Program
{
    public static void Main
        (
            string[] args
        )
    {
        if (args.Length < 2)
        {
            Console.WriteLine ("NamerConsole [options] <specification> <directory> ...");
            return;
        }

        var processor = new NameProcessor();
        var context = new NamingContext();
        var directories = processor.ParseCommandLine (context, args);

        foreach (var dirName in directories)
        {
            var directory = new DirectoryInfo (dirName);
            var pairs = processor.Render (context, directory);
            NamePair.Render (pairs);
        }
    }
}
