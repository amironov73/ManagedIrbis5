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
using System.IO;

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
    /// Считывание запроса из указанного файла.
    /// </summary>
    public static TextToImageRequest FromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        // TODO определять YAML по расширению

        var content = File.ReadAllText (fileName);
        var result = JsonConvert.DeserializeObject<TextToImageRequest> (content)
            .ThrowIfNull();

        return result;
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
        Seed = Seed is 0 ? other.Seed : Seed;
        SamplerName ??= other.SamplerName;
        BatchSize = BatchSize is 0 ? other.BatchSize : BatchSize;
        Iterations = Iterations is 0 ? other.Iterations : Iterations;
        Steps = Steps is 0 ? other.Steps : Steps;
        CfgScale = CfgScale is 0.0f ? other.CfgScale : CfgScale;
        Width = Width is 0 ? other.Width : Width;
        Height = Height is 0 ? other.Height : Height;
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
