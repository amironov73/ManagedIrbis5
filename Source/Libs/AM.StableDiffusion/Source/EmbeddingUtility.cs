// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* EmbeddingUtility.cs -- полезные методы для тренировки текстовой инверсии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Linq;

using AM.Collections;

using SixLabors.ImageSharp;

#endregion

#nullable enable

namespace AM.StableDiffusion;

/// <summary>
/// Полезные методы для тренировки текстовой инверсии.
/// </summary>
public static class EmbeddingUtility
{
    #region Constants

    /// <summary>
    /// Ожидаемые расширения для файлов изображений.
    /// </summary>
    public static readonly string[] ExpectedImageExtensions =
        { ".jpg", ".jpeg", ".png" };

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка изображения на соответствие требуемым параметрам.
    /// </summary>
    public static string? CheckPreparedImage
        (
            Image image
        )
    {
        Sure.NotNull (image);

        if (image.Width != 512 || image.Height != 512)
        {
            return $"Invalid image size: {image.Size}";
        }

        return null;
    }

    /// <summary>
    /// Проверка изображения на соответствие требуемым параметрам.
    /// </summary>
    public static string? CheckPreparedImage
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var image = Image.Load (fileName);
        var result = CheckPreparedImage (image);
        if (!string.IsNullOrEmpty (result))
        {
            result = $"File: {fileName}, {result}";
        }

        return result;
    }

    /// <summary>
    /// Проверка всех изображений в папке
    /// на соответствие требуемым параметрам.
    /// </summary>
    public static IEnumerable<string> CheckPreparedImages
        (
            string directoryName
        )
    {
        Sure.DirectoryExists (directoryName);

        var allFiles = Directory.GetFiles (directoryName);
        var goodFiles = allFiles.Where
                (
                    it => Path.GetExtension (it)
                            .SameString (ExpectedImageExtensions)
                )
            .ToArray();
        if (goodFiles.IsNullOrEmpty())
        {
            throw new InvalidDataException();
        }

        foreach (var fileName in goodFiles)
        {
            var result = CheckPreparedImage (fileName);
            if (!string.IsNullOrEmpty (result))
            {
                yield return result;
            }
        }
    }

    #endregion
}
