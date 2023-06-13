// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TextToImageResponse.cs -- ответ на запрос о генерации изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Ответ на запрос о генерации изображения.
/// </summary>
[PublicAPI]
public sealed class TextToImageResponse
{
    #region Properties

    /// <summary>
    /// Сгенерированные изображения в формате Base64.
    /// </summary>
    [JsonProperty ("images", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? Images { get; set; }

    /// <summary>
    /// Параметры.
    /// </summary>
    [JsonProperty ("parameters", NullValueHandling = NullValueHandling.Ignore)]
    public JObject? Parameters { get; set; }

    /// <summary>
    /// Общая информация.
    /// </summary>
    [JsonProperty ("info", NullValueHandling = NullValueHandling.Ignore)]
    public string? Information { get; set; }

    #endregion
}
