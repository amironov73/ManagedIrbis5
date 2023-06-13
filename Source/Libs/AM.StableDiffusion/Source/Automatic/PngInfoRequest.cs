// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PngInfoRequest.cs -- запрос на получение информации об изображении
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Запрос на получение информации об изображении.
/// </summary>
[PublicAPI]
public sealed class PngInfoRequest
{
    #region Properties

    /// <summary>
    /// Изображение, закодированное в Base64.
    /// </summary>
    [JsonProperty ("image", NullValueHandling = NullValueHandling.Ignore)]
    public string? Image { get; set; }

    #endregion
}
