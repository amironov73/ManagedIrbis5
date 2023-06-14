// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ImageStats.cs -- статистика по изображению
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Статистика по изображению.
/// </summary>
public sealed class ImageStats
{
    #region Properties

    /// <summary>
    /// Количество заплакавших.
    /// </summary>
    [JsonProperty ("cryCount")]
    public int CryCount { get; set; }

    /// <summary>
    /// Количество засмеявшихся.
    /// </summary>
    [JsonProperty ("laughCount")]
    public int LaughCount { get; set; }

    /// <summary>
    /// Количество лайков.
    /// </summary>
    [JsonProperty ("likeCount")]
    public int LikeCount { get; set; }

    /// <summary>
    /// Количество дизлайков.
    /// </summary>
    [JsonProperty ("dislikeCount")]
    public int DislikeCount { get; set; }

    /// <summary>
    /// Количество сердечек.
    /// </summary>
    [JsonProperty ("heartCount")]
    public int HeartCount { get; set; }

    /// <summary>
    /// Количество комментариев.
    /// </summary>
    [JsonProperty ("commentCount")]
    public int CommentCount { get; set; }

    #endregion
}
