// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EmbeddingInfo.cs -- информация о текстовой инверсии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Информация о текстовой инверсии.
/// </summary>
[PublicAPI]
public sealed class EmbeddingInfo
{
    #region Properties

    /// <summary>
    /// Количество шагов, затраченных на обучение инверсии.
    /// </summary>
    [JsonProperty ("step", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Step { get; set; }

    /// <summary>
    /// Модель, на которой обучалась инверсия.
    /// </summary>
    [JsonProperty ("sd_checkpoint", NullValueHandling = NullValueHandling.Ignore)]
    public string? Model { get; set; }

    /// <summary>
    /// Имя модели, которое использовал автор.
    /// </summary>
    [JsonProperty ("sd_checkpoint_name", NullValueHandling = NullValueHandling.Ignore)]
    public string? ModelName { get; set; }

    /// <summary>
    /// Размерность векторов в инверсии.
    /// </summary>
    [JsonProperty ("shape", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Shape { get; set; }

    /// <summary>
    /// Количество векторов в инверсии.
    /// </summary>
    [JsonProperty ("vectors", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Vectors { get; set; }

    #endregion
}
