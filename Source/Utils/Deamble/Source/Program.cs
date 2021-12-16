// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/*

    Удаляет преамбулу UTF-8 в указанных файлах.

    Командная строка:

    ```shell
    Deamble <pattern> [pattern...]
    ```

    где `pattern` - шаблон имени файла, например, `*.cs`.

 */

#region Using directives

using System;
using System.IO;
using System.Linq;

#endregion

internal static class Program
{
    /// <summary>
    /// Удаляем преамбулу из указанного файла, копируя его во временный файл.
    /// </summary>
    private static void DeambleFile
        (
            string fileName
        )
    {
        FileStream sourceStream;

        try
        {
            sourceStream = File.OpenRead (fileName);
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception.Message);
            return;
        }

        const int bufferSize = 4096;
        var buffer = new byte [bufferSize];
        var read = sourceStream.Read (buffer);
        if (read < 3)
        {
            // файл слишком короткий, в нем не может быть преамбулы
            Console.WriteLine ("too short");
            return;
        }

        if (buffer[0] != 0xEF || buffer[1] != 0xBB || buffer[2] != 0xBF)
        {
            // нет преамбулы, удалять нечего
            Console.WriteLine ("no preamble");
            return;
        }

        var temporaryName = fileName + "_" + Guid.NewGuid().ToString ("N");
        var outputStream = File.Create (temporaryName);

        // пропускаем первые три байта
        outputStream.Write (buffer.AsSpan (3, read - 3));

        while (true)
        {
            read = sourceStream.Read (buffer);
            if (read == 0)
            {
                break;
            }

            outputStream.Write (buffer.AsSpan (0, read));
        }

        sourceStream.Dispose();
        outputStream.Dispose();

        File.Delete (fileName);
        File.Move (temporaryName, fileName);

        Console.WriteLine ("done");
    }

    /// <summary>
    /// Отыскиваем файлы с именами, соответствующими шаблону.
    /// Рекурсивно по вложенным директориям.
    /// </summary>
    private static string[] DiscoverFiles
        (
            string pattern
        )
    {
        var path = Path.GetDirectoryName (pattern);
        if (path is not null)
        {
            pattern = pattern[path.Length..];
            var firstChar = pattern.First();
            if (firstChar == Path.DirectorySeparatorChar
                || firstChar == Path.AltDirectorySeparatorChar)
            {
                pattern = pattern[1..];
            }
        }

        if (string.IsNullOrEmpty (path))
        {
            path = Directory.GetCurrentDirectory();
        }

        return Directory.GetFiles (path, pattern, SearchOption.AllDirectories);
    }

    private static int Main
        (
            string[] args
        )
    {
        if (args.Length == 0)
        {
            Console.WriteLine ("Usage: Deamble <pattern> [pattern...]");

            return 0;
        }

        foreach (var pattern in args)
        {
            var files = DiscoverFiles (pattern);
            foreach (var file in files)
            {
                Console.Write ($"{file} ");
                DeambleFile (file);
            }
        }

        return 0;
    }
}
