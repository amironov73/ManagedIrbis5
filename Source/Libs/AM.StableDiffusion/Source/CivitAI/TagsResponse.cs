// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* TagsResponse.cs -- ответ на запрос о метках
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Ответ на запрос о метках.
/// </summary>
public sealed class TagsResponse
{
    #region Properties

    /// <summary>
    /// Массив меток.
    /// </summary>
    [JsonProperty ("items")]
    public TagInfo[]? Items { get; set; }

    /// <summary>
    /// Метаданные.
    /// </summary>
    [JsonProperty ("metadata")]
    public CommonMetadata? Metadata { get; set; }

    #endregion
}
