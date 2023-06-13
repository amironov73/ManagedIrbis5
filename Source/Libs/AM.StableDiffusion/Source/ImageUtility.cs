// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ImageUtility.cs -- полезные методы для работы со сгенерированными изображениями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using SixLabors.ImageSharp;

#endregion

#nullable enable

namespace AM.StableDiffusion;

/// <summary>
/// Полезные методы для работы со сгенерированными изображениями.
/// </summary>
[PublicAPI]
public static class ImageUtility
{
    #region Public methods

    /// <summary>
    /// Извлечение текстовых данных из указанного файла с изображением.
    /// </summary>
    public static string? RetrieveTextData
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        using var image = Image.Load (fileName);

        var genericMeta = image.Metadata;
        var pngMeta = genericMeta?.GetPngMetadata();
        if (pngMeta is null)
        {
            Magna.Logger.LogError ("{FileName}: can\'t get PNG metadata", fileName);
            return null;
        }

        foreach (var data in pngMeta.TextData)
        {
            return data.Value;
        }

        Magna.Logger.LogError ("{FileName}: no text data", fileName);
        return null;
    }

    #endregion
}
