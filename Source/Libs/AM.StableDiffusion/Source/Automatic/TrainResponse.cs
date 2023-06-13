// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TrainResponse.cs -- ответ на запрос о тренировке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Ответ на запрос о тренировке.
/// </summary>
[PublicAPI]
public sealed class TrainResponse
{
    #region Properties

    /// <summary>
    /// Общая информация.
    /// </summary>
    [JsonProperty ("info")]
    public string? Information { get; set; }

    #endregion
}
