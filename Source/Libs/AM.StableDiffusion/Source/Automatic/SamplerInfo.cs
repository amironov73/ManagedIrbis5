// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* SamplerInfo.cs -- информация о семплере
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Информация о семплере.
/// </summary>
[PublicAPI]
public sealed class SamplerInfo
{
    #region Property

    /// <summary>
    /// Наименование семплера.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Псевдонимы.
    /// </summary>
    [JsonProperty ("aliases", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? Aliases { get; set; }


    /// <summary>
    /// Опции.
    /// </summary>
    [JsonProperty ("options", NullValueHandling = NullValueHandling.Ignore)]
    public JObject? Options { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonConvert.SerializeObject (this);

    #endregion
}
