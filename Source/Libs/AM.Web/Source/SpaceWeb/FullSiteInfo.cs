// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* FullSiteInfo.cs -- полная (детальная) информация о сайте
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.SpaceWeb;

/// <summary>
/// Полная (детальная) информация о сайте.
/// </summary>
[PublicAPI]
public sealed class FullSiteInfo
{
    #region Properties

    /// <summary>
    /// Тип бэкенда.
    /// </summary>
    [JsonProperty ("backEnd")]
    public string? BackEnd { get; set; }

    /// <summary>
    /// Идентификатор бэкенда.
    /// </summary>
    [JsonProperty ("backEndId")]
    public int? BackEndId { get; set; }

    /// <summary>
    /// Показывать файлы?
    /// </summary>
    [JsonProperty ("viewFiles")]
    public bool ViewFiles { get; set; }

    /// <summary>
    /// Запуск скриптов.
    /// </summary>
    [JsonProperty ("runScripts")]
    public bool RunScripts { get; set; }

    /// <summary>
    /// Доступен ли Redis для подключения
    /// (определяется по тарифу и серверу)?
    /// </summary>
    [JsonProperty ("redisAvailable")]
    public bool RedisAvailable { get; set; }

    /// <summary>
    /// True, если Redis можно подключить на другом сервере,
    /// т.е. нужен перенос (при этом redisAvailable будет false).
    /// </summary>
    [JsonProperty ("redisNeedTransfer")]
    public bool RedisNeedTransfer { get; set; }

    /// <summary>
    /// Возможно ли подключение Redis (определяется по доступности
    /// (redisAvailable) и отсутствию текущих операций
    /// подключения/отключения).
    /// </summary>
    [JsonProperty ("redisEnabled")]
    public bool RedisEnabled { get; set; }

    /// <summary>
    /// Доступен ли Redis для текущего бэкенда данного сайта
    /// (сейчас Redis доступен только для php7.*, php8).
    /// </summary>
    [JsonProperty ("redisBackendAvailable")]
    public bool RedisBackendAvailable { get; set; }

    /// <summary>
    /// Включено ли сейчас хранение сессий этого сайта в Redis.
    /// </summary>
    [JsonProperty ("redisSessionEnabled")]
    public bool RedisSessionEnabled { get; set; }

    /// <summary>
    /// Возможно ли включение хранений сессий в Redis
    /// (определяется по доступности (redisAvailable)
    /// и отсутствию текущих операций подключения/отключения).
    /// </summary>
    [JsonProperty ("redisCanEnableSession")]
    public bool RedisCanEnableSession { get; set; }

    /// <summary>
    /// Добавлен ли этот сайт в список сайтов,
    /// сессии которых хранятся в Redis?
    /// </summary>
    [JsonProperty ("redisSessionSelected")]
    public bool RedisSessionSelected { get; set; }

    /// <summary>
    /// Кодировка.
    /// </summary>
    [JsonProperty ("encoding")]
    public string? Encoding { get; set; }

    /// <summary>
    /// Домены.
    /// </summary>
    [JsonProperty ("domains")]
    public string[]? Domains { get; set; }

    /// <summary>
    /// Установленные приложения.
    /// </summary>
    [JsonProperty ("programs")]
    public string[]? Applications { get; set; }

    #endregion
}
