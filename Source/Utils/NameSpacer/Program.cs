// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* Program.cs -- простая утилита для замены пространств имен
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace NameSpacer;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal static class Program
{
    private static void ProcessFile
        (
            string sourceFile,
            string oldNamespace,
            string newNamespace
        )
    {
        Console.Write ($"{sourceFile} ... ");
        
        var lines = File.ReadAllLines (sourceFile);
        var found = false;
        foreach (var line in lines)
        {
            if (line.Contains (oldNamespace))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            Console.WriteLine ("SKIP");
            return;
        }

        for (var i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Replace (oldNamespace, newNamespace);
        }
        
        File.WriteAllLines (sourceFile, lines);
        Console.WriteLine ("OK");
    }
    
    private static void ProcessDirectory
        (
            string directory,
            string oldNamespace,
            string newNamespace
        )
    {
        var sourceFiles = Directory.EnumerateFiles 
            (
                directory, 
                "*.cs", 
                SearchOption.TopDirectoryOnly
            );

        foreach (var sourceFile in sourceFiles)
        {
            ProcessFile (sourceFile, oldNamespace, newNamespace);
        }

        foreach (var subDirectory in Directory.EnumerateDirectories (directory))
        {
            ProcessDirectory (subDirectory, oldNamespace, newNamespace);
        }
    }
    
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <returns>Признак успешного выполнения.</returns>
    public static int Main 
        (
            string[] args
        )
    {
        if (args.Length is < 2 or > 3)
        {
            Console.Error.WriteLine ("NameSpacer <old> <new> [directory]");
            
            return 1;
        }

        var oldNamespace = args[0];
        var newNamespace = args[1];
        var currentDirectory = args.Length > 2
            ? args[2]
            : Directory.GetCurrentDirectory();
        ProcessDirectory (currentDirectory, oldNamespace, newNamespace);

        return 0;
    }
}
