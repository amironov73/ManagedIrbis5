// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* ImageMeta.cs -- метаданные изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Метаданные загруженного на CivitAI изображения.
/// </summary>
public sealed class ImageMeta
{
    #region Properties

    /// <summary>
    /// Размер в виде строки.
    /// </summary>
    [JsonProperty ("Size")]
    public string? Size { get; set; }

    /// <summary>
    /// Сид.
    /// </summary>
    [JsonProperty ("seed")]
    public long Seed { get; set; }

    /// <summary>
    /// Модель.
    /// </summary>
    [JsonProperty ("Model")]
    public string? Model { get; set; }

    /// <summary>
    /// Количество шагов.
    /// </summary>
    [JsonProperty ("steps")]
    public int Steps { get; set; }

    /// <summary>
    /// Подсказка.
    /// </summary>
    [JsonProperty ("prompt")]
    public string? Prompt { get; set; }

    /// <summary>
    /// Версия ПО, например, Automatic1111.
    /// </summary>
    [JsonProperty ("Version")]
    public string? Version { get; set; }

    /// <summary>
    /// Семплер.
    /// </summary>
    [JsonProperty ("sampler")]
    public string? Sampler { get; set; }

    /// <summary>
    /// CFG scale.
    /// </summary>
    [JsonProperty ("cfgScale")]
    public float CfgScale { get; set; }

    /// <summary>
    /// CLIP skip.
    /// </summary>
    [JsonProperty ("Clip skip")]
    public string? ClipSkip { get; set; }

    /// <summary>
    /// Увеличение изображения.
    /// </summary>
    [JsonProperty ("Hires upscale")]
    public string? Upscale { get; set; }

    /// <summary>
    /// Инструмент, использовавшийся для увеличения.
    /// </summary>
    [JsonProperty ("Hires upscaler")]
    public string? Upscaler { get; set; }

    /// <summary>
    /// Подавление шума.
    /// </summary>
    [JsonProperty ("Denoising strength")]
    public string? DenoisingStrength { get; set; }

    #endregion
}
