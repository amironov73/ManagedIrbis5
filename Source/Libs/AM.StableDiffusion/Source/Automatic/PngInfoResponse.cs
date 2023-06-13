// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PngInfoResponse.cs -- ответ на запрос о получении информации об изображении
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
/// Ответ на запрос о получении информации об изображении.
/// </summary>
[PublicAPI]
public sealed class PngInfoResponse
{
    #region Properties

    /// <summary>
    /// Строка с параметрами, использовавшимися при генераци изображения.
    /// </summary>
    [JsonProperty ("info")]
    public string? Information { get; set; }

    /// <summary>
    /// Объект, содержащий информацию, содержащуюся в изображении.
    /// </summary>
    [JsonProperty ("items")]
    public JObject? Items { get; set; }

    #endregion
}
