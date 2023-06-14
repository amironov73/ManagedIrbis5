// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CreatorsResponse.cs -- ответ на запрос о создателях
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Ответ на запрос о создателях.
/// </summary>
[PublicAPI]
public sealed class CreatorsResponse
{
    #region Properties

    /// <summary>
    /// Массив создателей.
    /// </summary>
    [JsonProperty ("items")]
    public CreatorInfo[]? Items { get; set; }

    /// <summary>
    /// Метаданные.
    /// </summary>
    [JsonProperty ("metadata")]
    public CivitMetadata? Metadata { get; set; }

    #endregion
}
