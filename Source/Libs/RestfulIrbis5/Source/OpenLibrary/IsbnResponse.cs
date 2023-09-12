// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* IsbnResponse.cs -- ответ на запрос информации об ISBN
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.OpenLibrary;


/* Пример ответа

{
  "publishers": [
    "Princeton University Press"
  ],
  "subtitle": "mathematics at the limits of computation",
  "description": {
    "type": "/type/text",
    "value": "\"What is the shortest possible route for a traveling salesman seeking to visit each city on a list exactly once and return to his city of origin? It sounds simple enough, yet the traveling salesman problem is one of the most intensely studied puzzles in applied mathematics--and it has defied solution to this day. In this book, William Cook takes readers on a mathematical excursion, picking up the salesman's trail in the 1800s when Irish mathematician W. R. Hamilton first defined the problem, and venturing to the furthest limits of today's state-of-the-art attempts to solve it. Cook examines the origins and history of the salesman problem and explores its many important applications, from genome sequencing and designing computer processors to arranging music and hunting for planets. He looks at how computers stack up against the traveling salesman problem on a grand scale, and discusses how humans, unaided by computers, go about trying to solve the puzzle. Cook traces the salesman problem to the realms of neuroscience, psychology, and art, and he also challenges readers to tackle the problem themselves. The traveling salesman problem is--literally--a $1 million question. That's the prize the Clay Mathematics Institute is offering to anyone who can solve the problem or prove that it can't be done. In Pursuit of the Traveling Salesman travels to the very threshold of our understanding about the nature of complexity, and challenges you yourself to discover the solution to this captivating mathematical problem\"--\n\n\"In Pursuit of the Traveling Salesman covers the history, applications, theory, and computation of the traveling salesman problem right up to state-of-the-art solution machinery\"--"
  },
  "covers": [
    9942902
  ],
  "local_id": [
    "urn:sfpl:31223099619380",
    "urn:sfpl:31223100099499",
    "urn:bwbsku:O8-DER-017"
  ],
  "full_title": "In pursuit of the traveling salesman mathematics at the limits of computation",
  "lc_classifications": [
    "QA164 .C69 2012",
    "QA402.5"
  ],
  "key": "/books/OL25018438M",
  "authors": [
    {
      "key": "/authors/OL6992041A"
    }
  ],
  "ocaid": "pursuittraveling00cook",
  "publish_places": [
    "Princeton"
  ],
  "isbn_13": [
    "9780691152707"
  ],
  "pagination": "p. cm.",
  "source_records": [
    "marc:marc_loc_updates/v39.i36.records.utf8:9605908:2685",
    "marc:marc_loc_updates/v40.i08.records.utf8:14686142:2773",
    "marc:marc_loc_updates/v40.i27.records.utf8:4783186:2773",
    "marc:marc_openlibraries_sanfranciscopubliclibrary/sfpl_chq_2018_12_24_run04.mrc:235241281:5776",
    "bwb:9780691152707",
    "marc:marc_loc_2016/BooksAll.2016.part38.utf8:198063606:2773",
    "marc:harvard_bibliographic_metadata/ab.bib.13.20150123.full.mrc:103649704:3184",
    "marc:marc_columbia/Columbia-extract-20221130-019.mrc:117533985:4668",
    "promise:bwb_daily_pallets_2022-12-15",
    "ia:inpursuitoftrave0000cook"
  ],
  "title": "In pursuit of the traveling salesman",
  "lccn": [
    "2011030626"
  ],
  "notes": {
    "type": "/type/text",
    "value": "Includes bibliographical references and index."
  },
  "languages": [
    {
      "key": "/languages/eng"
    }
  ],
  "dewey_decimal_class": [
    "511/.5"
  ],
  "subjects": [
    "MATHEMATICS / General",
    "Computational complexity",
    "MATHEMATICS / Recreations & Games",
    "Traveling salesman problem"
  ],
  "publish_date": "2012",
  "publish_country": "nju",
  "by_statement": "William J. Cook",
  "works": [
    {
      "key": "/works/OL16134512W"
    }
  ],
  "type": {
    "key": "/type/edition"
  },
  "oclc_numbers": [
    "724663194"
  ],
  "number_of_pages": 228,
  "latest_revision": 13,
  "revision": 13,
  "created": {
    "type": "/type/datetime",
    "value": "2011-10-22T00:22:43.651755"
  },
  "last_modified": {
    "type": "/type/datetime",
    "value": "2023-04-05T04:39:18.385575"
  }
}

*/

/// <summary>
/// Ответ на запрос информации об ISBN.
/// </summary>
[PublicAPI]
public sealed class IsbnResponse
{
    #region Properties

    /// <summary>
    /// Издательства.
    /// </summary>
    [JsonProperty ("publishers")]
    public string[]? Publishers { get; set; }

    /// <summary>
    /// Подзаголовочные данные.
    /// </summary>
    [JsonProperty ("subtitle")]
    public string? Subtitle { get; set; }

    /// <summary>
    /// Аннотация.
    /// </summary>
    [JsonProperty ("description")]
    public TypedValue? Description { get; set; }

    /// <summary>
    /// Обложки.
    /// </summary>
    [JsonProperty ("covers")]
    public string[]? Covers { get; set; }

    /// <summary>
    /// Идентификаторы в OpenLibrary.
    /// </summary>
    [JsonProperty ("local_id")]
    public string[]? Id { get; set; }

    /// <summary>
    /// Полный заголовок.
    /// </summary>
    [JsonProperty ("full_title")]
    public string? FullTitle { get; set; }

    /// <summary>
    /// Классификационные индексы
    /// Библиотеки Конгресса США.
    /// </summary>
    [JsonProperty ("lc_classification")]
    public string[]? Classification { get; set; }

    /// <summary>
    /// Ключ.
    /// </summary>
    [JsonProperty ("key")]
    public string? Key { get; set; }

    /// <summary>
    /// Авторы.
    /// </summary>
    [JsonProperty ("authors")]
    public KeyedObject[]? Authors { get; set; }

    /// <summary>
    /// Идентификатор OCA.
    /// </summary>
    [JsonProperty ("ocaid")]
    public string[]? Oca { get; }

    /// <summary>
    /// Места издания.
    /// </summary>
    [JsonProperty ("publish_places")]
    public string[]? Places { get; set; }

    /// <summary>
    /// ISBN.
    /// </summary>
    [JsonProperty ("isbn_13")]
    public string[]? Isbn { get; set; }

    /// <summary>
    /// Пагинация.
    /// </summary>
    [JsonProperty ("pagination")]
    public string? Pagination { get; set; }

    /// <summary>
    /// Записи-источники.
    /// </summary>
    [JsonProperty ("source_records")]
    public string[]? SourceRecords { get; set; }

    /// <summary>
    /// Заглавие (краткий вариант).
    /// </summary>
    [JsonProperty ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// LCCN.
    /// </summary>
    [JsonProperty ("lccn")]
    public string[]? Lccn { get; set; }

    /// <summary>
    /// Примечания.
    /// </summary>
    [JsonProperty ("notes")]
    public TypedValue? Notes { get; set; }

    /// <summary>
    /// Языки.
    /// </summary>
    [JsonProperty ("languages")]
    public KeyedObject[]? Languages { get; set; }

    /// <summary>
    /// Индекс классификации Дьюи.
    /// </summary>
    [JsonProperty ("dewey_decimal_class")]
    public string[]? Dewey { get; set; }

    /// <summary>
    /// Рубрки.
    /// </summary>
    [JsonProperty ("subjects")]
    public string[]? Subjects { get; set; }

    /// <summary>
    /// Дата публикации, как правило, 4 цифры года.
    /// </summary>
    [JsonProperty ("publish_date")]
    public string? PublishDate { get; set; }

    /// <summary>
    /// Страна.
    /// </summary>
    [JsonProperty ("publish_country")]
    public string? PublishCountry { get; set; }

    /// <summary>
    /// Указание на авторство.
    /// </summary>
    [JsonProperty ("by_statement")]
    public string? By { get; set; }

    /// <summary>
    /// Труды.
    /// </summary>
    [JsonProperty ("works")]
    public KeyedObject[]? Works { get; set; }

    /// <summary>
    /// Тип.
    /// </summary>
    public string[]? Type { get; set; }

    /// <summary>
    /// Идентификаторы OCLC.
    /// </summary>
    [JsonProperty ("oclc_numbers")]
    public string[]? Oclc { get; set; }

    /// <summary>
    /// Объем в страницах.
    /// </summary>
    [JsonProperty ("number_of_pages")]
    public int Pages { get; set; }

    /// <summary>
    /// Номер текущей ревизии.
    /// </summary>
    [JsonProperty ("last_revision")]
    public int LastRevision { get; set; }

    /// <summary>
    /// Номер текущей ревизии.
    /// </summary>
    [JsonProperty ("revision")]
    public int Revision { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    [JsonProperty ("created")]
    public TypedValue? Created { get; set; }

    /// <summary>
    /// Дата последней модификации.
    /// </summary>
    [JsonProperty ("last_modified")]
    public TypedValue? LastModified { get; set; }

    #endregion
}
