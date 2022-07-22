// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* FileUtility.cs -- утилиты для работы с файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Утилиты для работы с файлами.
/// </summary>
public static class FileUtility
{
    #region Private members

    private static readonly char[] _InvalidFileNameChars =
    {
        '\"', '<', '>', '|', '\0', ':', '*', '?', '\\', '/', ';', ',',
        (char)1, (char)2, (char)3, (char)4, (char)5, (char)6, (char)7,
        (char)8, (char)9, (char)10, (char)11, (char)12, (char)13,
        (char)14, (char)15, (char)16, (char)17, (char)18, (char)19,
        (char)20, (char)21, (char)22, (char)23, (char)24, (char)25,
        (char)26, (char)27, (char)28, (char)29, (char)30, (char)31
    };

    private static readonly string[] _SpecialFileNames =
    {
        "CON", "PRN", "AUX", "CLOCK$", "NUL", "COM0", "COM1", "COM2",
        "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT0",
        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8",
        "LPT9", "CONFIG$", "$Mft", "$MftMirr", "$LogFile", "$Volume",
        "$AttrDef", "$Bitmap", "$Boot", "$BadClus", "$Secure", "$Upcase",
        "$Extend", "$Quota", "$ObjId", "$Reparse"
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Побайтовое сравнение двух файлов.
    /// </summary>
    public static int CompareFiles
        (
            string first,
            string second
        )
    {
        Sure.FileExists (first);
        Sure.FileExists (second);

        using FileStream firstStream = File.OpenRead (first),
            secondStream = File.OpenRead (second);

        return StreamUtility.CompareTo
            (
                firstStream,
                secondStream
            );
    }

    /// <summary>
    /// Преобразование текста в валидное имя файла.
    /// </summary>
    public static string ConvertToFileName
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var invalidCharacters = GetInvalidFileNameChars();
        if (text.Length >= PathUtility.MaxPath)
        {
            text = text.Substring (0, PathUtility.MaxPath);
        }

        var found = false;
        foreach (var c in text)
        {
            if (c.IsOneOf (invalidCharacters))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            return text.Trim();
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (text.Length);
        var first = true;
        foreach (var c in text)
        {
            if (c.IsOneOf (invalidCharacters))
            {
                if (first)
                {
                    builder.Append ('_');
                }

                // съедаем лишние замещающие символы
                first = false;
            }
            else
            {
                builder.Append (c);
                first = true;
            }
        }

        builder.Trim();

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Копирование указанного исходного файла поверх указанного файла назначения.
    /// Отличается тем, что переносит времена (создания, последнего доступа и модификации).
    /// </summary>
    public static void Copy
        (
            string sourceName,
            string targetName,
            bool overwrite
        )
    {
        Sure.FileExists (sourceName);
        Sure.NotNullNorEmpty (targetName);

        File.Copy (sourceName, targetName, overwrite);

        // переносим времена с исходного файла
        var creationTime = File.GetCreationTime (sourceName);
        File.SetCreationTime (targetName, creationTime);
        var lastAccessTime = File.GetLastAccessTime (sourceName);
        File.SetLastAccessTime (targetName, lastAccessTime);
        var lastWriteTime = File.GetLastWriteTime (sourceName);
        File.SetLastWriteTime (targetName, lastWriteTime);
        var attributes = File.GetAttributes (sourceName);
        File.SetAttributes (targetName, attributes);
    }

    /// <summary>
    /// Copies given file only if source is newer than destination.
    /// </summary>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="targetPath">The target path.</param>
    /// <param name="backup">If set to <c>true</c>
    /// create backup copy of destination file.</param>
    /// <returns><c>true</c> if file copied; <c>false</c> otherwise.
    /// </returns>
    public static bool CopyNewer
        (
            string sourcePath,
            string targetPath,
            bool backup
        )
    {
        Sure.FileExists (sourcePath);
        Sure.NotNullNorEmpty (targetPath);

        if (File.Exists (targetPath))
        {
            var sourceInfo = new FileInfo (sourcePath);
            var targetInfo = new FileInfo (targetPath);
            if (sourceInfo.LastWriteTime < targetInfo.LastWriteTime)
            {
                return false;
            }

            if (backup)
            {
                CreateBackup (targetPath, true);
            }
        }

        File.Copy (sourcePath, targetPath, true);

        return true;
    }

    /// <summary>
    /// Copies given file and creates backup copy of target file.
    /// </summary>
    /// <param name="sourcePath">The source path.</param>
    /// <param name="targetPath">The target path.</param>
    /// <returns>Name of backup file or <c>null</c>
    /// if no backup created.</returns>
    public static string? CopyWithBackup
        (
            string sourcePath,
            string targetPath
        )
    {
        Sure.FileExists (sourcePath);
        Sure.NotNullNorEmpty (targetPath);

        string? result = null;
        if (File.Exists (targetPath))
        {
            result = CreateBackup (targetPath, true);
        }

        File.Copy (sourcePath, targetPath, false);

        return result;
    }

    /// <summary>
    /// Creates backup copy for given file.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <param name="rename">If set to <c>true</c>
    /// given file will be renamed; otherwise it will be copied.</param>
    /// <returns>Name of the backup file.</returns>
    public static string CreateBackup
        (
            string path,
            bool rename
        )
    {
        Sure.FileExists (path);

        var result = GetNotExistentFileName
            (
                path,
                "_backup_"
            );
        if (rename)
        {
            File.Move (path, result);
        }
        else
        {
            File.Copy (path, result, false);
        }

        return result;
    }

    /// <summary>
    /// Удаление файла, если он существует.
    /// Если файл с указанным именем не существует, ничего не происходит.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    public static void DeleteIfExists
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        if (Directory.Exists (fileName))
        {
            throw new FileNotFoundException ("Directory, not file", fileName);
        }

        if (File.Exists (fileName))
        {
            File.SetAttributes (fileName, FileAttributes.Normal);
            File.Delete (fileName);
        }
    }

    /// <summary>
    /// Найти файл по путям, используя стандартный разделитель.
    /// </summary>
    /// <returns></returns>
    public static string? FindFileInPath
        (
            string fileName,
            string? path
        )
    {
        return FindFileInPath (fileName, path, Path.PathSeparator);
    }

    /// <summary>
    /// Найти файл по путям, используя указанный разделитель.
    /// </summary>
    public static string? FindFileInPath
        (
            string fileName,
            string? path,
            char pathSeparator
        )
    {
        Sure.NotNullNorEmpty (fileName);

        if (string.IsNullOrWhiteSpace (path))
        {
            return null;
        }

        var elements = path.Split (pathSeparator);
        foreach (var element in elements)
        {
            var fullPath = Path.Combine
                (
                    element,
                    fileName
                );
            if (File.Exists (fullPath))
            {
                return fullPath;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение массива символов, которые не должны употребляться в имени файла.
    /// Метод намеренно сделан не портабельным, чтобы то, что запрещено под
    /// Windows, не создавалось бы и под Unix, и наоборот.
    /// </summary>
    public static char[] GetInvalidFileNameChars()
    {
        return _InvalidFileNameChars;
    }

    /// <summary>
    /// Gets the name of the not existent file.
    /// </summary>
    /// <param name="original">The original.</param>
    /// <param name="suffix">The suffix.</param>
    /// <returns>Name of not existent file.</returns>
    public static string GetNotExistentFileName
        (
            string original,
            string suffix
        )
    {
        Sure.NotNullNorEmpty (original);
        Sure.NotNullNorEmpty (suffix);

        var path = Path.GetDirectoryName (original) ?? string.Empty;
        var name = Path.GetFileNameWithoutExtension (original);
        var ext = Path.GetExtension (original);

        for (var i = 1; i < 10000; i++)
        {
            var result = Path.Combine
                (
                    path,
                    name + suffix + i + ext
                );
            if (!File.Exists (result) && !Directory.Exists (result))
            {
                return result;
            }
        }

        // TODO diagnostics

        Magna.Logger.LogError
            (
                nameof (FileUtility) + "::" + nameof (GetNotExistentFileName)
                + ": giving up"
            );

        throw new ArsMagnaException();
    }

    /// <summary>
    /// Проверяет, не является ли указанное имя зарезервированным.
    /// Метод намеренно сделан не портабельным, чтобы то, что запрещено под
    /// Windows, не создавалось бы и под Unix, и наоборот.
    /// </summary>
    public static bool IsSpecialName
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        foreach (var name in _SpecialFileNames)
        {
            if (fileName.SameString (name))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Sets file modification date to current date.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <remarks>If no such file exists it will be created.</remarks>
    public static void Touch
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        if (File.Exists (fileName))
        {
            File.SetLastWriteTime (fileName, DateTime.Now);
        }
        else
        {
            File.WriteAllBytes (fileName, Array.Empty<byte>());
        }
    }

    /// <summary>
    /// Attempts to delete the file at the specified path, returning <c>true</c> if successful.
    /// </summary>
    public static bool TryDelete
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        // delete the file, ignoring common exceptions (e.g., file in use, insufficient permissions)
        try
        {
            DeleteIfExists (fileName);

            return true;
        }
        catch (IOException exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (FileUtility) + "::" + nameof (TryDelete)
                );
        }
        catch (UnauthorizedAccessException exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (FileUtility) + "::" + nameof (TryDelete)
                );
        }

        return false;
    }

    /// <summary>
    /// Attempts to move the specified file to a new location, returning <c>true</c> if successful.
    /// </summary>
    /// <param name="sourceFileName">The name of the file to move.</param>
    /// <param name="destinationFileName">The new path for the file.</param>
    /// <returns><c>true</c> if the file was successfully moved, otherwise <c>false</c>.</returns>
    /// <remarks><paramref name="destinationFileName"/> must not exist.</remarks>
    public static bool TryMove
        (
            string sourceFileName,
            string destinationFileName
        )
    {
        Sure.NotNullNorEmpty (sourceFileName);
        Sure.NotNullNorEmpty (destinationFileName);

        try
        {
            File.Move (sourceFileName, destinationFileName);

            return true;
        }
        catch (FileNotFoundException exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (FileUtility) + "::" + nameof (TryMove)
                );
        }
        catch (IOException exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (FileUtility) + "::" + nameof (TryMove)
                );
        }
        catch (UnauthorizedAccessException exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (FileUtility) + "::" + nameof (TryMove)
                );
        }

        return false;
    }

    #endregion
}
