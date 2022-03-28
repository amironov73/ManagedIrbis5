// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* PathUtility.cs -- работа с путями к файлам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Работа с путями к файлам.
/// </summary>
public static class PathUtility
{
    #region Constants

    /// <summary>
    /// Максимальная длина имени файла.
    /// </summary>
    public const int MaxPath = 256;

    #endregion

    #region Private members

    private static readonly string _backslash
        = new (Path.DirectorySeparatorChar, 1);

    #endregion

    #region Public methods

    /// <summary>
    /// Appends trailing backslash (if none exists)
    /// to given path.
    /// </summary>
    /// <param name="path">Path to convert.</param>
    /// <returns>Converted path.</returns>
    /// <remarks>Path need NOT to be existent.</remarks>
    public static string AppendBackslash
        (
            string path
        )
    {
        Sure.NotNullNorEmpty (path);

        var result = ConvertSlashes (path);
        if (!result.EndsWith (_backslash))
        {
            result += _backslash;
        }

        return result;
    }

    /// <summary>
    /// Converts ordinary slashes to backslashes.
    /// </summary>
    /// <param name="path">Path to convert.</param>
    /// <returns>Converted path.</returns>
    /// <remarks>Path need NOT to be existent.</remarks>
    public static string ConvertSlashes
        (
            string path
        )
    {
        Sure.NotNull (path);

        var result = path.Replace
            (
                Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar
            );

        return result;
    }

    /// <summary>
    /// Get relative path.
    /// </summary>
    public static string GetRelativePath
        (
            string absolutePath,
            string baseDirectory
        )
    {
        Sure.NotNullNorEmpty (absolutePath);
        Sure.NotNullNorEmpty (baseDirectory);

        // absolutePath = Path.GetFullPath(absolutePath);
        // baseDirectory = Path.GetFullPath(baseDirectory);

        var mainSeparator = char.ToString (Path.DirectorySeparatorChar);
        var altSeparator = char.ToString (Path.AltDirectorySeparatorChar);

        string[] separators =
        {
            mainSeparator,
            altSeparator
        };

        var absoluteParts = absolutePath.Split (separators, StringSplitOptions.RemoveEmptyEntries);
        var baseParts = baseDirectory.Split (separators, StringSplitOptions.RemoveEmptyEntries);
        var length = Math.Min
            (
                absoluteParts.Length,
                baseParts.Length
            );

        var offset = 0;
        for (var i = 0; i < length; i++)
        {
            if (absoluteParts[i].SameString (baseParts[i]))
            {
                offset++;
            }
            else
            {
                break;
            }
        }

        if (0 == offset)
        {
            if (!absolutePath.StartsWith (mainSeparator)) // Linux
            {
                throw new ArgumentException
                    (
                        "Paths do not have a common base!"
                    );
            }
        }

        var relativePath = new StringBuilder();

        for (var i = 0; i < baseParts.Length - offset; i++)
        {
            relativePath.Append ("..");
            relativePath.Append (mainSeparator);
        }

        for (var i = offset; i < absoluteParts.Length - 1; i++)
        {
            relativePath.Append (absoluteParts[i]);
            relativePath.Append (mainSeparator);
        }

        relativePath.Append (absoluteParts[^1]);

        return relativePath.ToString();
    }

    /// <summary>
    /// Maps the path relative to the executable name.
    /// </summary>
    public static string MapPath
        (
            string path
        )
    {
        Sure.NotNullNorEmpty (path);

        var appDirectory = AppContext.BaseDirectory;
        var result = string.IsNullOrEmpty (appDirectory)
            ? path
            : Path.Combine (appDirectory, path);

        return result;
    }

    /// <summary>
    /// Strips extension from given path.
    /// </summary>
    public static string StripExtension
        (
            string path
        )
    {
        var extension = Path.GetExtension (path);
        var result = path;
        if (!string.IsNullOrEmpty (extension))
        {
            result = result.Substring
                (
                    0,
                    result.Length - extension.Length
                );
        }

        return result;
    }

    /// <summary>
    /// Removes trailing backslash (if exists) from the path.
    /// </summary>
    /// <param name="path">Path to convert.</param>
    /// <returns>Converted path.</returns>
    /// <remarks>Path need NOT to be existent.</remarks>
    public static string StripTrailingBackslash
        (
            string path
        )
    {
        var result = ConvertSlashes (path);
        while (result.EndsWith (_backslash))
        {
            result = result.Substring
                (
                    0,
                    result.Length - _backslash.Length
                );
        }

        return result;
    }

    #endregion
}
