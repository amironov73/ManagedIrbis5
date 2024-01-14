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

using System.Text.Json.Serialization;

using Newtonsoft.Json;

using Ignore=System.Text.Json.Serialization.JsonIgnoreAttribute;

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
    [JsonPropertyName ("checkpoint")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonProperty ("checkpoint", NullValueHandling = NullValueHandling.Ignore)]
    public string? Checkpoint { get; set; }

    /// <summary>
    /// Промпт.
    /// </summary>
    [JsonPropertyName ("prompt")]
    [JsonProperty ("prompt", NullValueHandling = NullValueHandling.Ignore)]
    public string? Prompt { get; set; }

    /// <summary>
    /// Негативный промпт.
    /// </summary>
    [JsonPropertyName ("negative_prompt")]
    [JsonProperty ("negative_prompt", NullValueHandling = NullValueHandling.Ignore)]
    public string? NegativePrompt { get; set; }

    /// <summary>
    /// Используемые стили.
    /// </summary>
    [JsonPropertyName ("styles")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonProperty ("styles", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? Styles { get; set; }

    /// <summary>
    /// Начальное значение.
    /// </summary>
    [JsonPropertyName ("seed")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("seed", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Seed { get; set; }

    /// <summary>
    /// Имя используемого семплера.
    /// </summary>
    [JsonPropertyName ("sampler_name")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonProperty ("sampler_name", NullValueHandling = NullValueHandling.Ignore)]
    public string? SamplerName { get; set; }

    /// <summary>
    /// Размер пачки.
    /// </summary>
    [JsonPropertyName ("batch_size")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("batch_size", NullValueHandling = NullValueHandling.Ignore)]
    public int BatchSize { get; set; }

    /// <summary>
    /// Количество итераций.
    /// </summary>
    [JsonPropertyName ("n_iter")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("n_iter", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Iterations { get; set; }

    /// <summary>
    /// Количество шагов семплирования.
    /// </summary>
    [JsonPropertyName ("steps")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("steps", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Steps { get; set; }

    /// <summary>
    /// Сила промпта.
    /// </summary>
    [JsonPropertyName ("cfg_scale")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("cfg_scale", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public float CfgScale { get; set; }

    /// <summary>
    /// Ширина изображения в пикселах.
    /// </summary>
    [JsonPropertyName ("width")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("width", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Width { get; set; }

    /// <summary>
    /// Высота изображения в пикселах.
    /// </summary>
    [JsonPropertyName ("height")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("height", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Height { get; set; }

    /// <summary>
    /// Исправлять лица?
    /// </summary>
    [JsonPropertyName ("restore_faces")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("restore_faces", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool RestoreFaces { get; set; }

    /// <summary>
    /// Генерировать бесшовную текстуру?
    /// </summary>
    [JsonPropertyName ("tiling")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("tiling", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool Tiling { get; set; }

    /// <summary>
    /// Сила обесшумливания.
    /// </summary>
    [JsonPropertyName ("denoising_strength")]
    [Ignore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [JsonProperty ("denoising_strength", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public float DenoisingStrength { get; set; }

    #endregion
}
