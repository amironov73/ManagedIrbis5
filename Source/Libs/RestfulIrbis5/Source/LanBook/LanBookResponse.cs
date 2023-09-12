// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* LanBookResponse.cs -- ответ сервера LanBook.com
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.LanBook;

/* Пример ответа сервера:

{
  "type": "object",
  "data": {
    "id": 233312,
    "name": "Методические рекомендации по эффективному внедрению и использованию системы «Антиплагиат.ВУЗ»",
    "authors": "Чехович Ю. В., Беленькая О. С., Ивахненко А. А.",
    "isbn": "978-5-507-44351-2",
    "year": 2022,
    "publisher": "Издательство \"Лань\"",
    "lang": "RU"
  },
  "count": 1,
  "status": 200,
  "message": "Ok",
  "ip": "0.0.0.0",
  "token": "7c0c2193d27108a509abd8ea84a8750c82b3a520",
  "datetime": "2023-09-12 10:38:41"
}

 */

/// <summary>
/// Ответ сервера LanBook.com.
/// </summary>
[PublicAPI]
[DebuggerDisplay ("Status={Status}")]
public sealed class LanBookResponse<TData>
    where TData: class
{
    #region Properties

    /// <summary>
    /// Тип ответа: скаляр, вектор, еще что-нибудь.
    /// </summary>
    [JsonProperty ("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Собственно данные (при успешном выполнении запроса).
    /// </summary>
    [JsonProperty ("data")]
    public TData? Data { get; set; }

    /// <summary>
    /// Количество объектов данных.
    /// </summary>
    [JsonProperty ("count")]
    public int Count { get; set; }

    /// <summary>
    /// Статус. 200 означает успех.
    /// </summary>
    [JsonProperty ("status")]
    public int Status { get; set; }

    /// <summary>
    /// Текстовое сообщение.
    /// </summary>
    [JsonProperty ("message")]
    public string? Message { get; set; }

    /// <summary>
    /// IP-адрес.
    /// </summary>
    [JsonProperty ("ip")]
    public string? Ip { get; set; }

    /// <summary>
    /// Использованный токен.
    /// </summary>
    [JsonProperty ("token")]
    public string? Token { get; set; }

    /// <summary>
    /// Момент времени.
    /// </summary>
    [JsonProperty ("datetime")]
    public string? Moment { get; set; }

    #endregion
}
