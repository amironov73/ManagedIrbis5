// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* BookInfo.cs -- информация о книге
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
/// Информация о книге.
/// </summary>
[PublicAPI]
[DebuggerDisplay ("{Id}: {Name}")]
public sealed class BookInfo
{
    #region Properties

    /// <summary>
    /// Идентификатор.
    /// </summary>
    [JsonProperty ("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Заглавие.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Авторы гуртом.
    /// </summary>
    [JsonProperty ("authors")]
    public string? Authors { get; set; }

    /// <summary>
    /// ISBN.
    /// </summary>
    [JsonProperty ("isbn")]
    public string? Isbn { get; set; }

    /// <summary>
    /// Год издания.
    /// </summary>
    [JsonProperty ("year")]
    public string? Year { get; set; }

    /// <summary>
    /// Издательство.
    /// </summary>
    [JsonProperty ("publisher")]
    public string? Publisher { get; set; }

    /// <summary>
    /// Язык основного текста.
    /// </summary>
    [JsonProperty ("lang")]
    public string? Language { get; set; }

    #endregion
}
