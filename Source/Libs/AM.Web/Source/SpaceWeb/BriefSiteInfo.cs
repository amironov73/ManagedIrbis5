// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* BriefSiteInfo.cs -- краткая информация о сайте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.SpaceWeb;

/// <summary>
/// Краткая информация о сайте.
/// </summary>
[PublicAPI]
public sealed class BriefSiteInfo
{
    #region Properties

    /// <summary>
    /// Идентификатор сайта.
    /// </summary>
    [JsonProperty ("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Домашняя директория.
    /// </summary>
    [JsonProperty ("docRoot")]
    public string? DocumentsRoot { get; set; }

    /// <summary>
    /// Полный путь к домашней директории.
    /// </summary>
    [JsonProperty ("docRootFull")]
    public string? FullRoot { get; set; }

    /// <summary>
    /// Название сайта.
    /// </summary>
    [JsonProperty ("alias")]
    public string? Alias { get; set; }

    /// <summary>
    /// Технический домен.
    /// </summary>
    [JsonProperty ("domainTech")]
    public string? TechDomain { get; set; }

    /// <summary>
    /// Дата окончания антивируса.
    /// </summary>
    [JsonProperty ("antivirusExpired")]
    public string? AntivirusExpiration { get; set; }

    /// <summary>
    /// Доступен заказ антивируса?
    /// </summary>
    [JsonProperty ("antivirusAvailable")]
    public bool AntivirusAvailable { get; set; }

    /// <summary>
    /// Активен ли антивирус?
    /// </summary>
    [JsonProperty ("antivirusActive")]
    public int AntivirusActive { get; set; }

    /// <summary>
    /// Стоимость антивируса.
    /// </summary>
    [JsonProperty ("antivirusPrice")]
    public int AntivirusPrice { get; set; }

    /// <summary>
    /// Добавлен ли этот сайт в список сайтов, сессии которых хранятся в Redis?
    /// </summary>
    [JsonProperty ("redisSessionSelected")]
    public bool RedisSessionSelected { get; set; }

    /// <summary>
    /// Включено ли сейчас хранение сессий этого сайта в Redis?
    /// </summary>
    [JsonProperty ("redisSessionEnabled")]
    public bool RedisSessionEnabled { get; set; }

    #endregion
}
