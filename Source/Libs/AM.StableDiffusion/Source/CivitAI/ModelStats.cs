// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ModelStats.cs -- статистика по модели
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Статистика по модели.
/// </summary>
public sealed class ModelStats
{
    #region Properties

    /// <summary>
    /// Счетчик скачиваний.
    /// </summary>
    [JsonProperty ("downloadCount")]
    public int DownloadCount { get; set; }

    /// <summary>
    /// Счетчик сердечек.
    /// </summary>
    [JsonProperty ("favoriteCount")]
    public int FavoriteCount { get; set; }


    /// <summary>
    /// Счетчик комментариев.
    /// </summary>
    [JsonProperty ("commentCount")]
    public int CommentCount { get; set; }

    /// <summary>
    /// Сколько раз был поставлен рейтинг.
    /// </summary>
    [JsonProperty ("ratingCount")]
    public int RatingCount { get; set; }

    /// <summary>
    /// Средний рейтинг.
    /// </summary>
    [JsonProperty ("rating")]
    public float Rating { get; set; }

    #endregion
}
