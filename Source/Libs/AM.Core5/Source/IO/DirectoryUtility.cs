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

/* DirectoryUtility.cs -- работа с директориями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;

using AM.Text;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Работа с директориями.
/// </summary>
public static class DirectoryUtility
{
    #region Private members

    private static void _GetFiles
        (
            ICollection<string> found,
            string path,
            string[] masks,
            bool recursive
        )
    {
        foreach (var mask in masks)
        {
            var files = Directory.GetFiles (path, mask);
            foreach (var file in files)
            {
                if (!found.Contains (file))
                {
                    found.Add (file);
                }
            }
        }

        if (recursive)
        {
            var directories = Directory.GetDirectories (path);
            foreach (var dir in directories)
            {
                _GetFiles (found, dir, masks, true);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка возможности записи в папку по указанному пути.
    /// </summary>
    public static bool CanWriteTo
        (
            string path
        )
    {
        Sure.DirectoryExists (path);

        var pool = ArrayPool<byte>.Shared;
        var bytes = pool.Rent (1024);
        var fileName = Path.Combine
            (
                path,
                Guid.NewGuid().ToString ("N")
            );
        try
        {
            File.WriteAllBytes (fileName, bytes);
        }
        catch
        {
            return false;
        }
        finally
        {
            pool.Return (bytes);
        }

        File.Delete (fileName);

        return true;
    }

    /// <summary>
    /// Clears the specified directory. Deletes all files
    /// and subdirectories
    /// from the directory.
    /// </summary>
    public static void ClearDirectory
        (
            string path
        )
    {
        Sure.NotNullNorEmpty (path);

        foreach (var subdirectory in Directory.EnumerateDirectories (path))
        {
            Directory.Delete
                (
                    Path.Combine (path, subdirectory),
                    true
                );
        }

        foreach (var fileName in Directory.EnumerateFiles (path))
        {
            File.Delete (Path.Combine (path, fileName));
        }
    }

    /// <summary>
    /// Gets list of files in specified path.
    /// </summary>
    public static string[] GetFiles
        (
            string path,
            string mask,
            bool recursive
        )
    {
        Sure.NotNullNorEmpty (path);
        Sure.NotNullNorEmpty (mask);

        var found = new List<string>();

        var masks = mask.Split
            (
                CommonSeparators.Semicolon,
                StringSplitOptions.RemoveEmptyEntries
            );
        _GetFiles (found, path, masks, recursive);

        return found.ToArray();
    }

    /// <summary>
    /// Расширяет регулярное выражение DOS/Windows до списка файлов.
    /// </summary>
    /// <param name="wildcard">Регулярное выражение, включающее
    /// в себя символы * и ?, например *.exe или c:\*.bat.</param>
    /// <returns>Массив имен файлов, соответствующих регулярному
    /// выражению. Если параметр <paramref name="wildcard"/>
    /// включал имя директории, то каждое имя в массив также
    /// будет содержать имя директории.</returns>
    /// <remarks>В поиске участвуют только файлы, но не директории.
    /// </remarks>
    public static string[] Glob
        (
            string wildcard
        )
    {
        Sure.NotNullNorEmpty (wildcard);

        var dir = Path.GetDirectoryName (wildcard);
        if (string.IsNullOrEmpty (dir))
        {
            return Directory.GetFiles (Directory.GetCurrentDirectory(), wildcard);
        }

        var name = Path.GetFileName (wildcard);

        return Directory.GetFiles (dir, name);
    }

    #endregion
}
