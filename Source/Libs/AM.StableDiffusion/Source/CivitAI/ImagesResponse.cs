// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ImagesResponse.cs -- ответ на запрос об изображениях
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Ответ на запрос об изображениях.
/// </summary>
public sealed class ImagesResponse
{
    #region Properties

    /// <summary>
    /// Массив изображений.
    /// </summary>
    [JsonProperty ("items")]
    public ImageInfo[]? Items { get; set; }

    /// <summary>
    /// Метаданные.
    /// </summary>
    [JsonProperty ("metadata")]
    public CommonMetadata? Metadata { get; set; }

    #endregion
}
