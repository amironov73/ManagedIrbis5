// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* TextToImageRequest.cs -- запрос на генерацию изображения по тексту
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Запрос на генерацию изображения по тексту.
/// </summary>
[PublicAPI]
public sealed class TextToImageRequest
    : ProcessingRequest
{
    #region Properties

    /// <summary>
    /// Кратность масштабирования.
    /// </summary>
    [JsonProperty ("hr_scale", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public float Scale { get; set; }

    /// <summary>
    /// Применяемый апскейлер, если есть.
    /// </summary>
    [JsonProperty ("hr_upscaler", NullValueHandling = NullValueHandling.Ignore)]
    public string? Upscaler { get; set; }

    /// <summary>
    /// Масштабирование до указанного размера по ширине.
    /// </summary>
    [JsonProperty ("hr_resize_x", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ResizeX { get; set; }

    /// <summary>
    /// Масштабирование до указанного размера по высоте.
    /// </summary>
    [JsonProperty ("hr_resize_y", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ResizeY { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Формирование запроса из командной строки.
    /// </summary>
    public static TextToImageRequest FromCommandLine
        (
            string[]? args
        )
    {
        var result = new TextToImageRequest
        {
            Steps = 20,
            Iterations = 1,
            BatchSize = 1,
            CfgScale = 7,
            Width = 512,
            Height = 768
        };

        if (args is null)
        {
            return result;
        }

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--batch":
                    result.BatchSize = args[++i].SafeToInt32 (1);
                    break;

                case "--cfg":
                    result.CfgScale = float.Parse (args[++i], CultureInfo.InvariantCulture);
                    break;

                case "--height":
                    result.Height = args[++i].SafeToInt32 (512);
                    break;

                case "--iter":
                    result.Iterations = args[++i].SafeToInt32 (1);
                    break;

                case "--negative":
                    result.NegativePrompt = args[++i];
                    break;

                case "--prompt":
                    result.Prompt = args[++i];
                    break;

                case "--sampler":
                    result.SamplerName = args[++i];
                    break;

                case "--steps":
                    result.Steps = args[++i].SafeToInt32 (20);
                    break;

                case "--width":
                    result.Width = args[++i].SafeToInt32 (512);
                    break;
            }
        }

        return result;
    }

    #endregion
}
