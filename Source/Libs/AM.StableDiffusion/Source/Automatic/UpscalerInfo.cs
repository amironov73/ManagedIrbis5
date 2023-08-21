// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* UpscalerInfo.cs -- информация об апскейлере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Информация об апскейлере.
/// </summary>
[PublicAPI]
public sealed class UpscalerInfo
{
    #region Properties

    /// <summary>
    /// Наименование апскейлера.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Наименование модели
    /// </summary>
    [JsonProperty ("model_name")]
    public string? Model { get; set; }

    /// <summary>
    /// Путь.
    /// </summary>
    [JsonProperty ("path")]
    public string? Path { get; set; }

    /// <summary>
    /// Где скачать.
    /// </summary>
    [JsonProperty ("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Увеличение (опционально).
    /// </summary>
    [JsonProperty ("scale", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public float Scale { get; set; }

    #endregion
}
