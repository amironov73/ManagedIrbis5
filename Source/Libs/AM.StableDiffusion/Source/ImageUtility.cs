// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ImageUtility.cs -- полезные методы для работы со сгенерированными изображениями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AM.Collections;

using DynamicData;

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
    #region Constants

    private const string NegativePrompt = "Negative prompt";

    #endregion

    #region Private members

    private static void ParseLine
        (
            Dictionary<string, string> dictionary,
            string line
        )
    {
        var regex = new Regex (@"^([\w\s]+): ");
        var match = regex.Match (line);
        if (match.Success)
        {
            var key = match.Groups[1].Value;
            var restOfLine = line.Substring (match.Length);
            var value = restOfLine;
            if (string.IsNullOrEmpty (value))
            {
                return;
            }

            if (value[0] == '"')
            {
                var endOfValue = value.IndexOf ('"', 1);
                if (endOfValue > 0)
                {
                    value = value.Substring (1, endOfValue - 1).Trim();
                    restOfLine = restOfLine.Substring (endOfValue + 2).Trim();
                    dictionary.Add (key, value);
                    if (!string.IsNullOrEmpty (restOfLine))
                    {
                        ParseLine (dictionary, restOfLine);
                    }
                }
            }
            else
            {
                var endOfValue = value.IndexOf (',');
                if (endOfValue > 0)
                {
                    value = value.Substring (0, endOfValue);
                    restOfLine = restOfLine.Substring (endOfValue + 1).Trim();
                    dictionary.Add (key, value);
                }

                if (!string.IsNullOrEmpty (restOfLine))
                {
                    ParseLine (dictionary, restOfLine);
                }
            }

        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Декодирование текстовых данных, извлеченных из изображения.
    /// </summary>
    public static Dictionary<string, string> DecodeTextData
        (
            string textData
        )
    {
        Sure.NotNull (textData);

        var result = new Dictionary<string, string>();
        var lines = textData.SplitLines();
        if (lines.IsNullOrEmpty())
        {
            return result;
        }

        result.Add ("Prompt", lines[0]);
        foreach (var line in lines.Skip (1))
        {
            if (line.StartsWith (NegativePrompt))
            {
                result.Add (NegativePrompt, line.Substring (NegativePrompt.Length + 2));
            }
            else
            {
                ParseLine (result, line);
            }
        }

        return result;
    }

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
