// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CrossrefResponse.cs -- Crossref-ответ на запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.Crossref;

/* Пример ответа

{
  "status": "ok",
  "message-type": "work-list",
  "message-version": "1.0.0",
  "message": {
    "facets": {},
    "total-results": 959087,
    "items": [],
    "items-per-page": 0,
    "query": {
      "start-index": 0,
      "search-terms": null
    }
  }
}
 */

/// <summary>
/// Crossref-ответ на запрос.
/// </summary>
[PublicAPI]
public sealed class CrossrefResponse<TResult>
    where TResult: MessageBase
{
    #region Properties

    /// <summary>
    /// Статус ответа, например, <c>"ok"</c>.
    /// </summary>
    [JsonProperty ("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Тип ответа, например, <c>"work-list"</c>.
    /// </summary>
    [JsonProperty ("message-type")]
    public string? MessageType { get; set; }

    /// <summary>
    /// Версия ответа, например, <c>"1.0.0"</c>.
    /// </summary>
    [JsonProperty ("message-version")]
    public string? MessageVersion { get; set; }

    /// <summary>
    /// Собственно сообщение, представляющее собой ответ.
    /// </summary>
    [JsonProperty ("message")]
    public TResult? Message { get; set; }

    #endregion
}
