// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* StyleInfo.cs -- инфрмация о пользовательском стиле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Информация о пользовательском стиле.
/// </summary>
[PublicAPI]
public sealed class StyleInfo
{
    #region Properties

    /// <summary>
    /// Наименование стиля.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Позитивный промпт.
    /// </summary>
    [JsonProperty ("prompt", NullValueHandling = NullValueHandling.Ignore)]
    public string? Prompt { get; set; }

    /// <summary>
    /// Негативный промпт.
    /// </summary>
    [JsonProperty ("negative_prompt", NullValueHandling = NullValueHandling.Ignore)]
    public string? NegativePrompt { get; set; }

    #endregion
}
