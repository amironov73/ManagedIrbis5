// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* InterrogateRequest.cs -- запрос на определение содержимого изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Запрос на определение содержимого изображения.
/// </summary>
[PublicAPI]
public sealed class InterrogateRequest
{
    #region Properties

    /// <summary>
    /// Изображение в кодировке Base64.
    /// </summary>
    [JsonProperty ("image")]
    public string? Image { get; set; }

    /// <summary>
    /// Модель, используемая для определения содержимого.
    /// </summary>
    [JsonProperty ("model")]
    public string? Model { get; set; }

    #endregion
}
