// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CrossrefClient.cs -- клиент Crossref.org
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using JetBrains.Annotations;

using Newtonsoft.Json;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

#endregion

namespace RestfulIrbis.Crossref;

//
// https://ru.wikipedia.org/wiki/Crossref
//
// Crossref (ранее CrossRef) — официальное агентство
// регистрации Цифровых Идентификаторов Объекта (DOI)
// международного DOI фонда. Оно объединяет издателей
// академических публикаций (журналы, монографии, сборники
// материалов конференций) и создано в 2000 для создания
// системы персистентных библиографических ссылок в статьях.
//

/// <summary>
/// Клиент Crossref.org.
/// </summary>
[PublicAPI]
public sealed class CrossrefClient
{
    #region Constants

    /// <summary>
    /// URL для API по умолчанию.
    /// </summary>
    public const string DefaultUrl = "https://api.crossref.org/";

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CrossrefClient()
        : this (DefaultUrl)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CrossrefClient
        (
            string baseUrl
        )
    {
        Sure.NotNullNorEmpty (baseUrl);

        var options = new RestClientOptions
        {
            BaseUrl = new Uri (baseUrl)
        };
        _restClient = new RestClient
            (
                options,
                configureSerialization: s => s.UseNewtonsoftJson()
            );
    }

    #endregion

    #region Private members

    private readonly RestClient _restClient;

    private RestRequest CreateRequest
        (
            string resource,
            int rows = -1
        )
    {
        var result = new RestRequest (resource);
        result.AddHeader ("Accept", "application/json");

        if (rows >= 0)
        {
            result.AddParameter ("rows", rows.ToInvariantString());
        }

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение работ, удовлетворяющих заданным критериям.
    /// </summary>
    public CrossrefResponse<WorksResponse>? GetWorks
        (
            CrossrefQuery query,
            int rows = -1
        )
    {
        Sure.NotNull (query);

        var request = CreateRequest ("works", rows);
        query.Encode (request);

        var response = _restClient.Execute (request);

        return response.Content is { } content
            ? JsonConvert.DeserializeObject<CrossrefResponse<WorksResponse>> (content)
            : default;
    }

    #endregion
}
