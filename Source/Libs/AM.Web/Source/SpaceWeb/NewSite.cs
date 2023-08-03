// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* NewSite.cs -- конфигурация добавляемого сайта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.SpaceWeb;

/// <summary>
/// Конфигурация добавляемого сайта.
/// </summary>
[PublicAPI]
public sealed class NewSite
{
    #region Properties

    /// <summary>
    /// Название сайта.
    /// </summary>
    [JsonProperty ("alias")]
    public string? Alias { get; set; }

    /// <summary>
    /// Корневая директория сайта.
    /// </summary>
    [JsonProperty ("docRoot")]
    public string? RootPath { get; set; }

    /// <summary>
    /// Домен, например, "новыйсайт.рф".
    /// </summary>
    [JsonProperty ("domain")]
    public string? Domain { get; set; }

    /// <summary>
    /// Поддомен.
    /// </summary>
    [JsonProperty ("machine")]
    public string? Machine { get; set; }

    /// <summary>
    /// Включить запись сессий в Redis.
    /// </summary>
    [JsonProperty ("enableRedisSession")]
    public bool EnableRedisSession { get; set; }

    #endregion
}
