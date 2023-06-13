// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* ScriptsResponse.cs -- ответ на запрос информации о доступных скриптах
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Ответ на запрос информации о доступных скриптах.
/// </summary>
[PublicAPI]
public sealed class ScriptsResponse
{
    #region Properties

    /// <summary>
    /// Скрипты, доступные для режима "текст в картинку".
    /// </summary>
    [JsonProperty ("txt2img", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? TextToImage { get; set; }

    ///<summary>
    /// Скрипты, доступные для режима "картинка в картинку".
    /// </summary>
    [JsonProperty ("img2img", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? ImageToImage { get; set; }

    #endregion
}
