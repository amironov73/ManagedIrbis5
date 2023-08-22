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

using System.Globalization;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

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
    [JsonPropertyName ("hr_scale")]
    [JsonProperty ("hr_scale", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public float Scale { get; set; }

    /// <summary>
    /// Применяемый апскейлер, если есть.
    /// </summary>
    [JsonPropertyName ("hr_upscaler")]
    [JsonProperty ("hr_upscaler", NullValueHandling = NullValueHandling.Ignore)]
    public string? Upscaler { get; set; }

    /// <summary>
    /// Масштабирование до указанного размера по ширине.
    /// </summary>
    [JsonPropertyName ("hr_resize_x")]
    [JsonProperty ("hr_resize_x", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ResizeX { get; set; }

    /// <summary>
    /// Масштабирование до указанного размера по высоте.
    /// </summary>
    [JsonPropertyName ("hr_resize_y")]
    [JsonProperty ("hr_resize_y", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int ResizeY { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Считывание запроса из указанного файла.
    /// </summary>
    public static TextToImageRequest FromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        return StableUtility.FromFile<TextToImageRequest> (fileName);
    }

    /// <summary>
    /// Объединение с другим.
    /// </summary>
    public TextToImageRequest MergeWith
        (
            TextToImageRequest? other
        )
    {
        if (other is null)
        {
            return this;
        }

        // TODO Styles
        Prompt ??= other.Prompt;
        NegativePrompt ??= other.NegativePrompt;
        Seed = other.Seed is 0 ? Seed : other.Seed;
        SamplerName ??= other.SamplerName;
        BatchSize = other.BatchSize is 0 ? BatchSize : other.BatchSize;
        Iterations = other.Iterations is 0 ? Iterations : other.Iterations;
        Steps = other.Steps is 0 ? Steps : other.Steps;
        CfgScale = other.CfgScale is 0.0f ? CfgScale : other.CfgScale;
        Width = other.Width is 0 ? Width : other.Width;
        Height = other.Height is 0 ? Height : other.Height;
        // TODO прочие члены

        return this;
    }

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
                case "batch":
                    result.BatchSize = args[++i].SafeToInt32 (1);
                    break;

                case "--cfg":
                case "cfg":
                    result.CfgScale = float.Parse (args[++i], CultureInfo.InvariantCulture);
                    break;

                case "--file":
                case "file":
                case "--from":
                case "from":
                    var fromFile = FromFile (args[++i]);
                    result.MergeWith (fromFile);
                    break;

                case "--height":
                case "height":
                    result.Height = args[++i].SafeToInt32 (512);
                    break;

                case "--iter":
                case "iter":
                case "--number":
                case "number":
                case "--count":
                case "count":
                    result.Iterations = args[++i].SafeToInt32 (1);
                    break;

                case "--negative":
                case "--neg":
                case "negative":
                case "neg":
                    result.NegativePrompt = args[++i];
                    break;

                case "--prompt":
                case "prompt":
                    result.Prompt = args[++i];
                    break;

                case "--sampler":
                case "sampler":
                    result.SamplerName = args[++i];
                    break;

                case "--steps":
                case "steps":
                    result.Steps = args[++i].SafeToInt32 (20);
                    break;

                case "--width":
                case "width":
                    result.Width = args[++i].SafeToInt32 (512);
                    break;
            }
        }

        return result;
    }

    #endregion
}
