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
}
