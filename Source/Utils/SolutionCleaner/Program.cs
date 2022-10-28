// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/*

  Простая утилита, удаляющая директории `bin` и `obj` во всех проектах по указанному пути.

  SolutionCleaner <dir> [<dir> ...]

 */

/* Program.cs -- вся логика программы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace SolutionCleaner;

/// <summary>
/// Вся логика программы.
/// </summary>
public static class Program
{
    /// <summary>
    /// Рекурсивное удаление указанной директории, если она существует.
    /// </summary>
    private static bool RemoveDirectory
        (
            string rootDir,
            string subDir
        )
    {
        var directory = Path.Combine (rootDir, subDir);
        if (Directory.Exists (directory))
        {
            Directory.Delete (directory, true);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Очистка указанного проекта.
    /// </summary>
    private static void CleanProject
        (
            string projectFile
        )
    {
        var projectDir = Path.GetDirectoryName (projectFile);
        if (string.IsNullOrEmpty (projectDir))
        {
            return;
        }

        Console.Write (projectFile);
        Console.Write (" => ");
        var success = RemoveDirectory (projectDir, "bin")
            || RemoveDirectory (projectDir, "obj");
        Console.WriteLine
            (
                success ? "OK" : "ALREADY"
            );
    }

    /// <summary>
    /// Поиск проектов в указанной директории.
    /// </summary>
    private static void FindProjects
        (
            string rootPath
        )
    {
        var found = Directory.GetFiles
            (
                rootPath,
                "*.csproj",
                SearchOption.AllDirectories
            );

        foreach (var projectFile in found)
        {
            CleanProject (projectFile);
        }
    }

    public static int Main
        (
            string[] args
        )
    {
        if (args.Length == 0)
        {
            Console.Error.WriteLine ("Usage: SolutionCleaner <dir> [<dir> ...]");

            return 1;
        }

        try
        {
            foreach (var rootPath in args)
            {
                FindProjects (rootPath);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);

            return 1;
        }

        return 0;
    }

}
