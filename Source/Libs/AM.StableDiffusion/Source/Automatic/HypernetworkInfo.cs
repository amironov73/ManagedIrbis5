// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HypernetworkInfo.cs -- информация о гиперсети
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Информация о гиперсети.
/// </summary>
[PublicAPI]
public sealed class HypernetworkInfo
{
    #region Properties

    /// <summary>
    /// Наименование гиперсети.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Путь.
    /// </summary>
    [JsonProperty ("path", NullValueHandling = NullValueHandling.Ignore)]
    public string? Path { get; set; }

    #endregion
}
