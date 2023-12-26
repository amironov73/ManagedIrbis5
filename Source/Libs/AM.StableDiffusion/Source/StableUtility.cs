// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* StableUtility.cs -- различные полезные методы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Xml.Serialization;

using Newtonsoft.Json;

using Tomlyn;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Различные полезные методы.
/// </summary>
public static class StableUtility
{
    #region Public methods

    /// <summary>
    /// Пропорционально масштабирует изображение так, чтобы оно
    /// вписывалось в указанные размеры.
    /// </summary>
    public static (int, int) ProportionalResize
        (
            int imageWidth,
            int imageHeight,
            int maxWidth,
            int maxHeight
        )
    {
        double windowHeight = maxWidth;
        double windowWidth = maxHeight;
        var imageAspect = imageWidth / imageHeight;
        var panelAspect = windowWidth / windowHeight;
        var superAspect = imageAspect / panelAspect;
        var ratio = (superAspect > 1.0)
            ? windowWidth / imageWidth
            : windowHeight / imageHeight;
        imageWidth = (int) (imageWidth * ratio);
        imageHeight = (int) (imageHeight * ratio);

        return (imageWidth, imageHeight);
    }

    /// <summary>
    /// Считывание запроса из указанного файла.
    /// </summary>
    public static TResult FromFile<TResult>
        (
            string fileName
        )
        where TResult: class, new()
    {
        Sure.FileExists (fileName);

        var extension = Path.GetExtension (fileName).ToLowerInvariant();
        var content = File.ReadAllText (fileName);
        var result = (extension switch
                {
                    ".toml" => Toml.ToModel<TResult> (content),

                    ".xml" => (TResult) new XmlSerializer (typeof (TResult))
                        .Deserialize (new StringReader (content))
                        .ThrowIfNull(),

                    ".yaml" or ".yml" => new DeserializerBuilder()
                        .WithNamingConvention (UnderscoredNamingConvention.Instance)
                        .Build()
                        .Deserialize<TResult> (content),

                    _ => JsonConvert.DeserializeObject<TResult> (content)
                }
            ).ThrowIfNull();

        return result;
    }

    #endregion
}
