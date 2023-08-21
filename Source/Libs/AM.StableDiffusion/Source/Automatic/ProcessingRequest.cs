// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ProcessingRequest.cs -- базовый класс для запросов к API
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Базовый класс для запросов к API.
/// </summary>
public class ProcessingRequest
{
    #region Properties

    /// <summary>
    /// Имя модели (опционально).
    /// </summary>
    [JsonProperty ("checkpoint")]
    public string? Checkpoint { get; set; }

    /// <summary>
    /// Промпт.
    /// </summary>
    [JsonProperty ("prompt")]
    public string? Prompt { get; set; }

    /// <summary>
    /// Негативный промпт.
    /// </summary>
    [JsonProperty ("negative_prompt", NullValueHandling = NullValueHandling.Ignore)]
    public string? NegativePrompt { get; set; }

    /// <summary>
    /// Используемые стили.
    /// </summary>
    [JsonProperty ("styles", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? Styles { get; set; }

    /// <summary>
    /// Начальное значение.
    /// </summary>
    [JsonProperty ("seed", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Seed { get; set; }

    /// <summary>
    /// Имя используемого семплера.
    /// </summary>
    [JsonProperty ("sampler_name", NullValueHandling = NullValueHandling.Ignore)]
    public string? SamplerName { get; set; }

    /// <summary>
    /// Размер пачки.
    /// </summary>
    [JsonProperty ("batch_size", NullValueHandling = NullValueHandling.Ignore)]
    public int BatchSize { get; set; }

    /// <summary>
    /// Количество итераций.
    /// </summary>
    [JsonProperty ("n_iter", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Iterations { get; set; }

    /// <summary>
    /// Количество шагов семплирования.
    /// </summary>
    [JsonProperty ("steps", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Steps { get; set; }

    /// <summary>
    /// Сила промпта.
    /// </summary>
    [JsonProperty ("cfg_scale")]
    public float CfgScale { get; set; }

    /// <summary>
    /// Ширина изображения в пикселах.
    /// </summary>
    [JsonProperty ("width")]
    public int Width { get; set; }

    /// <summary>
    /// Высота изображения в пикселах.
    /// </summary>
    [JsonProperty ("height")]
    public int Height { get; set; }

    /// <summary>
    /// Исправлять лица?
    /// </summary>
    [JsonProperty ("restore_faces")]
    public bool RestoreFaces { get; set; }

    /// <summary>
    /// Генерировать бесшовную текстуру?
    /// </summary>
    [JsonProperty ("tiling")]
    public bool Tiling { get; set; }

    /// <summary>
    /// Сила обесшумливания.
    /// </summary>
    [JsonProperty ("denoising_strength")]
    public float DenoisingStrength { get; set; }

    #endregion
}
