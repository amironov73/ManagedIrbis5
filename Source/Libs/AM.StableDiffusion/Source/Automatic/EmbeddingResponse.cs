// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EmbeddingResponse.cs -- ответ на запрос о текстовых инверсиях
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Ответ на запрос о текстовых инверсиях.
/// </summary>
[PublicAPI]
public sealed class EmbeddingResponse
{
    #region Properties

    /// <summary>
    /// Загруженные инверсии.
    /// </summary>
    [JsonProperty ("loaded", NullValueHandling = NullValueHandling.Ignore)]
    public EmbeddingInfo[]? Loaded { get; set; }

    /// <summary>
    /// Пропущенные (не загруженные) инверсии.
    /// </summary>
    [JsonProperty ("skipped", NullValueHandling = NullValueHandling.Ignore)]
    public EmbeddingInfo[]? Skipped { get; set; }

    #endregion
}
