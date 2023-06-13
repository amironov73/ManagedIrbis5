// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* ProgressResponse.cs -- ответ на запрос о прогрессе длительной операции
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
/// Ответ на запрос о прогрессе длительной операции.
/// </summary>
[PublicAPI]
public sealed class ProgressResponse
{
    #region Properties

    /// <summary>
    /// Величина прогресса, от 0 до 1.
    /// </summary>
    [JsonProperty ("progress")]
    public float Progress { get; set; }

    /// <summary>
    /// Примерное оставшееся время до окончания операции в секундах.
    /// </summary>
    [JsonProperty ("eta_relative")]
    public float Eta { get; set; }

    /// <summary>
    /// Состояние.
    /// </summary>
    [JsonProperty ("state")]
    public JObject? State { get; set; }

    /// <summary>
    /// Текущее обрабатываемое изображение в кодировке Base64.
    /// </summary>
    [JsonProperty ("current_image")]
    public string? CurrentImage { get; set; }

    /// <summary>
    /// Общая информация.
    /// </summary>
    [JsonProperty ("textinfo")]
    public string? Information { get; set; }

    #endregion
}
