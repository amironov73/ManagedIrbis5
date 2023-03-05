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

using AM.IO;

using NamerCommon;

using Spectre.Console;

#endregion

#nullable enable

namespace NamerConsole;

/// <summary>
/// Точка входа в программу.
/// </summary>
internal static class Program
{
    private static bool RenameImpl<T>
        (
            Folder folder,
            NamePair pair,
            T? arg
        )
    {
        var oldName = Path.Combine (folder.DirectoryName!, pair.Old);
        var newName = Path.Combine (folder.DirectoryName!, pair.New);

        // var result = FileUtility.TryMove (oldName, newName);
        File.Move (oldName, newName);
        // var color = result ? Color.Green : Color.Red;
        var color = Color.Green;
        var style = new Style (color);
        AnsiConsole.Write ("  ");
        AnsiConsole.Write (new Text ($"{pair.Old} => {pair.New}", style));
        //if (!result)
        //{
        //    AnsiConsole.Write (new Text (" ERROR", new Style (Color.Red)));
        //}

        AnsiConsole.WriteLine();

        return true;
    }
    
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
            processor.Reset();
            var directory = new DirectoryInfo (dirName);
            var pairs = processor.Render (context, directory);
            var folder = new Folder (dirName, pairs);
            if (processor.DryRun)
            {
                folder.CheckNames();
                folder.Render();
            }
            else
            {
                if (!folder.CheckNames())
                {
                    folder.Render (errorsOnly: true);
                }
                else
                {
                    AnsiConsole.Write (new Text (folder.DirectoryName!, new Style (Color.Aqua)));
                    AnsiConsole.WriteLine();
                    folder.Rename<object> (RenameImpl, null);
                    AnsiConsole.WriteLine();
                }
            }
        }
    }
}
