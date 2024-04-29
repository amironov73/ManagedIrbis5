// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* Unix.cs -- возня вокруг регистрозависимости имен файлов в системе UNIX
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM;

/// <summary>
/// Возня вокру регистрозависимости имен файлов
/// в системе UNIX.
/// </summary>
[PublicAPI]
public static class Unix
{
    #region Private members

    private static string? _Correct
        (
            string parent,
            string child
        )
    {
        var combined = Path.Combine (parent, child);
        if (Directory.Exists (combined))
        {
            return child;
        }

        var options = new EnumerationOptions
        {
            MatchCasing = MatchCasing.CaseInsensitive
        };
        foreach (var candidate in
                 Directory.EnumerateDirectories (parent, child, options))
        {
            return Path.GetFileName (candidate);
        }

        return null;
    }

    private static string? _FixDirectory
        (
            string directory
        )
    {
        if (Directory.Exists (directory))
        {
            return directory;
        }

        directory = directory.ConvertSlashes();
        var rooted = Path.IsPathRooted (directory);
        if (rooted)
        {
            directory = directory.Substring (1);
        }

        var trailed = Path.EndsInDirectorySeparator (directory);
        if (trailed)
        {
            directory = directory.Substring (0, directory.Length - 1);
        }

        var parts = directory.Split (Path.DirectorySeparatorChar);
        var builder = StringBuilderPool.Shared.Get();
        if (rooted)
        {
            builder.Append (Path.DirectorySeparatorChar);
        }

        var first = true;
        foreach (var child in parts)
        {
            var parent = builder.ToString();
            var corrected = _Correct (parent, child);
            if (corrected is null)
            {
                return null;
            }

            if (!first)
            {
                builder.Append (Path.DirectorySeparatorChar);
            }

            builder.Append (corrected);
            first = false;
        }

        if (trailed)
        {
            builder.Append (Path.DirectorySeparatorChar);
        }

        return builder.ReturnShared();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление строк в указанный файл.
    /// </summary>
    public static void AppendAllLines
        (
            string path,
            IEnumerable<string> lines,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        File.AppendAllLines (found, lines, encoding);
    }

    /// <summary>
    /// Добавление текста в указанный файл.
    /// </summary>
    public static void AppendAllText
        (
            string path,
            string text,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        File.AppendAllText (found, text, encoding);
    }

    /// <summary>
    /// Создание <see cref="StreamWriter"/> для добавления текста.
    /// </summary>
    public static StreamWriter AppendText
        (
            string path,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return new StreamWriter (found, true, encoding);
    }

    /// <summary>
    /// Создание <see cref="StreamWriter"/> с нуля.
    /// </summary>
    public static StreamWriter CreateText
        (
            string path,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFile (path) ?? path;

        return new StreamWriter (found, false, encoding);
    }

    /// <summary>
    /// Удаление файла с указанным именем.
    /// </summary>
    public static void DeleteFile
        (
            string path
        )
    {
        var found = FindFile (path);
        if (found is not null)
        {
            File.Delete (found);
        }
    }

    /// <summary>
    /// Директория с похожим именем существует?
    /// </summary>
    public static bool DirectoryExists (string directoryName) =>
        FindDirectory (directoryName) is not null;

    /// <summary>
    /// Файл с похожим именем существует?
    /// </summary>
    public static bool FileExists (string fileName) =>
        FindFile (fileName) is not null;

    /// <summary>
    /// Поиск директории с подходящим именем.
    /// </summary>
    public static string? FindDirectory
        (
            string directoryName
        )
    {
        Sure.NotNullNorEmpty (directoryName);

        if (Directory.Exists (directoryName))
        {
            return directoryName;
        }

        if (OperatingSystem.IsWindows())
        {
            return null;
        }

        var parent = Path.GetDirectoryName (directoryName) ?? ".";
        parent = _FixDirectory (parent);
        if (string.IsNullOrEmpty (parent))
        {
            return null;
        }

        var options = new EnumerationOptions
        {
            MatchCasing = MatchCasing.CaseInsensitive
        };
        var child = Path.GetFileName (directoryName);
        var directories = Directory.EnumerateDirectories
            (
                parent,
                child,
                options
            );
        foreach (var candidate in directories)
        {
            if (candidate.SameString (directoryName))
            {
                return candidate;
            }
        }

        return null;
    }

    /// <summary>
    /// Поиск директории с подходящим именем.
    /// Если директория не найдена, бросается исключение.
    /// </summary>
    public static string FindDirectoryOrThrow
        (
            string directoryName
        )
    {
        Sure.NotNullNorEmpty (directoryName);

        var result = FindDirectory (directoryName);
        if (string.IsNullOrEmpty (result))
        {
            throw new DirectoryNotFoundException (directoryName);
        }

        return result;
    }

    /// <summary>
    /// Поиск файла с подходящим именем.
    /// </summary>
    public static string? FindFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        if (File.Exists (fileName))
        {
            return fileName;
        }

        if (OperatingSystem.IsWindows())
        {
            return null;
        }

        var parent = Path.GetDirectoryName (fileName) ?? ".";
        parent = _FixDirectory (parent);
        if (string.IsNullOrEmpty (parent))
        {
            return null;
        }

        var options = new EnumerationOptions
        {
            MatchCasing = MatchCasing.CaseInsensitive
        };
        var child = Path.GetFileName (fileName);
        foreach (var candidate in
                 Directory.EnumerateFiles (parent, child, options))
        {
            if (candidate.SameString (fileName))
            {
                return candidate;
            }
        }

        return null;
    }

    /// <summary>
    /// Поиск файла с подходящим именем.
    /// Если файл не найден, бросается исключение.
    /// </summary>
    public static string FindFileOrThrow
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var result = FindFile (fileName);
        if (string.IsNullOrEmpty (result))
        {
            throw new FileNotFoundException (fileName);
        }

        return result;
    }

    /// <summary>
    /// Открытие указанного файла.
    /// </summary>
    public static FileStream OpenFile
        (
            string path,
            FileMode mode
        )
    {
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return File.Open (found, mode);
    }

    /// <summary>
    /// Открытие указанного файла.
    /// </summary>
    public static FileStream OpenFile
        (
            string path,
            FileMode mode,
            FileAccess access
        )
    {
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return File.Open (found, mode, access);
    }

    /// <summary>
    /// Открытие указанного файла.
    /// </summary>
    public static FileStream OpenFile
        (
            string path,
            FileMode mode,
            FileAccess access,
            FileShare share
        )
    {
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return File.Open (found, mode, access, share);
    }

    /// <summary>
    /// Открытие файла для чтения.
    /// </summary>
    public static FileStream OpenRead
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty(fileName);

        var found = FindFileOrThrow (fileName);

        return File.OpenRead (found);
    }

    /// <summary>
    /// Открытие текстового файла для чтения.
    /// </summary>
    public static StreamReader OpenText
        (
            string path
        )
    {
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return File.OpenText (found);
    }

    /// <summary>
    /// Открытие файла для чтения/записи.
    /// </summary>
    public static FileStream OpenWrite
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var found = FindFileOrThrow (fileName);

        return File.OpenWrite (found);
    }

    /// <summary>
    /// Чтение всех данных из указанного файла.
    /// </summary>
    public static byte[] ReadAllBytes
        (
            string path
        )
    {
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return File.ReadAllBytes (found);
    }

    /// <summary>
    /// Чтение всех строк из указанного файла.
    /// </summary>
    public static string[] ReadAllLines
        (
            string path,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return File.ReadAllLines (found, encoding);
    }

    /// <summary>
    /// Чтение всего текста из указанного файла.
    /// </summary>
    public static string ReadAllText
        (
            string path,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        return File.ReadAllText (found, encoding);
    }

    /// <summary>
    /// Построчное чтение строк из указанного файла.
    /// </summary>
    public static IEnumerable<string> ReadLines
        (
            string path,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindDirectoryOrThrow (path);

        return File.ReadLines (found, encoding);
    }

    /// <summary>
    /// Запись данных в указанный файл.
    /// </summary>
    public static void WriteAllBytes
        (
            string path,
            byte[] bytes
        )
    {
        Sure.NotNullNorEmpty (path);
        Sure.NotNull (bytes);

        var found = FindFileOrThrow (path);

        File.WriteAllBytes (found, bytes);
    }

    /// <summary>
    /// Запись строк в указанный файл.
    /// </summary>
    public static void WriteAllLines
        (
            string path,
            IEnumerable<string> lines,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        File.WriteAllLines (found, lines, encoding);
    }

    /// <summary>
    /// Запись текста в указанный файл.
    /// </summary>
    public static void WriteAllText
        (
            string path,
            string text,
            Encoding? encoding = default
        )
    {
        encoding ??= Encoding.UTF8;
        Sure.NotNullNorEmpty (path);

        var found = FindFileOrThrow (path);

        File.WriteAllText (found, text, encoding);
    }

    #endregion
}
