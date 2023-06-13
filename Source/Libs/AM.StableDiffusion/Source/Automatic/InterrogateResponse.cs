// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* InterrogateResponse.cs -- ответ на запрос об определении содержимого изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Ответ на запрос об определении содержимого изображения.
/// </summary>
[PublicAPI]
public sealed class InterrogateResponse
{
    #region Properties

    /// <summary>
    /// Сгенерированное описание содержимого изображения.
    /// </summary>
    [JsonProperty ("caption")]
    public string? Caption { get; set; }

    #endregion
}
